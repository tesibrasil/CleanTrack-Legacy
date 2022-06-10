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
	public partial class InsertText : Form
	{
		public bool DataValid { get; private set; } = false;
		public InsertText()
		{
			InitializeComponent();
		}
		public string Label 
		{ 
			set 
			{
				this.lbltext.Text = value;
			} 
		}
		public string UserText { get => this.tbtext.Text; }

		private void btn_cancel_Click(object sender, EventArgs e)
		{
			DataValid = false;
			Close();
		}

		private void btn_ok_Click(object sender, EventArgs e)
		{
			if (tbtext.TextLength == 0)
			{
				DataValid = false;
				MessageBox.Show("Inserire un valore", 
					"Valore Mancante",
					MessageBoxButtons.OK,
					MessageBoxIcon.Stop);
				return;
			}
			DataValid = true;
			Close();
		}
	}
}
