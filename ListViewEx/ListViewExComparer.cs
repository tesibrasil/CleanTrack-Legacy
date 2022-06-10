using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ListViewEx
{
    public class ListViewExComparer : IComparer
    {
        private int _col = -1;
        private bool _asc = true;
        private ListViewEx _listView;

        public ListViewExComparer(ListViewEx listView)
        {
            _listView = listView;
        }

        public int Column
        {
            get
            {
                return _col;
            }
            set
            {
                if (_col == value)
                    _asc = !_asc;
                else
                    _asc = true;

                _col = value;
                _listView.Sort();
            }
        }

        public int Compare(object x, object y)
        {
            if (_col < 0 || _col >= _listView.Columns.Count)
                return 0;

            if (_col >= ((ListViewItem)x).SubItems.Count || _col >= ((ListViewItem)y).SubItems.Count)
                return 0;

            string textX = ((ListViewItem)x).SubItems[_col].Text;
            string textY = ((ListViewItem)y).SubItems[_col].Text;

            int ret = 0;

            switch (_listView.Columns[_col].GetColumnType())
            {
                case ColumnType.Number:
                    int numX = 0, numY = 0;
                    if (int.TryParse(textX, out numX) && int.TryParse(textY, out numY))
                    {
                        if (_asc == true)
                            ret = numX > numY ? 1 : 0;
                        else
                            ret = numY > numX ? 1 : 0;
                    }
                    else
                    {
                        if (_asc == true)
                            ret = String.Compare(textX, textY);
                        else
                            ret = String.Compare(textY, textX);
                    }
                    break;

                case ColumnType.Date:
                    string a = textX, b = textY;
                    if (textX.Length == 19 && textY.Length == 19) // 01/01/2000 01:00:00
                    {
                        a = textX.Substring(6, 4) + textX.Substring(3, 2) + textX.Substring(0, 2) + textX.Substring(11, 2) + textX.Substring(14, 2) + textX.Substring(17, 2);
                        b = textY.Substring(6, 4) + textY.Substring(3, 2) + textY.Substring(0, 2) + textY.Substring(11, 2) + textY.Substring(14, 2) + textY.Substring(17, 2);
                    }
                    else if (textX.Length == 16 && textY.Length == 16) // 01/01/2000 01:00
                    {
                        a = textX.Substring(6, 4) + textX.Substring(3, 2) + textX.Substring(0, 2) + textX.Substring(11, 2) + textX.Substring(14, 2);
                        b = textY.Substring(6, 4) + textY.Substring(3, 2) + textY.Substring(0, 2) + textY.Substring(11, 2) + textY.Substring(14, 2);
                    }
                    else if (textX.Length == 10 && textY.Length == 10) // 01/01/2000
                    {
                        a = textX.Substring(6, 4) + textX.Substring(3, 2) + textX.Substring(0, 2);
                        b = textY.Substring(6, 4) + textY.Substring(3, 2) + textY.Substring(0, 2);
                    }

                    if (_asc == true)
                        ret = String.Compare(a, b);
                    else
                        ret = String.Compare(b, a);
                    break;

                case ColumnType.String:
                default:
                    if (_asc == true)
                        ret = String.Compare(textX, textY);
                    else
                        ret = String.Compare(textY, textX);
                    break;
            }

            return ret;
        }
    }
}
