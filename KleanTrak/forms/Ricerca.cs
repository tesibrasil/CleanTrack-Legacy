using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using ExtModules;
using OdbcExtensions;

namespace KleanTrak
{
	/// <summary>
	/// Summary description for Ricerca.
	/// </summary>
	public class Ricerca : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBoxDispositivo;
		private System.Windows.Forms.ComboBox comboBoxTipo;
		private System.Windows.Forms.ComboBox comboBoxStato;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBoxFornitore;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button btnRicerca;
		private System.Windows.Forms.TextBox textBox_data_dismissione_A;
		private System.Windows.Forms.TextBox textBox_data_dismissione_DA;
		private System.Windows.Forms.TextBox textBox_Num_Serie;
		private System.Windows.Forms.TextBox textBox_Esami_eseguiti;
		private System.Windows.Forms.TextBox textBox_data_utilizzo_A;
		private System.Windows.Forms.TextBox textBox_data_utilizzo_DA;
		private System.Windows.Forms.TextBox textBoxCodice;
		private System.Windows.Forms.ComboBox comboBox_Esami_eseguiti;

		private ArrayList m_listTipo = null;
		private ArrayList m_listFornitore = null;
		private ArrayList m_listStato = null;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		public string strTipo_dispositivo;
		public string strFornitore;
		public int Tipo_dispositivo;
		public int Fornitore;
		public string str_stato_dispositivo;
		private System.Windows.Forms.CheckBox check_dismessi;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Button btnChiudi;
		public int Stato_dispositivo;

		public Ricerca()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ricerca));
			this.textBox_Num_Serie = new System.Windows.Forms.TextBox();
			this.textBox_data_dismissione_A = new System.Windows.Forms.TextBox();
			this.textBox_data_dismissione_DA = new System.Windows.Forms.TextBox();
			this.textBox_Esami_eseguiti = new System.Windows.Forms.TextBox();
			this.textBox_data_utilizzo_A = new System.Windows.Forms.TextBox();
			this.textBox_data_utilizzo_DA = new System.Windows.Forms.TextBox();
			this.textBoxCodice = new System.Windows.Forms.TextBox();
			this.textBoxDispositivo = new System.Windows.Forms.TextBox();
			this.comboBoxTipo = new System.Windows.Forms.ComboBox();
			this.comboBoxStato = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBoxFornitore = new System.Windows.Forms.ComboBox();
			this.comboBox_Esami_eseguiti = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.check_dismessi = new System.Windows.Forms.CheckBox();
			this.btnRicerca = new System.Windows.Forms.Button();
			this.label13 = new System.Windows.Forms.Label();
			this.btnChiudi = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBox_Num_Serie
			// 
			this.textBox_Num_Serie.Location = new System.Drawing.Point(160, 65);
			this.textBox_Num_Serie.Name = "textBox_Num_Serie";
			this.textBox_Num_Serie.Size = new System.Drawing.Size(200, 26);
			this.textBox_Num_Serie.TabIndex = 135;
			// 
			// textBox_data_dismissione_A
			// 
			this.textBox_data_dismissione_A.Location = new System.Drawing.Point(270, 275);
			this.textBox_data_dismissione_A.MaxLength = 10;
			this.textBox_data_dismissione_A.Name = "textBox_data_dismissione_A";
			this.textBox_data_dismissione_A.ReadOnly = true;
			this.textBox_data_dismissione_A.Size = new System.Drawing.Size(90, 26);
			this.textBox_data_dismissione_A.TabIndex = 134;
			// 
			// textBox_data_dismissione_DA
			// 
			this.textBox_data_dismissione_DA.Location = new System.Drawing.Point(160, 275);
			this.textBox_data_dismissione_DA.MaxLength = 10;
			this.textBox_data_dismissione_DA.Name = "textBox_data_dismissione_DA";
			this.textBox_data_dismissione_DA.ReadOnly = true;
			this.textBox_data_dismissione_DA.Size = new System.Drawing.Size(90, 26);
			this.textBox_data_dismissione_DA.TabIndex = 133;
			this.textBox_data_dismissione_DA.Tag = "€€";
			// 
			// textBox_Esami_eseguiti
			// 
			this.textBox_Esami_eseguiti.Location = new System.Drawing.Point(270, 185);
			this.textBox_Esami_eseguiti.Name = "textBox_Esami_eseguiti";
			this.textBox_Esami_eseguiti.Size = new System.Drawing.Size(90, 26);
			this.textBox_Esami_eseguiti.TabIndex = 122;
			this.textBox_Esami_eseguiti.TextChanged += new System.EventHandler(this.textBoxEsami_TextChanged);
			// 
			// textBox_data_utilizzo_A
			// 
			this.textBox_data_utilizzo_A.Location = new System.Drawing.Point(270, 215);
			this.textBox_data_utilizzo_A.MaxLength = 10;
			this.textBox_data_utilizzo_A.Name = "textBox_data_utilizzo_A";
			this.textBox_data_utilizzo_A.Size = new System.Drawing.Size(90, 26);
			this.textBox_data_utilizzo_A.TabIndex = 124;
			// 
			// textBox_data_utilizzo_DA
			// 
			this.textBox_data_utilizzo_DA.Location = new System.Drawing.Point(160, 215);
			this.textBox_data_utilizzo_DA.MaxLength = 10;
			this.textBox_data_utilizzo_DA.Name = "textBox_data_utilizzo_DA";
			this.textBox_data_utilizzo_DA.Size = new System.Drawing.Size(90, 26);
			this.textBox_data_utilizzo_DA.TabIndex = 123;
			// 
			// textBoxCodice
			// 
			this.textBoxCodice.Location = new System.Drawing.Point(160, 35);
			this.textBoxCodice.Name = "textBoxCodice";
			this.textBoxCodice.Size = new System.Drawing.Size(200, 26);
			this.textBoxCodice.TabIndex = 121;
			// 
			// textBoxDispositivo
			// 
			this.textBoxDispositivo.Location = new System.Drawing.Point(160, 5);
			this.textBoxDispositivo.Name = "textBoxDispositivo";
			this.textBoxDispositivo.Size = new System.Drawing.Size(200, 26);
			this.textBoxDispositivo.TabIndex = 120;
			// 
			// comboBoxTipo
			// 
			this.comboBoxTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxTipo.Location = new System.Drawing.Point(160, 95);
			this.comboBoxTipo.Name = "comboBoxTipo";
			this.comboBoxTipo.Size = new System.Drawing.Size(200, 26);
			this.comboBoxTipo.TabIndex = 125;
			// 
			// comboBoxStato
			// 
			this.comboBoxStato.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxStato.ItemHeight = 18;
			this.comboBoxStato.Location = new System.Drawing.Point(160, 155);
			this.comboBoxStato.Name = "comboBoxStato";
			this.comboBoxStato.Size = new System.Drawing.Size(200, 26);
			this.comboBoxStato.TabIndex = 127;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(150, 25);
			this.label1.TabIndex = 137;
			this.label1.Text = "Dispositivo";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(5, 185);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(150, 25);
			this.label2.TabIndex = 138;
			this.label2.Text = "Esami eseguiti";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBoxFornitore
			// 
			this.comboBoxFornitore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxFornitore.ItemHeight = 18;
			this.comboBoxFornitore.Location = new System.Drawing.Point(160, 125);
			this.comboBoxFornitore.Name = "comboBoxFornitore";
			this.comboBoxFornitore.Size = new System.Drawing.Size(200, 26);
			this.comboBoxFornitore.TabIndex = 126;
			// 
			// comboBox_Esami_eseguiti
			// 
			this.comboBox_Esami_eseguiti.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox_Esami_eseguiti.ItemHeight = 18;
			this.comboBox_Esami_eseguiti.Items.AddRange(new object[] {
            "Maggiore di",
            "Minore di",
            "Uguale a "});
			this.comboBox_Esami_eseguiti.Location = new System.Drawing.Point(160, 185);
			this.comboBox_Esami_eseguiti.Name = "comboBox_Esami_eseguiti";
			this.comboBox_Esami_eseguiti.Size = new System.Drawing.Size(90, 26);
			this.comboBox_Esami_eseguiti.TabIndex = 139;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(5, 215);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(150, 25);
			this.label3.TabIndex = 140;
			this.label3.Text = "Data primo utilizzo da";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(5, 35);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(150, 25);
			this.label4.TabIndex = 141;
			this.label4.Text = "Codice";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(5, 65);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(150, 25);
			this.label5.TabIndex = 142;
			this.label5.Text = "N° di serie";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(5, 155);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(150, 25);
			this.label6.TabIndex = 143;
			this.label6.Text = "Stato";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(5, 95);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(150, 25);
			this.label7.TabIndex = 144;
			this.label7.Text = "Tipo";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(5, 125);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(150, 25);
			this.label8.TabIndex = 145;
			this.label8.Text = "Fornitore";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(5, 275);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(150, 25);
			this.label9.TabIndex = 146;
			this.label9.Text = "Data dismissione da";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(873, 357);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(0, 29);
			this.label10.TabIndex = 147;
			this.label10.Text = "A";
			// 
			// label11
			// 
			this.label11.Location = new System.Drawing.Point(250, 275);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(20, 25);
			this.label11.TabIndex = 148;
			this.label11.Text = "a";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label12
			// 
			this.label12.Location = new System.Drawing.Point(5, 245);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(150, 25);
			this.label12.TabIndex = 150;
			this.label12.Text = "Dispositivo dismesso";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// check_dismessi
			// 
			this.check_dismessi.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.check_dismessi.Location = new System.Drawing.Point(160, 245);
			this.check_dismessi.Name = "check_dismessi";
			this.check_dismessi.Size = new System.Drawing.Size(26, 25);
			this.check_dismessi.TabIndex = 151;
			this.check_dismessi.CheckedChanged += new System.EventHandler(this.check_dismesso_CheckedChanged);
			// 
			// btnRicerca
			// 
			this.btnRicerca.BackColor = System.Drawing.SystemColors.Control;
			this.btnRicerca.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnRicerca.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnRicerca.Location = new System.Drawing.Point(115, 310);
			this.btnRicerca.Name = "btnRicerca";
			this.btnRicerca.Size = new System.Drawing.Size(120, 30);
			this.btnRicerca.TabIndex = 152;
			this.btnRicerca.Text = "Avvia ricerca";
			this.btnRicerca.UseVisualStyleBackColor = false;
			this.btnRicerca.Click += new System.EventHandler(this.btnRicerca_Click);
			// 
			// label13
			// 
			this.label13.Location = new System.Drawing.Point(250, 215);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(20, 25);
			this.label13.TabIndex = 152;
			this.label13.Text = "a";
			this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnChiudi
			// 
			this.btnChiudi.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnChiudi.Location = new System.Drawing.Point(240, 310);
			this.btnChiudi.Name = "btnChiudi";
			this.btnChiudi.Size = new System.Drawing.Size(120, 30);
			this.btnChiudi.TabIndex = 155;
			this.btnChiudi.Text = "Esci";
			this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
			// 
			// Ricerca
			// 
			this.ClientSize = new System.Drawing.Size(365, 345);
			this.ControlBox = false;
			this.Controls.Add(this.check_dismessi);
			this.Controls.Add(this.btnChiudi);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.textBox_data_utilizzo_DA);
			this.Controls.Add(this.btnRicerca);
			this.Controls.Add(this.textBox_data_dismissione_DA);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxDispositivo);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.textBox_data_dismissione_A);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBox_data_utilizzo_A);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textBox_Esami_eseguiti);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.comboBox_Esami_eseguiti);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.comboBoxStato);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.comboBoxFornitore);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.comboBoxTipo);
			this.Controls.Add(this.textBoxCodice);
			this.Controls.Add(this.textBox_Num_Serie);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Ricerca";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CLEAN TRACK - Ricerca Dispositivo";
			this.Load += new System.EventHandler(this.Ricerca_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void groupBox1_Enter(object sender, System.EventArgs e)
		{

		}

		private void Ricerca_Load(object sender, System.EventArgs e)
		{
			Globals.LocalizzaDialog(this);

			m_listTipo = new ArrayList();
			m_listFornitore = new ArrayList();
			m_listStato = new ArrayList();
			RiempiCombos();

		}

		private void textBoxEsami_TextChanged(object sender, System.EventArgs e)
		{

		}

		private void RiempiCombos()
		{
			comboBoxTipo.Items.Clear();
			m_listTipo.Clear();
			comboBoxTipo.Items.Add("");
			OdbcCommand cmd = null;
			OdbcDataReader rdr = null;
			try
			{
				m_listTipo.Add(0);
				string query = "SELECT ID, Descrizione FROM TipiDispositivi ORDER BY Descrizione";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					comboBoxTipo.Items.Add(rdr.GetString(rdr.GetOrdinal("Descrizione")));
					m_listTipo.Add(rdr.GetIntEx("ID"));
				}
				rdr.Close();
				comboBoxFornitore.Items.Clear();
				m_listFornitore.Clear();
				comboBoxFornitore.Items.Add("");
				m_listFornitore.Add(0);
				cmd.CommandText = "SELECT ID, Descrizione FROM Fornitori ORDER BY Descrizione";
				rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					comboBoxFornitore.Items.Add(rdr.GetStringEx("Descrizione"));
					m_listFornitore.Add(rdr.GetIntEx("ID"));
				}
				rdr.Close();
				comboBoxStato.Items.Clear();
				m_listStato.Clear();
				comboBoxStato.Items.Add("");
				m_listStato.Add(0);
				comboBoxStato.Items.Add("Pulito");
				m_listStato.Add(1);
				comboBoxStato.Items.Add("Sporco");
				m_listStato.Add(2);
				comboBoxStato.Items.Add("In lavaggio");
				m_listStato.Add(3);
			}
			catch (Exception e)
			{
				throw;
			}
			finally
			{
				if (rdr != null && !rdr.IsClosed)
					rdr.Close();
				if (cmd != null)
					cmd.Dispose();
			}
		}


		private void check_fornitore()
		{
			if (comboBoxFornitore.Text.Length > 0)
			{ strFornitore = comboBoxFornitore.Text; }
			else return;

			string strQuery_tipo_dispositivi = "SELECT ID FROM FORNITORI where DESCRIZIONE  = '" + strFornitore.ToUpper() + "'";

			OdbcConnection connTemp = DBUtil.GetODBCConnection();
			OdbcCommand command = new OdbcCommand("", connTemp);
			OdbcDataReader reader;
			command.CommandText = strQuery_tipo_dispositivi;
			Fornitore = -1;
			try
			{
				reader = command.ExecuteReader();
				if (reader.Read())
					Fornitore = reader.GetIntEx("ID");
			}
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			connTemp.Close();
		}


		private void check_tipo_dispositivo()
		{
			if (comboBoxTipo.Text.Length > 0)
			{ strTipo_dispositivo = comboBoxTipo.Text; }
			else return;

			string strQuery_tipo_dispositivi = "SELECT ID FROM TIPIDISPOSITIVI where DESCRIZIONE  = '" + strTipo_dispositivo + "'";

			OdbcConnection connTemp = DBUtil.GetODBCConnection();
			OdbcCommand command = new OdbcCommand("", connTemp);
			OdbcDataReader reader;
			command.CommandText = strQuery_tipo_dispositivi;
			Tipo_dispositivo = -1;
			try
			{
				reader = command.ExecuteReader();
				if (reader.Read())
					Tipo_dispositivo = reader.GetIntEx("ID");
			}
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			connTemp.Close();
		}



		private void check_dismesso_CheckedChanged(object sender, System.EventArgs e)
		{
			if (check_dismessi.Checked)
			{
				textBox_data_dismissione_DA.ReadOnly = false;
				textBox_data_dismissione_A.ReadOnly = false;
			}
			else if (check_dismessi.Checked == false)
			{
				textBox_data_dismissione_DA.ReadOnly = true;
				textBox_data_dismissione_A.ReadOnly = true;
			}
		}

		private void btnAnnullaRicerca_Click(object sender, System.EventArgs e)
		{

		}

		private void btnRicerca_Click(object sender, System.EventArgs e)
		{
			//avvia la ricerca dei dispositivi.

			string strWhere = "";

			string strDispositivo_nome = "";
			if (textBoxDispositivo.Text.Length > 0)
			{ strDispositivo_nome = textBoxDispositivo.Text; }

			string strRFID = "";
			if (textBoxCodice.Text.Length > 0)
			{ strRFID = textBoxCodice.Text; }

			string Num_serie = "";

			if (textBox_Num_Serie.Text.Length > 0)
			{ Num_serie = textBox_Num_Serie.Text; }

			//selezione del tipo di dispositivo dalla lista.
			strTipo_dispositivo = "";
			check_tipo_dispositivo();
			/*if (textBox_Num_Serie.Text.Length > 0)
			{strTipo_dispositivo = textBox_Num_Serie.Text;}*/

			//selezione del tipo di fornitore dalla lista.
			strFornitore = "";
			check_fornitore();

			string strEsamiEseguiti = "";
			if (textBox_Esami_eseguiti.Text.Length > 0)
			{ strEsamiEseguiti = textBox_Esami_eseguiti.Text; }

			string strInizio_utilizzo = "";
			if (textBox_data_utilizzo_DA.Text.Length > 0)
				strInizio_utilizzo = textBox_data_utilizzo_DA.Text;

			string str_data_dismissione = "";
			if (textBox_data_dismissione_DA.Text.Length > 0)
			{ str_data_dismissione = textBox_data_dismissione_DA.Text; }

			if (strDispositivo_nome != "")
				strWhere += "Dispositivi.DESCRIZIONE LIKE '%" + strDispositivo_nome.ToUpper() + "%'";

			if (strRFID != "")
			{
				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "Dispositivi.Matricola LIKE '" + strRFID.ToUpper() + "%'";
			}

			if (Num_serie != "")
			{
				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "Dispositivi.Seriale LIKE '" + Num_serie.ToUpper() + "%'";
			}

			if (comboBoxTipo.Text.Length > 0)
			{
				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "Dispositivi.IDTIPO = '" + Tipo_dispositivo + "'";
			}

			if (comboBoxFornitore.Text.Length > 0)
			{
				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "Dispositivi.IDFORNITORE = '" + Fornitore + "'";
			}

			if (comboBoxStato.Text.Length > 0)
			{
				if (comboBoxStato.Text == "Pulito")
				{ Stato_dispositivo = 1; }
				else if (comboBoxStato.Text == "Sporco")
				{ Stato_dispositivo = 2; }
				else if (comboBoxStato.Text == "In lavaggio")
				{ Stato_dispositivo = 3; }

				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "Dispositivi.STATO = '" + Stato_dispositivo + "'";
			}

			if (strEsamiEseguiti != "")
			{  //valore di esami MAGGIORE del valore inserito in strEsamiEseguiti
				if (comboBox_Esami_eseguiti.SelectedIndex == 0)
				{
					if (strWhere != "")
						strWhere += " AND ";
					strWhere += "Dispositivi.ESAMIESEGUITI > '" + strEsamiEseguiti.ToUpper() + "'";
				}
				//valore di esami MINORE del valore inserito in strEsamiEseguiti
				else if (comboBox_Esami_eseguiti.SelectedIndex == 1)
				{
					if (strWhere != "")
						strWhere += " AND ";
					strWhere += "Dispositivi.ESAMIESEGUITI < '" + strEsamiEseguiti.ToUpper() + "'";
				}
				else if (comboBox_Esami_eseguiti.SelectedIndex == 2)
				{
					if (strWhere != "")
						strWhere += " AND ";
					strWhere += "Dispositivi.ESAMIESEGUITI < '" + strEsamiEseguiti.ToUpper() + "'";
				}
				//valore di esami UGUALE al valore inserito in strEsamiEseguiti
				else
				{
					if (strWhere != "")
						strWhere += " AND ";
					strWhere += "Dispositivi.ESAMIESEGUITI = '" + strEsamiEseguiti.ToUpper() + "'";
				}
			}

			if (strInizio_utilizzo != "")
			{
				try
				{
					DateTime dtInizio = DateTime.Parse(textBox_data_utilizzo_DA.Text);
					//strInizio_utilizzo = CleanTrack.Globals.ConvertDate(dtInizio) + "000000";
					strInizio_utilizzo = KleanTrak.Globals.ConvertDate(dtInizio);
				}
				catch (FormatException)
				{
					MessageBox.Show(KleanTrak.Globals.strTable[41], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				string str_A_Inizio_utilizzo = "";
				try
				{
					if (textBox_data_utilizzo_A.Text.Length == 10)
					{
						DateTime dt_A_Inizio = DateTime.Parse(textBox_data_utilizzo_A.Text);
						//str_A_Inizio_utilizzo = CleanTrack.Globals.ConvertDate(dt_A_Inizio) + "000000";
						str_A_Inizio_utilizzo = KleanTrak.Globals.ConvertDate(dt_A_Inizio);
					}
				}
				catch
				{
					MessageBox.Show(KleanTrak.Globals.strTable[42], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				if (str_A_Inizio_utilizzo != "")
				{//verifico se viene inserito il range da data (da..a)
					if (strWhere != "")
						strWhere += " AND ";
					strWhere += "Dispositivi.DATAINIZIO >= '" + strInizio_utilizzo.ToUpper() + "' and Dispositivi.DATAINIZIO <= '" + str_A_Inizio_utilizzo.ToUpper() + "'";
				}

				if (str_A_Inizio_utilizzo == "")
				{//caso in cui NON viene inserito il range da data (da..a)
					if (strWhere != "")
						strWhere += " AND ";
					strWhere += "Dispositivi.DATAINIZIO LIKE '" + strInizio_utilizzo.ToUpper() + "%'";
				}
			}


			if (str_data_dismissione != "")
			{
				try
				{
					DateTime dtFine = DateTime.Parse(textBox_data_dismissione_DA.Text);
					str_data_dismissione = KleanTrak.Globals.ConvertDate(dtFine);
				}
				catch (FormatException)
				{
					str_data_dismissione = "";
					MessageBox.Show(KleanTrak.Globals.strTable[43], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}


				string str_data_dismissione_A = "";
				try
				{
					if (textBox_data_dismissione_A.Text.Length == 10)
					{
						DateTime dt_A_Fine = DateTime.Parse(textBox_data_dismissione_A.Text);
						str_data_dismissione_A = KleanTrak.Globals.ConvertDate(dt_A_Fine);
					}
				}
				catch
				{
					str_data_dismissione_A = "";
					MessageBox.Show(KleanTrak.Globals.strTable[44], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (str_data_dismissione_A != "" && str_data_dismissione != "")
				{
					DateTime dtFine = DateTime.Parse(textBox_data_dismissione_DA.Text);
					DateTime dt_A_Fine = DateTime.Parse(textBox_data_dismissione_A.Text);
					if (dt_A_Fine < dtFine)
					{
						MessageBox.Show(KleanTrak.Globals.strTable[45], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}

				if (str_data_dismissione_A != "")
				{//verifico se viene inserito il range da data (da..a)
					if (strWhere != "")
						strWhere += " AND ";
					strWhere += "Dispositivi.DATAFINE >= '" + str_data_dismissione.ToUpper() + "' and Dispositivi.DATAFINE <= '" + str_data_dismissione_A.ToUpper() + "'";
				}

				if (str_data_dismissione_A == "")
				{//caso in cui NON viene inserito il range da data (da..a)
					if (strWhere != "")
						strWhere += " AND ";
					strWhere += "Dispositivi.DATAFINE LIKE '" + str_data_dismissione.ToUpper() + "%'";
				}
			}

			//dismissione del dispositivo
			if (check_dismessi.Checked)
			{
				if (strWhere != "")
				{
					strWhere += " AND ";
				}
				strWhere += "Dispositivi.DISMESSO = '1'";
			}
			else
			{
				if (strWhere != "")
				{
					strWhere += " AND ";
				}
				strWhere += "Dispositivi.DISMESSO = '0'";
			}

			string strQuery = "SELECT Dispositivi.ID, Dispositivi.TAG, Dispositivi.Seriale, Dispositivi.DESCRIZIONE, Dispositivi.idrfid1, Dispositivi.MATRICOLA, TipiDispositivi.Descrizione AS TipoDispositivo, Fornitori.Descrizione AS Fornitore, STATO, ESAMIESEGUITI, DATAINIZIO, DATAFINE, DISMESSO  FROM (DISPOSITIVI LEFT OUTER JOIN TipiDispositivi ON Dispositivi.IDTipo=TipiDispositivi.ID) LEFT OUTER JOIN Fornitori ON Dispositivi.IDFornitore=Fornitori.ID ";

			if (strWhere != "")
				strQuery += " WHERE " + strWhere;
			else
			{
				MessageBox.Show(KleanTrak.Globals.strTable[46], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			strQuery += " ORDER BY Dispositivi.ID";

			KleanTrak.Globals.Query_ricerca = strQuery;

			this.Close();

		}

		private void btnChiudi_Click(object sender, System.EventArgs e)
		{
			KleanTrak.Globals.Query_ricerca = "";
			this.Close();
		}
	}
}
