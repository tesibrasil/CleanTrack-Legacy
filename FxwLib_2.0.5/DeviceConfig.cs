// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.DeviceConfig
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

namespace It.IDnova.Fxw
{
  internal class DeviceConfig
  {
    private ushort _type;
    private string _class;
    private string _description;
    private string _mode;

    public ushort Type
    {
      get
      {
        return this._type;
      }
      set
      {
        this._type = value;
      }
    }

    public string Class
    {
      get
      {
        return this._class;
      }
      set
      {
        this._class = value;
      }
    }

    public string Description
    {
      get
      {
        return this._description;
      }
      set
      {
        this._description = value;
      }
    }

    public string Mode
    {
      get
      {
        return this._mode;
      }
      set
      {
        this._mode = value;
      }
    }
  }
}
