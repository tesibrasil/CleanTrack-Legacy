using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KleanTrak.Core;
using KleanTrak.Model;

namespace FileParserTest
{
    public partial class FrmMain : Form
    {
        string logdir = Application.StartupPath + @"\log";
        string receiptdir = "";
        public FrmMain()
        {
            InitializeComponent();
        }
        private WPBase GetParser(out Washer washer)
        {
            folderBrowserDialog1.ShowDialog();
            receiptdir = folderBrowserDialog1.SelectedPath;
            washer = new Washer()
            {
                ID = 0,
                IDSede = 0,
                Code = tbmatricola.Text,
                Description = "prova",
                SerialNumber = tbseriale.Text,
                TimeToClean = 60,
                Type = (WasherStorageTypes)((ComboItem)cbmodello.SelectedItem).Value,
                PollingTime = 10,
                FolderOrFileName = receiptdir,
                User = "",
                Password = ""
            };
            var parser = WPBase.Get(washer);
            parser.IsPreWasher = true;
            parser.TestMode = true;
            return parser;
        }
        private void loadTestReceipt_Click(object sender, EventArgs e)
        {
            if (cbmodello.SelectedItem == null)
            {
                MessageBox.Show("Selezionare un modello");
                return;
            }
            var parser = GetParser(out Washer washer);
            parser.GetCycles(washer, DateTime.MinValue);
        }
        private class ComboItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
            public override string ToString() => Text;
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            var machineTypes = Enum.GetValues(typeof(WasherStorageTypes));
            foreach (var item in machineTypes)
            {
                cbmodello.Items.Add(new ComboItem { Value = (int)item, Text = item.ToString() });
            }
            logfilewatcher.EnableRaisingEvents = false;
            if (Directory.Exists(logdir))
                Directory.Delete(logdir);
            Directory.CreateDirectory(logdir);
            logfilewatcher.Path = logdir;
            logfilewatcher.EnableRaisingEvents = true;
        }

        private void logfilewatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted)
                return;
            foreach (string line in File.ReadAllLines(e.FullPath))
                tbmessages.Text += $"{Environment.NewLine}{line}";
        }

        private void btnmovetoold_Click(object sender, EventArgs e)
        {
            var parser = GetParser(out Washer w);
            foreach (string file in Directory.GetFiles(receiptdir))
            {
                parser.MoveToOldDir(file);
                parser.MoveToBadDir(file);
            }
        }
    }
}
