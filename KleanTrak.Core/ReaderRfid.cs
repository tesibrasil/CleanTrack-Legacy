using It.IDnova.Fxw;
using KleanTrak.Model;
using KleanTrak.Rfid;

namespace KleanTrak.Core
{
	public class ReaderRfid : KleanTrakRfid
	{
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string m_sDeviceBarcode = "";
		private string m_sDeviceDescription = "";

		private string m_sOperatorBarcode = "";
		private string m_sOperatorDescription = "";

		private int m_iNewStateID;

		public ReaderRfid(int iNewStateID, int iTimeout, string sAddress, int iPort, string sHostName) : base(iTimeout, sAddress, iPort, sHostName)
		{
			m_iNewStateID = iNewStateID;
		}

		protected override void ReadedTagCallback(string sInput)
		{
			bool bError = false;
			string sTemp = "";

			if (Devices.TryToFindDeviceBarcode(sInput, ref sTemp))
			{
				if (m_sDeviceBarcode.Length <= 0)
				{
					m_sDeviceBarcode = sInput;
					m_sDeviceDescription = sTemp;

					Logger.Info("Reader " + m_sHostName + ": found device " + m_sDeviceBarcode + " (" + m_sDeviceDescription + ")");

					StartTimeoutTimer();
				}
				else
				{
					bError = true;
				}
			}
			else
			{
				if (Operators.TryToFindUserBarcode(sInput, ref sTemp))
				{
					if (m_sOperatorBarcode.Length <= 0)
					{
						m_sOperatorBarcode = sInput;
						m_sOperatorDescription = sTemp;

						Logger.Info("Reader " + m_sHostName + ": found operator " + m_sOperatorBarcode + " (" + m_sOperatorDescription + ")");

						StartTimeoutTimer();
					}
					else
					{
						bError = true;
					}
				}
				else
				{
					bError = true;
				}
			}

			if (bError)
			{
				Led(AbstractRfidReader.LedIO.RED, 1500, 0, 0);

				m_sDeviceBarcode = "";
				m_sDeviceDescription = "";

				m_sOperatorBarcode = "";
				m_sOperatorDescription = "";

				ResetState(); // 3000);
			}
			else
			{
				if ((m_sDeviceBarcode.Length > 0) && (m_sOperatorBarcode.Length > 0))
				{
					if(Complete(m_sDeviceBarcode, m_sOperatorBarcode))
						Led(AbstractRfidReader.LedIO.GREEN, 150, 150, 3);
					else
						Led(AbstractRfidReader.LedIO.RED, 1500, 0, 0);

					m_sDeviceBarcode = "";
					m_sDeviceDescription = "";

					m_sOperatorBarcode = "";
					m_sOperatorDescription = "";

					ResetState(); // 3000);
				}
				else if ((m_sDeviceBarcode.Length > 0) || (m_sOperatorBarcode.Length > 0))
				{
					Led(AbstractRfidReader.LedIO.ORANGE, 200, 0, 0);
				}
			}
		}

		protected override void ReadedTagTimeout()
		{
			Led(AbstractRfidReader.LedIO.RED, 1500, 0, 0);

			m_sDeviceBarcode = "";
			m_sDeviceDescription = "";

			m_sOperatorBarcode = "";
			m_sOperatorDescription = "";

			ResetState(); // 500);
		}

		private bool Complete(string sDevice, string sOperator)
		{
			CmdSetDeviceStatus req = new CmdSetDeviceStatus();

			req.OperationID = m_iNewStateID.ToString();
			req.DeviceBarcode = sDevice;
			req.UserBarcode = sOperator;

			return Devices.SetDeviceStatus(req).Successed;
		}
	}
}
