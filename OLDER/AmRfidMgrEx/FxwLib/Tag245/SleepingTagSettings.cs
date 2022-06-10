// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.Tag245.SleepingTagSettings
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

using System;

namespace It.IDnova.Fxw.Tag245
{
  public class SleepingTagSettings
  {
    private static ushort _RxRate;
    private static ushort _TxRate;
    private static ushort _OnRate;
    private static ushort _RxWindow;
    private static byte _RfPower;
    private static uint _UTCTime;
    private static uint _AlarmTime;

    public ushort RxRate
    {
      get
      {
        return SleepingTagSettings._RxRate;
      }
      set
      {
        SleepingTagSettings._RxRate = value;
      }
    }

    public ushort TxRate
    {
      get
      {
        return SleepingTagSettings._TxRate;
      }
      set
      {
        SleepingTagSettings._TxRate = value;
      }
    }

    public ushort OnRate
    {
      get
      {
        return SleepingTagSettings._OnRate;
      }
      set
      {
        SleepingTagSettings._OnRate = value;
      }
    }

    public ushort RxWindow
    {
      get
      {
        return SleepingTagSettings._RxWindow;
      }
      set
      {
        SleepingTagSettings._RxWindow = value;
      }
    }

    public byte RfPower
    {
      get
      {
        return SleepingTagSettings._RfPower;
      }
      set
      {
        SleepingTagSettings._RfPower = value;
      }
    }

    public uint UTCTime
    {
      get
      {
        return SleepingTagSettings._UTCTime;
      }
      set
      {
        SleepingTagSettings._UTCTime = value;
      }
    }

    public uint AlarmTime
    {
      get
      {
        return SleepingTagSettings._AlarmTime;
      }
      set
      {
        SleepingTagSettings._AlarmTime = value;
      }
    }

    public SleepingTagSettings()
    {
    }

    public SleepingTagSettings(RfidTagReply msg)
    {
      if (msg.Data.Length != 24)
        throw new ArgumentException("Invalid data length");
      SleepingTagSettings._RxRate = RfidUtils.littleEndianBytesToUint16(new byte[2]
      {
        msg.Data[0],
        msg.Data[1]
      });
      SleepingTagSettings._TxRate = RfidUtils.littleEndianBytesToUint16(new byte[2]
      {
        msg.Data[2],
        msg.Data[3]
      });
      SleepingTagSettings._OnRate = RfidUtils.littleEndianBytesToUint16(new byte[2]
      {
        msg.Data[4],
        msg.Data[5]
      });
      SleepingTagSettings._RxWindow = RfidUtils.littleEndianBytesToUint16(new byte[2]
      {
        msg.Data[6],
        msg.Data[7]
      });
      SleepingTagSettings._RfPower = msg.Data[8];
      SleepingTagSettings._UTCTime = RfidUtils.littleEndianBytesToUInt32(new byte[4]
      {
        msg.Data[9],
        msg.Data[10],
        msg.Data[11],
        msg.Data[12]
      });
      SleepingTagSettings._AlarmTime = RfidUtils.littleEndianBytesToUInt32(new byte[4]
      {
        msg.Data[13],
        msg.Data[14],
        msg.Data[15],
        msg.Data[16]
      });
    }
  }
}
