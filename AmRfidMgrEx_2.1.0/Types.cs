// Decompiled with JetBrains decompiler
// Type: amrfidmgrex.Types
// Assembly: amrfidmgrex, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C64A34BB-7820-4D59-B5A9-AB5E58A7ECDA
// Assembly location: D:\Sorgenti\BRZ\AmRfidMgrEx.dll

namespace amrfidmgrex
{
  public class Types
  {
    public enum Result
    {
      Unknown,
      Error,
      Success,
      Timeout,
    }

    public class Info
    {
      public Types.Result Result { get; set; }

      public string Description { get; set; }

      public int IdStepType { get; set; }
    }

    public delegate void CompleteDelegate(Types.Info info);
  }
}
