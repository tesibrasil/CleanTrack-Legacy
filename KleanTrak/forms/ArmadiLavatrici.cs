using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using System.Collections.Generic;
using OdbcExtensions;

namespace KleanTrak
{
	public class ArmadiLavatrici : System.Windows.Forms.Form
	{
		private System.ComponentModel.Container components = null;
		private ListViewEx.ListViewEx listView;
		private System.Windows.Forms.ColumnHeader columnSterilizzatrice;
		private System.Windows.Forms.ColumnHeader columnSeriale;
		private System.Windows.Forms.ColumnHeader columnMatricola;
		private System.Windows.Forms.ColumnHeader columnTempoLavaggio;
        private System.Windows.Forms.ColumnHeader columnTipo;
        private System.Windows.Forms.ColumnHeader columnPercorso;
        private System.Windows.Forms.ColumnHeader columnPolling;
        private System.Windows.Forms.ColumnHeader columnDismesso;
        private System.Windows.Forms.ColumnHeader columnCausale;
        private ToolStripContainer tscontainer;
        private ToolStrip toolStrip1;
		private ToolStripButton tsb_add;
		private ToolStripButton tsb_delete;
		private ToolStripButton tsb_close;
		private string sCode = "CODE";
		public bool Modified { private set; get; } = false;
		public ArmadiLavatrici()
		{
            InitializeComponent();
            listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetSubItemType(1, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetSubItemType(2, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetSubItemType(3, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(4, ListViewEx.ListViewEx.FieldType.combo, GetComboTipoList());
            listView.SetSubItemType(5, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(6, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(7, ListViewEx.ListViewEx.FieldType.check, null);
            listView.SetSubItemType(8, ListViewEx.ListViewEx.FieldType.combo, GetComboDismissioneList());
            listView.SetEndEditCallback(new ListViewEx.ListViewEx.EndEditingCallbackEdit(EndEdit), 
				new ListViewEx.ListViewEx.EndEditingCallbackCombo(EndCombo), 
				null, 
				new ListViewEx.ListViewEx.EndEditingCallbackCheck(EndCheck));
			SetupTooltip();
		}
		private void SetupTooltip()
		{
			try
			{
				tsb_add.ToolTipText = Globals.strTable[190];
				tsb_delete.ToolTipText = Globals.strTable[191];
				tsb_close.ToolTipText = Globals.strTable[155];
			}
			catch (Exception e)
			{
				Globals.WarnAndLog(e);
			}
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
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArmadiLavatrici));
			this.listView = new ListViewEx.ListViewEx();
			this.columnMatricola = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnSterilizzatrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnSeriale = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnTempoLavaggio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnTipo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnPercorso = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnPolling = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnDismesso = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnCausale = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tscontainer = new System.Windows.Forms.ToolStripContainer();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsb_add = new System.Windows.Forms.ToolStripButton();
			this.tsb_delete = new System.Windows.Forms.ToolStripButton();
			this.tsb_close = new System.Windows.Forms.ToolStripButton();
			this.tscontainer.ContentPanel.SuspendLayout();
			this.tscontainer.TopToolStripPanel.SuspendLayout();
			this.tscontainer.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnMatricola,
            this.columnSterilizzatrice,
            this.columnSeriale,
            this.columnTempoLavaggio,
            this.columnTipo,
            this.columnPercorso,
            this.columnPolling,
			this.columnDismesso,
            this.columnCausale});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(1205, 515);
			this.listView.TabIndex = 18;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listview_ColumnClick);
			this.listView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView_ItemSelectionChanged);
			this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
			// 
			// columnMatricola
			// 
			this.columnMatricola.Text = "Matricola ";
			this.columnMatricola.Width = 150;
			// 
			// columnSterilizzatrice
			// 
			this.columnSterilizzatrice.Text = "Descrizione ";
			this.columnSterilizzatrice.Width = 200;
			// 
			// columnSeriale
			// 
			this.columnSeriale.Text = "N° di serie ";
			this.columnSeriale.Width = 150;
			// 
			// columnTempoLavaggio
			// 
			this.columnTempoLavaggio.Text = "Tempo di lavaggio (min) ";
			this.columnTempoLavaggio.Width = 210;
			// 
			// columnTipo
			// 
			this.columnTipo.Text = "Tipo ";
			this.columnTipo.Width = 150;
			// 
			// columnPercorso
			// 
			this.columnPercorso.Text = "Percorso ";
			this.columnPercorso.Width = 250;
			// 
			// columnPolling
			// 
			this.columnPolling.Text = "Tempo di polling (sec) ";
			this.columnPolling.Width = 210;
			// 
			// columnDismesso
			// 
			this.columnDismesso.Text = "Dismessa ";
			this.columnDismesso.Width = 90;
			// 
			// columnCausale
			// 
			this.columnCausale.Text = "Causale ";
			this.columnCausale.Width = 150;
			// 
			// tscontainer
			// 
			// 
			// tscontainer.ContentPanel
			// 
			this.tscontainer.ContentPanel.Controls.Add(this.listView);
			this.tscontainer.ContentPanel.Size = new System.Drawing.Size(1214, 535);
			this.tscontainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tscontainer.Location = new System.Drawing.Point(0, 0);
			this.tscontainer.Name = "tscontainer";
			this.tscontainer.Size = new System.Drawing.Size(1214, 590);
			this.tscontainer.TabIndex = 55;
			this.tscontainer.Text = "toolStripContainer1";
			// 
			// tscontainer.TopToolStripPanel
			// 
			this.tscontainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_add,
            this.tsb_delete,
            this.tsb_close});
			this.toolStrip1.Location = new System.Drawing.Point(3, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(199, 55);
			this.toolStrip1.TabIndex = 0;
			// 
			// tsb_add
			// 
			this.tsb_add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_add.Image = global::kleanTrak.Properties.Resources.add;
			this.tsb_add.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_add.Name = "tsb_add";
			this.tsb_add.Size = new System.Drawing.Size(52, 52);
			this.tsb_add.Text = "toolStripButton1";
			this.tsb_add.Click += new System.EventHandler(this.tsb_add_Click);
			// 
			// tsb_delete
			// 
			this.tsb_delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_delete.Image = global::kleanTrak.Properties.Resources.delete;
			this.tsb_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_delete.Name = "tsb_delete";
			this.tsb_delete.Size = new System.Drawing.Size(52, 52);
			this.tsb_delete.Text = "toolStripButton1";
			this.tsb_delete.Click += new System.EventHandler(this.tsb_delete_Click);
			// 
			// tsb_close
			// 
			this.tsb_close.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_close.Image = global::kleanTrak.Properties.Resources.close;
			this.tsb_close.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_close.Name = "tsb_close";
			this.tsb_close.Size = new System.Drawing.Size(52, 52);
			this.tsb_close.Text = "toolStripButton1";
			this.tsb_close.Click += new System.EventHandler(this.tsb_close_Click);
			// 
			// ArmadiLavatrici
			// 
			this.ClientSize = new System.Drawing.Size(1214, 590);
			this.ControlBox = false;
			this.Controls.Add(this.tscontainer);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ArmadiLavatrici";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CLEAN TRACK - Armadi e lavatrici in uso";
			this.Load += new System.EventHandler(this.ArmadiLavatrici_Load);
			this.ClientSizeChanged += new System.EventHandler(this.ArmadiLavatrici_ClientSizeChanged);
			this.tscontainer.ContentPanel.ResumeLayout(false);
			this.tscontainer.TopToolStripPanel.ResumeLayout(false);
			this.tscontainer.TopToolStripPanel.PerformLayout();
			this.tscontainer.ResumeLayout(false);
			this.tscontainer.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}
        private void ArmadiLavatrici_Load(object sender, System.EventArgs e)
        {
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Globals.LocalizzaDialog(this);
				listView.ListViewItemSorter = new ListViewEx.ListViewExComparer(listView);
				RiempiLista();
				WindowState = FormWindowState.Maximized;
				MinimumSize = Size;
				Globals.ResizeList(this, listView, false);
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
            string query = $"SELECT ARMADI_LAVATRICI.ID, MATRICOLA, DESCRIZIONE, SERIALE, DISMESSO, " +
				$"CAUSALI.CAUSALE AS CAUSALEDISMISSIONE, TEMPOLAVAGGIO, TIPO, PERCORSO, POLLINGTIME " +
				$"FROM ARMADI_LAVATRICI " +
				$"LEFT OUTER JOIN CAUSALI ON ARMADI_LAVATRICI.IDCAUSALEDISMISSIONE = CAUSALI.ID " +
				$"WHERE IDSEDE = {Globals.IDSEDE} " +
				$"ORDER BY DESCRIZIONE";
			OdbcCommand cmd = null;
			OdbcDataReader rdr = null;
			try
			{
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					ListViewItem lvItem = listView.Items.Add(rdr.GetStringEx("Matricola").ToUpper());
					lvItem.Tag = rdr.GetIntEx("ID");
					//lvItem.SubItems.Add(rdr.GetStringEx("Matricola").ToUpper());
					lvItem.SubItems.Add(rdr.GetStringEx("Descrizione").ToUpper());
					lvItem.SubItems.Add(rdr.GetStringEx("Seriale").ToUpper());
					lvItem.SubItems.Add(rdr.GetIntEx("TempoLavaggio").ToString().ToUpper());
					if (!rdr.IsDBNull(rdr.GetOrdinal("Tipo")))
					{
						int tipo = rdr.GetIntEx("Tipo");
						if (Enum.IsDefined(typeof(KleanTrak.Model.WasherStorageTypes), tipo))
							lvItem.SubItems.Add(((KleanTrak.Model.WasherStorageTypes)tipo).ToString());
						else
							lvItem.SubItems.Add("");
					}
					else
						lvItem.SubItems.Add("");
					lvItem.SubItems.Add(rdr.GetStringEx("Percorso"));
					lvItem.SubItems.Add(rdr.GetIntEx("PollingTime").ToString().ToUpper());
					lvItem.SubItems.Add(rdr.GetBoolEx("Dismesso") ? Globals.strTable[138] : Globals.strTable[139]);
					lvItem.SubItems.Add(rdr.GetStringEx("CausaleDismissione"));
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.ToString(), "CleanTrack", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				if (rdr != null && !rdr.IsClosed)
					rdr.Close();
				if (cmd != null)
					cmd.Dispose();
				ManageToolstripButtons();
			}
		}
		private bool MatricolaPresente(string strText, int id)
		{
			OdbcCommand cmd = null;
			try
			{
				cmd = new OdbcCommand("SELECT COUNT(*) FROM ARMADI_LAVATRICI WHERE UPPER(MATRICOLA) = UPPER(?) AND ID <> ? AND UPPER(MATRICOLA) <> 'NUOVO'", DBUtil.GetODBCConnection());
				cmd.Parameters.Add(new OdbcParameter("pmatricola", OdbcType.NVarChar, 50) { Value = strText });
				cmd.Parameters.Add(new OdbcParameter("pid", OdbcType.Int) { Value = id });
				return cmd.ExecuteScalarInt() != 0;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return true;
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}

		private bool SerialePresente(string strText, int id)
		{
			OdbcCommand cmd = null;
			try
			{
				cmd = new OdbcCommand("SELECT COUNT(*) FROM ARMADI_LAVATRICI WHERE UPPER(SERIALE) = UPPER(?) AND ID <> ? AND UPPER(SERIALE) <> 'NUOVO'", DBUtil.GetODBCConnection());
				cmd.Parameters.Add(new OdbcParameter("pseriale", OdbcType.NVarChar, 50) { Value = strText });
				cmd.Parameters.Add(new OdbcParameter("pid", OdbcType.Int) { Value = id });
				return cmd.ExecuteScalarInt() != 0;
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return true;
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}

		private bool EndEdit(int iItemNum, int iSubitemNum, string strText)
		{
			OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = null;

			if (strText == sCode) //nel caso in cui non venga editato nulla, elimino la riga corrispondente
			{
				commTemp = new OdbcCommand($"Delete from ARMADI_LAVATRICI WHERE ID={listView.Items[iItemNum].Tag}", connTemp);
				commTemp.ExecuteNonQuery();

				RiempiLista();
				return false;
			}
			Modified = true;
			switch (iSubitemNum)
			{
				case 0:
					{
						if (MatricolaPresente(strText, (int)listView.Items[iItemNum].Tag))
						{
							MessageBox.Show(KleanTrak.Globals.strTable[207], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						}

						commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Matricola = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
						//INSERIMENTO NEL LOG DI SISTEMA
						//valore originale
						string ValoreOriginale = "";
						OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Matricola FROM ARMADI_LAVATRICI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
						OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
						if (readerValoreOriginale.Read())
						{
							if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Matricola")))
								ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Matricola")).ToString();
						}
						if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
							readerValoreOriginale.Close();

						if (ValoreOriginale != strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
						{
							OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
										" VALUES ('" + Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'CODICE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
							cmdLog.ExecuteNonQuery();
						}
						break;
					}
				case 1:
					{
						commTemp = new OdbcCommand($"UPDATE ARMADI_LAVATRICI SET DESCRIZIONE = '{strText.Replace("'", "''").ToUpper()}' " +
							$"WHERE ID = {listView.Items[iItemNum].Tag}", 
							connTemp);
						//INSERIMENTO NEL LOG DI SISTEMA
						//valore originale
						string ValoreOriginale = "";
						OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Descrizione FROM ARMADI_LAVATRICI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
						OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
						if (readerValoreOriginale.Read())
						{
							if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Descrizione")))
								ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Descrizione")).ToString();
						}

                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale != strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                        {
                            OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                        " VALUES ('" + Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'DESCRIZIONE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                            cmdLog.ExecuteNonQuery();
                        }
						break;
					}
				case 2:
					{
						if (SerialePresente(strText, (int)listView.Items[iItemNum].Tag))
						{
							MessageBox.Show(KleanTrak.Globals.strTable[208], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return false;
						}
						commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Seriale = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

						//INSERIMENTO NEL LOG DI SISTEMA
						//valore originale
						string ValoreOriginale = "";
						OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Seriale FROM ARMADI_LAVATRICI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
						OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
						if (readerValoreOriginale.Read())
						{
							if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Seriale")))
								ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Seriale")).ToString();
						}

                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale != strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                        {
                            OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                        " VALUES ('" + Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'NUMERO DI SERIE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                            cmdLog.ExecuteNonQuery();
                        }
						break;
					}
				case 3:
					{
                        int num = 0;
                        if (int.TryParse(strText, out num))
                        {
                            commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET TempoLavaggio = " + strText.Replace("'", "''").ToUpper() + " WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                            //INSERIMENTO NEL LOG DI SISTEMA
                            //valore originale
                            string ValoreOriginale = "";
                            OdbcCommand commValoreOriginale = new OdbcCommand("SELECT TempoLavaggio FROM ARMADI_LAVATRICI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                            OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                            if (readerValoreOriginale.Read())
                            {
                                if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("TempoLavaggio")))
                                {
                                    ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("TempoLavaggio")).ToString();

                                    if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                        readerValoreOriginale.Close();
                                }
                            }
                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();

                            if (ValoreOriginale != strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            {
                                OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                            " VALUES ('" + Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'TEMPO LAVAGGIO', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                                cmdLog.ExecuteNonQuery();
                            }
                        }
                        else
                            MessageBox.Show(Globals.strTable[100], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Information);
						break;
					}
                case 5:
                    {
                        commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Percorso = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Percorso FROM ARMADI_LAVATRICI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Percorso")))
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Percorso")).ToString();
                        }

                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale != strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                        {
                            OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                        " VALUES ('" + Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'Percorso', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                            cmdLog.ExecuteNonQuery();
                        }
                        break;
                    }
                case 6:
                    {
                        int num = 0;
                        if (int.TryParse(strText, out num))
                        {
                            commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET PollingTime = " + strText.Replace("'", "''").ToUpper() + " WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                            //INSERIMENTO NEL LOG DI SISTEMA
                            //valore originale
                            string ValoreOriginale = "";
                            OdbcCommand commValoreOriginale = new OdbcCommand("SELECT PollingTime FROM ARMADI_LAVATRICI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                            OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                            if (readerValoreOriginale.Read())
                            {
                                if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("PollingTime")))
                                {
                                    ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("PollingTime")).ToString();

                                    if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                        readerValoreOriginale.Close();
                                }
                            }
                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();

                            if (ValoreOriginale != strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            {
                                OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                            " VALUES ('" + Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'PollingTime', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                                cmdLog.ExecuteNonQuery();
                            }
                        }
                        else
                            MessageBox.Show(Globals.strTable[100], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
            }

            bool bReturn = false;

			try
			{
				if (commTemp != null)
				{
					commTemp.ExecuteNonQuery();
					bReturn = true;
				}
			}
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			connTemp.Close();
			return bReturn;
		}
        private bool EndCombo(int iItemNum, int iSubitemNum, object obj)
        {
			Modified = true;
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = null;

            string strToUpdate = "NULL";
            if (obj != null)
                strToUpdate = ((ItemCombo)obj).iID.ToString();

            switch (iSubitemNum)
            {
                case 4:
                {
                    commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Tipo = " + strToUpdate + " WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                    string ValoreOriginale = "";
                    OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Tipo FROM ARMADI_LAVATRICI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                    if (readerValoreOriginale.Read())
                    {
                        if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Tipo")))
                            ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Tipo")).ToString();
                    }

                    if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                        readerValoreOriginale.Close();

                    if (ValoreOriginale != strToUpdate) //se non viene modificato il valore non effettuo la query nel LOG.
                    {
                        OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO)" +
                                    " VALUES ('" + Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'Tipo', '" + ValoreOriginale.Replace("'", "''") + "', '" + strToUpdate.Replace("'", "''") + "')", connTemp);
                        cmdLog.ExecuteNonQuery();
                    }
                    break;
                }
                case 8:
                {
                    commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET IDCausaleDismissione = " + strToUpdate + " WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                    string ValoreOriginale = "";
                    OdbcCommand commValoreOriginale = new OdbcCommand("SELECT IDCausaleDismissione FROM ARMADI_LAVATRICI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                    if (readerValoreOriginale.Read())
                    {
                        if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("IDCausaleDismissione")))
                            ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("IDCausaleDismissione")).ToString();
                    }

                    if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                        readerValoreOriginale.Close();

                    if (ValoreOriginale != strToUpdate) //se non viene modificato il valore non effettuo la query nel LOG.
                    {
                        OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO)" +
                                    " VALUES ('" + Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'IDCausaleDismissione', '" + ValoreOriginale.Replace("'", "''") + "', '" + strToUpdate.Replace("'", "''") + "')", connTemp);
                        cmdLog.ExecuteNonQuery();
                    }
                    break;
                }
            }

            bool bReturn = false;

            try
            {
                if (commTemp != null)
                {
                    commTemp.ExecuteNonQuery();
                    bReturn = true;
                }
            }
            catch (OdbcException Ex)
            {
                MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            connTemp.Close();
            return bReturn;
        }
        private bool EndCheck(int iItemNum, int iSubitemNum, bool bChecked)
        {
            if (iSubitemNum != 7)
                return false;
			Modified = true;
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Dismesso = " + (bChecked ? 1 : 0).ToString() + " WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

            string ValoreOriginale = "";
            OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Dismesso FROM ARMADI_LAVATRICI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
            OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
            if (readerValoreOriginale.Read())
            {
                if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Dismesso")))
                    ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Dismesso")).ToString();
            }

            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                readerValoreOriginale.Close();

            if (ValoreOriginale != (bChecked ? 1 : 0).ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
            {
                OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                            " VALUES ('" + Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'Dismesso', '" + ValoreOriginale.Replace("'", "''") + "', '" + (bChecked ? 1 : 0).ToString().Replace("'", "''") + "')", connTemp);
                cmdLog.ExecuteNonQuery();
            }

            bool bReturn = false;

            try
            {
                if (commTemp != null)
                {
                    commTemp.ExecuteNonQuery();
                    bReturn = true;
                }
            }
            catch (OdbcException Ex)
            {
                MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            connTemp.Close();
            return bReturn;
        }
		private void listview_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            ((ListViewEx.ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
        }
		private void listView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			//elimino una sterilizzatrice dalla lista
			if (e.KeyCode == Keys.Delete && (listView.SelectedIndices.Count > 0))
			{
                if (MessageBox.Show(Globals.strTable[57], "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    DeleteItem();
			}
		}
        private void DeleteItem()
        {
			var id = (int)listView.Items[listView.SelectedIndices[0]].Tag;
			//verifica che la sterilizzatrice non sia già stata utilizzata
			OdbcCommand cmd = null;
			try
			{
				cmd = new OdbcCommand($"SELECT COUNT(*) FROM CICLI " +
				$"LEFT OUTER JOIN ARMADI_LAVATRICI ON " +
				$"CICLI.IDSTERILIZZATRICE = ARMADI_LAVATRICI.ID " +
				$"WHERE ARMADI_LAVATRICI.ID = {id}",
				DBUtil.GetODBCConnection());
				if ((int)cmd.ExecuteScalar() > 0)
				{
					MessageBox.Show(Globals.strTable[52], 
						"Clean Track", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Exclamation);
					return;
				}
				cmd.CommandText = $"SELECT COUNT(*) FROM STERILIZZATRICIPARSING WHERE IDSTERILIZZATRICE = {id}";
				if ((int)cmd.ExecuteScalar() > 0)
				{
					MessageBox.Show(Globals.strTable[52],
						"Clean Track",
						MessageBoxButtons.OK,
						MessageBoxIcon.Exclamation);
					return;
				}
				cmd.CommandText = $"DELETE FROM ARMADI_LAVATRICI WHERE ID = {id}";
				cmd.ExecuteNonQuery();
				Modified = true;
				DBUtil.InsertDbLog("ARMADI_LAVATRICI", DBUtil.LogOperation.Delete, id);
				RiempiLista();
			}
			catch (Exception e)
			{
				Globals.WarnAndLog(e);
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}
        public class ItemCombo
		{
            public int iID { set; get; }
            public string sName { set; get; }

			public override string ToString() { return sName; }
        }
        private ArrayList GetComboTipoList()
        {
            Array values = Enum.GetValues(typeof(KleanTrak.Model.WasherStorageTypes));
            string[] names = Enum.GetNames(typeof(KleanTrak.Model.WasherStorageTypes));

			ArrayList list = new ArrayList();

			ItemCombo itemEmpty = new ItemCombo();
			itemEmpty.iID = -1;
			itemEmpty.sName = "";
			list.Add(itemEmpty);

            for (int i = 0; i < names.Length; i++)
            {
                list.Add(new ItemCombo() { iID = (int)(KleanTrak.Model.WasherStorageTypes)values.GetValue(i), sName = names[i] });
            }

			return list;
        }
        private ArrayList GetComboDismissioneList()
        {
			ArrayList list = new ArrayList();

			OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("SELECT ID, Causale FROM Causali ORDER BY Causale", connTemp);
            OdbcDataReader readerTemp = null;

			ItemCombo itemEmpty = new ItemCombo();
			itemEmpty.iID = 0;
			itemEmpty.sName = "";
			list.Add(itemEmpty);

			try
			{
                readerTemp = commTemp.ExecuteReader();
                while (readerTemp.Read())
                {
                    ItemCombo item = new ItemCombo();
                    item.iID = readerTemp.GetIntEx("ID");
                    item.sName = readerTemp.GetString(readerTemp.GetOrdinal("Causale"));
                    list.Add(item);
                }
            }
            catch (OdbcException Ex)
            {
                MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if ((readerTemp != null) && (readerTemp.IsClosed == false))
                readerTemp.Close();

            connTemp.Close();
			return list;
        }
		private void ArmadiLavatrici_ClientSizeChanged(object sender, EventArgs e) => Globals.ResizeList(this, listView);
		private void tsb_delete_Click(object sender, EventArgs e)
		{
			try
			{
				if (listView.SelectedItems.Count == 0)
				{
					MessageBox.Show(Globals.strTable[50], 
						"Clean Track", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Exclamation);
					return;
				}
				if (MessageBox.Show(Globals.strTable[51], 
					"Clean Track", 
					MessageBoxButtons.YesNo, 
					MessageBoxIcon.Question) == DialogResult.Yes)
					DeleteItem();
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
				throw;
			}
		}
		private void tsb_close_Click(object sender, EventArgs e) => Close();
		private void tsb_add_Click(object sender, EventArgs e)
		{
			OdbcCommand cmd = null;
			try
			{
				string query = $"INSERT INTO ARMADI_LAVATRICI " +
					$"(Descrizione, Matricola, Seriale, IDSEDE, Tipo) " +
					$"VALUES " +
					$"('NEW', '{sCode}', '', {Globals.IDSEDE}, -1)";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				cmd.ExecuteNonQuery();
				//INSERIMENTO NEL LOG DI SISTEMA
				cmd.CommandText = "SELECT MAX (ID) as MAXID FROM ARMADI_LAVATRICI";
				int max_id = cmd.ExecuteScalarInt();
				if (max_id <= 0)
					throw new ApplicationException("impossibile creare un nuovo record per AMRADI_LAVATRICI");
				cmd.CommandText = $"INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD ) " +
					$"VALUES " +
					$"('{Globals.m_strUser}', 'ARMADI_LAVATRICI', 'Inserimento', '{Globals.ConvertDateTime(DateTime.Now)}', '{max_id}')";
				cmd.ExecuteNonQuery();
				RiempiLista();
				listView.EnsureVisible(listView.Items.Count - 1);
				listView.EditCell(listView.Items.Count - 1, 1);
			}
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, 
					"Clean Track", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}
		private void ManageToolstripButtons() => tsb_delete.Visible = listView.SelectedIndices.Count > 0;
		private void listView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) => ManageToolstripButtons();
		private bool DismissDevice(int id, bool dismiss)
		{
			OdbcCommand cmd = null;
			try
			{
				string query = $"UPDATE ARMADI_LAVATRICI SET DISMESSO = {((dismiss) ? 1 : 0)} WHERE ID = {id}";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				return 1 == cmd.ExecuteNonQuery();
			}
			catch (Exception e)
			{
				Globals.Log(e, $"id: {id}");
				return false;
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}
		private void tsb_dismiss_Click(object sender, EventArgs e)
		{
			try
			{
				if (listView.SelectedItems.Count == 0)
					return;
				if (MessageBox.Show(Globals.strTable[196], "Celan Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return;
				if (!DismissDevice((int)listView.SelectedItems[0].Tag, true))
				{
					MessageBox.Show(Globals.strTable[197], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				RiempiLista();
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
			}
		}
		private void tsb_activate_device_Click(object sender, EventArgs e)
		{
			try
			{
				if (listView.SelectedItems.Count == 0)
					return;
				if (MessageBox.Show(Globals.strTable[198], "Celan Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return;
				if (!DismissDevice((int)listView.SelectedItems[0].Tag, false))
				{
					MessageBox.Show(Globals.strTable[197], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				RiempiLista();
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
			}
		}
	}
}
