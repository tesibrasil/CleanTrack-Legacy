// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.ReaderInfo
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: Y:\amrfidex\CleanTrack\FxwLib.dll

namespace It.IDnova.Fxw
{
  public class ReaderInfo
  {
    private FxwLog _logger = FxwLog.getInstance();
    private string _serialNumber = "UNDEFINED";
    private string _model = "UNDEFINED";
    private string _firmware = "UNDEFINED";
    private string _status = "UNDEFINED";
    public const string UNDEFINED_FIELD = "UNDEFINED";
    public const string STATUS_BOOTING = "BOOTING";
    public const string STATUS_VALID = "VALID";

    public string Status
    {
      get
      {
        return this._status;
      }
    }

    public string SerialNumber
    {
      get
      {
        return this._serialNumber;
      }
      set
      {
        this._serialNumber = value;
      }
    }

    public string Model
    {
      get
      {
        return this._model;
      }
      set
      {
        this._model = value;
      }
    }

    public string Firmware
    {
      get
      {
        return this._firmware;
      }
      set
      {
        this._firmware = value;
      }
    }

    public ReaderInfo()
    {
      this._status = "BOOTING";
      this._serialNumber = "UNDEFINED";
      this._model = "UNDEFINED";
      this._firmware = "UNDEFINED";
      this._status = "UNDEFINED";
    }

    public void resetStatus()
    {
      this._status = "UNDEFINED";
      this._serialNumber = "UNDEFINED";
      this._model = "UNDEFINED";
      this._firmware = "UNDEFINED";
    }

    internal void checkStatus()
    {
      if (this._serialNumber.Equals("UNDEFINED") || this._model.Equals("UNDEFINED"))
        return;
      this._status = "VALID";
      this._logger.debug("");
      this._logger.debug("[ReaderInfo] Status: " + this._status + " | Model: " + this._model + " | SerialNum: " + this._serialNumber + " | FW: " + this._firmware);
    }
  }
}
