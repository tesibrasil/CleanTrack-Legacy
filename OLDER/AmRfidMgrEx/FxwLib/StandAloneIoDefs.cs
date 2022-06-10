// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.StandAloneIoDefs
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

using System;

namespace It.IDnova.Fxw
{
  public class StandAloneIoDefs
  {
    public enum IoType : byte
    {
      OUTPUT,
      INPUT,
    }

    public enum ConfigIoOp : byte
    {
      READ,
      WRITE,
      ACTIVATE_OUT,
    }

    public abstract class IoSettings
    {
      private StandAloneIoDefs.IoType _flags;
      private StandAloneIoDefs.ConfigIoOp _operation;
      private byte _channel;

      public IoSettings(RfidMsg msg)
      {
        if ((int) msg.CommandIdentifier != 81)
          throw new ArgumentException("Invalid message: must have CommandIdentifier SA_CONFIG_IO (0x51)");
        this._flags = (StandAloneIoDefs.IoType) msg.CommandFlags;
      }

      internal IoSettings(StandAloneIoDefs.IoType flags, StandAloneIoDefs.ConfigIoOp operation, byte channel)
      {
        this._flags = flags;
        this._operation = operation;
        this._channel = channel;
      }

      internal StandAloneIoDefs.IoType getFlags()
      {
        return this._flags;
      }

      internal virtual byte[] getRawData()
      {
        return new byte[2]
        {
          (byte) this._operation,
          this._channel
        };
      }
    }

    public enum EventFrontBitMask : byte
    {
      NONE,
      LOW_HIGH,
      HIGH_LOW,
      BOTH,
    }

    public class InputSettings : StandAloneIoDefs.IoSettings
    {
      private ushort _timeHold;
      private ushort _timeUp;
      private ushort _timeDown;
      private StandAloneIoDefs.EventFrontBitMask _eventFront;

      public InputSettings(RfidMsg msg)
        : base(msg)
      {
        this._timeUp = RfidUtils.bigEndianBytesToUInt16(new byte[2]
        {
          msg.getPayloadByte(0),
          msg.getPayloadByte(1)
        });
        this._timeDown = RfidUtils.bigEndianBytesToUInt16(new byte[2]
        {
          msg.getPayloadByte(2),
          msg.getPayloadByte(3)
        });
        this._timeHold = RfidUtils.bigEndianBytesToUInt16(new byte[2]
        {
          msg.getPayloadByte(4),
          msg.getPayloadByte(5)
        });
        this._eventFront = (StandAloneIoDefs.EventFrontBitMask) msg.getPayloadByte(6);
      }

      public InputSettings(byte channel, ushort timeHold, ushort timeUp, ushort timeDown)
        : base(StandAloneIoDefs.IoType.INPUT, StandAloneIoDefs.ConfigIoOp.WRITE, channel)
      {
        this._timeHold = timeHold;
        this._timeUp = timeUp;
        this._timeDown = timeDown;
        this._eventFront = StandAloneIoDefs.EventFrontBitMask.BOTH;
      }

      public InputSettings(byte channel, ushort timeHold, ushort timeUp, ushort timeDown, bool RiseEvent, bool FallEvent)
        : base(StandAloneIoDefs.IoType.INPUT, StandAloneIoDefs.ConfigIoOp.WRITE, channel)
      {
        this._timeHold = timeHold;
        this._timeUp = timeUp;
        this._timeDown = timeDown;
        if (RiseEvent && FallEvent)
          this._eventFront = StandAloneIoDefs.EventFrontBitMask.BOTH;
        else if (RiseEvent)
          this._eventFront = StandAloneIoDefs.EventFrontBitMask.HIGH_LOW;
        else if (FallEvent)
          this._eventFront = StandAloneIoDefs.EventFrontBitMask.LOW_HIGH;
        else
          this._eventFront = StandAloneIoDefs.EventFrontBitMask.NONE;
      }

      public ushort getTimeHold()
      {
        return this._timeHold;
      }

      public ushort getTimeUp()
      {
        return this._timeUp;
      }

      public ushort getTimeDown()
      {
        return this._timeDown;
      }

      public bool getFallEvent()
      {
        return this._eventFront == StandAloneIoDefs.EventFrontBitMask.LOW_HIGH || this._eventFront == StandAloneIoDefs.EventFrontBitMask.BOTH;
      }

      public bool getRiseEvent()
      {
        return this._eventFront == StandAloneIoDefs.EventFrontBitMask.HIGH_LOW || this._eventFront == StandAloneIoDefs.EventFrontBitMask.BOTH;
      }

      internal override byte[] getRawData()
      {
        byte[] rawData = base.getRawData();
        byte[] numArray = new byte[rawData.Length + 7];
        Array.Copy((Array) rawData, (Array) numArray, rawData.Length);
        byte[] bigEndianBytes1 = RfidUtils.ushortToBigEndianBytes(this._timeHold);
        byte[] bigEndianBytes2 = RfidUtils.ushortToBigEndianBytes(this._timeUp);
        byte[] bigEndianBytes3 = RfidUtils.ushortToBigEndianBytes(this._timeDown);
        numArray[rawData.Length] = bigEndianBytes2[0];
        numArray[rawData.Length + 1] = bigEndianBytes2[1];
        numArray[rawData.Length + 2] = bigEndianBytes3[0];
        numArray[rawData.Length + 3] = bigEndianBytes3[1];
        numArray[rawData.Length + 4] = bigEndianBytes1[0];
        numArray[rawData.Length + 5] = bigEndianBytes1[1];
        numArray[rawData.Length + 6] = (byte) this._eventFront;
        return numArray;
      }
    }

    public enum ConfigOutFlags : byte
    {
      CONFIG_OUT = 0,
      ACTIVATE_OUT = 3,
    }

    public enum ConfigIoMeasUnit : byte
    {
      MILLISECONDS,
      SECONDS,
    }

    public class OutputSettings : StandAloneIoDefs.IoSettings
    {
      private StandAloneIoDefs.ConfigIoMeasUnit _measUnitOn;
      private StandAloneIoDefs.ConfigIoMeasUnit _measUnitOff;
      private ushort _timeOn;
      private ushort _timeOff;
      private byte _cycles;

      public OutputSettings(RfidMsg msg)
        : base(msg)
      {
        this._measUnitOn = (StandAloneIoDefs.ConfigIoMeasUnit) msg.getPayloadByte(0);
        this._measUnitOff = (StandAloneIoDefs.ConfigIoMeasUnit) msg.getPayloadByte(1);
        this._timeOn = RfidUtils.bigEndianBytesToUInt16(new byte[2]
        {
          msg.getPayloadByte(2),
          msg.getPayloadByte(3)
        });
        this._timeOff = RfidUtils.bigEndianBytesToUInt16(new byte[2]
        {
          msg.getPayloadByte(4),
          msg.getPayloadByte(5)
        });
        this._cycles = msg.getPayloadByte(6);
      }

      public OutputSettings(byte channel, StandAloneIoDefs.ConfigIoMeasUnit measUnitOn, StandAloneIoDefs.ConfigIoMeasUnit measUnitOff, ushort timeOn, ushort timeOff, byte cycles)
        : base(StandAloneIoDefs.IoType.OUTPUT, StandAloneIoDefs.ConfigIoOp.WRITE, channel)
      {
        this._measUnitOn = measUnitOn;
        this._measUnitOff = measUnitOff;
        this._timeOn = timeOn;
        this._timeOff = timeOff;
        this._cycles = cycles;
      }

      public StandAloneIoDefs.ConfigIoMeasUnit getMeasUnitOn()
      {
        return this._measUnitOn;
      }

      public StandAloneIoDefs.ConfigIoMeasUnit getMeasUnitOff()
      {
        return this._measUnitOff;
      }

      public ushort getTimeOn()
      {
        return this._timeOn;
      }

      public ushort getTimeOff()
      {
        return this._timeOff;
      }

      public byte getCycles()
      {
        return this._cycles;
      }

      internal override byte[] getRawData()
      {
        byte[] rawData = base.getRawData();
        byte[] numArray = new byte[rawData.Length + 7];
        Array.Copy((Array) rawData, (Array) numArray, rawData.Length);
        byte[] bigEndianBytes1 = RfidUtils.ushortToBigEndianBytes(this._timeOn);
        byte[] bigEndianBytes2 = RfidUtils.ushortToBigEndianBytes(this._timeOff);
        numArray[rawData.Length] = (byte) this._measUnitOn;
        numArray[rawData.Length + 1] = (byte) this._measUnitOff;
        numArray[rawData.Length + 2] = bigEndianBytes1[0];
        numArray[rawData.Length + 3] = bigEndianBytes1[1];
        numArray[rawData.Length + 4] = bigEndianBytes2[0];
        numArray[rawData.Length + 5] = bigEndianBytes2[1];
        numArray[rawData.Length + 6] = this._cycles;
        return numArray;
      }
    }
  }
}
