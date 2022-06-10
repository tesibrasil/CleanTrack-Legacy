using OdbcExtensions;
using System;
using System.Data.Odbc;
using System.Windows.Forms;

namespace KleanTrak
{
	partial class DismissionList : Form
	{
		private System.ComponentModel.IContainer components = null;
		private Button btnStampa = new Button();
		private Button btnChiudi = new Button();

		private string iddispositivo, matricola = "";
		private ColumnHeader columnDismissione;
		private ColumnHeader columnData;
		private ColumnHeader columnCausale;
		private ColumnHeader columnUtente;
		private ListViewEx.ListViewEx listView;

		public DismissionList(string iddispositivo, string matricola)
		{
			this.iddispositivo = iddispositivo;
			this.matricola = matricola;

			InitializeComponent();
			// InitializeListView();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DismissionList));
			this.btnStampa = new System.Windows.Forms.Button();
			this.btnChiudi = new System.Windows.Forms.Button();
			this.listView = new ListViewEx.ListViewEx();
			this.columnDismissione = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnData = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnCausale = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnUtente = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.SuspendLayout();
			// 
			// btnStampa
			// 
			this.btnStampa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStampa.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnStampa.Location = new System.Drawing.Point(400, 310);
			this.btnStampa.Name = "btnStampa";
			this.btnStampa.Size = new System.Drawing.Size(100, 30);
			this.btnStampa.TabIndex = 122;
			this.btnStampa.Text = "Stampa";
			this.btnStampa.Click += new System.EventHandler(this.btnStampa_Click);
			// 
			// btnChiudi
			// 
			this.btnChiudi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnChiudi.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnChiudi.Location = new System.Drawing.Point(505, 310);
			this.btnChiudi.Name = "btnChiudi";
			this.btnChiudi.Size = new System.Drawing.Size(100, 30);
			this.btnChiudi.TabIndex = 37;
			this.btnChiudi.Text = "Esci";
			this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.BackColor = System.Drawing.SystemColors.Window;
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnDismissione,
            this.columnData,
            this.columnCausale,
            this.columnUtente});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(5, 5);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(600, 300);
			this.listView.TabIndex = 123;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			// 
			// columnDismissione
			// 
			this.columnDismissione.Text = "Operazione";
			this.columnDismissione.Width = 140;
			// 
			// columnData
			// 
			this.columnData.Text = "Data";
			this.columnData.Width = 140;
			// 
			// columnCausale
			// 
			this.columnCausale.Text = "Causale";
			this.columnCausale.Width = 140;
			// 
			// columnUtente
			// 
			this.columnUtente.Text = "Utente";
			this.columnUtente.Width = 90;
			// 
			// DismissionList
			// 
			this.ClientSize = new System.Drawing.Size(610, 345);
			this.Controls.Add(this.listView);
			this.Controls.Add(this.btnChiudi);
			this.Controls.Add(this.btnStampa);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DismissionList";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.Dismission_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnChiudi_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void Dismission_Load(object sender, System.EventArgs e)
		{
			KleanTrak.Globals.LocalizzaDialog(this);
			this.Text = "CLEAN TRACK - Registro di Dismissione e Ripristino ";

			this.Text = this.Text + "- Dispositivo " + this.matricola;

			RiempiLista();
		}

		private void RiempiLista()
		{
			string query = "SELECT ID,DISMISSIONE,DATA,CAUSALE,UTENTE FROM STORICODISMISSIONE WHERE ELIMINATO=0 AND IDDISPOSITIVO=" + iddispositivo + " ORDER BY DATA DESC";

			OdbcConnection connTemp = DBUtil.GetODBCConnection();
			OdbcCommand commTemp = new OdbcCommand(query, connTemp);
			OdbcDataReader readerTemp = null;

			try
			{
				readerTemp = commTemp.ExecuteReader();

				ListViewItem lvItem;
				while (readerTemp.Read())
				{
					int dismix =readerTemp.GetIntEx("DISMISSIONE");
					string disData = dismix == 1 ? "DISMISSIONE" : "RIPRISTINO";
					lvItem = listView.Items.Add(disData);

					lvItem.Tag = readerTemp.GetIntEx("ID");

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("DATA")))
					{
						lvItem.SubItems.Add("");
					}
					else
					{
						string data = readerTemp.GetString(readerTemp.GetOrdinal("DATA"));
						lvItem.SubItems.Add(data.Substring(6, 2) + "/" + data.Substring(4, 2) + "/" + data.Substring(0, 4) + " " + data.Substring(8, 2) + ":" + data.Substring(10, 2));

					}
					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("CAUSALE")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("CAUSALE")));

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("UTENTE")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("UTENTE")));
				}
			}
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			if (readerTemp != null && readerTemp.IsClosed == false)
				readerTemp.Close();

			connTemp.Close();

			listView.SetReadOnly(true);

			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		private void btnStampa_Click(object sender, EventArgs e)
		{
			listView.PrintList(this.Text, "Cleantrack 5.1.0             © Tesi Elettronica e Sistemi Informativi SPA");
		}
	}
}
