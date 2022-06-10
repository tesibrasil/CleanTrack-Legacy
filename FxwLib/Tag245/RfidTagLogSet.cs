using System;

namespace It.IDnova.Fxw.Tag245
{
  public class RfidTagLogSet
  {
    private static DateTime UTC_ORIGIN = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    internal const int LOG_DATA_LEN_BYTES = 24;
    private byte[] _rawLogData;
    private TemperatureLogMode _logMode;

    public RfidTagLogSet(TemperatureLogMode logMode, byte[] data)
    {
      if (data.Length != 24)
        throw new Exception("Invalid data length");
      this._rawLogData = data;
      this._logMode = logMode;
    }

    public string[] getLogs()
    {
      string[] strArray = (string[]) null;
      if (this._logMode == TemperatureLogMode.THRESHOLD || this._logMode == TemperatureLogMode.CONTINUOUS_WITH_TS)
        strArray = this.decodeLogRecordWithTimestamp();
      else if (this._logMode == TemperatureLogMode.CONTINUOUS)
        strArray = this.decodeLogRecord();
      return strArray;
    }

    private string[] decodeLogRecordWithTimestamp()
    {
      string[] strArray = new string[4];
      for (int index1 = 0; index1 < 4; ++index1)
      {
        int index2 = index1 * 6;
        float num = (float) RfidUtils.twoComplement14BitsToShort((ushort) ((uint) this._rawLogData[1 + index2] + ((uint) this._rawLogData[index2] << 8))) / 32f;
        long uint32 = (long) RfidUtils.bigEndianBytesToUInt32(new byte[4]
        {
          this._rawLogData[index2 + 2],
          this._rawLogData[index2 + 3],
          this._rawLogData[index2 + 4],
          this._rawLogData[index2 + 5]
        });
        DateTime dateTime = RfidTagLogSet.UTC_ORIGIN;
        dateTime = dateTime.AddSeconds((double) uint32);
        strArray[index1] = dateTime.ToString() + " " + num.ToString("F2");
      }
      return strArray;
    }

    private string[] decodeLogRecord()
    {
      string[] strArray = new string[12];
      for (int index = 0; index < 12; ++index)
      {
        float num = (float) RfidUtils.twoComplement14BitsToShort((ushort) ((uint) this._rawLogData[index * 2 + 1] + ((uint) this._rawLogData[index * 2] << 8))) / 32f;
        strArray[index] = num.ToString("F2");
      }
      return strArray;
    }
  }
}
