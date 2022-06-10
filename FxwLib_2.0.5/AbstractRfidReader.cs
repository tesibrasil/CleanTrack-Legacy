// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.AbstractRfidReader
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

using It.IDnova.Fxw.Tag245;
using System;
using System.Collections.Generic;
using System.Threading;

namespace It.IDnova.Fxw
{
  public abstract class AbstractRfidReader
  {
    protected static FxwLog _logger = FxwLog.getInstance();
    protected static bool _DEBOUNCING_ENABLED = false;
    protected static TimeSpan _KEYB_EMU_WAIT_TIME_MS = new TimeSpan(0, 0, 0, 0, 1000);
    protected ReaderInfo _readerInfo = new ReaderInfo();
    protected FxwKbd.KeybEmuMode _keybEmuMode = FxwKbd.KeybEmuMode.DATA_CRLF;
    protected volatile string _lastTagIdRead = "NO ID DEF";
    protected const int _DEBOUNCING_TIMER_INTERVAL_DEFAULT = 2000;
    protected const string _DEBOUNCE_STRING_DEFAULT = "NO ID DEF";
    private const int _TIME_IO_WAIT_INTERVAL = 131;
    private const ushort _DEV_TYPE_UNDEF = 0;
    private const int _DEV_TYPE_MSGLEN = 3;
    protected ushort _deviceType;
    protected bool _isInventorying;
    protected bool _keyEmuAddTimestamp;
    protected FxwKbd _keybEmulator;
    internal AbstractReaderWorker _readerWorker;

    public event AbstractRfidReader.ReceivedDataHandler receivedData;

    public event AbstractRfidReader.ReceiveTagDataHandler receivedTagData;

    public abstract byte inventory(AbstractRfidReader.InventoryMode invMode);

    public abstract byte inventoryTID(AbstractRfidReader.InventoryMode invMode);

    public abstract int getKeyboardEmulationDebouncing();

    public abstract void setKeyboardEmulationDebouncing(int interval_ms);

    protected abstract void emulateKeyboard(string data);

    public AbstractRfidReader(FxwPhysicalType theRdrType)
    {
      if (!RfidDefs.isLibValid())
        throw new Exception("FxwLib license expired");
      this._readerWorker = (AbstractReaderWorker) new ReaderWorker(theRdrType, new AbstractReaderWorker.ReceivedDataHandler(this.onWorkerReceivedData));
    }

    public byte connect(string aConnString)
    {
      return this.connect(aConnString, 3000);
    }

    public byte connect(string aConnString, int timeout_ms)
    {
      AbstractRfidReader._logger.debug("[RfidReader] Connecting reader...");
      byte num = 1;
      if (!this.isValid())
        return num;
      if (this._readerWorker.start(aConnString) == (byte) 0)
      {
        this.identifyDevice(timeout_ms);
        if (this._readerInfo.Status == "VALID")
        {
          AbstractRfidReader._logger.debug("Device succesfully identified");
          num = (byte) 0;
        }
        else
        {
          AbstractRfidReader._logger.debug("Device not identified. Stopping ReaderWorker...");
          this._readerWorker.stop();
          num = byte.MaxValue;
          AbstractRfidReader._logger.debug("...succesfully stopped");
        }
      }
      if (num == (byte) 0)
        AbstractRfidReader._logger.debug("[RfidReader] Reader connected");
      else
        AbstractRfidReader._logger.debug("[RfidReader] Reader connection failed");
      return num;
    }

    public virtual byte disconnect()
    {
      if (!this.isValid())
        return 1;
      if (this.isConnected())
      {
        this._isInventorying = false;
        this._readerWorker.stop();
        AbstractRfidReader._logger.debug("[RfidReader] Reader disconnected");
      }
      return 0;
    }

    public string getConnectionString()
    {
      if (this.isValid())
        return this._readerWorker.getConnectionString();
      return "UNDEFINED";
    }

    public ReaderInfo getReaderInfo()
    {
      return this._readerInfo;
    }

    public bool isValid()
    {
      return this._readerWorker != null;
    }

    public bool isConnected()
    {
      return this.isValid() && this._readerWorker.isConnected() && this._readerInfo.Status == "VALID";
    }

    public bool isInventorying()
    {
      return this._isInventorying;
    }

    public void setKeyboardEmulation(bool enable, FxwKbd.KeybEmuMode mode, bool addTimestamp)
    {
      if (enable)
      {
        this._keybEmuMode = mode;
        this._keyEmuAddTimestamp = addTimestamp;
        this._keybEmulator = new FxwKbd();
      }
      else
        this._keybEmulator = (FxwKbd) null;
    }

    public byte systemCommand(RfidDefs.SystemFunctions function, byte flags, byte[] value)
    {
      if (!this.isValid())
        return 1;
      byte[] aPayload = new byte[1 + (value != null ? value.Length : 0)];
      aPayload[0] = (byte) function;
      if (value != null)
      {
        int num1 = 0;
        foreach (byte num2 in value)
        {
          aPayload[1 + num1] = num2;
          ++num1;
        }
      }
      this._readerWorker.send(new RfidMsg((byte) 97, flags, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte getParam(RfidDefs.FxwParam aParam)
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 79, (byte) 0, new byte[1]
      {
        (byte) aParam
      }, (byte) 1));
      return 0;
    }

    public byte setParam(RfidDefs.FxwParam aParam, byte[] parValue)
    {
      if (!this.isValid())
        return 1;
      byte[] aPayload = new byte[parValue.Length + 1];
      int num1 = 0;
      byte[] numArray = aPayload;
      int index = num1;
      int num2 = index + 1;
      int num3 = (int) aParam;
      numArray[index] = (byte) num3;
      foreach (byte num4 in parValue)
        aPayload[num2++] = num4;
      this._readerWorker.send(new RfidMsg((byte) 78, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte setRtc(int yy, int MM, int dd, int dow, int hh, int mm, int ss)
    {
      byte[] parValue = new byte[8];
      int num = yy > 2000 ? yy - 2000 : yy;
      parValue[0] = RfidUtils.encodeBcd((byte) num);
      parValue[1] = RfidUtils.encodeBcd((byte) MM);
      parValue[2] = RfidUtils.encodeBcd((byte) dd);
      parValue[3] = RfidUtils.encodeBcd((byte) dow);
      parValue[4] = RfidUtils.encodeBcd((byte) hh);
      parValue[5] = RfidUtils.encodeBcd((byte) mm);
      parValue[6] = RfidUtils.encodeBcd((byte) ss);
      parValue[7] = (byte) 0;
      return this.setParam(RfidDefs.FxwParam.RTC_TIME, parValue);
    }

    public byte setRtc(DateTime aDt)
    {
      int dow = aDt.DayOfWeek.GetHashCode();
      if (dow == 0)
        dow = 7;
      return this.setRtc(aDt.Year, aDt.Month, aDt.Day, dow, aDt.Hour, aDt.Minute, aDt.Second);
    }

    public byte getRtc()
    {
      return this.getParam(RfidDefs.FxwParam.RTC_TIME);
    }

    public byte readDataEpcC1G2(RfidDefs.MemoryBank bank, ushort address, ushort numBytes)
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 99, (byte) 0, new byte[6]
      {
        (byte) 2,
        (byte) bank,
        (byte) ((uint) address / 256U),
        (byte) address,
        (byte) ((uint) numBytes / 256U),
        (byte) numBytes
      }, (byte) 6));
      return 0;
    }

    public byte applyParam()
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 99, (byte) 0, new byte[1]
      {
        (byte) 4
      }, (byte) 1));
      return 0;
    }

    public byte writeDataEpcC1G2(
      RfidDefs.MemoryBank bank,
      ushort address,
      ushort numBytes,
      byte[] data)
    {
      if (!this.isValid())
        return 1;
      byte aPayloadLen = (byte) (6 + data.Length);
      byte[] aPayload = new byte[(int) aPayloadLen];
      aPayload[0] = (byte) 3;
      aPayload[1] = (byte) bank;
      aPayload[2] = (byte) ((uint) address / 256U);
      aPayload[3] = (byte) address;
      aPayload[4] = (byte) ((uint) numBytes / 256U);
      aPayload[5] = (byte) numBytes;
      for (int index = 0; index < data.Length; ++index)
        aPayload[index + 6] = data[index];
      this._readerWorker.send(new RfidMsg((byte) 99, (byte) 0, aPayload, aPayloadLen));
      return 0;
    }

    public byte setAccessPassword(byte[] password)
    {
      if (!this.isValid())
        return 1;
      if (password.Length != 4)
        return 2;
      byte aPayloadLen = 6;
      byte[] aPayload = new byte[(int) aPayloadLen];
      aPayload[0] = (byte) 7;
      aPayload[1] = (byte) 4;
      aPayload[2] = password[0];
      aPayload[3] = password[1];
      aPayload[4] = password[2];
      aPayload[5] = password[3];
      this._readerWorker.send(new RfidMsg((byte) 99, (byte) 0, aPayload, aPayloadLen));
      return 0;
    }

    public byte writeAccessPasswordToTag(byte[] newPassword)
    {
      if (!this.isValid())
        return 1;
      if (newPassword.Length != 4)
        return 2;
      byte aPayloadLen = (byte) (6 + newPassword.Length);
      byte[] aPayload = new byte[(int) aPayloadLen];
      aPayload[0] = (byte) 3;
      aPayload[1] = (byte) 0;
      aPayload[2] = (byte) 0;
      aPayload[3] = (byte) 4;
      aPayload[4] = (byte) 0;
      aPayload[5] = (byte) 4;
      for (int index = 0; index < newPassword.Length; ++index)
        aPayload[index + 6] = newPassword[index];
      this._readerWorker.send(new RfidMsg((byte) 99, (byte) 0, aPayload, aPayloadLen));
      return 0;
    }

    public byte lockUnlockTag(byte[] maskAndLockBits)
    {
      if (!this.isValid())
        return 1;
      if (maskAndLockBits.Length != 4)
        return 2;
      byte aPayloadLen = (byte) (6 + maskAndLockBits.Length);
      byte[] aPayload = new byte[(int) aPayloadLen];
      aPayload[0] = (byte) 3;
      aPayload[1] = (byte) 0;
      aPayload[2] = (byte) 0;
      aPayload[3] = (byte) 0;
      aPayload[4] = (byte) 0;
      aPayload[5] = (byte) 4;
      for (int index = 0; index < maskAndLockBits.Length; ++index)
        aPayload[index + 6] = maskAndLockBits[index];
      this._readerWorker.send(new RfidMsg((byte) 99, (byte) 1, aPayload, aPayloadLen));
      return 0;
    }

    public byte writeTagEpc(byte[] newId)
    {
      if (newId == null || newId.Length == 0 || newId.Length > 62)
        return 2;
      if (!this.isValid())
        return 1;
      byte[] aPayload = new byte[newId.Length + 1];
      aPayload[0] = (byte) 1;
      int num1 = 0;
      foreach (byte num2 in newId)
        aPayload[++num1] = num2;
      this._readerWorker.send(new RfidMsg((byte) 99, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte readBlock(bool addressed, byte[] uid, byte blockNum)
    {
      if (addressed && (uid == null || uid.Length != 8))
        return 2;
      byte[] numArray = (byte[]) null;
      byte num1 = 4;
      byte num2 = 2;
      if (addressed)
      {
        numArray = new byte[uid.Length];
        Array.Copy((Array) uid, (Array) numArray, uid.Length);
        Array.Reverse((Array) numArray);
        num2 |= (byte) 32;
        num1 += (byte) numArray.Length;
      }
      byte[] transpCommand = new byte[(int) num1];
      transpCommand[0] = (byte) 0;
      transpCommand[1] = num2;
      transpCommand[2] = (byte) 32;
      int index = 3;
      if (addressed)
      {
        for (; index < 3 + numArray.Length; ++index)
          transpCommand[index] = numArray[index - 3];
      }
      transpCommand[index] = blockNum;
      return this.sendTransparentIso(transpCommand);
    }

    public byte writeBlock(
      bool addressed,
      byte[] uid,
      byte tagType,
      byte blockNum,
      byte[] blockData)
    {
      if (blockData.Length != 4)
        return byte.MaxValue;
      if (addressed && (uid == null || uid.Length != 8))
        return 2;
      byte[] numArray = (byte[]) null;
      byte num1 = 8;
      byte num2 = 2;
      if (addressed)
      {
        numArray = new byte[uid.Length];
        Array.Copy((Array) uid, (Array) numArray, uid.Length);
        Array.Reverse((Array) numArray);
        num2 |= (byte) 32;
        num1 += (byte) numArray.Length;
      }
      byte[] transpCommand = new byte[(int) num1];
      transpCommand[0] = (byte) 0;
      transpCommand[1] = (byte) ((uint) num2 | (uint) tagType);
      transpCommand[2] = (byte) 33;
      int index = 3;
      if (addressed)
      {
        for (; index < 3 + numArray.Length; ++index)
          transpCommand[index] = numArray[index - 3];
      }
      transpCommand[index] = blockNum;
      transpCommand[index + 1] = blockData[0];
      transpCommand[index + 2] = blockData[1];
      transpCommand[index + 3] = blockData[2];
      transpCommand[index + 4] = blockData[3];
      return this.sendTransparentIso(transpCommand);
    }

    public byte get245AntennaSwitchConfig(byte antenna)
    {
      byte[] aPayload = new byte[2]{ (byte) 5, antenna };
      this._readerWorker.send(new RfidMsg((byte) 99, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte set245AntennaSwitchConfig(byte antenna, RfidDefs.Antenna245Config config)
    {
      byte[] aPayload = RfidUtils.concatArray(new byte[2]
      {
        (byte) 6,
        antenna
      }, config.getRawData());
      this._readerWorker.send(new RfidMsg((byte) 99, (byte) 0, aPayload, (byte) aPayload.Length));
      return 0;
    }

    public byte ap245SendId(
      ushort replyDelayMs,
      byte replyChannel,
      byte repeatNum,
      ushort repeatDelay_ms)
    {
      try
      {
        byte[] bigEndianBytes = RfidUtils.ushortToBigEndianBytes(repeatDelay_ms);
        byte[] data = new byte[3]
        {
          repeatNum,
          bigEndianBytes[0],
          bigEndianBytes[1]
        };
        int num = (int) this.sendTransparentTag245(new RfidTagMsg(RfidTagMsg.TagMsgFlags.BROADCAST, RfidTagMsg.TagMsgCommands.SEND_ID, (byte) 0, replyDelayMs, replyChannel, data));
        return 0;
      }
      catch (ArgumentException ex)
      {
        AbstractRfidReader._logger.debug("ap245SendId error:");
        AbstractRfidReader._logger.debug(ex.Message);
        return 2;
      }
    }

    public byte ap245SetParam(
      RfidTagMsg.TagMsgSetParams param,
      byte[] value,
      ushort replyDelayMs,
      byte replyChannel,
      byte[] tagId)
    {
      try
      {
        RfidTagMsg tagMsg;
        if (tagId != null)
        {
          byte[] data = new byte[12];
          data[0] = (byte) param;
          Buffer.BlockCopy((Array) value, 0, (Array) data, 1, value.Length);
          tagMsg = new RfidTagMsg(RfidTagMsg.TagMsgFlags.ADDRESSED, RfidTagMsg.TagMsgCommands.SET_PARAM, (byte) 0, replyDelayMs, replyChannel, data, tagId);
        }
        else
        {
          byte[] data = new byte[24];
          data[0] = (byte) param;
          Buffer.BlockCopy((Array) value, 0, (Array) data, 1, value.Length);
          tagMsg = new RfidTagMsg(RfidTagMsg.TagMsgFlags.BROADCAST, RfidTagMsg.TagMsgCommands.SET_PARAM, (byte) 0, replyDelayMs, replyChannel, data);
        }
        int num = (int) this.sendTransparentTag245(tagMsg);
        return 0;
      }
      catch (ArgumentException ex)
      {
        AbstractRfidReader._logger.debug("ap245SetParam error:");
        AbstractRfidReader._logger.debug(ex.Message);
        return 2;
      }
    }

    public byte ap245SetMultiParam(
      List<RfidTagMsg.TagMsgSetParams> paramsList,
      List<byte[]> valuesList,
      ushort replyDelayMs,
      byte replyChannel,
      byte[] tagId)
    {
      int length = tagId == null ? 24 : 12;
      int count = paramsList.Count;
      int num1 = 0;
      foreach (byte[] values in valuesList)
        num1 += values.Length;
      if (count + num1 > length)
        return 2;
      byte[] data = new byte[length];
      int num2 = 0;
      for (int index = 0; index < count; ++index)
      {
        data[num2++] = (byte) paramsList[index];
        foreach (byte num3 in valuesList[index])
          data[num2++] = num3;
      }
      try
      {
        int num3 = (int) this.sendTransparentTag245(tagId == null ? new RfidTagMsg(RfidTagMsg.TagMsgFlags.BROADCAST, RfidTagMsg.TagMsgCommands.SET_PARAM, (byte) 0, replyDelayMs, replyChannel, data) : new RfidTagMsg(RfidTagMsg.TagMsgFlags.ADDRESSED, RfidTagMsg.TagMsgCommands.SET_PARAM, (byte) 0, replyDelayMs, replyChannel, data, tagId));
        return 0;
      }
      catch (ArgumentException ex)
      {
        AbstractRfidReader._logger.debug("ap245SetMultiParam error:");
        AbstractRfidReader._logger.debug(ex.Message);
        return 2;
      }
    }

    public byte ap245GetParam(
      RfidTagMsg.TagMsgGetParams param,
      ushort replyDelayMs,
      byte replyChannel,
      byte[] tagId)
    {
      try
      {
        if (tagId == null)
          throw new ArgumentException("Invalid null parameter", nameof (tagId));
        int num = (int) this.sendTransparentTag245(new RfidTagMsg(RfidTagMsg.TagMsgFlags.ADDRESSED, RfidTagMsg.TagMsgCommands.GET_PARAM, (byte) 0, replyDelayMs, replyChannel, new byte[1]
        {
          (byte) param
        }, tagId));
        return 0;
      }
      catch (ArgumentException ex)
      {
        AbstractRfidReader._logger.debug("ap245SetParam error:");
        AbstractRfidReader._logger.debug(ex.Message);
        return 2;
      }
    }

    public byte ap245GetLog(ushort logNum, ushort replyDelayMs, byte replyChannel, byte[] tagId)
    {
      try
      {
        if (tagId == null)
          throw new ArgumentException("Invalid null parameter", nameof (tagId));
        int num = (int) this.sendTransparentTag245(new RfidTagMsg(RfidTagMsg.TagMsgFlags.ADDRESSED, RfidTagMsg.TagMsgCommands.GET_LOG, (byte) 0, replyDelayMs, replyChannel, RfidUtils.ushortToBigEndianBytes(logNum), tagId));
        return 0;
      }
      catch (ArgumentException ex)
      {
        AbstractRfidReader._logger.debug("ap245GetLog error:");
        AbstractRfidReader._logger.debug(ex.Message);
        return 2;
      }
    }

    public byte ap245DelLog(ushort logNum, ushort replyDelayMs, byte replyChannel, byte[] tagId)
    {
      try
      {
        if (tagId == null)
          throw new ArgumentException("Invalid null parameter", nameof (tagId));
        int num = (int) this.sendTransparentTag245(new RfidTagMsg(RfidTagMsg.TagMsgFlags.ADDRESSED, RfidTagMsg.TagMsgCommands.DEL_LOG, (byte) 0, replyDelayMs, replyChannel, RfidUtils.ushortToBigEndianBytes(logNum), tagId));
        return 0;
      }
      catch (ArgumentException ex)
      {
        AbstractRfidReader._logger.debug("ap245DelLog error:");
        AbstractRfidReader._logger.debug(ex.Message);
        return 2;
      }
    }

    public byte ap245DelAllLog(ushort replyDelayMs, byte replyChannel, byte[] tagId)
    {
      try
      {
        if (tagId == null)
          throw new ArgumentException("Invalid null parameter", nameof (tagId));
        int num = (int) this.sendTransparentTag245(new RfidTagMsg(RfidTagMsg.TagMsgFlags.ADDRESSED, RfidTagMsg.TagMsgCommands.DEL_ALL_LOG, (byte) 0, replyDelayMs, replyChannel, (byte[]) null, tagId));
        return 0;
      }
      catch (ArgumentException ex)
      {
        AbstractRfidReader._logger.debug("ap245DelAllLog error:");
        AbstractRfidReader._logger.debug(ex.Message);
        return 2;
      }
    }

    public byte ap245TurnLedOn(
      RfidTagMsg.TagLed led,
      ushort timeOn_ms,
      ushort timeOff_ms,
      byte blinkNumber,
      ushort replyDelayMs,
      byte replyChannel,
      byte[] tagId)
    {
      try
      {
        if (tagId == null)
          throw new ArgumentException("Invalid null parameter", nameof (tagId));
        byte[] bigEndianBytes1 = RfidUtils.ushortToBigEndianBytes(timeOn_ms);
        byte[] bigEndianBytes2 = RfidUtils.ushortToBigEndianBytes(timeOff_ms);
        byte[] data = new byte[6]
        {
          (byte) led,
          bigEndianBytes2[0],
          bigEndianBytes2[1],
          bigEndianBytes1[0],
          bigEndianBytes1[1],
          blinkNumber
        };
        int num = (int) this.sendTransparentTag245(new RfidTagMsg(RfidTagMsg.TagMsgFlags.ADDRESSED, RfidTagMsg.TagMsgCommands.SET_LED_ON, (byte) 0, replyDelayMs, replyChannel, data, tagId));
        return 0;
      }
      catch (ArgumentException ex)
      {
        AbstractRfidReader._logger.debug("ap245DelAllLog error:");
        AbstractRfidReader._logger.debug(ex.Message);
        return 2;
      }
    }

    public byte ap245EraseAllUsers(ushort replyDelayMs, byte replyChannel, byte[] tagId)
    {
      try
      {
        if (tagId == null)
          throw new ArgumentException("Invalid null parameter", nameof (tagId));
        int num = (int) this.sendTransparentTag245(new RfidTagMsg(RfidTagMsg.TagMsgFlags.ADDRESSED, RfidTagMsg.TagMsgCommands.ERASE_ALL_USERS, (byte) 0, replyDelayMs, replyChannel, (byte[]) null, tagId));
        return 0;
      }
      catch (ArgumentException ex)
      {
        AbstractRfidReader._logger.debug("ap245EraseAllLogs error:");
        AbstractRfidReader._logger.debug(ex.Message);
        return 2;
      }
    }

    private void identifyDevice(int timout_ms)
    {
      int num1 = 5;
      int millisecondsTimeout = timout_ms / num1;
      this._readerInfo.resetStatus();
      AbstractRfidReader._logger.debug("[RfidReader] Requesting devices info...");
      if (!(this._readerInfo.Status != "VALID"))
        return;
      AbstractRfidReader._logger.debug("");
      int num2 = (int) this.getParam(RfidDefs.FxwParam.DEV_TYPE);
      AbstractRfidReader._logger.debug("[RfidReader] DEV_TYPE requested");
      for (int index = num1; this._readerInfo.Model == "UNDEFINED" && index > 0; --index)
        Thread.Sleep(millisecondsTimeout);
      if (this._readerInfo.Model == "UNDEFINED")
      {
        AbstractRfidReader._logger.debug("[RfidReader] DEV_TYPE request failed");
      }
      else
      {
        AbstractRfidReader._logger.debug("");
        int num3 = (int) this.getParam(RfidDefs.FxwParam.DEV_CODE);
        AbstractRfidReader._logger.debug("[RfidReader] DEV_CODE requested");
        for (int index = num1; this._readerInfo.SerialNumber == "UNDEFINED" && index > 0; --index)
          Thread.Sleep(millisecondsTimeout);
        if (this._readerInfo.SerialNumber == "UNDEFINED")
        {
          AbstractRfidReader._logger.debug("[RfidReader] DEV_CODE request failed");
        }
        else
        {
          AbstractRfidReader._logger.debug("");
          int num4 = (int) this.getParam(RfidDefs.FxwParam.FW_RELEASE);
          AbstractRfidReader._logger.debug("[RfidReader] FW_RELEASE requested");
          for (int index = num1; this._readerInfo.Firmware == "UNDEFINED" && index > 0; --index)
            Thread.Sleep(millisecondsTimeout);
          if (this._readerInfo.Firmware == "UNDEFINED")
            AbstractRfidReader._logger.debug("[RfidReader] FW_RELEASE request failed");
          this._readerInfo.checkStatus();
        }
      }
    }

    public byte sendTransparentTag245(RfidTagMsg tagMsg)
    {
      if (!this.isValid())
        return 1;
      byte[] byteArray = tagMsg.toByteArray();
      byte[] aPayload = new byte[1 + byteArray.Length];
      aPayload[0] = (byte) 75;
      Array.Copy((Array) byteArray, 0, (Array) aPayload, 1, byteArray.Length);
      byte aCmdFlags = 0;
      if (tagMsg.Flags == RfidTagMsg.TagMsgFlags.ADDRESSED)
        aCmdFlags = (byte) 16;
      this._readerWorker.send(new RfidMsg((byte) 75, aCmdFlags, aPayload, (byte) aPayload.Length));
      return 0;
    }

    private byte sendTransparentIso(byte[] transpCommand)
    {
      if (!this.isValid())
        return 1;
      this._readerWorker.send(new RfidMsg((byte) 74, (byte) 0, transpCommand, (byte) transpCommand.Length));
      return 0;
    }

    protected virtual void onWorkerReceivedData(RfidMsg rcvMsg)
    {
      switch (rcvMsg.CommandIdentifier)
      {
        case 79:
          AbstractRfidReader._logger.debug("[RfidReader] Got parameter 0x" + rcvMsg.getPayloadByte(0).ToString("X2") + " (" + (object) rcvMsg.getPayloadLen() + " bytes)");
          switch (rcvMsg.getPayloadByte(0))
          {
            case 7:
              AbstractRfidReader._logger.debug("[RfidReader] DEV_CODE received");
              this._readerInfo.SerialNumber = rcvMsg.getPayloadHexString(true);
              break;
            case 11:
              AbstractRfidReader._logger.debug("[RfidReader] FW_RELEASE received");
              this._readerInfo.Firmware = rcvMsg.getPayloadString();
              break;
            case 14:
              AbstractRfidReader._logger.debug("[RfidReader] DEV_TYPE received");
              this._readerInfo.Model = rcvMsg.getPayloadHexString(true);
              break;
          }
          if (this.receivedData == null)
            break;
          this.receivedData(rcvMsg);
          break;
        case 224:
          if (this._keybEmulator != null)
            this.emulateKeyboard(rcvMsg.getPayloadHexString(false));
          if (this.receivedData == null)
            break;
          this.receivedData(rcvMsg);
          break;
        case 230:
          AbstractRfidReader._logger.debug("[RfidReader] 245 Tag message received");
          this.receivedTagData(new RfidTagReply(rcvMsg.getPayload()));
          break;
        default:
          if (this.receivedData == null)
            break;
          AbstractRfidReader._logger.debug("[RfidReader] CmdId: " + rcvMsg.CommandIdentifier.ToString("X2") + " Flags: " + rcvMsg.CommandFlags.ToString("X2") + " Payload: " + rcvMsg.getPayloadHexString(false));
          this.receivedData(rcvMsg);
          break;
      }
    }

    public enum InventoryMode : byte
    {
      SINGLE_ROUND,
      START_LOOP,
      STOP_LOOP,
    }

    public enum TypeIO : byte
    {
      LED,
      BUZZER,
      OUT,
    }

    public enum LedIO : byte
    {
      ORANGE,
      RED,
      GREEN,
      BLUE,
    }

    public enum CommandIO : byte
    {
      OPENDOOR,
    }

    public enum BuzzerIO : byte
    {
      BUZZZ,
    }

    public enum ModeIO : byte
    {
      ON,
      OFF,
      BLINK,
      PULSE,
      PULSE_LOOP,
    }

    public delegate void ReceivedDataHandler(RfidMsg aMsg);

    public delegate void ReceiveTagDataHandler(RfidTagReply aMsg);
  }
}
