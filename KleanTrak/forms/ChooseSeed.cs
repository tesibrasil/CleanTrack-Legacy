using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KleanTrak
{
	public partial class ChooseSeed : Form
	{
		public bool DataValid { get; private set; } = false;
		public int Idseed { get; private set; } = Globals.IDSEDE;
		public ChooseSeed()
		{
			InitializeComponent();
			dgv_seeds.DataSource = DBUtil.GetSeeds();
		}

		private void ChooseSeed_Load(object sender, EventArgs e)
		{
			try
			{
				Globals.ResizeGrid(this, dgv_seeds);
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
			}
		}

		private void dgv_seeds_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			try
			{
				Idseed = (int)dgv_seeds.SelectedRows[0].Cells[1].Value;
				DataValid = true;
				Close();
			}
			catch (Exception ex)
			{
				Globals.WarnAndLog(ex);
				throw;
			}
		}

		private void ChooseSeed_ClientSizeChanged(object sender, EventArgs e) => Globals.ResizeGrid(this, dgv_seeds);
	}
}
