using System;
using System.Data.Odbc;
using System.Windows.Forms;

namespace KleanTrak
{
	public partial class CicliDispositivoDettaglio : Form
	{
		private int m_iIDCiclo = 0;

		public CicliDispositivoDettaglio(int iIDCiclo)
		{
			m_iIDCiclo = iIDCiclo;

			InitializeComponent();
		}

		private void CicliDispositivoDettaglio_Load(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;

			RiempiLista();

			Cursor.Current = Cursors.Default;

			WindowState = FormWindowState.Maximized;
			MinimumSize = Size;
		}

		private void btnStampa_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void btnChiudi_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void RiempiLista()
		{
			listView.Items.Clear();

			using (OdbcConnection connTemp = DBUtil.GetODBCConnection())
			{
				// using (OdbcCommand commTemp = new OdbcCommand("SELECT Data, Descrizione, Valore FROM CicliExt WHERE IDCICLO = " + m_iIDCiclo + " ORDER BY ID", connTemp))
				using (OdbcCommand commTemp = new OdbcCommand("SELECT Data, Descrizione, Valore FROM CicliExt WHERE IDCICLO = " + m_iIDCiclo + " ORDER BY DATA", connTemp))
				{
					using (OdbcDataReader readTemp = commTemp.ExecuteReader())
					{
						while (readTemp.Read())
						{
							ListViewItem lviTemp = listView.Items.Add("");

							if (readTemp.IsDBNull(readTemp.GetOrdinal("Data")))
								lviTemp.SubItems.Add("");
							else
								lviTemp.SubItems.Add(Globals.ConvertDateTime(readTemp.GetString(readTemp.GetOrdinal("Data"))));

							if (readTemp.IsDBNull(readTemp.GetOrdinal("Descrizione")))
								lviTemp.SubItems.Add("");
							else
								lviTemp.SubItems.Add(readTemp.GetString(readTemp.GetOrdinal("Descrizione")));

							if (readTemp.IsDBNull(readTemp.GetOrdinal("Valore")))
								lviTemp.SubItems.Add("");
							else
								lviTemp.SubItems.Add(readTemp.GetString(readTemp.GetOrdinal("Valore")));
						}
					}
				}
			}

			listView.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.HeaderSize);
			listView.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.HeaderSize);
			listView.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.HeaderSize);
		}
	}
}
