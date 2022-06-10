using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Timers;
using System.IO;
using System.Reflection;
using ImageSI.Configuration;
using LibLog;

namespace WindowsService1
{
    public partial class CleanerSynchro : ServiceBase
    {
        private int m_iLastCheckedFile = 0;
        private List<String> m_listFileToParse = new List<String>();
        private Timer timer = new Timer(10000);
        private bool running = false;

        public String ConnectionString { get; set; }

        public String CycleDone { get; set; }

        public String PathCleanerData { get; set; }

        public String GetConfigFilePath
        {
            get
            {
                string curPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\config.ini";

                if (!System.IO.File.Exists(curPath))
                    System.IO.File.Create(curPath);

                return curPath;
            }
        }

        public String IdSterilizzatrice { get; set; }
        
        public String NewState { get; set; }

        public String DatePrefix { get; set; }

        public void manageConfigDataNew()
        {
            string configPath = GetConfigFilePath;
            var ConfigFile = new ConfigurationFile(configPath, true);
            ConfigFile.LoadSettings();
            PathCleanerData = ConfigFile.Get("Cleaner_Directory", "C:\\Temp\\");
            IdSterilizzatrice = ConfigFile.Get("IdSterilizzatrice", "1");
            NewState = ConfigFile.Get("IdStatoPulito", "1");
            CycleDone = ConfigFile.Get("StatoCorretto", "1");
            DatePrefix = ConfigFile.Get("PrefissoData", "20");
            ConnectionString = ConfigFile.Get("ConnectionString", "DRIVER={SQL Server};UID=sa;PWD=nautilus;SERVER=vmp-srv05;DATABASE=CleanTrack");
            ConfigFile.SaveSettings();
        }

        public CleanerSynchro()
        {
            InitializeComponent();
            Logger.Get().ActivateFileDestination(new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory.Name, DateTime.Now.ToString("yyyyMMdd") + ".log", true, true);
            Logger.Get().Write("internal",
                                     "Cleantrack.Service",
                                     "Starting Service",
                                     null,
                                     Logger.LogLevel.Info);

            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!running)
            {
                running = true;
                Job();
                running = false;
            }
        }

        private void Job()
        {
            WriteLog("");
            WriteLog("INIZIO");

            string sConfigPath = GetConfigFilePath;
            ConfigurationFile cfgFile = new ConfigurationFile(sConfigPath, true);

            cfgFile.LoadSettings();

            m_iLastCheckedFile = Convert.ToInt32(cfgFile.Get("Ultimo_File", "000000"));

            if (checkDirectory())
            {
                try
                {
                    foreach (var item in m_listFileToParse)
                    {
                        InfoCleaner ic = new InfoCleaner(CycleDone, DatePrefix);

                        if (ic.loadInfos(item, IdSterilizzatrice, NewState, ConnectionString))
                        {
                            m_iLastCheckedFile = GetCycleNumberEx(item);
                            WriteLog("Updating settings with: lastfile:" + m_iLastCheckedFile.ToString());
                            cfgFile.Set("Ultimo_File", m_iLastCheckedFile.ToString(), false);
                            cfgFile.SaveSettings();
                        }
                        else
                        {
                            WriteLog("Errore caricando i dati da:" + item);
                        }


                    }
                }
                catch (Exception exc)
                {
                    WriteLog(exc.Message);
                    WriteLog(exc.StackTrace);
                }
            }

            if (m_listFileToParse.Count > 0)
                m_listFileToParse.RemoveRange(0, m_listFileToParse.Count);
            
            WriteLog("FINE");
            WriteLog("");
        }

        public void startTest()
        {
            string[] args = new string[0];
            OnStart(args);
        }

        public void stopTest()
        {
            OnStop();
        }

        public void DebugService()
        {
            manageConfigDataNew();
            timer.Start();
            Console.ReadLine();
            timer.Stop();
        }

        protected override void OnStart(string[] args)
        {
            manageConfigDataNew();
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.Stop();
        }

        private bool checkDirectory()
        {
            bool bReturn = false;

            WriteLog("Searching files over: " + m_iLastCheckedFile.ToString());

            try
            {
                String[] sFileArray;
                sFileArray = System.IO.Directory.GetFiles(PathCleanerData, IdSterilizzatrice + "*.txt" );

                if (sFileArray.Length > 0)
                {
                    Array.Sort(sFileArray);

                    foreach (var item in sFileArray)
                    {
                        if (item.ToUpper().EndsWith(".TXT"))
                        {
                            // !!!
                            // int iTemp = GetCycleNumber(item);
                            int iTemp = GetCycleNumberEx(item);

                            if (iTemp > m_iLastCheckedFile)
                            {
                                m_listFileToParse.Add(item);
                                WriteLog("Added file: " + item);
                                bReturn = true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                WriteLog("- checkDirectory --> " + e.Message);
                WriteLog(e.StackTrace);
            }

            return bReturn;
        }

        private int GetCycleNumberEx(string path)
        {
            int iInd1 = path.LastIndexOf('.');
            int iInd2 = path.IndexOf(this.IdSterilizzatrice) + this.IdSterilizzatrice.Length;

            if ((iInd1 > 0) && (iInd2 > 0))
            {
                string sTemp = path.Substring(iInd2 + 1, iInd1 - (iInd2 + 1));

                int iTemp;
                if (Int32.TryParse(sTemp, out iTemp))
                {
                    WriteLog("- cycleNumberEx result:" + iTemp);
                    return iTemp;
                }

                WriteLog("- cycleNumberEx error parsing");
                return -1;
            }

            WriteLog("- cycleNumberEx error getting indexof");

            return -1;
        }


        private int GetCycleNumber(string path)
        {
            int iInd1 = path.LastIndexOf('.');
            int iInd2 = path.LastIndexOf('_', iInd1 - 1);

            if ((iInd1 > 0) && (iInd2 > 0))
            {
                string sTemp = path.Substring(iInd2 + 1, iInd1 - (iInd2 + 1));

                int iTemp;
                if (Int32.TryParse(sTemp, out iTemp))
                    return iTemp;

            }

            return -1;       
        }

        private string GetHistoryDBPath()
        {
            var finfo = new FileInfo(Assembly.GetExecutingAssembly().FullName);
            string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            return dir + @"\CyclesHistory.mdb";
        }

        private bool GoEx()
        {
            bool result = false;
            HistoryDBManager historydb = new HistoryDBManager();
            historydb.DBQ = GetHistoryDBPath();
            DateTime dateStart = DateTime.MinValue;
            dateStart = historydb.GetLastDate();
            result = true;
            return result;
        }

        public static void WriteLog(string sText)
        {
            try
            {
                Logger.Get().Write("internal", "Steelco Parser", sText, null, Logger.LogLevel.Info);
                Console.WriteLine(sText);
            }
            catch
            {
            }
        }
    }
}
