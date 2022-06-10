using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KleanTrak
{
    public partial class FrmDateSelect : Form
    {
        public DateTime SelectedDate { get; private set; } = DateTime.Now;
        public bool DataValid { get; private set; } = false;
        public FrmDateSelect()
        {
            InitializeComponent();
            lbl_instructions.Text = KleanTrak.Globals.strTable[134];
            btn_ok.Text = KleanTrak.Globals.strTable[135];
            btn_cancel.Text = KleanTrak.Globals.strTable[136];
            mont_calendar.SelectionStart = DateTime.Now;
        }
        private void mont_calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            SelectedDate = e.Start;
        }
        public void SetDate(DateTime date)
        {
            SelectedDate = date;
            mont_calendar.SelectionStart = date;
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DataValid = false;
            Close();
        }
        private void btn_ok_Click(object sender, EventArgs e)
        {
            DataValid = true;
            Close();
        }
    }
}
