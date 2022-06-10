using System;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Globalization;
using System.Runtime.InteropServices;

namespace ListViewEx
{
	public class DrawHeaderEventArgs : EventArgs
	{
		Graphics graphics;
		Rectangle bounds;
		int height;
		public DrawHeaderEventArgs(Graphics dc, Rectangle rect, int h)
		{
			graphics = dc;
			bounds = rect;
			height = h;
		}
		public Graphics Graphics
		{
			get { return graphics; }
		}
		public Rectangle Bounds
		{
			get { return bounds; }
		}
		public int HeaderHeight
		{
			get { return height; }
		}
	}
	
	/// <summary>
	/// Summary description for ListViewEx.
	/// </summary>
	public class ListViewEx : System.Windows.Forms.ListView
	{
		public class MyFormat
		{
			static public NumberFormatInfo m_format = (NumberFormatInfo)System.Globalization.NumberFormatInfo.CurrentInfo.Clone();

			static public void Update()
			{
				m_format.CurrencyNegativePattern = 8;
				m_format.CurrencyPositivePattern = 3;
			}
		}

		private class SubItemType
		{
			public int iItem;
			public int iSubItem;
			public FieldType type;
			public ArrayList items = null;
		}

		public enum FieldType
		{
			none,
			textbox,
			textbox_number,
			textbox_currency,
			textbox_percentual,
			textbox_date,
			combo,
			check
		}

		public delegate void RefreshListEventHandler(object sender, EventArgs e);
		public event RefreshListEventHandler RefreshEvent;
		public delegate bool EndEditingCallbackEdit(int iItemNum, int iSubitemNum, string strText);
		public delegate bool EndEditingCallbackDate(int iItemNum, int iSubitemNum, DateTime date);
		public delegate bool EndEditingCallbackCombo(int iItemNum, int iSubitemNum, object objSelected);
		public delegate bool EndEditingCallbackCheck(int iItemNum, int iSubitemNum, bool bChecked);

		private ArrayList m_listType = new ArrayList();
		private bool m_bEditDestroyHandled = false;
		private bool m_bReadOnly = false;

		private int m_iEditItem = -1;
		private int m_iEditSubItem = -1;

		private TextBox m_Edit = null;
		private ComboBox m_Combo = null;
		private DateTimePicker m_Date = null;

		private EndEditingCallbackEdit m_CallbackFunctionEdit = null;
		private EndEditingCallbackDate m_CallbackFunctionDate = null;
		private EndEditingCallbackCombo m_CallbackFunctionCombo = null;
		private EndEditingCallbackCheck m_CallbackFunctionCheck = null;

		private System.Drawing.Printing.PrintDocument printGraph;
		private string m_strHeader, m_strFooter;
		private int m_iCurPages = 0;
		private HeaderControl Header;

		public ListViewEx()
		{
			this.DoubleClick += new EventHandler(ListViewEx_Click);
			this.printGraph = new System.Drawing.Printing.PrintDocument();
			this.printGraph.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printList_PrintPage);
		}

		public void PrintList(string strHeader, string strFooter)
		{
			PrintDialog prDiag = new PrintDialog();
			DialogResult rs = prDiag.ShowDialog();
			if (rs == DialogResult.OK)
			{
				printGraph.PrinterSettings = prDiag.PrinterSettings;
				printGraph.DefaultPageSettings.Landscape = true;
				m_iCurPages = 0;
				m_strHeader = strHeader;
				m_strFooter = strFooter;
				printGraph.Print();
			}
		}

		public void setLandscape(bool val)
		{
			printGraph.DefaultPageSettings.Landscape = val;
		}

		public void checkItem(int index, int sub) => Items[index].SubItems[sub].Tag = "X";

		public bool isChecked(int index, int sub) => (string)Items[index].SubItems[sub].Tag == "X";

		private void printList_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			const int iRowForPages = 30;

			Rectangle rectDraw = new Rectangle();
			rectDraw.X = 0;
			rectDraw.Y = 0;
			rectDraw.Width = (int)e.Graphics.VisibleClipBounds.Width;
			rectDraw.Height = (int)e.Graphics.VisibleClipBounds.Height;

			Rectangle rectTable = new Rectangle();
			rectTable = rectDraw;

			// Serve per calcolare lo spazio effettivo per la stampa della tabella (cioè escuso header e footer)
			int iPageCells = iRowForPages;
			if (m_strHeader != "")
				iPageCells++;
			if (m_strFooter != "")
				iPageCells++;

			float fHFSize = (float)rectDraw.Height / (float)iPageCells;
			if (m_strHeader != "")
			{
				rectTable.Y += (int)fHFSize * 2;
				rectTable.Height -= (int)fHFSize * 2;
			}
			// Footer c'è sempre (numero di pagina)
			rectTable.Height -= (int)fHFSize * 2;

			Font font = new Font("Tahoma", 10);
			Font fontBold = new Font("Tahoma", 12, FontStyle.Bold);
			Brush brush = new SolidBrush(Color.Black);
			RectangleF rect = new RectangleF();
			StringFormat format = new StringFormat();
			format.Alignment = StringAlignment.Center;

			rect.X = rectDraw.X;
			rect.Y = rectDraw.Y;
			rect.Width = rectDraw.Width;
			rect.Height = fHFSize;

			e.Graphics.DrawString(m_strHeader,
								  fontBold,
								  brush,
								  rect,
								  format);

			rect.Y = rectDraw.Bottom - (int)fHFSize;
			format.LineAlignment = StringAlignment.Far;
			e.Graphics.DrawString(m_strFooter,
								  font,
								  brush,
								  rect,
								  format);

			int iPagesTot = 1;
			while (iPagesTot * iRowForPages < Items.Count)
				iPagesTot++;

			format.Alignment = StringAlignment.Far;
			e.Graphics.DrawString("Pagina " + (m_iCurPages + 1).ToString() + "/" + iPagesTot.ToString(),
				font,
				brush,
				rect,
				format);

			//
			int iTotRowsToPrint = Math.Min(iRowForPages, Items.Count - m_iCurPages * iRowForPages);
			for (int i = -1; i < iTotRowsToPrint; i++)
				listView_PrintRow(i, iRowForPages + 1, m_iCurPages * iRowForPages, e.Graphics, rectTable);

			m_iCurPages++;
			e.HasMorePages = (m_iCurPages < iPagesTot);
		}

		private void listView_PrintRow(int iRow, int iTotRows, int iRowOffset, Graphics dc, Rectangle rectDraw)
		{
			const string strFontFamily = "Tahoma";
			const int iFontSize = 10;

			Pen pen = new Pen(Color.Black);
			Brush brush = new SolidBrush(Color.Black);

			int iPosX = rectDraw.Left;

			int iColumnsWidth = 0;
			for (int i = 0; i < Columns.Count; i++)
				iColumnsWidth += Columns[i].Width;

			float fRapp = (float)rectDraw.Width / (float)iColumnsWidth;
			for (int x = 0; x <= Columns.Count; x++)
			{
				if (x < Columns.Count && Columns[x].Width == 0)
					continue;

				dc.DrawLine(pen,
					iPosX,
					rectDraw.Y + (float)(iRow + 1) * ((float)rectDraw.Height / (float)iTotRows),
					iPosX,
					rectDraw.Y + (float)(iRow + 2) * ((float)rectDraw.Height / (float)iTotRows));

				if (x < Columns.Count)
				{
					RectangleF rect = new RectangleF();
					rect.X = iPosX;
					rect.Y = rectDraw.Y + (float)(iRow + 1) * ((float)rectDraw.Height / (float)iTotRows);
					rect.Width = (int)((float)Columns[x].Width * fRapp);
					rect.Height = ((float)rectDraw.Height / (float)iTotRows);

					StringFormat format = new StringFormat();
					format.LineAlignment = StringAlignment.Center;
					format.FormatFlags = StringFormatFlags.LineLimit;
					switch (Columns[x].TextAlign)
					{
						case HorizontalAlignment.Left:
							format.Alignment = StringAlignment.Near;
							break;

						case HorizontalAlignment.Center:
							format.Alignment = StringAlignment.Center;
							break;

						case HorizontalAlignment.Right:
							format.Alignment = StringAlignment.Far;
							break;
					}

					string strText = "";
					Font font = new Font(strFontFamily, iFontSize);
					if (iRow == -1)
					{
						strText = Columns[x].Text;
						font = new Font(strFontFamily, iFontSize, FontStyle.Bold | FontStyle.Italic);
					}
					else if (Items.Count > (iRow + iRowOffset) && Items[iRow + iRowOffset].SubItems.Count > x)
					{
						if (isChecked(iRow + iRowOffset, x))
						{
							strText = "X";
						}
						else
						{
							strText = Items[iRow + iRowOffset].SubItems[x].Text;
							Font itemFont = Items[iRow + iRowOffset].SubItems[x].Font;
							FontStyle fontStyle = FontStyle.Regular;
							if (itemFont.Bold)
								fontStyle |= FontStyle.Bold;
							font = new Font(strFontFamily, iFontSize, fontStyle);
						}
					}

					dc.DrawString(strText,
						font,
						brush,
						rect,
						format);
					iPosX += (int)((float)Columns[x].Width * fRapp);
				}
			}

			dc.DrawLine(pen,
				rectDraw.X,
				rectDraw.Y + (float)(iRow + 1) * ((float)rectDraw.Height / (float)iTotRows),
				iPosX,
				rectDraw.Y + (float)(iRow + 1) * ((float)rectDraw.Height / (float)iTotRows));

			dc.DrawLine(pen,
				rectDraw.X,
				rectDraw.Y + (float)(iRow + 2) * ((float)rectDraw.Height / (float)iTotRows),
				iPosX,
				rectDraw.Y + (float)(iRow + 2) * ((float)rectDraw.Height / (float)iTotRows));
		}

		public void SetReadOnly(bool bReadOnly) { m_bReadOnly = bReadOnly; }

		private void ListViewEx_Click(object sender, EventArgs e)
		{
			if (m_bReadOnly)
				return;
			GetCursorPos(ref m_iEditItem, ref m_iEditSubItem);
			CreateMyControl();
		}

		private void CreateMyControl()
		{
			if (m_iEditItem < 0 || m_iEditSubItem < 0 || m_Edit != null || m_Combo != null)
			{
				m_bEditDestroyHandled = false;
				return;
			}
			int iPosX = Items[m_iEditItem].Bounds.X;
			for (int i = 0; i < m_iEditSubItem; i++)
				iPosX += Columns[i].Width;
			FieldType fieldtype = FieldType.none;
			ArrayList items = null;
			for (int i = 0; i < m_listType.Count; i++)
			{
				SubItemType item = (SubItemType)m_listType[i];
				if (item.iSubItem == m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem))
				{
					fieldtype = item.type;
					items = item.items;
					break;
				}
			}
			switch (fieldtype)
			{
				case FieldType.textbox_date:
					m_Date = new DateTimePicker();
					m_Date.Parent = this;
					m_Date.Location = new Point(iPosX, Items[m_iEditItem].Bounds.Top - 3);
					m_Date.Size = new Size(Columns[m_iEditSubItem].Width + 1, Items[m_iEditItem].Bounds.Height);
					// m_Edit.KeyDown += new KeyEventHandler(this.textboxEdit_KeyDown);
					m_Date.Leave += new EventHandler(Date_Leave);
					m_Date.CloseUp += new EventHandler(Date_CloseUp);
					m_Date.Format = DateTimePickerFormat.Short;
					m_Date.Show();
					if (Items[m_iEditItem].SubItems[m_iEditSubItem].Text.Length == 10)
					{
						string str = Items[m_iEditItem].SubItems[m_iEditSubItem].Text;
						DateTime date = new DateTime(System.Convert.ToInt32(str.Substring(6, 4), 10),
							System.Convert.ToInt32(str.Substring(3, 2), 10),
							System.Convert.ToInt32(str.Substring(0, 2), 10));
						m_Date.Value = date;
					}
					m_Date.Focus();
					break;
				case FieldType.textbox:
				case FieldType.textbox_number:
				case FieldType.textbox_currency:
				case FieldType.textbox_percentual:
					m_Edit = new TextBox();
					m_Edit.Parent = this;
					m_Edit.Location = new Point(iPosX, Items[m_iEditItem].Bounds.Top - 3);
					m_Edit.Size = new Size(Columns[m_iEditSubItem].Width + 1, Items[m_iEditItem].Bounds.Height);
					m_Edit.KeyDown += new KeyEventHandler(this.textboxEdit_KeyDown);
					m_Edit.Leave += new EventHandler(this.textboxEdit_Leave);
					m_Edit.Show();

					if (fieldtype == FieldType.textbox_currency)
					{
						if (Items[m_iEditItem].SubItems[m_iEditSubItem].Text.Length > 2)
							m_Edit.Text = Items[m_iEditItem].SubItems[m_iEditSubItem].Text.Substring(0, Items[m_iEditItem].SubItems[m_iEditSubItem].Text.Length - 2).Replace(".", "");
						else
							m_Edit.Text = "0";
					}
					else
					{
						if (fieldtype == FieldType.textbox_percentual)
						{
							if (Items[m_iEditItem].SubItems[m_iEditSubItem].Text.Length > 0)
								m_Edit.Text = Items[m_iEditItem].SubItems[m_iEditSubItem].Text.Substring(0, Items[m_iEditItem].SubItems[m_iEditSubItem].Text.Length - 1);
							else
								m_Edit.Text = "0";
						}
						else
						{
							m_Edit.Text = Items[m_iEditItem].SubItems[m_iEditSubItem].Text;
						}
					}

					if ((fieldtype == FieldType.textbox_number) || (fieldtype == FieldType.textbox_percentual))
					{
						int iStyles = Win32.GetWindowLong(m_Edit.Handle, -16);     // GWL_STYLE
						Win32.SetWindowLong(m_Edit.Handle, -16, iStyles | 0x2000); // ES_NUMBER
						m_Edit.MaxLength = 7;
					}
					m_Edit.Focus();
					break;

				case FieldType.combo:
					m_Combo = new ComboBox();
					m_Combo.Parent = this;
					m_Combo.Location = new Point(iPosX, Items[m_iEditItem].Bounds.Top - 3);
					m_Combo.Size = new Size(Columns[m_iEditSubItem].Width + 1, Items[m_iEditItem].Bounds.Height);
					m_Combo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBox_KeyDown);
					m_Combo.SelectionChangeCommitted += new EventHandler(Combo_SelectionChangeCommitted);
					m_Combo.Leave += new EventHandler(Combo_Leave);
					m_Combo.DropDownStyle = ComboBoxStyle.DropDownList;

					if (items != null)
						for (int i = 0; i < items.Count; i++)
							m_Combo.Items.Add(items[i]);

					m_Combo.Show();
					m_Combo.Text = Items[m_iEditItem].SubItems[m_iEditSubItem].Text;
					m_Combo.Focus();

					m_Combo.DroppedDown = true;

					break;

				case FieldType.check:
					Items[m_iEditItem].SubItems[m_iEditSubItem].Text = (Items[m_iEditItem].SubItems[m_iEditSubItem].Text == "Si") ? "No" : "Si";
					if (m_CallbackFunctionCheck == null || m_CallbackFunctionCheck(m_iEditItem, m_iEditSubItem, Items[m_iEditItem].SubItems[m_iEditSubItem].Text == "Si"))
						Invalidate();
					else
						Items[m_iEditItem].SubItems[m_iEditSubItem].Text = (Items[m_iEditItem].SubItems[m_iEditSubItem].Text == "Si") ? "No" : "Si";
					break;

				default:
					break;
			}
			m_bEditDestroyHandled = false;
		}

		private void textboxEdit_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (!e.Control)
				return;
			DestroyEdit();
			int nFind = 0;
			switch (e.KeyCode)
			{
				case Keys.Up:
					for (int i = 0; i < m_listType.Count; i++)
					{
						SubItemType item = (SubItemType)m_listType[i];
						if (item.iSubItem == m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem - 1) && m_iEditItem > 0)
						{
							Items[m_iEditItem].Selected = false;
							m_iEditItem--;
							Items[m_iEditItem].Selected = true;
							Items[m_iEditItem].Focused = true;
							EnsureVisible(m_iEditItem);
							CreateMyControl();
							break;
						}
					}
					break;
				case Keys.Down:
					for (int i = 0; i < m_listType.Count; i++)
					{
						SubItemType item = (SubItemType)m_listType[i];
						if (item.iSubItem == m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem + 1) && m_iEditItem < Items.Count - 1)
						{
							Items[m_iEditItem].Selected = false;
							m_iEditItem++;
							Items[m_iEditItem].Selected = true;
							Items[m_iEditItem].Focused = true;
							EnsureVisible(m_iEditItem);
							CreateMyControl();
							break;
						}
					}
					break;
				case Keys.Left:
					nFind = -1;
					for (int i = 0; i < m_listType.Count; i++)
					{
						SubItemType item = (SubItemType)m_listType[i];
						if (item.iSubItem < m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem))
							nFind = Math.Max(nFind, item.iSubItem);
					}
					if (nFind >= 0)
					{
						m_iEditSubItem = nFind;
						SelectedItems.Clear();
						Items[m_iEditItem].Selected = true;
						CreateMyControl();
					}
					break;
				case Keys.Right:
					nFind = 1000000;
					for (int i = 0; i < m_listType.Count; i++)
					{
						SubItemType item = (SubItemType)m_listType[i];
						if (item.iSubItem > m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem))
							nFind = Math.Min(nFind, item.iSubItem);
					}
					if (nFind < 1000000)
					{
						m_iEditSubItem = nFind;
						SelectedItems.Clear();
						Items[m_iEditItem].Selected = true;
						CreateMyControl();
					}
					break;
				case Keys.Enter:
				case Keys.Escape:
				default:
					break;
			}
		}

		private void comboBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (!e.Control)
				return;
			DestroyCombo();
			int nFind = -1;
			switch (e.KeyCode)
			{
				case Keys.Escape:
					DestroyCombo();
					break;
				case Keys.Enter:
					for (int i = 0; i < m_listType.Count; i++)
					{
						SubItemType item = (SubItemType)m_listType[i];
						if (item.iSubItem == m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem + 1) && m_iEditItem < Items.Count - 1)
						{
							m_iEditItem++;
							SelectedItems.Clear();
							Items[m_iEditItem].Selected = true;
							CreateMyControl();
							break;
						}
					}
					break;
				case Keys.Up:
					for (int i = 0; i < m_listType.Count; i++)
					{
						SubItemType item = (SubItemType)m_listType[i];
						if (item.iSubItem == m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem - 1))
						{
							m_iEditItem--;
							SelectedItems.Clear();
							Items[m_iEditItem].Selected = true;
							CreateMyControl();
							break;
						}
					}
					break;
				case Keys.Down:
					for (int i = 0; i < m_listType.Count; i++)
					{
						SubItemType item = (SubItemType)m_listType[i];
						if (item.iSubItem == m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem + 1))
						{
							m_iEditItem++;
							SelectedItems.Clear();
							Items[m_iEditItem].Selected = true;
							CreateMyControl();
							break;
						}
					}
					break;
				case Keys.Left:
					for (int i = 0; i < m_listType.Count; i++)
					{
						SubItemType item = (SubItemType)m_listType[i];
						if (item.iSubItem < m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem))
							nFind = Math.Max(nFind, item.iSubItem);
					}

					if (nFind >= 0)
					{
						m_iEditSubItem = nFind;
						SelectedItems.Clear();
						Items[m_iEditItem].Selected = true;
						CreateMyControl();
					}
					break;
				case Keys.Right:
					nFind = 1000000;
					for (int i = 0; i < m_listType.Count; i++)
					{
						SubItemType item = (SubItemType)m_listType[i];
						if (item.iSubItem > m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem))
							nFind = Math.Min(nFind, item.iSubItem);
					}
					if (nFind < 1000000)
					{
						m_iEditSubItem = nFind;
						SelectedItems.Clear();
						Items[m_iEditItem].Selected = true;
						CreateMyControl();
					}
					break;
			}
		}

		private void textboxEdit_Leave(object sender, System.EventArgs e)
		{
			DestroyEdit();
		}

		private void Combo_SelectionChangeCommitted(object sender, EventArgs e)
		{
			DestroyCombo();
		}

		private void Combo_Leave(object sender, EventArgs e)
		{
			DestroyCombo();
		}

		private void Date_Leave(object sender, EventArgs e)
		{
			DestroyDate();
		}

		private void Date_CloseUp(object sender, EventArgs e)
		{
			DestroyDate();
		}

		private void DestroyEdit()
		{
			if (m_Edit == null || m_bEditDestroyHandled == true)
				return;

			if (m_CallbackFunctionEdit == null || m_CallbackFunctionEdit(m_iEditItem, m_iEditSubItem, m_Edit.Text) == true)
			{
				FieldType fieldtype = FieldType.none;
				for (int i = 0; i < m_listType.Count; i++)
				{
					SubItemType item = (SubItemType)m_listType[i];
					if (item.iSubItem == m_iEditSubItem && (item.iItem == -1 || item.iItem == m_iEditItem))
					{
						fieldtype = item.type;
						// items = item.items;
						break;
					}
				}

				if (fieldtype == FieldType.textbox_currency)
				{
					float fValue;
					try
					{
						fValue = Convert.ToSingle(m_Edit.Text.Replace('.', ','));
					}
					catch (InvalidCastException e1)
					{
						if (m_Edit.Text.Length > 0)
							MessageBox.Show(e1.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						fValue = 0;
					}
					catch (FormatException e2)
					{
						if (m_Edit.Text.Length > 0)
							MessageBox.Show(e2.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						fValue = 0;
					}

					Items[m_iEditItem].SubItems[m_iEditSubItem].Text = fValue.ToString("C", MyFormat.m_format);
				}
				else
				{
					if (fieldtype == FieldType.textbox_percentual)
					{
						int iValue;
						try
						{
							iValue = Convert.ToInt32(m_Edit.Text);
						}
						catch (InvalidCastException e1)
						{
							if (m_Edit.Text.Length > 0)
								MessageBox.Show(e1.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							iValue = 0;
						}
						catch (FormatException e2)
						{
							if (m_Edit.Text.Length > 0)
								MessageBox.Show(e2.Message, "Clean Track", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							iValue = 0;
						}

						Items[m_iEditItem].SubItems[m_iEditSubItem].Text = iValue.ToString() + "%";
					}
					else
					{
						Items[m_iEditItem].SubItems[m_iEditSubItem].Text = m_Edit.Text;
					}
				}
			}

			m_bEditDestroyHandled = true;
			m_Edit.Dispose();
			m_Edit = null;
			if (this.RefreshEvent != null)
			{
				RefreshEvent(this, EventArgs.Empty);
			}

		}

		private void DestroyCombo()
		{
			if (m_Combo == null || m_bEditDestroyHandled == true)
				return;

			if (m_CallbackFunctionCombo == null || m_CallbackFunctionCombo(m_iEditItem, m_iEditSubItem, m_Combo.SelectedItem) == true)
				Items[m_iEditItem].SubItems[m_iEditSubItem].Text = m_Combo.Text;

			m_bEditDestroyHandled = true;
			m_Combo.Dispose();
			m_Combo = null;
		}

		private void DestroyDate()
		{
			if (m_Date == null || m_bEditDestroyHandled == true)
				return;

			if (m_CallbackFunctionDate == null || m_CallbackFunctionDate(m_iEditItem, m_iEditSubItem, m_Date.Value) == true)
				Items[m_iEditItem].SubItems[m_iEditSubItem].Text = String.Format("{0:d2}/{1:d2}/{2:d2}",
					m_Date.Value.Day,
					m_Date.Value.Month,
					m_Date.Value.Year).ToString();

			m_bEditDestroyHandled = true;
			m_Date.Dispose();
			m_Date = null;
		}

		public void ResetSubItemType()
		{
			m_listType.Clear();
		}

		public void SetSubItemType(int iItem, int iSubItem, FieldType type, ArrayList items)
		{
			for (int i = 0; i < m_listType.Count; i++)
			{
				SubItemType item = (SubItemType)m_listType[i];
				if (item.iSubItem == iSubItem && item.iItem == iItem)
				{
					m_listType.RemoveAt(i);
					break;
				}
			}

			SubItemType itemNew = new SubItemType();
			itemNew.iSubItem = iSubItem;
			itemNew.iItem = iItem;
			itemNew.type = type;
			itemNew.items = items;
			m_listType.Add(itemNew);
		}

		public void SetSubItemType(int iSubItem, FieldType type, ArrayList items)
		{
			SetSubItemType(-1, iSubItem, type, items);
		}

		public void SetEndEditCallback(EndEditingCallbackEdit callbackEdit,
			EndEditingCallbackCombo callbackCombo,
			EndEditingCallbackDate callbackDate,
			EndEditingCallbackCheck callbackCheck)
		{
			m_CallbackFunctionCombo = callbackCombo;
			m_CallbackFunctionEdit = callbackEdit;
			m_CallbackFunctionDate = callbackDate;
			m_CallbackFunctionCheck = callbackCheck;
		}

		public void GetCursorPos(ref int iItem, ref int iSubItem)
		{
			iItem = iSubItem = -1;
			Point point = PointToClient(Cursor.Position);

			for (int i = 0; i < Items.Count; i++)
			{
				if (Items[i].Bounds.Contains(point))
				{
					iItem = i;
					int iSize = Items[m_iEditItem].Bounds.X;
					for (int j = 0; j < Columns.Count; j++)
					{
						int iSizePrec = iSize;
						iSize += Columns[j].Width;
						if (point.X > iSizePrec && point.X < iSize)
							iSubItem = j;
					}
				}
			}
		}

		protected void DrawCheck(int iItem, int iSubItem, Graphics g)
		{

			int iPosX = 0;
			for (int j = 0; j < iSubItem; j++)
				iPosX += Columns[j].Width;

			Rectangle rect = new Rectangle(iPosX,
				Items[iItem].Bounds.Top,
				Columns[iSubItem].Width,
				Items[iItem].Bounds.Height);

			Win32.RECT chkboxrect = new Win32.RECT();
			chkboxrect.left = rect.X + (rect.Width - rect.Height) / 2;
			chkboxrect.top = rect.Y;
			chkboxrect.right = chkboxrect.left + rect.Height - 1;
			chkboxrect.bottom = chkboxrect.top + rect.Height - 1;

			// fill rect around checkbox with white
			bool bItemSelected = false;
			for (int i = 0; i < SelectedItems.Count; i++)
				if (SelectedItems[i].Index == iItem)
					bItemSelected = true;

			Color crWindow = bItemSelected ? (this.Focused ? Color.FromKnownColor(KnownColor.MediumBlue) : Color.FromKnownColor(KnownColor.Control)) : Color.FromKnownColor(KnownColor.Window);
			Color crGrayText = Color.FromArgb(192, 192, 192);
			Brush brWindow = new SolidBrush(crWindow);
			rect.Inflate(-1, -1);
			g.FillRectangle(brWindow, rect);
			Pen penGray = new Pen(crGrayText, 1);
			Brush brWhite = new SolidBrush(Color.White);
			g.FillRectangle(brWhite, chkboxrect.left + 1, chkboxrect.top + 1, chkboxrect.right - chkboxrect.left - 2, chkboxrect.bottom - chkboxrect.top - 2);
			Pen pen = new Pen(Color.Black, 1);
			if (Items[iItem].SubItems[iSubItem].Text == "Si")
			{
				int x = chkboxrect.left + 9;

				int y = chkboxrect.top + 3;
				int i;

				for (i = 0; i < 4; i++)
				{
					g.DrawLine(pen, x, y, x, y + 3);
					x--;
					y++;
				}
				for (i = 0; i < 3; i++)
				{
					g.DrawLine(pen, x, y, x, y + 3);
					x--;
					y--;
				}
			}
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			//Create a new HeaderControl object
			Header = new HeaderControl(this);
			base.OnHandleCreated(e);
		}

		internal class HeaderControl : NativeWindow
		{
			private ListViewEx m_Parent;
			private Color bkColor;
			private bool mouseDown = false;

			public HeaderControl(ListViewEx parent)
			{
				//Get the header control handle
				IntPtr handle = (IntPtr)Win32.SendMessage(parent.Handle, (0x1000 + 31), IntPtr.Zero, IntPtr.Zero);
				this.AssignHandle(handle);
				m_Parent = parent;

				bkColor = Color.FromArgb(235, 234, 219);
			}

			protected override void WndProc(ref Message m)
			{
				switch (m.Msg)
				{
					case Win32.WM_PAINT:
						Win32.RECT update = new Win32.RECT();
						if (Win32.GetUpdateRect(m.HWnd, ref update, false) == 0)
							break;
						//Fill the paintstruct
						Win32.PAINTSTRUCT ps = new Win32.PAINTSTRUCT();
						IntPtr hdc = Win32.BeginPaint(m.HWnd, ref ps);
						//Create graphics object from the hdc
						Graphics g = Graphics.FromHdc(hdc);
						//Get the non-item rectangle
						int left = 0;
						Win32.RECT itemRect = new Win32.RECT();
						for (int i = 0; i < m_Parent.Columns.Count; i++)
						{
							Win32.SendMessage(m.HWnd, Win32.HDM_GETITEMRECT, i, ref itemRect);
							left += itemRect.right - itemRect.left;
						}
						//Davidem_Parent.headerHeight = itemRect.bottom-itemRect.top;
						if (left >= ps.rcPaint.left)
							left = ps.rcPaint.left;

						Rectangle r = new Rectangle(left, ps.rcPaint.top, ps.rcPaint.right - left, ps.rcPaint.bottom - ps.rcPaint.top);
						Rectangle r1 = new Rectangle(ps.rcPaint.left, ps.rcPaint.top, ps.rcPaint.right - left, ps.rcPaint.bottom - ps.rcPaint.top);

						g.FillRectangle(new SolidBrush(bkColor), r);
						m_Parent.DrawHeaderBorder(new DrawHeaderEventArgs(g, r, itemRect.bottom - itemRect.top));

						//Now we have to check if we have owner-draw columns and fill
						//the DRAWITEMSTRUCT appropriately
						int counter = 0;
						for (int i = 0; i < m_Parent.Columns.Count; i++)
						{
							Win32.DRAWITEMSTRUCT dis = new Win32.DRAWITEMSTRUCT();
							dis.ctrlType = 100;//ODT_HEADER
							dis.hwnd = m.HWnd;
							dis.hdc = hdc;
							dis.itemAction = 0x0001;//ODA_DRAWENTIRE
							dis.itemID = counter;

							//Must find if some item is pressed
							Win32.HDHITTESTINFO hi = new Win32.HDHITTESTINFO();
							hi.pt.X = m_Parent.PointToClient(MousePosition).X;
							hi.pt.Y = m_Parent.PointToClient(MousePosition).Y;
							int hotItem = Win32.SendMessage(m.HWnd, 0x1200 + 6, 0, ref hi);

							//If clicked on a divider - we don't have hot item
							if (hi.flags == 0x0004 || hotItem != counter)
								hotItem = -1;
							if (hotItem != -1 && mouseDown)
								dis.itemState = 0x0001;//ODS_SELECTED
							else
								dis.itemState = 0x0020;

							Win32.SendMessage(m.HWnd, Win32.HDM_GETITEMRECT, counter, ref itemRect);
							dis.rcItem = itemRect;

							Win32.SendMessage(m_Parent.Handle, Win32.WM_DRAWITEM, 0, ref dis);
							counter++;
						}
						g.Dispose();
						Win32.EndPaint(m.HWnd, ref ps);
						break;

					case Win32.WM_ERASEBKGND:
						break;

					case Win32.WM_LBUTTONDOWN:
						mouseDown = true;
						Win32.RedrawWindow(this.Handle, IntPtr.Zero, IntPtr.Zero, 1 /*RDW_INVALIDATE*/);
						base.WndProc(ref m);
						break;

					case Win32.WM_LBUTTONUP:
						mouseDown = false;
						Win32.RedrawWindow(this.Handle, IntPtr.Zero, IntPtr.Zero, 1 /*RDW_INVALIDATE*/);
						base.WndProc(ref m);
						break;

					/*
					case Win32.WM_FONT:
						if (10 > 0)
						{
							System.Drawing.Font f = new System.Drawing.Font(m_Parent.Font.Name, m_Parent.Font.SizeInPoints + 30);
							m.WParam = f.ToHfont();
						}						
						base.WndProc(ref m);
						break;*/

					default:
						base.WndProc(ref m);
						break;
				}
			}
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			switch (m.Msg)
			{
				case Win32.WM_DRAWITEM:
					//Get the DRAWITEMSTRUCT from the LParam of the message
					Win32.DRAWITEMSTRUCT dis = (Win32.DRAWITEMSTRUCT)Marshal.PtrToStructure(m.LParam, typeof(Win32.DRAWITEMSTRUCT));
					//Check if this message comes from the header
					if (dis.ctrlType == 100)//ODT_HEADER - it do comes from the header
					{
						//Get the graphics from the hdc field of the DRAWITEMSTRUCT
						Graphics g = Graphics.FromHdc(dis.hdc);
						//Create a rectangle from the RECT struct
						Rectangle r = new Rectangle(dis.rcItem.left, dis.rcItem.top, dis.rcItem.right - dis.rcItem.left, dis.rcItem.bottom - dis.rcItem.top);
						//Create new DrawItemState in its default state                    
						DrawItemState d = DrawItemState.Default;
						//Set the correct state for drawing
						if (dis.itemState == 0x0001)
							d = DrawItemState.Selected;
						//Create the DrawItemEventArgs object
						DrawItemEventArgs e = new DrawItemEventArgs(g, this.Font, r, dis.itemID, d);
						//If we have a handler attached call 
						//it and we don't want the default drawing
						//if(DrawColumn != null && !defaultCustomDraw)
						//	DrawColumn(this.Columns[dis.itemID], e);
						//else if(defaultCustomDraw)
						DoMyCustomHeaderDraw(this.Columns[dis.itemID], e);
						//Release the graphics object                    
						g.Dispose();
					}
					break;

				case Win32.WM_HSCROLL:
				case Win32.WM_VSCROLL:
				case Win32.WM_MOUSEWHEEL:
					IntPtr hWnd = (IntPtr)Win32.SendMessage(this.Handle, Win32.LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
					Win32.RECT rect = new Win32.RECT();
					Win32.GetClientRect(hWnd, ref rect);

					//
					if (m.Msg == Win32.WM_VSCROLL)
					{
						switch ((int)m.WParam) // mi dice il tipo di spostamento //
						{
							case 0: // sposto verso l'alto di 1 - ridisegno sopra //
								{
									System.Drawing.Rectangle rectInv = new System.Drawing.Rectangle(0, 20, Size.Width, 20);
									Invalidate(rectInv);
									break;
								}
							case 1: // sposto verso il basso di 1 - ridisegno sotto //
								{
									System.Drawing.Rectangle rectInv = new System.Drawing.Rectangle(0, Size.Height - 40, Size.Width, 40);
									Invalidate(rectInv);
									break;
								}
							case 2: // sposto verso l'alto di N - ridisegno tutto //
								{
									Invalidate();
									break;
								}
							case 3: // sposto verso il basso di N - ridisegno tutto //
								{
									Invalidate();
									break;
								}
							default:
								{
									break;
								}
						}
					}
					//

					if (m_Edit != null || m_Combo != null || m_Date != null)
					{
						int iPosX = Items[m_iEditItem].Bounds.X;
						for (int j = 0; j < m_iEditSubItem; j++)
							iPosX += Columns[j].Width;

						int iPosY = Items[m_iEditItem].Bounds.Y;
						if (iPosY < rect.bottom)
							iPosY -= rect.bottom * 2;
						Point point = new Point(iPosX, iPosY - 3);

						if (m_Edit != null)
							m_Edit.Location = point;
						if (m_Combo != null)
							m_Combo.Location = point;
						if (m_Date != null)
							m_Date.Location = point;
					}
					break;
			}
		}

		public void ResettaRighe()
		{
			m_listType.Clear();
		}

		public void AggiuntaRiga(int nIndex)
		{
			for (int i = 0; i < m_listType.Count; i++)
			{
				if (((SubItemType)m_listType[i]).iItem >= nIndex)
					((SubItemType)m_listType[i]).iItem++;
			}
		}

		public void RimossaRiga(int nIndex)
		{
			for (int i = 0; i < m_listType.Count; i++)
			{
				if (((SubItemType)m_listType[i]).iItem == nIndex)
				{
					m_listType.RemoveAt(i);
					i--;
				}
				else
				{
					if (((SubItemType)m_listType[i]).iItem > nIndex)
						((SubItemType)m_listType[i]).iItem--;
				}
			}
		}

		public void SetSubItemImage(int iItem, int iSubItem, int iImage)
		{
			Win32.SendMessage(Handle, Win32.LVM_SETEXTENDEDLISTVIEWSTYLE, (IntPtr)Win32.LVS_EX_SUBITEMIMAGES, (IntPtr)Win32.LVS_EX_SUBITEMIMAGES);

			Win32.LV_ITEM lvi = new Win32.LV_ITEM();
			lvi.iItem = iItem;
			lvi.iSubItem = iSubItem;
			lvi.mask = Win32.LVIF_IMAGE;
			lvi.iImage = iImage;
			Win32.SendMessage(Handle, Win32.LVM_SETITEM, 0, ref lvi);
		}

		void DrawHeaderBorder(DrawHeaderEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle r = new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height);
			if (r.Bottom == e.HeaderHeight)
			{
				g.DrawLine(new Pen(Color.Black, 1), r.Left, r.Bottom - 1, r.Right, r.Bottom - 1);
				g.DrawLine(new Pen(Color.Black, 1), r.Left, r.Bottom - 2, r.Right, r.Bottom - 2);
				g.DrawLine(new Pen(Color.Black, 1), r.Left, r.Bottom - 3, r.Right, r.Bottom - 3);
			}
		}

		void DoMyCustomHeaderDraw(object sender, DrawItemEventArgs e)
		{
			ColumnHeader m = sender as ColumnHeader;
			Graphics g = e.Graphics;
			//Get the text width

			// Font font = new Font(e.Font.FontFamily, e.Font.Size, e.Font.Bold ? FontStyle.Bold : FontStyle.Regular);
			Font font = new Font(e.Font.FontFamily, e.Font.Size, FontStyle.Bold);

			SizeF szf = g.MeasureString(m.Text, font);
			int textWidth = (int)szf.Width + 10;
			//Image image = null;

			Rectangle r = e.Bounds;
			int leftOffset = 4;
			int rightOffset = 4;

			StringFormat s = new StringFormat();
			s.FormatFlags = StringFormatFlags.NoWrap;
			s.Trimming = StringTrimming.EllipsisCharacter;
			switch (m.TextAlign)
			{
				case HorizontalAlignment.Left:
					s.Alignment = StringAlignment.Near;
					break;
				case HorizontalAlignment.Center:
					s.Alignment = StringAlignment.Center;
					break;
				case HorizontalAlignment.Right:
					s.Alignment = StringAlignment.Far;
					break;
			}
			s.LineAlignment = StringAlignment.Center;
			if (textWidth + leftOffset + rightOffset > r.Width)
				textWidth = r.Width - leftOffset - rightOffset;

			Rectangle text = new Rectangle(r.Left + leftOffset, r.Top, textWidth, r.Height);
			Rectangle img = Rectangle.Empty;

			//This occurs when column is pressed
			if ((e.State & DrawItemState.Selected) != 0)
			{
				g.FillRectangle(new SolidBrush(Color.FromArgb(222, 223, 216)), r.Left, r.Top, r.Width, r.Height);
				g.DrawRectangle(new Pen(Color.FromArgb(165, 165, 151), 1), r.Left, r.Top, r.Width, r.Height - 1);

				g.DrawLine(new Pen(Color.FromArgb(193, 194, 184), 1), r.Left + 1, r.Top, r.Left + 1, r.Bottom - 2);
				g.DrawLine(new Pen(Color.FromArgb(193, 194, 184), 1), r.Left + 1, r.Top, r.Right - 1, r.Top);

				g.DrawLine(new Pen(Color.FromArgb(208, 209, 201), 1), r.Left + 2, r.Top + 1, r.Left + 2, r.Bottom - 2);
				g.DrawLine(new Pen(Color.FromArgb(208, 209, 201), 1), r.Left + 2, r.Top + 1, r.Right - 1, r.Top + 1);

				text.Offset(1, 1);
				g.DrawString(m.Text, font, SystemBrushes.WindowText, text, s);
				text.Offset(-1, -1);
			}
			//Default state
			else
			{
				int iSeparatorLineHeight = (int)(r.Height * 0.7);

				g.DrawLine(SystemPens.ControlDark, r.Right - 1, r.Top + (r.Height - iSeparatorLineHeight) / 2, r.Right - 1, r.Bottom - iSeparatorLineHeight / 2);
				g.DrawLine(SystemPens.ControlLightLight, r.Right, r.Top + (r.Height - iSeparatorLineHeight) / 2, r.Right, r.Bottom - iSeparatorLineHeight / 2);

				g.DrawString(m.Text, font/*e.Font*/, SystemBrushes.WindowText, text, s);
			}
		}

		public void EditCell(int iItem, int iSubItem)
		{
			for (int i = 0; i < m_listType.Count; i++)
			{
				SubItemType item = (SubItemType)m_listType[i];
				if (item.iSubItem == iSubItem && (item.iItem == -1 || item.iItem == iItem))
				{
					m_iEditItem = iItem;
					m_iEditSubItem = iSubItem;
					CreateMyControl();
					break;
				}
			}
		}
	}
}
