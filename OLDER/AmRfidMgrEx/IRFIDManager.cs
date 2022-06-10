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

    RFIDCiclo GetCycleDataFromEsameDispositivo(int iIDEsame, int iIDDispositivo, int iPrevious);
  }
}
