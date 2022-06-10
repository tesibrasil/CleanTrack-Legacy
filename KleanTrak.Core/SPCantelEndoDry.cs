using KleanTrak.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KleanTrak.Core
{
	public partial class SPCantelEndoDry : WPBase
	{
		public XmlSerializer serializer = new XmlSerializer(typeof(HIS));
		public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
		{
			var retcycles = new List<WasherCycle>();
			try
			{
				if (!CheckWasherSerialNumber(washer))
					return retcycles;
				if (!Directory.Exists(FolderOrFileName))
					throw new ApplicationException($"Directory {FolderOrFileName} doesn't exists");
				var dirinfo = new DirectoryInfo(FolderOrFileName);
				foreach (var file in dirinfo.GetFiles("*.xml", SearchOption.AllDirectories))
				{
					if (file.FullName.Contains(OldDir))
						continue;
					var cycle = ParseFile(file);
					if (cycle != null)
						retcycles.Add(cycle);
					// StateTransaction memorizes filename and data as already processed
					MoveToOldDir(file.FullName);
				}
			}
			catch (Exception e)
			{
				Logger.Error($"lastDate: {lastDate}", e);
			}
			return retcycles;
		}

		private WasherCycle ParseFile(FileInfo file)
		{
			FileStream fstream = null;
			try
			{
				Logger.Info($"parsing file {file.FullName}");
				var newcycle = CreateCycle(file);
				fstream = File.OpenRead(file.FullName);
				var his = (HIS)serializer.Deserialize(fstream);
				if (his.IN != null)
					FillInCycle(newcycle, his.IN);
				else if (his.OUT != null)
					FillOutCyle(newcycle, his.OUT);
				else
					throw new ApplicationException("IN and OUT info are null");
				return newcycle;
			}
			catch (Exception e)
			{
				Logger.Error($"filename: {file.FullName}", e);
				return null;
			}
			finally
			{
				if (fstream != null)
					fstream.Close();
			}
		}

		private void FillOutCyle(WasherCycle newcycle, OUT hisout)
		{
			try
			{
				newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.End_store);
				newcycle.Failed = false; // hisout.Processdata.ProcessResult != 2; non si capisce quali siano i codici di errore
				newcycle.CycleCount = hisout.Processdata.ActualRunCycle;
				newcycle.StartTimestamp = hisout.TakeOut.Time;
				newcycle.EndTimestamp = hisout.TakeOut.Time;
				newcycle.StartTimestamp = hisout.TakeOut.Time;
				newcycle.WasherExternalID = hisout.EndoStore.SerialNo;
				newcycle.WasherID = TranscodeWasher(hisout.EndoStore.SerialNo);
				newcycle.DeviceExternalID = hisout.Instrument.Barcode;
				newcycle.DeviceID = TranscodeDevice(hisout.Instrument.Barcode);
				newcycle.OperatorEndExternalID = hisout.TakeOut.UserBarcode;
				newcycle.OperatorEndID = TranscodeOperator(hisout.TakeOut.UserBarcode);
				SetAddittionalInfo(newcycle, hisout.Processdata, hisout.TakeOut.Time);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				throw;
			}
		}

		private void FillInCycle(WasherCycle newcycle, IN hisin)
		{
			try
			{
				newcycle.DesiredDestinationState = StateTransactions.GetStateId(FixedStates.Start_store);
				newcycle.Failed = false; // essendo inserimento in armadio non può fallire
				newcycle.CycleCount = hisin.Processdata.ActualRunCycle;
				newcycle.StartTimestamp = hisin.PutIn.Time;
				newcycle.EndTimestamp = hisin.PutIn.Time;
				newcycle.WasherExternalID = hisin.EndoStore.SerialNo;
				newcycle.WasherID = TranscodeWasher(hisin.EndoStore.SerialNo);
				newcycle.DeviceExternalID = hisin.Instrument.Barcode;
				newcycle.DeviceID = TranscodeDevice(hisin.Instrument.Barcode);
				newcycle.OperatorStartExternalID = hisin.PutIn.UserBarcode;
				newcycle.OperatorStartID = TranscodeOperator(hisin.PutIn.UserBarcode);
				SetAddittionalInfo(newcycle, hisin.Processdata, hisin.PutIn.Time);
			}
			catch (Exception e)
			{
				Logger.Error(e);
				throw;
			}
		}

		private void SetAddittionalInfo(WasherCycle newcycle, Processdata processdata, DateTime datetime)
		{
			try
			{
				// additional info
				newcycle.AdditionalInfoList.Add(new WasherCycleInfo
				{
					Description = "Processdata.Patientinfo",
					Value = processdata.Patientinfo,
					Date = datetime,
					isAlarm = false
				});
				newcycle.AdditionalInfoList.Add(new WasherCycleInfo
				{
					Description = "Processdata.SecurityCheck",
					Value = processdata.SecurityCheck.ToString(),
					Date = datetime,
					isAlarm = false
				});
				newcycle.AdditionalInfoList.Add(new WasherCycleInfo
				{
					Description = "Processdata.WdRunCycle",
					Value = processdata.WdRunCycle,
					Date = datetime,
					isAlarm = false
				});
				newcycle.AdditionalInfoList.Add(new WasherCycleInfo
				{
					Description = "Processdata.LastRunCycle",
					Value = processdata.LastRunCycle,
					Date = datetime,
					isAlarm = false
				});
				newcycle.AdditionalInfoList.Add(new WasherCycleInfo
				{
					Description = "Processdata.Runtime",
					Value = processdata.Runtime,
					Date = datetime,
					isAlarm = false
				});

			}
			catch (Exception e)
			{
				Logger.Error($"newcycle: {newcycle}", e);
				throw;
			}
		}

		private WasherCycle CreateCycle(FileInfo file)
		{
			FileStream fstream = null;
			StreamReader streamreader = null;
			try
			{
				fstream = File.OpenRead(file.FullName);
				streamreader = new StreamReader(fstream);
				return new WasherCycle
				{
					Filename = file.FullName,
					FileDatetime = file.CreationTime,
					FileContent = streamreader.ReadToEnd(),
					IsStorage = true,
					Completed = true
				};
			}
			catch (Exception e)
			{
				Logger.Error($"filename: {file.FullName}", e);
				throw;
			}
			finally
			{
				if (streamreader != null)
					streamreader.Close();
				if (fstream != null)
					fstream.Close();
			}
		}
	}
}
