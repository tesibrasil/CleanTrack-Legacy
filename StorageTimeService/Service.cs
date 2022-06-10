using ImageSI.Configuration;
using System;
using System.Data.Odbc;
using System.ServiceProcess;

namespace StorageTimeService
{
	public partial class Service : ServiceBase
	{
		private static string odbcConnection = "";
		private static System.Timers.Timer TimerRefresh = null;
		private static TimeSpan MaxStorageTime = new TimeSpan(0, 0, 0, 0, 0);
		private static bool Debug = true;
		private static bool EnableLog = true;
		private static bool NewKleantrak = true;


		public Service()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			StartProc();
		}

		protected override void OnStop()
		{
			StopProc();
		}

		static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			writeLog("Service.TimerElapsed...");

			if (Debug)
				writeLog("Debug Mode ON!!!");

			if (NewKleantrak)
				NewTimerElapsed();
			else
				TimerElapsed();

			writeLog("...Service.TimerElapsed");
		}

		private static void NewTimerElapsed()
		{
			OdbcConnection db = new OdbcConnection(odbcConnection);

			try
			{
				db.Open();
				OdbcCommand cmd = new OdbcCommand();
				cmd.Connection = db;
				cmd.CommandText = "SELECT id, stato, idstatodestinazione, idoperatorestato from vistadispositiviscaduti";

				OdbcDataReader rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					var devID = rdr.GetInt32(0);
					var stateOld = rdr.GetInt32(1);
					var stateNew = rdr.GetInt32(2);
					var userID = rdr.GetInt32(3);
					DateTime now = DateTime.Now;

					writeLog("Found device " + devID.ToString() + " in Overtime...");

					int lastCycleID = 0;
					var db2 = new OdbcConnection(odbcConnection);
					db2.Open();
					var rdrMaxId = new OdbcCommand("SELECT MAX(ID) AS IDCICLO FROM CICLI WHERE IDDispositivo = " + devID.ToString(), db2).ExecuteReader();
					if (rdrMaxId.HasRows)
					{
						lastCycleID = rdrMaxId.GetInt32(0);
						var queryIns = string.Format("INSERT INTO CicliStatoLog (IDCICLO, IDSTATOOLD, IDSTATONEW, IDOPERATORE, DATAORA) VALUES ({0:d}, {1:d}, {2:d}, {3:d}, '{4:d4}{5:d2}{6:d2}{7:d2}{8:d2}{9:d2}')",
													 lastCycleID, stateOld, stateNew, userID,
													 now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

						var db3 = new OdbcConnection(odbcConnection);
						db3.Open();
						writeLog("Adding item in ciclistatolog, idciclo: " + lastCycleID.ToString());
						new OdbcCommand(queryIns, db3).ExecuteNonQuery();
						db3.Close();
					}
					rdrMaxId.Close();
					db2.Close();

					var queryUpd = string.Format("UPDATE Dispositivi SET Stato = {0:d}, IDOperatoreStato = {1:d}, DataStato = '{2:d4}{3:d2}{4:d2}{5:d2}{6:d2}{7:d2}' WHERE ID = {8:d}",
												 stateNew, userID,
												 now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second,
												 devID);

					var db4 = new OdbcConnection(odbcConnection);
					db4.Open();
					writeLog("Updating dispositivi setting device " + devID.ToString() + " with new state:" + stateNew.ToString());
					new OdbcCommand(queryUpd, db4).ExecuteNonQuery();
					db4.Close();
				}

				if (rdr != null && !rdr.IsClosed)
					rdr.Close();
			}
			catch (Exception ex)
			{
				writeLog(ex.Message);
				writeLog(ex.StackTrace);
			}

			db.Close();

			return;
		}

		private static void TimerElapsed()
		{
			var devices = amrfidmgrex.DBUtilities.getCleanDevice(odbcConnection);

			foreach (var item in devices)
			{
				DateTime cleanDate = DateTime.Now;
				if (amrfidmgrex.DBUtilities.GetCleanDate(ref cleanDate, item, odbcConnection))
				{
					if (DateTime.Now.Subtract(cleanDate) > MaxStorageTime)
					{
						writeLog("Found device " + item.ToString() + " in Overtime... Current StoreDate:" + cleanDate.ToShortDateString());
						if (Debug)
							writeLog("Debug Mode On UpdateNotExecuted on: " + item.ToString());
						else
							amrfidmgrex.DBUtilities.SetOvertTimeState(item, odbcConnection);
					}
					else
						writeLog("Device " + item.ToString() + " not in Overtime, " + DateTime.Now.Subtract(cleanDate).TotalSeconds.ToString() + " from last clean");
				}
			}
		}

		public static void StopProc()
		{
			TimerRefresh.Stop();
			TimerRefresh.Dispose();
			writeLog("Stopping Service");
		}

		public static void writeLog(string text)
		{
			if (!EnableLog)
				return;

			try
			{
				Program.Log.Info(text);
				Console.WriteLine(text);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
		}

		public static void StartProc()
		{
			writeLog("Starting process....");
			GetSettings();
			TimerRefresh.Elapsed += timer_Elapsed;
			timer_Elapsed(null, null);
			TimerRefresh.Start();
			writeLog("...process Started");
		}

		public static void GetSettings()
		{
			try
			{
				string configPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\storagetime.cfg";
				var ConfigFile = new ConfigurationFile(configPath, true);
				writeLog("Loading settings...");
				ConfigFile.LoadSettings();
				writeLog("Settings loaded successfully");

				//
				odbcConnection = ConfigFile.GetODBCConnectionString("DRIVER={SQL Server};UID=sa;PWD=nautilus;SERVER=vmp-srv05;DATABASE=CleanTrack");
				writeLog("Connection string:" + odbcConnection);


				EnableLog = true;
				writeLog("Enable Log: " + EnableLog.ToString());

				//
				Debug = false;
				writeLog("Debug: " + Debug.ToString());

				//
				NewKleantrak = true;
				writeLog("NewKleantrak: " + NewKleantrak.ToString());

				TimerRefresh = new System.Timers.Timer(60 * 1000);
				MaxStorageTime = new TimeSpan(30, 0, 0, 0, 0);

				//
				//string enableLogString = ConfigFile.Get("EnableLog", "0");
				//EnableLog = (enableLogString == "1");
				//writeLog("Enable Log: " + EnableLog.ToString());

				////
				//string DebugString = ConfigFile.Get("Debug", "0");
				//Debug = (DebugString == "1");
				//writeLog("Debug: " + Debug.ToString());

				////
				//var newKleantrak = ConfigFile.Get("NewKleantrak", "1");
				//NewKleantrak = (newKleantrak == "1");
				//writeLog("NewKleantrak: " + NewKleantrak.ToString());

				//TimerRefresh = new System.Timers.Timer(Int32.Parse(ConfigFile.Get("TimerPollingSeconds", "60")) * 1000);
				//MaxStorageTime = new TimeSpan(Int32.Parse(ConfigFile.Get("MaxStorageTimeDay", "30")), 0, 0, 0, 0);

				ConfigFile.SaveSettings();
			}
			catch (Exception exc)
			{
				writeLog(exc.Message);
				writeLog(exc.StackTrace);
			}

		}
	}
}
