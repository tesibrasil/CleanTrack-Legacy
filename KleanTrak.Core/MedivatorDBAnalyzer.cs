using System;
using System.Collections.Generic;
using System.Data.Odbc;
using KleanTrak.Model;

namespace KleanTrak.Core
{
	public class MedivatorsDBAnalyzer : DBManager
	{
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool ExtractExamToClose(List<WasherCycle> CycleList, IEnumerable<DateTime> dateTimeList)
		{
			foreach (DateTime dt in dateTimeList)
			{
				WasherCycle cycle = GetCycleByDate(dt);

				if (cycle != null && cycle.EndTimestamp != null)
				{
					CycleList.Add(cycle);
				}
			}

			return true;
		}

		private void SortByEndDate(List<WasherCycle> list)
		{
			for (int i = 0; i < list.Count - 1; i++)
			{
				int min = i;

				for (int j = i + 1; j < list.Count; j++)
				{
					if (list[j].EndTimestamp < list[i].EndTimestamp)
					{
						min = j;
					}
				}

				WasherCycle temp = list[i];
				list[i] = list[min];
				list[min] = temp;
			}

		}

		public WasherCycle GetCycleByDate(DateTime date)
		{
			Logger.Info("GetCycleByDate...");
			WasherCycle result = null;

			string formattedDate = date.ToString("MM/dd/yyyy HH:mm:ss");
			string selectString = "SELECT * FROM CycleReports";
			selectString += " WHERE CreateDateTime = #" + formattedDate + "#";
            Logger.Info(selectString);
			// 

			OdbcConnection conn = new OdbcConnection(GetConnString());
			//
			try
			{
				conn.Open();
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				if (rdr.Read())
				{
					result = new WasherCycle();
					// result.Key = this.GetString(rdr, "Key");
					result.WasherExternalID = this.GetNumber(rdr, "MachineID").ToString();
					// result.Station = this.GetString(rdr, "Station");
					result.StartTimestamp = this.GetDateTime(rdr, "StartTimestamp");
					result.EndTimestamp = this.GetDateTime(rdr, "EndTimestamp");
					result.CycleCount = this.GetNumber(rdr, "CycleCount").ToString();
					// result.DisinfectantCycleCount = this.GetNumber(rdr, "DisinfectantCycleCount");
					result.OperatorStartExternalID = this.GetNumber(rdr, "OperatorID").ToString();
					result.OperatorEndExternalID = this.GetNumber(rdr, "OperatorID").ToString();
					// result.PatientID = this.GetNumber(rdr, "PatientID");
					// result.PhysicianID = this.GetNumber(rdr, "PhysicianID");
					result.DeviceExternalID = this.GetNumber(rdr, "ScopeID").ToString();
					// result.SSGRatio = this.GetString(rdr, "SSGRatio");
					result.FileDatetime = this.GetDateTime(rdr, "CreateDateTime");

					result.AdditionalInfoList = GetAdditionalInfo(this.GetString(rdr, "Key"));
				}

				rdr.Close();
				cmd.Dispose();

				conn.Close();
				conn.Dispose();
			}
			catch (Exception)
			{
				conn.Close();
				conn.Dispose();
				result = null;
			}

            Logger.Info("...GetCycleByDate");
			return result;
		}

		public WasherCycle GetCycleByStartDate(DateTime date, int scope)
		{
            Logger.Info("GetCycleByStartDate...");
			WasherCycle result = null;

			string formattedDate = date.ToString("MM/dd/yyyy HH:mm:ss");
			string selectString = "SELECT * FROM CycleReports";
			selectString += " WHERE StartTimestamp = #" + formattedDate + "# AND SCOPEID=" + scope;
            Logger.Info(selectString);
			// 

			OdbcConnection conn = new OdbcConnection(GetConnString());
			//
			try
			{
				conn.Open();
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				if (rdr.Read())
				{
					result = new WasherCycle();
					// result.Key = this.GetString(rdr, "Key");
					result.WasherExternalID = this.GetNumber(rdr, "MachineID").ToString();
					// result.Station = this.GetString(rdr, "Station");
					result.StartTimestamp = this.GetDateTime(rdr, "StartTimestamp");
					result.EndTimestamp = this.GetDateTime(rdr, "EndTimestamp");
					result.CycleCount = this.GetNumber(rdr, "CycleCount").ToString();
					// result.DisinfectantCycleCount = this.GetNumber(rdr, "DisinfectantCycleCount");
					result.OperatorStartExternalID = this.GetNumber(rdr, "OperatorID").ToString();
					result.OperatorEndExternalID = this.GetNumber(rdr, "OperatorID").ToString();
					// result.PatientID = this.GetNumber(rdr, "PatientID");
					// result.PhysicianID = this.GetNumber(rdr, "PhysicianID");
					result.DeviceExternalID = this.GetNumber(rdr, "ScopeID").ToString();
					// result.SSGRatio = this.GetString(rdr, "SSGRatio");
					result.FileDatetime = this.GetDateTime(rdr, "CreateDateTime");

					result.AdditionalInfoList = GetAdditionalInfo(this.GetString(rdr, "Key"));
				}

				rdr.Close();
				cmd.Dispose();

				conn.Close();
				conn.Dispose();
			}
			catch (Exception)
			{
				conn.Close();
				conn.Dispose();
				result = null;
			}

            Logger.Info("....GetCycleByStartDate");
			return result;
		}

		public WasherCycle GetCycleByEndDate(DateTime date, int scope)
		{
            Logger.Info("getCycleByEndDate...");
			WasherCycle result = null;

			string formattedDate = date.ToString("MM/dd/yyyy HH:mm:ss");
			string selectString = "SELECT * FROM CycleReports";
			selectString += " WHERE EndTimestamp = #" + formattedDate + "# AND SCOPEID=" + scope;
            Logger.Info(selectString);
			OdbcConnection conn = new OdbcConnection(GetConnString());
			//
			try
			{
				conn.Open();
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				if (rdr.Read())
				{
					result = new WasherCycle();
					// result.Key = this.GetString(rdr, "Key");
					result.WasherExternalID = this.GetNumber(rdr, "MachineID").ToString();
					// result.Station = this.GetString(rdr, "Station");
					result.StartTimestamp = this.GetDateTime(rdr, "StartTimestamp");
					result.EndTimestamp = this.GetDateTime(rdr, "EndTimestamp");
					result.CycleCount = this.GetNumber(rdr, "CycleCount").ToString();
					// result.DisinfectantCycleCount = this.GetNumber(rdr, "DisinfectantCycleCount");
					result.OperatorStartExternalID = this.GetNumber(rdr, "OperatorID").ToString();
					result.OperatorEndExternalID = this.GetNumber(rdr, "OperatorID").ToString();
					// result.PatientID = this.GetNumber(rdr, "PatientID");
					// result.PhysicianID = this.GetNumber(rdr, "PhysicianID");
					result.DeviceExternalID = this.GetNumber(rdr, "ScopeID").ToString();
					// result.SSGRatio = this.GetString(rdr, "SSGRatio");
					result.FileDatetime = this.GetDateTime(rdr, "CreateDateTime");

					result.AdditionalInfoList = GetAdditionalInfo(this.GetString(rdr, "Key"));
				}

				rdr.Close();
				cmd.Dispose();

				conn.Close();
				conn.Dispose();
			}
			catch (Exception)
			{
				conn.Close();
				conn.Dispose();
				result = null;
			}

            Logger.Info("....getCycleByEndDate");
			return result;
		}

		private DateTime GetLastStartDate(int id)
		{
            Logger.Info("GetLastStartDate...");
			DateTime max = DateTime.MinValue;
			string selectString = "SELECT MAX(StartTimestamp) AS MAXID FROM CycleReports where ScopeID=" + id;
            Logger.Info(selectString);
			OdbcConnection conn = new OdbcConnection(GetConnString());
			//
			try
			{

				conn.Open();
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				if (rdr.Read())
					max = this.GetDateTime(rdr, "MAXID");

				rdr.Close();
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
			catch (Exception)
			{
				conn.Close();
				conn.Dispose();

				return DateTime.MinValue;
			}

            Logger.Info("....GetLastStartDate");
			return max;
		}

		private DateTime GetLastEndDate(int id)
		{
            Logger.Info("GetLastEndDate...");
			DateTime max = DateTime.MinValue;
			string selectString = "SELECT MAX(EndTimestamp) AS MAXID FROM CycleReports where ScopeID=" + id;
            Logger.Info(selectString);
			OdbcConnection conn = new OdbcConnection(GetConnString());
			//
			try
			{
				conn.Open();
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				if (rdr.Read())
				{
					max = this.GetDateTime(rdr, "MAXID");
				}

				rdr.Close();
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
			catch (Exception)
			{
				conn.Close();
				conn.Dispose();
				return DateTime.MinValue;
			}

            Logger.Info("....GetLastEndDate");
			return max;
		}

		private List<int> GetScopes()
		{
            Logger.Info("GetScopes....");
			List<int> result = new List<int>();
            Logger.Info("Getting scopes id ...");
			string selectString = "SELECT DISTINCT(SCOPEID) FROM CycleReports";
            Logger.Info(selectString);

			OdbcConnection conn = new OdbcConnection(GetConnString());
			//
			try
			{
                Logger.Info("Opening Connection...");
				conn.Open();
                Logger.Info("Connection opened successfully!");
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					int id = GetNumber(rdr, "ScopeID");
					if (id >= 0)
						result.Add(id);
				}

				rdr.Close();
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
			catch (Exception e)
			{
                Logger.Error(e.ToString());
				conn.Close();
				conn.Dispose();
			}

            Logger.Info("....GetScopes");
			return result;
		}

		public List<WasherCycle> FilterStart()
		{
            Logger.Info("filterStart...");
			List<WasherCycle> resCycleList = new List<WasherCycle>();
			List<int> scopes = GetScopes();
			foreach (int scope in scopes)
			{
				DateTime lastStart = GetLastStartDate(scope);
				WasherCycle cyc = GetCycleByStartDate(lastStart, scope);
				resCycleList.Add(cyc);
			}

            Logger.Info("...filterStart: resCycleList:" + resCycleList.Count.ToString());
			return resCycleList;
		}

		public List<WasherCycle> FilterEnd()
		{
            Logger.Info("filterEnd...");
			List<WasherCycle> resCycleList = new List<WasherCycle>();
			List<int> scopes = GetScopes();
			foreach (int scope in scopes)
			{
				DateTime lastStart = GetLastEndDate(scope);

				WasherCycle cyc = GetCycleByEndDate(lastStart, scope);
				resCycleList.Add(cyc);

			}

            Logger.Info("...filterEnd resCycleList:" + resCycleList.Count.ToString());
			return resCycleList;
		}

		public List<WasherCycle> Extract(DateTime minDate)
		{
            Logger.Info("Extract...");
			List<WasherCycle> result = new List<WasherCycle>();
			List<WasherCycle> CycleList = FilterStart();

			foreach (WasherCycle cycle in CycleList)
			{
				if (cycle.FileDatetime > minDate)
					result.Add(cycle);
			}

            Logger.Info("....Extract: " + result.Count.ToString());
			return result;
		}

		private List<string> GetAdditionalIdList(string key)
		{
            Logger.Info("getAdditionalIdList...");
			List <string> additionalId = new List<string>();
            Logger.Info("Getting linked info for " + key + " ...");
			string selectString = "SELECT LogRecordKey FROM CycleReportLogRecords WHERE CycleReportKey='" + key + "'";
            Logger.Info(selectString);
			OdbcConnection conn = new OdbcConnection(GetConnString());
			//
			try
			{
				conn.Open();
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					string id = this.GetString(rdr, "LogRecordKey");
					if (id != null && id.Length > 0)
						additionalId.Add(id);
				}

				rdr.Close();
				cmd.Dispose();
				conn.Close();
				conn.Dispose();
			}
			catch (Exception ex)
			{
                Logger.Error(ex);
				conn.Close();
				conn.Dispose();
			}

            Logger.Info("...getAdditionalIdList");
			return additionalId;
		}

		private List<WasherCycleInfo> GetAdditionalInfoAux(List<string> additionalId)
		{
            Logger.Info("getAdditionalIdList...");
			List<WasherCycleInfo> result = new List<WasherCycleInfo>();

			foreach (string key in additionalId)
			{
				string selectString = "SELECT Key,Text,Data,Timestamp,IsAlarm FROM LogRecords WHERE key='" + key + "'";
                Logger.Info(selectString);
				OdbcConnection conn = new OdbcConnection(GetConnString());
				//

				try
				{
					conn.Open();
					OdbcCommand cmd = new OdbcCommand(selectString, conn);
					OdbcDataReader rdr = cmd.ExecuteReader();

					if (rdr.Read())
					{
						try
						{
							WasherCycleInfo info = new WasherCycleInfo();

							info.Description = this.GetString(rdr, "Text");
							info.Value = this.GetString(rdr, "Data");
							info.Date = this.GetDateTime(rdr, "Timestamp");

							/*
							info.Key_sub = "";
							try
							{
								info.Key_sub = this.GetString(rdr, "Key");
								if (info.Key_sub.Length >= 17)
									info.Key_sub = info.Key_sub.Substring(0, 17);
							}
							catch
							{
								info.Key_sub = "";
							}
							*/

							string temp = this.GetYN(rdr, "IsAlarm");
							info.isAlarm = temp == "True" ? true : false;
							result.Add(info);
						}
						catch (Exception ex)
						{
                            Logger.Error(ex);
						}
					}

					rdr.Close();
					cmd.Dispose();
					conn.Close();
					conn.Dispose();
				}
				catch (Exception ex)
				{
                    Logger.Error(ex);
					conn.Close();
					conn.Dispose();
				}
			}
			Logger.Info("....getAdditionalIdList");
			return result;
		}

		public List<WasherCycleInfo> GetAdditionalInfo(string key)
		{
			List<string> idList = GetAdditionalIdList(key);
			return GetAdditionalInfoAux(idList);
		}

	}
}

