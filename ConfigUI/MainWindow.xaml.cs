using System;
using System.Windows;
using System.Windows.Media;
using System.Data.Odbc;

namespace ConfigUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (App.server)
            {
                lblTitle.Content = "SERVER CONFIGURATION";
            }

            if (App.client)
            {
                lblTitle.Content = "CLIENT CONFIGURATION";
            }

            tbDriverMssql.Text = "SQL Server";
            tbDriverOracle.Text = "Oracle in instantclient_19_5";

            RadioButtonMssql.IsChecked = true;
            RadioButton_Checked(this.RadioButtonMssql, null);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == RadioButtonMssql)
            {
                BorderMssql.IsEnabled = true;
                BorderOracle.IsEnabled = false;
                tbDriverMssql.Text = "SQL Server";
            }
            else if (sender == RadioButtonOracle)
            {
                BorderMssql.IsEnabled = false;
                BorderOracle.IsEnabled = true;
                tbDriverOracle.Text = "Oracle in instantclient_19_5";
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Save();
            Close();
        }

        private bool GetConnectionString(out string connStr)
        {
            connStr = null;
            bool valid = false;
            if (RadioButtonMssql.IsChecked == true)
            {
                //per mssql:  DbConnStr=Driver={SQL Server};Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;
                string svr = tbServer.Text + ((tbPort.Text != null && tbPort.Text.Trim().Length > 0) ? $",{tbPort.Text}" : "");
                connStr = $"Driver={{{tbDriverMssql.Text}}};Server={svr};Database={tbDatabase.Text};Uid={tbUid.Text};Pwd={tbPwd.Text};";
                valid = tbDriverMssql.Text.Length > 0 && svr.Length > 0 && tbDatabase.Text.Length > 0 && tbUid.Text.Length > 0 && tbPwd.Text.Length > 0;
            }
            else if (RadioButtonOracle.IsChecked == true)
            {
                // per oracle: DbConnStr=Driver={Oracle in instantclient_19_5};Dbq=myTNSServiceName;Uid=myUsername;Pwd=myPassword;
                connStr = $"Driver={{{tbDriverOracle.Text}}};Dbq={tbDbq.Text};Uid={tbUid.Text};Pwd={tbPwd.Text};";
                valid = tbDriverOracle.Text.Length > 0 && tbDbq.Text.Length > 0 && tbUid.Text.Length > 0 && tbPwd.Text.Length > 0;
            }
            return valid;
        }

        private void Save()
        {
            IniFile ini = new IniFile(App.iniFilePath);
            if (App.server)
            {
                bool valid = GetConnectionString(out string connStr);
                ini.IniWriteValue("Settings", "DbConnStr", Crypto.Encrypt(connStr));
            }
            if (App.client)
            {
                if (RadioButtonMssql.IsChecked == true)
                {
                    /*
                    [ConnessioneDB]
                    Driver=Sql Server                       //Driver odbc per db (vedere dirver odbc presenti su pc e paragrafo dedicato alla connessione a db)
                    UserName=CLEANTRACK_PAVIA               //Nome utente per connessione al db (vedere paragrafo dedicato alla connessione a db)
                    Password=xxxxxxxx                       //Password per connessione al db (vedere paragrafo dedicato alla connessione a db)
                    Server=server=indirizzo_ip_server,porta //indirizzo ip del server (porta di solito 1433)
                    Database=CLEANTRACK_PAVIA               //Nome db, per oracle è lo stesso nome utilizzato in UserName
                     */
                    ini.IniWriteValue("ConnessioneDB", "Driver", tbDriverMssql.Text);
                    ini.IniWriteValue("ConnessioneDB", "UserName", Crypto.Encrypt(tbUid.Text));
                    ini.IniWriteValue("ConnessioneDB", "Password", Crypto.Encrypt(tbPwd.Text));
                    ini.IniWriteValue("ConnessioneDB", "Server",
                        $"server={tbServer.Text}" + ((tbPort.Text != null && tbPort.Text.Trim().Length > 0) ? $",{tbPort.Text}" : ""));
                    ini.IniWriteValue("ConnessioneDB", "Database", tbDatabase.Text);
                }
                else if (RadioButtonOracle.IsChecked == true)
                {
                    /*
                     [ConnessioneDB]
                        Driver=Oracle in instantclient_19_5     //Driver odbc per db (vedere dirver odbc presenti su pc e paragrafo dedicato alla connessione a db)
                        UserName=CLEANTRACK_PAVIA               //Nome utente per connessione al db (vedere paragrafo dedicato alla connessione a db)
                        Password=xxx xxxxx                      //Password per connessione al db (vedere paragrafo dedicato alla connessione a db)
                        Server=dbq=ORACLESRV                    //Connessione specificata in tnsnames.ora
                        Database=CLEANTRACK_PAVIA               //Nome db, per oracle è lo stesso nome utilizzato in UserName             
                     */
                    ini.IniWriteValue("ConnessioneDB", "Driver", tbDriverOracle.Text);
                    ini.IniWriteValue("ConnessioneDB", "UserName", Crypto.Encrypt(tbUid.Text));
                    ini.IniWriteValue("ConnessioneDB", "Password", Crypto.Encrypt(tbPwd.Text));
                    ini.IniWriteValue("ConnessioneDB", "Server", $"dbq={tbDbq.Text}");
                    ini.IniWriteValue("ConnessioneDB", "Database", tbUid.Text);
                }
                ini.IniWriteValue("Generale", "IDSEDE", "1");
            }
        }

        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool valid = GetConnectionString(out string connStr);
                if (connStr == null || !valid)
                    throw new Exception("Connection string incomplete!");
                OdbcConnection conn = new OdbcConnection(connStr);
                conn.Open();
                RectTest.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                RectTest.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

    }
}
