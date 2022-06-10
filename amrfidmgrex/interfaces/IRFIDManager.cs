using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("150F7CE7-212D-373C-9D3D-0E8AB94055D9")]
    public interface IRFIDManager
	{
		void startListening(string address, int fromState, int examId);
		void addUserListener(IRFIDEvent evt);
		void addDeviceListener(IRFIDEvent evt);
		void addCompletedListener(IRFIDEvent evt);
		void cleanUpListeners();
		void setConnectionString(string connectionString);
		void stopListening();
		long checkAndInsertManualCycle(int op, int device, int ExamId, bool check_transaction = true);
		long forceInsertManualCycle(int op, int device, int ExamId);
		long checkUserValidity(string op);
		long getUserIdFromMat(string mat);
		long getDeviceIdFromMat(string mat);
		string getDeviceDesc(int id);
		string getUserName(int id);
		string getUserSurname(int id);

		string getSeparator();
		string getCycleAdditionalInfo(int examId, long previous);

		RFIDCycle GetCycleData(int iIDEsame, bool bPrevious);
		RFIDCycle GetCycleDataFromEsameDispositivo(int iIDEsame, int iIDDispositivo, bool bPrevious);
        RFIDCycleEx GetCycleDataFromEsameDispositivoEx(int iIDEsame, int iIDDispositivo, bool bPrevious);
    }
}
