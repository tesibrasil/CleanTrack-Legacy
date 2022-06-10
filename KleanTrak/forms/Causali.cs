using OdbcExtensions;
using System;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

namespace KleanTrak
{
	/// <summary>
	/// Descrizione di riepilogo per Causali.
	/// </summary>
	public class Causali : System.Windows.Forms.Form
    {
        private ListViewEx.ListViewEx listView;
        private ColumnHeader columnHeader2;
		private ToolStripContainer tscontainer;
		private ToolStrip ts_main;
		private ToolStripButton tsb_add;
		private ToolStripButton tsb_delete;
		private ToolStripButton tsb_close;

		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Causali()
		{
			InitializeComponent();
			listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetEndEditCallback(new ListViewEx.ListViewEx.EndEditingCallbackEdit(EndEditCausale_Causali), null, null, null);
			listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetEndEditCallback(new ListViewEx.ListViewEx.EndEditingCallbackEdit(EndEditCausale_Causali), null, null, null);
			tsb_add.ToolTipText = Globals.strTable[174];
			tsb_delete.ToolTipText = Globals.strTable[175];
			tsb_close.ToolTipText = Globals.strTable[155];
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Causali));
			this.listView = new ListViewEx.ListViewEx();
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tscontainer = new System.Windows.Forms.ToolStripContainer();
			this.ts_main = new System.Windows.Forms.ToolStrip();
			this.tsb_add = new System.Windows.Forms.ToolStripButton();
			this.tsb_delete = new System.Windows.Forms.ToolStripButton();
			this.tsb_close = new System.Windows.Forms.ToolStripButton();
			this.tscontainer.ContentPanel.SuspendLayout();
			this.tscontainer.TopToolStripPanel.SuspendLayout();
			this.tscontainer.SuspendLayout();
			this.ts_main.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(500, 316);
			this.listView.TabIndex = 44;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
			this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Causali di dismissione";
			this.columnHeader2.Width = 538;
			// 
			// tscontainer
			// 
			// 
			// tscontainer.ContentPanel
			// 
			this.tscontainer.ContentPanel.Controls.Add(this.listView);
			this.tscontainer.ContentPanel.Size = new System.Drawing.Size(510, 336);
			this.tscontainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tscontainer.Location = new System.Drawing.Point(0, 0);
			this.tscontainer.Name = "tscontainer";
			this.tscontainer.Size = new System.Drawing.Size(510, 395);
			this.tscontainer.TabIndex = 49;
			this.tscontainer.Text = "toolStripContainer1";
			// 
			// tscontainer.TopToolStripPanel
			// 
			this.tscontainer.TopToolStripPanel.Controls.Add(this.ts_main);
			// 
			// ts_main
			// 
			this.ts_main.Dock = System.Windows.Forms.DockStyle.None;
			this.ts_main.ImageScalingSize = new System.Drawing.Size(52, 52);
			this.ts_main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_add,
            this.tsb_delete,
            this.tsb_close});
			this.ts_main.Location = new System.Drawing.Point(3, 0);
			this.ts_main.Name = "ts_main";
			this.ts_main.Size = new System.Drawing.Size(180, 59);
			this.ts_main.TabIndex = 0;
			// 
			// tsb_add
			// 
			this.tsb_add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_add.Image = global::kleanTrak.Properties.Resources.add;
			this.tsb_add.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_add.Name = "tsb_add";
			this.tsb_add.Size = new System.Drawing.Size(56, 56);
			this.tsb_add.Text = "toolStripButton1";
			this.tsb_add.Click += new System.EventHandler(this.tsb_add_Click);
			// 
			// tsb_delete
			// 
			this.tsb_delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_delete.Image = global::kleanTrak.Properties.Resources.delete;
			this.tsb_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_delete.Name = "tsb_delete";
			this.tsb_delete.Size = new System.Drawing.Size(56, 56);
			this.tsb_delete.Text = "toolStripButton2";
			this.tsb_delete.Click += new System.EventHandler(this.tsb_delete_Click);
			// 
			// tsb_close
			// 
			this.tsb_close.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_close.Image = global::kleanTrak.Properties.Resources.close;
			this.tsb_close.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_close.Name = "tsb_close";
			this.tsb_close.Size = new System.Drawing.Size(56, 56);
			this.tsb_close.Text = "toolStripButton1";
			this.tsb_close.Click += new System.EventHandler(this.tsb_close_Click);
			// 
			// Causali
			// 
			this.ClientSize = new System.Drawing.Size(510, 395);
			this.ControlBox = false;
			this.Controls.Add(this.tscontainer);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Causali";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CLEAN TRACK - Causali di dismissione";
			this.Load += new System.EventHandler(this.Causali_Load);
			this.ClientSizeChanged += new System.EventHandler(this.Causali_ClientSizeChanged);
			this.tscontainer.ContentPanel.ResumeLayout(false);
			this.tscontainer.TopToolStripPanel.ResumeLayout(false);
			this.tscontainer.TopToolStripPanel.PerformLayout();
			this.tscontainer.ResumeLayout(false);
			this.tscontainer.PerformLayout();
			this.ts_main.ResumeLayout(false);
			this.ts_main.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion
		private bool EndEditCausale_Causali(int iItemNum, int iSubitemNum, string strText)
		{
			bool bReturn = false;

			OdbcConnection connTemp = DBUtil.GetODBCConnection();
			OdbcCommand commTemp = new OdbcCommand("", connTemp);

			if (strText == "Nuova Causale") //nel caso in cui non venga editato nulla, elimino la riga corrispondente
			{
				commTemp = new OdbcCommand("Delete from Causali WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
				commTemp.ExecuteNonQuery();
				RiempiLista();
				return bReturn;
			}

			switch (iSubitemNum)
			{
				case 0:
					{
						commTemp = new OdbcCommand("UPDATE Causali SET Causale = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[listView.SelectedIndices[0]].Tag.ToString(), connTemp);

						//INSERIMENTO NEL LOG DI SISTEMA
						//valore originale
						string ValoreOriginale = "";
						OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Causale as Causale FROM Causali WHERE ID=" + listView.Items[listView.SelectedIndices[0]].Tag.ToString(), connTemp);
						OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
						if (readerValoreOriginale.Read())
						{
							if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Causale")))
							{
								ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Causale")).ToString();

								if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
									readerValoreOriginale.Close();
							}
						}

						if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
						{ break; }

						OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
									" VALUES ('" + KleanTrak.Globals.m_strUser + "', 'CAUSALI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "', 'CAUSALE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
						commTemp_LOG.ExecuteNonQuery();

						break;
					}
			}
			try
			{
				commTemp.ExecuteNonQuery();
				bReturn = true;
			}
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			connTemp.Close();
			return bReturn;
		}
		private void Causali_Load(object sender, System.EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Globals.LocalizzaDialog(this);
				CaricaEtichette();
				RiempiLista();
				WindowState = FormWindowState.Maximized;
				MinimumSize = Size;
				Globals.ResizeList(this, listView);
				tsb_delete.Visible = false;
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}
		private void RiempiLista()
		{
			listView.Items.Clear();

			OdbcConnection connTemp = DBUtil.GetODBCConnection();

			OdbcCommand commTemp = new OdbcCommand("SELECT * FROM Causali ORDER BY ID", connTemp);
			OdbcDataReader readerTemp = commTemp.ExecuteReader();

			ListViewItem lvItem;
			while (readerTemp.Read())
			{
				lvItem = listView.Items.Add(readerTemp.GetString(readerTemp.GetOrdinal("Causale")));
				lvItem.Tag = readerTemp.GetIntEx("ID");
			}

			if ((readerTemp != null) && (readerTemp.IsClosed == false))
				readerTemp.Close();

			connTemp.Close();

			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}
		private void CaricaEtichette()
		{
			OdbcConnection connTemp = new OdbcConnection(KleanTrak.Globals.strDictionary);
			connTemp.Open();

			OdbcCommand commTemp = new OdbcCommand("SELECT * FROM Dictionary ", connTemp);
			OdbcDataReader readerTemp = commTemp.ExecuteReader();
			while (readerTemp.Read())
			{
			}

			if ((readerTemp != null) && (readerTemp.IsClosed == false))
				readerTemp.Close();

			if (connTemp.State == ConnectionState.Open)
				connTemp.Close();
		}
		private void listView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Delete && (listView.SelectedIndices.Count == 0))
				return;
			tsb_delete_Click(null, null);
		}

		private void Causali_ClientSizeChanged(object sender, EventArgs e) => Globals.ResizeList(this, listView);

		private void tsb_add_Click(object sender, EventArgs e)
		{
			OdbcCommand cmd = null;
			try
			{
				string query = $"INSERT INTO CAUSALI (CAUSALE) VALUES ('NUOVA CAUSALE')";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				cmd.ExecuteNonQuery();
				DBUtil.InsertDbLog("CAUSALI", DBUtil.LogOperation.Insert, 
					cmd.GetMaxKeyValue("CAUSALI", "ID"), "CAUSALE", "NUOVA CAUSALE");
				RiempiLista();
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}
		private void tsb_close_Click(object sender, EventArgs e) => Close();
		private void listView_SelectedIndexChanged(object sender, EventArgs e) => tsb_delete.Visible = (listView.SelectedIndices.Count > 0);

		private void tsb_delete_Click(object sender, EventArgs e)
		{
			try
			{
				if (MessageBox.Show(KleanTrak.Globals.strTable[2], "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return;
				OdbcConnection connTemp = DBUtil.GetODBCConnection();
				//verifica che la sterilizzatrice non sia già stata utilizzata
				OdbcCommand commTemp_check = new OdbcCommand("SELECT Dispositivi.idcausaledismissione from Dispositivi " +
					"LEFT OUTER JOIN Causali ON Dispositivi.idcausaledismissione = Causali.ID " +
							"where Causali.ID  = " + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "", connTemp);
				OdbcDataReader readerTemp = commTemp_check.ExecuteReader();
				if (readerTemp.Read())
				{
					MessageBox.Show(KleanTrak.Globals.strTable[3], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (readerTemp != null && !readerTemp.IsClosed)
					readerTemp.Close();
				OdbcCommand commTemp = new OdbcCommand("DELETE FROM Causali WHERE ID=" + listView.Items[listView.SelectedIndices[0]].Tag.ToString(), connTemp);
				commTemp.ExecuteNonQuery();
				//LOG DI SISTEMA
				OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD ) VALUES ('" + KleanTrak.Globals.m_strUser + "', 'CAUSALI', 'Cancellazione', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "')", connTemp);
				commTemp_LOG.ExecuteNonQuery();
				listView.Items.RemoveAt(listView.SelectedIndices[0]);
				connTemp.Close();
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
			}
		}
	}
}
