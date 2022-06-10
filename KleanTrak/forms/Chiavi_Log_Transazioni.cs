using System;
using System.Windows.Forms;

namespace KleanTrak
{
	/// <summary>
	/// Descrizione di riepilogo per Chiavi_Log_Transazioni.
	/// </summary>
	public class Chiavi_Log_Transazioni : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox comboBox4;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox comboBox5;
		private System.Windows.Forms.Button btnChiudi;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBoxUtente;
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Chiavi_Log_Transazioni()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Chiavi_Log_Transazioni));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textBoxUtente = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label9 = new System.Windows.Forms.Label();
			this.comboBox5 = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.comboBox4 = new System.Windows.Forms.ComboBox();
			this.btnChiudi = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textBoxUtente);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.textBox3);
			this.groupBox1.Controls.Add(this.textBox4);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textBox2);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.comboBox2);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.comboBox1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(5, 5);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(580, 185);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Chiavi di selezione";
			// 
			// textBoxUtente
			// 
			this.textBoxUtente.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBoxUtente.Location = new System.Drawing.Point(160, 150);
			this.textBoxUtente.Name = "textBoxUtente";
			this.textBoxUtente.Size = new System.Drawing.Size(200, 26);
			this.textBoxUtente.TabIndex = 14;
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(5, 150);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(150, 25);
			this.label7.TabIndex = 12;
			this.label7.Text = "Utente";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(365, 120);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(20, 25);
			this.label5.TabIndex = 11;
			this.label5.Text = "a";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textBox3
			// 
			this.textBox3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox3.Location = new System.Drawing.Point(390, 120);
			this.textBox3.MaxLength = 10;
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(184, 26);
			this.textBox3.TabIndex = 10;
			// 
			// textBox4
			// 
			this.textBox4.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox4.Location = new System.Drawing.Point(160, 120);
			this.textBox4.MaxLength = 10;
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(200, 26);
			this.textBox4.TabIndex = 9;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(5, 120);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(150, 25);
			this.label6.TabIndex = 8;
			this.label6.Text = "Data transazione da";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(365, 90);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(20, 25);
			this.label4.TabIndex = 7;
			this.label4.Text = "a";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// textBox2
			// 
			this.textBox2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox2.Location = new System.Drawing.Point(390, 90);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(184, 26);
			this.textBox2.TabIndex = 6;
			// 
			// textBox1
			// 
			this.textBox1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox1.Location = new System.Drawing.Point(160, 90);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(200, 26);
			this.textBox1.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(5, 90);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(150, 25);
			this.label3.TabIndex = 4;
			this.label3.Text = "Codice elemento da";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBox2
			// 
			this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboBox2.Items.AddRange(new object[] {
            "",
            "Inserimento",
            "Modifica",
            "Cancellazione"});
			this.comboBox2.Location = new System.Drawing.Point(160, 60);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(200, 26);
			this.comboBox2.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(5, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(150, 25);
			this.label2.TabIndex = 2;
			this.label2.Text = "Tipo transazione";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboBox1.Items.AddRange(new object[] {
            "",
            "Dispositivi",
            "Operatori sterilizzazione",
            "Sterilizzatrici",
            "Tipi di dispositivi",
            "Fornitori"});
			this.comboBox1.Location = new System.Drawing.Point(160, 30);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(200, 26);
			this.comboBox1.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(5, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(150, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "Classe elementi";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.comboBox5);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.comboBox4);
			this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(5, 195);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(580, 95);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Ordinamento";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(5, 60);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(150, 25);
			this.label9.TabIndex = 16;
			this.label9.Text = "Seconda chiave";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBox5
			// 
			this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox5.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboBox5.Items.AddRange(new object[] {
            "",
            "Classe elemento",
            "Tipo transazione",
            "Data transazione",
            "Utente"});
			this.comboBox5.Location = new System.Drawing.Point(160, 60);
			this.comboBox5.Name = "comboBox5";
			this.comboBox5.Size = new System.Drawing.Size(200, 26);
			this.comboBox5.TabIndex = 17;
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(5, 30);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(150, 25);
			this.label8.TabIndex = 14;
			this.label8.Text = "Prima chiave";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// comboBox4
			// 
			this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox4.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.comboBox4.Items.AddRange(new object[] {
            "",
            "Classe elemento",
            "Tipo transazione",
            "Data transazione",
            "Utente"});
			this.comboBox4.Location = new System.Drawing.Point(160, 30);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new System.Drawing.Size(200, 26);
			this.comboBox4.TabIndex = 15;
			// 
			// btnChiudi
			// 
			this.btnChiudi.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnChiudi.Location = new System.Drawing.Point(485, 295);
			this.btnChiudi.Name = "btnChiudi";
			this.btnChiudi.Size = new System.Drawing.Size(100, 30);
			this.btnChiudi.TabIndex = 39;
			this.btnChiudi.Text = "Esci";
			this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(5, 295);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 30);
			this.button1.TabIndex = 52;
			this.button1.Text = "Cerca";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Chiavi_Log_Transazioni
			// 
			this.ClientSize = new System.Drawing.Size(590, 330);
			this.ControlBox = false;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnChiudi);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Chiavi_Log_Transazioni";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CLEAN TRACK - Log Transazioni";
			this.Load += new System.EventHandler(this.Chiavi_Log_Transazioni_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnChiudi_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			//compongo la query per il LOG transazioni
			string strWhere = "";

			string tipo_operazione = "";
			if (comboBox2.SelectedIndex == 1)
			{ tipo_operazione = "Inserimento"; }
			else if (comboBox2.SelectedIndex == 2)
			{ tipo_operazione = "Modifica"; }
			else if (comboBox2.SelectedIndex == 3)
			{ tipo_operazione = "Cancellazione"; }

			string classe_elemento = "";
			if (comboBox1.SelectedIndex == 1)
			{ classe_elemento = "DISPOSITIVI"; }
			else if (comboBox1.SelectedIndex == 2)
			{ classe_elemento = "OPERATORI"; }
			else if (comboBox1.SelectedIndex == 3)
			{ classe_elemento = "STERILIZZATRICI"; }
			else if (comboBox1.SelectedIndex == 4)
			{ classe_elemento = "TIPI DISPOSITIVI"; }
			else if (comboBox1.SelectedIndex == 5)
			{ classe_elemento = "FORNITORI"; }

			string record_da = "";
			if (textBox1.Text.Length > 0)
			{ record_da = textBox1.Text; }

			string record_a = "";
			if (textBox2.Text.Length > 0)
			{ record_a = textBox2.Text; }

			string utente = "";
			if (textBoxUtente.Text.Length > 0)
			{ utente = textBoxUtente.Text.ToUpper(); }

			string transazione_da_string = "";
			DateTime transazione_da;
			if (textBox4.Text.Length == 10)
			{
				try
				{
					transazione_da = DateTime.Parse(textBox4.Text);
					//transazione_da_string = CleanTrack.Globals.ConvertDate(transazione_da) + "000000";
					transazione_da_string = KleanTrak.Globals.ConvertDate(transazione_da);
				}
				catch (FormatException)
				{
					transazione_da_string = "";
					MessageBox.Show(KleanTrak.Globals.strTable[43], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

			}

			string transazione_a_string = "";
			DateTime transazione_a;
			if (textBox3.Text.Length == 10)
			{
				try
				{
					transazione_a = DateTime.Parse(textBox3.Text);
					//transazione_a_string = CleanTrack.Globals.ConvertDate(transazione_a) + "000000";
					transazione_a_string = KleanTrak.Globals.ConvertDate(transazione_a);
				}
				catch (FormatException)
				{
					transazione_a_string = "";
					MessageBox.Show(KleanTrak.Globals.strTable[43], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}

			if (tipo_operazione != "")
				strWhere += "operazione = '" + tipo_operazione + "'";

			if (classe_elemento != "")
			{
				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "tabella = '" + classe_elemento + "'";
			}

			if (record_da != "")
			{
				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "record >= '" + record_da + "'";
			}

			if (record_a != "")
			{
				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "record <= '" + record_a + "'";
			}

			if (utente != "")
			{
				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "utente = '" + utente + "'";
			}

			if (transazione_da_string != "")
			{
				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "data >= '" + transazione_da_string + "'";
			}

			if (transazione_a_string != "")
			{
				if (strWhere != "")
					strWhere += " AND ";
				strWhere += "data <='" + transazione_a_string + "'";
			}

			if (strWhere == "")
			{
				if (MessageBox.Show(KleanTrak.Globals.strTable[92], "Clean Track", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
				{
					KleanTrak.Globals.Query_log_Transazioni = strWhere.ToString();
				}
				else
					return;
			}
			else if (strWhere != "")
			{
				KleanTrak.Globals.Query_log_Transazioni = strWhere.ToString();
			}

			//composizione ORDER BY

			string strOrderBy = "";
			string prima_chiave = "";
			if (comboBox4.SelectedIndex == 1)
			{ prima_chiave = "TABELLA "; }
			else if (comboBox4.SelectedIndex == 2)
			{ prima_chiave = "OPERAZIONE "; }
			else if (comboBox4.SelectedIndex == 3)
			{ prima_chiave = "DATA "; }
			else if (comboBox4.SelectedIndex == 4)
			{ prima_chiave = "UTENTE "; }

			string seconda_chiave = "";
			if (comboBox5.SelectedIndex == 1)
			{ seconda_chiave = "TABELLA "; }
			else if (comboBox5.SelectedIndex == 2)
			{ seconda_chiave = "OPERAZIONE "; }
			else if (comboBox5.SelectedIndex == 3)
			{ seconda_chiave = "DATA "; }
			else if (comboBox5.SelectedIndex == 4)
			{ seconda_chiave = "UTENTE "; }

			if (prima_chiave != "")
				strOrderBy += "" + prima_chiave + "";

			if (seconda_chiave != "")
			{
				if (strOrderBy != "" && seconda_chiave != prima_chiave)
				{
					strOrderBy += " , ";
					strOrderBy += "" + seconda_chiave + "";
				}
			}

			KleanTrak.Globals.log_OrderBy = strOrderBy.ToString();
			///////

			LogOperazioni dlg = new LogOperazioni();
			dlg.ShowDialog();
		}

		private void Chiavi_Log_Transazioni_Load(object sender, System.EventArgs e)
		{
			Globals.LocalizzaDialog(this);
		}
	}
}

