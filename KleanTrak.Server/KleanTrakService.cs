using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Commons;
using KleanTrack.License;
using KleanTrak.Core;
using LicenseManager.Model;

namespace KleanTrak.Server
{
    partial class KleanTrakService : ServiceBase
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        WebServiceHost _host = null;
        protected static string sConnString = "";
        protected static int localPort = 6666;
        public static string HttpEndpoint = "";

        [DllImport("Kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("Kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        string DeviceStateNotificationEnpoint;

        ManualResetEvent _terminateThreadEvent = new ManualResetEvent(false);
        Thread _threadWcf;
        private string _ct_license_id = "ct_server";

        protected KleanTrakService()
        {
        }

        public static void Install(string pathCfg, bool bVerbose)
        {
            KleanTrakService service = new KleanTrakService();
            if (!service.ReadSettings(pathCfg, false, true))
                return;

            (new ServiceManagement()).Install(service.ServiceName, false, "", "");

            try
            {
                (new ServiceControlManager()).SetRestartOnFailure(service.ServiceName);
            }
            catch (Exception /* ex */)
            {
            }

            if (bVerbose)
            {
                Console.WriteLine("\nPress a key to close...");
                Console.ReadKey();
            }
        }

        public static void Uninstall(string pathCfg, bool bVerbose)
        {
            KleanTrakService service = new KleanTrakService();
            service.ReadSettings(pathCfg, false, false);

            if (!bVerbose)
            {
                try
                {
                    ServiceController serviceController = new ServiceController(service.ServiceName);

                    TimeSpan timeout = TimeSpan.FromMilliseconds(10000);
                    serviceController.Stop();
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                }
                catch (Exception)
                {
                }
            }

            (new ServiceManagement()).Uninstall(service.ServiceName);

            if (bVerbose)
            {
                Console.WriteLine("\nPress a key to close...");
                Console.ReadKey();
            }
        }

        public static void Debug(string pathCfg)
        {
            KleanTrakService service = new KleanTrakService();
            service.ReadSettings(pathCfg, true, false);
            service.Debug();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathCfg"></param>
        public static void Run(string pathCfg)
        {
            KleanTrakService service = new KleanTrakService();
            if (!service.ReadSettings(pathCfg, false, false))
                return;

            ServiceBase.Run(service);
        }

        protected bool ReadSettings(string pathCfg, bool debug, bool install)
        {
            Logger.Info(pathCfg);

            // prendo la directory dove risiede il servizio per creare il nome del config file //
            var uri = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            var sAppDir = new FileInfo(uri.LocalPath).DirectoryName;

            string sCfgFile = sAppDir + "\\" + pathCfg;

            // prendo dal cfg il nome del servizio //            
            StringBuilder strRetVal0 = new StringBuilder(255);
            GetPrivateProfileString("Settings", "ServiceName", "", strRetVal0, 255, sCfgFile);
            ServiceName = strRetVal0.ToString();

            //Se siamo in installazione cerco un nome valido
            if (install)
            {
                if (ServiceName == null || ServiceName.Length == 0)
                    ServiceName = "Cleantrack";
            }
            WritePrivateProfileString("Settings", "ServiceName", ServiceName.ToString(), sCfgFile);

            strRetVal0 = new StringBuilder(255);
            GetPrivateProfileString("Settings", "LogInfos", "", strRetVal0, 255, sCfgFile);
            string logInfos = strRetVal0.ToString();
            WritePrivateProfileString("Settings", "LogInfos", logInfos, sCfgFile);

            if (!debug && (ServiceName == null || ServiceName == ""))
            {
                Logger.Error(Dictionary.Instance["errorGettingServiceName"]);
                Console.ReadKey();
                return false;
            }

            //Controllo che il servizio sia installato
            if (!debug && !install)
            {
                if (!Exists(ServiceName))
                {
                    Logger.Error(Dictionary.Instance["serviceNotInstalled"]);
                    Console.ReadKey();
                    return false;
                }
            }
            else if (install)
            {
                //Se siamo in installazione controllo che il servizio non esista già
                if (Exists(ServiceName))
                {
                    Logger.Error(Dictionary.Instance["serviceAlreadyInstalled"]);
                    Console.ReadKey();
                    return false;
                }
            }

            //Recupero la stringa di connessione al db            
            strRetVal0 = new StringBuilder(255);
            GetPrivateProfileString("Settings", "DbConnStr", "", strRetVal0, 255, sCfgFile);
            //se non trovo la scritta DRIVER, vuol dire che è un base64 e lo decodifico
            if (strRetVal0.ToString().ToUpper().Contains("DRIVER="))
                sConnString = strRetVal0.ToString();
            else
                sConnString = Crypto.Decrypt(strRetVal0.ToString());

            if (debug)
                Logger.Info("dbConn:" + sConnString);

            //codifico in base64 e salvo            
            WritePrivateProfileString("Settings", "DbConnStr", Crypto.Encrypt(sConnString), sCfgFile);

            if ((sConnString == null || sConnString == "") && !install)
            {
                Logger.Error(Dictionary.Instance["setDbConnectionString"]);
                Console.ReadKey();
                return false;
            }

            DbConnection.ConnectionString = sConnString;

            strRetVal0 = new StringBuilder(255);
            GetPrivateProfileString("Settings", "DeviceStateNotificationEnpoint", "", strRetVal0, 255, sCfgFile);
            DeviceStateNotificationEnpoint = strRetVal0.ToString();
            WritePrivateProfileString("Settings", "DeviceStateNotificationEnpoint", DeviceStateNotificationEnpoint, sCfgFile);

            if (!install)
            {
                try
                {
                    DbConnection conn = new DbConnection();
                    DbRecordset dataset = conn.ExecuteReader("select valore from configurazione where chiave = 'Server Port'");
                    if (dataset.Count == 0)
                    {
                        conn.ExecuteNonQuery("insert into configurazione (chiave, valore) values ('Server Port','" + localPort.ToString() + "')");
                    }
                    else
                    {
                        string strValue = dataset[0].GetString("valore");
                        if (strValue != null && strValue.Length > 0)
                            localPort = int.Parse(strValue);
                    }

                    string hostname = Environment.MachineName;
                    dataset = conn.ExecuteReader("select valore from configurazione where chiave = 'Server Host'");
                    if (dataset.Count == 0)
                    {
                        conn.ExecuteNonQuery("insert into configurazione (chiave, valore) values ('Server Host','" + hostname + "')");
                    }
                    else
                    {
                        string strValue = dataset[0].GetString("valore");
                        if (strValue != null && strValue.Length > 0)
                        {
                            if (strValue.ToUpper() != hostname.ToUpper())
                            {
                                Logger.Info("Current host '" + hostname + "' doesn't match current machine '" + strValue + "'");
                                bool bFound = false;
                                try
                                {
                                    IPHostEntry hostCurr = Dns.GetHostEntry(hostname);
                                    foreach (IPAddress ip1 in hostCurr.AddressList)
                                        Logger.Info("Current host " + hostname + " address " + ip1.ToString());
                                    IPHostEntry hostDb = Dns.GetHostEntry(strValue);
                                    foreach (IPAddress ip2 in hostDb.AddressList)
                                        Logger.Info("Host " + strValue + " address " + ip2.ToString());
                                    foreach (IPAddress ip1 in hostCurr.AddressList)
                                    {
                                        foreach (IPAddress ip2 in hostDb.AddressList)
                                        {
                                            if (ip1.ToString() == ip2.ToString())
                                            {
                                                bFound = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                if (!bFound)
                                {
                                    Logger.Error("Current host " + hostname + " isn't compatible with the current machine!");
                                    conn.ExecuteNonQuery("update configurazione set valore = '" + hostname + "' where chiave = 'Server Host'");
                                }
                                else
                                {
                                    Logger.Info("Current host OK!!!!");
                                    hostname = strValue;
                                }
                            }
                            else
                                hostname = strValue;
                        }
                        else
                        {
                            Logger.Error("Server host config empty!!");
                            conn.ExecuteNonQuery("update configurazione set valore = '" + hostname + "' where chiave = 'Server Host'");
                        }
                    }

                    HttpEndpoint = "http://" + hostname + ":" + localPort.ToString() + "/";
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    Console.ReadKey();
                    return false;
                }
            }
            return true;
        }

        protected bool Exists(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            for (int h = 0; h < services.Length; h++)
            {
                if (services[h].ServiceName.CompareTo(serviceName) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Start();
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(e.ToString(), System.Diagnostics.EventLogEntryType.Error);
                Logger.Error(e);
            }
        }

        private bool VerifyLicense()
        {
            try
            {
                var license_status = CTLicenseManager.CheckoutLicense(_ct_license_id);
                string msg = "";
                switch (license_status)
                {
                    case LicenseCheck.LICENSE_VALID:
                        return true;
                    case LicenseCheck.FLOATING_LICENSE_OVERUSED:
                    case LicenseCheck.FLOATING_LICENSE_NOT_AVAILABLE_ALL_IN_USE:
                        msg = "###### LICENZA PRESENTE MA NESSUNA DISPONIBILE";
                        break;
                    case LicenseCheck.FLOATING_LICENSE_NOT_FOUND:
                    case LicenseCheck.FLOATING_LICENSE_SERVER_NOT_AVAILABLE:
                        msg = "###### LICENZA NON PRESENTE";
                        break;
                    case LicenseCheck.LICENSE_EXPIRED:
                        msg = "###### LICENZA SCADUTA";
                        break;
                    case LicenseCheck.LICENSE_INVALID:
                        msg = "###### LICENZA NON VALIDA";
                        break;
                    case LicenseCheck.MISMATCH_HARDWARE_ID:
                        msg = "###### LICENZA CON CHIAVE HARDWARE ERRATA";
                        break;
                    case LicenseCheck.MISMATCH_PRODUCT_ID:
                        msg = "###### LICENZA RELATIVA AD UN ALTRO PRODOTTO";
                        break;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Logger.Error(msg);
                Console.ResetColor();
                return false;
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(e.ToString(), System.Diagnostics.EventLogEntryType.Error);
                Logger.Error("", e);
                return false;
            }
        }

        private bool VerifyDbVersion()
        {
            try
            {
                var db = new DbConnection();

                var db_version = db.ExecuteScalar("SELECT VALORE FROM CONFIGURAZIONE WHERE CHIAVE = 'db_version'");
                if (db_version == null)
                    throw new ApplicationException("db_version key is missing in table configurazione!");
                if ((string)db_version != UtilityFunctions.GetShortAssemblyVersion())
                    throw new ApplicationException($"db_version is wrong! version in db: {db_version}, version in app: {UtilityFunctions.GetShortAssemblyVersion()}");
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return false;
            }
        }
        protected override void OnStop()
        {
            try
            {
                Logger.Info("1");
                End();
                Logger.Info("2");
            }
            catch (Exception e)
            {
                Logger.Error("", e);
                EventLog.WriteEntry(e.ToString(), System.Diagnostics.EventLogEntryType.Error);
                CTLicenseManager.CheckinLicense(_ct_license_id);
                throw;
            }
        }

        protected void Debug()
        {
            Start(true);
            Console.Write("Press a key to exit...\r\n\r\n");
            Console.ReadKey();
            End();
        }

        private void Start(bool debug = false)
        {
            if (!VerifyDbVersion())
                throw new ApplicationException("incorrect db version!");
            
            CTLicenseManager.Initialize(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            
            if (!debug && !VerifyLicense())
                throw new ApplicationException("license invalid!");

            _threadWcf = new Thread(ThreadWcf);
            _threadWcf.Start();

            WasherManager.Instance.Start(debug);
            ExpiringStateManager.Instance.Start(debug);
            RfidReaderManager.Instance.Start(debug);

            if (DeviceStateNotificationEnpoint != null && DeviceStateNotificationEnpoint.Length > 0)
                StateTransactions.DeviceStateChanged += StateTransactions_DeviceStateChanged;
        }

        private void End()
        {
            try
            {
                if (DeviceStateNotificationEnpoint != null && DeviceStateNotificationEnpoint.Length > 0)
                    StateTransactions.DeviceStateChanged -= StateTransactions_DeviceStateChanged;

                RfidReaderManager.Instance?.Stop();
                ExpiringStateManager.Instance?.Stop();
                WasherManager.Instance?.Stop();
                if (Logger == null)

                    Logger?.Info("1");
                _terminateThreadEvent.Set();
                Logger?.Info("2");
                if (_threadWcf != null)
                    _threadWcf.Join();
                else
                    Logger.Info("thread wcf is null");
                Logger?.Info("3");
            }
            catch (Exception e)
            {
                Logger?.Error(e);
            }
        }

        protected void ThreadWcf()
        {
            Uri baseHttpAddress = new Uri(HttpEndpoint);
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.HttpGetUrl = baseHttpAddress;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            WebHttpBinding webHttpBinding = new System.ServiceModel.WebHttpBinding();
            webHttpBinding.Security.Mode = WebHttpSecurityMode.TransportCredentialOnly;
            webHttpBinding.MaxReceivedMessageSize = 65536000;
            if (_host == null)
                _host = new WebServiceHost(typeof(Service), baseHttpAddress);
            _host.Description.Behaviors.Add(smb);
            ServiceEndpoint ep = _host.AddServiceEndpoint(typeof(IService), webHttpBinding, baseHttpAddress);
            ServiceDebugBehavior sdb = _host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.HttpHelpPageEnabled = false;
            try
            {
                _host.Open();
                Logger.Info("Endpoint started: " + HttpEndpoint);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                _host = null;
            }
            (new Service()).SendPiConfiguration();
            _terminateThreadEvent.WaitOne();
            if (_host != null)
                _host.Close();
        }

        private string Serialize(object obj)
        {
            XmlSerializer s = new XmlSerializer(obj.GetType());
            StringWriter sw = new StringWriter();
            s.Serialize(sw, obj);
            sw.Close();
            sw.Dispose();
            return sw.ToString();
        }

        private void StateTransactions_DeviceStateChanged(DeviceStateChangeData obj)
        {
            Logger.Info("start");

            byte[] request = System.Text.Encoding.GetEncoding(1252).GetBytes(Serialize(obj));

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(DeviceStateNotificationEnpoint);
            req.Proxy = new WebProxy();
            req.Timeout = 30000;
            req.Method = "POST";
            req.ContentLength = request.Length;

            StreamReader readStream = null;
            Stream reqStream = null, receiveStream = null;
            string response = "";

            try
            {
                reqStream = req.GetRequestStream();
                reqStream.Write(request, 0, request.Length);
                reqStream.Flush();
                reqStream.Close();
                reqStream.Dispose();

                //Gestione risposta
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                receiveStream = res.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                readStream = new StreamReader(receiveStream, System.Text.Encoding.UTF8);
                response = readStream.ReadToEnd();
                readStream.Close();
                readStream.Dispose();

                receiveStream.Close();
                receiveStream.Dispose();
            }
            catch (System.Net.WebException ex)
            {
                Logger.Error(ex);
            }
            finally
            {
                if (reqStream != null)
                {
                    reqStream.Flush();
                    reqStream.Close();
                    reqStream.Dispose();
                }

                if (readStream != null)
                {
                    readStream.Close();
                    readStream.Dispose();
                }

                if (receiveStream != null)
                {
                    receiveStream.Close();
                    receiveStream.Dispose();
                }
            }

            Logger.Info("end");
        }
    }
}