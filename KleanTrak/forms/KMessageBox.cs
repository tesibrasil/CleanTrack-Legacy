using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace kleanTrak.forms
{
    public partial class KMessageBox : Form
    {
        private int SecondsToClose = -1;
        public KMessageBox(string description, string title)
        {
            InitializeComponent();

            this.Text = title;
            this.tbMessage.Text = description;
            SecondsToClose = KleanTrak.Globals.ConfirmTimer;
        }

        private void KMessageBox_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SecondsToClose--;

            this.button1.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));

            button1.Text = KleanTrak.Globals.strTable[115] + SecondsToClose.ToString() + KleanTrak.Globals.strTable[116];

            if (SecondsToClose == 0)
                button1_Click(null, null);
        }
    }
}
