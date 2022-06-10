// Decompiled with JetBrains decompiler
// Type: amrfidmgrex.DBObject
// Assembly: amrfidmgrex, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C64A34BB-7820-4D59-B5A9-AB5E58A7ECDA
// Assembly location: D:\Sorgenti\BRZ\AmRfidMgrEx.dll

namespace amrfidmgrex
{
  public class DBObject
  {
    protected static readonly object Locker = new object();

    protected static string ODBCConnectionString { get; set; }
  }
}
