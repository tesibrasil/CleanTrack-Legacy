using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;

namespace KleanTrak
{
	public class Sterilizzatrici : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnElimina;
		private System.Windows.Forms.Button btnChiudi;
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
        private System.Windows.Forms.Button button1;
        private PictureBox listPlacer;

		public Sterilizzatrici()
		{
            InitializeComponent();
            InitializeListView();

            listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetSubItemType(1, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetSubItemType(2, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetSubItemType(3, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(4, ListViewEx.ListViewEx.FieldType.combo, GetComboTipoList());
            listView.SetSubItemType(5, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(6, ListViewEx.ListViewEx.FieldType.textbox, null);
            listView.SetSubItemType(7, ListViewEx.ListViewEx.FieldType.check, null);
            listView.SetSubItemType(8, ListViewEx.ListViewEx.FieldType.combo, GetComboDismissioneList());

            listView.SetEndEditCallback(new ListViewEx.ListViewEx.EndEditingCallbackEdit(EndEdit), new ListViewEx.ListViewEx.EndEditingCallbackCombo(EndCombo), null, new ListViewEx.ListViewEx.EndEditingCallbackCheck(EndCheck));
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

		private void InitializeListView()
		{
			this.listView = new ListViewEx.ListViewEx();
			this.columnMatricola = new System.Windows.Forms.ColumnHeader();
			this.columnSterilizzatrice = new System.Windows.Forms.ColumnHeader();
			this.columnSeriale = new System.Windows.Forms.ColumnHeader();
			this.columnTempoLavaggio = new System.Windows.Forms.ColumnHeader();
            this.columnTipo = new System.Windows.Forms.ColumnHeader();
            this.columnPercorso = new System.Windows.Forms.ColumnHeader();
            this.columnPolling = new System.Windows.Forms.ColumnHeader();
            this.columnDismesso = new System.Windows.Forms.ColumnHeader();
            this.columnCausale = new System.Windows.Forms.ColumnHeader();
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
                                                                                        this.columnCausale });
			this.listView.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.Location = new System.Drawing.Point(listPlacer.Location.X, listPlacer.Location.Y);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(listPlacer.Size.Width, listPlacer.Size.Height);
			this.listView.TabIndex = 18;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
			this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listview_ColumnClick);
			// 
			// columnMatricola
			// 
			this.columnMatricola.Text = "Codice";
			this.columnMatricola.Width = 150;
			// 
			// columnSterilizzatrice
			// 
			this.columnSterilizzatrice.Text = "Descrizione";
			this.columnSterilizzatrice.Width = 200;
			// 
			// columnSeriale
			// 
			this.columnSeriale.Text = "N° di Serie";
			this.columnSeriale.Width = 150;
			// 
			// columnTempoLavaggio
			// 
			this.columnTempoLavaggio.Text = "Tempo di Lavaggio (min)";
			this.columnTempoLavaggio.Width = 210;
            // 
            // columnTipo
            // 
            this.columnTipo.Text = "Tipo";
            this.columnTipo.Width = 150;
            // 
            // columnPercorso
            // 
            this.columnPercorso.Text = "Percorso";
            this.columnPercorso.Width = 250;
            // 
            // columnPolling
            // 
            this.columnPolling.Text = "Tempo di polling (sec)";
            this.columnPolling.Width = 210;
            // 
            // columnDismesso
            // 
            this.columnDismesso.Text = "Dismessa";
            this.columnDismesso.Width = 90;
            // 
            // columnCausale
            // 
            this.columnCausale.Text = "Causale";
            this.columnCausale.Width = 150;

            this.Controls.Add(this.listView);
		}

		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Sterilizzatrici));
            this.btnElimina = new System.Windows.Forms.Button();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listPlacer = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.listPlacer)).BeginInit();
            this.SuspendLayout();
            // 
            // btnElimina
            // 
            this.btnElimina.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnElimina.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnElimina.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnElimina.Location = new System.Drawing.Point(90, 547);
            this.btnElimina.Name = "btnElimina";
            this.btnElimina.Size = new System.Drawing.Size(66, 30);
            this.btnElimina.TabIndex = 38;
            this.btnElimina.Text = "Elimina";
            this.btnElimina.Click += new System.EventHandler(this.btnElimina_Click);
            // 
            // btnChiudi
            // 
            this.btnChiudi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChiudi.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnChiudi.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChiudi.Location = new System.Drawing.Point(1117, 547);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(84, 30);
            this.btnChiudi.TabIndex = 41;
            this.btnChiudi.Text = "Esci";
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(12, 547);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 30);
            this.button1.TabIndex = 52;
            this.button1.Text = "Nuovo";
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // listPlacer
            // 
            this.listPlacer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listPlacer.Location = new System.Drawing.Point(12, 12);
            this.listPlacer.Name = "listPlacer";
            this.listPlacer.Size = new System.Drawing.Size(1189, 523);
            this.listPlacer.TabIndex = 54;
            this.listPlacer.TabStop = false;
            this.listPlacer.Visible = false;
            // 
            // Sterilizzatrici
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 16);
            this.ClientSize = new System.Drawing.Size(1214, 590);
            this.Controls.Add(this.btnElimina);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listPlacer);
            this.Controls.Add(this.btnChiudi);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(738, 486);
            this.Name = "Sterilizzatrici";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CLEAN TRACK - Sterilizzatrici in uso";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Sterilizzatrici_Closing);
            this.Load += new System.EventHandler(this.Sterilizzatrici_Load);
            ((System.ComponentModel.ISupportInitialize)(this.listPlacer)).EndInit();
            this.ResumeLayout(false);

		}

        private void Sterilizzatrici_Load(object sender, System.EventArgs e)
        {
            Globals.LocalizzaDialog(this);

            listView.ListViewItemSorter = new ListViewEx.ListViewExComparer(listView);
            listView.SetLoadSaveParameter(Application.StartupPath + "\\kleantrak.ini", "Sterilizzatrici");
            listView.LoadColumnSize();

            RiempiLista();
            LoadFormSize();
        }

        private void RiempiLista()
        {
            listView.Items.Clear();

            string query = "";
            query += "SELECT ARMADI_LAVATRICI.ID, Matricola, Descrizione, Seriale, Dismesso, Causali.Causale AS CausaleDismissione, TempoLavaggio, Tipo, Percorso, PollingTime ";
            query += "FROM ARMADI_LAVATRICI ";
            query += "     LEFT OUTER JOIN Causali ON ARMADI_LAVATRICI.IDCausaleDismissione = Causali.ID ";
            query += "WHERE UO = " + KleanTrak.Globals.UO.ToString() + " ORDER BY ID";

            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand(query, connTemp);
            OdbcDataReader readerTemp = commTemp.ExecuteReader();

            while (readerTemp.Read())
            {
                ListViewItem lvItem = listView.Items.Add(readerTemp.GetString(readerTemp.GetOrdinal("Matricola")).ToUpper());

                lvItem.Tag = DBUtil.GetIntValue(readerTemp, "ID");

                lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("Descrizione")).ToUpper());

                if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("Seriale")))
                    lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("Seriale")).ToUpper());
                else
                    lvItem.SubItems.Add("");

                if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("TempoLavaggio")))
                    lvItem.SubItems.Add(Convert.ToString(DBUtil.GetIntValue(readerTemp, "TempoLavaggio")));
                else
                    lvItem.SubItems.Add("");

                if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("Tipo")))
                {
                    int tipo = DBUtil.GetIntValue(readerTemp, "Tipo");
                    if (Enum.IsDefined(typeof(KleanTrak.Models.WasherTypes), tipo))
                        lvItem.SubItems.Add(((KleanTrak.Models.WasherTypes)tipo).ToString());
                    else
                        lvItem.SubItems.Add("");
                }
                else
                    lvItem.SubItems.Add("");

                if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("Percorso")))
                    lvItem.SubItems.Add(DBUtil.GetStringValue(readerTemp, "Percorso"));
                else
                    lvItem.SubItems.Add("");

                if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("PollingTime")))
                    lvItem.SubItems.Add(Convert.ToString(DBUtil.GetIntValue(readerTemp, "PollingTime")));
                else
                    lvItem.SubItems.Add("");

                if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("Dismesso")))
                    lvItem.SubItems.Add(DBUtil.GetIntValue(readerTemp, "Dismesso") > 0 ? "Si" : "No");
                else
                    lvItem.SubItems.Add("");

                if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("CausaleDismissione")))
                    lvItem.SubItems.Add(DBUtil.GetStringValue(readerTemp, "CausaleDismissione"));
                else
                    lvItem.SubItems.Add("");
            }

            if ((readerTemp != null) && (readerTemp.IsClosed == false))
                readerTemp.Close();

            connTemp.Close();
        }

        private bool EndEdit(int iItemNum, int iSubitemNum, string strText)
		{
			OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = null;

			if (strText == "DEFINIRE MATRICOLA") //nel caso in cui non venga editato nulla, elimino la riga corrispondente
			{
				commTemp = new OdbcCommand("Delete from ARMADI_LAVATRICI WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);
				commTemp.ExecuteNonQuery();

				RiempiLista();
				return false;
			}

			switch (iSubitemNum)
			{
				case 0:
					{
                        //verifica che il nuovo RFID non corrisponda ad uno già stato inserito. 
                        bool bFound = false;
                        OdbcCommand cmd = new OdbcCommand("SELECT MATRICOLA from ARMADI_LAVATRICI where MATRICOLA  = '" + strText.Replace("'", "''") + "'", connTemp);
						OdbcDataReader readerTemp = cmd.ExecuteReader();
						if (readerTemp.Read())
						{
							MessageBox.Show(KleanTrak.Globals.strTable[49], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            bFound = true;
                        }

						if ((readerTemp != null) && (readerTemp.IsClosed == false))
							readerTemp.Close();

                        if (!bFound)
                        {
                            commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Matricola = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);

                            //INSERIMENTO NEL LOG DI SISTEMA
                            //valore originale
                            string ValoreOriginale = "";
                            OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Matricola FROM ARMADI_LAVATRICI WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);
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
                                            " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'CODICE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                                cmdLog.ExecuteNonQuery();
                            }
                        }
						break;
					}
				case 1:
					{
						commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Descrizione = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);

						//INSERIMENTO NEL LOG DI SISTEMA
						//valore originale
						string ValoreOriginale = "";
						OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Descrizione FROM ARMADI_LAVATRICI WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);
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
                                        " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'DESCRIZIONE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                            cmdLog.ExecuteNonQuery();
                        }
						break;
					}
				case 2:
					{
						commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Seriale = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);

						//INSERIMENTO NEL LOG DI SISTEMA
						//valore originale
						string ValoreOriginale = "";
						OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Seriale FROM ARMADI_LAVATRICI WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);
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
                                        " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'NUMERO DI SERIE', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                            cmdLog.ExecuteNonQuery();
                        }
						break;
					}
				case 3:
					{
                        int num = 0;
                        if (int.TryParse(strText, out num))
                        {
                            commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET TempoLavaggio = " + strText.Replace("'", "''").ToUpper() + " WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);

                            //INSERIMENTO NEL LOG DI SISTEMA
                            //valore originale
                            string ValoreOriginale = "";
                            OdbcCommand commValoreOriginale = new OdbcCommand("SELECT TempoLavaggio FROM ARMADI_LAVATRICI WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);
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
                                            " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'TEMPO LAVAGGIO', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                                cmdLog.ExecuteNonQuery();
                            }
                        }
                        else
                            MessageBox.Show(KleanTrak.Globals.strTable[100], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Information);
						break;
					}
                case 5:
                    {
                        commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Percorso = '" + strText.Replace("'", "''").ToUpper() + "' WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);

                        //INSERIMENTO NEL LOG DI SISTEMA
                        //valore originale
                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Percorso FROM ARMADI_LAVATRICI WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);
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
                                        " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'Percorso', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                            cmdLog.ExecuteNonQuery();
                        }
                        break;
                    }
                case 6:
                    {
                        int num = 0;
                        if (int.TryParse(strText, out num))
                        {
                            commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET PollingTime = " + strText.Replace("'", "''").ToUpper() + " WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);

                            //INSERIMENTO NEL LOG DI SISTEMA
                            //valore originale
                            string ValoreOriginale = "";
                            OdbcCommand commValoreOriginale = new OdbcCommand("SELECT PollingTime FROM ARMADI_LAVATRICI WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);
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
                                            " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'PollingTime', '" + ValoreOriginale.Replace("'", "''") + "', '" + strText.Replace("'", "''") + "')", connTemp);
                                cmdLog.ExecuteNonQuery();
                            }
                        }
                        else
                            MessageBox.Show(KleanTrak.Globals.strTable[100], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = null;

            string strToUpdate = "NULL";
            if (obj != null)
                strToUpdate = ((ItemCombo)obj).id.ToString();

            switch (iSubitemNum)
            {
                case 4:
                    {
                        commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Tipo = " + strToUpdate + " WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);

                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Tipo FROM ARMADI_LAVATRICI WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);
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
                            OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                        " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'Tipo', '" + ValoreOriginale.Replace("'", "''") + "', '" + strToUpdate.Replace("'", "''") + "')", connTemp);
                            cmdLog.ExecuteNonQuery();
                        }
                        break;
                    }

                case 8:
                    {
                        commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET IDCausaleDismissione = " + strToUpdate + " WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);

                        string ValoreOriginale = "";
                        OdbcCommand commValoreOriginale = new OdbcCommand("SELECT IDCausaleDismissione FROM ARMADI_LAVATRICI WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);
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
                            OdbcCommand cmdLog = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD, NOMECAMPO, VALOREORIGINALE, VALOREMODIFICATO )" +
                                        " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'IDCausaleDismissione', '" + ValoreOriginale.Replace("'", "''") + "', '" + strToUpdate.Replace("'", "''") + "')", connTemp);
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

            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("UPDATE ARMADI_LAVATRICI SET Dismesso = " + (bChecked ? 1 : 0).ToString() + " WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);

            string ValoreOriginale = "";
            OdbcCommand commValoreOriginale = new OdbcCommand("SELECT Dismesso FROM ARMADI_LAVATRICI WHERE ID= " + listView.Items[iItemNum].Tag.ToString(), connTemp);
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
                            " VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Modifica', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[iItemNum].Tag.ToString() + "', 'Dismesso', '" + ValoreOriginale.Replace("'", "''") + "', '" + (bChecked ? 1 : 0).ToString().Replace("'", "''") + "')", connTemp);
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

        private void btnElimina_Click(object sender, System.EventArgs e)
		{
			if (listView.SelectedItems.Count == 0)
			{
				MessageBox.Show(KleanTrak.Globals.strTable[50], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

            if (MessageBox.Show(KleanTrak.Globals.strTable[51], "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                DeleteItemSelected();
		}

		private void btnChiudi_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void LoadFormSize()
		{
			string X = ListViewEx.Win32.GetPrivateProfileString("Form_Sterilizzatrici", "X", "", Application.StartupPath + "\\kleantrak.ini");
			string Y = ListViewEx.Win32.GetPrivateProfileString("Form_Sterilizzatrici", "Y", "", Application.StartupPath + "\\kleantrak.ini");
			string Width_column = ListViewEx.Win32.GetPrivateProfileString("Form_Sterilizzatrici", "Width", "", Application.StartupPath + "\\kleantrak.ini");
			string Height_column = ListViewEx.Win32.GetPrivateProfileString("Form_Sterilizzatrici", "Height", "", Application.StartupPath + "\\kleantrak.ini");

			if (Width_column.Length > 0 && Height_column.Length > 0 && X.Length > 0 && Y.Length > 0)
			{
				int Height_column2 = Convert.ToInt32(Height_column);
				int Width_column2 = Convert.ToInt32(Width_column);
				int X2 = Convert.ToInt32(X);
				int Y2 = Convert.ToInt32(Y);

				this.DesktopBounds = new Rectangle(X2, Y2, Width_column2, Height_column2);
				this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			}
		}

		private void SaveFormSize()
		{
			string altezza;
			string larghezza;
			altezza = Dispositivi.ActiveForm.Size.Height.ToString();
			larghezza = Dispositivi.ActiveForm.Size.Width.ToString();

			ListViewEx.Win32.WritePrivateProfileString("Form_Sterilizzatrici", "Width", larghezza, Application.StartupPath + "\\kleantrak.ini");
			ListViewEx.Win32.WritePrivateProfileString("Form_Sterilizzatrici", "Height", altezza, Application.StartupPath + "\\kleantrak.ini");
		}

		private void listview_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e)
		{
            ((ListViewEx.ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
        }

		private void button1_Click_1(object sender, System.EventArgs e)
		{
			//nuova sterilizzatrice
			string strCommand = "";
			//OdbcConnection connTemp = new OdbcConnection(CleanTrack.Globals.strDatabase);
			//connTemp.Open();
			try
			{
				strCommand = "insert into ARMADI_LAVATRICI (descrizione, MATRICOLA, Seriale, UO) values ('Nuova Sterilizzatrice', 'DEFINIRE MATRICOLA', '', " + KleanTrak.Globals.UO.ToString() + ")";

				OdbcConnection connTemp = DBUtil.GetODBCConnection();
				OdbcCommand commTemp = new OdbcCommand(strCommand, connTemp);
				commTemp.ExecuteNonQuery();

				//INSERIMENTO NEL LOG DI SISTEMA
				//OdbcConnection connTemp2 = new OdbcConnection(CleanTrack.Globals.strDatabase);
				//connTemp2.Open();

				OdbcCommand commTemp2 = new OdbcCommand("SELECT MAX (ID) as MAXID FROM ARMADI_LAVATRICI", connTemp);
				OdbcDataReader readerTemp2 = commTemp2.ExecuteReader();
				if (readerTemp2.Read())
				{
					if (!readerTemp2.IsDBNull(readerTemp2.GetOrdinal("MAXID")))
					{
						int MAXID = DBUtil.GetIntValue(readerTemp2, "MAXID");//readerTemp2.GetInt32(readerTemp2.GetOrdinal("MAXID"));

						if ((readerTemp2 != null) && (readerTemp2.IsClosed == false))
							readerTemp2.Close();

						OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD ) VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Inserimento', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + MAXID + "')", connTemp);
						commTemp_LOG.ExecuteNonQuery();
					}
				}
				//////

				connTemp.Close();

				RiempiLista();

				listView.EnsureVisible(listView.Items.Count - 1);
				listView.EditCell(listView.Items.Count - 1, 0);
			}
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void listView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			//elimino una sterilizzatrice dalla lista
			if (e.KeyCode == Keys.Delete && (listView.SelectedIndices.Count > 0))
			{
                if (MessageBox.Show(KleanTrak.Globals.strTable[57], "Clean Track", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    DeleteItemSelected();
			}
		}

		private void Sterilizzatrici_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveFormSize();
		}

        private void DeleteItemSelected()
        {
            OdbcConnection connTemp = DBUtil.GetODBCConnection();

            //verifica che la sterilizzatrice non sia già stata utilizzata
            OdbcCommand commTemp_check = new OdbcCommand("SELECT Cicli.idsterilizzatrice from Cicli " +
				"LEFT OUTER JOIN ARMADI_LAVATRICI ON Cicli.idsterilizzatrice = ARMADI_LAVATRICI.ID " +
						"where ARMADI_LAVATRICI.ID  = " + listView.Items[listView.SelectedIndices[0]].Tag.ToString(), connTemp);

            OdbcDataReader readerTemp = null; 

            try
            {
                readerTemp = commTemp_check.ExecuteReader();
                if (!readerTemp.Read())
                {
                    OdbcCommand commTemp = new OdbcCommand("DELETE FROM ARMADI_LAVATRICI WHERE ID=" + listView.Items[listView.SelectedIndices[0]].Tag.ToString(), connTemp);
                    commTemp.ExecuteNonQuery();

                    //LOG DI SISTEMA
                    OdbcCommand commTemp_LOG = new OdbcCommand("INSERT INTO LOG (UTENTE, TABELLA, OPERAZIONE, DATA, RECORD ) VALUES ('" + KleanTrak.Globals.m_strUser + "', 'ARMADI_LAVATRICI', 'Cancellazione', '" + KleanTrak.Globals.ConvertDateTime(DateTime.Now) + "', '" + listView.Items[listView.SelectedIndices[0]].Tag.ToString() + "')", connTemp);
                    commTemp_LOG.ExecuteNonQuery();

                    listView.Items.RemoveAt(listView.SelectedIndices[0]);
                }
                else
                    MessageBox.Show(KleanTrak.Globals.strTable[52], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception)
            {
            }

            if (readerTemp != null && !readerTemp.IsClosed)
                readerTemp.Close();
            connTemp.Close();
        }

        public class ItemCombo
        {
            public int id { set; get; }
            public string strName { set; get; }
            public override string ToString() { return strName; }
        }

        private ArrayList GetComboTipoList()
        {
            Array values = Enum.GetValues(typeof(KleanTrak.Models.WasherTypes));
            string[] names = Enum.GetNames(typeof(KleanTrak.Models.WasherTypes));

            ArrayList list = new ArrayList();
            for (int i = 0; i < names.Length; i++)
            {
                list.Add(new ItemCombo() { id = (int)(KleanTrak.Models.WasherTypes)values.GetValue(i), strName = names[i] });
            }

            return list;
        }

        private ArrayList GetComboDismissioneList()
        {
            ArrayList list = new ArrayList();

            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("SELECT ID, Causale FROM Causali", connTemp);
            OdbcDataReader readerTemp = null;

            try
            {
                readerTemp = commTemp.ExecuteReader();
                while (readerTemp.Read())
                {
                    ItemCombo item = new ItemCombo();
                    item.id = DBUtil.GetIntValue(readerTemp, "ID");
                    item.strName = readerTemp.GetString(readerTemp.GetOrdinal("Causale"));
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
    }
}
