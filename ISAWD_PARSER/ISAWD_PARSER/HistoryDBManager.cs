using System;
using System.Data.Odbc;

namespace ISAWD_PARSER
{
	public class HistoryDBManager : DBManager
    {
        OdbcConnection conn = null;

        public DateTime GetLastDate()
        {
            DateTime max = DateTime.MinValue;
            string selectString = "SELECT MAX(CYCLEDATE) AS MAXID FROM CYCLESHISTORY";

            OpenDB();

            try
            {
				using (OdbcCommand cmd = new OdbcCommand(selectString, conn))
				{
					using (OdbcDataReader rdr = cmd.ExecuteReader())
					{
						if (rdr.Read())
							max = GetDateTime(rdr, "MAXID");

						rdr.Close();
					}
				}
            }
            catch (Exception exc)
            {
				Program.WriteLog("GetLastDate", "ERROR", exc.Message);
				Program.WriteLog("GetLastDate", "ERROR", exc.StackTrace);
            }

            CloseDB();

            return max;
        }

		private bool OpenDB()
        {
            //
            try
            {
                conn = new OdbcConnection(GetConnString());
                conn.Open();
            }
            catch (Exception exc)
            {
				Program.WriteLog("OpenDB", "ERROR", exc.Message);
				Program.WriteLog("OpenDB", "ERROR", exc.StackTrace);
                return false;
            }

            return true;
        }

        private void CloseDB()
        {
            conn.Close();
        }

        public bool AddClosedCycle(string sFileName)
        {
			bool bReturn = false;

			DateTime date = Program.FromFilenameToDate(sFileName);

			string formattedDate = date.ToString("MM/dd/yyyy HH:mm:ss");
            string insQuery = "INSERT INTO CYCLESHISTORY (CYCLEDATE, COMPLETED) VALUES (#" + formattedDate + "#, TRUE)";

            try
            {
				OpenDB();

				OdbcCommand cmd = new OdbcCommand(insQuery, conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                CloseDB();

				bReturn = true;
            }
            catch (Exception exc)
            {
				Program.WriteLog("AddAndCloseCycle", "ERROR", exc.Message);
				Program.WriteLog("AddAndCloseCycle", "ERROR", exc.StackTrace);
            }

            return bReturn;
		}

		public void DeleteAllCycle()
        {
            OpenDB();

            try
            {
                OdbcCommand cmd = new OdbcCommand("DELETE FROM CYCLESHISTORY", conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                CloseDB();
            }
            catch (Exception exc)
            {
				Program.WriteLog("DeleteAllCycle", "ERROR", exc.Message);
				Program.WriteLog("DeleteAllCycle", "ERROR", exc.StackTrace);
            }

            CloseDB();
        }
    }
}
