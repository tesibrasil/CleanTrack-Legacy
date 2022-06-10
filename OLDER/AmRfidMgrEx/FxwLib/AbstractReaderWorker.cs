// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.AbstractReaderWorker
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace It.IDnova.Fxw
{
  internal abstract class AbstractReaderWorker
  {
    protected static FxwLog _logger = FxwLog.getInstance();
    protected AbstractReaderWorker.Status _status = AbstractReaderWorker.Status.NOT_STARTED;
    protected int _pollingInterval_ms = 100;
    private const int MAX_CONSECUTIVE_SEND = 10;
    private const int ERROR_MAX_NUMBER = 15;
    protected const int POLLING_INTERVAL_DEFAULT_TIMEOUT_MS = 100;
    private string _connString;
    protected FxwPhysicalType _rdrType;
    protected IFxwPhysicalLayer _physicalLayer;
    protected byte[] _pendingRxBuf;
    protected List<byte[]> _rawMsgList;
    protected List<RfidMsg> _outMsgList;
    protected int _errorCounter;
    protected bool _inventoryUsingTid;
    private Thread _pollingThread;
    protected RfidDefs.PollingType _pollingType;
    private AbstractReaderWorker.ReceivedDataHandler receivedData;

    internal AbstractReaderWorker(FxwPhysicalType theRdrType, AbstractReaderWorker.ReceivedDataHandler callback)
    {
      ThreadPool.SetMaxThreads(1, 0);
      this.receivedData += callback;
      this._rawMsgList = new List<byte[]>();
      this._outMsgList = new List<RfidMsg>();
      this._rdrType = theRdrType;
      this.startPhyLayer();
    }

    internal abstract byte setPollingParams(RfidDefs.PollingType theType, int interval_ms);

    protected abstract void pollingWorker();

    protected abstract void signalInventoryPollingReply();

    internal byte start(string aConnString)
    {
      try
      {
        this._connString = aConnString;
        byte num = this._physicalLayer.connect(aConnString);
        if ((int) num == 0)
          this._errorCounter = 0;
        return num;
      }
      catch (Exception ex)
      {
        AbstractReaderWorker._logger.debug("[ReaderWorker] Unexpected failure:" + ex.Message);
        return 3;
      }
    }

    internal void stop()
    {
      if (this.isValid() && this._physicalLayer != null)
        this._physicalLayer.disconnect();
      this.enableInventoryPolling(false);
      if (this._rawMsgList == null || this._rawMsgList.Count <= 0)
        return;
      this._rawMsgList.Clear();
    }

    internal bool isValid()
    {
      return this._physicalLayer != null && this._physicalLayer.isValid();
    }

    internal bool isConnected()
    {
      if (this.isValid() && this._physicalLayer != null)
        return this._physicalLayer.isConnected();
      return false;
    }

    internal string getConnectionString()
    {
      if (this._connString != null)
        return this._connString;
      return "UNDEFINED";
    }

    internal AbstractReaderWorker.Status getStatus()
    {
      return this._status;
    }

    protected abstract void startPhyLayer();

    internal bool isPhyLayerConnected()
    {
      bool flag = true;
      if (this._errorCounter > 15)
      {
        this._errorCounter = 0;
        AbstractReaderWorker._logger.debug("[ReaderWorker] Reached max number of errors => reconnect");
        this.startPhyLayer();
        flag = false;
        Thread.Sleep(20);
      }
      return flag;
    }

    internal void onReceivedData(byte[] dataBuff, int dataLen)
    {
      if (dataBuff == null)
        return;
      this._rawMsgList.Add(dataBuff);
      int num = (int) this.manageReceive();
    }

    internal void send(RfidMsg aMsg)
    {
      if (!this.isValid())
        return;
      AbstractReaderWorker._logger.debug("[ReaderWorker] Msg ADDED to OUT list");
      this._outMsgList.Add(aMsg);
      int num = (int) this.manageSend();
    }

    internal void rawSend(RfidMsg currMsg)
    {
      if (!this.isValid())
        AbstractReaderWorker._logger.debug("[ReaderWorker] Invalid phisical layer");
      else if (currMsg == null || !currMsg.isValid())
      {
        AbstractReaderWorker._logger.debug("[ReaderWorker] Invalid data out");
      }
      else
      {
        byte[] rawMsg = currMsg.getRawMsg();
        byte length = (byte) rawMsg.Length;
        if ((int) this._physicalLayer.sendData(rawMsg, (int) length) == 0)
        {
          this._errorCounter = 0;
          AbstractReaderWorker._logger.debug("[ReaderWorker] Sent msg (" + (object) length + "): " + currMsg.getRawMsgHexString());
        }
        else
          ++this._errorCounter;
      }
    }

    internal byte manageReceive()
    {
      int num = 0;
      if (this._rawMsgList == null || this._rawMsgList.Count == 0)
        return 0;
      byte[] rawMsg = this._rawMsgList[0];
      this._rawMsgList.RemoveAt(0);
      int length = num + rawMsg.Length;
      byte[] dataBuffer;
      if (this._pendingRxBuf != null)
      {
        dataBuffer = new byte[length + this._pendingRxBuf.Length];
        Array.Copy((Array) this._pendingRxBuf, (Array) dataBuffer, this._pendingRxBuf.Length);
        Array.Copy((Array) rawMsg, 0, (Array) dataBuffer, this._pendingRxBuf.Length, rawMsg.Length);
        this._pendingRxBuf = (byte[]) null;
      }
      else
      {
        dataBuffer = new byte[length];
        Array.Copy((Array) rawMsg, (Array) dataBuffer, rawMsg.Length);
      }
      this.decodeRawData(dataBuffer);
      return 0;
    }

    internal byte manageSend()
    {
      if (this._outMsgList == null || this._outMsgList.Count < 1)
        return 0;
      RfidMsg outMsg = this._outMsgList[0];
      this._outMsgList.RemoveAt(0);
      AbstractReaderWorker._logger.debug("[ReaderWorker] Msg GOT from OUT list");
      if (outMsg == null || !outMsg.isValid())
      {
        AbstractReaderWorker._logger.debug("[ReaderWorker] Msg not valid");
        return 0;
      }
      switch (outMsg.CommandIdentifier)
      {
        case 0:
          return 0;
        case 1:
          byte[] payload = outMsg.getPayload();
          if (payload != null)
          {
            Thread.Sleep(((int) payload[0] << 8) + (int) payload[1]);
            goto case 0;
          }
          else
            goto case 0;
        case 2:
          this._inventoryUsingTid = false;
          this.enableInventoryPolling(true);
          goto case 0;
        case 3:
          this._inventoryUsingTid = false;
          this.enableInventoryPolling(false);
          goto case 0;
        case 4:
          this._inventoryUsingTid = true;
          this.enableInventoryPolling(true);
          goto case 0;
        case 5:
          this._inventoryUsingTid = true;
          this.enableInventoryPolling(false);
          goto case 0;
        case 77:
          this.rawSend(outMsg);
          goto case 0;
        case 96:
          this._status = AbstractReaderWorker.Status.LOG_LOOP;
          this.rawSend(outMsg);
          goto case 0;
        default:
          this.rawSend(outMsg);
          goto case 0;
      }
    }

    internal void decodeRawData(byte[] dataBuffer)
    {
      if (dataBuffer == null || dataBuffer.Length == 0)
        return;
      int index1 = 0;
      int num1 = 0;
      int num2 = 0;
      while (index1 < dataBuffer.Length)
      {
        num1 = 0;
        num2 = 0;
        byte num3 = 0;
        while (index1 < dataBuffer.Length && (int) dataBuffer[index1] != 42)
          ++index1;
        int num4 = index1;
        if (num4 == dataBuffer.Length)
          break;
        if (dataBuffer.Length == 1 || index1 >= dataBuffer.Length - 1 || (int) dataBuffer[index1 + 1] + 3 > dataBuffer.Length - num4)
        {
          this.savependingdata(dataBuffer, num4);
          break;
        }
        int length = (int) dataBuffer[index1 + 1] + 3;
        for (int index2 = 2; index2 < length - 1; ++index2)
          num3 += dataBuffer[num4 + index2];
        if ((int) num3 == (int) dataBuffer[num4 + length - 1])
        {
          byte[] aMsgData = new byte[length];
          Array.Copy((Array) dataBuffer, num4, (Array) aMsgData, 0, length);
          RfidMsg aMsg = new RfidMsg(aMsgData, (byte) aMsgData.Length);
          if (this._status == AbstractReaderWorker.Status.INVENTORY_LOOP && (int) aMsg.CommandIdentifier == 224)
          {
            AbstractReaderWorker._logger.debug("[ReaderWorker] ANTICOLLISION reply got");
            this.signalInventoryPollingReply();
          }
          AbstractReaderWorker._logger.debug("[ReaderWorker] Received msg (" + (object) aMsg.getRawMsg().Length + "): " + aMsg.getRawMsgHexString());
          this.receivedData(aMsg);
        }
        index1 += length;
      }
    }

    internal void savependingdata(byte[] dataBuffer, int sofPosition)
    {
      int length = dataBuffer.Length - sofPosition;
      this._pendingRxBuf = new byte[length];
      Array.Copy((Array) dataBuffer, sofPosition, (Array) this._pendingRxBuf, 0, length);
      string str = "";
      foreach (byte num in this._pendingRxBuf)
        str = str + num.ToString("X2") + " ";
      AbstractReaderWorker._logger.debug("[ReaderWorker] Saved unused data (" + (object) this._pendingRxBuf.Length + " bytes): " + str);
    }

    private void enableInventoryPolling(bool enable)
    {
      if (enable)
      {
        AbstractReaderWorker._logger.debug("[ReaderWorker] Enabling inventory (polling)...");
        this._status = AbstractReaderWorker.Status.INVENTORY_LOOP;
        this._pollingThread = new Thread(new ThreadStart(this.pollingWorker));
        this._pollingThread.Start();
      }
      else
      {
        AbstractReaderWorker._logger.debug("[ReaderWorker] Disabling inventory...");
        this._status = AbstractReaderWorker.Status.IDLE;
        if (this._pollingThread == null)
          return;
        this._pollingThread.Join();
      }
    }

    internal RfidDefs.PollingType getPollingType()
    {
      return this._pollingType;
    }

    internal double getPollingInterval()
    {
      return (double) this._pollingInterval_ms;
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
