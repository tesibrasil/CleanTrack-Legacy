using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KleanTrak.Model;

namespace KleanTrak
{
	public partial class ChooseItem : FormBase
	{
		public string Combo_label { get => this.lbl_combo.Text; set => this.lbl_combo.Text = value; }
		public ComboItem Selected_item { get => (ComboItem)cb_items.SelectedItem; }
		public ChooseItem() => InitializeComponent();
		private bool ValidateData() => cb_items.SelectedItem != null;
		private void btn_ok_Click(object sender, EventArgs e)
		{
			if (!ValidateData())
				return;
			Data_valid = true;
			Close();
		}
		private void btn_cancel_Click(object sender, EventArgs e)
		{
			Data_valid = false;
			Close();
		}
		public void SetItems(List<ComboItem> items) => cb_items.Items.AddRange(items.ToArray());
	}
}
