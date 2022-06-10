using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KleanTrak.Core;

namespace MDGTest
{
    public partial class FrmMain : Form
    {
        private PWPCantelMDG mdgCommunicator = new PWPCantelMDG("0000");
        public FrmMain()
        {
            InitializeComponent();
            mdgCommunicator.CommandReceived += MdgCommunicator_CommandReceived;
            mdgCommunicator.CommandSent += MdgCommunicator_CommandSent;
            mdgCommunicator.ErrorsThrown += MdgCommunicator_ErrorsThrown;
        }
        private void MdgCommunicator_ErrorsThrown(string data)
        {
            try
            {
                Invoke(new MethodInvoker(() => Log($"!!!ERROR: {data}")));
            }
            catch (Exception)
            {
            }
        }
        private void MdgCommunicator_CommandSent(string data)
        {
            try
            {
                Invoke(new MethodInvoker(() => Log($"COMMAND SENT ---> {data}")));
            }
            catch (Exception)
            {
            }
        }
        private void MdgCommunicator_CommandReceived(string data)
        {
            try
            {
                Invoke(new MethodInvoker(() => Log($"COMMAND RECEIVED <--- {data}")));
            }
            catch (Exception)
            {
            }
        }
        private void Log(Exception e)
        {
            Log(e.ToString());
        }
        private void Log(string msg)
        {
            tberrors.Text = msg + Environment.NewLine + tberrors.Text;
        }
        private void btnopencloseconn_Click(object sender, EventArgs e)
        {
            try
            {
                mdgCommunicator.SetNewIdMac(nudmacid.Value.ToString());
                mdgCommunicator.SetFolderOrFilename($"{nudip1.Value}.{nudip2.Value}.{nudip3.Value}.{nudip4.Value}:{nudport.Value}");
                mdgCommunicator.SetConnectionsParameters();
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            cbcmdtosend.Items.AddRange(new string[] { "GETSTATUS", "GETMEMORY", "DELMEMORY", "INFO", "TM hh:mm:ss", "DT dd/mm/yyyy" });
        }

        private void btnsend_Click(object sender, EventArgs e)
        {
            try
            {
                if (mdgCommunicator.Ipaddress == null || mdgCommunicator.Portnumber < 0)
                {
                    MessageBox.Show("Parametri di connessione non settati, specificare ip e porta");
                    return;
                }
                mdgCommunicator.SendCommand(cbcmdtosend.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        private void cbcmdtosend_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnsend.Enabled = cbcmdtosend.SelectedItem != null && cbcmdtosend.SelectedItem.ToString().Length > 0;
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        private void btngetchecksum_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbcommand.Text.Length == 0)
                {
                    MessageBox.Show("Inserire un comando in 'COMMAND'");
                    tbchecksum.Text = "";
                    return;
                }
                var checksum = mdgCommunicator.GetChecksum(Encoding.ASCII.GetBytes(tbcommand.Text));
                tbchecksum.Text = Encoding.ASCII.GetString(checksum);
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }
    }
}
