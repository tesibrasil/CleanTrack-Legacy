using KleanTrak.Core;
using KleanTrak.Model;
using OdbcExtensions;
using System;
using System.Collections;
using System.Data.Odbc;
using System.Windows.Forms;

namespace KleanTrak
{
	public partial class ConfigurazioneScadenzaDispositivi : Form
	{
		private bool m_bFatteModifiche = false;

		private class ComboItem
		{
			public int iID;
			public string sName;

			public override string ToString() { return sName; }
		}

		public ConfigurazioneScadenzaDispositivi()
		{
			InitializeComponent();
			listView.ListViewItemSorter = new ListViewEx.ListViewExComparer(listView);
			listView.SetSubItemType(1, ListViewEx.ListViewEx.FieldType.combo, ComboTipoDispositivo());
			listView.SetSubItemType(2, ListViewEx.ListViewEx.FieldType.combo, ComboStato());
			listView.SetSubItemType(3, ListViewEx.ListViewEx.FieldType.textbox_number, null);
			listView.SetSubItemType(4, ListViewEx.ListViewEx.FieldType.combo, ComboStato());
			listView.SetEndEditCallback(new ListViewEx.ListViewEx.EndEditingCallbackEdit(EndEdit), new ListViewEx.ListViewEx.EndEditingCallbackCombo(EndCombo), null, null);
			tsb_add.ToolTipText = Globals.strTable[204];
			tsb_delete.ToolTipText = Globals.strTable[205];
			tsb_close.ToolTipText = Globals.strTable[206];
		}

		private void ConfigurazioneScadenzaDispositivi_Load(object sender, EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Globals.LocalizzaDialog(this);
				RiempiLista(0);
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
		private void btnNuovo_Click(object sender, EventArgs e)
		{
		}

		private void btnElimina_Click(object sender, EventArgs e)
		{
		}

		private void btnChiudi_Click(object sender, EventArgs e)
		{
		}

		private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			((ListViewEx.ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e) => tsb_delete.Visible =  (listView.SelectedIndices.Count > 0);
		private bool EndCombo(int iItemNum, int iSubitemNum, object obj)
		{
			string sColumn = "";

			switch (iSubitemNum)
			{
				case 1:
				{
					sColumn = "IDTipoDispositivo";
					break;
				}
				case 2:
				{
					sColumn = "IDStato";
					break;
				}
				case 4:
				{
					sColumn = "IDStatoDestinazione";
					break;
				}
			}

			if (sColumn.Length > 0)
			{
				int iID = (int)listView.Items[iItemNum].Tag;
				if (iID > 0)
				{
					using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
					{
						using (OdbcCommand commTemp = new OdbcCommand("UPDATE TipiDispositiviStatoScadenza SET " + sColumn + " = " + ((ComboItem)obj).iID.ToString() + " WHERE ID = " + iID.ToString(), connTemp))
						{
							if (commTemp.ExecuteNonQuery() > 0)
								m_bFatteModifiche = true;
						}
					}
				}
			}

			return true;
		}
		private bool EndEdit(int iItemNum, int iSubitemNum, string strText)
		{
			string sColumn = "";

			switch (iSubitemNum)
			{
				case 3:
				{
					sColumn = "ScadenzaMinuti";
					break;
				}
			}

			if (sColumn.Length > 0)
			{
				int iID = (int)listView.Items[iItemNum].Tag;
				if (iID > 0)
				{
					using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
					{
						int iValue;
						Int32.TryParse(strText, out iValue);

						using (OdbcCommand commTemp = new OdbcCommand("UPDATE TipiDispositiviStatoScadenza SET " + sColumn + " = " + iValue.ToString() + " WHERE ID = " + iID.ToString(), connTemp))
						{
							if (commTemp.ExecuteNonQuery() > 0)
								m_bFatteModifiche = true;
						}
					}
				}
			}

			return true;
		}
		private ArrayList ComboStato()
		{
			ArrayList listStati = new ArrayList();

			using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
			{
				using (OdbcCommand commTemp = new OdbcCommand("SELECT ID, Descrizione FROM Stato WHERE Eliminato = 0 ORDER BY Descrizione", connTemp))
				{
					using (OdbcDataReader readTemp = commTemp.ExecuteReader())
					{
						while (readTemp.Read())
						{
							listStati.Add(new ComboItem
							{
								iID = readTemp.GetIntEx("ID"),
								sName = readTemp.GetStringEx("Descrizione")
							});
						}
					}
				}
			}
			return listStati;
		}
		private ArrayList ComboTipoDispositivo()
		{
			ArrayList listTipiDispositivi = new ArrayList();

			using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
			{
				using (OdbcCommand commTemp = new OdbcCommand("SELECT ID, Descrizione FROM TipiDispositivi ORDER BY Descrizione", connTemp))
				{
					using (OdbcDataReader readTemp = commTemp.ExecuteReader())
					{
						while (readTemp.Read())
						{
							listTipiDispositivi.Add(new ComboItem
							{
								iID = readTemp.GetIntEx("ID"),
								sName = readTemp.GetStringEx("Descrizione")
							});
						}
					}
				}
			}
			return listTipiDispositivi;
		}
		private void RiempiLista(int iIDToSelect)
		{
			listView.Items.Clear();
			tsb_delete.Visible = false;
			using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
			{
				using (OdbcCommand commTemp = new OdbcCommand("SELECT TipiDispositiviStatoScadenza.ID, IDTipoDispositivo, TipiDispositivi.Descrizione AS DescrTipoDispositivo, IDStato, S1.Descrizione AS DescrStato, ScadenzaMinuti, IDStatoDestinazione, S2.Descrizione AS DescrStatoDestinazione FROM TipiDispositiviStatoScadenza INNER JOIN TipiDispositivi ON TipiDispositiviStatoScadenza.IDTipoDispositivo = TipiDispositivi.ID INNER JOIN Stato S1 ON TipiDispositiviStatoScadenza.IDStato = S1.ID INNER JOIN Stato S2 ON TipiDispositiviStatoScadenza.IDStatoDestinazione = S2.ID WHERE TipiDispositiviStatoScadenza.Eliminato = 0 ORDER BY S1.Descrizione, S2.Descrizione", connTemp))
				{
					using (OdbcDataReader readerTemp = commTemp.ExecuteReader())
					{
						while (readerTemp.Read())
						{
							if (!readerTemp.IsDBNull(readerTemp.GetOrdinal("DescrTipoDispositivo")) && !readerTemp.IsDBNull(readerTemp.GetOrdinal("DescrStato")) && !readerTemp.IsDBNull(readerTemp.GetOrdinal("DescrStatoDestinazione")))
							{
								ListViewItem itemTemp = listView.Items.Add("");
								itemTemp.Tag = readerTemp.GetIntEx("ID");
								itemTemp.SubItems.Add(readerTemp.GetStringEx("DescrTipoDispositivo"));
								itemTemp.SubItems.Add(readerTemp.GetStringEx("DescrStato"));
								itemTemp.SubItems.Add(readerTemp.GetIntEx("ScadenzaMinuti", 60).ToString());
								itemTemp.SubItems.Add(readerTemp.GetStringEx("DescrStatoDestinazione"));
							}
						}
					}
				}
			}
			listView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
			listView.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.HeaderSize);
			listView.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.HeaderSize);
			listView.AutoResizeColumn(4, ColumnHeaderAutoResizeStyle.HeaderSize);

			if (iIDToSelect > 0)
			{
				for (int i = 0; i < listView.Items.Count; i++)
				{
					if ((int)listView.Items[i].Tag == iIDToSelect)
					{
						listView.Items[i].Selected = true;
						listView.Select();
						break;
					}
				}
			}
		}

		private void tsb_add_Click(object sender, EventArgs e)
		{
			using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
			{
				using (OdbcCommand commTemp1 = new OdbcCommand("SELECT MIN (ID) FROM TipiDispositivi", connTemp))
				{
					object objTemp1 = commTemp1.ExecuteScalar();
					if (objTemp1 != null)
					{
						int iTemp1;
						Int32.TryParse(objTemp1.ToString(), out iTemp1);
						if (iTemp1 == 0)
						{
							MessageBox.Show(KleanTrak.Globals.strTable[214], 
								"Cleantrack", 
								MessageBoxButtons.OK, 
								MessageBoxIcon.Exclamation);
							return;
						}
						string query = $"INSERT INTO TIPIDISPOSITIVISTATOSCADENZA " +
							$"(IDTIPODISPOSITIVO, IDSTATO, SCADENZAMINUTI, IDSTATODESTINAZIONE) " +
							$"VALUES" +
							$"({iTemp1}, " +
							$"{StateTransactions.GetStateId(FixedStates.End_wash)}, " + // sono stati che ci devono sempre essere
							$"60, " +
							$"{StateTransactions.GetStateId(FixedStates.Start_cycle)})";// sono stati che ci devono sempre essere
						using (OdbcCommand commTemp2 = new OdbcCommand(query, connTemp))
						{
							if (commTemp2.ExecuteNonQuery() > 0)
							{
								m_bFatteModifiche = true;
								using (OdbcCommand commTemp3 = new OdbcCommand("SELECT MAX(ID) FROM TipiDispositiviStatoScadenza", connTemp))
								{
									object objTemp3 = commTemp3.ExecuteScalar();
									if (objTemp3 != null)
									{
										int iTemp3;
										Int32.TryParse(objTemp3.ToString(), out iTemp3);
										if (iTemp3 > 0)
										{
											RiempiLista(iTemp3);
											m_bFatteModifiche = true;
										}
									}
								}
							}
						}
					}
				}
			}

		}

		private void tsb_delete_Click(object sender, EventArgs e)
		{
			if (listView.SelectedIndices.Count == 1)
			{
				if (MessageBox.Show("Desideri davvero eliminare la scadenza selezionata?", "ATTENZIONE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
					{
						using (OdbcCommand commTemp = new OdbcCommand("UPDATE TipiDispositiviStatoScadenza SET Eliminato = 1 WHERE ID = " + ((int)listView.Items[listView.SelectedIndices[0]].Tag).ToString(), connTemp))
						{
							if (commTemp.ExecuteNonQuery() > 0)
							{
								m_bFatteModifiche = true;
								RiempiLista(0);
							}
						}
					}
				}
			}
		}

		private void tsb_close_Click(object sender, EventArgs e)
		{
			if (m_bFatteModifiche)
				MessageBox.Show("Riavvia CleanTrackServer per rendere effettive le modifiche!", "ATTENZIONE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			Close();
		}

		private void ConfigurazioneScadenzaDispositivi_ClientSizeChanged(object sender, EventArgs e) => Globals.ResizeList(this, listView);
	}
}
