using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using amrfidmgrex;
using ImageSI.Configuration;

namespace IMS7_PARSER
{
    public partial class Service : ServiceBase
    {
        public bool IsRunning { get; set; }

        public string IMSPath { get; set; }
        public string ConnectionDBString { get; set; }
        public string NetUseArgs { get; set; }

        public int TimerDelay { get; set; }

        private System.Timers.Timer timer;

        public Service()
        {

            InitializeComponent();
            IsRunning = false;
        }


        protected override void OnStart(string[] args)
        {
            Start();
        }

        public void Start()
        {
            IMSPath = "";

            string configPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\ims7.cfg";
            var ConfigFile = new ConfigurationFile(configPath, true);

            Program.Log.Info("Loading settings...");

            ConfigFile.LoadSettings();

            Program.Log.Info("Settings loaded successfully");

            try
            {
                IMSPath = ConfigFile.Get("Database", "Path", @"C:\sterilizzatrice\rpt");
                ConnectionDBString = ConfigFile.Get("Database", "SQLConnectionString", "DRIVER={SQL Server};UID=sa;PWD=nautilus;SERVER=vmp-srv05;DATABASE=CleanTrack");
                string doubleq = @"""""";
                NetUseArgs = ConfigFile.Get("Database", "NetUseArgs", "use \\\\10.67.220.21 /user:10.67.220.21\\administrator " + doubleq);
                TimerDelay = Int32.Parse(ConfigFile.Get("Timer", "Delay", "180000", false));
            }
            catch (Exception e)
            {
                Program.Log.Error(e.Message);
                Program.Log.Error(e.StackTrace);
            }


            ConfigFile.Set("Database", "Path", IMSPath);
            ConfigFile.Set("Database", "SQLConnectionString", ConnectionDBString);
            ConfigFile.Set("Database", "NetUseArgs", NetUseArgs);
            ConfigFile.Set("Timer", "Delay", TimerDelay.ToString());
            ConfigFile.SaveSettings();

            timer = new System.Timers.Timer(TimerDelay);
            timer.Enabled = true;
            timer.Elapsed += timer_Elapsed;

            try
            {
                if (NetUseArgs != null && NetUseArgs != "")
                {
                    Program.Log.Info(NetUseArgs);
                    System.Diagnostics.Process.Start("net.exe", NetUseArgs);
                }

                timer.Start();
            }
            catch (Exception e) {
                Program.Log.Error(e.Message);
                Program.Log.Error(e.StackTrace);
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Program.Log.Info("timer_Elapsed...");
            if (!IsRunning)
            {
                IsRunning = true;

                try {
                    ParseNewReports(IMSPath);
                }
                catch (Exception ez)
                {
                    Program.Log.Error(ez.Message);
                    Program.Log.Error(ez.StackTrace);
                }

                IsRunning = false;
            }
            Program.Log.Info("...timer_Elapsed");
        }

        private void ParseNewReports(string IMS7)
        {
            Program.Log.Info("ParseNewReports...");
            // estrai dall'history l'ultimo 

            HistoryDBManager historydb = new HistoryDBManager();
            historydb.DBQ = GetHistoryDBPath();
            Program.Log.Info("historydb: " + historydb.DBQ);

            DateTime lastDate = historydb.GetLastDate();
            Program.Log.Info("Ultima data " + lastDate.ToString("HH:mm:ss"));

            if (lastDate > DateTime.MinValue)
            {
                int index = historydb.GetLastIndex(lastDate);
                //estrai cicli

                if (index > 0)
                {
                    string[] files = Directory.GetFiles(IMS7, "*.txt");
                    List<string> fileToParse = new List<string>();

                    Program.Log.Info("Trovati " + files.Length + " files totali");

                    foreach (string f in files)
                    {
                        string fName = System.IO.Path.GetFileName(f);

                        // Sandro 31/01/2014 // a volte i file non arrivano in ordine di numero e quindi qualche ciclo finiva per essere perso... //
                        // if (FromFilenameToDate(fName) >= lastDate && FromFilenameToIndex(fName) >= index && !historydb.IsFilePresent(fName))
                        if (FromFilenameToDate(fName) >= lastDate && !historydb.IsFilePresent(fName))
                            fileToParse.Add(f);
                    }

                    Program.Log.Info("Trovati " + fileToParse.Count + " files da processare");

                    foreach (string file in fileToParse)
                    {
                        Program.Log.Info("Processo il file " + file);

                        Cycle cycle = IMS7Object.GetCycleFromFile(file);

                        if (cycle != null)
                        {
                            Program.Log.Info(cycle.Dump());

                            if (cycle.ScopeID == "" || cycle.ScopeID.Trim() == "0")
                                cycle.ScopeID = "UNKNOWN";

                            string deviceTag = AMRFIDMGREXExtension.getDeviceTagFromSerial("" + cycle.ScopeID, ConnectionDBString);
                            if (deviceTag == "")
                                deviceTag = AMRFIDMGREXExtension.getDeviceTagFromSerial("UNKNOWN", ConnectionDBString);

                            string nome = "";
                            string cognome = "";
                            try
                            {
                                string[] splitChars = { " " };
                                string[] splitted = cycle.OperatorID.Trim().Split(splitChars, StringSplitOptions.RemoveEmptyEntries);

                                if (splitted.Length >= 2)
                                {
                                    if (splitted.Length == 3)
                                    {
                                        cognome = splitted[0] + " " + splitted[1];
                                        nome = splitted[2];
                                    }
                                    else
                                    {
                                        cognome = splitted[0];
                                        nome = splitted[1];
                                    }
                                }
                                else
                                {
                                    if (splitted.Length == 1)
                                    {
                                        nome = splitted[0].Trim();
                                        cognome = splitted[0].Trim();

                                        if (nome.Trim() == "0" || nome.Trim().ToUpper() == "OPERATORE")
                                        {
                                            nome = "";
                                            cognome = "";
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                nome = "";
                                cognome = "";
                            }
                            if (nome == "" && cognome == "")
                            {
                                nome = "UNKNOWN";
                                cognome = "UNKNOWN";
                            }

                            string operatorTag = AMRFIDMGREXExtension.getOperatorTagFromNomeCognome(nome, cognome, ConnectionDBString);
                            if (operatorTag == "")
                                operatorTag = AMRFIDMGREXExtension.getOperatorTagFromNomeCognome("UNKNOWN", "UNKNOWN", ConnectionDBString);

                            if (cycle.MachineID == "" || cycle.MachineID.Trim() == "0")
                                cycle.MachineID = "UNKNOWN";

                            int machine = amrfidmgrex.DBUtilities.getMachineIdFromMat("" + cycle.MachineID, ConnectionDBString);
                            if (operatorTag != null && operatorTag != "" & deviceTag != null & deviceTag != "" && machine > 0)
                            {
                                if (DBUtilities.insertnewCycleWithDate(deviceTag, operatorTag, machine, 3, 2, 0, cycle.StartTimestamp, ConnectionDBString))
                                {
                                    string fName = System.IO.Path.GetFileName(file);
                                    int HistoryDate = FromFilenameToIndex(fName);
                                    DateTime cycleDate = FromFilenameToDate(fName);

                                    historydb.AddCycle(HistoryDate, fName, cycleDate);

                                    AMRFIDMGREXExtension.insertNewCycleMedivators(deviceTag, operatorTag, machine, cycle.Failed ? 99999 : 1, 3, 0, cycle.EndTimestamp, cycle, ConnectionDBString);

                                    historydb.CloseCycle(HistoryDate, cycleDate);
                                }
                            }
                        }
                    }
                }
            }
            Program.Log.Info("...ParseNewReports");
        }

        private int FromFilenameToIndex(string fName)
        {
            int ret = -1;

            try
            {
                string num = fName.Substring(fName.IndexOf("-")+1, 11);
                ret = int.Parse(num);
            }
            catch
            {
                ret = -1;
            }

            return ret;
        }

        private DateTime FromFilenameToDate(string fName)
        {
            DateTime ret = DateTime.MinValue;

            try
            {
                if (fName.IndexOf("-") == 8)
                {
                    string num = fName.Substring(0, 8);
                    ret = DateTime.ParseExact(num, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                ret = DateTime.MinValue;
            }

            return ret;
        }

        private string GetHistoryDBPath()
        {
            var finfo = new FileInfo(Assembly.GetExecutingAssembly().FullName);
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            return dir + @"\CyclesHistory.mdb";
        }
    }
}
