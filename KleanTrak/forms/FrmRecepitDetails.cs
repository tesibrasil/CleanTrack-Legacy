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
    public partial class FrmRecepitDetails : Form
    {
        private string _receipt_content = "";
        public string ReceiptContent
        {
            get => _receipt_content;
            set
            {
                _receipt_content = value;
                tb_receipt.Text = value;
            }
        }
        public FrmRecepitDetails()
        {
            InitializeComponent();
        }
    }
}
