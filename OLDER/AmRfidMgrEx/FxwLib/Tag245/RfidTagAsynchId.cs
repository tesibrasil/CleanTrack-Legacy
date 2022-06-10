// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.Tag245.RfidTagAsynchId
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

using System;

namespace It.IDnova.Fxw.Tag245
{
  public class RfidTagAsynchId
  {
    private static FxwLog _logger = FxwLog.getInstance();
    public const int MAX_DATA_LEN_BYTES = 12;
    public const int MSG_LEN_BYTES = 13;
    public const int TAG_TYPE_ID12_START_BYTE = 0;
    public const int TAG_TYPE_TEMP_ID8_START_BYTE = 4;
    private RfidTagAsynchId.TagType _type;
    private byte[] _data;

    public RfidTagAsynchId.TagType Type
    {
      get
      {
        return this._type;
      }
    }

    public byte[] Data
    {
      get
      {
        return this._data;
      }
    }

    public RfidTagAsynchId(byte[] data)
    {
      if (data.Length != 13)
        throw new ArgumentException("Invalid data buffer", "data (length)");
      this._type = (RfidTagAsynchId.TagType) data[0];
      if (!this.isValidType())
        throw new ArgumentException("Invalid data buffer", "type");
      this._data = new byte[12];
      for (int index = 0; index < 12; ++index)
        this._data[index] = data[1 + index];
    }

    public byte[] getTagId()
    {
      byte[] numArray = (byte[]) null;
      switch (this._type)
      {
        case RfidTagAsynchId.TagType.TYPE_ID12:
          numArray = new byte[12];
          for (int index = 0; index < 12; ++index)
            numArray[index] = this._data[index];
          break;
        case RfidTagAsynchId.TagType.TYPE_TEMP_ID8:
        case RfidTagAsynchId.TagType.TYPE_TEMP_RANGE_INT_ID8_LOG:
        case RfidTagAsynchId.TagType.TYPE_TEMP_RANGE_EXT_ID8_LOG:
          numArray = new byte[8];
          for (int index = 4; index < 12; ++index)
            numArray[index - 4] = this._data[index];
          break;
      }
      return numArray;
    }

    public string getTagIdAsHex()
    {
      string str = "";
      foreach (byte num in this.getTagId())
        str += num.ToString("X2");
      return str;
    }

    public float getInstantTemp()
    {
      switch (this._type)
      {
        case RfidTagAsynchId.TagType.TYPE_TEMP_ID8:
        case RfidTagAsynchId.TagType.TYPE_TEMP_RANGE_INT_ID8_LOG:
        case RfidTagAsynchId.TagType.TYPE_TEMP_RANGE_EXT_ID8_LOG:
          return (float) RfidUtils.twoComplement14BitsToShort((ushort) (((uint) this.Data[1] << 8) + (uint) this.Data[0])) / 32f;
        default:
          throw new InvalidOperationException("Temperature unavailable for type " + (object) this._type + " tags");
      }
    }

    public void log()
    {
      RfidTagAsynchId._logger.debug("> RFID TAG ASYNCH REPLY");
      RfidTagAsynchId._logger.debug("> Type: " + (object) this.Type + " (0x" + ((byte) this.Type).ToString("X2") + ")");
      string msg = "> Data: ";
      for (int index = 0; index < 12; ++index)
        msg = msg + "0x" + this.Data[index].ToString("X2") + " ";
      RfidTagAsynchId._logger.debug(msg);
      if (this._type != RfidTagAsynchId.TagType.TYPE_TEMP_ID8 && this._type != RfidTagAsynchId.TagType.TYPE_TEMP_RANGE_INT_ID8_LOG && this._type != RfidTagAsynchId.TagType.TYPE_TEMP_RANGE_EXT_ID8_LOG)
        return;
      RfidTagAsynchId._logger.debug("> Temp: " + (object) this.getInstantTemp());
    }

    private bool isValidType()
    {
      return this._type == RfidTagAsynchId.TagType.TYPE_ID12 || this._type == RfidTagAsynchId.TagType.TYPE_TEMP_ID8 || (this._type == RfidTagAsynchId.TagType.TYPE_TEMP_RANGE_INT_ID8_LOG || this._type == RfidTagAsynchId.TagType.TYPE_TEMP_RANGE_EXT_ID8_LOG);
    }

    public enum TagType
    {
      TYPE_ID12,
      TYPE_TEMP_ID8,
      TYPE_TEMP_RANGE_INT_ID8_LOG,
      TYPE_TEMP_RANGE_EXT_ID8_LOG,
    }
  }
}
