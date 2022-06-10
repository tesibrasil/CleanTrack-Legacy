using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using amrfidmgrex;
using ImageSI.Configuration;

namespace MedivatorsCleantrackParser
{
	public partial class MedivatorsService : ServiceBase
	{

		public bool IsRunning { get; set; }
		public string MedUser { get; set; }
		public string MedPassword { get; set; }
		public string MedPath { get; set; }
		public string ConnectionDBString { get; set; }

		// private System.Timers.Timer timer = new System.Timers.Timer(300000);
		private System.Timers.Timer timer = new System.Timers.Timer(5000);

		private string tempLocalPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\temp.mdb";

		public MedivatorsService()
		{
			InitializeComponent();
			IsRunning = false;
		}

		private bool checkAndTransfer()
		{
			bool res = true;

			try
			{
				if (System.IO.File.Exists(tempLocalPath))
					System.IO.File.Delete(tempLocalPath);

				if (System.IO.File.Exists(MedPath))
					System.IO.File.Copy(MedPath, tempLocalPath);
			}
			catch (Exception f)
			{
				writeLog(f.ToString());
				res = false;
			}
			return res;
		}

		public void Start()
		{
			OnStart(null);
		}

		protected override void OnStart(string[] args)
		{

			MedUser = "";
			MedPassword = "";
			MedPath = "";

			string configPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\medivators.cfg";
			var ConfigFile = new ConfigurationFile(configPath, true);

			writeLog("Loading settings...");

			ConfigFile.LoadSettings();

			writeLog("Settings loaded successfully");

			MedUser = ConfigFile.Get("Database", "User", "admin");
			MedPassword = ConfigFile.Get("Database", "Password", "");
			MedPath = ConfigFile.Get("Database", "Path", "./Medivators.Reporting.mdb");
			ConnectionDBString = ConfigFile.Get("Database", "SQLConnectionString", "DRIVER={SQL Server};UID=sa;PWD=nautilus;SERVER=vmp-srv05;DATABASE=CleanTrack");
			ConfigFile.Set("Database", "User", MedUser);
			ConfigFile.Set("Database", "Password", MedPassword);
			ConfigFile.Set("Database", "Path", MedPath);
			ConfigFile.Set("Database", "SQLConnectionString", ConnectionDBString);
			ConfigFile.SaveSettings();
			timer.Elapsed += timer_Elapsed;
			timer.Start();
		}

		void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			if (!IsRunning)
			{
				IsRunning = true;
				checkAndTransfer();
				GoLimitedEx();
				IsRunning = false;
			}
		}

		protected override void OnStop()
		{
			timer.Stop();
		}

		private string GetHistoryDBPath()
		{
			var finfo = new FileInfo(Assembly.GetExecutingAssembly().FullName);
			string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
			return dir + @"\CyclesHistory.mdb";
		}

		private bool IsValid(DateTime dt)
		{
			return dt != null && dt != DateTime.MinValue && dt != DateTime.MaxValue && !(dt.Hour == 0 && dt.Minute == 0);
		}


		private bool GoExToClose()
		{
			bool result = false;

			HistoryDBManager historydb = new HistoryDBManager();
			historydb.DBQ = GetHistoryDBPath();
			IEnumerable<DateTime> dateList = historydb.GetExamToClose();

			var medDb = new MedivatorsDBAnalyzer();
			medDb.UID = MedUser;
			// medDb.DBQ = MedPath;
			medDb.DBQ = tempLocalPath;
			medDb.PWD = MedPassword;

			List<Cycle> cyclesToParse = new List<Cycle>();

			if (medDb.ExtractExamToClose(cyclesToParse, dateList))
			{
				foreach (Cycle serieToParse in cyclesToParse)
				{
					if (IsValid(serieToParse.CreateDateTime) && IsValid(serieToParse.EndTimestamp))
					{
						int deviceId = DBUtilities.getDeviceIdFromMat("" + serieToParse.ScopeID, ConnectionDBString);
						string deviceTag = DBUtilities.getDeviceTagFromId(deviceId, ConnectionDBString);
						int operatorId = DBUtilities.getOperatorIdFromMat("" + serieToParse.OperatorID, ConnectionDBString);
						string operatortag = DBUtilities.getOperatorTagFromId(operatorId, ConnectionDBString);

						int machine = amrfidmgrex.DBUtilities.getMachineIdFromMat("" + serieToParse.MachineID, ConnectionDBString);
						bool clean = false;

						foreach (AdditionalInfo info in serieToParse.AdditionalInfoList)
						{
							if (info.Description.Trim() == "COMPLETED")
							{
								clean = true;
								break;
							}
						}

						AmRfidManagerExtension.insertNewCycleMedivators(deviceTag, operatortag, machine, clean ? 1 : 99999, 3, 0, serieToParse.EndTimestamp, serieToParse, ConnectionDBString);

						historydb.CloseCycle(serieToParse.CreateDateTime);
					}
				}

				result = true;
			}
			else
				return false;

			return result;
		}

		private bool GoLimitedEx()
		{
			try
			{

				if (File.Exists(GetHistoryDBPath()))
				{
					bool result = false;

					HistoryDBManager historydb = new HistoryDBManager();
					historydb.DBQ = GetHistoryDBPath();

					writeLog("Getting last date ...");

					DateTime date = historydb.GetLastDate();

					writeLog(date.ToString("dd/MM/yyyy HH:mm:ss"));

					if (File.Exists(tempLocalPath))
					{

						var medDb = new MedivatorsDBAnalyzer();
						medDb.UID = MedUser;
						medDb.DBQ = tempLocalPath;
						medDb.PWD = MedPassword;

						writeLog("Extracting cycles ...");

						List<Cycle> cyclesToParse = medDb.Extract(date);

						foreach (Cycle serieToParse in cyclesToParse)
						{
							if (IsValid(serieToParse.CreateDateTime) && IsValid(serieToParse.StartTimestamp))
							{
								int deviceId = DBUtilities.getDeviceIdFromMat("" + serieToParse.ScopeID, ConnectionDBString);
								string deviceTag = DBUtilities.getDeviceTagFromId(deviceId, ConnectionDBString);

								writeLog("Device ID:" + deviceId.ToString() + " - TAG:" + deviceTag);

								int operatorId = DBUtilities.getOperatorIdFromMat("" + serieToParse.OperatorID, ConnectionDBString);
								string operatortag = DBUtilities.getOperatorTagFromId(operatorId, ConnectionDBString);

								writeLog("Operator ID:" + operatorId.ToString() + " - TAG:" + operatortag);
								//Sterilizzatrice

								int machine = amrfidmgrex.DBUtilities.getMachineIdFromMat("" + serieToParse.MachineID, ConnectionDBString);
								writeLog("WashMachine ID:" + machine.ToString());
								//Sterilizzatrice

								DBUtilities.insertnewCycleWithDate(deviceTag, operatortag, machine, 3, 2, 0, serieToParse.StartTimestamp, ConnectionDBString);
								historydb.AddCycle(serieToParse.CreateDateTime);
							}

							result = true;
						}

						result = GoExToClose() && result;

						return result;
					}
					else
					{
						writeLog("Path DB Medivators non raggiungibile ...");
						return false;
					}
				}
				else
				{
					writeLog("Path DB History non raggiungibile ...");
					return false;
				}
			}
			catch (Exception e)
			{
				writeLog(e.ToString());
				return false;
			}
		}

		private void writeLog(string text)
		{
			try
			{
				string logName = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
				StreamWriter sw = new StreamWriter(logName, true);
				sw.WriteLine(DateTime.Now.ToString("HH:mm") + " - " + text);
				sw.Close();
			}
			catch
			{
			}
		}
	}
}
