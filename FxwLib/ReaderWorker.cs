using System.Threading;

namespace It.IDnova.Fxw
{
	internal class ReaderWorker : AbstractReaderWorker
	{
		private static EventWaitHandle _pollingReply = (EventWaitHandle)new AutoResetEvent(true);

		public ReaderWorker(FxwPhysicalType theRdrType, AbstractReaderWorker.ReceivedDataHandler callback)
		  : base(theRdrType, callback)
		{
		}

		protected override void startPhyLayer()
		{
			if (_physicalLayer != null)
			{
				_outMsgList.Clear();
				_errorCounter = 0;
				_physicalLayer.disconnect();
				_physicalLayer = (IFxwPhysicalLayer)null;
			}
			switch (_rdrType)
			{
				case FxwPhysicalType.USB:
					_physicalLayer = new FxwUsb(new PhysicalReceiveDataHandler(onReceivedData));
					break;
				case FxwPhysicalType.TCPIP:
					_physicalLayer = new FxwTcp(new PhysicalReceiveDataHandler(onReceivedData));
					break;
			}
			if (_physicalLayer == null)
				return;
			_logger.debug("[ReaderWorker] Worker started!");
		}

		internal override byte setPollingParams(RfidDefs.PollingType theType, int interval_ms)
		{
			_pollingType = theType;
			_pollingInterval_ms = interval_ms;
			return theType != RfidDefs.PollingType.UNKNOWN ? (byte)0 : (byte)2;
		}

		protected override void signalInventoryPollingReply()
		{
			_pollingReply.Set();
		}

		protected override void pollingWorker()
		{
			// Sandro 10/05/2017 // devo gestire la caduta della connessione di rete (vedi sendData o onReceivedData) //

			while (_status == Status.INVENTORY_LOOP)
			{
				_logger.debug("Waiting anticollision reply...");
				if (!_pollingReply.WaitOne(100))
					_logger.debug("...timed out !");
				if (_inventoryUsingTid)
					_outMsgList.Add(new RfidMsg(77, (byte)(_pollingType | RfidDefs.PollingType.UNIQUE_125KHZ), null, 0));
				else
					_outMsgList.Add(new RfidMsg(77, (byte)_pollingType, null, 0));

				if (manageSend() == 4)
				{
					int i = 0;
					i++;
				}

				_logger.debug("New anticollision requested");
				Thread.Sleep(_pollingInterval_ms);
			}
		}
	}
}
