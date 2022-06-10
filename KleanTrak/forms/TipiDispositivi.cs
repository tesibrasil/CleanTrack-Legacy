using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using OdbcExtensions;

namespace KleanTrak
{
	public class TipiDispositivi : System.Windows.Forms.Form
	{
		private System.ComponentModel.Container components = null;
		private ToolStripPanel BottomToolStripPanel;
		private ToolStripPanel TopToolStripPanel;
		private ToolStrip main_toolstrip;
		private ToolStripButton tsb_add;
		private ToolStripButton tsb_delete;
		private ToolStripButton tsb_close;
		private ToolStripPanel RightToolStripPanel;
		private ToolStripPanel LeftToolStripPanel;
		private ToolStripContentPanel ContentPanel;
		private ListViewEx.ListViewEx listView;
		private ColumnHeader columnTipoDispositivo;
		private ToolStripContainer tscontainer;
		public int riempilista_nuovo = 0;
        
		public TipiDispositivi()
		{
			InitializeComponent();
			listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetEndEditCallback(new ListViewEx.ListViewEx.EndEditingCallbackEdit(EndEditTipo_dispositivi), null, null, null);
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
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TipiDispositivi));
			this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.main_toolstrip = new System.Windows.Forms.ToolStrip();
			this.tsb_add = new System.Windows.Forms.ToolStripButton();
			this.tsb_delete = new System.Windows.Forms.ToolStripButton();
			this.tsb_close = new System.Windows.Forms.ToolStripButton();
			this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
			this.listView = new ListViewEx.ListViewEx();
			this.columnTipoDispositivo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tscontainer = new System.Windows.Forms.ToolStripContainer();
			this.main_toolstrip.SuspendLayout();
			this.tscontainer.ContentPanel.SuspendLayout();
			this.tscontainer.TopToolStripPanel.SuspendLayout();
			this.tscontainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// BottomToolStripPanel
			// 
			this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.BottomToolStripPanel.Name = "BottomToolStripPanel";
			this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// TopToolStripPanel
			// 
			this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.TopToolStripPanel.Name = "TopToolStripPanel";
			this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// main_toolstrip
			// 
			this.main_toolstrip.Dock = System.Windows.Forms.DockStyle.None;
			this.main_toolstrip.ImageScalingSize = new System.Drawing.Size(52, 52);
			this.main_toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_add,
            this.tsb_delete,
            this.tsb_close});
			this.main_toolstrip.Location = new System.Drawing.Point(3, 0);
			this.main_toolstrip.Name = "main_toolstrip";
			this.main_toolstrip.Size = new System.Drawing.Size(180, 59);
			this.main_toolstrip.TabIndex = 0;
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
			this.tsb_delete.Text = "toolStripButton1";
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
			// RightToolStripPanel
			// 
			this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.RightToolStripPanel.Name = "RightToolStripPanel";
			this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// LeftToolStripPanel
			// 
			this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
			this.LeftToolStripPanel.Name = "LeftToolStripPanel";
			this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
			// 
			// ContentPanel
			// 
			this.ContentPanel.Size = new System.Drawing.Size(636, 536);
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.BackColor = System.Drawing.SystemColors.Window;
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTipoDispositivo});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(625, 516);
			this.listView.TabIndex = 37;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
			this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
			this.listView.DoubleClick += new System.EventHandler(this.listView_dbclick);
			this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
			// 
			// columnTipoDispositivo
			// 
			this.columnTipoDispositivo.Text = "Tipo dispositivo";
			this.columnTipoDispositivo.Width = 578;
			// 
			// tscontainer
			// 
			// 
			// tscontainer.ContentPanel
			// 
			this.tscontainer.ContentPanel.Controls.Add(this.listView);
			this.tscontainer.ContentPanel.Size = new System.Drawing.Size(636, 536);
			this.tscontainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tscontainer.Location = new System.Drawing.Point(0, 0);
			this.tscontainer.Name = "tscontainer";
			this.tscontainer.Size = new System.Drawing.Size(636, 595);
			this.tscontainer.TabIndex = 93;
			this.tscontainer.Text = "toolStripContainer1";
			// 
			// tscontainer.TopToolStripPanel
			// 
			this.tscontainer.TopToolStripPanel.Controls.Add(this.main_toolstrip);
			// 
			// TipiDispositivi
			// 
			this.ClientSize = new System.Drawing.Size(636, 595);
			this.ControlBox = false;
			this.Controls.Add(this.tscontainer);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TipiDispositivi";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CLEAN TRACK - Tipi dispositivi";
			this.Load += new System.EventHandler(this.TipiDispositivi_Load);
			this.ClientSizeChanged += new System.EventHandler(this.TipiDispositivi_ClientSizeChanged);
			this.main_toolstrip.ResumeLayout(false);
			this.main_toolstrip.PerformLayout();
			this.tscontainer.ContentPanel.ResumeLayout(false);
			this.tscontainer.TopToolStripPanel.ResumeLayout(false);
			this.tscontainer.TopToolStripPanel.PerformLayout();
			this.tscontainer.ResumeLayout(false);
			this.tscontainer.PerformLayout();
			this.ResumeLayout(false);

		}

		private void listView_dbclick(object sender, EventArgs e)
		{
			tsb_add.Visible = false;
			tsb_delete.Visible = false;
			tsb_close.Visible = false;
		}
		#endregion

		private bool EndEditTipo_dispositivi(int iItemNum, int iSubitemNum, string strText)
		{
			bool bReturn = false;
			OdbcConnection connTemp = DBUtil.GetODBCConnection();
			OdbcCommand commTemp = new OdbcCommand("", connTemp);

			if (strText == "Nuovo Tipo Dispositivo") //nel caso in cui non venga editato nulla, elimino la riga corrispondente
			{
				commTemp = new OdbcCommand("Delete from TipiDispositivi WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
				commTemp.ExecuteNonQuery();

				tsb_add.Visible = true;
				tsb_delete.Visible = true;
				tsb_close.Visible = true;

                RiempiLista();
                return bReturn;
			}

			switch (iSubitemNum)
			{
				case 0:
					{
						commTemp = new OdbcCommand("UPDATE TipiDispositivi SET Descrizione = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);


						//INSERIMENTO NEL LOG DI SISTEMA
						//valore originale
						string ValoreOriginale = "";
						OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Descrizione FROM TipiDispositivi WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
						OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
						if (readerValoreOriginale.Read())
						{
							if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Descrizione")))
							{
								ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Descrizione")).ToString();

								if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
									readerValoreOriginale.Close();
							}
							if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
								readerValoreOriginale.Close();
						}
						OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
									" VALUES ('" + KleanTrak.Globals.m_strUser + "', 'TIPI DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'DESCRIZIONE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
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

			tsb_add.Visible = true;
			tsb_delete.Visible = true;
			tsb_close.Visible = true;

			return bReturn;
		}

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e) => tsb_delete.Visible = listView.SelectedItems.Count > 0;

		private void TipiDispositivi_Load(object sender, System.EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Globals.LocalizzaDialog(this);
				listView.ListViewItemSorter = new ListViewEx.ListViewExComparer(listView);
				RiempiLista();
				WindowState = FormWindowState.Maximized;
				MinimumSize = Size;
				Globals.ResizeList(this, listView);
				tsb_delete.Visible = false;
				tsb_add.ToolTipText = Globals.strTable[176];
				tsb_delete.ToolTipText = Globals.strTable[177];
				tsb_close.ToolTipText = Globals.strTable[155];
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

			string query = "";

			if (riempilista_nuovo == 1)//caso in cui voglia riordinare gli elementi per ID
			{
				query = "SELECT * FROM TipiDispositivi ORDER BY ID";
				riempilista_nuovo = 0;
			}
			else
			{
				query = "SELECT * FROM TipiDispositivi ORDER BY Descrizione";
			}

			OdbcConnection connTemp = DBUtil.GetODBCConnection();
			OdbcCommand commTemp = new OdbcCommand(query, connTemp);
			OdbcDataReader readerTemp = commTemp.ExecuteReader();

			ListViewItem lvItem;
			while (readerTemp.Read())
			{
				lvItem = listView.Items.Add(readerTemp.GetString(readerTemp.GetOrdinal("Descrizione")));
				lvItem.Tag = readerTemp.GetIntEx("ID");

			}

			if ((readerTemp != null) && (readerTemp.IsClosed == false))
				readerTemp.Close();

			connTemp.Close();

			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}
		private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            ((ListViewEx.ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
        }
        private void listView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode != Keys.Delete || (listView.SelectedIndices.Count == 0))
				return;
			tsb_delete_Click(null, null);
		}
		private void TipiDispositivi_ClientSizeChanged(object sender, EventArgs e) => Globals.ResizeList(this, listView);
		private void tsb_add_Click(object sender, EventArgs e)
		{

			OdbcCommand cmd = null;
			OdbcDataReader rdr = null;
			try
			{
				string query = "INSERT INTO TIPIDISPOSITIVI (DESCRIZIONE) VALUES ('NUOVO TIPO DISPOSITIVO')";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				cmd.ExecuteNonQuery();
				DBUtil.InsertDbLog("TIPIDISPOSITIVI", DBUtil.LogOperation.Insert, cmd.GetMaxKeyValue("TIPIDISPOSITIVI", "ID"));
				riempilista_nuovo = 1;
				RiempiLista();
				listView.EnsureVisible(listView.Items.Count - 1);
				listView.EditCell(listView.Items.Count - 1, 0);
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
			}
			finally
			{
				if (rdr != null && !rdr.IsClosed)
					rdr.Close();
				if (cmd != null)
					cmd.Dispose();
			}
		}
		private void tsb_delete_Click(object sender, EventArgs e)
		{
			try
			{
				if (MessageBox.Show(KleanTrak.Globals.strTable[66], "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return;
				OdbcConnection connTemp = DBUtil.GetODBCConnection();
				//verifico che il tipo dispositivo non sia già stata utilizzato!
				OdbcCommand commTemp_check = new OdbcCommand("SELECT Dispositivi.idtipo from Dispositivi " +
					"LEFT OUTER JOIN Tipidispositivi ON Dispositivi.idtipo = Tipidispositivi.ID " +
							"where Tipidispositivi.ID  = " + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "", connTemp);
				OdbcDataReader readerTemp = commTemp_check.ExecuteReader();
				if (readerTemp.Read())
				{
					MessageBox.Show(KleanTrak.Globals.strTable[67], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (readerTemp != null && !readerTemp.IsClosed)
					readerTemp.Close();
				OdbcCommand commTemp = new OdbcCommand("DELETE FROM Tipidispositivi WHERE ID=" + listView.Items[listView.SelectedIndices[0]].Tag.ToString(), connTemp);
				commTemp.ExecuteNonQuery();
				DBUtil.InsertDbLog("TIPIDISPOSITIVI", DBUtil.LogOperation.Delete, commTemp.GetMaxKeyValue("TIPIDISPOSITIVI", "ID"));
				listView.Items.RemoveAt(listView.SelectedIndices[0]);
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
				throw;
			}
		}
		private void tsb_close_Click(object sender, EventArgs e) => Close();
	}
}
