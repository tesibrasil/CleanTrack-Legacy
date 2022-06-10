using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data.Odbc;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [Guid("74E19A89-811D-3601-A235-2791AA9A253B")]
    public class NetworkUtility
	{
        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public static bool PingAddress(string address)
		{
			Logger.Info("Pinging address:" + address);

			if (address != "")
			{
				try
				{
					string ipOnly = address.Substring(0, address.IndexOf(":"));
					Ping ping = new Ping();
					PingReply rep = ping.Send(ipOnly);
					Logger.Info("result:" + rep.Status.ToString());
					return rep.Status == IPStatus.Success;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
					return false;
				}
			}

			return false;
		}

		public static string GetHostName()
		{
			string hostname = "internal";
			try
			{
				hostname = System.Net.Dns.GetHostName();
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}

			return hostname;
		}

	}
}
