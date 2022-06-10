using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;

namespace KleanTrak
{
    public class Database : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button btnSalva;
        private System.Windows.Forms.Button btnProva;
        private System.Windows.Forms.Label labelDriver;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelServer;
        private System.Windows.Forms.Button btnAnnulla;
        private System.Windows.Forms.TextBox textBoxDriver;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelPasswordAccesso;
        private System.Windows.Forms.Button btnAnnullaAccesso;
        private System.Windows.Forms.Button btnConfermaAccesso;
        private System.Windows.Forms.TextBox textBoxPasswordAccesso;
        private System.Windows.Forms.Panel panel2;
        private System.ComponentModel.Container components = null;

        public Database()
        {
            InitializeComponent();
        }

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

        #region Codice generato da Progettazione Windows Form
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Database));
            this.btnSalva = new System.Windows.Forms.Button();
            this.btnProva = new System.Windows.Forms.Button();
            this.labelDriver = new System.Windows.Forms.Label();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelServer = new System.Windows.Forms.Label();
            this.textBoxDriver = new System.Windows.Forms.TextBox();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.btnAnnulla = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelPasswordAccesso = new System.Windows.Forms.Label();
            this.btnAnnullaAccesso = new System.Windows.Forms.Button();
            this.btnConfermaAccesso = new System.Windows.Forms.Button();
            this.textBoxPasswordAccesso = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSalva
            // 
            this.btnSalva.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSalva.Location = new System.Drawing.Point(560, 180);
            this.btnSalva.Name = "btnSalva";
            this.btnSalva.Size = new System.Drawing.Size(200, 45);
            this.btnSalva.TabIndex = 6;
            this.btnSalva.Text = "Salva";
            this.btnSalva.Click += new System.EventHandler(this.btnSalva_Click);
            // 
            // btnProva
            // 
            this.btnProva.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnProva.Location = new System.Drawing.Point(0, 180);
            this.btnProva.Name = "btnProva";
            this.btnProva.Size = new System.Drawing.Size(200, 45);
            this.btnProva.TabIndex = 5;
            this.btnProva.Text = "Test";
            this.btnProva.Click += new System.EventHandler(this.btnProva_Click);
            // 
            // labelDriver
            // 
            this.labelDriver.Location = new System.Drawing.Point(0, 0);
            this.labelDriver.Name = "labelDriver";
            this.labelDriver.Size = new System.Drawing.Size(150, 34);
            this.labelDriver.TabIndex = 2;
            this.labelDriver.Text = "Driver ODBC";
            this.labelDriver.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelUserName
            // 
            this.labelUserName.Location = new System.Drawing.Point(0, 45);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(150, 35);
            this.labelUserName.TabIndex = 3;
            this.labelUserName.Text = "Nome utente";
            this.labelUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPassword
            // 
            this.labelPassword.Location = new System.Drawing.Point(0, 90);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(150, 34);
            this.labelPassword.TabIndex = 4;
            this.labelPassword.Text = "Password";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelServer
            // 
            this.labelServer.Location = new System.Drawing.Point(0, 135);
            this.labelServer.Name = "labelServer";
            this.labelServer.Size = new System.Drawing.Size(150, 35);
            this.labelServer.TabIndex = 5;
            this.labelServer.Text = "Server";
            this.labelServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxDriver
            // 
            this.textBoxDriver.Location = new System.Drawing.Point(160, 0);
            this.textBoxDriver.Name = "textBoxDriver";
            this.textBoxDriver.Size = new System.Drawing.Size(600, 34);
            this.textBoxDriver.TabIndex = 1;
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(160, 45);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(600, 34);
            this.textBoxUserName.TabIndex = 2;
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(160, 90);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(600, 34);
            this.textBoxPassword.TabIndex = 3;
            // 
            // textBoxServer
            // 
            this.textBoxServer.Location = new System.Drawing.Point(160, 135);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(600, 34);
            this.textBoxServer.TabIndex = 4;
            // 
            // btnAnnulla
            // 
            this.btnAnnulla.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAnnulla.Location = new System.Drawing.Point(350, 180);
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.Size = new System.Drawing.Size(200, 45);
            this.btnAnnulla.TabIndex = 7;
            this.btnAnnulla.Text = "Annulla";
            this.btnAnnulla.Click += new System.EventHandler(this.btnAnnulla_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelPasswordAccesso);
            this.panel1.Controls.Add(this.btnAnnullaAccesso);
            this.panel1.Controls.Add(this.btnConfermaAccesso);
            this.panel1.Controls.Add(this.textBoxPasswordAccesso);
            this.panel1.Location = new System.Drawing.Point(10, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(760, 225);
            this.panel1.TabIndex = 1;
            // 
            // labelPasswordAccesso
            // 
            this.labelPasswordAccesso.Location = new System.Drawing.Point(0, 0);
            this.labelPasswordAccesso.Name = "labelPasswordAccesso";
            this.labelPasswordAccesso.Size = new System.Drawing.Size(160, 34);
            this.labelPasswordAccesso.TabIndex = 4;
            this.labelPasswordAccesso.Text = "PASSWORD";
            this.labelPasswordAccesso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAnnullaAccesso
            // 
            this.btnAnnullaAccesso.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAnnullaAccesso.Location = new System.Drawing.Point(350, 180);
            this.btnAnnullaAccesso.Name = "btnAnnullaAccesso";
            this.btnAnnullaAccesso.Size = new System.Drawing.Size(200, 45);
            this.btnAnnullaAccesso.TabIndex = 3;
            this.btnAnnullaAccesso.Text = "Esci";
            this.btnAnnullaAccesso.Click += new System.EventHandler(this.btnAnnullaAccesso_Click);
            // 
            // btnConfermaAccesso
            // 
            this.btnConfermaAccesso.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnConfermaAccesso.Location = new System.Drawing.Point(560, 180);
            this.btnConfermaAccesso.Name = "btnConfermaAccesso";
            this.btnConfermaAccesso.Size = new System.Drawing.Size(200, 45);
            this.btnConfermaAccesso.TabIndex = 2;
            this.btnConfermaAccesso.Text = "Conferma";
            this.btnConfermaAccesso.Click += new System.EventHandler(this.btnConfermaAccesso_Click);
            // 
            // textBoxPasswordAccesso
            // 
            this.textBoxPasswordAccesso.Location = new System.Drawing.Point(160, 0);
            this.textBoxPasswordAccesso.Name = "textBoxPasswordAccesso";
            this.textBoxPasswordAccesso.PasswordChar = '*';
            this.textBoxPasswordAccesso.Size = new System.Drawing.Size(600, 34);
            this.textBoxPasswordAccesso.TabIndex = 1;
            this.textBoxPasswordAccesso.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPasswordAccesso_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxServer);
            this.panel2.Controls.Add(this.textBoxPassword);
            this.panel2.Controls.Add(this.btnSalva);
            this.panel2.Controls.Add(this.textBoxUserName);
            this.panel2.Controls.Add(this.btnProva);
            this.panel2.Controls.Add(this.labelDriver);
            this.panel2.Controls.Add(this.labelUserName);
            this.panel2.Controls.Add(this.labelPassword);
            this.panel2.Controls.Add(this.labelServer);
            this.panel2.Controls.Add(this.textBoxDriver);
            this.panel2.Controls.Add(this.btnAnnulla);
            this.panel2.Location = new System.Drawing.Point(10, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(760, 225);
            this.panel2.TabIndex = 2;
            this.panel2.Visible = false;
            // 
            // Database
            // 
            this.ClientSize = new System.Drawing.Size(777, 242);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Database";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database";
            this.Load += new System.EventHandler(this.Database_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private void Database_Load(object sender, System.EventArgs e)
        {
            string strInitFile = Application.StartupPath + "\\kleantrak.ini";

            KleanTrak.Globals.LocalizzaDialog(this);

            textBoxDriver.Text = ExtModules.InteropKernel32.GetPrivateProfileString("ConnessioneDB", "Driver", "", strInitFile);
            string usrIni = ExtModules.InteropKernel32.GetPrivateProfileString("ConnessioneDB", "UserName", "", strInitFile);
            textBoxUserName.Text = Core.Crypto.TryDecrypt(usrIni, out string usr) ? usr : usrIni; // if TryDecrypt fails, the original string is already clear
            string pwdIni = ExtModules.InteropKernel32.GetPrivateProfileString("ConnessioneDB", "Password", "", strInitFile);
            textBoxPassword.Text = Core.Crypto.TryDecrypt(pwdIni, out string pwd) ? pwd : pwdIni;
            textBoxServer.Text = ExtModules.InteropKernel32.GetPrivateProfileString("ConnessioneDB", "Server", "", strInitFile);
        }

        private void btnAnnullaAccesso_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnConfermaAccesso_Click(object sender, System.EventArgs e)
        {
            if (textBoxPasswordAccesso.Text == "nautilus")
            {
                panel1.Visible = false;
                panel2.Visible = true;
            }
            else
            {
                textBoxPasswordAccesso.Text = "";
            }
        }

        private void btnProva_Click(object sender, System.EventArgs e)
        {
            OdbcConnection connTemp = null;

            try
            {
                connTemp = new OdbcConnection("DRIVER={" + textBoxDriver.Text + "};UID=" + textBoxUserName.Text + ";PWD=" + textBoxPassword.Text + ";" + textBoxServer.Text);
                connTemp.Open();
                MessageBox.Show(KleanTrak.Globals.strTable[14]);
            }
            catch (OdbcException Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            catch (ArgumentException Ex)
            {
                MessageBox.Show(Ex.Message);
            }

            if (connTemp != null && connTemp.State == ConnectionState.Open)
                connTemp.Close();
        }

        private void btnAnnulla_Click(object sender, System.EventArgs e)
        {
            btnAnnullaAccesso_Click(sender, e);
        }

        private void btnSalva_Click(object sender, System.EventArgs e)
        {
            string strInitFile = Application.StartupPath + "\\kleantrak.ini";

            ExtModules.InteropKernel32.WritePrivateProfileString("ConnessioneDB", "Driver", textBoxDriver.Text, strInitFile);
            ExtModules.InteropKernel32.WritePrivateProfileString("ConnessioneDB", "UserName", Core.Crypto.Encrypt(textBoxUserName.Text), strInitFile);
            ExtModules.InteropKernel32.WritePrivateProfileString("ConnessioneDB", "Password", Core.Crypto.Encrypt(textBoxPassword.Text), strInitFile);
            ExtModules.InteropKernel32.WritePrivateProfileString("ConnessioneDB", "Server", textBoxServer.Text, strInitFile);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBoxPasswordAccesso_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnConfermaAccesso_Click(sender, null);
        }
    }
}
