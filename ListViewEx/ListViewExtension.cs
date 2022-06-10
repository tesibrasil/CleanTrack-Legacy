using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ListViewEx
{
    public enum ColumnType
    {
        String,
        Number,
        Date
    }

    public static class ListViewExtension
    {
        public static void SetColumnType(this ColumnHeader column, ColumnType type)
        {
            column.Tag = type;
        }

        public static ColumnType GetColumnType(this ColumnHeader column)
        {
            return (column.Tag != null && column.Tag is ColumnType) ? (ColumnType)column.Tag : ColumnType.String;
        }
    }
}
