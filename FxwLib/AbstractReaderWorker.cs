using System;
using System.Collections.Generic;
using System.Threading;

namespace It.IDnova.Fxw
{
	internal abstract class AbstractReaderWorker
	{
		protected static FxwLog _logger = FxwLog.getInstance();
		private string _connString = null;
		protected IFxwPhysicalLayer _physicalLayer = null;
		protected Status _status = Status.NOT_STARTED;
		protected byte[] _pendingRxBuf = null;
		protected List<byte[]> _rawMsgList = null;
		protected List<RfidMsg> _outMsgList = null;
		protected int _errorCounter = 0;
		protected bool _inventoryUsingTid = false;
		private Thread _pollingThread = null;
		protected RfidDefs.PollingType _pollingType = RfidDefs.PollingType.HF_UHF_SINGLE;
		protected int _pollingInterval_ms = 100;
		internal Action<byte[]> DataFromReader;
		internal Action<byte[]> DataToReader;
		private const int MAX_CONSECUTIVE_SEND = 10;
		private const int ERROR_MAX_NUMBER = 15;
		protected const int POLLING_INTERVAL_DEFAULT_TIMEOUT_MS = 100;
		protected FxwPhysicalType _rdrType;
		private ReceivedDataHandler receivedData;

		internal AbstractReaderWorker(FxwPhysicalType theRdrType, ReceivedDataHandler callback)
		{
			receivedData = receivedData + callback;
			_rawMsgList = new List<byte[]>();
			_outMsgList = new List<RfidMsg>();
			_rdrType = theRdrType;
			startPhyLayer();
		}

		internal abstract byte setPollingParams(RfidDefs.PollingType theType, int interval_ms);

		protected abstract void pollingWorker();

		protected abstract void signalInventoryPollingReply();

		internal byte start(string aConnString, bool useAsciiTransport, object transportChannel, int txDelayMs)
		{
			try
			{
				_connString = aConnString;
				byte num = _physicalLayer.connect(aConnString, useAsciiTransport, transportChannel, txDelayMs);
				if ((int)num == 0)
					_errorCounter = 0;
				return num;
			}
			catch (Exception ex)
			{
				_logger.debug("[ReaderWorker] Unexpected failure:" + ex.Message);
				return 3;
			}
		}

		internal void stop()
		{
			if (isValid() && _physicalLayer != null)
				_physicalLayer.disconnect();
			enableInventoryPolling(false);
			if (_rawMsgList == null || _rawMsgList.Count <= 0)
				return;
			_rawMsgList.Clear();
		}

		internal bool isValid()
		{
			return _physicalLayer != null && _physicalLayer.isValid();
		}

		internal bool isConnected()
		{
			if (isValid() && _physicalLayer != null)
				return _physicalLayer.isConnected();
			return false;
		}

		internal string getConnectionString()
		{
			if (_connString != null)
				return _connString;
			return "UNDEFINED";
		}

		internal Status getStatus()
		{
			return _status;
		}

		protected abstract void startPhyLayer();

		internal bool isPhyLayerConnected()
		{
			bool flag = true;
			if (_errorCounter > 15)
			{
				_errorCounter = 0;
				_logger.debug("[ReaderWorker] Reached max number of errors => reconnect");
				startPhyLayer();
				flag = false;
				Thread.Sleep(20);
			}
			return flag;
		}

		internal void onReceivedData(byte[] dataBuff, int dataLen)
		{
			if (dataBuff == null)
				return;

			if (DataFromReader != null)
				DataFromReader(dataBuff);

			_rawMsgList.Add(dataBuff);
			int num = (int)manageReceive();
		}

		internal int send(RfidMsg aMsg)
		{
			if (!isValid())
			{
				// Sandro 10/05/2017 //

				switch (aMsg.CommandIdentifier)
				{
					case 2:
					{
						_inventoryUsingTid = false;
						enableInventoryPolling(true);
						break;
					}
					case 3:
					{
						_inventoryUsingTid = false;
						enableInventoryPolling(false);
						break;
					}
				}

				// boh... decido io... 0 1 4 erano già utilizzati //
				return 2; 
			}

			if (DataToReader != null)
				DataToReader(aMsg.getRawMsg());

			_logger.debug("[ReaderWorker] Msg ADDED to OUT list");
			_outMsgList.Add(aMsg);
			return manageSend();
		}

		internal int rawSend(RfidMsg currMsg)
		{
			int iRet = 2; // Sandro 10/05/2017 // boh... decido io... 0 1 4 erano già utilizzati //

			if (!isValid())
			{
				_logger.debug("[ReaderWorker] Invalid phisical layer");
			}
			else if (currMsg == null || !currMsg.isValid())
			{
				_logger.debug("[ReaderWorker] Invalid data out");
			}
			else
			{
				byte[] rawMsg = currMsg.getRawMsg();
				byte length = (byte)rawMsg.Length;

				iRet = _physicalLayer.sendData(rawMsg, (int)length);
				if (iRet == 0)
				{
					// tutto OK //
					_errorCounter = 0;
					_logger.debug("[ReaderWorker] Sent msg (" + (object)length + "): " + currMsg.getRawMsgHexString());
				}
				else
				{
					// in teoria:
					// 1 --> _physicalLayer not valid
					// 4 --> SocketException (che è quello che interessa a me) //
					_errorCounter = _errorCounter + 1;
				}
			}

			return iRet;
		}

		internal byte manageReceive()
		{
			int num = 0;
			if (_rawMsgList == null || _rawMsgList.Count == 0)
				return 0;
			byte[] rawMsg = _rawMsgList[0];
			_rawMsgList.RemoveAt(0);
			int length = num + rawMsg.Length;
			byte[] dataBuffer;
			if (_pendingRxBuf != null)
			{
				dataBuffer = new byte[length + _pendingRxBuf.Length];
				Array.Copy((Array)_pendingRxBuf, (Array)dataBuffer, _pendingRxBuf.Length);
				Array.Copy((Array)rawMsg, 0, (Array)dataBuffer, _pendingRxBuf.Length, rawMsg.Length);
				_pendingRxBuf = (byte[])null;
			}
			else
			{
				dataBuffer = new byte[length];
				Array.Copy((Array)rawMsg, (Array)dataBuffer, rawMsg.Length);
			}
			decodeRawData(dataBuffer);
			return 0;
		}

		internal int manageSend()
		{
			if (_outMsgList == null || _outMsgList.Count < 1)
				return 0;

			RfidMsg outMsg = _outMsgList[0];
			_outMsgList.RemoveAt(0);
			_logger.debug("[ReaderWorker] Msg GOT from OUT list");
			if (outMsg == null || !outMsg.isValid())
			{
				_logger.debug("[ReaderWorker] Msg not valid");
				return 0;
			}

			switch (outMsg.CommandIdentifier)
			{
				case 0:
				{
					return 0;
				}
				case 1:
				{
					byte[] payload = outMsg.getPayload();
					if (payload != null)
						Thread.Sleep(((int)payload[0] << 8) + (int)payload[1]);

					return 0;
				}
				case 2:
				{
					_inventoryUsingTid = false;
					enableInventoryPolling(true);
					return 0;
				}
				case 3:
				{
					_inventoryUsingTid = false;
					enableInventoryPolling(false);
					return 0;
				}
				case 4:
				{
					_inventoryUsingTid = true;
					enableInventoryPolling(true);
					return 0;
				}
				case 5:
				{
					_inventoryUsingTid = true;
					enableInventoryPolling(false);
					return 0;
				}
				case 77:
				{
					return rawSend(outMsg);
				}
				case 96:
				{
					_status = AbstractReaderWorker.Status.LOG_LOOP;
					return rawSend(outMsg);
				}
				default:
				{
					return rawSend(outMsg);
				}
			}
		}

		internal void decodeRawData(byte[] dataBuffer)
		{
			if (dataBuffer == null)
				return;
			if (dataBuffer.Length == 0)
			{
				receivedData(new RfidMsg());
			}
			else
			{
				int index1 = 0;
				while (index1 < dataBuffer.Length)
				{
					byte num3 = 0;
					while (index1 < dataBuffer.Length && (int)dataBuffer[index1] != 42)
						++index1;
					int num4 = index1;
					if (num4 == dataBuffer.Length)
						break;
					if (dataBuffer.Length == 1 || index1 >= dataBuffer.Length - 1 || (int)dataBuffer[index1 + 1] + 3 > dataBuffer.Length - num4)
					{
						savependingdata(dataBuffer, num4);
						break;
					}
					int length = (int)dataBuffer[index1 + 1] + 3;
					for (int index2 = 2; index2 < length - 1; ++index2)
						num3 += dataBuffer[num4 + index2];
					if ((int)num3 == (int)dataBuffer[num4 + length - 1])
					{
						byte[] aMsgData = new byte[length];
						Array.Copy((Array)dataBuffer, num4, (Array)aMsgData, 0, length);
						RfidMsg aMsg = new RfidMsg(aMsgData, (byte)aMsgData.Length);
						if (_status == AbstractReaderWorker.Status.INVENTORY_LOOP && (int)aMsg.CommandIdentifier == 224)
						{
							_logger.debug("[ReaderWorker] ANTICOLLISION reply got");
							signalInventoryPollingReply();
						}
						_logger.debug("[ReaderWorker] Received msg (" + (object)aMsg.getRawMsg().Length + "): " + aMsg.getRawMsgHexString());
						receivedData(aMsg);
					}
					index1 += length;
				}
			}
		}

		internal void savependingdata(byte[] dataBuffer, int sofPosition)
		{
			int length = dataBuffer.Length - sofPosition;
			_pendingRxBuf = new byte[length];
			Array.Copy((Array)dataBuffer, sofPosition, (Array)_pendingRxBuf, 0, length);
			string str = "";
			foreach (byte num in _pendingRxBuf)
				str = str + num.ToString("X2") + " ";
			_logger.debug("[ReaderWorker] Saved unused data (" + (object)_pendingRxBuf.Length + " bytes): " + str);
		}

		protected void enableInventoryPolling(bool enable)
		{
			if (enable)
			{
				_logger.debug("[ReaderWorker] Enabling inventory (polling)...");
				_status = Status.INVENTORY_LOOP;
				_pollingThread = new Thread(new ThreadStart(pollingWorker));
				_pollingThread.Start();
			}
			else
			{
				_logger.debug("[ReaderWorker] Disabling inventory...");
				_status = Status.IDLE;
				if (_pollingThread != null)
					_pollingThread.Join();
			}
		}

		internal RfidDefs.PollingType getPollingType()
		{
			return _pollingType;
		}

		internal double getPollingInterval()
		{
			return (double)_pollingInterval_ms;
		}

		public enum Status
		{
			IDLE,
			NOT_STARTED,
			STARTING,
			WAITING_ANSW,
			INVENTORY_LOOP,
			LOG_LOOP,
			TIMED_OUT,
		}

		public delegate void ReceivedDataHandler(RfidMsg aMsg);
	}
}
