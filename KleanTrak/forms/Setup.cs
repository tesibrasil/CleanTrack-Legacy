using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;

namespace KleanTrak
{
	/// <summary>
	/// Descrizione di riepilogo per Setup.
	/// </summary>
	public class Setup : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnChiudi;
		private System.Windows.Forms.Button btn_save;
		private System.Windows.Forms.ComboBox comboBoxLingua;
		private System.Windows.Forms.Label labelLingua;
		private System.Windows.Forms.TextBox textBoxAddessIP;
		private System.Windows.Forms.TextBox textBoxPort;
		private System.Windows.Forms.TextBox textBoxTimeout;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox checkBoxEnable;
        private Label label6;
		private ComboBox cb_sede;
		private List<Query.ComboboxItem> seeds = DBUtil.GetSeeds();

		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Setup()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Pulire le risorse in uso.
		/// </summary>
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
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			this.btn_save = new System.Windows.Forms.Button();
			this.btnChiudi = new System.Windows.Forms.Button();
			this.comboBoxLingua = new System.Windows.Forms.ComboBox();
			this.labelLingua = new System.Windows.Forms.Label();
			this.textBoxAddessIP = new System.Windows.Forms.TextBox();
			this.textBoxPort = new System.Windows.Forms.TextBox();
			this.textBoxTimeout = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBoxEnable = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cb_sede = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btn_save.Location = new System.Drawing.Point(313, 442);
			this.btn_save.Name = "btn_save";
			this.btn_save.Size = new System.Drawing.Size(120, 36);
			this.btn_save.TabIndex = 43;
			this.btn_save.Text = "Salva";
			this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
			// 
			// btnChiudi
			// 
			this.btnChiudi.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnChiudi.Location = new System.Drawing.Point(447, 440);
			this.btnChiudi.Name = "btnChiudi";
			this.btnChiudi.Size = new System.Drawing.Size(120, 36);
			this.btnChiudi.TabIndex = 42;
			this.btnChiudi.Text = "Esci";
			this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
			// 
			// comboBoxLingua
			// 
			this.comboBoxLingua.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxLingua.ItemHeight = 18;
			this.comboBoxLingua.Items.AddRange(new object[] {
            "ITALIANO",
            "ENGLISH",
            "ESPAÑOL",
            "PORTUGUESE"});
			this.comboBoxLingua.Location = new System.Drawing.Point(260, 42);
			this.comboBoxLingua.Name = "comboBoxLingua";
			this.comboBoxLingua.Size = new System.Drawing.Size(277, 26);
			this.comboBoxLingua.TabIndex = 2;
			// 
			// labelLingua
			// 
			this.labelLingua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelLingua.Location = new System.Drawing.Point(13, 42);
			this.labelLingua.Name = "labelLingua";
			this.labelLingua.Size = new System.Drawing.Size(234, 36);
			this.labelLingua.TabIndex = 1;
			this.labelLingua.Text = "Lingua:";
			this.labelLingua.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textBoxAddessIP
			// 
			this.textBoxAddessIP.Location = new System.Drawing.Point(260, 84);
			this.textBoxAddessIP.Name = "textBoxAddessIP";
			this.textBoxAddessIP.Size = new System.Drawing.Size(277, 26);
			this.textBoxAddessIP.TabIndex = 44;
			// 
			// textBoxPort
			// 
			this.textBoxPort.Location = new System.Drawing.Point(260, 129);
			this.textBoxPort.Name = "textBoxPort";
			this.textBoxPort.Size = new System.Drawing.Size(277, 26);
			this.textBoxPort.TabIndex = 45;
			// 
			// textBoxTimeout
			// 
			this.textBoxTimeout.Location = new System.Drawing.Point(260, 174);
			this.textBoxTimeout.Name = "textBoxTimeout";
			this.textBoxTimeout.Size = new System.Drawing.Size(150, 26);
			this.textBoxTimeout.TabIndex = 46;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cb_sede);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.labelLingua);
			this.groupBox1.Controls.Add(this.comboBoxLingua);
			this.groupBox1.Location = new System.Drawing.Point(10, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(557, 151);
			this.groupBox1.TabIndex = 47;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Impostazioni generali";
			// 
			// label6
			// 
			this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label6.Location = new System.Drawing.Point(13, 88);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(234, 36);
			this.label6.TabIndex = 4;
			this.label6.Text = "Sede";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkBoxEnable);
			this.groupBox2.Controls.Add(this.label5);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.textBoxAddessIP);
			this.groupBox2.Controls.Add(this.textBoxTimeout);
			this.groupBox2.Controls.Add(this.textBoxPort);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(10, 202);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(557, 222);
			this.groupBox2.TabIndex = 48;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Lettore badge";
			// 
			// checkBoxEnable
			// 
			this.checkBoxEnable.Location = new System.Drawing.Point(260, 36);
			this.checkBoxEnable.Name = "checkBoxEnable";
			this.checkBoxEnable.Size = new System.Drawing.Size(23, 36);
			this.checkBoxEnable.TabIndex = 50;
			// 
			// label5
			// 
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label5.Location = new System.Drawing.Point(13, 39);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(234, 36);
			this.label5.TabIndex = 49;
			this.label5.Text = "Attivo:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label4.Location = new System.Drawing.Point(13, 174);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(234, 36);
			this.label4.TabIndex = 48;
			this.label4.Text = "Timeout:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label3.Location = new System.Drawing.Point(13, 129);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(234, 36);
			this.label3.TabIndex = 47;
			this.label3.Text = "Porta:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.label2.Location = new System.Drawing.Point(13, 84);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(234, 36);
			this.label2.TabIndex = 3;
			this.label2.Text = "Indirizzo IP:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// cb_sede
			// 
			this.cb_sede.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cb_sede.ItemHeight = 18;
			this.cb_sede.Location = new System.Drawing.Point(260, 88);
			this.cb_sede.Name = "cb_sede";
			this.cb_sede.Size = new System.Drawing.Size(277, 26);
			this.cb_sede.TabIndex = 5;
			this.cb_sede.SelectedIndexChanged += new System.EventHandler(this.cb_sede_SelectedIndexChanged);
			// 
			// Setup
			// 
			this.ClientSize = new System.Drawing.Size(595, 495);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnChiudi);
			this.Controls.Add(this.btn_save);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Setup";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CLEAN TRACK - Setup Sistema";
			this.Load += new System.EventHandler(this.Setup_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void Setup_Load(object sender, System.EventArgs e)
		{
			KleanTrak.Globals.LocalizzaDialog(this);

            comboBoxLingua.SelectedIndex = KleanTrak.Globals.nLinguaInUso;
            checkBoxEnable.Checked = KleanTrak.Globals.bBadgeActive;
			textBoxAddessIP.Text = KleanTrak.Globals.strBadgeAddressIP;
			textBoxPort.Text = KleanTrak.Globals.iBadgeAddressPort.ToString();
			textBoxTimeout.Text = KleanTrak.Globals.iBadgeTimeout.ToString();
			cb_sede.Items.AddRange(seeds.ToArray());
			for (int i = 0; i < cb_sede.Items.Count; i++)
			{
				if (((Query.ComboboxItem)cb_sede.Items[i]).Value == Globals.IDSEDE)
				{
					cb_sede.SelectedIndex = i;
					break;
				}
			}
		}

		private void btnChiudi_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btn_save_Click(object sender, System.EventArgs e)
		{
            try
            {
                KleanTrak.Globals.nLinguaInUso = comboBoxLingua.SelectedIndex;
                ExtModules.InteropKernel32.WritePrivateProfileString("Localizzazione", "lingua", KleanTrak.Globals.nLinguaInUso.ToString(), Application.StartupPath + "\\kleantrak.ini");

                KleanTrak.Globals.bBadgeActive = checkBoxEnable.Checked;
                ExtModules.InteropKernel32.WritePrivateProfileString("LettoreBadge", "Enable", (checkBoxEnable.Checked == true) ? "1" : "0", Application.StartupPath + "\\kleantrak.ini");

                KleanTrak.Globals.strBadgeAddressIP = textBoxAddessIP.Text;
                ExtModules.InteropKernel32.WritePrivateProfileString("LettoreBadge", "Address", textBoxAddessIP.Text, Application.StartupPath + "\\kleantrak.ini");

                KleanTrak.Globals.iBadgeAddressPort = Convert.ToUInt32(textBoxPort.Text, 10);
                ExtModules.InteropKernel32.WritePrivateProfileString("LettoreBadge", "Port", textBoxPort.Text.ToString(), Application.StartupPath + "\\kleantrak.ini");

                KleanTrak.Globals.iBadgeTimeout = Convert.ToUInt32(textBoxTimeout.Text, 10);
                ExtModules.InteropKernel32.WritePrivateProfileString("LettoreBadge", "Timeout", textBoxTimeout.Text.ToString(), Application.StartupPath + "\\kleantrak.ini");

                KleanTrak.Globals.IDSEDE = ((Query.ComboboxItem)cb_sede.SelectedItem).Value;
                ExtModules.InteropKernel32.WritePrivateProfileString("Generale", "IDSEDE", Globals.IDSEDE.ToString(), Application.StartupPath + "\\kleantrak.ini");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.Close();
		}

		private void cb_sede_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
