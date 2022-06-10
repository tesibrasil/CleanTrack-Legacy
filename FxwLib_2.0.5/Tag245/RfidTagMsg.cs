// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.Tag245.RfidTagMsg
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

using System;

namespace It.IDnova.Fxw.Tag245
{
  public class RfidTagMsg
  {
    private static FxwLog _logger = FxwLog.getInstance();
    internal const int MAX_ADDRESSED_DATA_LEN_BYTES = 12;
    internal const int MAX_BROADCAST_DATA_LEN_BYTES = 24;
    internal const int MAX_TAGID_LEN_BYTES = 12;
    internal const int MSG_LEN_BYTES = 30;
    private RfidTagMsg.TagMsgFlags _flags;
    private byte _cmdId;
    private byte _progr;
    private ushort _replyDelayMs;
    private byte _replyChannel;
    private byte[] _data;
    private byte[] _tagId;

    public RfidTagMsg.TagMsgFlags Flags
    {
      get
      {
        return this._flags;
      }
    }

    public RfidTagMsg.TagMsgCommands CommandIdentifier
    {
      get
      {
        return (RfidTagMsg.TagMsgCommands) this._cmdId;
      }
    }

    public byte Progr
    {
      get
      {
        return this._progr;
      }
    }

    public ushort ReplyDelayMs
    {
      get
      {
        return this._replyDelayMs;
      }
    }

    public byte ReplyChannel
    {
      get
      {
        return this._replyChannel;
      }
    }

    public byte[] Data
    {
      get
      {
        return this._data;
      }
    }

    public byte[] TagId
    {
      get
      {
        return this._tagId;
      }
    }

    public void log()
    {
      RfidTagMsg._logger.debug("> RFID TAG MESSAGE");
      RfidTagMsg._logger.debug("> Flags:   0x" + this.Flags.ToString("X2"));
      RfidTagMsg._logger.debug("> CmdId:   0x" + this.CommandIdentifier.ToString("X2"));
      RfidTagMsg._logger.debug("> Progr:   0x" + this.Progr.ToString("X2"));
      RfidTagMsg._logger.debug("> DelayMs: 0x" + this.ReplyDelayMs.ToString("X4"));
      RfidTagMsg._logger.debug("> ReplyCh: 0x" + this.ReplyChannel.ToString("X2"));
      string msg1 = "> Data:    ";
      for (int index = 0; index < 12; ++index)
        msg1 = msg1 + "0x" + this.Data[index].ToString("X2") + " ";
      RfidTagMsg._logger.debug(msg1);
      if (this._tagId == null)
        return;
      string msg2 = "> TagId:   ";
      for (int index = 0; index < 12; ++index)
        msg2 = msg2 + "0x" + this.TagId[index].ToString("X2") + " ";
      RfidTagMsg._logger.debug(msg2);
    }

    internal RfidTagMsg(
      RfidTagMsg.TagMsgFlags flags,
      RfidTagMsg.TagMsgCommands cmdId,
      byte progr,
      ushort replyDelayMs,
      byte replyChannel,
      byte[] data,
      byte[] tagId)
      : this(flags, (byte) cmdId, progr, replyDelayMs, replyChannel, data, tagId)
    {
    }

    public RfidTagMsg(
      RfidTagMsg.TagMsgFlags flags,
      byte cmdId,
      byte progr,
      ushort replyDelayMs,
      byte replyChannel,
      byte[] data,
      byte[] tagId)
    {
      if ((flags & RfidTagMsg.TagMsgFlags.ADDRESSED) == RfidTagMsg.TagMsgFlags.BROADCAST)
        throw new ArgumentException("Invalid parameter flags (ADDRESSED bit must be 1)");
      this._flags = flags;
      this._cmdId = cmdId;
      this._progr = progr;
      this._replyDelayMs = replyDelayMs;
      this._replyChannel = replyChannel;
      if (data != null && data.Length <= 12)
      {
        this._data = new byte[12];
        for (int index = 0; index < data.Length; ++index)
          this._data[index] = data[index];
        this.zeroDataAddressed(data.Length);
      }
      else
      {
        if (data != null)
          throw new ArgumentException("Invalid parameter lenght", nameof (data));
        this._data = new byte[12];
        this.zeroDataAddressed(0);
      }
      if (tagId != null && tagId.Length <= 12)
      {
        this._tagId = new byte[12];
        for (int index = 0; index < tagId.Length; ++index)
          this._tagId[index] = tagId[index];
        this.zeroTagId(tagId.Length);
      }
      else
      {
        if (tagId != null)
          throw new ArgumentException("Invalid parameter lenght", nameof (tagId));
        this._tagId = new byte[12];
        this.zeroTagId(0);
      }
    }

    internal RfidTagMsg(
      RfidTagMsg.TagMsgFlags flags,
      RfidTagMsg.TagMsgCommands cmdId,
      byte progr,
      ushort replyDelayMs,
      byte replyChannel,
      byte[] data)
      : this(flags, (byte) cmdId, progr, replyDelayMs, replyChannel, data)
    {
    }

    public RfidTagMsg(
      RfidTagMsg.TagMsgFlags flags,
      byte cmdId,
      byte progr,
      ushort replyDelayMs,
      byte replyChannel,
      byte[] data)
    {
      if ((flags & RfidTagMsg.TagMsgFlags.ADDRESSED) != RfidTagMsg.TagMsgFlags.BROADCAST)
        throw new ArgumentException("Invalid parameter flags (ADDRESSED bit must be 0)");
      this._flags = flags;
      this._cmdId = cmdId;
      this._progr = progr;
      this._replyDelayMs = replyDelayMs;
      this._replyChannel = replyChannel;
      this._tagId = (byte[]) null;
      if (data != null && data.Length <= 24)
      {
        this._data = new byte[24];
        for (int index = 0; index < data.Length; ++index)
          this._data[index] = data[index];
        this.zeroDataBroadcast(data.Length);
      }
      else
      {
        if (data != null)
          throw new ArgumentException("Invalid parameter lenght", nameof (data));
        this._data = new byte[24];
        this.zeroDataBroadcast(0);
      }
    }

    internal byte[] toByteArray()
    {
      byte[] numArray = new byte[30];
      numArray[0] = (byte) this._flags;
      numArray[1] = this._cmdId;
      numArray[2] = this._progr;
      numArray[3] = (byte) ((uint) this._replyDelayMs >> 8);
      numArray[4] = (byte) this._replyDelayMs;
      numArray[5] = this._replyChannel;
      Array.Copy((Array) this._data, 0, (Array) numArray, 6, this._data.Length);
      if (this._tagId != null)
        Array.Copy((Array) this._tagId, 0, (Array) numArray, 18, this._tagId.Length);
      return numArray;
    }

    private void zeroDataAddressed(int startByte)
    {
      for (int index = startByte; index < 12; ++index)
        this._data[index] = (byte) 0;
    }

    private void zeroDataBroadcast(int startByte)
    {
      for (int index = startByte; index < 24; ++index)
        this._data[index] = (byte) 0;
    }

    private void zeroTagId(int startByte)
    {
      for (int index = startByte; index < 12; ++index)
        this._tagId[index] = (byte) 0;
    }

    public enum TagMsgFlags : byte
    {
      BROADCAST,
      ADDRESSED,
    }

    public enum TagMsgCommands : byte
    {
      SET_PARAM = 1,
      GET_PARAM = 2,
      GET_LOG = 3,
      DEL_LOG = 4,
      SEND_ID = 5,
      DEL_ALL_LOG = 6,
      BULK_UPLOAD = 7,
      SET_LED_ON = 8,
      ERASE_ALL_USERS = 9,
    }

    public enum TagMsgSetParams : byte
    {
      RF_RATE = 26, // 0x1A
      RF_POWER = 132, // 0x84
      RF_ADD_LEN = 133, // 0x85
      RF_TX_ADD = 134, // 0x86
      RF_RX_ADD = 135, // 0x87
      RF_TX_CH = 145, // 0x91
      RF_RX_CH = 146, // 0x92
      TAD_ID = 147, // 0x93
      RF_RX_RATE = 148, // 0x94
      RF_TX_RATE = 149, // 0x95
      RF_RX_WINDOW = 150, // 0x96
      TEMP_LOG_MODE = 154, // 0x9A
      TEMP_THR_LT = 155, // 0x9B
      TEMP_THR_GT = 156, // 0x9C
      TEMP_RATE = 157, // 0x9D
      RTC_TIME = 160, // 0xA0
      TEMP_LOG_ENABLE = 163, // 0xA3
      UTC_TIME = 164, // 0xA4
      SENSIBILITY_A = 165, // 0xA5
      SLEEPING_ALARM_TIME = 165, // 0xA5
      SENSIBILITY_B = 166, // 0xA6
      SENSIBILITY_MODE = 167, // 0xA7
      SLEEPING_ON_RATE = 167, // 0xA7
      ATTENUATION_1 = 168, // 0xA8
      ATTENUATION_2 = 169, // 0xA9
      ATTENUATION_3 = 170, // 0xAA
      TIME_OK = 172, // 0xAC
      TIME_ERROR = 173, // 0xAD
      SYNC_DONE = 174, // 0xAE
    }

    public enum TagMsgGetParams : byte
    {
      SETTINGS = 162, // 0xA2
      HASH = 171, // 0xAB
    }

    public enum TagLed : byte
    {
      GREEN,
      RED,
    }
  }
}
