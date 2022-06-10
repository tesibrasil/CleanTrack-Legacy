using KleanTrak.Model;
using LibLog;
using OdbcExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Windows.Forms;

namespace KleanTrak
{
    public class Dispositivi : System.Windows.Forms.Form
    {
        private ListViewEx.ListViewEx listView;
        private System.Windows.Forms.ColumnHeader columnMatricola;
        private System.Windows.Forms.ColumnHeader columnDispositivo;
        private System.Windows.Forms.ColumnHeader ColumnSeriale;
        private System.Windows.Forms.ColumnHeader columnTipo;
        private System.Windows.Forms.ColumnHeader columnFornitore;
        private System.Windows.Forms.ColumnHeader columnStato;
        private System.Windows.Forms.ColumnHeader columnNumEsami;
        private System.Windows.Forms.ColumnHeader columnDataInizio;
        private System.Windows.Forms.ColumnHeader columnTAG;
        private System.ComponentModel.IContainer components = null;

        private ArrayList m_listTipo = null;
        private ArrayList m_listFornitore = null;
        private ArrayList m_listStato = null;
        private ToolStripContainer toolStripContainer1;
        private ToolStrip action_toolstrip;
        private ToolStripButton tsb_add;
        private ToolStripButton tsb_delete;
        private ToolStripButton tsb_rfid;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton tsb_dismiss;
        private ToolStripButton tsb_undo_dismiss;
        private ToolStripButton tsb_blocked_devices;
        private ToolStripSeparator ts_sep_windows_actions;
        private ToolStripButton tsb_find;
        private ToolStripButton tsb_registry;
        private ToolStripButton tsb_print;
        private ToolStripButton tsb_close;
        private ToolStripSeparator ts_sep_label;
        private ToolStripButton tsb_active_devices;
        private ToolStripButton tsbdevicecycles;
        private bool show_active_devices = true;

        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private void ManageToolstripButtons()
        {
            try
            {
                int selection_num = listView.SelectedItems.Count;
                tsb_add.Visible = show_active_devices;
                tsb_delete.Visible = !show_active_devices && selection_num > 0;
                tsb_rfid.Visible = Globals.bBadgeActive && selection_num > 0;
                tsb_blocked_devices.Visible = show_active_devices;
                tsb_active_devices.Visible = !show_active_devices;
                tsb_dismiss.Visible = show_active_devices && selection_num > 0;
                tsb_undo_dismiss.Visible = !show_active_devices && selection_num > 0;
                tsb_find.Visible = show_active_devices;
                tsb_registry.Visible = selection_num > 0;
                tsb_print.Visible = true;
                tsb_close.Visible = true;
            }
            catch (Exception e)
            {
                Globals.WarnAndLog(e);
            }
        }
        public Dispositivi()
        {
            InitializeComponent();
            listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(1, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(2, ListViewEx.ListViewEx.FieldType.textbox, null);
            ArrayList list_tipo = new ArrayList();
            listView.SetSubItemType(3, ListViewEx.ListViewEx.FieldType.combo, ComboTipiDispositivi());
            ArrayList list_fornitore = new ArrayList();
            listView.SetSubItemType(4, ListViewEx.ListViewEx.FieldType.combo, ComboFornitori());
            ArrayList list_stato = new ArrayList();
            listView.SetSubItemType(5, ListViewEx.ListViewEx.FieldType.combo, ComboStato());
            listView.SetSubItemType(6, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(7, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(8, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetEndEditCallback(new ListViewEx.ListViewEx.EndEditingCallbackEdit(EndEditDispositivo), new ListViewEx.ListViewEx.EndEditingCallbackCombo(EndEditCombo), null, null);
            ManageToolstripButtons();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dispositivi));
            this.listView = new ListViewEx.ListViewEx();
            this.columnMatricola = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDispositivo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnSeriale = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTipo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFornitore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnStato = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnNumEsami = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnDataInizio = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTAG = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.action_toolstrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ts_sep_windows_actions = new System.Windows.Forms.ToolStripSeparator();
            this.ts_sep_label = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_add = new System.Windows.Forms.ToolStripButton();
            this.tsb_delete = new System.Windows.Forms.ToolStripButton();
            this.tsb_rfid = new System.Windows.Forms.ToolStripButton();
            this.tsb_dismiss = new System.Windows.Forms.ToolStripButton();
            this.tsb_undo_dismiss = new System.Windows.Forms.ToolStripButton();
            this.tsb_active_devices = new System.Windows.Forms.ToolStripButton();
            this.tsb_blocked_devices = new System.Windows.Forms.ToolStripButton();
            this.tsb_find = new System.Windows.Forms.ToolStripButton();
            this.tsb_registry = new System.Windows.Forms.ToolStripButton();
            this.tsb_print = new System.Windows.Forms.ToolStripButton();
            this.tsb_close = new System.Windows.Forms.ToolStripButton();
            this.tsbdevicecycles = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.action_toolstrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnMatricola,
            this.columnDispositivo,
            this.ColumnSeriale,
            this.columnTipo,
            this.columnFornitore,
            this.columnStato,
            this.columnNumEsami,
            this.columnDataInizio,
            this.columnTAG});
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.GridLines = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.MultiSelect = false;
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(1474, 621);
            this.listView.TabIndex = 36;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
            // 
            // columnMatricola
            // 
            this.columnMatricola.Text = "Matricola ";
            this.columnMatricola.Width = 114;
            // 
            // columnDispositivo
            // 
            this.columnDispositivo.Text = "Dispositivo ";
            this.columnDispositivo.Width = 159;
            // 
            // ColumnSeriale
            // 
            this.ColumnSeriale.Text = "N° di serie ";
            this.ColumnSeriale.Width = 101;
            // 
            // columnTipo
            // 
            this.columnTipo.Text = "Tipo ";
            this.columnTipo.Width = 131;
            // 
            // columnFornitore
            // 
            this.columnFornitore.Text = "Fornitore ";
            this.columnFornitore.Width = 126;
            // 
            // columnStato
            // 
            this.columnStato.Text = "Stato ";
            this.columnStato.Width = 90;
            // 
            // columnNumEsami
            // 
            this.columnNumEsami.Text = "Nr. esami ";
            this.columnNumEsami.Width = 110;
            // 
            // columnDataInizio
            // 
            this.columnDataInizio.Text = "Data primo utilizzo ";
            this.columnDataInizio.Width = 223;
            // 
            // columnTAG
            // 
            this.columnTAG.Text = "TAG ";
            this.columnTAG.Width = 112;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.listView);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1474, 621);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1474, 676);
            this.toolStripContainer1.TabIndex = 125;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.action_toolstrip);
            // 
            // action_toolstrip
            // 
            this.action_toolstrip.BackColor = System.Drawing.SystemColors.Control;
            this.action_toolstrip.Dock = System.Windows.Forms.DockStyle.None;
            this.action_toolstrip.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.action_toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_add,
            this.tsb_delete,
            this.tsb_rfid,
            this.toolStripSeparator1,
            this.tsb_dismiss,
            this.tsb_undo_dismiss,
            this.tsb_active_devices,
            this.tsb_blocked_devices,
            this.ts_sep_windows_actions,
            this.tsb_find,
            this.tsb_registry,
            this.tsb_print,
            this.tsb_close,
            this.ts_sep_label,
            this.tsbdevicecycles});
            this.action_toolstrip.Location = new System.Drawing.Point(3, 0);
            this.action_toolstrip.Name = "action_toolstrip";
            this.action_toolstrip.Size = new System.Drawing.Size(685, 55);
            this.action_toolstrip.TabIndex = 0;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 55);
            // 
            // ts_sep_windows_actions
            // 
            this.ts_sep_windows_actions.Name = "ts_sep_windows_actions";
            this.ts_sep_windows_actions.Size = new System.Drawing.Size(6, 55);
            // 
            // ts_sep_label
            // 
            this.ts_sep_label.Name = "ts_sep_label";
            this.ts_sep_label.Size = new System.Drawing.Size(6, 55);
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
            this.tsb_delete.Text = "toolStripButton3";
            this.tsb_delete.Click += new System.EventHandler(this.tsb_delete_Click);
            // 
            // tsb_rfid
            // 
            this.tsb_rfid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_rfid.Image = global::kleanTrak.Properties.Resources.rf;
            this.tsb_rfid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_rfid.Name = "tsb_rfid";
            this.tsb_rfid.Size = new System.Drawing.Size(52, 52);
            this.tsb_rfid.Text = "toolStripButton4";
            this.tsb_rfid.Click += new System.EventHandler(this.tsb_rfid_Click);
            // 
            // tsb_dismiss
            // 
            this.tsb_dismiss.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_dismiss.Image = global::kleanTrak.Properties.Resources.block;
            this.tsb_dismiss.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_dismiss.Name = "tsb_dismiss";
            this.tsb_dismiss.Size = new System.Drawing.Size(52, 52);
            this.tsb_dismiss.Text = "toolStripButton5";
            this.tsb_dismiss.Click += new System.EventHandler(this.tsb_dismiss_Click);
            // 
            // tsb_undo_dismiss
            // 
            this.tsb_undo_dismiss.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_undo_dismiss.Image = global::kleanTrak.Properties.Resources.check;
            this.tsb_undo_dismiss.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_undo_dismiss.Name = "tsb_undo_dismiss";
            this.tsb_undo_dismiss.Size = new System.Drawing.Size(52, 52);
            this.tsb_undo_dismiss.Text = "toolStripButton8";
            this.tsb_undo_dismiss.Click += new System.EventHandler(this.tsb_undo_dismiss_Click);
            // 
            // tsb_active_devices
            // 
            this.tsb_active_devices.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_active_devices.Image = global::kleanTrak.Properties.Resources.check2;
            this.tsb_active_devices.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_active_devices.Name = "tsb_active_devices";
            this.tsb_active_devices.Size = new System.Drawing.Size(52, 52);
            this.tsb_active_devices.Text = "toolStripButton1";
            this.tsb_active_devices.Click += new System.EventHandler(this.tsb_active_devices_Click);
            // 
            // tsb_blocked_devices
            // 
            this.tsb_blocked_devices.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_blocked_devices.Image = global::kleanTrak.Properties.Resources.barrier;
            this.tsb_blocked_devices.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_blocked_devices.Name = "tsb_blocked_devices";
            this.tsb_blocked_devices.Size = new System.Drawing.Size(52, 52);
            this.tsb_blocked_devices.Text = "toolStripButton9";
            this.tsb_blocked_devices.Click += new System.EventHandler(this.tsb_blocked_devices_Click);
            // 
            // tsb_find
            // 
            this.tsb_find.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_find.Image = global::kleanTrak.Properties.Resources.magnify;
            this.tsb_find.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_find.Name = "tsb_find";
            this.tsb_find.Size = new System.Drawing.Size(52, 52);
            this.tsb_find.Text = "toolStripButton6";
            this.tsb_find.Click += new System.EventHandler(this.tsb_find_Click);
            // 
            // tsb_registry
            // 
            this.tsb_registry.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_registry.Image = global::kleanTrak.Properties.Resources.clipboard;
            this.tsb_registry.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_registry.Name = "tsb_registry";
            this.tsb_registry.Size = new System.Drawing.Size(52, 52);
            this.tsb_registry.Text = "toolStripButton7";
            this.tsb_registry.Click += new System.EventHandler(this.tsb_registry_Click);
            // 
            // tsb_print
            // 
            this.tsb_print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_print.Image = global::kleanTrak.Properties.Resources.printer;
            this.tsb_print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_print.Name = "tsb_print";
            this.tsb_print.Size = new System.Drawing.Size(52, 52);
            this.tsb_print.Text = "toolStripButton10";
            this.tsb_print.Click += new System.EventHandler(this.tsb_print_Click);
            // 
            // tsb_close
            // 
            this.tsb_close.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_close.Image = ((System.Drawing.Image)(resources.GetObject("tsb_close.Image")));
            this.tsb_close.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_close.Name = "tsb_close";
            this.tsb_close.Size = new System.Drawing.Size(52, 52);
            this.tsb_close.Text = "toolStripButton11";
            this.tsb_close.Click += new System.EventHandler(this.tsb_close_Click);
            // 
            // tsbdevicecycles
            // 
            this.tsbdevicecycles.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbdevicecycles.Image = global::kleanTrak.Properties.Resources.cycles;
            this.tsbdevicecycles.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbdevicecycles.Name = "tsbdevicecycles";
            this.tsbdevicecycles.Size = new System.Drawing.Size(52, 52);
            this.tsbdevicecycles.ToolTipText = "Cicli Dispositivo";
            this.tsbdevicecycles.Click += new System.EventHandler(this.tsbdevicecycles_Click);
            // 
            // Dispositivi
            // 
            this.ClientSize = new System.Drawing.Size(1474, 676);
            this.ControlBox = false;
            this.Controls.Add(this.toolStripContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "Dispositivi";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CLEAN TRACK - Dispositivi in uso";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Dispositivi_Closing);
            this.Load += new System.EventHandler(this.Dispositivi_Load);
            this.ClientSizeChanged += new System.EventHandler(this.Dispositivi_ClientSizeChanged);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.action_toolstrip.ResumeLayout(false);
            this.action_toolstrip.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion
        private bool EndEditCombo(int iItemNum, int iSubitemNum, object obj)
        {
            if (obj == null)
                return false;

            switch (iSubitemNum)
            {
                case 3:
                    EndEdit3(iItemNum, iSubitemNum, obj);
                    break;
                case 4:
                    EndEdit4(iItemNum, iSubitemNum, obj);
                    break;
                case 5:
                    EndEdit5(iItemNum, iSubitemNum, obj);
                    break;
            }

            return true;
        }
        private void EndEdit5(int iItemNum, int iSubitemNum, object obj)
        {
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            try
            {
                //valore Originale
                string ValoreOriginale = "";
                string queryvalorig = "select STATO.DESCRIZIONE from dispositivi inner join stato on dispositivi.stato= stato.id where dispositivi.id = " + listView.Items[iItemNum].Tag.ToString();
                OdbcCommand commValoreOriginale = new OdbcCommand(queryvalorig, connTemp);
                OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                if (readerValoreOriginale.Read())
                {
                    if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("descrizione")))
                        ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("descrizione")).ToString();
                }
                if (readerValoreOriginale != null && readerValoreOriginale.IsClosed == false)
                    readerValoreOriginale.Close();
                OdbcCommand commTemp = new OdbcCommand("", connTemp);
                Item_Stato Item = (Item_Stato)obj;
                string query = "UPDATE Dispositivi SET stato = " + Item.id.ToString() + " WHERE ID=" + listView.Items[iItemNum].Tag.ToString();
                commTemp = new OdbcCommand(query, connTemp);
                commTemp.ExecuteNonQuery();
                string ValoreModificato = "";
                string queryvalmod = "SELECT DESCRIZIONE FROM STATO WHERE ID='" + Item.id.ToString() + "' ";
                OdbcCommand commValoreModificato = new OdbcCommand(queryvalmod, connTemp);
                OdbcDataReader readerValoreModificato = commValoreModificato.ExecuteReader();
                if (readerValoreModificato.Read())
                {
                    if (!readerValoreModificato.IsDBNull(readerValoreModificato.GetOrdinal("DESCRIZIONE")))
                        ValoreModificato = readerValoreModificato.GetValue(readerValoreModificato.GetOrdinal("DESCRIZIONE")).ToString();
                }
                if ((readerValoreModificato != null) && (readerValoreModificato.IsClosed == false))
                    readerValoreModificato.Close();
                if (ValoreOriginale == ValoreModificato)
                    return;
                string querylog = "INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                            " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now).ToUpper() + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'STATO', '" + ValoreOriginale.Replace("'", "''") + "', '" + ValoreModificato.Replace("'", "''") + "')";
                OdbcCommand commTemp_LOG = new OdbcCommand(querylog, connTemp);
                commTemp_LOG.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connTemp.Close();
        }
        private void EndEdit4(int iItemNum, int iSubitemNum, object obj)
        {
            OdbcConnection connTemp = DBUtil.GetODBCConnection();

            try
            {
                OdbcCommand commTemp = new OdbcCommand("", connTemp);
                Item_Fornitore Item = (Item_Fornitore)obj;
                string query = "UPDATE Dispositivi SET idfornitore = '" + Item.id.ToString() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString();
                commTemp = new OdbcCommand(query, connTemp);
                commTemp.ExecuteNonQuery();

                string ValoreModificato = "";
                string queryfornitore = "SELECT FORNITORI.DESCRIZIONE AS DESCRIZIONE FROM FORNITORI WHERE FORNITORI.ID='" + Item.id.ToString() + "'";
                OdbcCommand commValoreModificato = new OdbcCommand(queryfornitore, connTemp);
                OdbcDataReader readerValoreModificato = commValoreModificato.ExecuteReader();
                if (readerValoreModificato.Read())
                {
                    if (!readerValoreModificato.IsDBNull(readerValoreModificato.GetOrdinal("DESCRIZIONE")))
                        ValoreModificato = readerValoreModificato.GetValue(readerValoreModificato.GetOrdinal("DESCRIZIONE")).ToString();
                }

                if ((readerValoreModificato != null) && (readerValoreModificato.IsClosed == false))
                    readerValoreModificato.Close();

                //valore Originale
                string ValoreOriginale = "";
                string queryvalorig = "select FORNITORI.descrizione AS DESCRIZIONE from FORNITORI inner join dispositivi on FORNITORI.id= dispositivi.IDFORNITORE where dispositivi.id = " + listView.Items[iItemNum].Tag.ToString();
                OdbcCommand commValoreOriginale = new OdbcCommand(queryvalorig, connTemp);
                OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                if (readerValoreOriginale.Read())
                {
                    if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("DESCRIZIONE")))
                        ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("DESCRIZIONE")).ToString();
                }

                if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                    readerValoreOriginale.Close();

                if (ValoreOriginale == ValoreModificato) //se non viene modificato il valore non effettuo la query nel LOG.
                    return;

                string querylog = "INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                            " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now).ToUpper() + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'FORNITORE', '" + ValoreOriginale.Replace("'", "''") + "', '" + ValoreModificato.Replace("'", "''") + "')";
                OdbcCommand commTemp_LOG = new OdbcCommand(querylog, connTemp);
                commTemp_LOG.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }


            connTemp.Close();
        }
        private void EndEdit3(int iItemNum, int iSubitemNum, object obj)
        {
            OdbcConnection connTemp = DBUtil.GetODBCConnection();

            try
            {

                // update tipo
                OdbcCommand commTemp = new OdbcCommand("", connTemp);
                Item_TipiDispositivi Item = (Item_TipiDispositivi)obj;
                string query = "UPDATE Dispositivi SET idtipo = " + Item.id.ToString() + " WHERE ID=" + listView.Items[iItemNum].Tag.ToString();
                commTemp = new OdbcCommand(query, connTemp);
                commTemp.ExecuteNonQuery();

                // valore modificato
                string ValoreModificato = "";
                string queryvalmod = "SELECT TIPIDISPOSITIVI.DESCRIZIONE as idtipo FROM TIPIDISPOSITIVI  WHERE TIPIDISPOSITIVI.ID = '" + Item.id.ToString() + "' ";
                OdbcCommand commValoreModificato = new OdbcCommand(queryvalmod, connTemp);
                OdbcDataReader readerValoreModificato = commValoreModificato.ExecuteReader();
                if (readerValoreModificato.Read())
                {
                    if (!readerValoreModificato.IsDBNull(readerValoreModificato.GetOrdinal("idtipo")))
                        ValoreModificato = readerValoreModificato.GetValue(readerValoreModificato.GetOrdinal("idtipo")).ToString();
                }
                if ((readerValoreModificato != null) && (readerValoreModificato.IsClosed == false))
                    readerValoreModificato.Close();

                //valore Originale
                string ValoreOriginale = "";
                string queryvalorig = "select tipidispositivi.descrizione from TIPIDISPOSITIVI inner join dispositivi on tipidispositivi.id= dispositivi.idtipo where dispositivi.id = " + listView.Items[iItemNum].Tag.ToString();
                OdbcCommand commValoreOriginale = new OdbcCommand(queryvalorig, connTemp);
                OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                if (readerValoreOriginale.Read())
                {
                    if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("descrizione")))
                        ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("descrizione")).ToString();
                }
                if (readerValoreOriginale != null && readerValoreOriginale.IsClosed == false)
                    readerValoreOriginale.Close();

                if (ValoreOriginale == ValoreModificato) //se non viene modificato il valore non effettuo la query nel LOG.
                    return;

                // ins log
                string querylog = "INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                            " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now).ToUpper() + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'TIPO DISPOSITIVO', '" + ValoreOriginale.Replace("'", "''") + "', '" + ValoreModificato.Replace("'", "''") + "')";
                OdbcCommand commTemp_LOG = new OdbcCommand(querylog, connTemp);
                commTemp_LOG.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            connTemp.Close();
        }
        private bool EndEditDispositivo(int iItemNum, int iSubitemNum, string strText)
        {
            bool bReturn = false;
            int termina_ciclo = -1; //variabile che tiene conto se l'RFID uinserito è gia stato utilizzato.
            switch (iSubitemNum)
            {
                case 0:
                    {
                        if (MatricolaPresente(strText, (int)listView.Items[iItemNum].Tag))
                        {
                            MessageBox.Show(KleanTrak.Globals.strTable[207], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        OdbcConnection connTemp = DBUtil.GetODBCConnection();
                        OdbcCommand commTemp = new OdbcCommand("", connTemp);
                        //verifica che il nuovo RFID non corrisponda ad uno già stato inserito. 
                        string RFIDDispositivo = strText;
                        OdbcCommand commTemp_RFIDDispositivo = new OdbcCommand("SELECT Matricola from Dispositivi where Matricola  = '" + RFIDDispositivo.ToUpper() + "' and ID <> " + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerTemp = commTemp_RFIDDispositivo.ExecuteReader();

                        if (readerTemp.Read())
                        {
                            if (readerTemp.GetValue(readerTemp.GetOrdinal("Matricola")).ToString() != "Nuovo Codice")
                            {
                                MessageBox.Show(KleanTrak.Globals.strTable[15], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                termina_ciclo = 1;
                                strText = "";
                            }
                            else if (readerTemp.GetValue(readerTemp.GetOrdinal("Matricola")).ToString() == "Nuovo Codice")
                            {
                                termina_ciclo = 1;
                            }

                            if ((readerTemp != null) && (readerTemp.IsClosed == false))
                                readerTemp.Close();

                            if (termina_ciclo == -1)
                            {
                                commTemp.ExecuteNonQuery();
                                bReturn = true;
                            }

                            break;
                        }

                        if ((readerTemp != null) && (readerTemp.IsClosed == false))
                            readerTemp.Close();

                        commTemp = new OdbcCommand("UPDATE Dispositivi SET Matricola = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        if (strText.Length == 0)
                            listView.Items[iItemNum].BackColor = System.Drawing.Color.Coral;
                        else
                            listView.Items[iItemNum].BackColor = System.Drawing.Color.Transparent;
                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Matricola FROM Dispositivi WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
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
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now).ToUpper() + "', '" + listView.Items[iItemNum].Tag.ToString().ToUpper() + "', 'CODICE', '" + ValoreOriginale.Replace("'", "''").ToUpper() + "', '" + strText.Replace("'", "''").ToUpper() + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();

                        if (termina_ciclo == -1)
                        {
                            commTemp.ExecuteNonQuery();
                            bReturn = true;
                        }

                        connTemp.Close();
                        break;
                    }

                case 1:
                    {
                        OdbcConnection connTemp = DBUtil.GetODBCConnection();
                        OdbcCommand commTemp = new OdbcCommand("", connTemp);
                        commTemp = new OdbcCommand("UPDATE Dispositivi SET Descrizione = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Descrizione as Descrizione FROM Dispositivi WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Descrizione")))
                            {
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Descrizione")).ToString();
                            }
                        }
                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'DESCRIZIONE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();

                        if (termina_ciclo == -1)
                        {
                            commTemp.ExecuteNonQuery();
                            bReturn = true;
                        }

                        connTemp.Close();
                        break;
                    }

                case 2:
                    {
                        if (SerialePresente(strText, (int)listView.Items[iItemNum].Tag))
                        {
                            MessageBox.Show(KleanTrak.Globals.strTable[208], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        OdbcConnection connTemp = DBUtil.GetODBCConnection();
                        OdbcCommand commTemp = new OdbcCommand("", connTemp);
                        commTemp = new OdbcCommand("UPDATE Dispositivi SET Seriale = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);


                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Seriale as Seriale FROM Dispositivi WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Seriale")))
                            {
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Seriale")).ToString();
                            }
                        }
                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'NUMERO DI SERIE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();

                        if (termina_ciclo == -1)
                        {
                            commTemp.ExecuteNonQuery();
                            bReturn = true;
                        }

                        connTemp.Close();
                        break;
                    }

                case 6:
                    {
                        OdbcConnection connTemp = DBUtil.GetODBCConnection();
                        OdbcCommand commTemp = new OdbcCommand("", connTemp);
                        commTemp = new OdbcCommand("UPDATE Dispositivi SET Esamieseguiti = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT ESAMIESEGUITI as ESAMIESEGUITI FROM Dispositivi WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("ESAMIESEGUITI")))
                            {
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("ESAMIESEGUITI")).ToString();
                            }
                        }
                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'ESAMI ESEGUITI', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();

                        if (termina_ciclo == -1)
                        {
                            commTemp.ExecuteNonQuery();
                            bReturn = true;
                        }

                        connTemp.Close();
                        break;
                    }

                case 7:
                    {
                        OdbcConnection connTemp = DBUtil.GetODBCConnection();
                        try
                        {
                            string data = "";
                            string mese = "";
                            string giorno = "";
                            string anno = "";
                            string ora = "";
                            string minuti = "";
                            string secondi = "00";
                            int anno_num = -1;
                            int mese_num = -1;
                            int giorno_num = -1;

                            try
                            {
                                if (strText.Length == 10)
                                {
                                    anno = strText.Substring(6, 4);
                                    anno_num = Convert.ToInt32(anno);

                                    mese = strText.Substring(3, 2);
                                    mese_num = Convert.ToInt32(mese);

                                    giorno = strText.Substring(0, 2);
                                    giorno_num = Convert.ToInt32(giorno);
                                }
                                else if (strText.Length == 16)
                                {
                                    anno = strText.Substring(6, 4);
                                    anno_num = Convert.ToInt32(anno);

                                    mese = strText.Substring(3, 2);
                                    mese_num = Convert.ToInt32(mese);

                                    giorno = strText.Substring(0, 2);
                                    giorno_num = Convert.ToInt32(giorno);

                                    ora = strText.Substring(11, 2);
                                    minuti = strText.Substring(14, 2);
                                }
                            }
                            catch (OdbcException e)
                            {
                                MessageBox.Show(e.Message + KleanTrak.Globals.strTable[16], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                data = KleanTrak.Globals.ConvertDateTime(DateTime.Now);
                            }

                            //compongo stringa:
                            if (anno_num > 1950 && anno_num < 2100 && mese_num > 0 && mese_num < 13 && giorno_num > 0 && giorno_num < 32)
                            {
                                if (anno != "" && mese != "" && giorno != "" && ora != "" && minuti != "")
                                    data = anno + mese + giorno + ora + minuti + secondi;
                                else
                                    if (anno != "" && mese != "" && giorno != "" && (ora == "" || minuti == ""))
                                    data = anno + mese + giorno + "00" + "00" + "00";
                            }
                            else
                            {
                                data = KleanTrak.Globals.ConvertDateTime(DateTime.Now);
                                MessageBox.Show(KleanTrak.Globals.strTable[17], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            OdbcCommand commTemp = new OdbcCommand("", connTemp);
                            commTemp = new OdbcCommand("UPDATE Dispositivi SET Datainizio = '" + data.ToUpper() + "'  WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                            //INSERIMENTO NEL LOG DI SISTEMA
                            //valore originale
                            string ValoreOriginale = "";
                            OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Datainizio as Datainizio FROM Dispositivi WHERE ID=" + listView.Items[iItemNum].Tag.ToString().ToUpper(), connTemp);
                            OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                            if (readerValoreOriginale.Read())
                                if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Datainizio")))
                                    ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Datainizio")).ToString();

                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();

                            if (ValoreOriginale == data.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                                break;

                            OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                         " VALUES ('" + KleanTrak.Globals.m_strUser.ToUpper() + "', 'DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now).ToUpper() + "', '" + listView.Items[iItemNum].Tag.ToString().ToUpper() + "', 'DATA PRIMO UTILIZZO', '" + ValoreOriginale.Replace("'", "''").ToUpper() + "', '" + data.ToUpper() + "')", connTemp);
                            commTemp_LOG.ExecuteNonQuery();

                            if (termina_ciclo == -1)
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
                        break;
                    }

                case 8:
                    {
                        if (TagPresente(strText, (int)listView.Items[iItemNum].Tag))
                        {
                            MessageBox.Show(KleanTrak.Globals.strTable[209], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }
                        OdbcConnection connTemp = DBUtil.GetODBCConnection();
                        OdbcCommand commTemp = new OdbcCommand("", connTemp);
                        commTemp = new OdbcCommand("UPDATE Dispositivi SET Tag = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Tag FROM Dispositivi WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Tag")))
                            {
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Tag")).ToString();
                            }
                        }
                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();

                        if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'DISPOSITIVI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'TAG', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();

                        if (termina_ciclo == -1)
                        {
                            commTemp.ExecuteNonQuery();
                            bReturn = true;
                        }

                        connTemp.Close();
                        break;
                    }
            }

            return bReturn;
        }
        private bool MatricolaPresente(string strText, int id)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand("SELECT COUNT(*) FROM DISPOSITIVI WHERE UPPER(MATRICOLA) = UPPER(?) AND ID <> ? AND UPPER(MATRICOLA) <> 'NUOVO' AND ELIMINATO = 0", DBUtil.GetODBCConnection());
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
                cmd = new OdbcCommand("SELECT COUNT(*) FROM DISPOSITIVI WHERE UPPER(SERIALE) = UPPER(?) AND ID <> ? AND UPPER(SERIALE) <> 'NUOVO' AND ELIMINATO = 0", DBUtil.GetODBCConnection());
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
        private bool TagPresente(string strText, int id)
        {
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand("SELECT COUNT(*) FROM DISPOSITIVI WHERE UPPER(TAG) = UPPER(?) AND ID <> ? AND UPPER(TAG) <> 'NUOVO' AND ELIMINATO = 0", DBUtil.GetODBCConnection());
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
        public class Item_Fornitore
        {
            public long id;
            public string strName;
            public override string ToString() { return strName; }
        }
        public class Item_TipiDispositivi
        {
            public long id;
            public string strName;
            public override string ToString() { return strName; }
        }
        public class Item_Stato
        {
            public long id;
            public string strName;
            public override string ToString() { return strName; }
        }
        private ArrayList ComboStato()
        {
            ArrayList list_stato = new ArrayList();

            OdbcDataReader reader = null;

            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("Select id, descrizione from Stato", connTemp);

            try
            {
                reader = commTemp.ExecuteReader();

                while (reader.Read())
                {
                    Item_Stato Item = new Item_Stato();
                    Item.id = reader.GetIntEx("ID");

                    Item.strName = reader.GetValue(reader.GetOrdinal("descrizione")).ToString();
                    list_stato.Add(Item);
                }
            }
            catch (OdbcException e)
            {
                MessageBox.Show(e.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if ((reader != null) && (reader.IsClosed == false))
                reader.Close();

            connTemp.Close();

            return list_stato;

        }
        private ArrayList ComboTipiDispositivi()
        {
            ArrayList list_tipo = new ArrayList();
            OdbcDataReader reader = null;
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("Select id, descrizione from TipiDispositivi", connTemp);
            try
            {
                reader = commTemp.ExecuteReader();
                while (reader.Read())
                {
                    Item_TipiDispositivi Item = new Item_TipiDispositivi();
                    Item.id = reader.GetIntEx("ID");
                    Item.strName = reader.GetValue(reader.GetOrdinal("descrizione")).ToString();
                    list_tipo.Add(Item);
                }
            }
            catch (OdbcException e)
            {
                MessageBox.Show(e.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if ((reader != null) && (reader.IsClosed == false))
                reader.Close();

            connTemp.Close();

            return list_tipo;
        }
        private ArrayList ComboFornitori()
        {
            ArrayList list_fornitore = new ArrayList();
            OdbcDataReader rdr = null;
            OdbcCommand cmd = null;
            try
            {
                cmd = new OdbcCommand($"SELECT ID, DESCRIZIONE FROM FORNITORI", DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list_fornitore.Add(new Item_Fornitore
                    {
                        id = rdr.GetIntEx("ID"),
                        strName = rdr.GetStringEx("DESCRIZIONE")
                    });
                }
                return list_fornitore;
            }
            catch (OdbcException e)
            {
                MessageBox.Show(e.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        private void menuTipiDispositivi_Click(object sender, System.EventArgs e)
        {
            TipiDispositivi dlg = new TipiDispositivi();
            dlg.ShowDialog();
        }
        private void menuFornitori_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void btnChiudi_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
        private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ManageToolstripButtons();
        }
        private void Dispositivi_Load(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Globals.LocalizzaDialog(this);
                listView.ListViewItemSorter = new ListViewEx.ListViewExComparer(listView);
                Text = Globals.strTable[117];
                tsb_blocked_devices.ToolTipText = Globals.strTable[119];
                m_listTipo = new ArrayList();
                m_listFornitore = new ArrayList();
                m_listStato = new ArrayList();
                RiempiLista();
                RiempiCombos();
                SetTooltips();
                Cursor.Current = Cursors.Default;
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
        private void SetTooltips()
        {
            try
            {
                tsb_add.ToolTipText = Globals.strTable[145];
                tsb_delete.ToolTipText = Globals.strTable[146];
                tsb_rfid.ToolTipText = Globals.strTable[147];
                tsb_dismiss.ToolTipText = Globals.strTable[148];
                tsb_undo_dismiss.ToolTipText = Globals.strTable[149];
                tsb_active_devices.ToolTipText = Globals.strTable[150];
                tsb_blocked_devices.ToolTipText = Globals.strTable[151];
                tsb_find.ToolTipText = Globals.strTable[152];
                tsb_registry.ToolTipText = Globals.strTable[153];
                tsb_print.ToolTipText = Globals.strTable[154];
                tsb_close.ToolTipText = Globals.strTable[155];
            }
            catch (Exception e)
            {
                Globals.WarnAndLog(e);
            }
        }
        private void RiempiLista()
        {
            Dictionary<int, string> stateList = GetStateList();
            listView.Items.Clear();
            string query = "SELECT Dispositivi.ID, Dispositivi.matricola, Dispositivi.Seriale as seriale, Dispositivi.Descrizione, Dispositivi.tag, ";
            query += " TipiDispositivi.Descrizione AS TipoDispositivo, Fornitori.Descrizione AS Fornitore, Stato, EsamiEseguiti, DataInizio, DataFine ";
            query += " FROM Dispositivi LEFT OUTER JOIN TipiDispositivi ON Dispositivi.IDTipo=TipiDispositivi.ID LEFT OUTER JOIN Fornitori ON ";
            query += " Dispositivi.IDFORNITORE = Fornitori.ID WHERE DISMESSO = 0 AND Dispositivi.ELIMINATO = 0 AND Dispositivi.IDSEDE = " + KleanTrak.Globals.IDSEDE.ToString() +
                " ORDER BY Dispositivi.ID, Dispositivi.Matricola";
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            try
            {
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                rdr = cmd.ExecuteReader();
                while (rdr.Read())
                    AddListViewItem(rdr, stateList);
                listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.textbox, null);
            }
            catch (OdbcException Ex)
            {
                MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        private void AddListViewItem(OdbcDataReader readerTemp, Dictionary<int, string> stateList)
        {
            try
            {
                var matricola = readerTemp.GetStringEx("MATRICOLA");
                ListViewItem lvItem = listView.Items.Add(matricola);
                lvItem.SubItems.Add(readerTemp.GetStringEx("DESCRIZIONE"));
                lvItem.Tag = readerTemp.GetIntEx("ID");
                lvItem.SubItems.Add(readerTemp.GetStringEx("SERIALE"));
                lvItem.SubItems.Add(readerTemp.GetStringEx("TIPODISPOSITIVO"));
                lvItem.SubItems.Add(readerTemp.GetStringEx("FORNITORE"));
                int nStato = readerTemp.GetIntEx("STATO");
                string stateDesc = "";
                lvItem.SubItems.Add(stateList.TryGetValue(nStato, out stateDesc) ? stateDesc : "???");
                lvItem.SubItems.Add(readerTemp.GetIntEx("EsamiEseguiti").ToString());
                if (readerTemp.IsDBNull(readerTemp.GetOrdinal("DataInizio")))
                    lvItem.SubItems.Add("");
                else
                    lvItem.SubItems.Add(KleanTrak.Globals.ConvertDateTime(readerTemp.GetString(readerTemp.GetOrdinal("DataInizio"))));
                if (readerTemp.IsDBNull(readerTemp.GetOrdinal("TAG")))
                    lvItem.SubItems.Add("");
                else
                    lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("TAG")));
                if (matricola == "")
                    lvItem.BackColor = System.Drawing.Color.Coral;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private Dictionary<int, string> GetStateList()
        {
            var result = new Dictionary<int, string>();
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("SELECT ID, DESCRIZIONE FROM STATO", connTemp);
            OdbcDataReader readerTemp = null;
            try
            {
                readerTemp = commTemp.ExecuteReader();
                while (readerTemp.Read())
                    result.Add(readerTemp.GetIntEx("ID"), readerTemp.GetStringEx("DESCRIZIONE"));
            }
            catch (OdbcException Ex)
            {
                MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }
        private List<ComboItem> _causali = new List<ComboItem>();
        private void RiempiCombos()
        {
            // caricamento dei dati dalla combo
            _causali.Clear();
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("SELECT ID, Causale FROM Causali", connTemp);
            OdbcDataReader readerTemp = null;
            try
            {
                readerTemp = commTemp.ExecuteReader();
                while (readerTemp.Read())
                {
                    ComboItem Item = new ComboItem();
                    Item.Id = readerTemp.GetIntEx("ID");
                    Item.Description = readerTemp.GetStringEx("Causale");
                    _causali.Add(Item);
                }
            }
            catch (OdbcException Ex)
            {
                MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (readerTemp != null && readerTemp.IsClosed == false)
                readerTemp.Close();

            connTemp.Close();
        }
        private void listView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (listView.SelectedIndices.Count == 0)
                return;
            if (e.KeyCode != Keys.Delete)
                return;
            var result = MessageBox.Show(KleanTrak.Globals.strTable[22],
                "Clean Track",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.No)
                return;
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            //verifico che il dispositivo non sia già stato utilizzato
            OdbcCommand commTemp_check = new OdbcCommand("SELECT Cicli.iddispositivo from Cicli " +
                "INNER JOIN dispositivi ON Cicli.iddispositivo = dispositivi.ID " +
                         "where dispositivi.ID  = " + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "", connTemp);
            OdbcDataReader readerTemp = commTemp_check.ExecuteReader();
            if (readerTemp.Read())
            {
                MessageBox.Show(KleanTrak.Globals.strTable[23], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                readerTemp.Close();
                return;
            }
            if ((readerTemp != null) && (readerTemp.IsClosed == false))
                readerTemp.Close();
            OdbcCommand commTemp = new OdbcCommand("DELETE FROM Dispositivi WHERE ID=" + listView.Items[listView.SelectedIndices[0]].Tag.ToString(), connTemp);
            commTemp.ExecuteNonQuery();
            //INSERIMENTO NEL LOG DI SISTEMA
            OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD ) VALUES ('" + KleanTrak.Globals.m_strUser + "', 'DISPOSITIVI', 'Cancellazione', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now).ToUpper() + "', '" + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "')", connTemp);
            commTemp_LOG.ExecuteNonQuery();
            listView.Items.RemoveAt(listView.SelectedIndices[0]);
            connTemp.Close();
        }
        private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
        {
            ((ListViewEx.ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
        }
        private void Dispositivi_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OdbcConnection connTemp = null;
            OdbcCommand commTemp_RFIDDispositivo = null;
            OdbcDataReader readerTemp = null;
            try
            {
                connTemp = DBUtil.GetODBCConnection();
                commTemp_RFIDDispositivo = new OdbcCommand("SELECT Matricola from Dispositivi", connTemp);
                readerTemp = commTemp_RFIDDispositivo.ExecuteReader();
                while (readerTemp.Read())
                {
                    string matricola = readerTemp.GetValue(readerTemp.GetOrdinal("Matricola")).ToString();
                    if (matricola == "")
                    {
                        MessageBox.Show(KleanTrak.Globals.strTable[27],
                            "Clean Track",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation);
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.WarnAndLog(ex);
            }
            finally
            {
                if (readerTemp != null && readerTemp.IsClosed == false)
                    readerTemp.Close();
                if (connTemp != null)
                    connTemp.Close();
            }
        }
        private void Dispositivi_ClientSizeChanged(object sender, EventArgs e) => Globals.ResizeList(this, listView);
        private void tsb_close_Click(object sender, EventArgs e) => Close();
        private void tsb_print_Click(object sender, EventArgs e) => listView.PrintList(this.Text, "Cleantrack");
        private void tsb_registry_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                DismissionList dl = new DismissionList(listView.Items[listView.SelectedIndices[0]].Tag.ToString(),
                    listView.Items[listView.SelectedIndices[0]].SubItems[0].Text);
                dl.ShowDialog();
            }
        }
        private void tsb_find_Click(object sender, EventArgs e)
        {
            Dictionary<int, string> stateList = GetStateList();
            Ricerca dlg = new Ricerca();
            dlg.ShowDialog();
            if (Globals.Query_ricerca != "")
            {
                OdbcConnection connTemp = DBUtil.GetODBCConnection();
                OdbcCommand command = new OdbcCommand("", connTemp);
                OdbcDataReader reader = null;
                command.CommandText = KleanTrak.Globals.Query_ricerca.ToString();
                try
                {
                    reader = command.ExecuteReader();
                    if (reader.HasRows == false)
                    {
                        MessageBox.Show(KleanTrak.Globals.strTable[21], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        reader.Close();
                        return;
                    }
                    listView.Items.Clear();
                    show_active_devices = false;
                    int num_risultati = 0;
                    while (reader.Read())
                    {
                        num_risultati = num_risultati + 1;
                        AddListViewItem(reader, stateList);
                    }
                }
                catch (OdbcException Ex)
                {
                    MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (reader != null && reader.IsClosed == false)
                    reader.Close();
                connTemp.Close();
            }
        }
        private void tsb_undo_dismiss_Click(object sender, EventArgs e)
        {
            OdbcCommand cmd = null;
            try
            {
                if (listView.SelectedIndices.Count == 0)
                {
                    MessageBox.Show(Globals.strTable[20],
                        "Clean Track",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                if (MessageBox.Show(KleanTrak.Globals.strTable[19] + listView.Items[listView.SelectedIndices[0]].SubItems[0].Text + "?",
                    "Clean Track",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                    return;
                string query = $"UPDATE DISPOSITIVI SET DATAFINE=NULL, " +
                    $"DISMESSO=0 " +
                    $"WHERE ID={listView.Items[listView.SelectedIndices[0]].Tag}";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                cmd.ExecuteNonQuery();
                StoricizzaDismissione(false, "RIPRISTINO", (int)listView.Items[listView.SelectedIndices[0]].Tag);
                listView.Enabled = true;
                //se ho fatto undo dismiss significa che siamo in visione dispositivi dismessi
                //pertanto non uso riempilista ma simulo il click per visione disp dismessi
                tsb_blocked_devices_Click(sender, e);
                //RiempiLista();
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
        private void StoricizzaDismissione(bool dismissed, string causale, int deviceid)
        {
            OdbcCommand cmd = null;
            try
            {
                string query = $"INSERT INTO STORICODISMISSIONE (UTENTE, DISMISSIONE, DATA, CAUSALE,IDDISPOSITIVO,ELIMINATO) " +
                    $"VALUES " +
                    $"('{Globals.m_strUser}', {((dismissed) ? "1" : "0")},'{Globals.ConvertDateTime(DateTime.Now)}'," +
                    $"'{causale}',{deviceid}, 0)";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Globals.Log(e);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        private void tsb_dismiss_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count == 0)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[18],
                    "Clean Track",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show(Globals.strTable[144],
                "Clean Track",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
                return;
            OdbcCommand cmd = null;
            try
            {
                var frm_causale = new ChooseItem();
                frm_causale.Combo_label = "SCEGLIERE CAUSALE";
                frm_causale.SetItems(_causali);
                frm_causale.ShowDialog();
                if (!frm_causale.Data_valid)
                    return;
                string query = $"UPDATE DISPOSITIVI SET TAG = NULL, " +
                    $"Dismesso = 1, " +
                    $"Datafine = '{Globals.ConvertDateTime(DateTime.Now)}', " +
                    $"IDCAUSALEDISMISSIONE = '{((ComboItem)frm_causale.Selected_item).Id}' " +
                    $"WHERE ID= '{listView.Items[listView.SelectedIndices[0]].Tag}'";
                cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
                cmd.ExecuteNonQuery();
                StoricizzaDismissione(true, ((ComboItem)frm_causale.Selected_item).Description, (int)listView.Items[listView.SelectedIndices[0]].Tag);
                listView.Items.RemoveAt(listView.SelectedIndices[0]);
                listView.Enabled = true;
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
        private void tsb_blocked_devices_Click(object sender, EventArgs e)
        {
            listView.Columns[listView.Columns.Count - 1].Text = KleanTrak.Globals.strTable[120];
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand command = new OdbcCommand("", connTemp);
            OdbcDataReader reader = null;
            string strQuery = "SELECT Dispositivi.ID, Dispositivi.DESCRIZIONE, Dispositivi.Matricola, dispositivi.seriale, TipiDispositivi.Descrizione AS TipoDispositivo, " +
                "Fornitori.Descrizione AS Fornitore, STATO, ESAMIESEGUITI, DATAINIZIO, DATAFINE, DISMESSO, Causali.causale as causale   " +
                "FROM (DISPOSITIVI LEFT OUTER JOIN TipiDispositivi ON Dispositivi.IDTipo=TipiDispositivi.ID LEFT OUTER JOIN Causali ON Dispositivi.idcausaledismissione=Causali.ID) " +
                "LEFT OUTER JOIN Fornitori ON Dispositivi.IDFornitore=Fornitori.ID " +
                "where DISMESSO = 1 AND ELIMINATO = 0 ORDER BY Dispositivi.ID";
            command.CommandText = strQuery;
            try
            {
                reader = command.ExecuteReader();
                //ATTENZIONE SE SONO GIÀ IN VISIONE DISPOSITIVI DISMESSI DEVO COMUNQUE MOSTRARE LISTA VUOTA SE 
                //HO LEVATO L'ULTIMO DISPOSITIVO.
                //if (reader.HasRows == false)
                //{
                //	MessageBox.Show(KleanTrak.Globals.strTable[26], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //	if (reader != null && !reader.IsClosed)
                //		reader.Close();
                //	return;
                //}
                show_active_devices = false;
                listView.BackColor = System.Drawing.SystemColors.ControlLight;
                ManageToolstripButtons();
                listView.Items.Clear();
                while (reader.Read())
                {
                    try
                    {
                        ListViewItem item = listView.Items.Add(reader.GetString(reader.GetOrdinal("Matricola")).ToString().ToUpper());

                        if (reader.IsDBNull(reader.GetOrdinal("DESCRIZIONE")))
                            item.SubItems.Add("");
                        else
                            item.SubItems.Add(reader.GetString(reader.GetOrdinal("DESCRIZIONE")));

                        item.Tag = reader.GetIntEx("ID");

                        if (reader.IsDBNull(reader.GetOrdinal("seriale")))
                            item.SubItems.Add("");
                        else
                            item.SubItems.Add(reader.GetString(reader.GetOrdinal("seriale")));

                        if (reader.IsDBNull(reader.GetOrdinal("TipoDispositivo")))
                            item.SubItems.Add("");
                        else
                            item.SubItems.Add(reader.GetString(reader.GetOrdinal("TipoDispositivo")));

                        if (reader.IsDBNull(reader.GetOrdinal("Fornitore")))
                            item.SubItems.Add("");
                        else
                            item.SubItems.Add(reader.GetString(reader.GetOrdinal("Fornitore")));

                        switch (reader.GetIntEx("STATO"))
                        {
                            case 1:
                                {
                                    item.SubItems.Add("Pulito");
                                    break;
                                }
                            case 2:
                                {
                                    item.SubItems.Add("Sporco");
                                    break;
                                }
                            case 3:
                                {
                                    item.SubItems.Add("In lavaggio");
                                    break;
                                }

                            case 4:
                                {
                                    item.SubItems.Add("PreLavaggio");
                                    break;
                                }

                            default:
                                {
                                    item.SubItems.Add("");
                                    break;
                                }
                        }
                        item.SubItems.Add(reader.GetValue(reader.GetOrdinal("ESAMIESEGUITI")).ToString());
                        string strDataInizio = "";
                        if (!reader.IsDBNull(reader.GetOrdinal("DataInizio")))
                            strDataInizio = KleanTrak.Globals.ConvertDateTime(reader.GetString(reader.GetOrdinal("DataInizio")));
                        item.SubItems.Add(strDataInizio);
                        item.SubItems.Add(KleanTrak.Globals.ConvertDateTime(reader.GetString(reader.GetOrdinal("DataFine"))));
                        item.SubItems.Add(reader.GetString(reader.GetOrdinal("Causale")));
                    }
                    catch (Exception /*exc*/)
                    {
                    }
                }
            }
            catch (OdbcException Ex)
            {
                MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (reader != null && !reader.IsClosed)
                reader.Close();
            connTemp.Close();
            this.Text = KleanTrak.Globals.strTable[121];
            show_active_devices = false;//btnDispositiviShowInUse = true;
            if (listView.Columns.Count <= 8)
            {
                listView.Columns.Add("Data dismissione", 150, HorizontalAlignment.Center);
                listView.Columns.Add("Causale dismissione", 150, HorizontalAlignment.Center);
            }
        }
        private void tsb_active_devices_Click(object sender, EventArgs e)
        {
            listView.BackColor = System.Drawing.SystemColors.Window;
            show_active_devices = true;
            ManageToolstripButtons();
            this.Text = Globals.strTable[117];
            if (listView.Columns.Count == 10)
            {
                listView.Columns.RemoveAt(9);
                listView.Columns.RemoveAt(8);
            }
            listView.Columns[listView.Columns.Count - 1].Text = "Tag";
            RiempiLista();
        }
        private void tsb_add_Click(object sender, EventArgs e)
        {
            string strCommand = "";
            OdbcCommand cmd = null;
            try
            {
                strCommand = $"INSERT INTO DISPOSITIVI " +
                    $"(MATRICOLA, STATO, DESCRIZIONE, SERIALE, DATAINIZIO, TAG, IDSEDE, DATASTATO) " +
                    $"VALUES ('Nuovo Codice', '1', 'NUOVO', 'NUOVO', " +
                    $"'{KleanTrak.Globals.ConvertDateTime(DateTime.Now).ToUpper()}',NULL, " +
                    $"{KleanTrak.Globals.IDSEDE}, '{KleanTrak.Globals.ConvertDateTime(DateTime.Now).ToUpper()}')";
                cmd = new OdbcCommand(strCommand, DBUtil.GetODBCConnection());
                cmd.ExecuteNonQuery();
                cmd.CommandText = "SELECT MAX (ID) FROM DISPOSITIVI";
                int max_id = cmd.ExecuteScalarInt();
                if (max_id < 0)
                    throw new ApplicationException("insert into dispositivi failed");
                DBUtil.InsertDbLog("DISPOSITIVI", DBUtil.LogOperation.Insert, max_id);
                RiempiLista();
                listView.EnsureVisible(listView.Items.Count - 1);
                listView.EditCell(listView.Items.Count - 1, 0);
            }
            catch (OdbcException ex)
            {
                Globals.WarnAndLog(ex);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        private void tsb_delete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count == 0)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[128], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (MessageBox.Show(KleanTrak.Globals.strTable[28], "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            OdbcCommand cmd = null;
            OdbcDataReader rdr = null;
            OdbcConnection db_conn = null;
            string device_id = listView.Items[listView.SelectedIndices[0]].Tag.ToString().ToUpper();
            try
            {
                db_conn = DBUtil.GetODBCConnection();
                //verifico che il dispositivo non sia già stato utilizzato
                cmd = new OdbcCommand("SELECT Cicli.iddispositivo from Cicli " +
                    "INNER JOIN dispositivi ON Cicli.iddispositivo = dispositivi.ID " +
                    "where dispositivi.ID  = " + device_id,
                    db_conn);
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    MessageBox.Show(KleanTrak.Globals.strTable[29], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                rdr.Close();
                //cmd.CommandText = $"DELETE FROM Dispositivi WHERE ID={device_id}";
                var matricola = listView.Items[listView.SelectedIndices[0]].Text;
                if (matricola.StartsWith("DELETED_"))
                    cmd.CommandText = $"UPDATE DISPOSITIVI SET ELIMINATO = 1 WHERE ID={device_id}";
                else
                    cmd.CommandText = $"UPDATE DISPOSITIVI SET ELIMINATO = 1, MATRICOLA = 'DELETED_' + MATRICOLA WHERE ID={device_id}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"UPDATE STORICODISMISSIONE SET ELIMINATO=1 WHERE IDDISPOSITIVO={device_id}";
                cmd.ExecuteNonQuery();
                cmd.CommandText = $"INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD ) VALUES ('{KleanTrak.Globals.m_strUser}', 'DISPOSITIVI', 'Cancellazione', '{KleanTrak.Globals.ConvertDateTime(DateTime.Now).ToUpper()}', '{device_id}')";
                cmd.ExecuteNonQuery();
                listView.Items.RemoveAt(listView.SelectedIndices[0]);
            }
            catch (Exception ex)
            {
                Logger.Error("Dispositivi.cs", ex);
                throw;
            }
            finally
            {
                if (db_conn != null)
                    db_conn.Close();
                if (rdr != null && !rdr.IsClosed)
                    rdr.Close();
                if (cmd != null)
                    cmd.Dispose();
            }
        }
        private void tsb_rfid_Click(object sender, EventArgs e)
        {
            //associazione TAG
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[95], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FormAssociaTag dlg = new FormAssociaTag();
            string rfid = "";
            dlg.FormClosing += (s, ea) => { rfid = dlg.m_RFID; };
            dlg.ShowDialog();
            if (dlg.DialogResult == DialogResult.Cancel)
                return;
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand connTemp2 = new OdbcCommand("SELECT id FROM dispositivi WHERE TAG ='" + rfid + "'", connTemp);
            OdbcDataReader readerTemp = connTemp2.ExecuteReader();
            bool bFound = readerTemp.Read();
            if (readerTemp != null && !readerTemp.IsClosed)
                readerTemp.Close();
            //controllo se per caso e già associato a un operatore,nel caso nn permetto l'associazione alla sonda
            OdbcCommand userSonda = new OdbcCommand("SELECT id FROM operatori WHERE TAG = '" + rfid + "'", connTemp);
            OdbcDataReader readerUs = userSonda.ExecuteReader();
            bFound = bFound || readerUs.Read();
            if (readerUs != null && !readerUs.IsClosed)
                readerUs.Close();
            if (!bFound)
            {
                OdbcCommand commIdRfid1 = new OdbcCommand("UPDATE dispositivi SET TAG = '" + rfid + "' WHERE ID = " + listView.SelectedItems[0].Tag.ToString(), connTemp);
                commIdRfid1.ExecuteNonQuery();
                RiempiLista();
            }
            else
                MessageBox.Show("Il tag è già utilizzato!", "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);

            connTemp.Close();
            dlg.Dispose();
        }

        private void tsbdevicecycles_Click(object sender, EventArgs e)
        {
            if (listView.SelectedIndices.Count > 0)
            {
                CicliDispositivo dlg = new CicliDispositivo((int)listView.Items[listView.SelectedIndices[0]].Tag);
                dlg.ShowDialog();
            }
        }
    }
}