// Decompiled with JetBrains decompiler
// Type: amrfidmgrex.IRFIDCiclo
// Assembly: amrfidmgrex, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C64A34BB-7820-4D59-B5A9-AB5E58A7ECDA
// Assembly location: D:\Sorgenti\BRZ\AmRfidMgrEx.dll

using System.Runtime.InteropServices;

namespace amrfidmgrex
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComVisible(true)]
  public interface IRFIDCiclo
  {
    string ExamDate { get; set; }

    string SterilizationStartDate { get; set; }

    string SterilizationEndDate { get; set; }

    string ExamOperatorName { get; set; }

    string ExamOperatorSurname { get; set; }

    string SterilizationStartOperatorName { get; set; }

    string SterilizationStartOperatorSurname { get; set; }

    string SterilizationEndOperatorName { get; set; }

    string SterilizationEndOperatorSurname { get; set; }

    string DeviceIdNumber { get; set; }

    string DeviceDescription { get; set; }

    string SterilizerDescription { get; set; }

    string CustomTrackingDate1 { get; set; }

    string CustomTrackingOperatorName1 { get; set; }

    string CustomTrackingOperatorSurname1 { get; set; }

    string CustomTrackingDate2 { get; set; }

    string CustomTrackingOperatorName2 { get; set; }

    string IdVasca { get; set; }

    string MachineCycleId { get; set; }

    string DisinfectantCycleId { get; set; }

    string AdditionalInfo { get; set; }
  }
}
