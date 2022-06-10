using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KleanTrak
{
	public partial class AdditionalInfo : Form
	{
		private ListViewEx.ListViewEx listView;
		private System.Windows.Forms.ColumnHeader DATA;
		private System.Windows.Forms.ColumnHeader DESCRIZIONE;
		private System.Windows.Forms.ColumnHeader VALORE;

		public AdditionalInfo()
		{
			InitializeComponent();
		}
		private void InitializeListView()
		{
			this.listView = new ListViewEx.ListViewEx();

			this.DATA = new System.Windows.Forms.ColumnHeader();
			this.DESCRIZIONE = new System.Windows.Forms.ColumnHeader();
			this.VALORE = new System.Windows.Forms.ColumnHeader();

			// listView
			// 

			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
				 | System.Windows.Forms.AnchorStyles.Left)
				 | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.BackColor = System.Drawing.SystemColors.Window;
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.DATA,
																						this.DESCRIZIONE,
																						this.VALORE
																					  });
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(6, 6);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(450, 500);
			this.listView.TabIndex = 36;
			this.listView.View = System.Windows.Forms.View.Details;


			this.listView.HideSelection = false;
			// 
			// columnRFID
			// 
			this.DATA.Text = "Data";
			this.DATA.Width = 150;
			// 
			// columnDispositivo
			// 
			this.DESCRIZIONE.Text = "Evento";
			this.DESCRIZIONE.Width = 150;
			// 
			// ColumnSeriale
			// 
			this.VALORE.Text = "Valore Associato";
			this.VALORE.Width = 135;


			this.Controls.Add(this.listView);
		}



		public AdditionalInfo(int id)
		{
			InitializeComponent();
			InitializeListView();

			listView.SetSubItemType(0, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetSubItemType(1, ListViewEx.ListViewEx.FieldType.textbox, null);
			listView.SetSubItemType(2, ListViewEx.ListViewEx.FieldType.textbox, null);

			RiempiLista(id);

			listView.SetReadOnly(true);


		}

		private void RiempiLista(int id)
		{
			amrfidmgrex.RFIDManager manager = new amrfidmgrex.RFIDManager();
			string sep = manager.getSeparator();
			string[] sepVec = { sep };

			string addInfo = amrfidmgrex.DBUtilities.getCycleAdditionalInfoFromCycle(id, Globals.strDatabase);

			if (addInfo != null && addInfo != "")
			{
				string[] splitted = addInfo.Split(sepVec, StringSplitOptions.None);

				ListViewItem lvItem;

				foreach (string s in splitted)
				{
					string[] sepSpace = { "@@" };
					string[] splittedDetails = s.Split(sepSpace, StringSplitOptions.None);

					if (splittedDetails.Length > 0)
					{
						lvItem = listView.Items.Add(splittedDetails[0]);

						if (splittedDetails.Length > 1)
						{
							lvItem.SubItems.Add(splittedDetails[1]);
						}
						if (splittedDetails.Length > 2)
						{
							lvItem.SubItems.Add(splittedDetails[2]);
						}
						if (splittedDetails.Length > 3)
						{
							if (splittedDetails[3] == "1")
							{
								lvItem.BackColor = System.Drawing.Color.Red;
							}
						}
					}
				}

			}
		}

		private void btnStampa_Click(object sender, EventArgs e)
		{
			// listView.setLandscape(false);
			listView.PrintList("Cycle Details", "Cleantrack 5.1.0             © Tesi Elettronica e Sistemi Informativi SPA");
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			Close();
		}
	}
}
