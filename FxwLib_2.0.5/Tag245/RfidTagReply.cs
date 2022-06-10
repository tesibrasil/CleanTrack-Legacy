// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.Tag245.RfidTagReply
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

using System;

namespace It.IDnova.Fxw.Tag245
{
  public class RfidTagReply
  {
    private static FxwLog _logger = FxwLog.getInstance();
    public const int MAX_DATA_LEN_BYTES = 24;
    public const int MSG_LEN_BYTES = 27;
    private byte _flags;
    private byte _progr;
    private byte _inc;
    private byte[] _data;

    public RfidTagReply(byte[] data)
    {
      if (data.Length != 27)
        throw new ArgumentException("Invalid data buffer", "data (length)");
      this._flags = data[0];
      this._progr = data[1];
      this._inc = data[2];
      this._data = new byte[24];
      for (int index = 0; index < 24; ++index)
        this._data[index] = data[3 + index];
    }

    public byte Flags
    {
      get
      {
        return this._flags;
      }
    }

    public byte Progr
    {
      get
      {
        return this._progr;
      }
    }

    public byte Increment
    {
      get
      {
        return this._inc;
      }
    }

    public byte[] Data
    {
      get
      {
        return this._data;
      }
    }

    public void log()
    {
      RfidTagReply._logger.debug("> RFID TAG REPLY");
      RfidTagReply._logger.debug("> Flags:   0x" + this.Flags.ToString("X2"));
      RfidTagReply._logger.debug("> Progr:   0x" + this.Progr.ToString("X2"));
      RfidTagReply._logger.debug("> Incr:    0x" + this.Increment.ToString("X2"));
      string msg = "> Data:    ";
      for (int index = 0; index < 24; ++index)
        msg = msg + "0x" + this.Data[index].ToString("X2") + " ";
      RfidTagReply._logger.debug(msg);
    }

    public enum ReplyFlags : byte
    {
      SUCCESS_DEFAULT = 0,
      SUCCESS_LOG = 240, // 0xF0
      SUCCESS_SERVICE = 254, // 0xFE
    }
  }
}
