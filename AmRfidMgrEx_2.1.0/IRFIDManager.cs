// Decompiled with JetBrains decompiler
// Type: amrfidmgrex.IRFIDManager
// Assembly: amrfidmgrex, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C64A34BB-7820-4D59-B5A9-AB5E58A7ECDA
// Assembly location: D:\Sorgenti\BRZ\AmRfidMgrEx.dll

using System.Runtime.InteropServices;

namespace amrfidmgrex
{
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComVisible(true)]
  public interface IRFIDManager
  {
    void startListening(string address, int fromState, int examId);

    void addUserListener(IRFIDEvents evt);

    void addDeviceListener(IRFIDEvents evt);

    void addCompletedListener(IRFIDEvents evt);

    void cleanUpListeners();

    void setConnectionString(string connectionString);

    void stopListening();

    long checkAndInsertManualCycle(int stateToMatch, int op, int device, int ExamId);

    long checkUserValidity(string op);

    long getUserIdFromMat(string mat);

    long getDeviceIdFromMat(string mat);

    long testDatabaseConnection();

    long testRFIDAddress(string address);

    string getDeviceDesc(int id);

    string getUserName(int id);

    string getUserSurname(int id);

    string getSeparator();

    string getCycleAdditionalInfo(int examId, long previous);

    RFIDCiclo GetCycleData(int iIDEsame, int iPrevious);

    RFIDCiclo GetCycleDataFromEsameDispositivo(
      int iIDEsame,
      int iIDDispositivo,
      int iPrevious);
  }
}
