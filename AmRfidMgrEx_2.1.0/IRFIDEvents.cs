// Decompiled with JetBrains decompiler
// Type: amrfidmgrex.IRFIDEvents
// Assembly: amrfidmgrex, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C64A34BB-7820-4D59-B5A9-AB5E58A7ECDA
// Assembly location: D:\Sorgenti\BRZ\AmRfidMgrEx.dll

using System.Runtime.InteropServices;

namespace amrfidmgrex
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComVisible(true)]
  public interface IRFIDEvents
  {
    void DeviceDetected(string desc, long id);

    void UserDetected(string nome, string cognome, long id);

    void BadgeDetected(string id);

    void Completed(long success);
  }
}
