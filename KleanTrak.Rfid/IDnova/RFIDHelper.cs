using It.IDnova.Fxw;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace KleanTrak.Rfid
{
	public class RFIDHelper
	{
		// metto a true così al primo giro mi spegne il led (che magari è stato acceso per qualche motivo) //
		bool m_bOnLedOrange = true; 
		bool m_bOnLedRed = true;
		bool m_bOnLedGreen = true;
		bool m_bOnLedBlue = false; // Sandro 10/05/2017 //

		//
		public delegate void AnticollisionLoopDataDelegate(string tagID);
		private AnticollisionLoopDataDelegate AnticollisionHandlerCallback = null;

		private List<string> m_listAnticollisionLoopTags = null;
		private RfidReader m_RfidReader = null;

		public RFIDHelper()
		{
			Debug.WriteLine("RFIDHelper.RFIDHelper");

/* 
#if DEBUG
			FxwLog.getInstance().logEnabled = true;
			FxwLog.getInstance().debugEnabled = true;
#endif
*/
		}

		private void OnReaderData(RfidMsg msg)
		{
			if (!msg.isValid())
			{
				Debug.WriteLine("RFIDHelper.OnReaderData(INVALID)");

				Thread thread = new Thread(() => { AnticollisionHandlerCallback?.Invoke("NETWORK_CRASH"); }); // invoca solo se non è nullo // 
				thread.Start();

				return;
			}
			else
			{
				if((RfidDefs.FxwCommands)msg.CommandIdentifier != RfidDefs.FxwCommands.ANTICOLL)
					Debug.WriteLine("RFIDHelper.OnReaderData(" + (RfidDefs.FxwCommands)msg.CommandIdentifier + ", " + msg.CommandFlags + ", " + msg.decodeErrorCode() + ", " + msg.getPayloadHexString(false) + ", " + msg.getPayloadString() + ")");
			}

			switch (msg.CommandIdentifier)
			{
				case (byte)RfidDefs.FxwCommands.IO_SET_OUTPUT:
				{
					// Debug.WriteLine("   BEFORE _eventWaitSetOutput.Set");
					_eventWaitSetOutput.Set();
					// Debug.WriteLine("   AFTER _eventWaitSetOutput.Set");
					break;
				}
				case RfidDefs.FXW_TYPE_TAGID:
				{
					if ((msg.CommandFlags == RfidDefs.FXW_RES_OK) &&
						(msg.getPayload() != null) &&
						(msg.getPayloadLen() != 0))
					{
						var payloadHex = RfidUtils.byteArrayToHexString(msg.getPayload());

						if (m_listAnticollisionLoopTags.FindIndex(delegate (string item) { return (item == payloadHex); }) < 0)
						{
							Debug.WriteLine("RFIDHelper.OnReaderData");

							m_listAnticollisionLoopTags.Add(payloadHex);

							Thread thread = new Thread(() => { AnticollisionHandlerCallback?.Invoke(payloadHex); }); // invoca solo se non è nullo // 
							thread.Start();
						}
					}

					break;
				}
			}
		}

		public bool Start(string sAddress, AnticollisionLoopDataDelegate callback)
		{
			Debug.WriteLine("RFIDHelper.Start");

			if (m_RfidReader != null)
				return false;

			m_RfidReader = new RfidReader(FxwPhysicalType.TCPIP);

			if (m_RfidReader.connect(sAddress) == RfidDefs.FXW_RES_OK)
				return StartAnticollisionLoop(callback);

			return false;
		}

		public void Stop()
		{
			Debug.WriteLine("RFIDHelper.Stop");

			if (m_RfidReader == null)
				return;

			StopAnticollisionLoop();
			m_RfidReader.disconnect();
			m_RfidReader = null;
		}

		public bool StartAnticollisionLoop(AnticollisionLoopDataDelegate callback)
		{
			Debug.WriteLine("RFIDHelper.StartAnticollisionLoop");

			if (callback == null)
				return false;

			if (m_RfidReader != null)
				if (m_RfidReader.isInventorying())
					StopAnticollisionLoop();

			Led(AbstractRfidReader.LedIO.BLUE, 1, 0, 0);

			m_listAnticollisionLoopTags = new List<string>();

			AnticollisionHandlerCallback = callback;

			if (m_RfidReader != null)
			{
				m_RfidReader.setPollingParams(RfidDefs.PollingType.HF_UHF_SINGLE, 250); // polling time prima era a 100
				m_RfidReader.receivedData += new RfidReader.ReceivedDataHandler(OnReaderData);
			}

			return PlayPollingLoop();
		}

		public bool StopAnticollisionLoop()
		{
			Debug.WriteLine("RFIDHelper.StopAnticollisionLoop");

			if (m_RfidReader != null)
			{
				if (m_RfidReader.isInventorying())
				{
					m_RfidReader.receivedData -= new RfidReader.ReceivedDataHandler(OnReaderData);
					m_listAnticollisionLoopTags.Clear();

					return PausePollingLoop();
				}
			}

			return false;
		}

		private bool PausePollingLoop()
		{
			Debug.WriteLine("RFIDHelper.PausePollingLoop");

			bool bReturn = true;

			if (m_RfidReader != null)
				if (m_RfidReader.isInventorying())
					bReturn = (m_RfidReader.inventory(RfidReader.InventoryMode.STOP_LOOP) == RfidDefs.FXW_RES_OK);

			return bReturn;
		}

		private bool PlayPollingLoop()
		{
			bool bReturn = true;

			if (m_RfidReader == null)
				return false;

			if (!m_RfidReader.isInventorying())
			{
				Debug.WriteLine("RFIDHelper.PlayPollingLoop");
				bReturn = (m_RfidReader.inventory(RfidReader.InventoryMode.START_LOOP) == RfidDefs.FXW_RES_OK);
			}

			return bReturn;
		}

		public void Led(AbstractRfidReader.LedIO ltType, int iBuzzerOnMsec, int iBuzzerOffMsec, byte byBuzzerRepeat)
		{
			Debug.WriteLine("RFIDHelper.Led (" + ltType + " --> buzzer: on " + iBuzzerOnMsec.ToString() + " msec - off " + iBuzzerOffMsec.ToString() + " msec - repeat " + byBuzzerRepeat.ToString() + ")");

			PausePollingLoop();

			if (m_bOnLedOrange && (ltType != AbstractRfidReader.LedIO.ORANGE))
			{
				m_bOnLedOrange = false;
				SetOutput(AbstractRfidReader.TypeIO.LED, AbstractRfidReader.LedIO.ORANGE, AbstractRfidReader.ModeIO.OFF, 0, 0, 0);
			}
			if (m_bOnLedRed && (ltType != AbstractRfidReader.LedIO.RED))
			{
				m_bOnLedRed = false;
				SetOutput(AbstractRfidReader.TypeIO.LED, AbstractRfidReader.LedIO.RED, AbstractRfidReader.ModeIO.OFF, 0, 0, 0);
			}
			if (m_bOnLedGreen && (ltType != AbstractRfidReader.LedIO.GREEN))
			{
				m_bOnLedGreen = false;
				SetOutput(AbstractRfidReader.TypeIO.LED, AbstractRfidReader.LedIO.GREEN, AbstractRfidReader.ModeIO.OFF, 0, 0, 0);
			}
			if (m_bOnLedBlue && (ltType != AbstractRfidReader.LedIO.BLUE))
			{
				m_bOnLedBlue = false;
				SetOutput(AbstractRfidReader.TypeIO.LED, AbstractRfidReader.LedIO.BLUE, AbstractRfidReader.ModeIO.OFF, 0, 0, 0);
			}

			switch (ltType)
			{
				case AbstractRfidReader.LedIO.ORANGE:
				{
					if (!m_bOnLedOrange)
					{
						m_bOnLedOrange = true;
						SetOutput(AbstractRfidReader.TypeIO.LED, AbstractRfidReader.LedIO.ORANGE, AbstractRfidReader.ModeIO.ON, 0, 0, 0);
					}
					break;
				}
				case AbstractRfidReader.LedIO.RED:
				{
					if (!m_bOnLedRed)
					{
						m_bOnLedRed = true;
						SetOutput(AbstractRfidReader.TypeIO.LED, AbstractRfidReader.LedIO.RED, AbstractRfidReader.ModeIO.ON, 0, 0, 0);
					}
					break;
				}
				case AbstractRfidReader.LedIO.GREEN:
				{
					if (!m_bOnLedGreen)
					{
						m_bOnLedGreen = true;
						SetOutput(AbstractRfidReader.TypeIO.LED, AbstractRfidReader.LedIO.GREEN, AbstractRfidReader.ModeIO.ON, 0, 0, 0);
					}
					break;
				}
				case AbstractRfidReader.LedIO.BLUE:
				{
					if (!m_bOnLedBlue)
					{
						m_bOnLedBlue = true;
						SetOutput(AbstractRfidReader.TypeIO.LED, AbstractRfidReader.LedIO.BLUE, AbstractRfidReader.ModeIO.ON, 0, 0, 0);
					}
					break;
				}
			}

			if (iBuzzerOnMsec > 0)
			{
				if (byBuzzerRepeat <= 1)
					SetOutput(AbstractRfidReader.TypeIO.BUZZER, AbstractRfidReader.LedIO.NOLED, AbstractRfidReader.ModeIO.PULSE, iBuzzerOnMsec, 0, 0);
				else
					SetOutput(AbstractRfidReader.TypeIO.BUZZER, AbstractRfidReader.LedIO.NOLED, AbstractRfidReader.ModeIO.PULSE_LOOP, iBuzzerOnMsec, iBuzzerOffMsec, byBuzzerRepeat);
			}

			PlayPollingLoop();
		}

		ManualResetEvent _eventWaitSetOutput = new ManualResetEvent(false);

		private void SetOutput(AbstractRfidReader.TypeIO anlOtype, AbstractRfidReader.LedIO aChannel, AbstractRfidReader.ModeIO mode, int onTime, int offTime, byte numRepeat)
		{
			// Debug.WriteLine("   BEFORE _eventWaitSetOutput.Reset");
			_eventWaitSetOutput.Reset();
			// Debug.WriteLine("   AFTER _eventWaitSetOutput.Reset");

			Debug.WriteLine("   Reader.setOutput(" + anlOtype + ", " + aChannel + ", " + mode + ", " + onTime + ", " + offTime + ", " + numRepeat + ")");

			if (anlOtype == AbstractRfidReader.TypeIO.BUZZER)
			{
				aChannel = AbstractRfidReader.LedIO.ORANGE;

#if DEBUG
				// pa no spacare massa i maroni in debug... //
				onTime = 1;
#endif
			}

			if (m_RfidReader != null)
				m_RfidReader.setOutput(anlOtype, (byte)aChannel, mode, onTime, offTime, numRepeat);

			// Debug.WriteLine("   BEFORE _eventWaitSetOutput.WaitOne");
			_eventWaitSetOutput.WaitOne(2000);
			// Debug.WriteLine("   AFTER _eventWaitSetOutput.WaitOne");
		}
	}
}
