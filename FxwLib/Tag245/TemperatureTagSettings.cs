using System;

namespace It.IDnova.Fxw.Tag245
{
  public class TemperatureTagSettings
  {
    private byte _LogMode = 0;
    private short _LT_Temp = 0;
    private short _GT_Temp = 0;
    private ushort _TempRate = 0;
    private ushort _TxRate = 0;
    private ushort _RxRate = 0;
    private ushort _RxWindow = 0;
    private byte _RfPower = 0;
    private bool _LogEnable = false;
    private uint _LogNumber = 0;
    private uint _UTCTime = 0;

    public byte LogMode
    {
      get
      {
        return this._LogMode;
      }
      set
      {
        this._LogMode = value;
      }
    }

    public short LessThanTemperature
    {
      get
      {
        return this._LT_Temp;
      }
      set
      {
        this._LT_Temp = value;
      }
    }

    public short GreaterThanTemperature
    {
      get
      {
        return this._GT_Temp;
      }
      set
      {
        this._GT_Temp = value;
      }
    }

    public ushort TemperatureRate
    {
      get
      {
        return this._TempRate;
      }
      set
      {
        this._TempRate = value;
      }
    }

    public ushort RxRate
    {
      get
      {
        return this._RxRate;
      }
      set
      {
        this._RxRate = value;
      }
    }

    public ushort TxRate
    {
      get
      {
        return this._TxRate;
      }
      set
      {
        this._TxRate = value;
      }
    }

    public ushort RxWindow
    {
      get
      {
        return this._RxWindow;
      }
      set
      {
        this._RxWindow = value;
      }
    }

    public uint LogNumber
    {
      get
      {
        return this._LogNumber;
      }
      set
      {
        this._LogNumber = value;
      }
    }

    public uint UTCTime
    {
      get
      {
        return this._UTCTime;
      }
      set
      {
        this._UTCTime = value;
      }
    }

    public byte RfPower
    {
      get
      {
        return this._RfPower;
      }
      set
      {
        this._RfPower = value;
      }
    }

    public bool LogEnable
    {
      get
      {
        return this._LogEnable;
      }
      set
      {
        this._LogEnable = value;
      }
    }

    public TemperatureTagSettings(RfidTagReply msg)
    {
      if (msg.Data.Length != 24)
        throw new ArgumentException("Invalid data length");
      this._LogMode = msg.Data[0];
      this._LT_Temp = (short) ((int) RfidUtils.littleEndianBytesToInt16(new byte[2]
      {
        msg.Data[1],
        msg.Data[2]
      }) / 128);
      this._GT_Temp = (short) ((int) RfidUtils.littleEndianBytesToInt16(new byte[2]
      {
        msg.Data[3],
        msg.Data[4]
      }) / 128);
      this._TempRate = RfidUtils.littleEndianBytesToUint16(new byte[2]
      {
        msg.Data[5],
        msg.Data[6]
      });
      this._TxRate = RfidUtils.littleEndianBytesToUint16(new byte[2]
      {
        msg.Data[7],
        msg.Data[8]
      });
      this._RxRate = RfidUtils.littleEndianBytesToUint16(new byte[2]
      {
        msg.Data[9],
        msg.Data[10]
      });
      this._RxWindow = RfidUtils.littleEndianBytesToUint16(new byte[2]
      {
        msg.Data[11],
        msg.Data[12]
      });
      this._RfPower = msg.Data[13];
      this._LogEnable = (uint) msg.Data[14] > 0U;
      this._LogNumber = RfidUtils.littleEndianBytesToUInt32(new byte[4]
      {
        msg.Data[15],
        msg.Data[16],
        msg.Data[17],
        msg.Data[18]
      });
      this._UTCTime = RfidUtils.littleEndianBytesToUInt32(new byte[4]
      {
        msg.Data[19],
        msg.Data[20],
        msg.Data[21],
        msg.Data[22]
      });
    }
  }
}
