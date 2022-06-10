using System.Collections.Generic;
using System.Runtime.InteropServices;
using It.IDnova.Fxw;

namespace amrfidmgrex
{
	[ComVisible(true)]
    [Guid("8D89F6C8-251F-30DE-AE1F-FE67F9682C49")]
    public class RFIDHelper
	{
        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public delegate void AnticollisionLoopDataDelegate(string tagID);

		public delegate void ConnectionLossHandler();

		public enum LedType
		{
			Unknown,
			Orange,
			Red,
			Green,
			Blue
		}

		private List<string> AntiCollisionLoopList = null;
		private AnticollisionLoopDataDelegate AnticollisionHandlerCallback = null;
		private RfidReader Reader = null;
		private FxwLog FxwLogger = null;

		private static RFIDHelper Instance = null;
		public static string Address { get; set; }

		public static string HostName { get; set; }

		public static bool isInitiated()
		{
			return (Address != null && Address.Length > 0 && HostName != null && HostName.Length > 0);
		}

		public static void Reset()
		{
			Instance = new RFIDHelper();
		}

		public static RFIDHelper Get()
		{
			// writeLog("Gettin' RFID Helper");

			if (Instance == null)
			{
				// writeLog("NEW HELPER");
				Instance = new RFIDHelper();
			}

			return Instance;
		}

		public static void Finish()
		{
			if (Instance != null)
				Instance.Stop();
		}

		public RFIDHelper()
		{
			FxwLogger = FxwLog.getInstance();
			FxwLogger.logEnabled = true;
			FxwLogger.debugEnabled = true;

            if (!Start())
                Logger.Error("Can't start RFIDHelper");
		}

        ~RFIDHelper()
        {

        }
        
        private void onReaderData(RfidMsg msg)
		{
			if (!msg.isValid())
				return;

			if ((msg.CommandIdentifier == RfidDefs.FXW_TYPE_TAGID) &&
				(msg.CommandFlags == RfidDefs.FXW_RES_OK) &&
				(msg.getPayload() != null) &&
				(msg.getPayloadLen() != 0))
			{
				var payloadHex = RfidUtils.byteArrayToHexString(msg.getPayload());
				//writeLog(payloadHex.ToString());
				if (AntiCollisionLoopList.FindIndex(delegate (string item) { return (item == payloadHex); }) < 0)
				{
					AntiCollisionLoopList.Add(payloadHex);
					if (AnticollisionHandlerCallback != null)
						AnticollisionHandlerCallback(payloadHex);
				}
			}
		}

		public void ResetIO()
		{
			Led(LedType.Orange, false);
			Led(LedType.Blue, false);
			Led(LedType.Green, false);
			Led(LedType.Red, false);
		}

		private bool Start()
		{
			if (Reader != null)
				return false;

			Reader = new RfidReader(FxwPhysicalType.TCPIP);
			if (Reader.connect(Address) == RfidDefs.FXW_RES_OK)
			{
				ResetIO();
				return true;
			}

			return false;
		}

		public void Stop()
		{
			if (Reader == null)
				return;

			//writeLog("disconnecting reader");
			Reader.disconnect();

			//Reader = null;
		}

		public bool isReaderConnected()
		{
			return Reader.isConnected();
		}

		public bool StartAnticollisionLoop(AnticollisionLoopDataDelegate callback)
		{
			if (callback == null)
				return false;

			if (Reader.isInventorying() == true)
			{
				StopAnticollisionLoop();
			}

			ResetIO();
			Led(LedType.Blue, true);

			AntiCollisionLoopList = new List<string>();

			AnticollisionHandlerCallback = callback;

			//POLLING: prima era a 100
			Reader.setPollingParams(RfidDefs.PollingType.HF_UHF_SINGLE, 300);
			Reader.receivedData += new RfidReader.ReceivedDataHandler(onReaderData);

			return (Reader.inventory(RfidReader.InventoryMode.START_LOOP) == RfidDefs.FXW_RES_OK);
		}

		public bool StopAnticollisionLoop()
		{
			if (Reader.isInventorying())
			{
				Reader.receivedData -= new RfidReader.ReceivedDataHandler(onReaderData);
				AntiCollisionLoopList.Clear();
				var result = (Reader.inventory(RfidReader.InventoryMode.STOP_LOOP) == RfidDefs.FXW_RES_OK);
				ResetIO();
				return result;
			}
			return false;
		}

		private byte GetByteFrom(LedType type)
		{
			byte val = 0;
			switch (type)
			{
				case LedType.Orange:
					val = 0;
					break;

				case LedType.Red:
					val = 1;
					break;

				case LedType.Green:
					val = 2;
					break;

				case LedType.Blue:
					val = 3;
					break;
			}

			return val;
		}

		public void Led(LedType type, bool On = true)
		{
			Reader.setOutput(AbstractRfidReader.TypeIO.LED,
							GetByteFrom(type),
							On ? AbstractRfidReader.ModeIO.ON : AbstractRfidReader.ModeIO.OFF,
							0, 0, 0);
			System.Threading.Thread.Sleep(100);
		}

		public void Buzzer(int duration)
		{
			Reader.setOutput(AbstractRfidReader.TypeIO.BUZZER,
							 0,
							 AbstractRfidReader.ModeIO.PULSE,
							 duration, 0, 0);

			System.Threading.Thread.Sleep(500);

		}

	}
}
