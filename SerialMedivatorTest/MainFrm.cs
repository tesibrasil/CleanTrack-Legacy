using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using log4net;
using KleanTrak.Core;
using KleanTrak.Model;
using System.IO;
using log4net.Config;
using System.Configuration;
namespace SerialMedivatorTest
{
	public partial class MainFrm : Form
	{
		Washer _fake_washer = null;
		WPCantelMedivatorsSerial _parser = null;
		ILog _logger = null;
		public MainFrm()
		{
			InitializeComponent();
			var repo = LogManager.GetRepository(System.Reflection.Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(repo, new FileInfo("log4net.config"));
			var reader = new AppSettingsReader();
			var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			var settings = configFile.AppSettings.Settings;
			DbConnection.ConnectionString = "Driver={SQL Server};" + 
				"Server=" + settings["dbserver"].Value + ";" + 
				"Database=" + settings["database"].Value + ";" +
				"Uid=" + settings["dbuser"].Value  + ";" + 
				"Pwd=" + settings["dbpassword"].Value + ";";
			_logger = LogManager.GetLogger("MainForm");
			FillComPorts();
			_fake_washer = new Washer
			{
				Type = WasherStorageTypes.Washer_Cantel_Medivators_Serial,
				ID = 1,
				IDSede = 1,
				Code = "test_connection",
				FolderOrFileName = ""
			};
			file_timer.Start();
		}
		private void LogError(Exception e)
		{
			_logger.Error(e);
			MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		private void FillComPorts() => cb_com.Items.AddRange(SerialPort.GetPortNames());
		private void btn_connect_Click(object sender, EventArgs e)
		{
			try
			{
				if (_parser == null || !_parser.Running)
					Connect();
				else
					Disconnect();
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}
		private void Connect()
		{
			try
			{
				if (cb_com.SelectedItem == null)
				{
					MessageBox.Show("selezionare una porta com");
					return;
				}
				_fake_washer.FolderOrFileName = cb_com.SelectedItem.ToString();
				_parser = (WPCantelMedivatorsSerial)WPBase.Get(_fake_washer);
				_parser.SerialLineReceived += _parser_SerialLineReceived;
				_parser.CycleReconstructed += _parser_CycleReconstructed;
				_parser.LogLineAdded += _parser_LogLineAdded;
				_parser.Start();
				btn_connect.BackColor = Color.LightSalmon;
				btn_connect.Text = "DISCONNECT";
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}
		private void _parser_LogLineAdded(string line)
		{
			Invoke((MethodInvoker)delegate 
			{
				tblog.Text = line +
				Environment.NewLine +
				"--------------------------" +
				Environment.NewLine +
				tblog.Text;
			});
		}
		private void _parser_CycleReconstructed(WasherCycle cycle)
		{
			Invoke((MethodInvoker)delegate
		   {
			   tbreconstructeddata.Text = cycle +
				Environment.NewLine +
				"--------------------------" +
				Environment.NewLine + 
				tbreconstructeddata.Text;
		   });
		}
		private void _parser_SerialLineReceived(string line)
		{
			Invoke((MethodInvoker)delegate
		   {
			   tbreceiveddata.Text = line + 
			   Environment.NewLine +
			   tbreceiveddata.Text;
		   });
		}
		private void Disconnect()
		{
			try
			{
				if (DialogResult.OK != MessageBox.Show("Sicuri di voler chiudere la connessione?", "Conferma", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
					return;
				_parser.Stop();
				btn_connect.BackColor = Color.LightGreen;
				btn_connect.Text = "CONNECT";
				_parser = null;
				GC.Collect();
			}
			catch (Exception ex)
			{
				LogError(ex);
			}
		}
		private void StoreData()
		{
			try
			{
				if (_parser == null)
					return;
				var cycles = _parser.GetCycles(_fake_washer, DateTime.Now);
				cycles.ForEach(c => SaveFile(c));
			}
			catch (Exception e)
			{
				LogError(e);
			}
		}
		private void SaveFile(WasherCycle cycle)
		{
			try
			{
				if (!Directory.Exists("OutputFiles"))
					Directory.CreateDirectory("OutputFiles");
				using (var fstream = new FileStream(GetFileName(), FileMode.CreateNew))
				using (var streamwriter = new StreamWriter(fstream))
					streamwriter.Write(cycle.ToString());
			}
			catch (Exception e)
			{
				LogError(e);
			}
		}
		private string GetFileName() => $"./OutputFiles/{DateTime.Now.Year}" +
			DateTime.Now.Month.ToString().PadLeft(2, '0') +
			DateTime.Now.Day.ToString().PadLeft(2, '0') +
			DateTime.Now.Hour.ToString().PadLeft(2, '0') +
			DateTime.Now.Minute.ToString().PadLeft(2, '0') +
			DateTime.Now.Second.ToString().PadLeft(2, '0') +
			DateTime.Now.Millisecond.ToString().PadLeft(3, '0') + 
			".txt";
		private void file_timer_Tick(object sender, EventArgs e)
		{
			StoreData();
		}

		private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			file_timer.Stop();
		}

		private void btnclearreceived_Click(object sender, EventArgs e)
		{
			tbreceiveddata.Text = "";
		}

		private void btnreconstructed_Click(object sender, EventArgs e)
		{
			tbreconstructeddata.Text = "";
		}

		private void btnlog_Click(object sender, EventArgs e)
		{
			tblog.Text = "";
		}
	}
}
