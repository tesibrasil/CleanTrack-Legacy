using System;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using KleanTrak.Core;

namespace amrfidmgrex
{
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("7F7E32D0-7555-3947-B8C7-4EB09EE9B157")]
	public class RFIDManager : IRFIDManager
	{
		protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		IRFIDEvent m_badge_listener = null;
		IRFIDEvent m_user_listener = null;
		IRFIDEvent m_device_listener = null;
		IRFIDEvent m_completed_listener = null;

		private string connectionDBString = "";

		public RFIDManager()
		{
		}

		~RFIDManager()
		{
		}

		public void setConnectionString(string connectionString)
		{
			connectionDBString = connectionString;
			DbConnection.ConnectionString = connectionDBString;
			//Logger.COMObject = true;
			//Logger.Get().ActivateFileDestination("c:\\temp", null, false, true);

			//if (EnableDBLogging)
			//Logger.Get().ActivateDatabaseDestination(connectionDBString);
		}

		public void addCompletedListener(IRFIDEvent evt)
		{
			m_completed_listener = evt;
		}

		public void addBadgeListener(IRFIDEvent evt)
		{
			m_badge_listener = evt;
		}

		public void addUserListener(IRFIDEvent evt)
		{
			m_user_listener = evt;
		}

		public void addDeviceListener(IRFIDEvent evt)
		{
			m_device_listener = evt;
		}

		public void cleanUpListeners()
		{
			cleanInternalListenersReferences();
		}

		public void startListening(string address, int fromState, int examId)
		{
			Logger.Info("RFIDManager.startListening...");
			if (StepManager.isInitiated())
			{
				StepManager.Reset();
				StepManager.DeviceDetected += StepManager_DeviceDetected;
				StepManager.UserDetected += StepManager_UserDetected;
				StepManager.DataCollectionCompleted += StepManager_DataCollectionCompleted;

			}
			else
			{
				if (connectionDBString != null && connectionDBString != "")
				{
					StepManager.Init(address, connectionDBString);
					StepManager.DeviceDetected += StepManager_DeviceDetected;
					StepManager.UserDetected += StepManager_UserDetected;
					StepManager.DataCollectionCompleted += StepManager_DataCollectionCompleted;
				}

			}

			StepManager.ReadForExam(2, fromState, examId);
			Logger.Info("...RFIDManager.startListening");
		}

		public void readData(string address)
		{
			Logger.Info("RFIDManager.readData...");
			if (!StepManager.isRFIDHelperInitiated())
			{
				StepManager.Init(address, connectionDBString);
				StepManager.BadgeDetected += StepManager_BadgeDetected;
			}
			else
			{
				StepManager.Reset();
				StepManager.BadgeDetected += StepManager_BadgeDetected;
			}

			StepManager.OneStep();
			Logger.Info("...RFIDManager.readData");
		}

		void StepManager_BadgeDetected(string id)
		{
			Logger.Info("RFIDManager.StepManager_BadgeDetected...");
			stopListening();

			if (m_badge_listener != null)
				m_badge_listener.BadgeDetected(id);

			Logger.Info("...RFIDManager.StepManager_BadgeDetected");
		}

		public string getDeviceDesc(int id)
		{
			Logger.Info("RFIDManager.getDeviceDesc");
			return DBUtilities.getDeviceDescFromId(id, connectionDBString);
		}

		public string getUserName(int id)
		{
			Logger.Info("RFIDManager.getUserName");
			return DBUtilities.getOperatorNameFromId(id, connectionDBString);
		}

		public string getUserSurname(int id)
		{
			Logger.Info("RFIDManager.getUserSurname");
			return DBUtilities.getOperatorSurnameFromId(id, connectionDBString);
		}

		/// <summary>
		/// Usata da endox per la lettura dell'operatore
		/// </summary>
		/// <param name="mat">matricola dell'operatore</param>
		/// <returns></returns>
		public long getUserIdFromMat(string mat)
		{
			Logger.Info("RFIDManager.getUserIdFromMat");
			return DBUtilities.getOperatorIdFromMat(mat, connectionDBString);
		}

		/// <summary>
		/// Usata da endox per la lettura della sonda
		/// </summary>
		/// <param name="matortag">matricola o tag della sonda</param>
		/// <returns></returns>
		public long getDeviceIdFromMat(string matortag)
		{
			Logger.Info("RFIDManager.getDeviceIdFromMat (or Tag)");
			long id = DBUtilities.getDeviceIdFromMat(matortag, connectionDBString);
			if (id <= 0)
				id = DBUtilities.getDevIdFromTag(matortag, connectionDBString);
			return id;
		}

		public long checkUserValidity(string op)
		{
			Logger.Info("RFIDManager.checkUserValidity");
			return DBUtilities.checkUser(op, connectionDBString) ? 1 : 0;
		}

		/// <summary>
		/// inserisce nuovo ciclo, verifica solo che il passaggio dallo stato attuale del device 
		/// allo stato sporco sia consentito dalle transazioni configurate
		/// </summary>
		/// <param name="op">codice operatore</param>
		/// <param name="device_id">codice device</param>
		/// <param name="ExamId">id esame</param>
		/// <param name="check_transaction">se true viene verificata la transazione altrimenti no</param>
		/// <returns></returns>
		public long checkAndInsertManualCycle(int op, int device_id, int ExamId, bool check_transaction = true)
		{
			Logger.Info("In checkandinsertmanual cycle");
			try
			{
				Logger.Info($"Enter in checkandinsertmanual cycle op {op}, device_di {device_id}, examid {ExamId}, check_transaction {check_transaction}");
				KleanTrak.Model.Device device = Devices.FromID(device_id);
				Logger.Info($"device {device}");
				int current_status_id = StateTransactions.GetDeviceStatusId(device_id).Item1;
				Logger.Info($"current_status_id {current_status_id}");
				int start_cyle_id = StateTransactions.GetStateId(FixedStates.Start_cycle);
				Logger.Info($"start_cycle_id {start_cyle_id}");
				if (device == null)
					throw new ApplicationException($"Device with id {device_id} not found");
				Logger.Info($"checkAndInsertManualCycle start Operator: {op}, " +
					$"device: {device_id}, " +
					$"device.Id_sede: {device?.Id_sede}, " +
					$"ExamId: {ExamId}, " +
					$"current_status_id: {current_status_id}, " +
					$"start_cyle_id: {start_cyle_id}, " +
					$"check_transaction: {check_transaction}");
				//verifica transazione verso sporco
				if (check_transaction && !StateTransactions.IsValid(current_status_id, start_cyle_id, device.Id_sede))
				{
					Logger.Info($"checkAndInsertManualCycle RETURNS 0");
					return 0;
				}
				var tagid = DBUtilities.getDeviceTagFromId(device_id, connectionDBString);
				var operatortag = DBUtilities.getOperatorTagFromId(op, connectionDBString);
				if (!DBUtilities.insertnewCycle(tagid, operatortag, -1, 2, current_status_id, ExamId, connectionDBString))
				{
					Logger.Error("insert new cycle failed" + Environment.NewLine +
						$"tagid: {tagid}, operatortag: {operatortag}, examid: {ExamId}");
					return 0;
				}
				Logger.Info($"checkAndInsertManualCycle RETURNS 1");
				return 1;
			}
			catch (Exception e)
			{
				Logger.Error($"op: {op}, device: {device_id}, ExamId: {ExamId}",e);
				return 0;
			}
			finally
			{
				Logger.Info("end");
			}
		}

		public long forceInsertManualCycle(int op, int device, int ExamId)
		{
			Logger.Info("RFIDManager.forceInsertManualCycle...");
			return checkAndInsertManualCycle(op, device, ExamId, false);
		}

		public RFIDCycle GetCycleData(int iIDEsame, bool bPrevious)
		{
			Logger.Info("RFIDManager.GetCycleData");
			return DBUtilities.GetCycleData(iIDEsame, connectionDBString, bPrevious);
		}

		public RFIDCycle GetCycleDataFromEsameDispositivo(int iIDEsame, int iIDDispositivo, bool bPrevious)
		{
			Logger.Info("RFIDManager.GetCycleDataFromEsameDispositivo");
			var c = DBUtilities.GetCycleDataFromEsameDispositivo(iIDEsame, iIDDispositivo, connectionDBString, bPrevious);
			// !!!
			//System.IO.File.WriteAllText(@"c:\temp\GetCycleDataFromEsameDispositivo.xml", ObjectToXml(c));
			return c;

		}

		private string ObjectToXml(object output)
		{
			string objectAsXmlString;

			System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(output.GetType());
			using (System.IO.StringWriter sw = new System.IO.StringWriter())
			{
				try
				{
					xs.Serialize(sw, output);
					objectAsXmlString = sw.ToString();
				}
				catch (Exception ex)
				{
					objectAsXmlString = ex.ToString();
				}
			}

			return objectAsXmlString;
		}


		public RFIDCycleEx GetCycleDataFromEsameDispositivoEx(int iIDEsame, int iIDDispositivo, bool bPrevious)
		{
			Logger.Info("RFIDManager.GetCycleDataFromEsameDispositivoEx");
			return DBUtilities.GetCycleDataFromEsameDispositivoEx(iIDEsame, iIDDispositivo, connectionDBString, bPrevious);
		}

		public string getCycleAdditionalInfo(int examId, long previous)
		{
			Logger.Info("RFIDManager.getCycleAdditionalInfo");
			return DBUtilities.getCycleAdditionalInfo(examId, connectionDBString, previous > 0 ? true : false);
		}

		public string getSeparator()
		{
			Logger.Info("RFIDManager.getSeparator");
			return DBUtilities.SEPARATOR;
		}

		public void stopListening()
		{
			Logger.Info("RFIDManager.stopListening");
			StepManager.Finish();
		}

		void StepManager_DataCollectionCompleted(int success)
		{
			Logger.Info("RFIDManager.StepManager_DataCollectionCompleted");
			if (connectionDBString != null && connectionDBString != "" && m_completed_listener != null)
				m_completed_listener.Completed(success);
		}

		void StepManager_UserDetected(string id)
		{
			Logger.Info("RFIDManager.StepManager_UserDetected");
			if (connectionDBString != null && connectionDBString != "" && m_user_listener != null)
				m_user_listener.UserDetected(DBUtilities.getOperatorNameFromTag(id, connectionDBString), DBUtilities.getOperatorSurnameFromTag(id, connectionDBString), DBUtilities.getOperatorIdFromTag(id, connectionDBString));
		}

		void StepManager_DeviceDetected(string id)
		{
			Logger.Info("RFIDManager.StepManager_DeviceDetected");
			if (connectionDBString != null && connectionDBString != "" && m_device_listener != null)
				m_device_listener.DeviceDetected(DBUtilities.getDeviceDescFromTag(id, connectionDBString), DBUtilities.getDevIdFromTag(id, connectionDBString));
		}

		private void cleanInternalListenersReferences()
		{
			Logger.Info("RFIDManager.StepManager_DeviceDetected...");
			StepManager.BadgeDetected -= StepManager_BadgeDetected;
			StepManager.DataCollectionCompleted -= StepManager_DataCollectionCompleted;
			StepManager.DeviceDetected -= StepManager_DeviceDetected;
			StepManager.UserDetected -= StepManager_UserDetected;
			Logger.Info("...RFIDManager.StepManager_DeviceDetected");
		}
	}
}
