using ImageSI.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using LibLog;
using amrfidmgrex;
using System.IO;
using System.Net.NetworkInformation;

namespace TRfid
{
	public partial class Service : ServiceBase
	{

		private static string hostName = "";
		private static string address = "";
		private static string OdbcConnection = "";
		private static int state = 3;
		private static int cleaner = -1;
		private static List<int> AllowedStateList = new List<int>();
		private static System.Timers.Timer timer = new System.Timers.Timer(60000);
		private static bool pingRunning = false;

		public Service()
		{
			InitializeComponent();
		}

		public void DebugService()
		{
			try
			{
				StartProc();
				Console.ReadLine();
				StopProc();
			}
			catch (Exception e)
		 {
				Console.WriteLine(e.Message);
			}
	  }

		protected override void OnStart(string[] args)
		{
			StartProc();
		}

		protected override void OnStop()
		{
			StopProc();
		}

		public static void StartProc()
		{
			Logger.Get().Verbosity = Logger.VerbosityEnum.All;
			Logger.Get().ActivateFileDestination(new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory.Name, 
				DateTime.Now.ToString("yyyyMMdd") + ".log", true, true);
			Logger.Get().Write("internal",
									 "Cleantrack.Service",
									 "Starting Service",
									 null,
									 Logger.LogLevel.Info);

			WriteLog("Starting process");
			hostName = NetworkUtility.GetHostName();
			GetSettings();

			if (NetworkUtility.PingAddress(address))
			{
				StepManager.Init(address, OdbcConnection);
				StepManager.RunLoop(cleaner, state, AllowedStateList);
				timer.Elapsed += Timer_Elapsed;
				timer.Start();
			}
			else
				 throw new Exception("Cannot reach RFID Reader ...");
		}

		public void UpdateDB(string devTag, string opTag)
		{
			Logger.Get().Verbosity = Logger.VerbosityEnum.All;
			Logger.Get().ActivateFileDestination(new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory.Name, 
				DateTime.Now.ToString("yyyyMMdd") + ".log", true, true);
			Logger.Get().Write("internal",
									 "Cleantrack.Service",
									 "UpdateDB",
									 null,
									 Logger.LogLevel.Info);

			WriteLog("Starting process");
			hostName = NetworkUtility.GetHostName();
			GetSettings();

			Step.ODBCConnectionString = OdbcConnection;
			Step.ListAllowedType = AllowedStateList;
			var step = new Step(state);
			step.DirectUpdateDB(devTag, opTag);

			WriteLog("end process...");
		}

		private static void GetSettings()
		{
			string configPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\trfid.cfg";
			var ConfigFile = new ConfigurationFile(configPath, true);
			WriteLog("Loading settings...");
			ConfigFile.LoadSettings();
			WriteLog("Settings loaded successfully");

			OdbcConnection = ConfigFile.GetODBCConnectionString("DRIVER={SQL Server};UID=sa;PWD=nautilus;SERVER=vmp-srv05;DATABASE=CleanTrack");
			Logger.Get().ActivateDatabaseDestination(OdbcConnection);
			WriteLog("Connection string:" + OdbcConnection);

			address = ConfigFile.Get("ReaderAddress", "10.171.1.50:10001");
			WriteLog("adress rfid reader: " + address);

			state = (int)Types.State.Washing;
			string stateString = ConfigFile.Get("StateToSet", state.ToString());

			try
			{
				state = int.Parse(stateString);
			}
			catch
			{
				state = (int)Types.State.Washing;
			}

			WriteLog(">>> State to set: " + state.ToString());

			AllowedStateList = new List<int>();
			string AllowedStateString = ConfigFile.Get("AllowedStateList", "-1");
			WriteLog(">>> AllowedStates in config : " + AllowedStateString);

			//
			cleaner = -1;
			string cleanerString = ConfigFile.Get("Cleaner", "-1");

			try
			{
				cleaner = int.Parse(cleanerString);
			}
			catch
			{
				cleaner = -1;
			}

			WriteLog("cleanerID: " + cleaner.ToString());

			ConfigFile.SaveSettings();

			try
			{
				string[] separatedStates;

				char[] separators = { ',' };

				separatedStates = AllowedStateString.Split(separators);

				if (separatedStates != null)
				{
					foreach (string a in separatedStates)
					{
						try
						{
							int parsed = int.Parse(a);
							AllowedStateList.Add(parsed);
						}
						catch (Exception ex)
						{
							WriteLog(ex.Message);
							WriteLog(ex.StackTrace);
						}
					}
				}
			}
			catch
			{
				AllowedStateList = new List<int>();
			}

			WriteLog("AllowedStateParsed: ");
			foreach (var item in AllowedStateList)
			{
				WriteLog(item.ToString());
			}

			var logVerbosity = ConfigFile.Get("LogVerbosity", "ExcludeInfo");
			switch (logVerbosity.ToUpper())
			{
				case "EXCLUDEINFO":
					Logger.Get().Verbosity = Logger.VerbosityEnum.ExcludeInfo;
				break;
				case "ONLYERROR":
					Logger.Get().Verbosity = Logger.VerbosityEnum.OnlyError;
					break;
				default:
					Logger.Get().Verbosity = Logger.VerbosityEnum.All;
					break;
			}

			WriteLog("Current log verbosity: " + Logger.Get().Verbosity.ToString());
		}

		static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			WriteLog("Service.TimerElapsed");
			if (!pingRunning)
			{
				pingRunning = true;
				try
				{
					if (!StepManager.isReaderConnected())
					{
						if (NetworkUtility.PingAddress(address))
						{
							StepManager.Reset();
							StepManager.RunLoop(cleaner, state, AllowedStateList);
						}
					}
				}
				catch (Exception ex)
				{
					WriteLog(ex.Message);
					WriteLog(ex.StackTrace);
				}

				pingRunning = false;
			}
		}

		public static void StopProc()
		{
			timer.Stop();
			timer.Dispose();
			WriteLog("Stopping Service");
			StepManager.Finish();
		}

		private static void WriteLog(string text)
		{
			try
			{
				Logger.Get().Write(hostName,
					"Cleantrack.Service",
					text, null,
					Logger.LogLevel.Info);
				Console.WriteLine(text);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
		}
	}
}
