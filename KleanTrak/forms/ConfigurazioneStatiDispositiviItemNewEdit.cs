using KleanTrak;
using OdbcExtensions;
using System;
using System.Data.Odbc;
using System.Windows.Forms;

namespace kleanTrak.forms
{
	public partial class ConfigurazioneStatiDispositiviItemNewEdit : Form
	{
		private int m_iStatoDaModificare;

		private int m_iVisibileMenuMax;
		private int m_iVisibileListaMax;
		private int m_iVisibileListaDettaglioMax;
        private int m_iVisibilePrincipaleMax;

        private int m_iVisibileMenuOriginal = 0;
		private int m_iVisibileListaOriginal = 0;
		private int m_iVisibileListaDettaglioOriginal = 0;
        private int m_iVisibilePrincipaleOriginal = 0;
        
        public ConfigurazioneStatiDispositiviItemNewEdit(int iStatoDaModificare, 
			int iVisibileMenuMax, 
			int iVisibileListaMax, 
			int iVisibileListaDettaglioMax, 
			int iVisibilePrincipaleMax)
		{
			InitializeComponent();
			m_iStatoDaModificare = iStatoDaModificare;
			m_iVisibileMenuMax = iVisibileMenuMax;
			m_iVisibileListaMax = iVisibileListaMax;
			m_iVisibileListaDettaglioMax = iVisibileListaDettaglioMax;
            m_iVisibilePrincipaleMax = iVisibilePrincipaleMax;
        }

		private void ConfigurazioneStatiDispositiviItemNewEdit_Load(object sender, EventArgs e)
		{
			nudVisibileMenu.Maximum = m_iVisibileMenuMax;
			nudVisibileLista.Maximum = m_iVisibileListaMax;
			nudVisibileListaDettaglio.Maximum = m_iVisibileListaDettaglioMax;

			if (m_iStatoDaModificare <= 0)
			{
				Text = "Nuovo stato dispositivo";
				txtID.Text = "NUOVO";

				State stateTemp = new State();
				stateTemp.Hue = 0;
				pctColore.Image = stateTemp.Image;
			}
			else
			{
				Text = "Modifica stato dispositivo";
				var selectquery = $"SELECT ID, DESCRIZIONE, DESCRIZIONEAZIONE, " +
					$"BARCODE, COLORE, VISIBILEMENU, VISIBILELISTA, " +
					$"VISIBILELISTADETTAGLIO, VISIBILEPRINCIPALE, " +
					$"INIZIOCICLO, INIZIOSTERILIZZAZIONE, " +
					$"FINESTERILIZZAZIONE, " +
					$"INIZIOSTOCCAGGIO, FINESTOCCAGGIO, " +
					$"INIZIOPRELAVAGGIO, FINEPRELAVAGGIO, SCELTARAPIDA " +
					$"FROM STATO WHERE ID = {m_iStatoDaModificare}";
				using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
				{
					using (OdbcCommand commTemp = new OdbcCommand(selectquery, connTemp))
					{
						using (OdbcDataReader readTemp = commTemp.ExecuteReader())
						{
							if (readTemp.Read())
							{
								txtID.Text = m_iStatoDaModificare.ToString();
								txtDescrizione.Text = readTemp.GetStringEx("Descrizione").Trim();
								btnSalva.Enabled = (txtDescrizione.Text.Length > 0);
								txtDescrizioneAzione.Text = readTemp.GetStringEx("DescrizioneAzione");
								txtBarcode.Text = readTemp.GetStringEx("Barcode");
								trkColore.Value = readTemp.GetIntEx("Colore") % trkColore.Maximum;
								State stateTemp = new State();
								stateTemp.Hue = trkColore.Value;
								pctColore.Image = stateTemp.Image;
								m_iVisibileMenuOriginal = readTemp.GetIntEx("VisibileMenu");
								if (m_iVisibileMenuOriginal > 0)
								{
									m_iVisibileMenuMax--;
									chkVisibileMenu.Checked = true;
									nudVisibileMenu.Value = m_iVisibileMenuOriginal;
								}
								m_iVisibileListaOriginal = readTemp.GetIntEx("VisibileLista");
								if (m_iVisibileListaOriginal > 0)
								{
									m_iVisibileListaMax--;
									chkVisibileLista.Checked = true;
									nudVisibileLista.Value = m_iVisibileListaOriginal;
								}
								m_iVisibileListaDettaglioOriginal = readTemp.GetIntEx("VisibileListaDettaglio");
								if (m_iVisibileListaDettaglioOriginal > 0)
								{
									m_iVisibileListaDettaglioMax--;
									chkVisibileListaDettaglio.Checked = true;
									nudVisibileListaDettaglio.Value = m_iVisibileListaDettaglioOriginal;
								}
								m_iVisibilePrincipaleOriginal = readTemp.GetIntEx("VisibilePrincipale");
                                if (m_iVisibilePrincipaleOriginal > 0)
                                {
                                    m_iVisibilePrincipaleMax--;
                                    chkVisibilePrincipale.Checked = true;
                                    nudVisibilePrincipale.Value = m_iVisibilePrincipaleOriginal;
                                }
                                bool bInizioCiclo = readTemp.GetBoolEx("InizioCiclo");
								chkInizioCiclo.Checked = bInizioCiclo;
								chkInizioCiclo.Enabled = !bInizioCiclo;
								bool bInizioSterilizzazione = readTemp.GetBoolEx("InizioSterilizzazione");
								chkInizioSterilizzazione.Checked = bInizioSterilizzazione;
								chkInizioSterilizzazione.Enabled = !bInizioSterilizzazione;
								bool bFineSterilizzazione = readTemp.GetBoolEx("FineSterilizzazione");
								chkFineSterilizzazione.Checked = bFineSterilizzazione;
								chkFineSterilizzazione.Enabled = !bFineSterilizzazione;
								bool bInizioStoccaggio = readTemp.GetBoolEx("InizioStoccaggio");
								chkInizioStoccaggio.Checked = bInizioStoccaggio;
								chkInizioStoccaggio.Enabled = !bInizioStoccaggio;
								bool bFineStoccaggio = readTemp.GetBoolEx("FineStoccaggio");
								chkFineStoccaggio.Checked = bFineStoccaggio;
								chkFineStoccaggio.Enabled = !bFineStoccaggio;
								bool startprewash = readTemp.GetBoolEx("INIZIOPRELAVAGGIO");
								chkstartprewash.Checked = startprewash;
								chkstartprewash.Enabled = !startprewash;
								bool endprewash = readTemp.GetBoolEx("FINEPRELAVAGGIO");
								chkendprewash.Checked = endprewash;
								chkendprewash.Enabled = !endprewash;
								txtSceltaRapida.Text = readTemp.GetStringEx("SceltaRapida");
							}
						}
					}
				}
			}
		}
		private bool BarCodePresente(string strText, int id)
		{
			OdbcCommand cmd = null;
			try
			{
				cmd = new OdbcCommand("SELECT COUNT(*) FROM STATO WHERE UPPER(BARCODE) = UPPER(?) AND ID <> ? AND ELIMINATO = 0", DBUtil.GetODBCConnection());
				cmd.Parameters.Add(new OdbcParameter("pbarcode", OdbcType.NVarChar, 255) { Value = strText });
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
		private void btnSalva_Click(object sender, EventArgs e)
		{
			try
			{
				int max_id = -1;
				if (m_iStatoDaModificare <= 0)
				{
					using (OdbcConnection conn = DBUtil.GetODBCConnection())
					{
						using (OdbcCommand cmd = new OdbcCommand("SELECT MAX (ID) FROM STATO", conn))
						{
							max_id = cmd.ExecuteScalarInt();
							max_id = (max_id < 0) ? 0 : max_id;
							max_id++;
							cmd.CommandText = $"INSERT INTO STATO (ID, DESCRIZIONE) VALUES ({max_id}, 'NEW STATE')";
							cmd.ExecuteNonQuery();
							m_iStatoDaModificare = max_id;
						}
					}
				}
				if (m_iStatoDaModificare > 0)
				{
					string sBarcode = txtBarcode.Text.Trim().Replace("'", "''");
					if (BarCodePresente(sBarcode, m_iStatoDaModificare))
					{
						MessageBox.Show(KleanTrak.Globals.strTable[210], "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}

					string sDescrizione = txtDescrizione.Text.Trim().Replace("'", "''");
					string sDescrizioneAzione = txtDescrizioneAzione.Text.Trim().Replace("'", "''");
					int iColore = trkColore.Value % trkColore.Maximum;
					int iVisibileMenu = (chkVisibileMenu.Checked ? (int)nudVisibileMenu.Value : 0);
					int iVisibileLista = (chkVisibileLista.Checked ? (int)nudVisibileLista.Value : 0);
					int iVisibilePrincipale = (chkVisibilePrincipale.Checked ? (int)nudVisibilePrincipale.Value : 0);
					int iVisibileListaDettaglio = (chkVisibileListaDettaglio.Checked ? (int)nudVisibileListaDettaglio.Value : 0);
					string sSceltaRapida = txtSceltaRapida.Text.Trim().Replace("'", "''");
					if (iVisibileMenu != m_iVisibileMenuOriginal)
					{
						if (iVisibileMenu > m_iVisibileMenuOriginal)
						{
							if (m_iVisibileMenuOriginal > 0)
								MoveDownOrder("VisibileMenu", m_iVisibileMenuOriginal, iVisibileMenu);
							else
								MoveUpOrder("VisibileMenu", iVisibileMenu);
						}
						else
						{
							if (iVisibileMenu > 0)
								MoveUpOrder("VisibileMenu", iVisibileMenu);
							else
								MoveDownOrder("VisibileMenu", m_iVisibileMenuOriginal, m_iVisibileMenuMax + 1);
						}
					}
					if (iVisibileLista != m_iVisibileListaOriginal)
					{
						if (iVisibileLista > m_iVisibileListaOriginal)
						{
							if (m_iVisibileListaOriginal > 0)
								MoveDownOrder("VisibileLista", m_iVisibileListaOriginal, iVisibileLista);
							else
								MoveUpOrder("VisibileLista", iVisibileLista);
						}
						else
						{
							if (iVisibileLista > 0)
								MoveUpOrder("VisibileLista", iVisibileLista);
							else
								MoveDownOrder("VisibileLista", m_iVisibileListaOriginal, m_iVisibileListaMax + 1);
						}
					}
					if (iVisibilePrincipale != m_iVisibilePrincipaleOriginal)
					{
						if (iVisibilePrincipale > m_iVisibilePrincipaleOriginal)
						{
							if (m_iVisibilePrincipaleOriginal > 0)
								MoveDownOrder("VisibilePrincipale", m_iVisibilePrincipaleOriginal, iVisibilePrincipale);
							else
								MoveUpOrder("VisibilePrincipale", iVisibilePrincipale);
						}
						else
						{
							if (iVisibilePrincipale > 0)
								MoveUpOrder("VisibilePrincipale", iVisibilePrincipale);
							else
								MoveDownOrder("VisibilePrincipale", m_iVisibilePrincipaleOriginal, m_iVisibilePrincipaleMax + 1);
						}
					}
					if (iVisibileListaDettaglio != m_iVisibileListaDettaglioOriginal)
					{
						if (iVisibileListaDettaglio > m_iVisibileListaDettaglioOriginal)
						{
							if (m_iVisibileListaDettaglioOriginal > 0)
								MoveDownOrder("VisibileListaDettaglio", m_iVisibileListaDettaglioOriginal, iVisibileListaDettaglio);
							else
								MoveUpOrder("VisibileListaDettaglio", iVisibileListaDettaglio);
						}
						else
						{
							if (iVisibileListaDettaglio > 0)
								MoveUpOrder("VisibileListaDettaglio", iVisibileListaDettaglio);
							else
								MoveDownOrder("VisibileListaDettaglio", m_iVisibileListaDettaglioOriginal, m_iVisibileListaDettaglioMax + 1);
						}
					}
					if (chkInizioCiclo.Checked)
						ResetFlag("InizioCiclo");
					if (chkInizioSterilizzazione.Checked)
						ResetFlag("InizioSterilizzazione");
					if (chkFineSterilizzazione.Checked)
						ResetFlag("FineSterilizzazione");
					if (chkInizioStoccaggio.Checked)
						ResetFlag("InizioStoccaggio");
					if (chkFineStoccaggio.Checked)
						ResetFlag("FineStoccaggio");
					if (chkstartprewash.Checked)
						ResetFlag("INIZIOPRELAVAGGIO");
					if (chkendprewash.Checked)
						ResetFlag("FINEPRELAVAGGIO");
					var updatestr = $"UPDATE Stato SET Descrizione = '{sDescrizione}', " +
						$"DescrizioneAzione = '{sDescrizioneAzione}', Barcode = '{sBarcode}', " +
						$"Colore = {iColore}, " +
						$"VisibileMenu = {iVisibileMenu}, VisibileLista = {iVisibileLista}, " +
						$"VisibileListaDettaglio = {iVisibileListaDettaglio}, " +
						$"VisibilePrincipale = {iVisibilePrincipale}, " +
						$"InizioCiclo = {(chkInizioCiclo.Checked? 1 : 0)}, " +
						$"InizioSterilizzazione = {(chkInizioSterilizzazione.Checked? 1 : 0)}, " +
						$"FineSterilizzazione = {(chkFineSterilizzazione.Checked? 1 : 0)}, " +
						$"InizioStoccaggio = {(chkInizioStoccaggio.Checked ? 1 : 0)}, " +
						$"FineStoccaggio = {(chkFineStoccaggio.Checked ? 1 : 0)}, " +
						$"INIZIOPRELAVAGGIO = {(chkstartprewash.Checked ? 1 : 0)}, " +
						$"FINEPRELAVAGGIO = {(chkendprewash.Checked ? 1 : 0)}, " +
						$"SceltaRapida = '{sSceltaRapida}' " +
						$"WHERE ID = { m_iStatoDaModificare}";
					using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
					using (OdbcCommand commTemp = new OdbcCommand(updatestr, connTemp))
						commTemp.ExecuteNonQuery();
				}
				else
				{
					MessageBox.Show("Errore durante l'inserimento del nuovo stato!",
						"ERRORE",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace,
					"ERRORE",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
			finally
			{
				Close();
			}
		}
		private void MoveDownOrder(string sCampo, int iVecchioOrdine, int iNuovoOrdine)
		{
			using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
			{
				using (OdbcCommand commTemp = new OdbcCommand("UPDATE Stato SET " + sCampo + " = " + sCampo + " - 1 WHERE " + sCampo + " > " + iVecchioOrdine.ToString() + " AND " + sCampo + " <= " + iNuovoOrdine.ToString(), connTemp))
				{
					commTemp.ExecuteNonQuery();
				}
			}
		}

		private void MoveUpOrder(string sCampo, int iNuovoOrdine)
		{
			using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
			{
				using (OdbcCommand commTemp = new OdbcCommand("UPDATE Stato SET " + sCampo + " = " + sCampo + " + 1 WHERE " + sCampo + " >= " + iNuovoOrdine.ToString(), connTemp))
				{
					commTemp.ExecuteNonQuery();
				}
			}
		}

		private void ResetFlag(string sCampo)
		{
			using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
			{
				using (OdbcCommand commTemp = new OdbcCommand($"UPDATE Stato SET {sCampo} = 0", connTemp))
				{
					commTemp.ExecuteNonQuery();
				}
			}
		}

		private void btnAnnulla_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void trkColore_Scroll(object sender, EventArgs e)
		{
			State stateTemp = new State();
			stateTemp.Hue = trkColore.Value;
			pctColore.Image = stateTemp.Image;
		}

		private void chkVisibileMenu_CheckedChanged(object sender, EventArgs e)
		{
			if (chkVisibileMenu.Checked)
			{
				m_iVisibileMenuMax++;
				nudVisibileMenu.Maximum = m_iVisibileMenuMax;
				nudVisibileMenu.Value = m_iVisibileMenuMax;
				nudVisibileMenu.Visible = true;
			}
			else
			{
				m_iVisibileMenuMax--;
				nudVisibileMenu.Visible = false;
			}
		}

        private void chkVisibilePrincipale_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVisibilePrincipale.Checked)
            {
                m_iVisibilePrincipaleMax++;
                nudVisibilePrincipale.Maximum = m_iVisibilePrincipaleMax;
                nudVisibilePrincipale.Value = m_iVisibilePrincipaleMax;
                nudVisibilePrincipale.Visible = true;
            }
            else
            {
                m_iVisibilePrincipaleMax--;
                nudVisibilePrincipale.Visible = false;
            }
        }

        private void chkVisibileLista_CheckedChanged(object sender, EventArgs e)
		{
			if (chkVisibileLista.Checked)
			{
				m_iVisibileListaMax++;
				nudVisibileLista.Maximum = m_iVisibileListaMax;
				nudVisibileLista.Value = m_iVisibileListaMax;
				nudVisibileLista.Visible = true;
			}
			else
			{
				m_iVisibileListaMax--;
				nudVisibileLista.Visible = false;
			}
		}

		private void chkVisibileListaDettaglio_CheckedChanged(object sender, EventArgs e)
		{
			if (chkVisibileListaDettaglio.Checked)
			{
				m_iVisibileListaDettaglioMax++;
				nudVisibileListaDettaglio.Maximum = m_iVisibileListaDettaglioMax;
				nudVisibileListaDettaglio.Value = m_iVisibileListaDettaglioMax;
				nudVisibileListaDettaglio.Visible = true;
			}
			else
			{
				m_iVisibileListaDettaglioMax--;
				nudVisibileListaDettaglio.Visible = false;
			}
		}

		private void txtDescrizione_TextChanged(object sender, EventArgs e)
		{
			btnSalva.Enabled = (txtDescrizione.Text.Trim().Length > 0);
		}
	}
}
