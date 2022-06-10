using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace KleanTrak.Core
{
	public class MedivatorHistoryDBManager : DBManager
	{
		OdbcConnection conn = null;

		public DateTime GetLastDate()
		{
			DateTime max = DateTime.MinValue;
			string selectString = "SELECT MAX(CYCLEDATE) AS MAXID FROM CYCLESHISTORY";

			OpenDB();
			//
			try
			{
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					max = this.GetDateTime(rdr, "MAXID");
				}

				rdr.Close();
				cmd.Dispose();
			}
			catch (Exception)
			{
			}

			CloseDB();

			return max;
		}

		public List<DateTime> GetExamToClose()
		{
			List<DateTime> dateList = new List<DateTime>();
			string selectString = "SELECT CYCLEDATE FROM CYCLESHISTORY WHERE COMPLETED=FALSE";

			OpenDB();
			//
			try
			{
				OdbcCommand cmd = new OdbcCommand(selectString, conn);
				OdbcDataReader rdr = cmd.ExecuteReader();

				while (rdr.Read())
				{
					DateTime date = this.GetDateTime(rdr, "CYCLEDATE");
					if (date > DateTime.MinValue)
					{
						dateList.Add(date);
					}
				}

				rdr.Close();
				cmd.Dispose();
			}
			catch (Exception)
			{
			}

			CloseDB();

			return dateList;
		}

		private bool OpenDB()
		{
			//
			try
			{
				conn = new OdbcConnection(GetConnString());
				conn.Open();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		private void CloseDB()
		{

			conn.Close();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serie"></param>
		/// <returns></returns>
		public bool AddCycle(DateTime date)
		{
			//
			string formattedDate = date.ToString("MM/dd/yyyy HH:mm:ss");
			string insQuery = "INSERT INTO CYCLESHISTORY (CYCLEDATE) VALUES (#" + formattedDate + "#)";

			OpenDB();

			try
			{
				OdbcCommand cmd = new OdbcCommand(insQuery, conn);
				cmd.ExecuteNonQuery();
				cmd.Dispose();
				CloseDB();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public bool CloseCycle(DateTime date)
		{
			string formattedDate = date.ToString("MM/dd/yyyy HH:mm:ss");
			string updateQuery = "UPDATE CYCLESHISTORY SET COMPLETED=TRUE WHERE CYCLEDATE = (#" + formattedDate + "#)";

			OpenDB();

			try
			{
				OdbcCommand cmd = new OdbcCommand(updateQuery, conn);
				cmd.ExecuteNonQuery();
				cmd.Dispose();
				CloseDB();
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}
	}
}

