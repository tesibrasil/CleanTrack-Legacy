using System;
using System.Collections;
using System.Data.Odbc;
using System.Windows.Forms;
using System.Linq;
using OdbcExtensions;

namespace KleanTrak
{
	public partial class ConfigurazioneTransazioni : Form
	{
		private bool m_bFatteModifiche = false;

		private class ItemStato
		{
			public int Id;
			public string Name;
			public override string ToString() { return Name; }
		}

		public ConfigurazioneTransazioni()
		{
			InitializeComponent();
			listView.ListViewItemSorter = new ListViewEx.ListViewExComparer(listView);
			listView.SetSubItemType(1, ListViewEx.ListViewEx.FieldType.combo, ComboStato());
			listView.SetSubItemType(2, ListViewEx.ListViewEx.FieldType.combo, ComboStato());
			listView.SetEndEditCallback(null, new ListViewEx.ListViewEx.EndEditingCallbackCombo(EndCombo), null, null);
			tsdelete.Visible = false;
			tsadd.ToolTipText = Globals.strTable[173];
			tsdelete.ToolTipText = Globals.strTable[172];
			tsclose.ToolTipText = Globals.strTable[155];
		}
		private void ConfigurazioneTransazioni_Load(object sender, EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				RiempiLista(0);
				Cursor.Current = Cursors.Default;
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
		private void listView_ColumnClick(object sender, ColumnClickEventArgs e) =>
			((ListViewEx.ListViewExComparer)listView.ListViewItemSorter).Column = e.Column;
		private void listView_SelectedIndexChanged(object sender, EventArgs e) => 
			tsdelete.Visible = (listView.SelectedIndices.Count == 1);
		private bool EndCombo(int iItemNum, int iSubitemNum, object obj)
		{
			OdbcCommand cmd = null;
			try
			{
				string sColumn = "";
				switch (iSubitemNum)
				{
					case 1:
						sColumn = "IDSTATOOLD";
						break;
					case 2:
						sColumn = "IDSTATONEW";
						break;
				}
				if (sColumn.Length == 0)
					return false;
				int id = (int)listView.Items[iItemNum].Tag;
				if (id <= 0)
					return false;
				string query = $"UPDATE STATOCAMBIO SET {sColumn} = {((ItemStato)obj).Id.ToString()} " +
					$"WHERE ID = {id} AND IDSEDE = {Globals.IDSEDE}";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				if (cmd.ExecuteNonQuery() > 0)
					m_bFatteModifiche = true;
				return true;
			}
			catch (Exception e)
			{
				Globals.WarnAndLog(e);
				return false;
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}
		private ArrayList ComboStato()
		{
			ArrayList listStati = new ArrayList();
			OdbcCommand cmd = null;
			OdbcDataReader rdr = null;
			try
			{
				string query = "SELECT ID, DESCRIZIONE FROM STATO " +
					"WHERE ELIMINATO = 0 ORDER BY DESCRIZIONE";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					listStati.Add(new ItemStato
					{
						Id = rdr.GetIntEx("ID"),
						Name = rdr.GetStringEx("DESCRIZIONE")
					});
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
			}
			return listStati;
		}
		private void RiempiLista(int iIDToSelect)
		{
			listView.Items.Clear();
			tsdelete.Visible = false;
			OdbcCommand cmd = null;
			OdbcDataReader rdr = null;
			try
			{
				string query = $"SELECT STATOCAMBIO.ID, IDSTATOOLD, " +
					$"S1.DESCRIZIONE AS DESCRSTATOOLD, IDSTATONEW, S2.DESCRIZIONE AS DESCRSTATONEW " +
					$"FROM STATOCAMBIO INNER JOIN STATO S1 ON STATOCAMBIO.IDSTATOOLD = S1.ID " +
					$"INNER JOIN STATO S2 ON STATOCAMBIO.IDSTATONEW = S2.ID " +
					$"WHERE STATOCAMBIO.ELIMINATO = 0 AND STATOCAMBIO.IDSEDE = {Globals.IDSEDE}" +
					$"ORDER BY S1.DESCRIZIONE, S2.DESCRIZIONE";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				rdr = cmd.ExecuteReader();
				while (rdr.Read())
				{
					if (rdr.IsDBNull(rdr.GetOrdinal("DESCRSTATOOLD")) ||
						rdr.IsDBNull(rdr.GetOrdinal("DESCRSTATONEW")))
						continue;
					ListViewItem itemTemp = listView.Items.Add("");
					itemTemp.Tag = rdr.GetIntEx("ID");
					itemTemp.SubItems.Add(rdr.GetStringEx("DESCRSTATOOLD"));
					itemTemp.SubItems.Add(rdr.GetString(rdr.GetOrdinal("DESCRSTATONEW")));
				}
				tsdelete.Visible = true;
				if (iIDToSelect <= 0)
					return;
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
			catch (Exception e)
			{
				throw;
			}
			finally
			{
				if (rdr != null && !rdr.IsClosed)
					rdr.Close();
				if (cmd != null)
					cmd.Dispose();
			}
		}
		private void ConfigurazioneTransazioni_ClientSizeChanged(object sender, EventArgs e) => Globals.ResizeList(this, listView);
		private void tsdelete_Click(object sender, EventArgs e)
		{
			OdbcCommand cmd = null;
			try
			{
				if (MessageBox.Show(Globals.strTable[167],
					Globals.strTable[168],
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question) == DialogResult.No)
					return;
				string query = $"UPDATE STATOCAMBIO SET ELIMINATO = 1 " +
					$"WHERE ID = {(int)listView.Items[listView.SelectedIndices[0]].Tag}";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				if (cmd.ExecuteNonQuery() == 0)
				{
					MessageBox.Show(Globals.strTable[169],
						"Clean Track",
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
					return;
				}
				m_bFatteModifiche = true;
				RiempiLista(0);
			}
			catch (Exception eX)
			{
				Globals.WarnAndLog(eX);
			}
			finally
			{
				if (cmd != null)
					cmd.Dispose();
			}
		}
		private void tsadd_Click(object sender, EventArgs e)
		{
			OdbcCommand cmd = null;
			try
			{
				string query = $"SELECT MIN(ID) FROM STATO";
				cmd = new OdbcCommand(query, DBUtil.GetODBCConnection());
				int min_stato_id = cmd.ExecuteScalarInt();
				if (min_stato_id < 0)
					throw new ApplicationException("table stato is empty");
				query = $"INSERT INTO STATOCAMBIO " +
					$"(IDSTATOOLD, IDSTATONEW, IDSEDE) " +
					$"VALUES ({min_stato_id}, {min_stato_id}, {Globals.IDSEDE})";
				cmd.CommandText = query;
				if (cmd.ExecuteNonQuery() == 0)
				{
					MessageBox.Show(Globals.strTable[171], 
						Globals.strTable[168], 
						MessageBoxButtons.OK, 
						MessageBoxIcon.Error);
					return;
				}
				cmd.CommandText = $"SELECT MAX(ID) FROM STATOCAMBIO WHERE IDSEDE = {Globals.IDSEDE}";
				int max_id = cmd.ExecuteScalarInt();
				if(max_id == -1)
					throw new ApplicationException("inserimento fallito");
				RiempiLista(max_id);
				m_bFatteModifiche = true;
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
		private void tsclose_Click(object sender, EventArgs e)
		{
			if (m_bFatteModifiche)
				MessageBox.Show(Globals.strTable[170],
					Globals.strTable[168],
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
			Close();
		}
	}
}
