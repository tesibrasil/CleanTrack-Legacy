// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.ReaderWorker
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

using System.Threading;

namespace It.IDnova.Fxw
{
  internal class ReaderWorker : AbstractReaderWorker
  {
    private static EventWaitHandle _pollingReply = (EventWaitHandle) new AutoResetEvent(true);

    public ReaderWorker(FxwPhysicalType theRdrType, AbstractReaderWorker.ReceivedDataHandler callback)
      : base(theRdrType, callback)
    {
    }

    protected override void startPhyLayer()
    {
      if (this._physicalLayer != null)
      {
        this._outMsgList.Clear();
        this._errorCounter = 0;
        this._physicalLayer.disconnect();
        this._physicalLayer = (IFxwPhysicalLayer) null;
      }
      switch (this._rdrType)
      {
        case FxwPhysicalType.USB:
          this._physicalLayer = (IFxwPhysicalLayer) new FxwUsb(new PhysicalReceiveDataHandler(((AbstractReaderWorker) this).onReceivedData));
          break;
        case FxwPhysicalType.TCPIP:
          this._physicalLayer = (IFxwPhysicalLayer) new FxwTcp(new PhysicalReceiveDataHandler(((AbstractReaderWorker) this).onReceivedData));
          break;
      }
      if (this._physicalLayer == null)
        return;
      AbstractReaderWorker._logger.debug("[ReaderWorker] Worker started!");
    }

    internal override byte setPollingParams(RfidDefs.PollingType theType, int interval_ms)
    {
      this._pollingType = theType;
      this._pollingInterval_ms = interval_ms;
      return theType != RfidDefs.PollingType.UNKNOWN ? (byte) 0 : (byte) 2;
    }

    protected override void signalInventoryPollingReply()
    {
      ReaderWorker._pollingReply.Set();
    }

    protected override void pollingWorker()
    {
      while (this._status == AbstractReaderWorker.Status.INVENTORY_LOOP)
      {
        AbstractReaderWorker._logger.debug("Waiting anticollision reply...");
        if (!ReaderWorker._pollingReply.WaitOne(100))
          AbstractReaderWorker._logger.debug("...timed out !");
        if (this._inventoryUsingTid)
          this._outMsgList.Add(new RfidMsg((byte) 77, (byte) (this._pollingType | RfidDefs.PollingType.UNIQUE_125KHZ), (byte[]) null, (byte) 0));
        else
          this._outMsgList.Add(new RfidMsg((byte) 77, (byte) this._pollingType, (byte[]) null, (byte) 0));
        int num = (int) this.manageSend();
        AbstractReaderWorker._logger.debug("New anticollision requested");
        Thread.Sleep(this._pollingInterval_ms);
      }
    }
  }
}
