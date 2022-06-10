using System;
using kleanTrak.forms;
using System.Data.Odbc;
using System.Drawing;
using System.Windows.Forms;
using OdbcExtensions;
using LibLog;

namespace KleanTrak
{
	public partial class ConfigurazioneStatiDispositivi : Form
	{
		private ListViewEx.ListViewEx listView;
		private ColumnHeader headID;
		private ColumnHeader headDescrizione;
		private ColumnHeader headDescrizioneAzione;
		private ColumnHeader headBarcode;
		private ColumnHeader headColore;
		private ColumnHeader headVisibileMenu;
		private ColumnHeader headVisibileLista;
		private ColumnHeader headVISIBILELISTADETTAGLIO;
		private ColumnHeader headInizioCiclo;
		private ColumnHeader headInizioSterilizzazione;
		private ColumnHeader headFineSterilizzazione;
		private ColumnHeader headInizioStoccaggio;
		private ColumnHeader headFineStoccaggio;
		private ColumnHeader headSCELTARAPIDA;

		private bool m_bFatteModifiche = false;

		private int m_iVisibileMenuMax = 0;
		private int m_iVisibileListaMax = 0;
		private int m_iVisibileListaDettaglioMax = 0;
        private int m_iVisibilePrincipaleMax = 0;
        
        public ConfigurazioneStatiDispositivi()
		{
			InitializeComponent();
		}

		private void ConfigurazioneStatiDispositivi_Load(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				RiempiLista();
				WindowState = FormWindowState.Maximized;
				MinimumSize = Size;
				SetTooltips();
				//Globals.ResizeList(this, listView); --> in questo caso deve restare in riempi lista.
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
			tsbnew.ToolTipText = KleanTrak.Globals.strTable[211];
			tsbupdate.ToolTipText = KleanTrak.Globals.strTable[212];
			tsbdelete.ToolTipText = KleanTrak.Globals.strTable[213];
			tsbclose.ToolTipText = KleanTrak.Globals.strTable[185];
		}
		private void RiempiLista()
		{
			foreach (Bitmap bmp in imageList.Images)
				bmp.Dispose();
			imageList.Images.Clear();
			listView.Items.Clear();
			tsbupdate.Enabled = false;
			tsbdelete.Enabled = false;
			m_iVisibileMenuMax = 0;
			m_iVisibileListaMax = 0;
			m_iVisibileListaDettaglioMax = 0;
            m_iVisibilePrincipaleMax = 0;

			using (OdbcConnection connProva1 = DBUtil.GetODBCConnection())
			{
				string sQuery1 = "SELECT ID, VisibileMenu FROM Stato WHERE Eliminato = 0 AND VisibileMenu > 0 ORDER BY VisibileMenu, ID";
				using (OdbcCommand commTemp1 = new OdbcCommand(sQuery1, connProva1))
				{
					using (OdbcDataReader readerTemp1 = commTemp1.ExecuteReader())
					{
						while (readerTemp1.Read())
						{
							m_iVisibileMenuMax++;

							if (m_iVisibileMenuMax != readerTemp1.GetIntEx("VisibileMenu"))
							{
								using (OdbcConnection connProva2 = DBUtil.GetODBCConnection())
								{
									using (OdbcCommand commTemp2 = new OdbcCommand($"UPDATE Stato SET VisibileMenu = {m_iVisibileMenuMax} WHERE ID = {readerTemp1.GetIntEx("ID")}", connProva2))
									{
										commTemp2.ExecuteNonQuery();
									}
								}
							}
						}
					}
				}
			}

			using (OdbcConnection connProva1 = DBUtil.GetODBCConnection())
			{
				string sQuery1 = "SELECT ID, VisibileLista FROM Stato WHERE Eliminato = 0 AND VisibileLista > 0 ORDER BY VisibileLista, ID";
				using (OdbcCommand commTemp1 = new OdbcCommand(sQuery1, connProva1))
				{
					using (OdbcDataReader readerTemp1 = commTemp1.ExecuteReader())
					{
						while (readerTemp1.Read())
						{
							m_iVisibileListaMax++;

							if (m_iVisibileListaMax != readerTemp1.GetIntEx("VisibileLista"))
							{
								using (OdbcConnection connProva2 = DBUtil.GetODBCConnection())
								{
									using (OdbcCommand commTemp2 = new OdbcCommand($"UPDATE Stato SET VisibileLista = {m_iVisibileListaMax} WHERE ID = {readerTemp1.GetIntEx("ID")}", connProva2))
									{
										commTemp2.ExecuteNonQuery();
									}
								}
							}
						}
					}
				}
			}

			using (OdbcConnection connProva1 = DBUtil.GetODBCConnection())
			{
				string sQuery1 = "SELECT ID, VisibileListaDettaglio FROM Stato WHERE Eliminato = 0 AND VisibileListaDettaglio > 0 ORDER BY VisibileListaDettaglio, ID";
				using (OdbcCommand commTemp1 = new OdbcCommand(sQuery1, connProva1))
				{
					using (OdbcDataReader readerTemp1 = commTemp1.ExecuteReader())
					{
						while (readerTemp1.Read())
						{
							m_iVisibileListaDettaglioMax++;

							if (m_iVisibileListaDettaglioMax != readerTemp1.GetIntEx("VisibileListaDettaglio"))
							{
								using (OdbcConnection connProva2 = DBUtil.GetODBCConnection())
								{
									using (OdbcCommand commTemp2 = new OdbcCommand($"UPDATE Stato SET VisibileListaDettaglio = {m_iVisibileListaDettaglioMax} WHERE ID = {readerTemp1.GetIntEx("ID")}", connProva2))
									{
										commTemp2.ExecuteNonQuery();
									}
								}
							}
						}
					}
				}
			}

            using (OdbcConnection connProva1 = DBUtil.GetODBCConnection())
            {
                string sQuery1 = "SELECT ID, VisibilePrincipale FROM Stato WHERE Eliminato = 0 AND VisibilePrincipale > 0 ORDER BY VisibilePrincipale, ID";
                using (OdbcCommand commTemp1 = new OdbcCommand(sQuery1, connProva1))
                {
                    using (OdbcDataReader readerTemp1 = commTemp1.ExecuteReader())
                    {
                        while (readerTemp1.Read())
                        {
                            m_iVisibilePrincipaleMax++;

                            if (m_iVisibilePrincipaleMax != readerTemp1.GetIntEx("VisibilePrincipale"))
                            {
                                using (OdbcConnection connProva2 = DBUtil.GetODBCConnection())
                                {
                                    using (OdbcCommand commTemp2 = new OdbcCommand($"UPDATE Stato SET VisibilePrincipale = {m_iVisibilePrincipaleMax} WHERE ID = {readerTemp1.GetIntEx("ID")}", connProva2))
                                    {
                                        commTemp2.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //

            string sQuery = $"SELECT ID, DESCRIZIONE, DESCRIZIONEAZIONE, BARCODE, COLORE, " +
				$"ELIMINATO, VISIBILEMENU, VISIBILELISTA, " +
				$"INIZIOCICLO, " +
				$"INIZIOSTERILIZZAZIONE, FINESTERILIZZAZIONE, " +
				$"INIZIOSTOCCAGGIO, FINESTOCCAGGIO, " +
				$"INIZIOPRELAVAGGIO, FINEPRELAVAGGIO, " +
				$"VISIBILELISTADETTAGLIO, SCELTARAPIDA, " +
				$"VISIBILEPRINCIPALE " +
				$"FROM STATO " +
				$"WHERE ELIMINATO=0 ORDER BY ID";

			OdbcConnection connTemp = DBUtil.GetODBCConnection();
			OdbcCommand commTemp = new OdbcCommand(sQuery, connTemp);
			OdbcDataReader readerTemp = null;

			try
			{
				readerTemp = commTemp.ExecuteReader();

				ListViewItem lvItem;
				while (readerTemp.Read())
				{
					lvItem = listView.Items.Add(readerTemp.GetIntEx("ID").ToString());
					lvItem.Tag = readerTemp.GetIntEx("ID");

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("Descrizione")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("Descrizione")));

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("DescrizioneAzione")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("DescrizioneAzione")));

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("Barcode")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("Barcode")));

					lvItem.SubItems.Add(""); // Colore
					if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("Colore")))
					{
						State stateTemp = new State();
                        stateTemp.Hue = readerTemp.GetIntEx("Colore");
                        imageList.Images.Add(stateTemp.Image);
						listView.SetSubItemImage(lvItem.Index, 4, imageList.Images.Count - 1);
					}

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("VisibileMenu")))
					{
						lvItem.SubItems.Add("");
					}
					else
					{
						int iVal = readerTemp.GetIntEx("VisibileMenu");
						if(iVal > 0)
							lvItem.SubItems.Add(iVal.ToString());
						else
							lvItem.SubItems.Add("");
					}

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("VisibileLista")))
					{
						lvItem.SubItems.Add("");
					}
					else
					{
						int iVal = readerTemp.GetIntEx("VisibileLista");
						if (iVal > 0)
							lvItem.SubItems.Add(iVal.ToString());
						else
							lvItem.SubItems.Add("");
					}

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("VISIBILELISTADETTAGLIO")))
					{
						lvItem.SubItems.Add("");
					}
					else
					{
						int iVal = readerTemp.GetIntEx("VISIBILELISTADETTAGLIO");
						if (iVal > 0)
							lvItem.SubItems.Add(iVal.ToString());
						else
							lvItem.SubItems.Add("");
					}

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("InizioCiclo")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetBoolEx("InizioCiclo") ? "X" : "");

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("InizioSterilizzazione")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetBoolEx("InizioSterilizzazione") ? "X" : "");

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("FineSterilizzazione")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetBoolEx("FineSterilizzazione") ? "X" : "");

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("InizioStoccaggio")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetBoolEx("InizioStoccaggio") ? "X" : "");

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("FineStoccaggio")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetBoolEx("FineStoccaggio") ? "X" : "");

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("INIZIOPRELAVAGGIO")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetBoolEx("INIZIOPRELAVAGGIO") ? "X" : "");

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("FINEPRELAVAGGIO")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetBoolEx("FINEPRELAVAGGIO") ? "X" : "");

					if (readerTemp.IsDBNull(readerTemp.GetOrdinal("SCELTARAPIDA")))
						lvItem.SubItems.Add("");
					else
						lvItem.SubItems.Add(readerTemp.GetString(readerTemp.GetOrdinal("SCELTARAPIDA")));

                    if (readerTemp.IsDBNull(readerTemp.GetOrdinal("VISIBILEPRINCIPALE")))
                    {
                        lvItem.SubItems.Add("");
                    }
                    else
                    {
						int iVal = readerTemp.GetIntEx("VISIBILEPRINCIPALE");
                        if (iVal > 0)
                            lvItem.SubItems.Add(iVal.ToString());
                        else
                            lvItem.SubItems.Add("");
                    }
                }
            }
			catch (OdbcException Ex)
			{
				MessageBox.Show(Ex.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			if (readerTemp != null && readerTemp.IsClosed == false)
				readerTemp.Close();
			connTemp.Close();
			Globals.ResizeList(this, listView);
		}

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			tsbupdate.Enabled = (listView.SelectedIndices.Count == 1);
			tsbdelete.Enabled = (listView.SelectedIndices.Count == 1);
		}

		private void ConfigurazioneStatiDispositivi_ClientSizeChanged(object sender, System.EventArgs e) => Globals.ResizeList(this, listView);

		private void tsbnew_Click(object sender, EventArgs e)
		{
			ConfigurazioneStatiDispositiviItemNewEdit dlg = new ConfigurazioneStatiDispositiviItemNewEdit(0, m_iVisibileMenuMax, m_iVisibileListaMax, m_iVisibileListaDettaglioMax, m_iVisibilePrincipaleMax);
			dlg.ShowDialog();
			if (dlg.DialogResult == DialogResult.OK)
			{
				m_bFatteModifiche = true;
				RiempiLista();
			}
		}

		private void tsbupdate_Click(object sender, EventArgs e)
		{
			if (listView.SelectedIndices.Count == 1)
			{
				ConfigurazioneStatiDispositiviItemNewEdit dlg = new ConfigurazioneStatiDispositiviItemNewEdit((int)listView.Items[listView.SelectedIndices[0]].Tag, m_iVisibileMenuMax, m_iVisibileListaMax, m_iVisibileListaDettaglioMax, m_iVisibilePrincipaleMax);
				dlg.ShowDialog();
				if (dlg.DialogResult == DialogResult.OK)
				{
					m_bFatteModifiche = true;
					RiempiLista();
				}
			}
		}

		private void tsbdelete_Click(object sender, EventArgs e)
		{
			if (listView.SelectedIndices.Count == 1)
			{
				if (MessageBox.Show("Desideri davvero eliminare lo stato selezionato?", "ATTENZIONE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
					{
						using (OdbcCommand commTemp = new OdbcCommand("UPDATE Stato SET Eliminato = 1, VisibileMenu = 0, VisibileLista = 0, VisibileListaDettaglio = 0, InizioCiclo = 0, InizioSterilizzazione = 0, FineSterilizzazione = 0, InizioStoccaggio = 0, FineStoccaggio = 0, SceltaRapida = '' WHERE ID = " + ((int)listView.Items[listView.SelectedIndices[0]].Tag).ToString(), connTemp))
						{
							if (commTemp.ExecuteNonQuery() > 0)
							{
								m_bFatteModifiche = true;
								RiempiLista();
							}
						}
					}
				}
			}
		}

		private void tsbclose_Click(object sender, EventArgs e)
		{
			if (m_bFatteModifiche)
				MessageBox.Show("Riavvia CleanTrack e CleanTrackServer per rendere effettive le modifiche!", 
					"ATTENZIONE", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Warning);
			Close();
		}
	}
}

