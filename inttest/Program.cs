using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inttest
{
    class Program
    {
        public enum State
        {
            Unknown = 0,
            Clean = 1,
            Dirty = 2,
            Washing = 3,
            PreWashing = 4,
            Stored = 5,
            Expired = 6
        };

        public static int GetIntValue(OdbcDataReader odbcdatareader, string sField)
        {
            int nReturn = 0;


            if (odbcdatareader.IsDBNull(odbcdatareader.GetOrdinal(sField)))
                Console.WriteLine("e' null");


            try
            {
                int nOrdinal = odbcdatareader.GetOrdinal(sField);

                if (!odbcdatareader.IsDBNull(nOrdinal))
                    nReturn = odbcdatareader.GetInt32(nOrdinal);
            }
            catch (System.InvalidCastException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (nReturn == 0)
            {
                try
                {
                    int nOrdinal = odbcdatareader.GetOrdinal(sField);

                    if (!odbcdatareader.IsDBNull(nOrdinal))
                        nReturn = Convert.ToInt32(odbcdatareader.GetDecimal(nOrdinal));
                }
                catch (System.InvalidCastException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (nReturn == 0)
            {
                try
                {
                    int nOrdinal = odbcdatareader.GetOrdinal(sField);

                    if (!odbcdatareader.IsDBNull(nOrdinal))
                        nReturn = Convert.ToInt32(odbcdatareader.GetDouble(nOrdinal));
                }
                catch (System.InvalidCastException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            if (nReturn == 0)
            {
                try
                {
                    int nOrdinal = odbcdatareader.GetOrdinal(sField);

                    if (!odbcdatareader.IsDBNull(nOrdinal))
                        nReturn = Convert.ToInt32(odbcdatareader.GetInt64(nOrdinal));
                }
                catch (System.InvalidCastException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }


            return nReturn;
        }
        private static int getLastValidCycleId(int idDev, string connectionString, State state)
        {
            int id = -1;

            if (idDev > 0)
            {
                OdbcConnection connection = new OdbcConnection(connectionString);

                string query = "SELECT MAX(ID) AS MAXID FROM CICLI WHERE IDDISPOSITIVO = " + idDev.ToString();

                try
                {
                    connection.Open();
                    OdbcCommand cmdFCod = new OdbcCommand();
                    cmdFCod.Connection = connection;
                    cmdFCod.CommandText = query;
                    OdbcDataReader dr = null;
                    dr = cmdFCod.ExecuteReader();
                    dr.Read();
                    if (!dr.IsDBNull(0))
                    {
                        id = GetIntValue(dr, "MAXID");
                    }
                    else

                        dr.Close();
                }
                catch (Exception)
                {
                    id = -1;
                }

                string queryNew = "SELECT * FROM CICLI WHERE ID = " + id.ToString();

                try
                {
                    OdbcCommand cmdFCod = new OdbcCommand();
                    cmdFCod.Connection = connection;
                    cmdFCod.CommandText = queryNew;
                    OdbcDataReader dr = null;
                    dr = cmdFCod.ExecuteReader();
                    dr.Read();

                    switch (state)
                    {
                        case State.Clean:
                            id = (dr.IsDBNull(dr.GetOrdinal("DATAARMADIO")) &&
                                    dr.IsDBNull(dr.GetOrdinal("IDOPERATOREARMADIO"))) ?
                                    GetIntValue(dr, "ID") : -1;
                            break;

                        case State.PreWashing:
                            id = (dr.IsDBNull(dr.GetOrdinal("DATAPERSONALIZZABILE1")) &&
                                    dr.IsDBNull(dr.GetOrdinal("IDOPERATOREPERSONALIZZABILE1")) &&
                                    dr.IsDBNull(dr.GetOrdinal("DATAINIZIOSTERILIZZAZIONE")) &&
                                    dr.IsDBNull(dr.GetOrdinal("IDOPERATOREINIZIOSTERILIZZAZIO")) &&
                                    dr.IsDBNull(dr.GetOrdinal("DATAARMADIO")) &&
                                    dr.IsDBNull(dr.GetOrdinal("IDOPERATOREARMADIO"))) ?
                                    GetIntValue(dr, "ID") : -1;
                            break;

                        case State.Washing:
                            id = (dr.IsDBNull(dr.GetOrdinal("DATAINIZIOSTERILIZZAZIONE")) &&
                                    dr.IsDBNull(dr.GetOrdinal("IDOPERATOREINIZIOSTERILIZZAZIO")) &&
                                    dr.IsDBNull(dr.GetOrdinal("DATAARMADIO")) &&
                                    dr.IsDBNull(dr.GetOrdinal("IDOPERATOREARMADIO"))) ?
                                    GetIntValue(dr, "ID") : -1;
                            break;
                    }

                    if (dr != null && !dr.IsClosed)
                        dr.Close();

                }
                catch (Exception)
                {
                    id = -1;
                }

                connection.Close();
            }

            return id;
        }

        static void Main(string[] args)
        {
            getLastValidCycleId(317, "Driver={SQL Server};UID=sa;PWD=nautilus;SERVER=127.0.0.1,1433;DATABASE=klynntruck", State.PreWashing);
            // getLastValidCycleId(198, "Driver={Oracle em OraClient11g_home1_32bit};UID=SYS_CLEANTRACK;PWD=S1r10clean2015abr;DBQ=TASY.HSL.PVT", State.PreWashing);
        }
    }
}
