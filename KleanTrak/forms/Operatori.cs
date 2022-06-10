using KleanTrak.Core;
using OdbcExtensions;
using System;
using System.Data.Odbc;
using System.Windows.Forms;

namespace KleanTrak
{
    public class Operatori : System.Windows.Forms.Form
    {
        private enum STATO
        {
            VISUALIZZA,
            NUOVO,
            MODIFICA
        }

        private ListViewEx.ListViewEx listView;
        private System.Windows.Forms.ColumnHeader columnCognome;
        private System.Windows.Forms.ColumnHeader columnNome;
        private System.Windows.Forms.ColumnHeader column_RFID;
        private System.Windows.Forms.ColumnHeader column_Cod_Utente;
        private System.Windows.Forms.ColumnHeader columnAttivo;
        private System.Windows.Forms.ColumnHeader columnTag;
        private System.ComponentModel.IContainer components;

        private System.Windows.Forms.ImageList imageList;
        private ToolStripContainer toolstripcontainer;
        private ToolStrip maintoolstrip;
        private ToolStripButton tsb_add;
        private ToolStripButton tsb_delete;
        private ToolStripButton tsb_disable;
        private ToolStripButton tsb_tag;
        private ToolStripButton tsb_undo_disable;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton tsb_close;
        public int iItemNum;

        public Operatori()
        {
            InitializeComponent();
            //listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.combo, ComboUser());
            listView.SetSubItemType(1, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(2, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(3, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(5, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetEndEditCallback(new ListViewEx.ListViewEx.EndEditingCallbackEdit(EndEditOperatori), new ListViewEx.ListViewEx.EndEditingCallbackCombo(EndComboEdit), null, null);
            tsb_add.ToolTipText = Globals.strTable[180];
            tsb_delete.ToolTipText = Globals.strTable[181];
            tsb_disable.ToolTipText = Globals.strTable[182];
            tsb_undo_disable.ToolTipText = Globals.strTable[183];
            tsb_tag.ToolTipText = Globals.strTable[184];
            tsb_close.ToolTipText = Globals.strTable[185];
            tsb_delete.Visible = false;
            tsb_disable.Visible = false;
            tsb_tag.Visible = false;
            tsb_undo_disable.Visible = false;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Operatori));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolstripcontainer = new System.Windows.Forms.ToolStripContainer();
            this.listView = new ListViewEx.ListViewEx();
            this.column_Cod_Utente = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnCognome = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnNome = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.column_RFID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnAttivo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.maintoolstrip = new System.Windows.Forms.ToolStrip();
            this.tsb_add = new System.Windows.Forms.ToolStripButton();
            this.tsb_delete = new System.Windows.Forms.ToolStripButton();
            this.tsb_tag = new System.Windows.Forms.ToolStripButton();
            this.tsb_disable = new System.Windows.Forms.ToolStripButton();
            this.tsb_undo_disable = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_close = new System.Windows.Forms.ToolStripButton();
            this.toolstripcontainer.ContentPanel.SuspendLayout();
            this.toolstripcontainer.TopToolStripPanel.SuspendLayout();
            this.toolstripcontainer.SuspendLayout();
            this.maintoolstrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
            // 
            // toolstripcontainer
            // 
            // 
            // toolstripcontainer.ContentPanel
            // 
            this.toolstripcontainer.ContentPanel.Controls.Add(this.listView);
            this.toolstripcontainer.ContentPanel.Size = new System.Drawing.Size(1101, 539);
            this.toolstripcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolstripcontainer.Location = new System.Drawing.Point(0, 0);
            this.toolstripcontainer.Name = "toolstripcontainer";
            this.toolstripcontainer.Size = new System.Drawing.Size(1101, 594);
            this.toolstripcontainer.TabIndex = 55;
            // 
            // toolstripcontainer.TopToolStripPanel
            // 
            this.toolstripcontainer.TopToolStripPanel.Controls.Add(this.maintoolstrip);
            // 
            // listView
            // 
            this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.column_Cod_Utente,
            this.columnCognome,
            this.columnNome,
            this.column_RFID,
            this.columnAttivo,
            this.columnTag});
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(1096, 558);
            this.listView.SmallImageList = this.imageList;
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.Click_colonna);
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
            // 
            // column_Cod_Utente
            // 
            this.column_Cod_Utente.Text = "Utente";
            this.column_Cod_Utente.Width = 109;
            // 
            // columnCognome
            // 
            this.columnCognome.Text = "Cognome";
            this.columnCognome.Width = 112;
            // 
            // columnNome
            // 
            this.columnNome.Text = "Nome";
            this.columnNome.Width = 102;
            // 
            // column_RFID
            // 
            this.column_RFID.Text = "Codice operatore";
            this.column_RFID.Width = 122;
            // 
            // columnAttivo
            // 
            this.columnAttivo.Text = "Attivo";
            this.columnAttivo.Width = 84;
            // 
            // columnTag
            // 
            this.columnTag.Text = "Tag";
            this.columnTag.Width = 84;
            // 
            // maintoolstrip
            // 
            this.maintoolstrip.Dock = System.Windows.Forms.DockStyle.None;
            this.maintoolstrip.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.maintoolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_add,
            this.tsb_delete,
            this.tsb_tag,
            this.tsb_disable,
            this.tsb_undo_disable,
            this.toolStripSeparator2,
            this.tsb_close});
            this.maintoolstrip.Location = new System.Drawing.Point(3, 0);
            this.maintoolstrip.Name = "maintoolstrip";
            this.maintoolstrip.Size = new System.Drawing.Size(361, 55);
            this.maintoolstrip.TabIndex = 0;
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
            // tsb_tag
            // 
            this.tsb_tag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_tag.Image = global::kleanTrak.Properties.Resources.rf;
            this.tsb_tag.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_tag.Name = "tsb_tag";
            this.tsb_tag.Size = new System.Drawing.Size(52, 52);
            this.tsb_tag.Text = "toolStripButton1";
            this.tsb_tag.Click += new System.EventHandler(this.tsb_tag_Click);
            // 
            // tsb_disable
            // 
            this.tsb_disable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_disable.Image = global::kleanTrak.Properties.Resources.block;
            this.tsb_disable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_disable.Name = "tsb_disable";
            this.tsb_disable.Size = new System.Drawing.Size(52, 52);
            this.tsb_disable.Text = "toolStripButton1";
            this.tsb_disable.Click += new System.EventHandler(this.tsb_disable_Click);
            // 
            // tsb_undo_disable
            // 
            this.tsb_undo_disable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_undo_disable.Image = global::kleanTrak.Properties.Resources.check;
            this.tsb_undo_disable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_undo_disable.Name = "tsb_undo_disable";
            this.tsb_undo_disable.Size = new System.Drawing.Size(52, 52);
            this.tsb_undo_disable.Text = "toolStripButton1";
            this.tsb_undo_disable.Click += new System.EventHandler(this.tsb_undo_disable_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 55);
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
            // Operatori
            // 
            this.ClientSize = new System.Drawing.Size(1101, 594);
            this.ControlBox = false;
            this.Controls.Add(this.toolstripcontainer);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "Operatori";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CLEAN TRACK - Operatori sterilizzazione";
            this.Load += new System.EventHandler(this.Operatori_Load);
            this.ClientSizeChanged += new System.EventHandler(this.Operatori_ClientSizeChanged);
            this.toolstripcontainer.ContentPanel.ResumeLayout(false);
            this.toolstripcontainer.TopToolStripPanel.ResumeLayout(false);
            this.toolstripcontainer.TopToolStripPanel.PerformLayout();
            this.toolstripcontainer.ResumeLayout(false);
            this.toolstripcontainer.PerformLayout();
            this.maintoolstrip.ResumeLayout(false);
            this.maintoolstrip.PerformLayout();
            this.ResumeLayout(false);

        }
        public class Item_User
        {
            public long id;
            public string strName;
            public override string ToString() { return strName; }
        }
        public int termina_EndEditOperatori = -1;
        private bool EndEditOperatori(int iItemNum, int iSubitemNum, string strText)
        {
            bool bReturn = false;

            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("", connTemp);

            if (strText == "Nuovo RFID") //nel caso in cui non venga editato nulla, elimino la riga corrispondente
            {
                commTemp = new OdbcCommand("Delete from Operatori WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                commTemp.ExecuteNonQuery();
                RiempiLista();
                return bReturn;
            }

            switch (iSubitemNum)
            {
                case 1:
                    {
                        commTemp = new OdbcCommand("UPDATE Operatori SET Cognome = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Cognome as Cognome FROM Operatori WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Cognome")))
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Cognome")).ToString();
                        }

                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'OPERATORI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'COGNOME', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();
                        break;
                    }

                case 2:
                    {
                        commTemp = new OdbcCommand("UPDATE Operatori SET Nome = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Nome as Nome FROM Operatori WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Nome")))
                            {
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Nome")).ToString();
                            }
                        }

                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'OPERATORI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'NOME', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();
                        break;
                    }

                case 3:
                    {
                        if (strText == "******")
                        {
                            termina_EndEditOperatori = 1;
                            break;

                        }
                        if (MatricolaPresente(strText, (int)listView.Items[iItemNum].Tag))
                        {
                            MessageBox.Show(KleanTrak.Globals.strTable[207], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        //verifica che il nuovo RFID non corrisponda ad uno già stato inserito. 
                        OdbcCommand commTemp_RFIDOperatore = new OdbcCommand("SELECT Matricola from Operatori where Matricola  = '" + strText.ToUpper() + "' and ID <>" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerTemp = commTemp_RFIDOperatore.ExecuteReader();
                        if (readerTemp.Read())
                        {
                            MessageBox.Show(KleanTrak.Globals.strTable[78], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            termina_EndEditOperatori = 1;
                            readerTemp.Close();
                            break;
                        }
                        if (readerTemp != null && !readerTemp.IsClosed)
                            readerTemp.Close();

                        commTemp = new OdbcCommand("UPDATE Operatori SET Matricola = '" + strText.ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Matricola FROM Operatori WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Matricola")))
                            {
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Matricola")).ToString();
                            }
                        }

                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                            " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'OPERATORI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'CODICE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();
                        break;
                    }

                case 5:
                    {
                        if (TagPresente(strText, (int)listView.Items[iItemNum].Tag))
                        {
                            MessageBox.Show(KleanTrak.Globals.strTable[209], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        commTemp = new OdbcCommand("UPDATE Operatori SET TAG = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT TAG FROM Operatori WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("TAG")))
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("TAG")).ToString();
                        }

                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'OPERATORI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'TAG', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();
                        break;
                    }
            }
            try
            {
                if (termina_EndEditOperatori == 1)
                {
                    bReturn = true;
                    termina_EndEditOperatori = -1;
                }
                else
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
        private bool MatricolaPresente(string strText, int id)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand("SELECT COUNT(*) FROM OPERATORI WHERE UPPER(MATRICOLA) = UPPER(?) AND ID <> ? AND UPPER(MATRICOLA) <> 'NUOVO' AND DISATTIVATO = 0", DBUtil.GetODBCConnection());
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
        private bool TagPresente(string strText, int id)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand("SELECT COUNT(*) FROM OPERATORI WHERE TAG = UPPER(?) AND ID <> ? AND DISATTIVATO = 0", DBUtil.GetODBCConnection());
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
        private bool EndComboEdit(int iItemNum, int iSubitemNum, object obj)
        {
            return true;
        }
        public bool IsActive = false;
        private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (listView.SelectedIndices.Count > 0)
            {
                IsActive = listView.Items[listView.SelectedIndices[0]].SubItems[4].Text == "X";
                tsb_delete.Visible = true;
                tsb_disable.Visible = true;
                tsb_tag.Visible = true;
                tsb_disable.Visible = IsActive;
                tsb_undo_disable.Visible = !IsActive;
            }
            else
                HideButtons();
        }
        private void HideButtons()
        {
            tsb_delete.Visible = false;
            tsb_disable.Visible = false;
            tsb_tag.Visible = false;
            tsb_undo_disable.Visible = false;
        }
        private void Operatori_Load(object sender, System.EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Globals.LocalizzaDialog(this);
                tsb_tag.Enabled = Globals.bBadgeActive;
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

            var query = $"SELECT OPERATORI.ID AS ID, OPERATORI.MATRICOLA, OPERATORI.COGNOME, OPERATORI.NOME, OPERATORI.TAG, " +
                $"OPERATORI.DISATTIVATO, OPERATORI.ID_UTENTE, OPERATORI.TAG, UTENTI.USERNAME AS USERNAME, UTENTI.ID_GRUPPO AS ID_GRUPPO, GRUPPI.PERMESSI AS PERMESSI " +
                $"FROM OPERATORI LEFT OUTER JOIN UTENTI ON OPERATORI.ID_UTENTE = UTENTI.ID LEFT OUTER JOIN GRUPPI ON GRUPPI.ID = UTENTI.ID_GRUPPO " +
                $"INNER JOIN OPERATORI_SEDI ON OPERATORI.ID = OPERATORI_SEDI.IDOPERATORE " +
                $"WHERE OPERATORI_SEDI.IDSEDE = {KleanTrak.Globals.IDSEDE} ORDER BY OPERATORI.COGNOME";
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    AddListviewItem(rdr);
                Globals.ResizeList(this, listView);
                HideButtons();
            }
            catch (OdbcException Ex)
            {
                Globals.WarnAndLog(Ex);
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        private void AddListviewItem(OdbcDataReader readerTemp)
        {
            ListViewItem lvItem = listView.Items.Add(readerTemp.GetStringEx("USERNAME", "NO USER DEFINED")); //username utente

            lvItem.Tag = readerTemp.GetIntEx("ID");

            if (readerTemp.IsDBNull(readerTemp.GetOrdinal("Cognome")))
                lvItem.SubItems.Add("");
            else
                lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("Cognome")));

            if (readerTemp.IsDBNull(readerTemp.GetOrdinal("Nome")))
                lvItem.SubItems.Add("");
            else
                lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("Nome")));

            if (readerTemp.IsDBNull(readerTemp.GetOrdinal("Matricola")))
                lvItem.SubItems.Add("");
            else
                lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("Matricola")));

            lvItem.SubItems.Add(readerTemp.GetIntEx("DISATTIVATO") == 0 ? "X" : "");

            if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("TAG")))
                lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("TAG")));
            else
                lvItem.SubItems.Add("");
        }
        private void Click_colonna(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            ((ListViewEx.ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
        }
        private void cancella_operatore()
        {
            OdbcCommand cmd = null;
            try
            {
                if (listView.SelectedItems.Count == 0)
                {
                    MessageBox.Show(KleanTrak.Globals.strTable[88],
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
                if (MessageBox.Show(Globals.strTable[89],
                    "Clean Track",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                    return;
                int operator_id = (int)listView.Items[listView.SelectedIndices[0]].Tag;
                //verifica che l' operatore esame non abbia già effettuato esami passati
                cmd = new OdbcCommand($"SELECT COUNT(*) FROM CICLI " +
                    $"LEFT OUTER JOIN OPERATORI ON CICLI.IDOPERATOREESAME = OPERATORI.ID " +
                    $"WHERE OPERATORI.ID  = {operator_id}",
                    DBUtil.GetODBCConnection());
                if ((int)cmd.ExecuteScalar() > 0)
                {
                    MessageBox.Show(Globals.strTable[90],
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }
                //verifica tentativo eliminazione operatore sconosciuto
                cmd.CommandText = $"SELECT COGNOME FROM OPERATORI WHERE ID = {operator_id}";
                string cognome = (string)cmd.ExecuteScalar();
                if (cognome.ToUpper() == WasherManager._UNKNOWN_OPERATOR.ToUpper())
                {
                    MessageBox.Show(Globals.strTable[129],
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
                //verifica che l'operatore STERILIZZAZIONE non abbia già effettuato esami passati
                cmd.CommandText = $"SELECT COUNT(*) " +
                    $"FROM CICLI LEFT OUTER JOIN OPERATORI " +
                    $"ON CICLI.IDOPERATOREINIZIOSTERILIZZAZIO = OPERATORI.ID " +
                    $"WHERE OPERATORI.ID  = {operator_id}";
                if ((int)cmd.ExecuteScalar() > 0)
                {
                    MessageBox.Show(Globals.strTable[91],
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    return;
                }
                //
                cmd.CommandText = $"DELETE FROM OPERATORI WHERE ID = {operator_id}";
                cmd.ExecuteNonQuery();
                DBUtil.InsertDbLog("OPERATORI", DBUtil.LogOperation.Delete, operator_id);
                listView.Items.RemoveAt(listView.SelectedIndices[0]);
            }
            catch (Exception e)
            {
                Globals.WarnAndLog(e);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        private void listView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                cancella_operatore();
        }
        public bool IsTagged(string ID)
        {
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTag = new OdbcCommand("", connTemp);
            OdbcDataReader readerTag = null;
            commTag.CommandText = "SELECT count(ID) as num FROM OPERATORI WHERE TAG IS NOT NULL AND ID = " + ID;
            //listView.Items[iItemNum].Tag.ToString() + ")";

            bool flag = false;
            try
            {
                readerTag = commTag.ExecuteReader();

                if (readerTag.Read())
                {
                    int count = -1;
                    try
                    {
                        count = readerTag.GetIntEx("num");
                        flag = count > 0;
                    }
                    catch (Exception)
                    {
                        flag = false;
                    }
                }
            }
            catch (Exception)
            {
                flag = false;
            }

            readerTag.Close();
            commTag.Dispose();
            connTemp.Close();

            return flag;
        }
        public bool tagThisOperator(string ID)
        {
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTag = new OdbcCommand("", connTemp);
            commTag.CommandText = "UPDATE OPERATORI SET TAG='" + DateTime.Now.Ticks + "' WHERE ID = " + ID;

            bool flag = false;
            try
            {
                commTag.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception)
            {
                flag = false;
            }

            commTag.Dispose();
            connTemp.Close();

            return flag;
        }
        private void Operatori_ClientSizeChanged(object sender, EventArgs e) => Globals.ResizeList(this, listView);
        private void tsb_close_Click(object sender, EventArgs e) => Close();
        private void tsb_delete_Click(object sender, EventArgs e) => cancella_operatore();
        private void tsb_add_Click(object sender, EventArgs e)
        {
            try
            {
                checkutente dlg = new checkutente();
                dlg.ShowDialog();
                RiempiLista();
                Globals.ResizeList(this, listView, false);
                if (Globals.Insert_check_utente >= 0)
                {
                    int newIdx = -1;
                    try
                    {
                        for (int i = 0; i < listView.Items.Count; i++)
                        {
                            if (Convert.ToInt32(listView.Items[i].Tag) == Globals.Insert_check_utente)
                            {
                                newIdx = i;
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (newIdx >= 0)
                    {
                        listView.EnsureVisible(newIdx);
                        listView.EditCell(newIdx, 1);
                    }
                    Globals.Insert_check_utente = -1;
                }
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex);
            }
        }
        private void tsb_tag_Click(object sender, EventArgs e)
        {
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                //associazione TAG
                if (listView.SelectedItems.Count == 0)
                {
                    MessageBox.Show(Globals.strTable[95],
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                FormAssociaTag dlg = new FormAssociaTag();
                string rfid = "";
                dlg.FormClosing += (s, ea) => { rfid = dlg.m_RFID; };
                dlg.ShowDialog();
                if (dlg.DialogResult == DialogResult.Cancel)
                    return;
                cmd = new OdbcCommand($"SELECT * FROM operatori WHERE TAG = '{rfid}'", DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    string operator_name = $"{rdr.GetStringEx("COGNOME")} " +
                        $"{rdr.GetStringEx("NOME")} - " +
                        $"{Globals.strTable[186]}: {rdr.GetBoolEx("DISATTIVATO")}";
                    MessageBox.Show($"{Globals.strTable[187]} {operator_name}",
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
                rdr.Close();
                cmd.CommandText = $"SELECT * FROM DISPOSITIVI WHERE TAG ='{rfid}'";
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    string device_name = $"{rdr.GetStringEx("DESCRIZIONE")} - " +
                        $"{Globals.strTable[188]}: {rdr.GetBoolEx("DISMESSO")}";
                    MessageBox.Show($"{Globals.strTable[187]} {device_name}",
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
                rdr.Close();
                cmd.CommandText = $"UPDATE OPERATORI SET TAG = '{rfid}' WHERE ID = {listView.SelectedItems[0].Tag}";
                cmd.ExecuteNonQuery();
                RiempiLista();
                dlg.Dispose();
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
        private bool SetOperatorState(int operator_id, bool disabled)
        {
            OdbcCommand cmd = null;
            try
            {
                if ((disabled && !IsActive) || (!disabled && IsActive))
                    return false;
                if (listView.SelectedIndices.Count == 0)
                    return false;
                if (MessageBox.Show((disabled) ? Globals.strTable[83] : Globals.strTable[82],
                    "Clean Track",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                    return false;
                string query = $"UPDATE OPERATORI " +
                    $"SET DISATTIVATO = {((disabled) ? 1 : 0)}, TAG = NULL " +
                    $"WHERE ID = {operator_id}";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        private void tsb_disable_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count == 0)
                {
                    MessageBox.Show(Globals.strTable[189],
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (SetOperatorState((int)listView.SelectedItems[0].Tag, true))
                    RiempiLista();
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex);
            }
        }
        private void tsb_undo_disable_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView.SelectedItems.Count == 0)
                {
                    MessageBox.Show(Globals.strTable[189],
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                if (SetOperatorState((int)listView.SelectedItems[0].Tag, false))
                    RiempiLista();
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex);
            }
        }
    }
}
