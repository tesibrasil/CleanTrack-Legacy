using System;

namespace It.IDnova.Fxw.AFR600
{
  public class AFR600Log
  {
    public DateTime timestamp { get; set; }

    public ushort user { get; set; }

    public bool dump { get; set; }

    public int eventType { get; set; }

    public bool antenna1 { get; set; }

    public bool antenna2 { get; set; }

    public string raw { get; set; }

    public static AFR600Log parseLog(byte[] payload)
    {
      if (payload == null || payload.Length != 11)
        return (AFR600Log) null;
      int year = AFR600Log.bcdToInt(payload[0]) + 2000;
      int month = AFR600Log.bcdToInt(payload[1]);
      int day = AFR600Log.bcdToInt(payload[2]);
      AFR600Log.bcdToInt(payload[3]);
      int hour = AFR600Log.bcdToInt(payload[4]);
      int minute = AFR600Log.bcdToInt(payload[5]);
      int second = AFR600Log.bcdToInt(payload[6]);
      int num1 = (int) payload[8];
      ushort num2 = (ushort) ((uint) payload[9] << 8 | (uint) payload[10]);
      return new AFR600Log()
      {
        raw = RfidUtils.byteArrayToHexString(payload),
        timestamp = new DateTime(year, month, day, hour, minute, second),
        dump = (uint) (num1 & 128) > 0U,
        antenna1 = (uint) (num1 & 32) > 0U,
        antenna2 = (uint) (num1 & 64) > 0U,
        eventType = num1 & 15,
        user = num2
      };
    }

    private static int bcdToInt(byte bcd)
    {
      return (((int) bcd & 240) >> 4) * 10 + ((int) bcd & 15);
    }
  }
}
