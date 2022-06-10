using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KleanTrak.Model;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace KleanTrak.Core
{
	/// <summary>
	/// Gestione lettura dati da porta seriale, si usa la proprietà FolderOrFileName 
	/// per memorizzare il nome della porta com
	/// </summary>
	public class WPCantelMedivatorsSerial : WPBase
	{
		public delegate void LogLineHandler(string line);
		public event LogLineHandler LogLineAdded;
		public delegate void SerialLineReceivedHandòer(string line);
		public event SerialLineReceivedHandòer SerialLineReceived;
		private HexConverter _converter = new HexConverter();
		public delegate void CycleReconstructedHandler(WasherCycle cycle);
		public event CycleReconstructedHandler CycleReconstructed;
		private const byte _CYCLE_LINE_IDENTIFIER = 0x0E;
		private SerialPort _com_port = null;
		private Task _port_polling_task = null;
		private CancellationTokenSource _token_source = new CancellationTokenSource();
		private readonly char _cr_char = Convert.ToChar(0x0D);
		private readonly char _lf_char = Convert.ToChar(0x0A);
		private readonly char _last_line_char = Convert.ToChar(0x0E);
		private readonly object _cycles_locker = new object();
		private List<WasherCycle> _cycles = new List<WasherCycle>();
		private ReaderWriterLockSlim _run_locker = new ReaderWriterLockSlim();
		private bool _running = false;
		public bool Running
		{
			get
			{
				try
				{
					_run_locker.EnterReadLock();
					return _running;
				}
				catch (Exception e)
				{
					LogError("", e);
					throw;
				}
				finally
				{
					_run_locker.ExitReadLock();
				}
			}
			set
			{
				try
				{
					_run_locker.EnterWriteLock();
					_running = value;
				}
				catch (Exception e)
				{
					LogError("", e);
					throw;
				}
				finally
				{
					_run_locker.ExitWriteLock();
				}
			}
		}
		private void LogInfo(string line)
		{
			LogLineAdded?.Invoke(line);
			Logger.Info(line);
		}
		private void LogError(string msg, Exception e)
		{
			LogLineAdded?.Invoke(msg + Environment.NewLine + e.ToString());
			Logger.Error(msg, e);
		}
		public override bool Start()
		{
			try
			{
				var input_str = FolderOrFileName.Trim();
				var match = Regex.Match(input_str, @"COM\d+", RegexOptions.IgnoreCase);
				if (!match.Success || match.Value != input_str)
					throw new ApplicationException($"FolderOrFilename not valid {FolderOrFileName}");
				_com_port = new SerialPort(input_str, GetBaudrate(), Parity.None, 8, StopBits.One);
				_com_port.Open();
				_port_polling_task = Task.Factory.StartNew(() => PortPolling(_token_source.Token), _token_source.Token);
				Running = true;
				return true;
			}
			catch (Exception ex)
			{
				LogError("", ex);
				return false;
			}
		}
		private void PortPolling(CancellationToken token)
		{
			try
			{
				var lines = new List<string>();
				string current_line = "";
				while (!token.IsCancellationRequested)
				{
					if (_com_port.BytesToRead == 0)
					{
						Thread.Sleep(100);
						continue;
					}
					LogInfo("reading new line...");
					current_line = ReadLine();
					//carattere di fine scontrino e linea vuota trascurati
					if (current_line.Length == 0)
					{
						LogInfo("...nothing to read");
						continue;
					}
					LogInfo("...new line read");
					//legge
					FireLineEvent(current_line);
					lines.Add(current_line);
					LogInfo($"new line added {current_line} lines {lines.Count}");
					if (IsLastLine(current_line))
					{
						//scontrino completo
						if (lines.Count == 0)
							continue;
						LogInfo($"parsing lines...");
						var cycle = ParseLines(lines, out string linecontent);
						LogInfo($"...lines parsed {Environment.NewLine + linecontent}");
						if (cycle != null)
						{
							LogInfo($"adding cycle...");
							AddCycle(cycle);
							LogInfo($"cycle added cycles: {_cycles.Count}{Environment.NewLine}{cycle}");
						}
						else
						{
							LogInfo($"!!! CYCLE IS NULL!!!");
						}
						//reset lines 
						LogInfo("clearing lines...");
						lines.Clear();
						LogInfo("...lines cleared");
					}
				}
			}
			catch (Exception e)
			{
				LogError("", e);
			}
			finally
			{
				LogInfo("exiting port reading loop");
			}
		}
		private bool IsLastLine(string line) => 
			line.ToUpper().Contains("ABORTED") || line.ToUpper().Contains("COMPLETED");
		//private bool IsReceiptEndChar(string line) =>
		//    line.Substring(0, 1).ToCharArray()[0] == _last_line_char;
		private string ReadLine()
		{
			try
			{
				if (_com_port == null || _com_port.IsOpen == false)
					throw new ApplicationException("com port is closed");
				List<char> buff = new List<char>();
				char current_byte = (char)0x00;
				do
				{
					current_byte = (char)_com_port.ReadByte();
					LogInfo(_converter.GetHexString(current_byte));
					if(current_byte != _cr_char && current_byte != _lf_char)
						buff.Add(current_byte);
				} while (current_byte != _cr_char);
				return new string(buff.ToArray());
			}
			catch (Exception e)
			{
				LogError("", e);
				throw;
			}
		}
		private void FireLineEvent(string line) => SerialLineReceived?.Invoke(line);
		private WasherCycle ParseLines(List<string> lines, out string linescontent)
		{
			linescontent = "";
			try
			{
				var retval = new WasherCycle();
				var content = "";
				//inserisce tutte le linee come contenuto del file
				lines.ForEach(l => content += l + Environment.NewLine);
				retval.FileContent = content;
				linescontent = content;
				foreach (string line in lines)
				{
					var line_date = GetDateFromString(line);
					var uline = line.ToUpper();
					if (uline.Contains("START"))
						retval.StartTimestamp = line_date;
					if (line.Contains(_last_line_char))
					{
						retval.EndTimestamp = line_date;
						retval.Failed = uline.Contains("ABORTED");
						retval.Completed = uline.Contains("COMPLETED");
						continue;
					}
					if (uline.Contains("MACHINE ID : "))
					{
						var match = Regex.Match(line, @"Machine ID : \d+");
						if (!match.Success)
							continue;
						var match_id = Regex.Match(match.Value, @"\d+");
						if (!match_id.Success)
							continue;
						retval.WasherExternalID = match_id.Value;
						retval.WasherID = TranscodeWasher(retval.WasherExternalID);
						continue;
					}
					if (uline.Contains("SCOPE"))
					{
						var match = Regex.Match(line, @"\d{10}");
						if (!match.Success)
						{
							retval.DeviceExternalID = "UNKNOWN";
							LogError(line, new ApplicationException("Scope id not found"));
							continue;
						}
						retval.DeviceExternalID = match.Value;
						retval.DeviceID = TranscodeDevice(retval.DeviceExternalID);
						continue;
					}
					if (uline.Contains("CYCLE COUNT"))
					{
						var match = Regex.Match(line, @":[ ]+\d+");
						if (!match.Success)
						{
							retval.CycleCount = "UNKNOWN";
							LogError(line, new ApplicationException("Cycle count not found"));
							continue;
						}
						var sub_match = Regex.Match(match.Value, @"\d+");
						if (sub_match.Success)
						{
							retval.CycleCount = sub_match.Value;
							continue;
						}
						else
						{
							retval.CycleCount = "UNKNOWN";
							LogError(line, new ApplicationException("Cycle count number not found"));
							continue;
						}
					}
					if (uline.Contains("OPERATOR"))
					{
						var match = Regex.Match(line, @"\d{10}");
						if (!match.Success)
						{
							retval.OperatorStartExternalID = "UNKNOWN";
							LogError(line, new ApplicationException("Operator id not found"));
							continue;
						}
						retval.OperatorStartExternalID = match.Value;
						retval.OperatorStartID = TranscodeOperator(retval.OperatorStartExternalID);
						continue;
					}
					if (uline.Contains("SCOPE"))
					{
						var match = Regex.Match(line, @"\d{10}");
						if (!match.Success)
						{
							retval.DeviceExternalID = "UNKNOWN";
							LogError(line, new ApplicationException("Scope id not found"));
							continue;
						}
						retval.DeviceExternalID = match.Value;
						retval.DeviceID = TranscodeDevice(retval.DeviceExternalID);
						continue;
					}
					//la linea viene archiviata come additionalinfo
					retval.AdditionalInfoList.Add(new WasherCycleInfo
					{
						Date = line_date,
						Description = (line.Length > 17) ? line.Substring(17) : "",
						Value = line,
						isAlarm = CheckAlarm(line)
					});
				}
				FireCycleReconstructed(retval);
				return retval;
			}
			catch (Exception e)
			{
				var str = "";
				lines.ForEach(l => str += l + Environment.NewLine);
				Logger.Error(str, e);
				return null;
			}
		}
		private bool CheckAlarm(string line)
		{
			try
			{
				string uline = line.ToUpper();
				return uline.Contains("LID AJAR") ||
					uline.Contains("DIS EXPIRED") ||
					uline.Contains("FLOW ERROR") ||
					uline.Contains("BASIN ERROR") ||
					uline.Contains("HIG DIS RES") ||
					uline.Contains("LOW CBR") ||
					uline.Contains("LOW DIS RES") ||
					uline.Contains("NO FLOW") ||
					uline.Contains("NO AIR FLOW") ||
					uline.Contains("POWER LOW") ||
					uline.Contains("SHEATH TEST FAIL");
			}
			catch (Exception e)
			{
				Logger.Error(line, e);
				return true; //cautelativo
			}
		}
		private void FireCycleReconstructed(WasherCycle cycle) => CycleReconstructed?.Invoke(cycle);
		private DateTime GetDateFromString(string line)
		{
			try
			{
				var str_date = Regex.Match(line, @"\d{2} \w{3} \d{2}:\d{2}");
				if (!str_date.Success)
					return DateTime.MinValue;
				return new DateTime(DateTime.Now.Year,
					GetMonth(str_date.Value.Substring(3, 3)),
					int.Parse(str_date.Value.Substring(0, 2)),
					int.Parse(str_date.Value.Substring(7,2)),
					int.Parse(str_date.Value.Substring(10,2)), 
					0);
			}
			catch (Exception e)
			{
				Logger.Error(line, e);
				return DateTime.MinValue;
			}
		}
		private int GetMonth(string month)
		{
			switch (month.ToUpper())
			{
				case "JAN":
					return 1;
				case "FEB":
					return 2;
				case "MAR":
					return 3;
				case "APR":
					return 4;
				case "MAY":
					return 5;
				case "JUN":
					return 6;
				case "JUL":
					return 7;
				case "AUG":
					return 8;
				case "SEP":
					return 9;
				case "OCT":
					return 10;
				case "NOV":
					return 11;
				case "DEC":
					return 12;
				default:
					return -1;
			}
		}
		private void AddCycle(WasherCycle cycle)
		{
			try
			{
				lock (_cycles_locker)
					_cycles.Add(cycle);
			}
			catch (Exception e)
			{
				Logger.Error(cycle, e);
			}
		}
		/// <summary>
		/// protected e virtual per una futura classe derivata per lavaendoscopi
		/// ancora più vecchie che richiedono velocità di connessioni più lente
		/// </summary>
		/// <returns></returns>
		protected virtual int GetBaudrate() => 4800;

		public override bool Stop()
		{
			try
			{
				if (_com_port == null || !_com_port.IsOpen)
					return true;
				if (_port_polling_task == null)
					return true;
				int msec_timeout = 5000;
				_token_source.Cancel();
				_port_polling_task.Wait(msec_timeout);
				Logger.Error($"port polling task not responding after {msec_timeout} msecs");
				_com_port.Close();
				Running = false;
				return true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return false;
			}
		}
		public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
		{
			try
			{
				lock (_cycles_locker)
				{
					_cycles.ForEach(c => c.WasherID = washer.ID);
					var retval = _cycles.ToList();
					_cycles.Clear();
					return retval;
				}
			}
			catch (Exception e)
			{
				Logger.Error("", e);
				throw;
			}
		}
	}
}
