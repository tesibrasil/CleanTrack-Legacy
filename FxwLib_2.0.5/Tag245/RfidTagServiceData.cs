// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.Tag245.RfidTagServiceData
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

using System;

namespace It.IDnova.Fxw.Tag245
{
  public class RfidTagServiceData
  {
    private const byte ADDRESS_MAX_LEN = 5;
    private byte _txChannel;
    private byte _rxChannel;
    private byte _addressLen;
    private string _rxAddress;
    private string _txAddress;
    private ushort _rxRate;
    private string _id;

    public RfidTagServiceData(RfidTagReply msg)
    {
      this._txChannel = msg.Data[0];
      this._rxChannel = msg.Data[1];
      this._addressLen = msg.Data[2];
      byte[] byteArray1 = new byte[5];
      Array.Copy((Array) msg.Data, 3, (Array) byteArray1, 0, 5);
      this._rxAddress = RfidUtils.byteArrayToHexString(byteArray1);
      byte[] byteArray2 = new byte[5];
      Array.Copy((Array) msg.Data, 8, (Array) byteArray2, 0, 5);
      this._txAddress = RfidUtils.byteArrayToHexString(byteArray2);
      this._rxRate = RfidUtils.littleEndianBytesToUint16(new byte[2]
      {
        msg.Data[13],
        msg.Data[14]
      });
      byte[] byteArray3 = new byte[8];
      Array.Copy((Array) msg.Data, 16, (Array) byteArray3, 0, 8);
      this._id = RfidUtils.byteArrayToHexString(byteArray3);
    }

    public byte getTxChannel()
    {
      return this._txChannel;
    }

    public byte getRxChannel()
    {
      return this._rxChannel;
    }

    public byte getAddressLen()
    {
      return this._addressLen;
    }

    public string getRxAddress()
    {
      return this._rxAddress;
    }

    public string getTxAddress()
    {
      return this._txAddress;
    }

    public ushort getRxRate()
    {
      return this._rxRate;
    }

    public string getId()
    {
      return this._id;
    }
  }
}
