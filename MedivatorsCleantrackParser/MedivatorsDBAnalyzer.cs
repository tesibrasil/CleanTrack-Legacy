using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;
using System.Text;
using System.Threading;


namespace MedivatorsCleantrackParser
{
	public class MedivatorsDBAnalyzer : DBManager
	{
		public bool ExtractExamToClose(List<Cycle> CycleList, IEnumerable<DateTime> dateTimeList)
		{
			foreach (DateTime dt in dateTimeList)
			{
				Cycle cycle = getCycleByDate(dt);

				if (cycle != null && cycle.EndTimestamp != null)
				{
					CycleList.Add(cycle);
				}
			}

			return true;
		}


		private void SortByEndDate(List<Cycle> list)
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

				Cycle temp = list[i];
				list[i] = list[min];
				list[min] = temp;
			}

		}

		public Cycle getCycleByDate(DateTime date)
		{
			writeLog("GetCycleByDate...");
			Cycle result = null;

			string formattedDate = date.ToString("MM/dd/yyyy HH:mm:ss");
			string selectString = "SELECT * FROM CycleReports";
			selectString += " WHERE CreateDateTime = #" + formattedDate + "#";
			writeLog(selectString);
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
					result = new Cycle();
					result.Key = this.GetString(rdr, "Key");
					result.MachineID = this.GetNumber(rdr, "MachineID");
					result.Station = this.GetString(rdr, "Station");
					result.StartTimestamp = this.GetDateTime(rdr, "StartTimestamp");
					result.EndTimestamp = this.GetDateTime(rdr, "EndTimestamp");
					result.CycleCount = this.GetNumber(rdr, "CycleCount");
					result.DisinfectantCycleCount = this.GetNumber(rdr, "DisinfectantCycleCount");
					result.OperatorID = this.GetNumber(rdr, "OperatorID");
					result.PatientID = this.GetNumber(rdr, "PatientID");
					result.PhysicianID = this.GetNumber(rdr, "PhysicianID");
					result.ScopeID = this.GetNumber(rdr, "ScopeID");
					result.SSGRatio = this.GetString(rdr, "SSGRatio");
					result.CreateDateTime = this.GetDateTime(rdr, "CreateDateTime");

					result.AdditionalInfoList = getAdditionalInfo(result.Key);

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

			writeLog("...GetCycleByDate");
			return result;
		}

		public Cycle getCycleByStartDate(DateTime date, int scope)
		{
			writeLog("GetCycleByStartDate...");
			Cycle result = null;

			string formattedDate = date.ToString("MM/dd/yyyy HH:mm:ss");
			string selectString = "SELECT * FROM CycleReports";
			selectString += " WHERE StartTimestamp = #" + formattedDate + "# AND SCOPEID=" + scope;
			writeLog(selectString);
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
					result = new Cycle();
					result.Key = this.GetString(rdr, "Key");
					result.MachineID = this.GetNumber(rdr, "MachineID");
					result.Station = this.GetString(rdr, "Station");
					result.StartTimestamp = this.GetDateTime(rdr, "StartTimestamp");
					result.EndTimestamp = this.GetDateTime(rdr, "EndTimestamp");
					result.CycleCount = this.GetNumber(rdr, "CycleCount");
					result.DisinfectantCycleCount = this.GetNumber(rdr, "DisinfectantCycleCount");
					result.OperatorID = this.GetNumber(rdr, "OperatorID");
					result.PatientID = this.GetNumber(rdr, "PatientID");
					result.PhysicianID = this.GetNumber(rdr, "PhysicianID");
					result.ScopeID = this.GetNumber(rdr, "ScopeID");
					result.SSGRatio = this.GetString(rdr, "SSGRatio");
					result.CreateDateTime = this.GetDateTime(rdr, "CreateDateTime");
					result.AdditionalInfoList = getAdditionalInfo(result.Key);
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

			writeLog("....GetCycleByStartDate");
			return result;
		}

		public Cycle getCycleByEndDate(DateTime date, int scope)
		{
			writeLog("getCycleByEndDate...");
			Cycle result = null;

			string formattedDate = date.ToString("MM/dd/yyyy HH:mm:ss");
			string selectString = "SELECT * FROM CycleReports";
			selectString += " WHERE EndTimestamp = #" + formattedDate + "# AND SCOPEID=" + scope;
			writeLog(selectString);
			OdbcConnection conn = new OdbcConnection(GetConnString());
			//
			try
			{

				conn.Open();
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				if (rdr.Read())
				{
					result = new Cycle();
					result.Key = this.GetString(rdr, "Key");
					result.MachineID = this.GetNumber(rdr, "MachineID");
					result.Station = this.GetString(rdr, "Station");
					result.StartTimestamp = this.GetDateTime(rdr, "StartTimestamp");
					result.EndTimestamp = this.GetDateTime(rdr, "EndTimestamp");
					result.CycleCount = this.GetNumber(rdr, "CycleCount");
					result.DisinfectantCycleCount = this.GetNumber(rdr, "DisinfectantCycleCount");
					result.OperatorID = this.GetNumber(rdr, "OperatorID");
					result.PatientID = this.GetNumber(rdr, "PatientID");
					result.PhysicianID = this.GetNumber(rdr, "PhysicianID");
					result.ScopeID = this.GetNumber(rdr, "ScopeID");
					result.SSGRatio = this.GetString(rdr, "SSGRatio");
					result.CreateDateTime = this.GetDateTime(rdr, "CreateDateTime");

					result.AdditionalInfoList = getAdditionalInfo(result.Key);

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

			writeLog("....getCycleByEndDate");
			return result;
		}

		private DateTime GetLastStartDate(int id)
		{
			writeLog("GetLastStartDate...");
			DateTime max = DateTime.MinValue;
			string selectString = "SELECT MAX(StartTimestamp) AS MAXID FROM CycleReports where ScopeID=" + id;
			writeLog(selectString);
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

			writeLog("....GetLastStartDate");
			return max;
		}

		private DateTime GetLastEndDate(int id)
		{
			writeLog("GetLastEndDate...");
			DateTime max = DateTime.MinValue;
			string selectString = "SELECT MAX(EndTimestamp) AS MAXID FROM CycleReports where ScopeID=" + id;
			writeLog(selectString);
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

			writeLog("....GetLastEndDate");
			return max;
		}

		private List<int> GetScopes()
		{
			writeLog("GetScopes....");
			List<int> result = new List<int>();
			writeLog("Getting scopes id ...");
			string selectString = "SELECT DISTINCT(SCOPEID) FROM CycleReports";
			writeLog(selectString);

			OdbcConnection conn = new OdbcConnection(GetConnString());
			//
			try
			{
				writeLog("Opening Connection...");
				conn.Open();
				writeLog("Connection opened successfully!");
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					int id = this.GetNumber(rdr, "ScopeID");
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
				writeLog(e.ToString());
				conn.Close();
				conn.Dispose();
			}

			writeLog("....GetScopes");
			return result;
		}

		public List<Cycle> filterStart()
		{
			writeLog("filterStart...");
			List<Cycle> resCycleList = new List<Cycle>();
			List<int> scopes = GetScopes();
			foreach (int scope in scopes)
			{
				DateTime lastStart = GetLastStartDate(scope);
				Cycle cyc = getCycleByStartDate(lastStart, scope);
				resCycleList.Add(cyc);
			}

			writeLog("...filterStart: resCycleList:" + resCycleList.Count.ToString());
			return resCycleList;
		}

		public List<Cycle> filterEnd()
		{
			writeLog("filterEnd...");
			List<Cycle> resCycleList = new List<Cycle>();
			List<int> scopes = GetScopes();
			foreach (int scope in scopes)
			{
				DateTime lastStart = GetLastEndDate(scope);

				Cycle cyc = getCycleByEndDate(lastStart, scope);
				resCycleList.Add(cyc);

			}

			writeLog("...filterEnd resCycleList:" + resCycleList.Count.ToString());
			return resCycleList;
		}

		public List<Cycle> Extract(DateTime minDate)
		{
			writeLog("Extract...");
			List<Cycle> result = new List<Cycle>();
			List<Cycle> CycleList = filterStart();

			foreach (Cycle cycle in CycleList)
			{
				if (cycle.CreateDateTime > minDate)
					result.Add(cycle);
			}

			writeLog("....Extract: " + result.Count.ToString());
			return result;
		}


		private List<string> getAdditionalIdList(string key)
		{
			writeLog("getAdditionalIdList...");
         List <string> additionalId = new List<string>();
			writeLog("Getting linked info for " + key + " ...");
			string selectString = "SELECT LogRecordKey FROM CycleReportLogRecords WHERE CycleReportKey='" + key + "'";
			writeLog(selectString);
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
			catch (Exception e)
			{
				writeLog(e.ToString());
				conn.Close();
				conn.Dispose();
			}

			writeLog("...getAdditionalIdList");
			return additionalId;
		}

		private List<AdditionalInfo> getAdditionalInfoAux(List<string> additionalId)
		{
			writeLog("getAdditionalIdList...");
			List<AdditionalInfo> result = new List<AdditionalInfo>();

			foreach (string key in additionalId)
			{
				string selectString = "SELECT Key,Text,Data,Timestamp,IsAlarm FROM LogRecords WHERE key='" + key + "'";
				writeLog(selectString);
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
							AdditionalInfo info = new AdditionalInfo();

							info.Description = this.GetString(rdr, "Text");
							info.Value = this.GetString(rdr, "Data");
							info.Date = this.GetDateTime(rdr, "Timestamp");

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

							string temp = this.GetYN(rdr, "IsAlarm");
							info.isAlarm = temp == "True" ? true : false;
							result.Add(info);
						}
						catch (Exception e)
						{
							writeLog(e.ToString());
						}
					}

					rdr.Close();
					cmd.Dispose();
					conn.Close();
					conn.Dispose();
				}
				catch (Exception e)
				{
					writeLog(e.ToString());
					conn.Close();
					conn.Dispose();
				}
			}
			writeLog("....getAdditionalIdList");
			return result;
		}

		public List<AdditionalInfo> getAdditionalInfo(string key)
		{
			List<string> idList = getAdditionalIdList(key);
			return getAdditionalInfoAux(idList);
		}


		private void writeLog(string text)
		{
			try
			{
				string logName = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
				StreamWriter sw = new StreamWriter(logName, true);
				sw.WriteLine(DateTime.Now.ToString("HH:mm") + " - " + text);
				sw.Close();
			}
			catch
			{
			}
		}
	}
}

