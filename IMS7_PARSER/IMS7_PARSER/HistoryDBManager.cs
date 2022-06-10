using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Text;


namespace IMS7_PARSER
{
    public class HistoryDBManager : DBManager
    {
        OdbcConnection conn = null;

        public int GetLastIndex(DateTime data)
        {
            int max = -1;
            string formattedDate = data.ToString("MM/dd/yyyy HH:mm:ss");
            string selectString = "SELECT MAX(INDEX) AS MAXID FROM CYCLESHISTORY WHERE DATA>=#"+formattedDate+"#";

            OpenDB();
            //
            try
            {
                OdbcCommand cmd = new OdbcCommand(selectString, conn);
                OdbcDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    max = this.GetAutoNumber(rdr, "MAXID");
                }

                rdr.Close();
                cmd.Dispose();
            }
            catch (Exception)
            {
                max = -1;
            }

            CloseDB();

            return max;
        }

        public DateTime GetLastDate()
        {
            DateTime max = DateTime.MinValue;
            string selectString = "SELECT MAX(DATA) AS MAXID FROM CYCLESHISTORY";

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
                max = DateTime.MinValue;
            }

            CloseDB();

            return max;
        }


        public bool IsFilePresent(string file)
        {
            int cnt = -1;
            string selectString = "SELECT COUNT(*) AS CNT FROM CYCLESHISTORY WHERE FILENAME='"+file+"'";

            OpenDB();
            //
            try
            {
                OdbcCommand cmd = new OdbcCommand(selectString, conn);
                OdbcDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    cnt = this.GetAutoNumber(rdr, "CNT");
                }

                rdr.Close();
                cmd.Dispose();
            }
            catch (Exception)
            {
                cnt = -1;
            }

            CloseDB();

            return cnt > 0;
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
        public bool AddCycle(int index,string filename,DateTime data)
        {

            string formattedDate = data.ToString("MM/dd/yyyy HH:mm:ss");
            string insQuery = "INSERT INTO cycleshistory (INDEX,FILENAME,DATA) VALUES (" + index + ",'"+filename+"',#"+formattedDate+"#)";

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


        public bool CloseCycle(int index,DateTime data)
        {
           
            string formattedDate = data.ToString("MM/dd/yyyy HH:mm:ss");
            string updateQuery = "UPDATE CYCLESHISTORY SET COMPLETED=TRUE WHERE INDEX = "+index+" AND DATA=#"+formattedDate+"#";

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
