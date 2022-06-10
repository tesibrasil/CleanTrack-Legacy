using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;

namespace WindowsService1
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
                OdbcCommand cmd = new OdbcCommand(selectString, conn);
                OdbcDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    max = this.GetDateTime(rdr, "MAXID");
                }

                rdr.Close();
                cmd.Dispose();
            }
            catch (Exception /*e*/)
            {
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
            catch (Exception /*e*/)
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
            catch (Exception /*e*/)
            {
                return false;
            }

            return true;
        }
    }
}
