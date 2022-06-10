namespace It.IDnova.Fxw
{
  internal class DeviceConfig
  {
    private ushort _type = 0;
    private string _class = (string) null;
    private string _description = (string) null;
    private string _mode = (string) null;

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
