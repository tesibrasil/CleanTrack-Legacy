namespace It.IDnova.Fxw
{
  public abstract class AbstractFxwLog
  {
    protected static FxwLog _instance = (FxwLog) null;
    protected static bool _isDebugEnabled = false;
    protected static bool _isLogEnabled = false;

    public bool debugEnabled
    {
      get
      {
        return AbstractFxwLog._isDebugEnabled;
      }
      set
      {
        AbstractFxwLog._isDebugEnabled = value;
      }
    }

    public bool logEnabled
    {
      get
      {
        return AbstractFxwLog._isLogEnabled;
      }
      set
      {
        AbstractFxwLog._isLogEnabled = value;
      }
    }

    internal abstract void debug(string msg);

    // public abstract void log(string msg);
  }
}
