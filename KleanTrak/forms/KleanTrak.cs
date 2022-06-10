using Commons;
using ExtModules;
using KleanTrak.Core;
using LibLog;
using ListViewEx;
using OdbcExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace KleanTrak
{
    public class MainForm : System.Windows.Forms.Form
    {
        public delegate void ChangeSeedHandler(int idseed);
        public event ChangeSeedHandler ChangeSeed;
        private ColumnHeader columnEmpty;
        private ColumnHeader columnMatricola;
        private ColumnHeader columnDispositivo;
        private ColumnHeader columnStato;
        private ColumnHeader columnEsami;
        private IContainer components;
        private ImageList imageList;
        private MainMenu mainMenu;
        private MenuItem mi_users;
        private MenuItem mi_actions;
        private MenuItem mi_logout;
        private MenuItem mi_change_pwd;
        private MenuItem mi_change_pin;
        private MenuItem mi_users_management;
        private MenuItem mi_info;
        private MenuItem mi_system_params;
        private MenuItem menuItem12;
        private MenuItem mi_dismiss_causes;
        private MenuItem menuItem14;
        private MenuItem mi_setup_dev_types;
        private MenuItem mi_suppliers;
        private MenuItem mi_exit;
        private MenuItem mi_setup;
        private MenuItem mi_setup_db_conn;
        private MenuItem mi_query;
        private Timer timer;
        private MenuItem mi_lists;
        private MenuItem mi_devices;
        private MenuItem mi_operators;
        private MenuItem mi_washers;
        private MenuItem mi_rf_readers;
        private MenuItem mi_view_log;
        private MenuItem mi_refresh_devs_state;
        private MenuItem mi_change_dev_state;
        private MenuItem menuIInfo;
        private MenuItem mi_modify_user_rights;
        private MenuItem mi_print_receipt;
        private ListViewEx.ListViewEx listView;
        private string CodeRequested = "";
        private MenuItem menuItem1;
        private MenuItem mi_dev_states;
        private MenuItem mi_state_transactions;
        private MenuItem mi_dev_expiration;
        private FlowLayoutPanel panelButtons;
        private MenuItem mi_receipts_view;
        private MenuItem mi_change_seed;
        private MenuItem mier214comtest;
        private MenuItem mi_file_licenza;
        private MenuItem mi_sedi_uo;
        private string CurrentLastDate = "";
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            string strInitFile = Application.StartupPath + "\\kleantrak.ini";
            string strDriverDictionary = ExtModules.InteropKernel32.GetPrivateProfileString("ConnessioneDictionary", "Driver", "Microsoft Access Driver (*.mdb)", strInitFile);
            ExtModules.InteropKernel32.WritePrivateProfileString("ConnessioneDictionary", "Driver", strDriverDictionary, strInitFile);
            Logger.Get().ActivateFileDestination(new FileInfo(new Uri(System.Reflection.Assembly.GetExecutingAssembly().Location).LocalPath).DirectoryName + "\\log" , DateTime.Now.ToString("yyyyMMdd") + "_liblog.log", false, false);
            KleanTrak.Globals.strDictionary = "DRIVER={" + strDriverDictionary + "};UID=administrator;PWD=nautilus;DBQ=" + Application.StartupPath + "\\Dictionary_KleanTrak.mdb";
            try
            {
                using (OdbcConnection connTemp = new OdbcConnection(KleanTrak.Globals.strDictionary))
                {
                    connTemp.Open();
                    OdbcCommand commTemp = new OdbcCommand("SELECT * FROM Dictionary;", connTemp);
                    OdbcDataReader readerTemp = commTemp.ExecuteReader();
                    KleanTrak.Globals.nColonnaLingua[(int)KleanTrak.Globals.LINGUA.ITA] = readerTemp.GetOrdinal("ITA");
                    KleanTrak.Globals.nColonnaLingua[(int)KleanTrak.Globals.LINGUA.ENG] = readerTemp.GetOrdinal("ENG");
                    KleanTrak.Globals.nColonnaLingua[(int)KleanTrak.Globals.LINGUA.ESP] = readerTemp.GetOrdinal("ESP");
                    KleanTrak.Globals.nColonnaLingua[(int)KleanTrak.Globals.LINGUA.POR] = readerTemp.GetOrdinal("POR");
                    if (readerTemp != null)
                        readerTemp.Close();
                    if (connTemp != null)
                        connTemp.Close();
                }
            }
            catch (OdbcException ex)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[8].ToUpper() + "\n" + ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Logger.Get().Stop();
                return;
            }
            try
            {
                // Sandro 24/07/2014 // carico le stringhe dal dictionary //
                KleanTrak.Globals.CaricaStringTable();

                //string strInitFile = Application.StartupPath + "\\kleantrak.ini";
                string strDriver = ExtModules.InteropKernel32.GetPrivateProfileString("ConnessioneDB", "Driver", "", strInitFile);
                ExtModules.InteropKernel32.WritePrivateProfileString("ConnessioneDB", "Driver", strDriver, strDriver);

                string strUserName = ExtModules.InteropKernel32.GetPrivateProfileString("ConnessioneDB", "UserName", "", strInitFile);
                string clearUserName = Core.Crypto.TryDecrypt(strUserName, out string usr) ? usr : strUserName; // if TryDecrypt fails, the original string is already clear
                ExtModules.InteropKernel32.WritePrivateProfileString("ConnessioneDB", "UserName", Core.Crypto.Encrypt(clearUserName), strInitFile);

                string strPassword = ExtModules.InteropKernel32.GetPrivateProfileString("ConnessioneDB", "Password", "", strInitFile);
                string clearPassword = Core.Crypto.TryDecrypt(strPassword, out string pwd) ? pwd : strPassword;
                ExtModules.InteropKernel32.WritePrivateProfileString("ConnessioneDB", "Password", Core.Crypto.Encrypt(clearPassword), strInitFile);

                string strServer = ExtModules.InteropKernel32.GetPrivateProfileString("ConnessioneDB", "Server", "", strInitFile);
                ExtModules.InteropKernel32.WritePrivateProfileString("ConnessioneDB", "Server", strServer, strInitFile);

                string strDB = ExtModules.InteropKernel32.GetPrivateProfileString("ConnessioneDB", "Database", "", strInitFile);
                ExtModules.InteropKernel32.WritePrivateProfileString("ConnessioneDB", "Database", strDB, strInitFile);

                KleanTrak.Globals.strDatabase = "DRIVER={" + strDriver + "};UID=" + clearUserName + ";PWD=" + clearPassword + ";" + strServer + ";DATABASE=" + strDB;
                KleanTrak.Globals.bBadgeActive = ExtModules.InteropKernel32.GetPrivateProfileInt("LettoreBadge", "Enable", 0, strInitFile) > 0 ? true : false;
                ExtModules.InteropKernel32.WritePrivateProfileString("LettoreBadge", "Enable", KleanTrak.Globals.bBadgeActive.ToString(), strInitFile);
                KleanTrak.Globals.ReadOnly = ExtModules.InteropKernel32.GetPrivateProfileInt("LettoreBadge", "ReadOnly", 0, strInitFile) > 0 ? true : false;
                ExtModules.InteropKernel32.WritePrivateProfileString("LettoreBadge", "ReadOnly", KleanTrak.Globals.ReadOnly.ToString(), strInitFile);
                KleanTrak.Globals.strBadgeAddressIP = ExtModules.InteropKernel32.GetPrivateProfileString("LettoreBadge", "Address", "", strInitFile);
                ExtModules.InteropKernel32.WritePrivateProfileString("LettoreBadge", "Address", KleanTrak.Globals.strBadgeAddressIP.ToString(), strInitFile);
                KleanTrak.Globals.iBadgeAddressPort = ExtModules.InteropKernel32.GetPrivateProfileInt("LettoreBadge", "Port", 8090, strInitFile);
                ExtModules.InteropKernel32.WritePrivateProfileString("LettoreBadge", "Port", KleanTrak.Globals.iBadgeAddressPort.ToString(), strInitFile);
                KleanTrak.Globals.iBadgeTimeout = ExtModules.InteropKernel32.GetPrivateProfileInt("LettoreBadge", "Timeout", 5, strInitFile);
                ExtModules.InteropKernel32.WritePrivateProfileString("LettoreBadge", "Timeout", KleanTrak.Globals.iBadgeTimeout.ToString(), strInitFile);
                KleanTrak.Globals.Refresh = (int)ExtModules.InteropKernel32.GetPrivateProfileInt("Gestione_Stati", "Refresh_Interfaccia", 30, strInitFile);
                ExtModules.InteropKernel32.WritePrivateProfileString("Gestione_Stati", "Refresh_Interfaccia", KleanTrak.Globals.Refresh.ToString(), strInitFile);
                KleanTrak.Globals.nLinguaInUso = (int)ExtModules.InteropKernel32.GetPrivateProfileInt("Localizzazione", "lingua", 0, strInitFile);
                ExtModules.InteropKernel32.WritePrivateProfileString("Localizzazione", "lingua", KleanTrak.Globals.nLinguaInUso.ToString(), strInitFile);
                KleanTrak.Globals.ConfirmTimer = (int)ExtModules.InteropKernel32.GetPrivateProfileInt("Generale", "TimerConferma", 3, strInitFile);
                ExtModules.InteropKernel32.WritePrivateProfileString("Generale", "TimerConferma", KleanTrak.Globals.ConfirmTimer.ToString(), strInitFile);
                KleanTrak.Globals.IDSEDE = (int)ExtModules.InteropKernel32.GetPrivateProfileInt("Generale", "IDSEDE", 0, strInitFile);
                ExtModules.InteropKernel32.WritePrivateProfileString("Generale", "IDSEDE", KleanTrak.Globals.IDSEDE.ToString(), strInitFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[123] + ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // tento una connessione al DB principale //
            try
            {
                using (OdbcConnection connTemp = new OdbcConnection(KleanTrak.Globals.strDatabase))
                {
                    connTemp.Open();
                    connTemp.Close();
                }
            }
            catch (OdbcException ex)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[6] + ex.Message + KleanTrak.Globals.strTable[7], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Database dlg = new Database();
                dlg.ShowDialog();

                Logger.Get().Stop();
                return;
            }
            if (!VerifyDatabase())
            {
                MessageBox.Show(KleanTrak.Globals.strTable[137],
                    "Clean Track",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
#if DEBUG
#else
				Logger.Get().Stop();
                return;
#endif
            }
            amrfidmgrex.DBObject.ODBCConnectionString = Globals.strDatabase;
            DbConnection.ConnectionString = Globals.strDatabase;
            StateList.Instance.Init(Globals.strDatabase);
            Globals.DESCR_SEDE = DBUtil.GetDescrSede(Globals.IDSEDE);
            try
            {
                InteropAmLogin.AmLogin_EnableSelfLocalization("Kleantrak", KleanTrak.Globals.nLinguaInUso);
            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[124] + ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
                InteropAmLogin.AmLogin_DisableSelfLocalization();
                Logger.Get().Stop();
                return;
            }
            try
            {
                InteropAmLogin.AmLogin_SetAvailableRights(65536);
                InteropAmLogin.AmLogin_SetPrivilegeDescription(1, "Configurazione");

                if (System.IO.File.Exists(new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + @"\enablesu"))
                {
                    Globals.su_user_mode = true;
                    Globals.m_strUser = "SAT";
                    Globals.m_iUserID = 2;
                    Globals.m_iUserPermission = 65536;
                    Globals.nLinguaInUso = (int)Globals.LINGUA.ITA;
                }
                else if (InteropAmLogin.AmLogin_Login_OLD(Globals.m_strUser, Globals.GetLocalIPAddress(), "Cleantrack", "Tesi Imaging - Cleantrack", KleanTrak.Globals.strDatabase))
                {
                    InteropAmLogin.AmLogin_GetUsername(ref Globals.m_strUser);
                    InteropAmLogin.AmLogin_GetUserID(ref Globals.m_iUserID);
                    InteropAmLogin.AmLogin_GetUserPermission(ref Globals.m_iUserPermission);
                }
                else
                {
                    InteropAmLogin.AmLogin_DisableSelfLocalization();
                    Logger.Get().Stop();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(Globals.strTable[125] + ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Application.DoEvents();
            Application.Run(new MainForm());
        }
        private static bool VerifyDatabase()
        {
            OdbcConnection conn = null;
            OdbcCommand cmd = null;
            try
            {
                conn = new OdbcConnection(Globals.strDatabase);
                conn.Open();
                cmd = new OdbcCommand("SELECT VALORE FROM CONFIGURAZIONE WHERE CHIAVE = 'db_version'", conn);
                var db_version = cmd.ExecuteScalar();
                if (db_version == null)
                    throw new ApplicationException("db_version key is missing in table configurazione!");
                if ((string)db_version != UtilityFunctions.GetShortAssemblyVersion())
                    throw new ApplicationException("db_version is wrong!");
                return true;
            }
            catch (Exception e)
            {
                Logger.Get().Write("", "Kleantrak.VerifyDatabase", e.ToString(), null, Logger.LogLevel.Error);
                return false;
            }
        }
        // non toccare mai il contenuto di InitializeComponent //
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.mi_users = new System.Windows.Forms.MenuItem();
            this.mi_logout = new System.Windows.Forms.MenuItem();
            this.mi_change_pwd = new System.Windows.Forms.MenuItem();
            this.mi_change_pin = new System.Windows.Forms.MenuItem();
            this.mi_modify_user_rights = new System.Windows.Forms.MenuItem();
            this.mi_users_management = new System.Windows.Forms.MenuItem();
            this.mi_view_log = new System.Windows.Forms.MenuItem();
            this.mi_exit = new System.Windows.Forms.MenuItem();
            this.mi_actions = new System.Windows.Forms.MenuItem();
            this.mi_refresh_devs_state = new System.Windows.Forms.MenuItem();
            this.mi_change_dev_state = new System.Windows.Forms.MenuItem();
            this.mi_print_receipt = new System.Windows.Forms.MenuItem();
            this.mi_change_seed = new System.Windows.Forms.MenuItem();
            this.mi_lists = new System.Windows.Forms.MenuItem();
            this.mi_devices = new System.Windows.Forms.MenuItem();
            this.mi_operators = new System.Windows.Forms.MenuItem();
            this.mi_washers = new System.Windows.Forms.MenuItem();
            this.mi_rf_readers = new System.Windows.Forms.MenuItem();
            this.mi_receipts_view = new System.Windows.Forms.MenuItem();
            this.mi_setup = new System.Windows.Forms.MenuItem();
            this.mi_system_params = new System.Windows.Forms.MenuItem();
            this.mi_setup_db_conn = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.mi_setup_dev_types = new System.Windows.Forms.MenuItem();
            this.mi_dismiss_causes = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.mi_suppliers = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mi_dev_states = new System.Windows.Forms.MenuItem();
            this.mi_state_transactions = new System.Windows.Forms.MenuItem();
            this.mi_dev_expiration = new System.Windows.Forms.MenuItem();
            this.mier214comtest = new System.Windows.Forms.MenuItem();
            this.mi_file_licenza = new System.Windows.Forms.MenuItem();
            this.mi_sedi_uo = new System.Windows.Forms.MenuItem();
            this.mi_query = new System.Windows.Forms.MenuItem();
            this.menuIInfo = new System.Windows.Forms.MenuItem();
            this.mi_info = new System.Windows.Forms.MenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.listView = new ListViewEx.ListViewEx();
            this.columnEmpty = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnMatricola = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDispositivo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnStato = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnEsami = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mi_users,
            this.mi_actions,
            this.mi_lists,
            this.mi_setup,
            this.mi_query,
            this.menuIInfo});
            // 
            // mi_users
            // 
            this.mi_users.Index = 0;
            this.mi_users.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mi_logout,
            this.mi_change_pwd,
            this.mi_change_pin,
            this.mi_modify_user_rights,
            this.mi_users_management,
            this.mi_view_log,
            this.mi_exit});
            this.mi_users.Text = "Utenti";
            this.mi_users.Click += new System.EventHandler(this.menuFile_Click);
            // 
            // mi_logout
            // 
            this.mi_logout.Index = 0;
            this.mi_logout.Text = "Disconnetti...";
            this.mi_logout.Click += new System.EventHandler(this.menuDisconnetti_Click);
            // 
            // mi_change_pwd
            // 
            this.mi_change_pwd.Index = 1;
            this.mi_change_pwd.Text = "Cambio password utente...";
            this.mi_change_pwd.Click += new System.EventHandler(this.menuCambioPassword_Click);
            // 
            // mi_change_pin
            // 
            this.mi_change_pin.Index = 2;
            this.mi_change_pin.Text = "Cambio PIN operatore";
            this.mi_change_pin.Click += new System.EventHandler(this.menuCambioPinClick);
            // 
            // mi_modify_user_rights
            // 
            this.mi_modify_user_rights.Index = 3;
            this.mi_modify_user_rights.Text = "Mostra diritti utente";
            this.mi_modify_user_rights.Click += new System.EventHandler(this.menuDirittiUtente_Click);
            // 
            // mi_users_management
            // 
            this.mi_users_management.Index = 4;
            this.mi_users_management.Text = "Gestione utenti...";
            this.mi_users_management.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // mi_view_log
            // 
            this.mi_view_log.Index = 5;
            this.mi_view_log.Text = "Visualizza log operazioni...";
            this.mi_view_log.Click += new System.EventHandler(this.menuItem1_Click_1);
            // 
            // mi_exit
            // 
            this.mi_exit.Index = 6;
            this.mi_exit.Text = "&Esci";
            this.mi_exit.Click += new System.EventHandler(this.menuEsci_Click);
            // 
            // mi_actions
            // 
            this.mi_actions.Index = 1;
            this.mi_actions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mi_refresh_devs_state,
            this.mi_change_dev_state,
            this.mi_print_receipt,
            this.mi_change_seed});
            this.mi_actions.Text = "Azioni";
            // 
            // mi_refresh_devs_state
            // 
            this.mi_refresh_devs_state.Index = 0;
            this.mi_refresh_devs_state.Text = "Aggiorna stato dispositivi";
            this.mi_refresh_devs_state.Click += new System.EventHandler(this.menuItemAggiorna_Click);
            // 
            // mi_change_dev_state
            // 
            this.mi_change_dev_state.Index = 1;
            this.mi_change_dev_state.Text = "Modifica stato dispositivo...";
            // 
            // mi_print_receipt
            // 
            this.mi_print_receipt.Index = 2;
            this.mi_print_receipt.Text = "Stampa scontrino ultimo ciclo";
            this.mi_print_receipt.Click += new System.EventHandler(this.btnStampaScontrino_Click);
            // 
            // mi_change_seed
            // 
            this.mi_change_seed.Index = 3;
            this.mi_change_seed.Text = "Cambio sede";
            this.mi_change_seed.Click += new System.EventHandler(this.m_change_seed_Click);
            // 
            // mi_lists
            // 
            this.mi_lists.Index = 2;
            this.mi_lists.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mi_devices,
            this.mi_operators,
            this.mi_washers,
            this.mi_rf_readers,
            this.mi_receipts_view});
            this.mi_lists.Text = "Liste";
            // 
            // mi_devices
            // 
            this.mi_devices.Index = 0;
            this.mi_devices.Text = "Dispositivi";
            this.mi_devices.Click += new System.EventHandler(this.menuDispositivi_Click);
            // 
            // mi_operators
            // 
            this.mi_operators.Index = 1;
            this.mi_operators.Text = "Operatori";
            this.mi_operators.Click += new System.EventHandler(this.menuOperatori_Click);
            // 
            // mi_washers
            // 
            this.mi_washers.Index = 2;
            this.mi_washers.Text = "Armadi e Lavatrici";
            this.mi_washers.Click += new System.EventHandler(this.menuArmadiLavatrici_Click);
            // 
            // mi_rf_readers
            // 
            this.mi_rf_readers.Index = 3;
            this.mi_rf_readers.Text = "Lettori RFID";
            this.mi_rf_readers.Click += new System.EventHandler(this.menuLettori_Click);
            // 
            // mi_receipts_view
            // 
            this.mi_receipts_view.Index = 4;
            this.mi_receipts_view.Text = "Visione Scontrini";
            this.mi_receipts_view.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // mi_setup
            // 
            this.mi_setup.Index = 3;
            this.mi_setup.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mi_system_params,
            this.mi_setup_db_conn,
            this.menuItem12,
            this.mi_setup_dev_types,
            this.mi_dismiss_causes,
            this.menuItem14,
            this.mi_suppliers,
            this.menuItem1,
            this.mi_dev_states,
            this.mi_state_transactions,
            this.mi_dev_expiration,
            this.mier214comtest,
            this.mi_file_licenza,
            this.mi_sedi_uo});
            this.mi_setup.Text = "Configurazione";
            // 
            // mi_system_params
            // 
            this.mi_system_params.Index = 0;
            this.mi_system_params.Text = "Parametri di sistema";
            this.mi_system_params.Click += new System.EventHandler(this.menuParametri_click);
            // 
            // mi_setup_db_conn
            // 
            this.mi_setup_db_conn.Index = 1;
            this.mi_setup_db_conn.Text = "Imposta connessione database";
            this.mi_setup_db_conn.Click += new System.EventHandler(this.menuOpzioniDatabase_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 2;
            this.menuItem12.Text = "-";
            // 
            // mi_setup_dev_types
            // 
            this.mi_setup_dev_types.Index = 3;
            this.mi_setup_dev_types.Text = "Tipi dispositivi";
            this.mi_setup_dev_types.Click += new System.EventHandler(this.menuTipiDispositivi_Click);
            // 
            // mi_dismiss_causes
            // 
            this.mi_dismiss_causes.Index = 4;
            this.mi_dismiss_causes.Text = "Causali di dismissione";
            this.mi_dismiss_causes.Click += new System.EventHandler(this.menuCausaliDismissione_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 5;
            this.menuItem14.Text = "-";
            // 
            // mi_suppliers
            // 
            this.mi_suppliers.Index = 6;
            this.mi_suppliers.Text = "Fornitori";
            this.mi_suppliers.Click += new System.EventHandler(this.menuFornitori_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 7;
            this.menuItem1.Text = "-";
            // 
            // mi_dev_states
            // 
            this.mi_dev_states.Index = 8;
            this.mi_dev_states.Text = "Stati dispositivi";
            this.mi_dev_states.Click += new System.EventHandler(this.menuConfigurazioneStatiDispositivi_Click);
            // 
            // mi_state_transactions
            // 
            this.mi_state_transactions.Index = 9;
            this.mi_state_transactions.Text = "Transazioni stati dispositivi";
            this.mi_state_transactions.Click += new System.EventHandler(this.menuConfigurazioneTransazioni_Click);
            // 
            // mi_dev_expiration
            // 
            this.mi_dev_expiration.Index = 10;
            this.mi_dev_expiration.Text = "Scadenza stati dispositivi";
            this.mi_dev_expiration.Click += new System.EventHandler(this.menuConfigurazioneScadenzaStati_Click);
            // 
            // mier214comtest
            // 
            this.mier214comtest.Index = 11;
            this.mier214comtest.Text = "ER214 communication test";
            this.mier214comtest.Click += new System.EventHandler(this.mier214comtest_Click);
            // 
            // mi_file_licenza
            // 
            this.mi_file_licenza.Index = 12;
            this.mi_file_licenza.Text = "File Licenza";
            this.mi_file_licenza.Click += new System.EventHandler(this.mi_file_licenza_Click);
            // 
            // mi_sedi_uo
            // 
            this.mi_sedi_uo.Index = 13;
            this.mi_sedi_uo.Text = "Sedi e UO";
            this.mi_sedi_uo.Click += new System.EventHandler(this.mi_sedi_uo_Click);
            // 
            // mi_query
            // 
            this.mi_query.Index = 4;
            this.mi_query.Text = "Query";
            this.mi_query.Click += new System.EventHandler(this.menuQuery_Click);
            // 
            // menuIInfo
            // 
            this.menuIInfo.Index = 5;
            this.menuIInfo.Text = "?";
            this.menuIInfo.Click += new System.EventHandler(this.menuInfo_Click);
            // 
            // mi_info
            // 
            this.mi_info.Index = -1;
            this.mi_info.Text = "Informazioni su CleanTrack...";
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList.ImageSize = new System.Drawing.Size(46, 46);
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.OnTimer);
            // 
            // panelButtons
            // 
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.Location = new System.Drawing.Point(0, 593);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(2268, 82);
            this.panelButtons.TabIndex = 4;
            // 
            // listView
            // 
            this.listView.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnEmpty,
            this.columnMatricola,
            this.columnDispositivo,
            this.columnStato,
            this.columnEsami});
            this.listView.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listView.ForeColor = System.Drawing.SystemColors.ControlText;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(2268, 587);
            this.listView.SmallImageList = this.imageList;
            this.listView.TabIndex = 3;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
            this.listView.DoubleClick += new System.EventHandler(this.MostraCicliDispositivo);
            this.listView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listView_KeyPress);
            // 
            // columnEmpty
            // 
            this.columnEmpty.Text = " ";
            this.columnEmpty.Width = 0;
            // 
            // columnMatricola
            // 
            this.columnMatricola.Text = "Matricola";
            this.columnMatricola.Width = 80;
            // 
            // columnDispositivo
            // 
            this.columnDispositivo.Text = "Dispositivo";
            this.columnDispositivo.Width = 300;
            // 
            // columnStato
            // 
            this.columnStato.Text = "Stato";
            this.columnStato.Width = 210;
            // 
            // columnEsami
            // 
            this.columnEsami.Text = "Esami";
            this.columnEsami.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnEsami.Width = 80;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1924, 684);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.listView);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CLEAN TRACK - Stato Dispositivi";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.mainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainForm_FormClosed);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mainForm_KeyPress_1);
            this.Resize += new System.EventHandler(this.mainForm_Resize);
            this.ResumeLayout(false);

        }
        public MainForm()
        {
            InitializeComponent();
            timer.Interval = Globals.Refresh * 1000;
            timer.Start();
        }
        private void OnChangeSeed(int idseed) => this.ChangeSeed?.Invoke(idseed);
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        private void mainForm_Load(object sender, System.EventArgs e)
        {
            try
            {
                Globals.LocalizzaDialog(this);
                columnMatricola.SetColumnType(ColumnType.String);
                columnDispositivo.SetColumnType(ColumnType.String);
                columnStato.SetColumnType(ColumnType.String);
                columnEsami.SetColumnType(ColumnType.Number);
                var stateMenuList = StateList.Instance.GetMenuList();
                foreach (var state in stateMenuList)
                {
                    MenuItem item = new MenuItem() { Text = state.ActionDescription, Tag = state };
                    item.Click += changeState_Click;
                    mi_change_dev_state.MenuItems.Add(item);
                }
                var buttonList = StateList.Instance.GetPrincipalInterfaceList();
                if (buttonList.Count > 0)
                {
                    int iIndex = 0;
                    foreach (var state in stateMenuList)
                    {
                        if (buttonList.Where(b => b.ID == state.ID).Count() == 0)
                            continue;
                        Button btn = new Button();
                        btn.Height = 80;
                        btn.Width = 140;
                        btn.Text = state.ActionDescription;
                        btn.TabIndex = iIndex++;
                        btn.Tag = state;
                        btn.Click += changeState_Click;
                        btn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.button_KeyPress);
                        btn.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.button_KeyDownPreview);
                        btn.Font = new Font("Tahoma", 12, FontStyle.Bold);
                        panelButtons.Controls.Add(btn);
                        ButtonResize();
                    }
                }
                else
                {
                    listView.Dock = System.Windows.Forms.DockStyle.Fill;
                    panelButtons.Visible = false;
                }
                LoadServerSettings();
                listView.ListViewItemSorter = new ListViewExComparer(listView);
                RefreshLista(null, null);
                SetupMenus(Globals.ReadOnly);
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex);
            }
            finally
            {
                listView.Visible = true;
                Cursor = Cursors.Default;
            }
        }
        private void SetupMenus(bool readonly_mode = false)
        {
            try
            {
                this.mi_users.Enabled = false;
                this.mi_devices.Enabled = false;
                this.mi_operators.Enabled = false;
                this.mi_washers.Enabled = false;
                this.mi_setup.Enabled = false;
                this.mi_query.Enabled = false;
                this.mi_change_seed.Enabled = false;
                this.mi_file_licenza.Enabled = false;
                this.mi_sedi_uo.Enabled = false;
                this.mier214comtest.Enabled = false;
                if (readonly_mode)
                    return;
                if (InteropAmLogin.IsAdmin() || Globals.su_user_mode)
                {
                    mi_devices.Enabled = true;
                    mi_operators.Enabled = true;
                    mi_washers.Enabled = true;
                    mi_setup.Enabled = true;
                    mi_users.Enabled = true;
                    mi_users_management.Enabled = true;
                    mi_view_log.Enabled = true;
                    mi_query.Enabled = true;
                    this.mi_change_seed.Enabled = true;
                    this.mi_file_licenza.Enabled = true;
                    this.mi_sedi_uo.Enabled = true;
                    this.mier214comtest.Enabled = true;
                    return;
                }
                mi_users.Enabled = true;
                mi_users_management.Enabled = false;
                mi_operators.Enabled = KleanTrak.Globals.m_iUserPermission == 0 && DBUtil.IsUserAnOperator(Globals.m_iUserID);
            }
            catch (Exception e)
            {
                Globals.WarnAndLog(e);
            }
        }
        private void changeState_Click(object sender, EventArgs e)
        {
            if (Globals.ServerHttpEndpoint == null || Globals.ServerHttpEndpoint == "")
            {
                MessageBox.Show(KleanTrak.Globals.strTable[126], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            State state = null;
            if (sender is MenuItem)
                state = (State)((MenuItem)sender).Tag;
            else if (sender is Button)
                state = (State)((Button)sender).Tag;

            if (state == null)
                return;

            ChangeStateGeneric dlg = new ChangeStateGeneric(state);
            if (dlg.ShowDialog() == DialogResult.OK)
                RefreshLista(null, null);

            listView.Focus();
        }
        void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _columns.Clear();
                for (int i = 0; i < listView.Columns.Count; i++)
                    _columns.Add(i, listView.Columns[i].Width);
                SerializeColumnsDict();
                InteropAmLogin.AmLogin_DisableSelfLocalization();
            }
            catch (Exception ex)
            {
                Globals.Log(ex);
                throw;
            }
        }
        void menuQuery_Click(object sender, EventArgs e)
        {
            Query q = new Query();
            q.ShowDialog();
        }
        private void UserPrivilegesChanged()
        {
        }
        private void menuItemLogout_Click(object sender, System.EventArgs e)
        {
            InteropAmLogin.AmLogin_Logout();

            if (InteropAmLogin.AmLogin_Login_OLD(Globals.m_strUser, Globals.GetLocalIPAddress(), "CLEANTRACK", "Tesi Imaging - Cleantrack", KleanTrak.Globals.strDatabase))
            {
                InteropAmLogin.AmLogin_GetUsername(ref KleanTrak.Globals.m_strUser);
                InteropAmLogin.AmLogin_GetUserID(ref KleanTrak.Globals.m_iUserID);
                InteropAmLogin.AmLogin_GetUserPermission(ref KleanTrak.Globals.m_iUserPermission);
                UserPrivilegesChanged();
            }
            else
            {
                Close();
            }
        }
        private void menuItemChangePassword_Click(object sender, System.EventArgs e)
        {
            InteropAmLogin.AmLogin_ChangePassword(Globals.m_strUser, Globals.GetLocalIPAddress(), "CLEANTRACK");
        }
        private void menuItemUserManage_Click(object sender, System.EventArgs e)
        {
            if (InteropAmLogin.IsAdmin())
                InteropAmLogin.AmLogin_AdministrationDlg(Globals.m_strUser, Globals.GetLocalIPAddress(), "CLEANTRACK");
            else
                MessageBox.Show(KleanTrak.Globals.strTable[12], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void menuItemCurrentUser_Click(object sender, System.EventArgs e)
        {
            InteropAmLogin.AmLogin_ViewUserRights();
        }
        private void menuEsci_Click(object sender, System.EventArgs e)
        {
            // metti anche con X //

            this.Close();
        }
        private void menuOperatori_Click(object sender, System.EventArgs e)
        {
            Operatori dlg = new Operatori();
            dlg.ShowDialog();
        }
        private void menuArmadiLavatrici_Click(object sender, System.EventArgs e)
        {
            ArmadiLavatrici dlg = new ArmadiLavatrici();
            dlg.ShowDialog();
            if (dlg.Modified)
            {
                MessageBox.Show(Globals.strTable[217],
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
            }
        }
        private void menuOpzioniDatabase_Click(object sender, System.EventArgs e)
        {
            Database dlg = new Database();
            if (dlg.ShowDialog() != DialogResult.Cancel)
                menuEsci_Click(sender, e);
        }
        private void btnChiudi_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private Dictionary<int, int> _columns = null;
        private string COLUMNS_FILE = Application.StartupPath + @"\columns.bin";
        private void ResizeCols()
        {
            try
            {
                int lvColCount = listView.Columns.Count;
                foreach (int key in _columns.Keys)
                {
                    if (key >= lvColCount)
                        continue;
                    listView.Columns[key].Width = _columns[key];
                }
            }
            catch (Exception e)
            {
                Globals.Log(e);
                throw;
            }
        }
        private void ResizeList()
        {
            try
            {
                if (_columns == null)
                    InflateColumnsDict();
                if (_columns.Count > 0)
                    ResizeCols();
                else
                    Globals.ResizeList(this, listView);
            }
            catch (Exception e)
            {
                Globals.Log(e);
                throw;
            }
        }
        private void InflateColumnsDict()
        {
            if (!File.Exists(COLUMNS_FILE))
            {
                _columns = new Dictionary<int, int>();
                return;
            }
            IFormatter formatter = new BinaryFormatter();
            FileStream stream = null;
            try
            {
                stream = new FileStream(COLUMNS_FILE, FileMode.Open, FileAccess.Read);
                _columns = (Dictionary<int, int>)formatter.Deserialize(stream);
            }
            catch (Exception e)
            {
                Globals.Log(e);
            }
            finally
            {
                if (_columns == null)
                    _columns = new Dictionary<int, int>();
                if (stream != null)
                    stream.Close();
            }
        }
        private void SerializeColumnsDict()
        {
            try
            {
                if (File.Exists(COLUMNS_FILE))
                    File.Delete(COLUMNS_FILE);
                IFormatter formatter = new BinaryFormatter();
                FileStream stream = null;
                try
                {
                    stream = new FileStream(COLUMNS_FILE, FileMode.OpenOrCreate, FileAccess.Write);
                    formatter.Serialize(stream, _columns);
                }
                catch (Exception e)
                {
                    Globals.Log(e);
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
            catch (Exception e)
            {
                Globals.Log(e);
            }
        }
        private void RiempiLista()
        {
            listView.Visible = false;
            Cursor.Current = Cursors.WaitCursor;
            OdbcDataReader readerTemp1 = null;
            OdbcConnection connTemp1 = null;
            OdbcCommand commTemp1 = null;
            try
            {
                SetupImageList();
                listView.Items.Clear();
                List<State> stateList = StateList.Instance.GetPrincipalList();
                SetupListCoumns(stateList);
                connTemp1 = new OdbcConnection(Globals.strDatabase);
                if (connTemp1 != null)
                {
                    connTemp1.Open();
                    string query = $"SELECT * FROM VistaDispositiviUltimoCiclo " +
                        $"WHERE Dismesso = 0 AND IDSEDE = {KleanTrak.Globals.IDSEDE.ToString()} " +
                        $"ORDER BY DataOra, Descrizione";
                    commTemp1 = new OdbcCommand(query, connTemp1);
                    readerTemp1 = commTemp1.ExecuteReader();
                    while (readerTemp1.Read())
                    {
                        int nIDDispositivo = readerTemp1.GetIntEx("ID");
                        ListViewItem lvItem = GetListItem(stateList, readerTemp1, nIDDispositivo);
                        int nIDStatoNew = readerTemp1.GetIntEx("IDStatoNew");
                        string sDataOra = readerTemp1.GetStringEx("DataOra");
                        for (int i = 0; i < stateList.Count(); i++)
                        {
                            if (stateList[i].ID == nIDStatoNew)
                            {
                                lvItem.SubItems[5 + i].Text = KleanTrak.Globals.ConvertDateTime(sDataOra);
                                break;
                            }
                        }
                    }
                }
                ResizeList();
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex);
            }
            finally
            {
                if (readerTemp1 != null && !readerTemp1.IsClosed)
                    readerTemp1.Close();
                if (commTemp1 != null)
                    commTemp1.Dispose();
                if (connTemp1 != null)
                {
                    connTemp1.Close();
                    connTemp1.Dispose();
                }
                listView.Visible = true;
                Cursor = Cursors.Default;
            }
        }
        private void SetupListCoumns(List<State> state_list)
        {
            while (listView.Columns.Count > 5)
                listView.Columns.RemoveAt(5);
            foreach (var state in state_list)
            {
                ColumnHeader col = new ColumnHeader() { Text = state.ActionDescription, TextAlign = HorizontalAlignment.Center, Width = 200 };
                col.SetColumnType(ColumnType.Date);
                listView.Columns.Add(col);
            }
        }
        private void SetupImageList()
        {
            foreach (Bitmap bmp in imageList.Images)
                bmp.Dispose();
            imageList.Images.Clear();
            foreach (var state in StateList.Instance.GetList())
                imageList.Images.Add(state.Image);
        }
        private ListViewItem GetListItem(List<State> stateList, OdbcDataReader readerTemp1, int nIDDispositivo)
        {
            ListViewItem retval = null;
            for (int i = 0; i < listView.Items.Count; i++)
            {
                if (((int)listView.Items[i].Tag) == nIDDispositivo)
                {
                    retval = listView.Items[i];
                    return retval;
                }
            }
            retval = listView.Items.Add("");
            retval.Tag = nIDDispositivo;
            retval.SubItems.Add(readerTemp1.GetStringEx("Matricola"));
            retval.SubItems.Add(readerTemp1.GetStringEx("Descrizione"));
            retval.SubItems.Add(readerTemp1.GetStringEx("Stato"));
            SetStateIcon(readerTemp1, retval);
            retval.SubItems.Add(readerTemp1.GetIntEx("CicliEseguiti").ToString());
            for (int i = 0; i < stateList.Count(); i++)
                retval.SubItems.Add("");
            return retval;
        }
        private void SetStateIcon(OdbcDataReader readerTemp1, ListViewItem lvItem)
        {
            int nState = readerTemp1.GetIntEx("IDStato");
            for (int i = 0; i < StateList.Instance.GetList().Count; i++)
            {
                if (StateList.Instance.GetList()[i].ID != nState)
                    continue;
                listView.SetSubItemImage(lvItem.Index, 3, i);
                break;
            }
        }
        private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            timer.Enabled = false;
            ((ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
            timer.Enabled = true;
        }
        private void OnTimer(object sender, EventArgs e)
        {
            // verifico se l'utente è mai stato inserito all'interno della lista operatori
            // vista dispositivi (usata per vistadispositiviultimociclo) già filtrata per Dispositivi.eliminato = 0 
            string query = $"SELECT MAX(DATAORA) FROM VISTADISPOSITIVIULTIMOCICLO " +
                $"WHERE DISMESSO = 0 AND IDSEDE = {KleanTrak.Globals.IDSEDE}";
            OdbcConnection conn = null;
            OdbcCommand cmd = null;
            OdbcDataReader reader = null;
            try
            {
                conn = DBUtil.GetODBCConnection();
                cmd = new OdbcCommand(query, conn);
                reader = cmd.ExecuteReader();
                if (reader.Read() && !reader.IsDBNull(0))
                {
                    var newVal = reader.GetString(0);
                    if (newVal != CurrentLastDate)
                    {
                        CurrentLastDate = newVal;
                        RefreshLista(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Get().Write("",
                    "KleanTrack.KleanTrack.OnTimer",
                    ex.ToString(),
                    null,
                    Logger.LogLevel.Error);
            }
            finally
            {
                if (reader != null || !reader.IsClosed)
                    reader.Close();
                if (cmd != null)
                    cmd.Dispose();
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }
        }
        private void RefreshLista(object sender, System.EventArgs e)
        {
            try
            {
                timer.Enabled = false;
                RiempiLista();
                listView.Sort();
                listView.Focus();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "", "KleanTrack.KleanTrack.RefreshList");
            }
            finally
            {
                timer.Enabled = true;
            }
        }
        private void MostraCicliDispositivo(object sender, System.EventArgs e)
        {
            if (listView.SelectedIndices.Count > 0)
            {
                CicliDispositivo dlg = new CicliDispositivo((int)listView.Items[listView.SelectedIndices[0]].Tag);
                dlg.ShowDialog();
            }
        }
        private void menuItem1_Click(object sender, System.EventArgs e)
        {
            Setup dlg = new Setup();
            dlg.ShowDialog();
        }
        private void menuDisconnetti_Click(object sender, System.EventArgs e)
        {
            InteropAmLogin.AmLogin_Logout();

            if (InteropAmLogin.AmLogin_Login_OLD(Globals.m_strUser, Globals.GetLocalIPAddress(), "CLEANTRACK", "Tesi Imaging - Cleantrack", KleanTrak.Globals.strDatabase))
            {
                InteropAmLogin.AmLogin_GetUsername(ref KleanTrak.Globals.m_strUser);
                InteropAmLogin.AmLogin_GetUserID(ref KleanTrak.Globals.m_iUserID);
                InteropAmLogin.AmLogin_GetUserPermission(ref KleanTrak.Globals.m_iUserPermission);
                UserPrivilegesChanged();

                if (KleanTrak.Globals.m_iUserPermission == 0)
                {
                    mi_devices.Enabled = false;
                    mi_washers.Enabled = false;
                    mi_setup.Enabled = false;
                    mi_users_management.Enabled = false;
                    mi_view_log.Enabled = false;
                    mi_change_pin.Enabled = true;

                    // verifico se l'utente è mai stato inserito all'interno della lista operatori
                    string query = "select id_utente from operatori where id_utente = " + KleanTrak.Globals.m_iUserID;

                    OdbcConnection connTemp = DBUtil.GetODBCConnection();
                    OdbcCommand commTemp = new OdbcCommand(query, connTemp);
                    OdbcDataReader readerTemp = commTemp.ExecuteReader();

                    if (readerTemp.HasRows == false)
                    {
                        mi_operators.Enabled = true;
                    }
                    else
                    {
                        mi_operators.Enabled = false;
                    }

                    if ((readerTemp != null) && (readerTemp.IsClosed == false))
                        readerTemp.Close();

                    connTemp.Close();
                }
                else if (InteropAmLogin.IsAdmin())
                {
                    mi_devices.Enabled = true;
                    mi_operators.Enabled = true;
                    mi_washers.Enabled = true;
                    mi_setup.Enabled = true;
                    mi_users_management.Enabled = true;
                    mi_view_log.Enabled = true;
                    mi_change_pin.Enabled = true;
                }
            }
            else
            {
                Close();
            }
        }
        private void menuCambioPassword_Click(object sender, System.EventArgs e)
        {
            InteropAmLogin.AmLogin_ChangePassword(Globals.m_strUser, Globals.GetLocalIPAddress(), "CLEANTRACK");
        }

        private void menuItem6_Click(object sender, System.EventArgs e)
        {
            if (InteropAmLogin.IsAdmin())
                InteropAmLogin.AmLogin_AdministrationDlg(Globals.m_strUser, Globals.GetLocalIPAddress(), "CLEANTRACK");
            else
                MessageBox.Show(KleanTrak.Globals.strTable[13], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void menuDirittiUtente_Click(object sender, System.EventArgs e)
        {
            InteropAmLogin.AmLogin_ViewUserRights();
        }

        private void menuTipiDispositivi_Click(object sender, System.EventArgs e)
        {
            TipiDispositivi dlg = new TipiDispositivi();
            dlg.ShowDialog();
        }

        private void menuCausaliDismissione_Click(object sender, System.EventArgs e)
        {
            Causali dlg = new Causali();
            dlg.ShowDialog();
        }

        private void menuParametri_click(object sender, System.EventArgs e)
        {
            Setup dlg = new Setup();
            dlg.ShowDialog();
        }

        private void menuItem1_Click_1(object sender, System.EventArgs e)
        {
            if (InteropAmLogin.IsAdmin())
            {
                Chiavi_Log_Transazioni dlg = new Chiavi_Log_Transazioni();
                dlg.ShowDialog();
            }
            else
                MessageBox.Show(KleanTrak.Globals.strTable[94], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void menuFile_Click(object sender, System.EventArgs e)
        {
        }

        private void menuLettori_Click(object sender, System.EventArgs e)
        {
            LettoriBadge dlg = new LettoriBadge();
            dlg.ShowDialog();
        }

        private void menuCambioPinClick(object sender, System.EventArgs e)
        {
            //verifico se l'utente è operatore
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("select id_utente from operatori where id_utente = " + KleanTrak.Globals.m_iUserID, connTemp);
            OdbcDataReader readerTemp = commTemp.ExecuteReader();

#if !DEBUG
			if (!readerTemp.HasRows)
			{
				MessageBox.Show(KleanTrak.Globals.strTable[127], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else
#endif
            {
                ChangePinUser dlg = new ChangePinUser();
                dlg.ShowDialog();
            }

            connTemp.Close();
        }

        private void listView_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (((int)e.KeyChar).Equals((int)Keys.Enter) ||
                ((int)e.KeyChar).Equals((int)Keys.Space))
            {
                if (CodeRequested.Length == 3)
                {
                    if (ChangeStateBaseForm.OpenChangeStateForms(CodeRequested))
                    {
                        CodeRequested = "";
                        RefreshLista(null, null);
                    }
                }
                else
                    CodeRequested = "";
            }
            else
                CodeRequested += e.KeyChar;
        }

        private void btnStampaScontrino_Click(object sender, EventArgs e)
        {
            StampaScontrino dlg = new StampaScontrino();
            if (dlg.ShowDialog() == DialogResult.OK)
                RefreshLista(null, null);
        }

        private void menuFornitori_Click(object sender, EventArgs e)
        {
            Fornitori dlg = new Fornitori();
            dlg.ShowDialog();
        }

        private void menuDispositivi_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Dispositivi dlg = new Dispositivi();
            dlg.ShowDialog();
        }

        private void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.Get().Stop();
        }

        private void menuItemAggiorna_Click(object sender, EventArgs e)
        {
            RefreshLista(null, null);
        }

        private void menuInfo_Click(object sender, EventArgs e)
        {
            FormInfo dlg = new FormInfo();
            dlg.ShowDialog();
        }

        private void mainForm_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            listView_KeyPress(sender, e);
        }

        private void LoadServerSettings()
        {
            OdbcConnection conn = DBUtil.GetODBCConnection();

            OdbcCommand cmd = new OdbcCommand("SELECT * FROM CONFIGURAZIONE", conn);
            OdbcDataReader reader = null;

            string key = "", value = "";
            string host = "", port = "";

            try
            {
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal("CHIAVE")))
                        key = reader.GetString(reader.GetOrdinal("CHIAVE"));
                    if (!reader.IsDBNull(reader.GetOrdinal("VALORE")))
                        value = reader.GetString(reader.GetOrdinal("VALORE"));
                    if (key.ToUpper() == "SERVER HOST")
                        host = value;
                    else if (key.ToUpper() == "SERVER PORT")
                        port = value;
                }
            }
            catch (Exception)
            {
            }

            if (reader != null && !reader.IsClosed)
                reader.Close();

            if (conn.State != ConnectionState.Closed)
                conn.Close();

            if (host != "" && port != "")
                Globals.ServerHttpEndpoint = "http://" + host + ":" + port + "/AcceptMessage";
        }

        private void menuConfigurazioneStatiDispositivi_Click(object sender, EventArgs e)
        {
            ConfigurazioneStatiDispositivi dlg = new ConfigurazioneStatiDispositivi();
            dlg.ShowDialog();
        }

        private void menuConfigurazioneTransazioni_Click(object sender, EventArgs e)
        {
            ConfigurazioneTransazioni dlg = new ConfigurazioneTransazioni();
            dlg.ShowDialog();
        }

        private void menuConfigurazioneScadenzaStati_Click(object sender, EventArgs e)
        {
            ConfigurazioneScadenzaDispositivi dlg = new ConfigurazioneScadenzaDispositivi();
            dlg.ShowDialog();
        }

        private void mainForm_Resize(object sender, EventArgs e)
        {
            ButtonResize();
        }

        void ButtonResize()
        {
            if (panelButtons.Controls.Count == 0)
                return;

            int iButtonSize = 0;
            for (int i = 0; i < panelButtons.Controls.Count; i++)
                iButtonSize += panelButtons.Controls[i].Width;

            int iSpace = (panelButtons.Width - iButtonSize) / (panelButtons.Controls.Count + 1);

            for (int i = 0; i < panelButtons.Controls.Count; i++)
                panelButtons.Controls[i].Margin = new Padding(iSpace, 0, 0, 0);
        }

        private void button_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            listView_KeyPress(sender, e);
        }

        // in modo da gestire l'invio con l'handler collegato alla lista
        private void button_KeyDownPreview(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                listView.Focus();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            new FrmReceiptsView().ShowDialog();
        }

        private void m_change_seed_Click(object sender, EventArgs e)
        {
            try
            {
                var frm_seeds = new ChooseSeed();
                frm_seeds.ShowDialog();
                if (!frm_seeds.DataValid)
                    return;
                Globals.IDSEDE = frm_seeds.Idseed;
                Globals.DESCR_SEDE = DBUtil.GetDescrSede(frm_seeds.Idseed);
                int dash_position = this.Text.LastIndexOf(" -");
                if (dash_position > 0)
                    this.Text = this.Text.Remove(dash_position, this.Text.Length - dash_position);
                this.Text += $" - {Globals.DESCR_SEDE}";
                this.RefreshLista(null, null);
                this.OnChangeSeed(frm_seeds.Idseed);
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex);
            }
        }

        private void mier214comtest_Click(object sender, EventArgs e)
        {
            try
            {
                new MDGTest.FrmMain().ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "mier214comtest_Click");
                MessageBox.Show(ex.ToString());
            }
        }

        private void mi_file_licenza_Click(object sender, EventArgs e)
        {
            try
            {
                new LicenseForm().ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "mi_file_licenza_click");
                MessageBox.Show(ex.ToString());
            }
        }

        private void mi_sedi_uo_Click(object sender, EventArgs e)
        {
            try
            {
                new SediUO().ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "mi_sedi_uo_Click");
                MessageBox.Show(ex.ToString());
            }
        }
    }
}