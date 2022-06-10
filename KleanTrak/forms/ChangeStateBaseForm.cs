using amrfidmgrex;
using OdbcExtensions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;
using Commons;
namespace KleanTrak
{
    public class ChangeStateBaseForm : System.Windows.Forms.Form
    {
        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string OkSoundPath = @"c:\Windows\Media\chimes.wav";
        private string ErrorSoundPath = @"c:\Windows\Media\chord.wav";

        protected int m_nOperatore = -1;
        protected int m_nDispositivo = -1;
        protected bool m_bEnableConfirmSeconds = true;
        protected int m_iConfirmSeconds;
        protected string nomeOperatore = "";
        protected System.Windows.Forms.Button btnAnnulla;
        protected System.Windows.Forms.TextBox textBoxDispositivo;
        protected System.Windows.Forms.TextBox textBoxOperatore;
        protected System.ComponentModel.IContainer components;
        protected System.Windows.Forms.Button btnSalva;
        protected System.Windows.Forms.Label labelOperatore1;
        protected System.Windows.Forms.Label labelOperatore2;
        protected System.Windows.Forms.Label labelDispositivo1;
        protected System.Windows.Forms.Label labelDispositivo2;
        protected System.Windows.Forms.Timer timer1;
        protected System.Windows.Forms.Label labelAutoOperatore;
        protected System.Windows.Forms.Label labelAutoDispositivo;
        protected State StateToAssign = null;

        protected void OnLoad()
        {
            KleanTrak.Core.Dictionary.Init();
            m_bEnableConfirmSeconds = KleanTrak.Globals.EnableConfirmTimer;
            m_iConfirmSeconds = KleanTrak.Globals.ConfirmTimer;
            labelOperatore1.Visible = true;
            textBoxOperatore.Visible = true;
        }

        public bool Salva(string deviceTag, string userTag, int stateToSave, int oldState, Dictionary<string, string> optionalField = null)
        {
            Console.Write("Insert New Cycle Ex -> OldState: " + oldState.ToString() + " NewState: " + stateToSave + "\n");
            return DBUtilities.insertnewCycleEx(deviceTag, userTag, -1, stateToSave, oldState, -1, KleanTrak.Globals.strDatabase, optionalField);
        }

        protected void QueryTag(ref Message m)
        {
            OdbcConnection connTemp = DBUtil.GetODBCConnection();
            OdbcCommand commTemp = new OdbcCommand("", connTemp);
            commTemp.CommandText = "SELECT id FROM Operatori WHERE idrfid1 = '" + m.WParam.ToString() + "' AND idrfid2 = '" + m.LParam.ToString() + "' AND DISATTIVATO = 0";

            OdbcDataReader readerTemp = commTemp.ExecuteReader();
            if (readerTemp.HasRows)
                m.Result = (IntPtr)1;

            if ((readerTemp != null) && (readerTemp.IsClosed == false))
                readerTemp.Close();

            commTemp.CommandText = "SELECT id FROM Dispositivi WHERE idrfid1 = '" + m.WParam.ToString() + "' AND idrfid2 = '" + m.LParam.ToString() + "' AND DISMESSO = 0  and eliminato = 0";

            readerTemp = commTemp.ExecuteReader();
            if (readerTemp.HasRows)
                m.Result = (IntPtr)2;

            if ((readerTemp != null) && (readerTemp.IsClosed == false))
                readerTemp.Close();

            connTemp.Close();
        }

        protected void textBoxOperatore_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
                return;
            if (textBoxOperatore.Text.Length <= 0)
                return;
            var request = new KleanTrak.Model.CmdGetInfoFromBarcode { Barcode = textBoxOperatore.Text };
            var response = KleanTrak.Core.Info.GetInfoFromBarcode(request);
            if (response.BarcodeType != Model.BarcodeTypes.Operator)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[216],
                    "Clean Track",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                textBoxDispositivo.Text = "";
                return;
            }
            if (!response.Successed)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[32],
                    "Clean Track",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                textBoxDispositivo.Text = "";
                return;
            }
            nomeOperatore = response.Description;
            textBoxOperatore.Visible = false;
            labelOperatore2.Text = nomeOperatore;
            labelOperatore1.ForeColor = Color.Orange;
            labelOperatore2.ForeColor = Color.Orange;
            labelDispositivo1.ForeColor = Color.Orange;
            labelOperatore2.Visible = true;
            labelDispositivo1.Visible = true;
            textBoxDispositivo.Visible = true;
            textBoxDispositivo.Focus();
            return;
        }

        protected void textBoxDispositivo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            m_nDispositivo = -1;
            if (e.KeyCode != Keys.Enter)
                return;
            if (textBoxDispositivo.Text.Length <= 0)
                return;
            var request = new KleanTrak.Model.CmdGetInfoFromBarcode { Barcode = textBoxDispositivo.Text };
            var response = KleanTrak.Core.Info.GetInfoFromBarcode(request);
            if (response.BarcodeType != Model.BarcodeTypes.Device)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[215],
                    "Clean Track",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                textBoxDispositivo.Text = "";
                return;
            }
            if (!response.Successed)
            {
                MessageBox.Show(KleanTrak.Globals.strTable[76],
                    "Clean Track",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                textBoxDispositivo.Text = "";
                return;
            }
            labelOperatore1.ForeColor = Color.Green;
            labelOperatore2.ForeColor = Color.Green;
            labelDispositivo1.ForeColor = Color.Green;
            labelDispositivo2.ForeColor = Color.Green;
            textBoxDispositivo.Visible = false;
            labelDispositivo2.Visible = true;
            labelDispositivo2.Text = response.Description;
            PlaySound(OkSoundPath);
            m_nDispositivo = Core.Devices.GetDeviceIdFromBarcode(request.Barcode);
            btnSalva.Enabled = true;
            btnSalva.Focus();
            if (m_bEnableConfirmSeconds)
            {
                timer1.Interval = 1000;
                timer1.Enabled = true;
            }
        }

        private void PlaySound(string soundPath)
        {
            try
            {
                using (var soundPlayer = new SoundPlayer(soundPath))
                {
                    soundPlayer.Play();
                }
            }
            catch (Exception)
            {
            }
        }

        protected void RFIDError(ref Message m)
        {
            labelAutoOperatore.Visible = true;
            labelOperatore1.Visible = false;
            labelOperatore2.Visible = false;
            labelAutoDispositivo.Visible = false;
            labelDispositivo1.Visible = false;
            labelDispositivo2.Visible = false;

            textBoxOperatore.Text = "";
            textBoxDispositivo.Text = "";

            labelOperatore2.Text = "";
            labelDispositivo2.Text = "";

            m_nOperatore = -1;
            m_nDispositivo = -1;
            m.Result = (IntPtr)1;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case RFIDManagerMsg._RFID_MSG_USER_BADGE:
                    break;

                case RFIDManagerMsg._RFID_MSG_DEVICE_TAG:
                    break;

                case RFIDManagerMsg._RFID_MSG_ERROR:
                    RFIDError(ref m);
                    break;

                case RFIDManagerMsg._RFID_MSG_QUERY_TAG:
                    QueryTag(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        public static bool OpenChangeStateForms(string v)
        {
            bool result = false;
            Console.WriteLine(v);
            IEnumerable<State> states = (from s in StateList.Instance.GetList() where s.ShortcutKeys.ToUpper() == v.ToUpper() select s);
            if (states.Count() > 0)
            {
                ChangeStateGeneric dlg = new ChangeStateGeneric(states.First());
                dlg.ShowDialog();
                result = true;
            }
            return result;
        }
    }
}
