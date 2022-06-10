using System;
using System.Text;

namespace It.IDnova.Fxw.AFR600
{
  public class AFR600Parameters
  {
    public const int N_RADIOS = 2;
    public const int N_INPUTS = 2;

    public string FirmwareRelease { get; set; }

    public string[] RfAddress { get; set; }

    public int[] RfAddressLen { get; set; }

    public int[] RfChannel { get; set; }

    public int[] RfDataRate { get; set; }

    public int[] RfPayloadLen { get; set; }

    public int[] RfRxMode { get; set; }

    public int[] RfAttenuation { get; set; }

    public int CommEventMode { get; set; }

    public DateTime RTC { get; set; }

    public int StandaloneMode { get; set; }

    public AFR600Parameters()
    {
      this.RfAddress = new string[2];
      this.RfAddressLen = new int[2];
      this.RfChannel = new int[2];
      this.RfDataRate = new int[2];
      this.RfPayloadLen = new int[2];
      this.RfRxMode = new int[2];
      this.RfAttenuation = new int[2];
      this.RTC = DateTime.MinValue;
    }

    public bool decodeParam(RfidDefs.FxwExtParam param, int idx, byte[] paramPayload, out string errMsg)
    {
      errMsg = "";
      byte[] numArray = new byte[paramPayload.Length];
      Array.Copy((Array) paramPayload, 0, (Array) numArray, 0, numArray.Length);
      try
      {
        switch (param)
        {
          case RfidDefs.FxwExtParam.EXTPAR_FW_RELEASE:
            this.FirmwareRelease = this.FirmwareRelease + Encoding.ASCII.GetString(numArray).Replace(char.MinValue, ' ');
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RTC_TIME:
            this.RTC = new DateTime(2000 + this.bcdToInt(numArray[0]), this.bcdToInt(numArray[1]), this.bcdToInt(numArray[2]), this.bcdToInt(numArray[4]), this.bcdToInt(numArray[5]), this.bcdToInt(numArray[6]), DateTimeKind.Local);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_ADDRESS:
            this.RfAddress[idx] = this.decodeHexString(numArray);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_ADDRESS_LEN:
            this.RfAddressLen[idx] = this.decodeSingleByte(numArray);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_CHANNEL:
            this.RfChannel[idx] = this.decodeSingleByte(numArray);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_DATA_RATE:
            this.RfDataRate[idx] = this.decodeSingleByte(numArray);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_PAYLOAD_LEN:
            this.RfPayloadLen[idx] = this.decodeSingleByte(numArray);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_RX_MODE:
            this.RfRxMode[idx] = this.decodeSingleByte(numArray);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_ATTENUATION:
            this.RfAttenuation[idx] = this.decodeSingleByte(numArray);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_COMM_EVENT_MODE:
            this.CommEventMode = this.decodeSingleByte(numArray);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_SA_ENABLED:
            this.StandaloneMode = this.decodeSingleByte(numArray);
            break;
          default:
            errMsg = "Parameter is not decodable";
            return false;
        }
      }
      catch (Exception ex)
      {
        errMsg = "Error decoding parameter: " + ex.Message;
        return false;
      }
      return true;
    }

    private int bcdToInt(byte bcd)
    {
      return (((int) bcd & 240) >> 4) * 10 + ((int) bcd & 15);
    }

    private string decodeHexString(byte[] data)
    {
      return RfidUtils.byteArrayToHexString(data);
    }

    private int decodeSingleByte(byte[] data)
    {
      return (int) data[0];
    }

    private byte[] encodeSingleByte(int value)
    {
      return new byte[1]{ (byte) value };
    }

    public byte[] encodeParam(RfidDefs.FxwExtParam param, int idx, out string errMsg)
    {
      errMsg = "";
      byte[] numArray;
      try
      {
        switch (param)
        {
          case RfidDefs.FxwExtParam.EXTPAR_RTC_TIME:
            DateTime rtc = this.RTC;
            byte num1 = RfidUtils.encodeBcd((byte) (rtc.Year - 2000));
            rtc = this.RTC;
            byte num2 = RfidUtils.encodeBcd((byte) rtc.Month);
            rtc = this.RTC;
            byte num3 = RfidUtils.encodeBcd((byte) rtc.Day);
            rtc = this.RTC;
            int num4 = (int) rtc.DayOfWeek;
            if (num4 == 0)
              num4 = 7;
            byte num5 = RfidUtils.encodeBcd((byte) num4);
            rtc = this.RTC;
            byte num6 = RfidUtils.encodeBcd((byte) rtc.Hour);
            rtc = this.RTC;
            byte num7 = RfidUtils.encodeBcd((byte) rtc.Minute);
            rtc = this.RTC;
            byte num8 = RfidUtils.encodeBcd((byte) rtc.Second);
            byte num9 = 0;
            numArray = new byte[8]
            {
              num1,
              num2,
              num3,
              num5,
              num6,
              num7,
              num8,
              num9
            };
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_ADDRESS:
            numArray = RfidUtils.hexStringToByteArray(this.RfAddress[idx]);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_ADDRESS_LEN:
            numArray = this.encodeSingleByte(this.RfAddressLen[idx]);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_CHANNEL:
            numArray = this.encodeSingleByte(this.RfChannel[idx]);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_DATA_RATE:
            numArray = this.encodeSingleByte(this.RfDataRate[idx]);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_PAYLOAD_LEN:
            numArray = this.encodeSingleByte(this.RfPayloadLen[idx]);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_RX_MODE:
            numArray = this.encodeSingleByte(this.RfRxMode[idx]);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_RF_ATTENUATION:
            numArray = this.encodeSingleByte(this.RfAttenuation[idx]);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_COMM_EVENT_MODE:
            numArray = this.encodeSingleByte(this.CommEventMode);
            break;
          case RfidDefs.FxwExtParam.EXTPAR_SA_ENABLED:
            numArray = this.encodeSingleByte(this.StandaloneMode);
            break;
          default:
            numArray = (byte[]) null;
            errMsg = "Unknown parameter " + (object) param;
            break;
        }
      }
      catch (Exception ex)
      {
        numArray = (byte[]) null;
        errMsg = "Error encoding parameter " + (object) param + " : " + ex.Message;
      }
      return numArray;
    }
  }
}
