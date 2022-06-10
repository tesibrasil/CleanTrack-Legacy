// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.RfidMsg
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

using System;

namespace It.IDnova.Fxw
{
  public class RfidMsg
  {
    private byte _startOfFrame = 42;
    private byte _msgLen;
    private byte _cmdId;
    private byte _cmdFlags;
    private byte[] _payload;
    private byte _crc;

    public byte CommandIdentifier
    {
      get
      {
        return this._cmdId;
      }
      set
      {
        this._cmdId = value;
      }
    }

    public byte CommandFlags
    {
      get
      {
        return this._cmdFlags;
      }
      set
      {
        this._cmdFlags = value;
      }
    }

    internal RfidMsg()
    {
    }

    internal RfidMsg(byte aCmdId, byte aCmdFlags, byte[] aPayload, byte aPayloadLen)
    {
      this.decodeMsg(aCmdId, aCmdFlags, aPayload, aPayloadLen);
    }

    internal RfidMsg(byte[] aMsgData, byte aMsgLen)
    {
      if ((int) aMsgLen < 5)
        throw new Exception("[RfidMsg] Invalid message lenght");
      if ((int) aMsgData[0] != (int) this._startOfFrame)
        return;
      byte aPayloadLen = (byte) ((uint) aMsgData[1] - 2U);
      byte aCmdId = aMsgData[2];
      byte aCmdFlags = aMsgData[3];
      byte[] aPayload = new byte[aMsgData.Length - 5];
      Array.Copy((Array) aMsgData, 4, (Array) aPayload, 0, aMsgData.Length - 5);
      this.decodeMsg(aCmdId, aCmdFlags, aPayload, aPayloadLen);
    }

    public bool isValid()
    {
      return (int) this._cmdId != 0 && (int) this._msgLen != 0;
    }

    public int getPayloadLen()
    {
      if (this._payload == null)
        return 0;
      return this._payload.Length;
    }

    public byte[] getPayload()
    {
      return this._payload;
    }

    public byte getPayloadByte(int byteIndex)
    {
      if (this._payload == null || byteIndex >= this._payload.Length)
        return 0;
      return this._payload[byteIndex];
    }

    public string getPayloadHexString(bool skipFirstByte)
    {
      string str = "";
      bool flag = !skipFirstByte;
      foreach (byte num in this._payload)
      {
        if (!flag)
          flag = true;
        else
          str += num.ToString("X2");
      }
      return str;
    }

    public string getPayloadString()
    {
      string str = "";
      bool flag = false;
      foreach (byte num in this._payload)
      {
        if (!flag)
          flag = true;
        else
          str += (string) (object) (char) num;
      }
      return str;
    }

    public RfidDefs.FxwProtoRes decodeErrorCode()
    {
      RfidDefs.FxwProtoRes fxwProtoRes = RfidDefs.FxwProtoRes.OK_ERR_NONE;
      if (((int) this._cmdFlags & 1) == 1)
        fxwProtoRes = (RfidDefs.FxwProtoRes) ((uint) this._cmdFlags >> 1);
      return fxwProtoRes;
    }

    internal void setPayload(byte[] aPayload)
    {
      if (aPayload == null || aPayload.Length == 0)
      {
        this._payload = (byte[]) null;
      }
      else
      {
        this._payload = (byte[]) aPayload.Clone();
        this._msgLen = (byte) (aPayload.Length + 2);
      }
    }

    internal byte[] getRawMsg()
    {
      byte[] numArray1 = new byte[(int) (byte) ((uint) this._msgLen + 3U)];
      byte num1 = 0;
      byte[] numArray2 = numArray1;
      int index1 = (int) num1;
      int num2 = 1;
      byte num3 = (byte) (index1 + num2);
      int startOfFrame = (int) this._startOfFrame;
      numArray2[index1] = (byte) startOfFrame;
      byte[] numArray3 = numArray1;
      int index2 = (int) num3;
      int num4 = 1;
      byte num5 = (byte) (index2 + num4);
      int msgLen = (int) this._msgLen;
      numArray3[index2] = (byte) msgLen;
      byte[] numArray4 = numArray1;
      int index3 = (int) num5;
      int num6 = 1;
      byte num7 = (byte) (index3 + num6);
      int cmdId = (int) this._cmdId;
      numArray4[index3] = (byte) cmdId;
      byte[] numArray5 = numArray1;
      int index4 = (int) num7;
      int num8 = 1;
      byte num9 = (byte) (index4 + num8);
      int cmdFlags = (int) this._cmdFlags;
      numArray5[index4] = (byte) cmdFlags;
      foreach (byte num10 in this._payload)
        numArray1[(int) num9++] = num10;
      byte[] numArray6 = numArray1;
      int index5 = (int) num9;
      int num11 = 1;
      byte num12 = (byte) (index5 + num11);
      int crc = (int) this._crc;
      numArray6[index5] = (byte) crc;
      return numArray1;
    }

    internal string getRawMsgHexString()
    {
      byte[] rawMsg = this.getRawMsg();
      string str = "";
      foreach (byte num in rawMsg)
        str = str + num.ToString("X2") + " ";
      return str;
    }

    private void decodeMsg(byte aCmdId, byte aCmdFlags, byte[] aPayload, byte aPayloadLen)
    {
      this._msgLen = (byte) ((uint) aPayloadLen + 2U);
      this._crc = (byte) 0;
      this._cmdId = aCmdId;
      this._crc += this._cmdId;
      this._cmdFlags = aCmdFlags;
      this._crc += this._cmdFlags;
      if ((int) aPayloadLen > 0 && (int) aPayloadLen > aPayload.Length)
        return;
      this._payload = new byte[(int) aPayloadLen];
      for (int index = 0; index < (int) aPayloadLen; ++index)
      {
        this._payload[index] = aPayload[index];
        this._crc += this._payload[index];
      }
    }
  }
}
