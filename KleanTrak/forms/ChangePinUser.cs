using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;


namespace KleanTrak
{
	/// <summary>
	/// Descrizione di riepilogo per ChangePinUser.
	/// </summary>
	public class ChangePinUser : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnSalva;
		private System.Windows.Forms.Button btnAnnulla;
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ChangePinUser()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePinUser));
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.btnSalva = new System.Windows.Forms.Button();
			this.btnAnnulla = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(160, 5);
			this.textBox1.Name = "textBox1";
			this.textBox1.PasswordChar = '*';
			this.textBox1.Size = new System.Drawing.Size(255, 26);
			this.textBox1.TabIndex = 0;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(160, 35);
			this.textBox2.Name = "textBox2";
			this.textBox2.PasswordChar = '*';
			this.textBox2.Size = new System.Drawing.Size(255, 26);
			this.textBox2.TabIndex = 1;
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(160, 65);
			this.textBox3.Name = "textBox3";
			this.textBox3.PasswordChar = '*';
			this.textBox3.Size = new System.Drawing.Size(255, 26);
			this.textBox3.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(150, 25);
			this.label1.TabIndex = 3;
			this.label1.Text = "Vecchio PIN";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(5, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(150, 25);
			this.label2.TabIndex = 4;
			this.label2.Text = "Nuovo PIN";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(5, 65);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(150, 25);
			this.label3.TabIndex = 5;
			this.label3.Text = "Conferma nuovo PIN";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnSalva
			// 
			this.btnSalva.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSalva.Location = new System.Drawing.Point(315, 95);
			this.btnSalva.Name = "btnSalva";
			this.btnSalva.Size = new System.Drawing.Size(100, 30);
			this.btnSalva.TabIndex = 41;
			this.btnSalva.Text = "Conferma";
			this.btnSalva.Click += new System.EventHandler(this.btnSalva_Click);
			// 
			// btnAnnulla
			// 
			this.btnAnnulla.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnAnnulla.Location = new System.Drawing.Point(210, 95);
			this.btnAnnulla.Name = "btnAnnulla";
			this.btnAnnulla.Size = new System.Drawing.Size(100, 30);
			this.btnAnnulla.TabIndex = 42;
			this.btnAnnulla.Text = "Annulla";
			this.btnAnnulla.Click += new System.EventHandler(this.btnAnnulla_Click);
			// 
			// ChangePinUser
			// 
			this.ClientSize = new System.Drawing.Size(420, 130);
			this.ControlBox = false;
			this.Controls.Add(this.btnSalva);
			this.Controls.Add(this.btnAnnulla);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChangePinUser";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Modifica PIN operatore";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private void btnAnnulla_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void btnSalva_Click(object sender, System.EventArgs e)
		{
			//conferma dati:
			//1 Verifica vecchio PIN:

			OdbcConnection connTemp = DBUtil.GetODBCConnection();

			string query = "";

			query = "SELECT Matricola as Matricola_ FROM Operatori where id_utente = " + KleanTrak.Globals.m_iUserID;
			OdbcCommand commTemp = new OdbcCommand(query, connTemp);
			try
			{
				OdbcDataReader readerTemp = commTemp.ExecuteReader();

				while (readerTemp.Read())
				{
					string matricola_value;
					matricola_value = readerTemp.GetString(readerTemp.GetOrdinal("Matricola_"));
					if (matricola_value.ToString() != textBox1.Text.ToString())
					{
						MessageBox.Show("Pin errato!");
						return;
					}

				}
				if ((readerTemp != null) && (readerTemp.IsClosed == false))
					readerTemp.Close();

			}
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			//2-3 Verifica PIN:
			if (textBox2.Text.ToString() != textBox3.Text.ToString())
			{
				MessageBox.Show("Il nuovo PIN inserito è differente da quanto precedentemente indicato!");
				return;
			}

			//3/3 Verifica che un operatore non abbia già inserito lo stesso PIN utente di un altro utente già registrato
			query = "SELECT Matricola FROM Operatori where Matricola = " + textBox3.Text.ToString();
			OdbcCommand commTemp2 = new OdbcCommand(query, connTemp);
			OdbcDataReader readerTemp2 = commTemp2.ExecuteReader();
			if (readerTemp2.Read())
			{
				MessageBox.Show(KleanTrak.Globals.strTable[101], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Information);

				return;
			}
			if (readerTemp2 != null && readerTemp2.IsClosed == false)
				readerTemp2.Close();




			//Update dei dati:
			try
			{
				if (textBox2.Text.ToString() != "" && textBox3.Text.ToString() != "")
				{
					OdbcCommand commTemp3 = new OdbcCommand("UPDATE Operatori SET Matricola = '" + textBox3.Text.ToString().ToUpper() + "'  " + " WHERE id_utente = " + KleanTrak.Globals.m_iUserID, connTemp);
					commTemp3.ExecuteNonQuery();
				}
				else { return; }

			}
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			connTemp.Close();

			MessageBox.Show("Cambio PIN Operatore effettuato!");
			this.Close();
		}
	}
}
