using OdbcExtensions;
using System;
using System.Data.Odbc;
using System.Drawing;
using System.Windows.Forms;


namespace KleanTrak
{
	/// <summary>
	/// Descrizione di riepilogo per LogOperazioni.
	/// </summary>
	public class LogOperazioni : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ColumnHeader columnTransazione;
		private System.Windows.Forms.ColumnHeader columnUtente;
		private System.Windows.Forms.ColumnHeader columnTipoTransaz;
		private System.Windows.Forms.ColumnHeader columnRecord;
		private System.Windows.Forms.ColumnHeader columnNomeCampo;
		private System.Windows.Forms.ColumnHeader columnValOriginale;
		private System.Windows.Forms.ColumnHeader columnValModificato;
		private System.Windows.Forms.Button btnChiudi;
		private ListViewEx.ListViewEx listView;
		private System.Windows.Forms.ColumnHeader columnTabella;
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LogOperazioni()
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogOperazioni));
			this.listView = new ListViewEx.ListViewEx();
			this.columnTransazione = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnUtente = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnTipoTransaz = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnRecord = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnTabella = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnNomeCampo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnValOriginale = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnValModificato = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnChiudi = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTransazione,
            this.columnUtente,
            this.columnTipoTransaz,
            this.columnRecord,
            this.columnTabella,
            this.columnNomeCampo,
            this.columnValOriginale,
            this.columnValModificato});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.Location = new System.Drawing.Point(5, 5);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(975, 515);
			this.listView.TabIndex = 0;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			// 
			// columnTransazione
			// 
			this.columnTransazione.Text = "Data transazione";
			this.columnTransazione.Width = 121;
			// 
			// columnUtente
			// 
			this.columnUtente.Text = "Utente";
			this.columnUtente.Width = 87;
			// 
			// columnTipoTransaz
			// 
			this.columnTipoTransaz.Text = "Tipo transazione";
			this.columnTipoTransaz.Width = 150;
			// 
			// columnRecord
			// 
			this.columnRecord.Text = "Elemento";
			this.columnRecord.Width = 117;
			// 
			// columnTabella
			// 
			this.columnTabella.Text = "Tabella";
			this.columnTabella.Width = 104;
			// 
			// columnNomeCampo
			// 
			this.columnNomeCampo.Text = "Nome campo";
			this.columnNomeCampo.Width = 132;
			// 
			// columnValOriginale
			// 
			this.columnValOriginale.Text = "Valore originale";
			this.columnValOriginale.Width = 155;
			// 
			// columnValModificato
			// 
			this.columnValModificato.Text = "Valore modificato";
			this.columnValModificato.Width = 181;
			// 
			// btnChiudi
			// 
			this.btnChiudi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnChiudi.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnChiudi.Location = new System.Drawing.Point(880, 525);
			this.btnChiudi.Name = "btnChiudi";
			this.btnChiudi.Size = new System.Drawing.Size(100, 30);
			this.btnChiudi.TabIndex = 38;
			this.btnChiudi.Text = "Esci";
			this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(5, 525);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 30);
			this.button1.TabIndex = 39;
			this.button1.Text = "Stampa";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// LogOperazioni
			// 
			this.ClientSize = new System.Drawing.Size(984, 561);
			this.ControlBox = false;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btnChiudi);
			this.Controls.Add(this.listView);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "LogOperazioni";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CLEAN TRACK - Log Transazioni";
			this.Load += new System.EventHandler(this.LogOperazioni_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnChiudi_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void LogOperazioni_Load(object sender, System.EventArgs e)
		{
			KleanTrak.Globals.LocalizzaDialog(this);

			Riempilista();

			WindowState = FormWindowState.Maximized;
			MinimumSize = Size;
		}

		private void Riempilista()
		{
			string strQuery = "SELECT * FROM LOG";

			if (KleanTrak.Globals.Query_log_Transazioni != "")
				strQuery += " WHERE " + KleanTrak.Globals.Query_log_Transazioni;

			if (KleanTrak.Globals.log_OrderBy != "")
				strQuery += " ORDER BY " + KleanTrak.Globals.log_OrderBy;

			//compongo la query per il LOG transazioni

			OdbcConnection connTemp = DBUtil.GetODBCConnection();
			OdbcCommand commTemp = new OdbcCommand(strQuery, connTemp);
			OdbcDataReader readerTemp = commTemp.ExecuteReader();

			ListViewItem lvItem;
			while (readerTemp.Read())
			{

				if (readerTemp.IsDBNull(readerTemp.GetOrdinal("DATA")))
					lvItem = listView.Items.Add("");
				else
					lvItem = listView.Items.Add(KleanTrak.Globals.ConvertDateTime(readerTemp.GetString(readerTemp.GetOrdinal("DATA"))));

				if (readerTemp.IsDBNull(readerTemp.GetOrdinal("UTENTE")))
					lvItem.SubItems.Add("");
				else
					lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("UTENTE")));

				if (readerTemp.IsDBNull(readerTemp.GetOrdinal("OPERAZIONE")))
					lvItem.SubItems.Add("");
				else
					lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("OPERAZIONE")));

				int AA = -1;
				if (readerTemp.IsDBNull(readerTemp.GetOrdinal("RECORD")))
					lvItem.SubItems.Add("");
				else
					AA = readerTemp.GetIntEx("RECORD"); 
				lvItem.SubItems.Add(AA.ToString());

				if (readerTemp.IsDBNull(readerTemp.GetOrdinal("TABELLA")))
					lvItem.SubItems.Add("");
				else
					lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("TABELLA")));


				if (readerTemp.IsDBNull(readerTemp.GetOrdinal("NOMECAMPO")))
					lvItem.SubItems.Add("");
				else
					lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("NOMECAMPO")));


				if (readerTemp.IsDBNull(readerTemp.GetOrdinal("VALOREORIGINALE")))
				{
					lvItem.SubItems.Add("");
				}
				else
				{
					if (readerTemp.GetString(readerTemp.GetOrdinal("VALOREORIGINALE")).ToString().Length == 14)
					{
						string strDataInizio = "";
						strDataInizio = KleanTrak.Globals.ConvertDateTime(readerTemp.GetString(readerTemp.GetOrdinal("VALOREORIGINALE")));
						lvItem.SubItems.Add(strDataInizio);
					}
					else
					{
						lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("VALOREORIGINALE")));
					}
				}

				if (readerTemp.IsDBNull(readerTemp.GetOrdinal("VALOREMODIFICATO")))
				{
					lvItem.SubItems.Add("");
				}
				else
				{
					if (readerTemp.GetString(readerTemp.GetOrdinal("VALOREMODIFICATO")).ToString().Length == 14)
					{
						string strDataInizio = "";
						strDataInizio = KleanTrak.Globals.ConvertDateTime(readerTemp.GetString(readerTemp.GetOrdinal("VALOREMODIFICATO")));
						lvItem.SubItems.Add(strDataInizio);
					}
					else
					{
						lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("VALOREMODIFICATO")));
					}
				}
			}
			if ((readerTemp != null) && (readerTemp.IsClosed == false))
				readerTemp.Close();

			connTemp.Close();

			// Sandro 30/03/2017 //

			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			listView.PrintList("LOG DELLE TRANSAZIONI", "Azienda Ospedaliera di Monselice");
		}
	}
}
