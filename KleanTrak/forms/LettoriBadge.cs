using OdbcExtensions;
using System;
using System.Collections;
using System.Data.Odbc;
using System.Net;
using System.Windows.Forms;

namespace KleanTrak
{
	public class LettoriBadge : System.Windows.Forms.Form
	{
		private System.ComponentModel.Container components = null;
		private ListViewEx.ListViewEx listView;
		private System.Windows.Forms.ColumnHeader ColumnDescrizione;
		private System.Windows.Forms.ColumnHeader columnIP;
        private ColumnHeader columnStato;
        private ColumnHeader columnTipo;
        private ColumnHeader columnEtichettaOperazione;
        private ColumnHeader columnEtichettaDispositivo;
        private ColumnHeader columnEtichettaOperatore;
        private ColumnHeader columnTimeout;
        private ColumnHeader columnSelezioneWorklist;
        private ColumnHeader columnHeaderAttesaPrimaApplica;
        private ColumnHeader columnHeaderAttesaPrimaChiusuraPopup;
        private ToolStripContainer tscontainer;
        private ToolStrip main_toolstrip;
		private ToolStripButton tsb_add;
		private ToolStripButton tsb_delete;
		private ToolStripButton tsb_send_configuration;
		private ToolStripButton tsb_close;
		private System.Windows.Forms.ColumnHeader columnPorta;
		private enum STATO
		{
			VISUALIZZA,
			NUOVO,
			MODIFICA
		}
		public LettoriBadge()
		{
			InitializeComponent();
            listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.textbox, null); //descrizione
            listView.SetSubItemType(1, ListViewEx.ListViewEx.FieldType.textbox, null); //ip
            // listView.SetSubItemType(2, ListViewEx.ListViewEx.FieldType.textbox, null); //porta
            listView.SetSubItemType(3, ListViewEx.ListViewEx.FieldType.combo, ComboStato()); //stato
            listView.SetSubItemType(4, ListViewEx.ListViewEx.FieldType.combo, ComboTipo()); //tipo
            listView.SetSubItemType(5, ListViewEx.ListViewEx.FieldType.textbox, null); //etichetta operazione
            listView.SetSubItemType(6, ListViewEx.ListViewEx.FieldType.textbox, null); //etichetta dispositivo
            listView.SetSubItemType(7, ListViewEx.ListViewEx.FieldType.textbox, null); //etichetta operatore
            listView.SetSubItemType(8, ListViewEx.ListViewEx.FieldType.textbox, null); //timeout completamento
            listView.SetSubItemType(9, ListViewEx.ListViewEx.FieldType.combo, ComboCheck()); //worklist
            listView.SetSubItemType(10, ListViewEx.ListViewEx.FieldType.textbox, null); //attesa prima applica
            listView.SetEndEditCallback(new ListViewEx.ListViewEx.EndEditingCallbackEdit(EndEditTextbox), 
				new ListViewEx.ListViewEx.EndEditingCallbackCombo(EndEditCombobox), 
				null, 
				null);
			tsb_add.ToolTipText = Globals.strTable[199];
			tsb_delete.ToolTipText = Globals.strTable[200];
			tsb_send_configuration.ToolTipText = Globals.strTable[201];
			tsb_close.ToolTipText = Globals.strTable[202];
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LettoriBadge));
			this.listView = new ListViewEx.ListViewEx();
			this.ColumnDescrizione = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnPorta = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnStato = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnTipo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnEtichettaOperazione = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnEtichettaDispositivo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnEtichettaOperatore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnTimeout = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnSelezioneWorklist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderAttesaPrimaApplica = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeaderAttesaPrimaChiusuraPopup = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tscontainer = new System.Windows.Forms.ToolStripContainer();
			this.main_toolstrip = new System.Windows.Forms.ToolStrip();
			this.tsb_add = new System.Windows.Forms.ToolStripButton();
			this.tsb_delete = new System.Windows.Forms.ToolStripButton();
			this.tsb_send_configuration = new System.Windows.Forms.ToolStripButton();
			this.tsb_close = new System.Windows.Forms.ToolStripButton();
			this.tscontainer.ContentPanel.SuspendLayout();
			this.tscontainer.TopToolStripPanel.SuspendLayout();
			this.tscontainer.SuspendLayout();
			this.main_toolstrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnDescrizione,
            this.columnIP,
            this.columnPorta,
            this.columnStato,
            this.columnTipo,
            this.columnEtichettaOperazione,
            this.columnEtichettaDispositivo,
            this.columnEtichettaOperatore,
            this.columnTimeout,
            this.columnSelezioneWorklist,
            this.columnHeaderAttesaPrimaApplica,
            this.columnHeaderAttesaPrimaChiusuraPopup});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(986, 486);
			this.listView.TabIndex = 54;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
			this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
			// 
			// ColumnDescrizione
			// 
			this.ColumnDescrizione.Text = "Descrizione";
			this.ColumnDescrizione.Width = 150;
			// 
			// columnIP
			// 
			this.columnIP.Text = "Indirizzo IP";
			this.columnIP.Width = 98;
			// 
			// columnPorta
			// 
			this.columnPorta.Text = "Porta";
			// 
			// columnStato
			// 
			this.columnStato.Text = "Stato default";
			this.columnStato.Width = 109;
			// 
			// columnTipo
			// 
			this.columnTipo.Text = "Tipo lettore";
			this.columnTipo.Width = 101;
			// 
			// columnEtichettaOperazione
			// 
			this.columnEtichettaOperazione.Text = "Etichetta operazione";
			this.columnEtichettaOperazione.Width = 157;
			// 
			// columnEtichettaDispositivo
			// 
			this.columnEtichettaDispositivo.Text = "Etichetta dispositivo";
			this.columnEtichettaDispositivo.Width = 155;
			// 
			// columnEtichettaOperatore
			// 
			this.columnEtichettaOperatore.Text = "Etichetta operatore";
			this.columnEtichettaOperatore.Width = 149;
			// 
			// columnTimeout
			// 
			this.columnTimeout.Text = "Timeout compl. operaz.";
			this.columnTimeout.Width = 175;
			// 
			// columnSelezioneWorklist
			// 
			this.columnSelezioneWorklist.Text = "Selezione worklist";
			this.columnSelezioneWorklist.Width = 145;
			// 
			// columnHeaderAttesaPrimaApplica
			// 
			this.columnHeaderAttesaPrimaApplica.Text = "Attesa prima applica";
			this.columnHeaderAttesaPrimaApplica.Width = 155;
			// 
			// columnHeaderAttesaPrimaChiusuraPopup
			// 
			this.columnHeaderAttesaPrimaChiusuraPopup.Text = "Attesa prima ch. popup";
			this.columnHeaderAttesaPrimaChiusuraPopup.Width = 155;
			// 
			// tscontainer
			// 
			// 
			// tscontainer.ContentPanel
			// 
			this.tscontainer.ContentPanel.Controls.Add(this.listView);
			this.tscontainer.ContentPanel.Size = new System.Drawing.Size(986, 466);
			this.tscontainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tscontainer.Location = new System.Drawing.Point(0, 0);
			this.tscontainer.Name = "tscontainer";
			this.tscontainer.Size = new System.Drawing.Size(986, 521);
			this.tscontainer.TabIndex = 56;
			this.tscontainer.Text = "toolStripContainer1";
			// 
			// tscontainer.TopToolStripPanel
			// 
			this.tscontainer.TopToolStripPanel.Controls.Add(this.main_toolstrip);
			// 
			// main_toolstrip
			// 
			this.main_toolstrip.Dock = System.Windows.Forms.DockStyle.None;
			this.main_toolstrip.ImageScalingSize = new System.Drawing.Size(48, 48);
			this.main_toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_add,
            this.tsb_delete,
            this.tsb_send_configuration,
            this.tsb_close});
			this.main_toolstrip.Location = new System.Drawing.Point(3, 0);
			this.main_toolstrip.Name = "main_toolstrip";
			this.main_toolstrip.Size = new System.Drawing.Size(251, 55);
			this.main_toolstrip.TabIndex = 0;
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
			// tsb_send_configuration
			// 
			this.tsb_send_configuration.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_send_configuration.Image = global::kleanTrak.Properties.Resources.configuration;
			this.tsb_send_configuration.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_send_configuration.Name = "tsb_send_configuration";
			this.tsb_send_configuration.Size = new System.Drawing.Size(52, 52);
			this.tsb_send_configuration.Text = "toolStripButton1";
			this.tsb_send_configuration.Click += new System.EventHandler(this.tsb_send_configuration_Click);
			// 
			// tsb_close
			// 
			this.tsb_close.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_close.Image = global::kleanTrak.Properties.Resources.close;
			this.tsb_close.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_close.Name = "tsb_close";
			this.tsb_close.Size = new System.Drawing.Size(52, 52);
			this.tsb_close.Text = "toolStripButton1";
			this.tsb_close.Click += new System.EventHandler(this.tsb_chiudi_Click);
			// 
			// LettoriBadge
			// 
			this.ClientSize = new System.Drawing.Size(986, 521);
			this.ControlBox = false;
			this.Controls.Add(this.tscontainer);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "LettoriBadge";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CLEAN TRACK - Lettori badge in uso";
			this.Load += new System.EventHandler(this.Lettori_Badge_Load);
			this.ClientSizeChanged += new System.EventHandler(this.LettoriBadge_ClientSizeChanged);
			this.tscontainer.ContentPanel.ResumeLayout(false);
			this.tscontainer.TopToolStripPanel.ResumeLayout(false);
			this.tscontainer.TopToolStripPanel.PerformLayout();
			this.tscontainer.ResumeLayout(false);
			this.tscontainer.PerformLayout();
			this.main_toolstrip.ResumeLayout(false);
			this.main_toolstrip.PerformLayout();
			this.ResumeLayout(false);

		}
        public class Item_Stato
        {
            public long id;
            public string strName;
            public override string ToString() { return strName; }
        }
        public class Item_Tipo
        {
            public long id { set; get; }
            public string strName { set; get; }
            public override string ToString() { return strName; }
        }
        public class Item_Check
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
            OdbcCommand commTemp = new OdbcCommand("SELECT ID, DESCRIZIONE FROM STATO WHERE ELIMINATO =	0", connTemp);
            try
            {
                reader = commTemp.ExecuteReader();
                while (reader.Read())
                {
                    Item_Stato Item = new Item_Stato();
                    Item.id = reader.GetIntEx("ID");
                    Item.strName = reader.GetValue(reader.GetOrdinal("DESCRIZIONE")).ToString();
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
        private ArrayList ComboTipo()
        {
            Array values = Enum.GetValues(typeof(KleanTrak.Model.DeviceReadersTypes));
            string[] names = Enum.GetNames(typeof(KleanTrak.Model.DeviceReadersTypes));

            ArrayList list = new ArrayList();
            for (int i = 0; i < names.Length; i++)
            {
                list.Add(new Item_Tipo() { id = (long)(KleanTrak.Model.DeviceReadersTypes)values.GetValue(i), strName = names[i] });
            }

            return list;
        }
        private ArrayList ComboCheck()
        {
            ArrayList list = new ArrayList();

            Item_Check Item = new Item_Check();
            Item.id = 0;
            Item.strName = "No";
            list.Add(Item);

            Item = new Item_Check();
            Item.id = 1;
            Item.strName = "Si";
            list.Add(Item);

            return list;
        }
        private bool EndEditTextbox(int iItemNum, int iSubitemNum, string strText)
		{
			bool bReturn = false;

			OdbcConnection connTemp = DBUtil.GetODBCConnection();
			OdbcCommand commTemp = new OdbcCommand("", connTemp);

			switch (iSubitemNum)
			{
				case 0:
				{
					commTemp = new OdbcCommand("UPDATE LETTORI SET Descrizione = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
					//INSERIMENTO NEL LOG DI SISTEMA
					//valore originale
					string ValoreOriginale = "";
					OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Descrizione FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
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

					if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
					{ break; }

					OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
									" VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI RFID', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'Descrizione', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
					commTemp_LOG.ExecuteNonQuery();

					break;
				}
				case 1:
				{
					commTemp = new OdbcCommand("UPDATE LETTORI SET IP = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

					//INSERIMENTO NEL LOG DI SISTEMA
					//valore originale
					string ValoreOriginale = "";
					OdbcCommand commValoreOriginale = new OdbcCommand("SELECT IP as IP FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
					OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
					if (readerValoreOriginale.Read())
					{
						if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("IP")))
						{
							ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("IP")).ToString();

							if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
								readerValoreOriginale.Close();
						}
						if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
							readerValoreOriginale.Close();
					}

					if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
					{ break; }

					OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
								" VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI BADGE', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'IP', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
					commTemp_LOG.ExecuteNonQuery();

					break;
				}
				case 2:
				{
					int retNum;
					if (!int.TryParse(Convert.ToString(strText.ToString()), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum))
					{
						MessageBox.Show("Inserire un valore numerico. Min 1  - Max 65535");
						return false;
					}

					if ((retNum < 0) || (retNum > 65535))
					{
						MessageBox.Show("Porta non impostata correttamente. Min 1  - Max 65535");
						return false;
					}

					commTemp = new OdbcCommand("UPDATE LETTORI SET Porta = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

					//INSERIMENTO NEL LOG DI SISTEMA
					//valore originale
					string ValoreOriginale = "";
					OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Porta as Porta FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
					OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
					if (readerValoreOriginale.Read())
					{
						if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Porta")))
						{
							ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Porta")).ToString();

							if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
								readerValoreOriginale.Close();
						}
						if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
							readerValoreOriginale.Close();
					}

					if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
						break;

					OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
								" VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI BADGE', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'PORTA DI COMUNIC.', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
					commTemp_LOG.ExecuteNonQuery();
					break;
				}
                case 5:
                {
                    commTemp = new OdbcCommand("UPDATE LETTORI SET EtichettaOperazione = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    //INSERIMENTO NEL LOG DI SISTEMA
                    //valore originale
                    string ValoreOriginale = "";
                    OdbcCommand commValoreOriginale = new OdbcCommand("SELECT EtichettaOperazione FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                    if (readerValoreOriginale.Read())
                    {
                        if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("EtichettaOperazione")))
                        {
                            ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("EtichettaOperazione")).ToString();

                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();
                        }
                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();
                    }

                    if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                    { break; }

                    OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI RFID', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'EtichettaOperazione', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                    commTemp_LOG.ExecuteNonQuery();
                    break;
                }
                case 6:
                {
                    commTemp = new OdbcCommand("UPDATE LETTORI SET EtichettaDispositivo = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    //INSERIMENTO NEL LOG DI SISTEMA
                    //valore originale
                    string ValoreOriginale = "";
                    OdbcCommand commValoreOriginale = new OdbcCommand("SELECT EtichettaDispositivo FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                    if (readerValoreOriginale.Read())
                    {
                        if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("EtichettaDispositivo")))
                        {
                            ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("EtichettaDispositivo")).ToString();

                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();
                        }
                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();
                    }

                    if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                    { break; }

                    OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI RFID', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'EtichettaDispositivo', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                    commTemp_LOG.ExecuteNonQuery();
                    break;
                }
                case 7:
                {
                    commTemp = new OdbcCommand("UPDATE LETTORI SET EtichettaOperatore = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    //INSERIMENTO NEL LOG DI SISTEMA
                    //valore originale
                    string ValoreOriginale = "";
                    OdbcCommand commValoreOriginale = new OdbcCommand("SELECT EtichettaOperatore FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                    if (readerValoreOriginale.Read())
                    {
                        if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("EtichettaOperatore")))
                        {
                            ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("EtichettaOperatore")).ToString();

                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();
                        }
                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();
                    }

                    if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                    { break; }

                    OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                    " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI RFID', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'EtichettaOperatore', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                    commTemp_LOG.ExecuteNonQuery();
                    break;
                }
                case 8:
                {
                    bool isNum;
                    int retNum;
                    isNum = int.TryParse(Convert.ToString(strText.ToString()), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
                    if (isNum == false)
                    {
                        MessageBox.Show("Inserire un valore numerico. Min 1  - Max 999");
                        return false;
                    }

                    if (isNum == true && strText.Length < 4 && strText.Length > 0)
                    {
                        if (Convert.ToInt32(strText.ToString()) < 0 && Convert.ToInt32(strText.ToString()) > 999)
                        {

                            MessageBox.Show("Timeout non impostato correttamente. Min 1  - Max 999");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Timeout non impostato correttamente. Min 1  - Max 999");
                        return false;
                    }

                    commTemp = new OdbcCommand("UPDATE LETTORI SET TimeoutCompletamentoStep = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                    //INSERIMENTO NEL LOG DI SISTEMA
                    //valore originale
                    string ValoreOriginale = "";
                    OdbcCommand commValoreOriginale = new OdbcCommand("SELECT TimeoutCompletamentoStep FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                    if (readerValoreOriginale.Read())
                    {
                        if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("TimeoutCompletamentoStep")))
                        {
                            ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("TimeoutCompletamentoStep")).ToString();

                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();
                        }
                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();
                    }

                    if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                        break;

                    OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI BADGE', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'TimeoutCompletamentoStep', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                    commTemp_LOG.ExecuteNonQuery();
                    break;
                }
                case 10:
                {
                    bool isNum;
                    int retNum;
                    isNum = int.TryParse(Convert.ToString(strText.ToString()), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
                    if (isNum == false)
                    {
                        MessageBox.Show("Inserire un valore numerico. Min 1  - Max 999");
                        return false;
                    }

                    if (isNum == true && strText.Length < 4 && strText.Length > 0)
                    {
                        if (Convert.ToInt32(strText.ToString()) < 0 && Convert.ToInt32(strText.ToString()) > 999)
                        {

                            MessageBox.Show("Attesa prima applica non impostato correttamente. Min 1  - Max 999");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Attesa prima applica non impostato correttamente. Min 1  - Max 999");
                        return false;
                    }

                    commTemp = new OdbcCommand("UPDATE LETTORI SET AttesaPrimaApplica = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                    //INSERIMENTO NEL LOG DI SISTEMA
                    //valore originale
                    string ValoreOriginale = "";
                    OdbcCommand commValoreOriginale = new OdbcCommand("SELECT AttesaPrimaApplica FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                    if (readerValoreOriginale.Read())
                    {
                        if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("AttesaPrimaApplica")))
                        {
                            ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("AttesaPrimaApplica")).ToString();

                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();
                        }
                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();
                    }

                    if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                        break;

                    OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI BADGE', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'AttesaPrimaApplica', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                    commTemp_LOG.ExecuteNonQuery();
                    break;
                }
                case 11:
                {
                    bool isNum;
                    int retNum;
                    isNum = int.TryParse(Convert.ToString(strText.ToString()), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
                    if (isNum == false)
                    {
                        MessageBox.Show("Inserire un valore numerico. Min 1  - Max 999");
                        return false;
                    }

                    if (isNum == true && strText.Length < 4 && strText.Length > 0)
                    {
                        if (Convert.ToInt32(strText.ToString()) < 0 && Convert.ToInt32(strText.ToString()) > 999)
                        {

                            MessageBox.Show("Attesa prima applica non impostato correttamente. Min 1  - Max 999");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Attesa prima applica non impostato correttamente. Min 1  - Max 999");
                        return false;
                    }

                    commTemp = new OdbcCommand("UPDATE LETTORI SET AttesaPrimaChiusuraPopup = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);

                    //INSERIMENTO NEL LOG DI SISTEMA
                    //valore originale
                    string ValoreOriginale = "";
                    OdbcCommand commValoreOriginale = new OdbcCommand("SELECT AttesaPrimaChiusuraPopup FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                    OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                    if (readerValoreOriginale.Read())
                    {
                        if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("AttesaPrimaChiusuraPopup")))
                        {
                            ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("AttesaPrimaChiusuraPopup")).ToString();

                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();
                        }
                        if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                            readerValoreOriginale.Close();
                    }

                    if (ValoreOriginale == strText.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                        break;

                    OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI BADGE', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'AttesaPrimaChiusuraPopup', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                    commTemp_LOG.ExecuteNonQuery();
                    break;
                }
            }

            try
			{
                if (commTemp.CommandText != "")
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
        private bool EndEditCombobox(int iItemNum, int iSubitemNum, object obj)
        {
            bool bReturn = false;

            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("", connTemp);

            switch (iSubitemNum)
            {
                case 3:
                    {
                        commTemp = new OdbcCommand("UPDATE LETTORI SET IDStatoDefault = '" + ((Item_Stato)obj).id + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT IDStatoDefault FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("IDStatoDefault")))
                            {
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("IDStatoDefault")).ToString();

                                if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                    readerValoreOriginale.Close();
                            }
                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();
                        }

                        if (ValoreOriginale == ((Item_Stato)obj).id.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                     " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI RFID', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'IDStatoDefault', '" + ValoreOriginale.Replace("'", "''") + "', '" + ((Item_Stato)obj).id + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();
                        break;
                    }

                case 4:
                    {
                        commTemp = new OdbcCommand("UPDATE LETTORI SET Tipo = '" + ((Item_Tipo)obj).id + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Tipo FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("Tipo")))
                            {
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("Tipo")).ToString();

                                if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                    readerValoreOriginale.Close();
                            }
                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();
                        }

                        if (ValoreOriginale == ((Item_Tipo)obj).id.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                     " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI RFID', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'Tipo', '" + ValoreOriginale.Replace("'", "''") + "', '" + ((Item_Tipo)obj).id + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();

						// Sandro 15/05/2017 //

						switch (((Item_Tipo)obj).id)
						{
							case 0:
							{
								listView.Items[iItemNum].SubItems[2].Text = "8090";
								EndEditTextbox(iItemNum, 2 /* porta */, "8090");
								break;
							}
							case 1:
							{
								listView.Items[iItemNum].SubItems[2].Text = "10001";
								EndEditTextbox(iItemNum, 2 /* porta */, "10001");
								break;
							}
						}

                        break;
                    }
                case 9:
                    {
                        commTemp = new OdbcCommand("UPDATE LETTORI SET SelezioneWorklist = '" + ((Item_Check)obj).id + "' WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT SelezioneWorklist FROM LETTORI WHERE ID=" + listView.Items[iItemNum].Tag.ToString(), connTemp);
                        OdbcDataReader readerValoreOriginale = commValoreOriginale.ExecuteReader();
                        if (readerValoreOriginale.Read())
                        {
                            if (!readerValoreOriginale.IsDBNull(readerValoreOriginale.GetOrdinal("SelezioneWorklist")))
                            {
                                ValoreOriginale = readerValoreOriginale.GetValue(readerValoreOriginale.GetOrdinal("SelezioneWorklist")).ToString();

                                if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                    readerValoreOriginale.Close();
                            }
                            if ((readerValoreOriginale != null) && (readerValoreOriginale.IsClosed == false))
                                readerValoreOriginale.Close();
                        }

                        if (ValoreOriginale == ((Item_Check)obj).id.ToString()) //se non viene modificato il valore non effettuo la query nel LOG.
                            break;

                        OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                     " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'LETTORI RFID', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'SelezioneWorklist', '" + ValoreOriginale.Replace("'", "''") + "', '" + ((Item_Check)obj).id + "')", connTemp);
                        commTemp_LOG.ExecuteNonQuery();
                        break;
                    }
            }

            try
            {
                if (commTemp.CommandText != "")
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
		private void Lettori_Badge_Load(object sender, System.EventArgs e)
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
			listView.Visible = false;
			listView.Items.Clear();
			string query = $"SELECT LETTORI.*, STATO.DESCRIZIONE AS DESCRIZIONESTATO FROM LETTORI " +
				$"LEFT OUTER JOIN STATO ON LETTORI.IDSTATODEFAULT = STATO.ID " +
				$"WHERE LETTORI.ELIMINATO = 0 AND LETTORI.IDSEDE = {Globals.IDSEDE} " +
				$"ORDER BY LETTORI.ID"; // ORDER BY Lettori.Descrizione ";
			OdbcCommand cmd = null;
			OdbcDataReader rdr = null;
			try
			{
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				rdr = cmd.ExecuteReader();
				ListViewItem lvItem;
				while (rdr.Read())
				{
					lvItem = listView.Items.Add(rdr.GetString(rdr.GetOrdinal("Descrizione")));
					lvItem.Tag = rdr.GetIntEx("ID");
					lvItem.SubItems.Add(rdr.GetStringEx("IP"));
					lvItem.SubItems.Add(rdr.GetIntEx("PORTA").ToString());
					lvItem.SubItems.Add(rdr.GetStringEx("DESCRIZIONESTATO"));
					if (!rdr.IsDBNull(rdr.GetOrdinal("Tipo")))
					{
						int tipo = rdr.GetIntEx("TIPO");
						if (Enum.IsDefined(typeof(KleanTrak.Model.DeviceReadersTypes), tipo))
							lvItem.SubItems.Add(((KleanTrak.Model.DeviceReadersTypes)tipo).ToString());
						else
							lvItem.SubItems.Add("");
					}
					else
						lvItem.SubItems.Add("");
					lvItem.SubItems.Add(rdr.GetStringEx("ETICHETTAOPERAZIONE"));
					lvItem.SubItems.Add(rdr.GetStringEx("ETICHETTADISPOSITIVO"));
					lvItem.SubItems.Add(rdr.GetStringEx("ETICHETTAOPERATORE"));
					lvItem.SubItems.Add(rdr.GetIntEx("TIMEOUTCOMPLETAMENTOSTEP").ToString());
					lvItem.SubItems.Add(rdr.GetBoolEx("SELEZIONEWORKLIST").ToString());
					lvItem.SubItems.Add(rdr.GetIntEx("ATTESAPRIMAAPPLICA").ToString());
					lvItem.SubItems.Add(rdr.GetIntEx("ATTESAPRIMACHIUSURAPOPUP").ToString());
				}
			}
			catch (Exception e)
			{
				Globals.WarnAndLog(e);
			}
			finally
			{
				if (rdr != null && !rdr.IsClosed)
					rdr.Close();
				if (cmd != null)
					cmd.Dispose();
				listView.Visible = true;
			}
		}
		public class comboitem
		{
			public long id;
			public string descrizione;
			public override string ToString() { return descrizione; }
		}
		private bool AddReader()
		{
			OdbcCommand cmd = null;
			try
			{
				string query = $"INSERT INTO LETTORI (DESCRIZIONE, IP, PORTA, IDSEDE) " +
					$"VALUES " +
					$"('NUOVO LETTORE BADGE', '192.168.0.1', '{Globals.iBadgeAddressPort}', {Globals.IDSEDE})";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				cmd.ExecuteNonQuery();
				DBUtil.InsertDbLog("LETTORI", DBUtil.LogOperation.Insert, cmd.GetMaxKeyValue("LETTORI", "ID"));
				return true;
			}
			catch (OdbcException Ex)
			{
				Globals.WarnAndLog(Ex);
				return false;
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}
		private bool DeleteReader(int reader_id)
		{
			OdbcCommand cmd = null;
			try
			{
				string query = $"SELECT COUNT(*) FROM CICLI " +
				$"LEFT OUTER JOIN LETTORI ON CICLI.IDSTERILIZZATRICE=LETTORI.ID " +
				$"WHERE LETTORI.ID = {reader_id}";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				if (cmd.ExecuteScalarInt() > 0)
				{
					MessageBox.Show(Globals.strTable[58], 
						"Clean Track", 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Exclamation);
					return false;
				}
				cmd.CommandText = $"UPDATE LETTORI SET ELIMINATO = 1 WHERE ID = {reader_id}";
				if (1 != cmd.ExecuteNonQuery())
					throw new ApplicationException("delete reader failed");
				DBUtil.InsertDbLog("LETTORI", DBUtil.LogOperation.Update, reader_id, "", "ELIMINATO", "0", "1");
				return true;
			}
			catch (Exception e)
			{
				Globals.Log(e, $"reader_id: {reader_id}");
				return false;
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}
		private void listView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			try
			{
				if (e.KeyCode != Keys.Delete || (listView.SelectedIndices.Count == 0))
					return;
				if (MessageBox.Show(Globals.strTable[203], "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return;
				int id = (int)listView.SelectedItems[0].Tag;
				if (!DeleteReader(id))
					return;
				RiempiLista();
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
			}
		}
		private void listView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            ((ListViewEx.ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
        }
        private void SendConfiguration()
        {
            if (Globals.ServerHttpEndpoint == null || Globals.ServerHttpEndpoint == "")
                return;
            
            string cmdToSend = (new Model.CmdSendPiConfiguration()).SaveObjectToXml();
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(cmdToSend);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Globals.ServerHttpEndpoint);
            req.Method = "POST";
            req.ContentType = "text/plain";
            req.ContentLength = cmdToSend.Length;
            req.Proxy = new WebProxy();

            HttpWebResponse resp = null;
            Model.Response respModel = null;

            try
            {
                var reqStream = req.GetRequestStream();
                reqStream.Write(buffer, 0, buffer.Length);
                reqStream.Close();

                resp = (HttpWebResponse)req.GetResponse();
                using (var respStream = new System.IO.StreamReader(resp.GetResponseStream()))
                {
                    respModel = (Model.Response)Model.Response.ReadObjectFromXml(respStream.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante l'invio della configurazione!\r\n" + ex.Message + "\r\n" + ex.StackTrace);
            }
            finally
            {
                if (resp != null)
                    resp.Close();
            }

            if (respModel != null)
            {
                if (!respModel.Successed)
                    MessageBox.Show("Errore durante l'invio della configurazione!\r\n" + respModel.ErrorMessage);
                else
                    MessageBox.Show("Configurazione aggiornata!");
            }
            else
                MessageBox.Show("Errore sconosciuto durante l'invio della configurazione!");
        }
		private void LettoriBadge_ClientSizeChanged(object sender, EventArgs e) => Globals.ResizeList(this, listView);
		private void tsb_add_Click(object sender, EventArgs e)
		{
			try
			{
				if (!AddReader())
					return;
				RiempiLista();
				listView.EnsureVisible(listView.Items.Count - 1);
				listView.EditCell(listView.Items.Count - 1, 0);
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
			}
		}
		private void tsb_delete_Click(object sender, EventArgs e)
		{
			try
			{
				if (MessageBox.Show(Globals.strTable[203], "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					return;
				int id = (int)listView.SelectedItems[0].Tag;
				if (!DeleteReader(id))
					return;
				RiempiLista();
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
			}
		}
		private void tsb_send_configuration_Click(object sender, EventArgs e) => SendConfiguration();
		private void tsb_chiudi_Click(object sender, EventArgs e) => Close();
	}
}