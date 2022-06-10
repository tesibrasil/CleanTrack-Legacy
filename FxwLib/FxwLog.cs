using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace It.IDnova.Fxw
{
	public class FxwLog : AbstractFxwLog
	{
		private static UdpClient _theClient = (UdpClient)null;
		private const string DEFAULT_ADDRESS = "127.0.0.1";
		private const int DEFAULT_PORT = 10313;

		private FxwLog(string toAddress, int toPort)
		{
			try
			{
				FxwLog._theClient = new UdpClient();
				FxwLog._theClient.Connect(toAddress, toPort);
			}
			catch (Exception)
			{
				FxwLog._theClient = (UdpClient)null;
			}
		}

		public static FxwLog getInstance()
		{
			return FxwLog.getInstance("127.0.0.1", 10313);
		}

		public static FxwLog getInstance(string toAddress, int toPort)
		{
			if (AbstractFxwLog._instance == null)
				AbstractFxwLog._instance = new FxwLog(toAddress, toPort);
			return AbstractFxwLog._instance;
		}

		internal override void debug(string msg)
		{
			if (!AbstractFxwLog._isDebugEnabled)
				return;
			byte[] bytes = Encoding.ASCII.GetBytes(DateTime.Now.ToString("[yy-MM-dd HH.mm.ss,fff]") + " " + msg);
			FxwLog._theClient.Send(bytes, bytes.Length);

			// Console.WriteLine(DateTime.Now.ToString("[yy-MM-dd HH.mm.ss,fff]") + " " + msg);
			// Debug.WriteLine(DateTime.Now.ToString("[yy-MM-dd HH.mm.ss,fff]") + " " + msg);
		}

		/*
		public override void log(string msg)
		{
			if (!AbstractFxwLog._isLogEnabled)
				return;
			byte[] bytes = Encoding.ASCII.GetBytes(DateTime.Now.ToString("[yy-MM-dd HH.mm.ss,fff]") + " " + msg);
			FxwLog._theClient.Send(bytes, bytes.Length);
		}
		*/
	}
}
