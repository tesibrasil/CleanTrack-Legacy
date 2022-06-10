using amrfidmgrex;
using ImageSI.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Timers;

namespace ISAWD_PARSER
{
	public partial class Service : ServiceBase
    {
        public bool m_bIsRunning { get; set; }

        public string m_sConnectionDBString { get; set; }
        public string ISAWDPath { get; set; }
        public string NetUseArgs { get; set; }

        private string m_sDoubleQuot { get; set; }

        private System.Timers.Timer timer = new System.Timers.Timer(5000);

        public Service()
        {
            InitializeComponent();

            m_bIsRunning = false;

            m_sConnectionDBString = "";
            ISAWDPath = "";
            NetUseArgs = "";

            m_sDoubleQuot = @"""""";
        }

        protected override void OnStart(string[] args)
        {
            Start(args);
        }

        public void Start(string[] args)
        {
			Program.WriteLog("Start", "INFO", "Starting service...");

            string sConfigPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\isawd.cfg";
            var cfSettings = new ConfigurationFile(sConfigPath, true);

			Program.WriteLog("Start", "INFO", "Loading settings...");

            cfSettings.LoadSettings();

			Program.WriteLog("Start", "INFO", "Settings loaded successfully");

            m_sConnectionDBString = cfSettings.Get("Database", "SQLConnectionString", "DRIVER={SQL Server};UID=sa;PWD=nautilus;SERVER=vmp-srv05;DATABASE=CleanTrack");
            ISAWDPath = cfSettings.Get("Database", "Path", @"C:\sterilizzatrice\report");
            NetUseArgs = cfSettings.Get("Database", "NetUseArgs", "use \\\\10.67.220.22 /user:10.67.220.22\\ims " + m_sDoubleQuot);

            cfSettings.SaveSettings();

            timer.Elapsed += timer_Elapsed;
            try
            {
                if (NetUseArgs != null && NetUseArgs != "")
                {
					Program.WriteLog("Start", "INFO", NetUseArgs);
                    System.Diagnostics.Process.Start("net.exe", NetUseArgs);
                }
            }
            catch (Exception e)
            {
				Program.WriteLog("Start", "ERROR", e.Message);
				Program.WriteLog("Start", "ERROR", e.StackTrace);
            }

            timer.Start();

			Program.WriteLog("Start", "INFO", "...service Started");
        }

        protected override void OnStop()
        {
            timer.Stop();
			Program.WriteLog("OnStop", "INFO", "ARRESTO SERVIZIO");
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!m_bIsRunning)
            {
                m_bIsRunning = true;

				Program.WriteLog("timer_Elapsed", "INFO", "INIZIO");
                try
                {
                    ParseNewReports();
                }
                catch (Exception ez)
                {
					Program.WriteLog("timer_Elapsed", "ERROR", ez.Message);
                }
				Program.WriteLog("timer_Elapsed", "INFO", "FINE");

                m_bIsRunning = false;
            }
        }

        private void ParseNewReports()
        {
            HistoryDBManager hdbmTemp = new HistoryDBManager();
            hdbmTemp.DBQ = GetHistoryDBPath();

            // leggo dall'history l'ultima data //
            DateTime lastDate = hdbmTemp.GetLastDate();
			Program.WriteLog("ParseNewReports", "INFO", "Ultima data " + lastDate.ToString("yyyy-MM-dd HH:mm:ss"));

            List<string> listFilesToParse = new List<string>();

            // string[] sFiles = Directory.GetFiles(ISAWDPath);
			IOrderedEnumerable<string> sFiles = Directory.GetFiles(ISAWDPath).OrderBy(f => f);
			foreach (string sFile in sFiles)
            {
                string sFileName = Path.GetFileName(sFile);

                if (Program.FromFilenameToDate(sFileName) > lastDate)
                {
                    // Program.Log.Info("Add file:" + sFile);
                    listFilesToParse.Add(sFile);
                }
            }

            // per ogni nuovo scontrino trovato... //
            foreach (string sFile in listFilesToParse)
                ParseFile(sFile, hdbmTemp);
        }

        private void ParseFile(string sFile, HistoryDBManager hdbmTemp)
        {
			Program.WriteLog("ParseFile", "INFO", "Parsing: " + sFile);

            Cycle myCycle = ISAWDObject.GetCycleFromFile(sFile);
			// Program.Log.Info(myCycle.Dump());

			if (myCycle.Type == ISAWDObject.m_sAutosanificazione)
			{
				Program.WriteLog("ParseFile", "INFO", "CICLO DI AUTOSANIFICAZIONE");

				hdbmTemp.AddClosedCycle(Path.GetFileName(sFile));

				return;
			}

			//

			if (myCycle.ScopeID == "" || myCycle.ScopeID.Trim() == "0")
				myCycle.ScopeID = "UNKNOWN";

			//

            string sDeviceTag = AMRFIDMGREXExtension.getDeviceTagFromSerial(myCycle.ScopeID, m_sConnectionDBString);
            if (sDeviceTag == "")
                sDeviceTag = AMRFIDMGREXExtension.getDeviceTagFromSerial("UNKNOWN", m_sConnectionDBString);
			int idDev = DBUtilities.getDevIdFromTag(sDeviceTag, m_sConnectionDBString);

			//

            string sOperatorTag = "";
            GetOperatorData(ref sOperatorTag, myCycle);
			int idOp = DBUtilities.getOperatorIdFromTag(sOperatorTag, m_sConnectionDBString);

			//

			if (myCycle.MachineID == "" || myCycle.MachineID.Trim() == "0")
                myCycle.MachineID = "UNKNOWN";
			int iMachine = amrfidmgrex.DBUtilities.getMachineIdFromMat(myCycle.MachineID, m_sConnectionDBString);

			//

			Program.WriteLog("ParseFile", "INFO", ISAWDObject.m_sNumeroSerialeFile + " " + myCycle.MachineID + " (" + iMachine.ToString() + ")");
			Program.WriteLog("ParseFile", "INFO", ISAWDObject.m_sMatricolaFile + " " + myCycle.ScopeID + " (" + sDeviceTag + " --> " + idDev.ToString() + ")");
			Program.WriteLog("ParseFile", "INFO", ISAWDObject.m_sOperatoreFile + " " + myCycle.OperatorID + " (" + sOperatorTag + " --> " + idOp.ToString() + ")");

			if (myCycle.Completed)
            {
				if (myCycle.Failed)
				{
					Program.WriteLog("ParseFile", "ERROR", "!!!!!! CICLO COMPLETATO CON ERRORI !!!!!!");
					return;
				}
				else
				{
					Program.WriteLog("ParseFile", "INFO", "CICLO COMPLETATO CON SUCCESSO");
				}

				// metto la sonda a "PULITO" //
				if (SaveCycleCompleted(sOperatorTag, idOp, sDeviceTag, idDev, iMachine, myCycle))
                    hdbmTemp.AddClosedCycle(Path.GetFileName(sFile));
            }
            else
            {
				string fName = Path.GetFileName(sFile);

				Program.WriteLog("ParseFile", "INFO", fName + " -> CICLO NON ANCORA COMPLETATO");

				// metto la sonda a "IN LAVAGGIO" //
                SaveCycleNotCompleted(sOperatorTag, idOp, sDeviceTag, idDev, iMachine, myCycle);
            }
        }

        private void SaveCycleNotCompleted(string operatorTag, int idOp, string sDeviceTag, int idDev, int machine, Cycle myCycle)
        {
            if (operatorTag != null && operatorTag != "" &&
                sDeviceTag != null & sDeviceTag != "" &&
                machine > 0)
            {
                var currentState = DBUtilities.getStateFromTag(sDeviceTag, m_sConnectionDBString);
				Program.WriteLog("SaveCycleNotCompleted", "INFO", "CurrentState: " + currentState.ToString());

                try
                {
                    if (currentState != (int)Types.State.Washing)
                    {
                        OdbcConnection conn = new OdbcConnection(m_sConnectionDBString);
                        conn.Open();

                        int idCycle = DBUtilities.getLastValidCycleId(idDev, conn, Types.State.Washing);

                        if (idCycle > 0)
                        {
                            if (!DBUtilities.setCycleWashInfo(idCycle, myCycle.StartTimestamp.ToString(@"yyyyMMddHHmmss"), idOp, machine, conn))
								Program.WriteLog("SaveCycleNotCompleted", "ERROR", "Unable setcycleCleanInfo");
                        }
                        else
                        {
							Program.WriteLog("SaveCycleNotCompleted", "ERROR", "NO VALID CYCLE FOUND!....INSERTING NEW ONE (1)");

                            if (!DBUtilities.insCycleStartingWash(idDev, myCycle.StartTimestamp.ToString(@"yyyyMMddHHmmss"), operatorTag, machine, conn))
								Program.WriteLog("SaveCycleNotCompleted", "ERROR", "Error inserting cycle starting Washing");

                            idCycle = DBUtilities.getLastValidCycleId(idDev, conn, Types.State.Clean);
                        }

                        if (idCycle > 0)
                        {
                            if (!DBUtilities.setState(sDeviceTag, (int)Types.State.Washing, conn))
								Program.WriteLog("SaveCycleNotCompleted", "ERROR", "Unable set state washin deviceTag");
                            else
								Program.WriteLog("SaveCycleNotCompleted", "ERROR", sDeviceTag + " in washing state");
                        }

                        conn.Close();

                    }
                    else
						Program.WriteLog("SaveCycleNotCompleted", "INFO", "Device already in washing state....no operation executed");
                }
                catch (Exception exc)
                {
					Program.WriteLog("SaveCycleNotCompleted", "ERROR", exc.Message);
                }
            }
        }

        private bool SaveCycleCompleted(string operatorTag, int idOp, string sDeviceTag, int idDev, int iMachine, Cycle myCycle)
        {
            bool result = false;

			if ((operatorTag != null) && (operatorTag != "") && (sDeviceTag != null) && (sDeviceTag != "") && (iMachine > 0))
			{
				try
				{
					OdbcConnection conn = new OdbcConnection(m_sConnectionDBString);
					conn.Open();

					int idCycle = DBUtilities.getLastValidCycleId(idDev, conn, Types.State.Clean);

					if (idCycle > 0)
					{
                        DBUtilities.setCycleWashInfo(idCycle, myCycle.StartTimestamp.ToString(@"yyyyMMddHHmmss"), idOp, iMachine, conn);
                        if (!DBUtilities.setCycleCleanInfo(idCycle, myCycle.EndTimestamp.ToString(@"yyyyMMddHHmmss"), idOp, m_sConnectionDBString))
    						Program.WriteLog("SaveCycleCompleted", "ERROR", "Unable setcycleCleanInfo");
						else
							result = true;
					}
					else
					{
						Program.WriteLog("SaveCycleCompleted", "INFO", "NO VALID CYCLE FOUND!....INSERTING NEW ONE (2)");
						
						// metto la sonda a "IN LAVAGGIO" //
						SaveCycleNotCompleted(operatorTag, idOp, sDeviceTag, idDev, iMachine, myCycle);

						idCycle = DBUtilities.getLastValidCycleId(idDev, conn, Types.State.Clean);
						if (idCycle > 0)
						{
							if (!DBUtilities.setCycleCleanInfo(idCycle, myCycle.EndTimestamp.ToString(@"yyyyMMddHHmmss"), idOp, m_sConnectionDBString))
								Program.WriteLog("SaveCycleCompleted", "ERROR", "Unable setcycleCleanInfo");
						}
						else
						{
							if (!DBUtilities.insCycleStartingClean(idDev, myCycle.EndTimestamp.ToString(@"yyyyMMddHHmmss"), operatorTag, conn))
								Program.WriteLog("SaveCycleCompleted", "ERROR", "Error inserting cycle starting dirty");
							else
								idCycle = DBUtilities.getLastValidCycleId(idDev, conn, Types.State.Unknown);
						}
					}

					if (idCycle > 0)
					{
						if (!DBUtilities.setState(sDeviceTag, (int)Types.State.Clean, conn))
							Program.WriteLog("SaveCycleCompleted", "ERROR", "Unable set state deviceTag");
						else
						{
							Program.WriteLog("SaveCycleCompleted", "INFO", sDeviceTag + " in clean State");
							AMRFIDMGREXExtension.insertNewCycleMedivators(sDeviceTag, operatorTag, iMachine, myCycle.Failed ? 99999 : 1, 3, 0, myCycle.EndTimestamp, myCycle, m_sConnectionDBString, idCycle);
							result = true;
						}
					}

					conn.Close();
				}
				catch (Exception exc)
				{
					Program.WriteLog("SaveCycleCompleted", "ERROR", exc.Message);
				}
			}
			else
			{
				string sError = "";

				if ((operatorTag == null) || (operatorTag == ""))
				{
					if (sError.Length > 0)
						sError += " - ";
					sError += "operatore non riconosciuto";
				}

				if ((sDeviceTag == null) || (sDeviceTag == ""))
				{
					if (sError.Length > 0)
						sError += " - ";
					sError += "strumento non riconosciuto";
				}

				if (iMachine <= 0)
				{
					if (sError.Length > 0)
						sError += " - ";
					sError += "sterilizzatrice non riconosciuta";
				}

				if (sError.Length > 0)
				{
					Program.WriteLog("SaveCycleCompleted", "ERROR", "ERRORE nel salvataggio del ciclo --> " + sError);
				}

				// Sandro 24/04/2019 // ritorno comunque true così aggiorno la data dell'ultimo scontrino processato, è inutile che continuo a riprocessarlo se non riesco a salvarlo nel db per un qualsiasi motivo //
				result = true;
			}

			return result;
        }

        private void GetOperatorData(ref string sOperatorTag, Cycle myCycle)
        {
			sOperatorTag = AMRFIDMGREXExtension.getOperatorTagFromMatricola(myCycle.OperatorID.Trim(), m_sConnectionDBString);

            if (sOperatorTag == "")
                sOperatorTag = AMRFIDMGREXExtension.getOperatorTagFromTag(myCycle.OperatorID.Trim(), m_sConnectionDBString);

            if (sOperatorTag == "")
                sOperatorTag = AMRFIDMGREXExtension.getOperatorTagFromNomeCognome("UNKNOWN", "UNKNOWN", m_sConnectionDBString);
        }

        private string GetHistoryDBPath()
        {
            var fiTemp = new FileInfo(Assembly.GetExecutingAssembly().FullName);
            string sDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            return sDir + @"\CyclesHistory.mdb";
        }

        public void CleanHistoryDB()
        {
            string sConfigPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\isawd.cfg";
            var cfSettings = new ConfigurationFile(sConfigPath, true);

			Program.WriteLog("CleanHistoryDB", "INFO", "Loading settings...");

            cfSettings.LoadSettings();

			Program.WriteLog("CleanHistoryDB", "INFO", "Settings loaded successfully");

            m_sConnectionDBString = cfSettings.Get("Database", "SQLConnectionString", "DRIVER={SQL Server};UID=sa;PWD=nautilus;SERVER=vmp-srv05;DATABASE=CleanTrack");
            ISAWDPath = cfSettings.Get("Database", "Path", @"C:\sterilizzatrice\report");
            NetUseArgs = cfSettings.Get("Database", "NetUseArgs", "use \\\\10.67.220.22 /user:10.67.220.22\\ims " + m_sDoubleQuot);

            cfSettings.SaveSettings();

			Program.WriteLog("CleanHistoryDB", "INFO", "Erasing CycleHistory...");
            HistoryDBManager hdbmTemp = new HistoryDBManager();
            hdbmTemp.DBQ = GetHistoryDBPath();
            hdbmTemp.DeleteAllCycle();
			Program.WriteLog("CleanHistoryDB", "INFO", "...Erasing CycleHistory");
        }
    }
}
