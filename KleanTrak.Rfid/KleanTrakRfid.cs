using It.IDnova.Fxw;
using LibLog;
using System;
using System.Diagnostics;
using System.Threading;

namespace KleanTrak.Rfid
{
	public abstract class KleanTrakRfid : IDisposable
    {
		private bool m_bDisposing = false;

		private int m_iTimeout;
		private string m_sAddress;
		private int m_iPort;
		protected string m_sHostName;

		private RFIDHelper m_helperRFID = null;

		private Timer m_timerTimeout = null;

		//

		protected abstract void ReadedTagCallback(string sInput);
		protected abstract void ReadedTagTimeout();

		protected KleanTrakRfid(int iTimeout, string sAddress, int iPort, string sHostName)
		{
			Debug.WriteLine("KleanTrakRfid.KleanTrakRfid");

			m_iTimeout = iTimeout;
			m_sAddress = sAddress;
			m_iPort = iPort;
			m_sHostName = sHostName;

			if (m_iTimeout <= 0)
				m_iTimeout = 10;
			if (m_iTimeout > 60)
				m_iTimeout = 60;

			if (m_sAddress.Trim().Length <= 0)
			{
				Logger.Get().Write(m_sHostName, "KleanTrak.Rfid", "KleanTrakRfid: empty address", null, Logger.LogLevel.Error);
				return;
			}

			Thread myThread = new Thread(() => CreateAndStartHelper());
			myThread.Start();
		}

		public void Dispose()
		{
			Debug.WriteLine("KleanTrakRfid.Dispose");

			m_bDisposing = true;

			if (m_helperRFID != null)
			{
				m_helperRFID.Stop();
				Thread.Sleep(250);
				m_helperRFID = null;
			}
		}

		private void AnticollisionCallback(string sInput)
		{
			Debug.WriteLine("KleanTrakRfid.AnticollisionCallback");
			// Logger.Get().Write(m_sHostName, "KleanTrak.Rfid", "AnticollisionCallback (" + m_sHostName + " --> " + sInput + ")", null, Logger.LogLevel.Info);

			if (String.Equals(sInput, "NETWORK_CRASH", StringComparison.OrdinalIgnoreCase))
			{
				Thread myThread = new Thread(() => CreateAndStartHelper());
				myThread.Start();
			}
			else
			{
				// invoco il metodo della classe derivata //
				ReadedTagCallback(sInput);
			}
		}

		protected void ResetState(/* int iPauseMsec */)
		{
			Debug.WriteLine("KleanTrakRfid.ResetState");

			StopTimeoutTimer();

            //Davide test per risolvere disconnessione dopo passaggio primo tag m_helperRFID.Stop();
            //Davide test per risolvere disconnessione dopo passaggio primo tag m_helperRFID.Start(m_sAddress + ":" + m_iPort.ToString(), AnticollisionCallback);

            CreateAndStartHelper();
        }

		protected void StartTimeoutTimer()
		{
			Debug.WriteLine("KleanTrakRfid.StartTimeoutTimer");
			// Logger.Get().Write(m_sHostName, "KleanTrak.Rfid", "StartTimeoutTimer --> value: " + (m_iTimeout * 1000).ToString() + " ms", null, Logger.LogLevel.Info);

			if (m_timerTimeout != null)
			{
				m_timerTimeout.Dispose();
				m_timerTimeout = null;
			}

			m_timerTimeout = new Timer(TimeoutTick, this, m_iTimeout * 1000, System.Threading.Timeout.Infinite);

			// Logger.Get().Write(m_sHostName, "KleanTrak.Rfid", "StartTimeoutTimer (end)", null, Logger.LogLevel.Info);
		}

		private void StopTimeoutTimer()
		{
			Debug.WriteLine("KleanTrakRfid.StopTimeoutTimer");

			if (m_timerTimeout != null)
			{
				m_timerTimeout.Dispose();
				m_timerTimeout = null;
			}
		}

		private void TimeoutTick(object state)
		{
			Debug.WriteLine("KleanTrakRfid.TimeoutTick");
			// Logger.Get().Write(m_sHostName, "KleanTrak.Rfid", "TimeoutTick", null, Logger.LogLevel.Info);

			if (m_timerTimeout != null)
			{
				m_timerTimeout.Dispose();
				m_timerTimeout = null;
			}

			// Logger.Get().Write(m_sHostName, "KleanTrak.Rfid", "TimeoutTick (end)", null, Logger.LogLevel.Info);

			// invoco il metodo della classe derivata //

			ReadedTagTimeout();
		}

		protected void Led(AbstractRfidReader.LedIO ltColor, int iBuzzerOnMsec, int iBuzzerOffMsec, byte byBuzzerRepeat)
		{
			Debug.WriteLine("KleanTrakRfid.Led");

			if (m_helperRFID != null)
				m_helperRFID.Led(ltColor, iBuzzerOnMsec, iBuzzerOffMsec, byBuzzerRepeat);
		}

		private void CreateAndStartHelper()
		{
			bool bOK = false;
			while (!bOK)
			{
				if (m_helperRFID != null)
				{
					m_helperRFID.Stop();
					Thread.Sleep(250);
					m_helperRFID = null;
				}

				if (m_bDisposing)
					return;

				m_helperRFID = new RFIDHelper();
				if (m_helperRFID.Start(m_sAddress + ":" + m_iPort.ToString(), AnticollisionCallback))
				{
					Logger.Get().Write("", "KleanTrak.Rfid", "Started " + m_sHostName + " (" + m_sAddress + ") reader.", null, Logger.LogLevel.Info);
					bOK = true;
				}
				else
				{
					Logger.Get().Write("", "KleanTrak.Rfid", m_sHostName + " (" + m_sAddress + ") --> Can't start RFIDHelper", null, Logger.LogLevel.Error);

					if (m_helperRFID != null)
					{
						m_helperRFID.Stop();
						Thread.Sleep(250);
						m_helperRFID = null;
					}
				}

				if(!bOK)
					Thread.Sleep(2500);
			}
		}
	}
}
