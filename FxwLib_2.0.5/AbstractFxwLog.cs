// Decompiled with JetBrains decompiler
// Type: It.IDnova.Fxw.AbstractFxwLog
// Assembly: FxwLib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=null
// MVID: DB1CF7CE-67A8-4C2C-96C8-A563E0565961
// Assembly location: D:\Sorgenti\BRZ\FxwLib.dll

namespace It.IDnova.Fxw
{
  public abstract class AbstractFxwLog
  {
    protected static FxwLog _instance;
    protected static bool _isDebugEnabled;
    protected static bool _isLogEnabled;

    internal abstract void debug(string msg);

    public abstract void log(string msg);

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
  }
}
