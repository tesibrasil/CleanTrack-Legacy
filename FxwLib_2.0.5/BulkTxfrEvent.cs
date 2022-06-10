// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.BulkTxfrEvent
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

using System;

namespace It.IDnova.Fxw
{
  public class BulkTxfrEvent
  {
    private bool _isValid;

    public bool isValid()
    {
      return this._isValid;
    }

    public BulkTxfrEvent.BULK_TXFR_EVENT type { get; set; }

    public int rtxCnt { get; set; }

    public byte[] hash { get; set; }

    public BulkTxfrEvent(RfidMsg msg)
    {
      this._isValid = false;
      try
      {
        if (msg.CommandIdentifier != (byte) 231)
          return;
        byte[] payload = msg.getPayload();
        this.type = (BulkTxfrEvent.BULK_TXFR_EVENT) payload[0];
        this.rtxCnt = 15 & (int) payload[1];
        if (this.type == BulkTxfrEvent.BULK_TXFR_EVENT.BULK_MODE_ENTERED)
        {
          this.hash = new byte[20];
          Array.Copy((Array) payload, 4, (Array) this.hash, 0, 20);
        }
      }
      catch
      {
        return;
      }
      this._isValid = true;
    }

    public enum BULK_TXFR_EVENT : byte
    {
      ACK_BLOCK = 0,
      BULK_MODE_ENTERED = 1,
      ACK_LAST_BLOCK = 2,
      TIMEOUT = 3,
      MAX_RETRY = 255, // 0xFF
    }
  }
}
