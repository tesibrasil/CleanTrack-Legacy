using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Data;
using System.Data.Odbc;
using System.Globalization;

namespace amrfidmgrex
{
	[ComVisible(true)]
	[Guid("7E7785A0-A119-3550-BBD1-9D1A14106B60")]
	public class DBUtilities
	{
		protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public const string SEPARATOR = "$3P4R470R";

		public static int GetIntValue(OdbcDataReader odbcdatareader, string sField)
		{
			int nReturn = 0;

			try
			{
				int nOrdinal = odbcdatareader.GetOrdinal(sField);

				if (!odbcdatareader.IsDBNull(nOrdinal))
					nReturn = odbcdatareader.GetInt32(nOrdinal);
			}
			catch (System.InvalidCastException)
			{
				Logger.Info("********* DbUtilities.GetIntValue getint32 fails");
			}
			catch (Exception)
			{
				//MessageBox.Show(e.Message);
			}

			if (nReturn == 0)
			{
				try
				{
					int nOrdinal = odbcdatareader.GetOrdinal(sField);

					if (!odbcdatareader.IsDBNull(nOrdinal))
						nReturn = Convert.ToInt32(odbcdatareader.GetDecimal(nOrdinal));
				}
				catch (System.InvalidCastException)
				{
					Logger.Info("********* DbUtilities.GetDecimal getint32 fails");
				}
				catch (Exception)
				{
					//MessageBox.Show(e.Message);
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
				catch (System.InvalidCastException)
				{
					Logger.Info("********* DbUtilities.GetDouble getint32 fails");
				}
				catch (Exception)
				{
					//MessageBox.Show(e.Message);
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
				catch (System.InvalidCastException)
				{
					Logger.Info("********* DbUtilities.GetInt64 getint32 fails");
				}
				catch (Exception)
				{
					//MessageBox.Show(e.Message);
				}
			}

			Logger.Info($"********* DbUtilities.return value {nReturn}");
			return nReturn;
		}

		public static bool SetOvertTimeState(int item, string odbcConn)
		{
			OdbcConnection connection = new OdbcConnection(odbcConn);
			OdbcDataReader dr = null;

			bool result = false;

			try
			{
				connection.Open();
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = connection;
				cmdFCod.CommandText = "UPDATE DISPOSITIVI SET STATO = 2 WHERE ID = " + item.ToString();
				Logger.Info(cmdFCod.CommandText);
				cmdFCod.ExecuteNonQuery();
				result = true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}

			if (dr != null && !dr.IsClosed)
				dr.Close();

			connection.Close();

			return result;
		}

		public static bool GetCleanDate(ref DateTime EndWashDateTime, int item, string odbcConn)
		{
			OdbcConnection connection = new OdbcConnection(odbcConn);
			OdbcDataReader dr = null;

			bool result = false;
			try
			{
				connection.Open();
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = connection;
				cmdFCod.CommandText = "SELECT DATAFINESTERILIZZAZIONE FROM CICLI WHERE IDDISPOSITIVO = " + item.ToString() + " ORDER BY ID DESC";
				Logger.Info(cmdFCod.CommandText);
				dr = cmdFCod.ExecuteReader();
				dr.Read();
				string dtStorage = "";
				if (!dr.IsDBNull(0))
					dtStorage = dr.GetString(0);

				if (dtStorage.Length >= 8)
				{
					EndWashDateTime = new DateTime(Int32.Parse(dtStorage.Substring(0, 4)), Int32.Parse(dtStorage.Substring(4, 2)), Int32.Parse(dtStorage.Substring(6, 2)));
					result = true;
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}

			if (dr != null && !dr.IsClosed)
				dr.Close();

			connection.Close();
			return result;
		}

		public static int getStateFromTag(string Tag, string connectionString)
		{
			Logger.Info("DBUtil.getStateFromTag");
			int id = -1;

			if (Tag != "")
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT STATO FROM DISPOSITIVI WHERE TAG = '" + Tag + "' AND ELIMINATO = 0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();

					if (!dr.IsDBNull(0))
						id = GetIntValue(dr, "STATO");
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					id = -1;
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return id;
		}

		public static int getDevIdFromTag(string Tag, string connectionString)
		{
			Logger.Info("DBUtil.getDevIdFromTag...");
			int id = -1;

			if (Tag == null)
				return -1;

			if (Tag != "")
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT ID FROM DISPOSITIVI WHERE TAG = '" + Tag + "' AND DISMESSO = 0 AND Dispositivi.eliminato = 0 ";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();

					if (!dr.IsDBNull(0))
						id = GetIntValue(dr, "ID");
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					id = -1;
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			Logger.Info("...DBUtil.getDevIdFromTag");
			return id;
		}

		public static int getOperatorIdFromTag(string Tag, string connectionString)
		{
			Logger.Info("DBUtil.getOperatoreIdFromTag");

			int id = -1;

			if (Tag == null)
				return -1;

			if (Tag != "")
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT ID FROM OPERATORI WHERE TAG= '" + Tag + "' AND DISATTIVATO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();

					if (!dr.IsDBNull(0))
						id = GetIntValue(dr, "ID");
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					id = -1;
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return id;
		}

		public static int getOperatorIdFromAnagraphicData(string AnagraphicData, string connectionString)
		{
			Logger.Info("DBUtil.getOperatorIdFromAnagraphicData");

			int id = -1;

			if (AnagraphicData != "")
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT ID FROM OPERATORI WHERE COGNOME + ' ' + NOME = '" + AnagraphicData.Replace("'", "''") + "' AND DISATTIVATO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();

					if (!dr.IsDBNull(0))
						id = GetIntValue(dr, "ID");
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					id = -1;
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return id;
		}

		public static int getOperatorIdFromTag(string Tag, OdbcConnection conn)
		{
			Logger.Info("DBUtil.getOperatoreIdFromTag");

			int id = -1;

			if (Tag != "")
			{
				OdbcDataReader dr = null;

				try
				{
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = conn;
					cmdFCod.CommandText = "SELECT ID FROM OPERATORI WHERE TAG= '" + Tag + "' AND DISATTIVATO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();

					if (!dr.IsDBNull(0))
						id = GetIntValue(dr, "ID");
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					id = -1;
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();
			}

			return id;
		}

		public static int getOperatorIdFromMat(string Mat, string connectionString)
		{
			Logger.Info("DBUtil.getOperatoreIdFromMat");
			if (Mat == "")
				return -111;
			OdbcConnection connection = new OdbcConnection(connectionString);
			OdbcCommand cmd = null;
			OdbcDataReader dr = null;
			try
			{
				connection.Open();
				cmd = new OdbcCommand();
				cmd.Connection = connection;
				cmd.CommandText = $"SELECT ID FROM OPERATORI WHERE MATRICOLA='{Mat.Trim()}' AND DISATTIVATO=0";
				Logger.Info($"mat: {Mat}{Environment.NewLine}" +
					$"connstr:{connectionString}{Environment.NewLine}" +
					$"commandtext: {cmd.CommandText}");
				dr = cmd.ExecuteReader();
				dr.Read();
				if (!dr.IsDBNull(0)) 
					return GetIntValue(dr, "ID");
				return -222;
			}
			catch (Exception ex)
			{
				Logger.Error($"mat: {Mat}{Environment.NewLine}" +
					$"connstr: {connectionString}{Environment.NewLine}" +
					$"Exception: {ex}");
				return -333;
			}
			finally
			{
				if (dr != null && !dr.IsClosed)
					dr.Close();
				if (cmd != null)
					cmd.Dispose();
				if (connection != null)
					connection.Close();
			}
		}

		public static string getDeviceDescFromTag(string tag, string connectionString)
		{
			Logger.Info("DBUtil.getDeviceDescFromTag");
			string desc = "";

			if (tag != null && tag != "")
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT DESCRIZIONE FROM DISPOSITIVI WHERE TAG= '" + tag + "' AND DISMESSO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();

					if (!dr.IsDBNull(0))
						desc = dr.GetString(0);
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					desc = "";
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return desc;
		}

		public static string getDeviceDescFromId(int id, string connectionString)
		{
			Logger.Info("DBUtil.getDeviceDescFromId");
			string desc = "";

			if (id > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT DESCRIZIONE FROM DISPOSITIVI WHERE ID=" + id + " AND DISMESSO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
						desc = dr.GetString(0);
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					desc = "";
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return desc;
		}

		public static string getDeviceMatFromId(int id, string connectionString)
		{
			Logger.Info("DBUtil.getDeviceMatFromId");
			string desc = "";

			if (id > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT MATRICOLA FROM DISPOSITIVI WHERE ID=" + id + " AND DISMESSO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();

					if (!dr.IsDBNull(0))
						desc = dr.GetString(0);
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					desc = "";
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return desc;
		}

		public static string getSterDescFromId(int id, string connectionString)
		{
			Logger.Info("DBUtil.getSterDescFromId");
			string desc = "";

			if (id > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT DESCRIZIONE FROM ARMADI_LAVATRICI WHERE ID=" + id + " AND DISMESSO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
					{
						desc = dr.GetString(0);
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					desc = "";
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return desc;
		}

		public static string getOperatorNameFromTag(string Tag, string connectionString)
		{
			Logger.Info("DBUtil.getOperatorNameFromTag");
			string nome = "";

			if (Tag != "")
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT NOME FROM OPERATORI WHERE TAG= '" + Tag + "' AND DISATTIVATO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
					{
						nome = dr.GetString(0);
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					nome = "";
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}


			return nome;

		}

		public static string getOperatorNameFromId(int id, string connectionString)
		{
			Logger.Info("DBUtil.getOperatorNameFromId");
			string nome = "";

			if (id > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT NOME FROM OPERATORI WHERE ID=" + id + " AND DISATTIVATO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
					{
						nome = dr.GetString(0);
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
					nome = "";
				}
				if (dr != null && !dr.IsClosed)
					dr.Close();
				connection.Close();
			}
			return nome;
		}

		public static string getOperatorSurnameFromTag(string Tag, string connectionString)
		{
			Logger.Info("DBUtil.getOperatorSurnameFromTag");
			string cognome = "";

			if (Tag != "")
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT COGNOME FROM OPERATORI WHERE TAG= '" + Tag + "' AND DISATTIVATO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
						cognome = dr.GetString(0);
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					cognome = "";

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return cognome;

		}

		public static string getOperatorSurnameFromId(int id, string connectionString)
		{
			Logger.Info("DBUtil.getOperatorSurnameFromId");
			string cognome = "";

			if (id > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT COGNOME FROM OPERATORI WHERE ID=" + id + " AND DISATTIVATO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
						cognome = dr.GetString(0);
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					cognome = "";
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return cognome;

		}

		public static string getOperatorTagFromId(int id, string connectionString)
		{
			Logger.Info("DBUtil.getOperatorTagFromId");
			string tag = "";

			if (id > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT TAG FROM OPERATORI WHERE ID=" + id + " AND DISATTIVATO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
						tag = dr.GetString(0);
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					tag = "";

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return tag;

		}

		public static bool checkUser(string id, string connectionString)
		{
			Logger.Info("DBUtil.checkUser");
			if (id != null && id != "")
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT ID FROM OPERATORI WHERE MATRICOLA= '" + id + "' AND DISATTIVATO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
						return true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return false;

		}

		public static string getDeviceTagFromId(int id, string connectionString)
		{
			Logger.Info("DBUtil.getDeviceTagFromId");
			string tag = "";

			if (id > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT TAG FROM DISPOSITIVI WHERE ID=" + id + " AND DISMESSO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
					{

						tag = dr.GetString(0);

					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					tag = "";
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return tag;

		}

		public static int getCleanerIdFromCode(string Code, string connectionString)
		{
			Logger.Info("DBUtil.getCleanerIdFromCode");
			int id = -1;

			if (Code != "")
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT ID FROM ARMADI_LAVATRICI WHERE MATRICOLA = '" + Code + "'";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
					{
						id = GetIntValue(dr, "ID");
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					id = -1;

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return id;

		}

		public static List<int> getCleanDevice(string connectionString)
		{
			List<int> result = new List<int>();

			OdbcConnection connection = new OdbcConnection(connectionString);
			OdbcDataReader dr = null;

			try
			{
				connection.Open();
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = connection;
				cmdFCod.CommandText = "SELECT ID FROM DISPOSITIVI WHERE STATO = 1 AND DISMESSO = 0 AND Dispositivi.eliminato = 0 ";
				Logger.Info(cmdFCod.CommandText);
				dr = cmdFCod.ExecuteReader();
				while (dr.Read())
					result.Add(GetIntValue(dr, "ID"));
			}
			catch (Exception ex)
			{
				Logger.Error(ex);

			}

			if (dr != null && !dr.IsClosed)
				dr.Close();

			connection.Close();

			return result;
		}

		public static int getMachineIdFromMat(string sMatricola, string connectionString)
		{
			Logger.Info("DBUtil.getMachineIdFromMat");
			int id = -1;

			if (sMatricola.Length > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					// cmdFCod.CommandText = "SELECT ID FROM ARMADI_LAVATRICI WHERE MATRICOLA LIKE '" + sMatricola + "' AND DISMESSO = 0";
					cmdFCod.CommandText = "SELECT ID FROM ARMADI_LAVATRICI WHERE MATRICOLA LIKE '" + sMatricola + "' AND DISMESSO = 0 AND Dispositivi.eliminato = 0 ";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
					{
						id = GetIntValue(dr, "ID");
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					id = -1;
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return id;
		}

		public static int getDeviceIdFromMat(string Mat, string connectionString)
		{
			Logger.Info("DBUtil.getDeviceIdFromMat");
			int id = -1;

			if (Mat != "")
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT ID FROM DISPOSITIVI WHERE MATRICOLA= '" + Mat + "' AND DISMESSO=0";
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
					{

						id = GetIntValue(dr, "ID");

					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					id = -1;
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return id;

		}

		public static int getDeviceIdFromExam(int Exam, string connectionString)
		{
			Logger.Info("DBUtil.getDeviceIdFromExam");
			int id = -1;

			if (Exam > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					cmdFCod.CommandText = "SELECT IDDISPOSITIVO FROM CICLI WHERE IDESAME= " + Exam;
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					dr.Read();
					if (!dr.IsDBNull(0))
					{

						id = GetIntValue(dr, "IDDISPOSITIVO");

					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					id = -1;
				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				connection.Close();
			}

			return id;

		}

		public static int getPreviousMaxCycle(int deviceId, int currentCycleId, string connectionString)
		{
			Logger.Info("DBUtil.getPreviousMaxCycle");
			int MAX = 0;

			OdbcConnection connection = new OdbcConnection(connectionString);
			OdbcDataReader dr = null;

			try
			{

				connection.Open();
				OdbcCommand cmd = new OdbcCommand();
				cmd.Connection = connection;
				cmd.CommandText = "SELECT MAX(ID) AS MAXVAL FROM CICLI WHERE " + /*DATAINIZIOSTERILIZZAZIONE IS NOT NULL AND*/ " IDDISPOSITIVO=" + deviceId + " AND ID<" + currentCycleId;
				Logger.Info(cmd.CommandText);
				dr = cmd.ExecuteReader();
				dr.Read();

				if (!dr.IsDBNull(0))
				{
					MAX = GetIntValue(dr, "MAXVAL");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex);

			}

			if (dr != null && !dr.IsClosed)
				dr.Close();

			connection.Close();

			return MAX;
		}

		public static int getCycleFromExam(int examId, string connectionString)
		{
			Logger.Info("DBUtil.getMaxCycle");
			int cycleId = 0;

			OdbcConnection connection = new OdbcConnection(connectionString);
			OdbcDataReader dr = null;

			try
			{
				connection.Open();
				OdbcCommand cmd = new OdbcCommand();
				cmd.Connection = connection;

				cmd.CommandText = "SELECT ID FROM CICLI WHERE IDESAME=" + examId;
				Logger.Info(cmd.CommandText);
				dr = cmd.ExecuteReader();
				dr.Read();

				if (!dr.IsDBNull(0))
				{
					cycleId = GetIntValue(dr, "ID");
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex);

			}

			if (dr != null && !dr.IsClosed)
				dr.Close();

			connection.Close();
			return cycleId;
		}

		public static RFIDCycle GetCycleData(int iIDEsame, string sConnectionString, bool bPrevious)
		{
			Logger.Info("DBUtil.GetCycleData");
			int id = -1;
			int idDispositivo = -1;
			int idOperatoreConsegna = -1;
			int idOperatoreEsame = -1;
			int idOperatoreInizioSterilizzazione = -1;
			int idOperatoreFineSterilizzazione = -1;
			int idOperatoreArmadio = -1;
			int IDOperatorePrelavaggio = -1;
			int idOperatoreTestTenuta = -1;
			int idSterilizzatrice = -1;

			string DataEsame = "";
			string DataConsegna = "";
			string DataInizioSterilizzazione = "";
			string DataFineSterilizzazione = "";
			string DataArmadio = "";
			string DataPrelavaggio = "";
			string DataTestTenuta = "";

			string LocalIdVasca = "";
			string LocalMachineCycleId = "";
			string LocalDisinfectantCycleId = "";

			string LocalAdditionalInfo = "";

			if (iIDEsame > 0)
			{
				OdbcConnection connection = new OdbcConnection(sConnectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					string query = "SELECT *  FROM CICLI ";
					if (!bPrevious)
						cmdFCod.CommandText = query + " WHERE IDESAME=" + iIDEsame + " ORDER BY ID DESC";
					else
						cmdFCod.CommandText = query + " WHERE ID=" +
							getPreviousMaxCycle(getDeviceIdFromExam(iIDEsame, sConnectionString), getCycleFromExam(iIDEsame, sConnectionString), sConnectionString);

					dr = cmdFCod.ExecuteReader();
					if (dr.Read())
					{
						try
						{
							id = GetIntValue(dr, "ID");
						}
						catch
						{
							id = -1;
						}

						try
						{
							idDispositivo = GetIntValue(dr, "IDDISPOSITIVO");
						}
						catch
						{
							idDispositivo = -1;
						}

						// ESAME
						try
						{
							idOperatoreEsame = GetIntValue(dr, "IDOPERATOREESAME");
						}
						catch
						{
							idOperatoreEsame = -1;
						}

						try
						{
							DataEsame = dr.GetString(dr.GetOrdinal("DATAESAME"));
							DataEsame = DateTime.ParseExact(DataEsame, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							DataEsame = "";
						}

						// CONSEGNA
						try
						{
							idOperatoreConsegna = GetIntValue(dr, "IDOPERATORECONSEGNA");
						}
						catch
						{
							idOperatoreConsegna = -1;
						}

						try
						{
							DataConsegna = dr.GetString(dr.GetOrdinal("DATACONSEGNA"));
							DataConsegna = DateTime.ParseExact(DataConsegna, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							DataEsame = "";
						}

						// INIZIO STERIL
						try
						{
							idOperatoreInizioSterilizzazione = GetIntValue(dr, "IDOPERATOREINIZIOSTERILIZZAZIO");
						}
						catch
						{
							idOperatoreInizioSterilizzazione = -1;
						}

						try
						{
							idSterilizzatrice = GetIntValue(dr, "IDSTERILIZZATRICE");
						}
						catch
						{
							idSterilizzatrice = -1;
						}


						try
						{
							DataInizioSterilizzazione = dr.GetString(dr.GetOrdinal("DATAINIZIOSTERILIZZAZIONE"));
							DataInizioSterilizzazione = DateTime.ParseExact(DataInizioSterilizzazione, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							DataInizioSterilizzazione = "";
						}


						// FINE STERIL
						try
						{
							idOperatoreFineSterilizzazione = GetIntValue(dr, "IDOPERATOREFINESTERILIZZAZIONE");
						}
						catch
						{
							idOperatoreFineSterilizzazione = -1;
						}

						try
						{
							DataFineSterilizzazione = dr.GetString(dr.GetOrdinal("DATAFINESTERILIZZAZIONE"));
							DataFineSterilizzazione = DateTime.ParseExact(DataFineSterilizzazione, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							DataFineSterilizzazione = "";
						}

						// ARMADIO
						try
						{
							idOperatoreArmadio = GetIntValue(dr, "IDOPERATOREARMADIO");
						}
						catch
						{
							idOperatoreArmadio = -1;
						}

						try
						{
							DataArmadio = dr.GetString(dr.GetOrdinal("DATAARMADIO"));
							DataArmadio = DateTime.ParseExact(DataArmadio, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							DataArmadio = "";
						}

						// TESTTENUTA
						try
						{
							idOperatoreTestTenuta = GetIntValue(dr, "IDOPERATORETESTTENUTA");
						}
						catch
						{
							idOperatoreTestTenuta = -1;
						}

						try
						{
							DataTestTenuta = dr.GetString(dr.GetOrdinal("DATATESTTENUTA"));
							DataTestTenuta = DateTime.ParseExact(DataTestTenuta, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							DataEsame = "";
						}


						// PRELAVAGGIO
						try
						{
							IDOperatorePrelavaggio = GetIntValue(dr, "IDOperatorePrelavaggio");
						}
						catch
						{
							IDOperatorePrelavaggio = -1;
						}

						try
						{
							DataPrelavaggio = dr.GetString(dr.GetOrdinal("DataPrelavaggio"));
							DataPrelavaggio = DateTime.ParseExact(DataPrelavaggio, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							DataPrelavaggio = "";
						}

						try
						{
							LocalIdVasca = dr.GetString(dr.GetOrdinal("IDVASCA"));

						}
						catch
						{
							LocalIdVasca = "";
						}

						try
						{
							LocalMachineCycleId = dr.GetString(dr.GetOrdinal("MACHINECYCLEID"));

						}
						catch
						{
							LocalMachineCycleId = "";
						}

						try
						{
							LocalDisinfectantCycleId = dr.GetString(dr.GetOrdinal("DISINFECTANTCYCLEID"));

						}
						catch
						{
							LocalDisinfectantCycleId = "";
						}

						LocalAdditionalInfo = getCycleAdditionalInfo(iIDEsame, sConnectionString, bPrevious);

					}

				}
				catch (Exception)
				{

				}

				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}



				connection.Close();
			}

			RFIDCycle ciclo = new RFIDCycle()
			{
				ID = (id > 0) ? id.ToString() : "",
				ExamDate = DataEsame,
				SterilizationStartDate = DataInizioSterilizzazione,
				SterilizationEndDate = DataFineSterilizzazione,
				ExamOperatorName = getOperatorNameFromId(idOperatoreEsame, sConnectionString),
				ExamOperatorSurname = getOperatorSurnameFromId(idOperatoreEsame, sConnectionString),
				SterilizationStartOperatorName = getOperatorNameFromId(idOperatoreInizioSterilizzazione, sConnectionString),
				SterilizationStartOperatorSurname = getOperatorSurnameFromId(idOperatoreInizioSterilizzazione, sConnectionString),
				SterilizationEndOperatorName = getOperatorNameFromId(idOperatoreFineSterilizzazione, sConnectionString),
				SterilizationEndOperatorSurname = getOperatorSurnameFromId(idOperatoreFineSterilizzazione, sConnectionString),
				DeviceIdNumber = getDeviceMatFromId(idDispositivo, sConnectionString),
				DeviceDescription = getDeviceDescFromId(idDispositivo, sConnectionString),
				SterilizerDescription = getSterDescFromId(idSterilizzatrice, sConnectionString),
				CustomTrackingOperatorName1 = getOperatorNameFromId(IDOperatorePrelavaggio, sConnectionString),
				CustomTrackingOperatorSurname1 = getOperatorSurnameFromId(IDOperatorePrelavaggio, sConnectionString),
				CustomTrackingDate1 = DataPrelavaggio,
				IdVasca = LocalIdVasca,
				MachineCycleId = LocalMachineCycleId,
				DisinfectantCycleId = LocalDisinfectantCycleId,
				AdditionalInfo = LocalAdditionalInfo
			};

			return ciclo;

		}

		public static string getCycleAdditionalInfo(int examId, string connectionString, bool previous)
		{
			Logger.Info("DBUtil.getCycleAdditionalInfo");
			string results = null;


			int idCiclo = -1;

			if (examId > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;
					if (!previous)
					{
						cmdFCod.CommandText = "SELECT ID FROM CICLI WHERE IDESAME=" + examId + " ORDER BY ID DESC";
					}
					else
					{
						cmdFCod.CommandText = "SELECT ID FROM CICLI WHERE ID=" + getPreviousMaxCycle(getDeviceIdFromExam(examId, connectionString), getCycleFromExam(examId, connectionString), connectionString);
					}
					dr = cmdFCod.ExecuteReader();
					if (dr.Read())
					{
						try
						{
							idCiclo = GetIntValue(dr, "ID");
							//idDispositivo = dr.GetInt32(0);
						}
						catch
						{
							idCiclo = -1;
						}
					}
				}
				catch (Exception /*e*/)
				{
				}

				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}



				connection.Close();
			}


			if (idCiclo > 0)
			{
				results = getCycleAdditionalInfoFromCycle(idCiclo, connectionString);
			}


			return results;

		}

		public static RFIDCycle GetCycleDataFromEsameDispositivo(int iIDEsame, int iIDDispositivo, string sConnectionString, bool bPrevious)
		{
			Logger.Info("DBUtilities.GetCycleDataFromEsameDispositivo 1");
			RFIDCycle cicloReturn = null;

			if ((iIDEsame > 0) && (iIDDispositivo > 0))
			{
				Logger.Info("DBUtilities.GetCycleDataFromEsameDispositivo 2");
				OdbcConnection odbcConn = new OdbcConnection(sConnectionString);

				try
				{
					odbcConn.Open();
					Logger.Info("DBUtilities.GetCycleDataFromEsameDispositivo 3");

					OdbcCommand odbcComm = new OdbcCommand();
					odbcComm.Connection = odbcConn;

					string query = "SELECT * FROM CICLI ";
					if (!bPrevious)
						odbcComm.CommandText = query + " WHERE IDEsame=" + iIDEsame + " AND IDDispositivo=" + iIDDispositivo + " ORDER BY ID DESC";
					else
						odbcComm.CommandText = query + " WHERE ID=" + getPreviousMaxCycle(iIDDispositivo, GetCycleFromEsameDispositivo(iIDEsame, iIDDispositivo, sConnectionString), sConnectionString);

					OdbcDataReader odbcDR = odbcComm.ExecuteReader();

					Logger.Info("DBUtilities.GetCycleDataFromEsameDispositivo 4 " + odbcComm.CommandText);
					if (odbcDR.Read())
					{
						int iIDCiclo = -1;
						try
						{
							iIDCiclo = GetIntValue(odbcDR, "ID");
						}
						catch
						{
							iIDCiclo = -1;
						}

						int iIDOperatoreEsame = -1;
						try
						{
							iIDOperatoreEsame = GetIntValue(odbcDR, "IDOPERATOREESAME");
						}
						catch
						{
							iIDOperatoreEsame = -1;
						}

						int iIDOperatoreInizioSterilizzazione = -1;
						try
						{
							iIDOperatoreInizioSterilizzazione = GetIntValue(odbcDR, "IDOPERATOREINIZIOSTERILIZZAZIO");
						}
						catch
						{
							iIDOperatoreInizioSterilizzazione = -1;
						}

						int iOperatoreFineSterilizzazione = -1;
						try
						{
							iOperatoreFineSterilizzazione = GetIntValue(odbcDR, "IDOPERATOREFINESTERILIZZAZIONE");
						}
						catch
						{
							iOperatoreFineSterilizzazione = -1;
						}

						int iIDOperatoreArmadio = -1;
						try
						{
							iIDOperatoreArmadio = GetIntValue(odbcDR, "IDOPERATOREARMADIO");
						}
						catch
						{
							iIDOperatoreArmadio = -1;
						}

						int iIDSterilizzatrice = -1;
						try
						{
							iIDSterilizzatrice = GetIntValue(odbcDR, "IDSTERILIZZATRICE");
						}
						catch
						{
							iIDSterilizzatrice = -1;
						}

						string sDataEsame = "";
						try
						{
							sDataEsame = odbcDR.GetString(odbcDR.GetOrdinal("DATAESAME"));
							sDataEsame = DateTime.ParseExact(sDataEsame, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							sDataEsame = "";
						}

						string sDataInizioSterilizzazione = "";
						try
						{
							sDataInizioSterilizzazione = odbcDR.GetString(odbcDR.GetOrdinal("DATAINIZIOSTERILIZZAZIONE"));
							sDataInizioSterilizzazione = DateTime.ParseExact(sDataInizioSterilizzazione, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							sDataInizioSterilizzazione = "";
						}

						string sDataFineSterilizzazione = "";
						try
						{
							sDataFineSterilizzazione = odbcDR.GetString(odbcDR.GetOrdinal("DATAFINESTERILIZZAZIONE"));
							sDataFineSterilizzazione = DateTime.ParseExact(sDataFineSterilizzazione, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							sDataFineSterilizzazione = "";
						}

						string sDataArmadio = "";
						try
						{
							sDataArmadio = odbcDR.GetString(odbcDR.GetOrdinal("DATAARMADIO"));
							sDataArmadio = DateTime.ParseExact(sDataArmadio, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							sDataArmadio = "";
						}

						int iIDOperatorePrelavaggio = -1;
						try
						{
							iIDOperatorePrelavaggio = GetIntValue(odbcDR, "IDOperatorePrelavaggio");
						}
						catch
						{
							iIDOperatorePrelavaggio = -1;
						}

						/*
						int iIDOperatorePersonalizzabile2 = -1;
						try
						{
							iIDOperatorePersonalizzabile2 = GetIntValue(odbcDR, "IDOPERATOREPERSONALIZZABILE2");
						}
						catch
						{
							iIDOperatorePersonalizzabile2 = -1;
						}*/

						string sDataPrelavaggio = "";
						try
						{
							sDataPrelavaggio = odbcDR.GetString(odbcDR.GetOrdinal("DataPrelavaggio"));
							sDataPrelavaggio = DateTime.ParseExact(sDataPrelavaggio, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							sDataPrelavaggio = "";
						}

						string sLocalIdVasca = "";
						try
						{
							sLocalIdVasca = odbcDR.GetString(odbcDR.GetOrdinal("IDVASCA"));
						}
						catch
						{
							sLocalIdVasca = "";
						}

						string sLocalMachineCycleId = "";
						try
						{
							sLocalMachineCycleId = odbcDR.GetString(odbcDR.GetOrdinal("MACHINECYCLEID"));
						}
						catch
						{
							sLocalMachineCycleId = "";
						}

						string sLocalDisinfectantCycleId = "";
						try
						{
							sLocalDisinfectantCycleId = odbcDR.GetString(odbcDR.GetOrdinal("DISINFECTANTCYCLEID"));
						}
						catch
						{
							sLocalDisinfectantCycleId = "";
						}

						string sLocalAdditionalInfo = GetCycleAdditionalInfoFromEsameDispositivo(iIDEsame, iIDDispositivo, sConnectionString, bPrevious);

						cicloReturn = new RFIDCycle()
						{
							ID = iIDCiclo.ToString(),
							ExamDate = sDataEsame,
							SterilizationStartDate = sDataInizioSterilizzazione,
							SterilizationEndDate = sDataFineSterilizzazione,
							ExamOperatorName = getOperatorNameFromId(iIDOperatoreEsame, sConnectionString),
							ExamOperatorSurname = getOperatorSurnameFromId(iIDOperatoreEsame, sConnectionString),
							SterilizationStartOperatorName = getOperatorNameFromId(iIDOperatoreInizioSterilizzazione, sConnectionString),
							SterilizationStartOperatorSurname = getOperatorSurnameFromId(iIDOperatoreInizioSterilizzazione, sConnectionString),
							SterilizationEndOperatorName = getOperatorNameFromId(iOperatoreFineSterilizzazione, sConnectionString),
							SterilizationEndOperatorSurname = getOperatorSurnameFromId(iOperatoreFineSterilizzazione, sConnectionString),
							DeviceIdNumber = getDeviceMatFromId(iIDDispositivo, sConnectionString),
							DeviceDescription = getDeviceDescFromId(iIDDispositivo, sConnectionString),
							SterilizerDescription = getSterDescFromId(iIDSterilizzatrice, sConnectionString),
							CustomTrackingOperatorName1 = getOperatorNameFromId(iIDOperatorePrelavaggio, sConnectionString),
							CustomTrackingOperatorSurname1 = getOperatorSurnameFromId(iIDOperatorePrelavaggio, sConnectionString),
							CustomTrackingDate1 = sDataPrelavaggio,
							IdVasca = sLocalIdVasca,
							MachineCycleId = sLocalMachineCycleId,
							DisinfectantCycleId = sLocalDisinfectantCycleId,
							AdditionalInfo = sLocalAdditionalInfo
						};
					}

					Logger.Info("DBUtilities.GetCycleDataFromEsameDispositivo 5");

					if (odbcDR != null && !odbcDR.IsClosed)
						odbcDR.Close();
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
				}

				odbcConn.Close();
				Logger.Info("DBUtilities.GetCycleDataFromEsameDispositivo 6");
			}

			Logger.Info("DBUtilities.GetCycleDataFromEsameDispositivo 7");
			return cicloReturn;
		}

		public static RFIDCycleEx GetCycleDataFromEsameDispositivoEx(int iIDEsame, int iIDDispositivo, string sConnectionString, bool bPrevious)
		{
			Logger.Info("DBUtil.GetCycleDataFromEsameDispositivoEx");
			if ((iIDEsame <= 0) || (iIDDispositivo <= 0))
				return null;

			RFIDCycleEx cicloReturn = null;
			OdbcConnection odbcConn = new OdbcConnection(sConnectionString);
			OdbcDataReader odbcDR = null;

			try
			{
				odbcConn.Open();

				OdbcCommand odbcComm = new OdbcCommand("SELECT DESCRIZIONE FROM DISPOSITIVI WHERE ID = " + iIDDispositivo, odbcConn);
				odbcDR = odbcComm.ExecuteReader();
				if (odbcDR.Read())
				{
					cicloReturn = new RFIDCycleEx();
					if (!odbcDR.IsDBNull(odbcDR.GetOrdinal("DESCRIZIONE")))
						cicloReturn.Description = odbcDR.GetString(odbcDR.GetOrdinal("DESCRIZIONE"));
					odbcDR.Close();

					string query = "";
					int previousid = 0;

					if (bPrevious)
					{
						previousid = getPreviousMaxCycle(iIDDispositivo, GetCycleFromEsameDispositivo(iIDEsame, iIDDispositivo, sConnectionString), sConnectionString);

						query += "SELECT ARMADI_LAVATRICI.DESCRIZIONE ";
						query += "FROM CICLI ";
						query += "     INNER JOIN ARMADI_LAVATRICI ON CICLI.IDSTERILIZZATRICE = ARMADI_LAVATRICI.ID ";
						query += "WHERE CICLI.ID=" + previousid;

						odbcComm = new OdbcCommand(query, odbcConn);
						odbcDR = odbcComm.ExecuteReader();
						if (odbcDR.Read())
						{
							if (!odbcDR.IsDBNull(odbcDR.GetOrdinal("DESCRIZIONE")))
								cicloReturn.WasherDescription = odbcDR.GetString(odbcDR.GetOrdinal("DESCRIZIONE"));
						}

						odbcDR.Close();
					}

					query = "";
					query += "SELECT STATO.ID AS IDSTATO, STATO.DESCRIZIONEAZIONE, OPERATORI.COGNOME, OPERATORI.NOME, CICLISTATOLOG.DATAORA ";
					query += "FROM CICLI ";
					query += "     INNER JOIN CICLISTATOLOG ON CICLI.ID = CICLISTATOLOG.IDCICLO ";
					query += "     INNER JOIN STATO ON STATO.ID = CICLISTATOLOG.IDSTATONEW ";
					query += "     INNER JOIN OPERATORI ON OPERATORI.ID = CICLISTATOLOG.IDOPERATORE ";

					if (!bPrevious)
						query += "WHERE CICLI.IDEsame=" + iIDEsame + " AND CICLI.IDDispositivo=" + iIDDispositivo;
					else
						query += "WHERE CICLI.ID=" + previousid;

					query += " ORDER BY DATAORA";

					List<RFIDCycleStep> listSteps = new List<RFIDCycleStep>();
					odbcComm = new OdbcCommand(query, odbcConn);
					odbcDR = odbcComm.ExecuteReader();
					while (odbcDR.Read())
					{
						RFIDCycleStep step = new RFIDCycleStep();
						step.ID = GetIntValue(odbcDR, "IDSTATO");
						if (!odbcDR.IsDBNull(odbcDR.GetOrdinal("DESCRIZIONEAZIONE")))
							step.Description = odbcDR.GetString(odbcDR.GetOrdinal("DESCRIZIONEAZIONE"));
						if (!odbcDR.IsDBNull(odbcDR.GetOrdinal("COGNOME")))
							step.OperatorSurname = odbcDR.GetString(odbcDR.GetOrdinal("COGNOME"));
						if (!odbcDR.IsDBNull(odbcDR.GetOrdinal("NOME")))
							step.OperatorName = odbcDR.GetString(odbcDR.GetOrdinal("NOME"));
						if (!odbcDR.IsDBNull(odbcDR.GetOrdinal("DATAORA")))
							step.DateTime = odbcDR.GetString(odbcDR.GetOrdinal("DATAORA"));

						listSteps.Add(step);
					}

					cicloReturn.Steps = listSteps.ToArray();
				}
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}

			if (odbcDR != null && !odbcDR.IsClosed)
				odbcDR.Close();

			odbcConn.Close();

			return cicloReturn;
		}

		public static string GetCycleAdditionalInfoFromEsameDispositivo(int iIDEsame, int iIDDispositivo, string sConnectionString, bool bPrevious)
		{
			Logger.Info("DBUtil.GetCycleAdditionalInfoFromEsameDispositivo");
			string sResults = null;

			if ((iIDEsame > 0) && (iIDDispositivo > 0))
			{
				OdbcConnection odbcConn = new OdbcConnection(sConnectionString);

				try
				{
					odbcConn.Open();

					OdbcCommand odbcComm = new OdbcCommand();
					odbcComm.Connection = odbcConn;

					if (!bPrevious)
						odbcComm.CommandText = "SELECT ID FROM Cicli WHERE IDESAME=" + iIDEsame + " AND IDDispositivo=" + iIDDispositivo + " ORDER BY ID DESC";
					else
						odbcComm.CommandText = "SELECT ID FROM Cicli WHERE ID=" + getPreviousMaxCycle(iIDDispositivo, GetCycleFromEsameDispositivo(iIDEsame, iIDDispositivo, sConnectionString), sConnectionString);

					OdbcDataReader odbcDR = odbcComm.ExecuteReader(); ;
					if (odbcDR.Read())
					{
						try
						{
							int iIDCiclo = GetIntValue(odbcDR, "ID");

							sResults = getCycleAdditionalInfoFromCycle(iIDCiclo, sConnectionString);
						}
						catch
						{
						}
					}
					if (odbcDR != null && !odbcDR.IsClosed)
						odbcDR.Close();
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
				}

				odbcConn.Close();
			}

			return sResults;
		}

		public static int GetCycleFromEsameDispositivo(int iIDEsame, int iIDDispositivo, string sConnectionString)
		{
			Logger.Info("DBUtil.GetCycleFromEsameDispositivo");
			int iReturn = 0;

			OdbcConnection odbcConn = new OdbcConnection(sConnectionString);

			try
			{
				odbcConn.Open();

				OdbcCommand odbcComm = new OdbcCommand("SELECT ID FROM Cicli WHERE IDEsame=" + iIDEsame + " AND IDDispositivo=" + iIDDispositivo + " ORDER BY ID DESC", odbcConn);

				OdbcDataReader odbcDR = odbcComm.ExecuteReader();

				if (odbcDR.Read())
					iReturn = GetIntValue(odbcDR, "ID");

				if (odbcDR != null && !odbcDR.IsClosed)
					odbcDR.Close();
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}

			odbcConn.Close();

			return iReturn;
		}

		public static string getCycleAdditionalInfoFromCycle(int idCiclo, string connectionString)
		{
			Logger.Info("DBUtil.getCycleAdditionalInfoFromCycle");
			string results = null;


			if (idCiclo > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);
				OdbcDataReader dr = null;

				try
				{
					connection.Open();
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = connection;

					cmdFCod.CommandText = "SELECT DATA,DESCRIZIONE,VALORE,ERROR FROM CICLIEXT WHERE IDCICLO = " + idCiclo + " ORDER BY ID DESC";

					dr = cmdFCod.ExecuteReader();
					while (dr.Read())
					{
						string data = "";
						string descrizione = "";
						string valore = "";
						string error = "";

						try
						{
							data = dr.GetString(dr.GetOrdinal("DATA"));
							data = DateTime.ParseExact(data, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
						}
						catch
						{
							data = "";
						}
						try
						{
							descrizione = dr.GetString(dr.GetOrdinal("DESCRIZIONE"));
						}
						catch
						{
							descrizione = "";
						}
						try
						{
							valore = dr.GetString(dr.GetOrdinal("VALORE"));
						}
						catch
						{
							valore = "";
						}
						try
						{
							error = "" + GetIntValue(dr, "ERROR");
						}
						catch
						{
							error = "";
						}


						string newRes = data + "@@" + descrizione + "@@" + valore + "@@" + error + SEPARATOR;

						results = results + newRes;

					}

				}
				catch (Exception ex)
				{
					Logger.Error(ex);
				}

				if (dr != null && !dr.IsClosed)
				{
					dr.Close();
				}



				connection.Close();
			}


			return results;
		}

		public static bool setState(string Tag, int state, OdbcConnection conn)
		{
			Logger.Info("DBUtil.setState");
			bool success = false;

			if (Tag != "")
			{
				try
				{
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = conn;
					cmdFCod.CommandText = "UPDATE DISPOSITIVI SET STATO=" + state + " WHERE TAG= '" + Tag + "'";
					Logger.Info(cmdFCod.CommandText);
					cmdFCod.ExecuteNonQuery();
					success = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					success = false;
				}
			}

			return success;
		}

		public static int getState(int id, string connectionString)
		{
			Logger.Info("DBUtil.getState");
			int state = -1;
			OdbcDataReader dr = null;

			if (id > 0)
			{
				OdbcConnection connection = new OdbcConnection(connectionString);

				try
				{
					connection.Open();
					OdbcCommand cmd = new OdbcCommand();
					cmd.Connection = connection;
					cmd.CommandText = "SELECT STATO FROM DISPOSITIVI WHERE ID = " + id;
					dr = cmd.ExecuteReader();
					dr.Read();

					if (!dr.IsDBNull(0))
					{
						state = GetIntValue(dr, "STATO");
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					state = -1;
				}
				if (dr != null && !dr.IsClosed)
				{

					dr.Close();
				}
				connection.Close();
			}
			return state;
		}

		public static bool insertnewCycle(string deviceTag, string userTag, int cleaner, int state, int oldState, int idExamToSave, string connectionString)
		{
			Logger.Info("DBUtil.insertnewCycle");
			bool success = false;

			//trick forzastato
			if (oldState < 0)
			{
				Logger.Info("!!Previous state unidentified -> forcing state to 2!!");
				oldState = 2;
			}

			if (deviceTag == null || deviceTag == "")
			{
				Logger.Error("deviceTag is null");
				return false;
			}

			if (userTag == null || userTag == "")
			{
				Logger.Error("userTag is null");
				return false;
			}

			if (state < 0 || oldState < 0)
			{
				Logger.Error("state is null");
				return false;
			}

			OdbcConnection ODBCConnection = new OdbcConnection();

			try
			{
				ODBCConnection.ConnectionString = connectionString;
				ODBCConnection.Open();
			}
			catch (Exception ex)
			{
				Logger.Info(ex);

				if (ODBCConnection != null)
					ODBCConnection.Dispose();

				return success;
			}
			OdbcCommand ODBCCommand = null;
			try
			{
				OdbcParameter result = new OdbcParameter("RES", OdbcType.Int);
				result.Direction = ParameterDirection.Output;

				OdbcParameter deviceTagParameter = new OdbcParameter("TAGDEVICE", OdbcType.VarChar, 50);
				deviceTagParameter.Direction = ParameterDirection.Input;
				deviceTagParameter.Value = deviceTag;

				OdbcParameter stateParameter = new OdbcParameter("STATE", OdbcType.Int);
				stateParameter.Direction = ParameterDirection.Input;
				stateParameter.Value = state;

				OdbcParameter dateCleanParameter = new OdbcParameter("DATECLEAN", OdbcType.VarChar, 14);
				string dateClean = DateTime.Now.ToString("yyyyMMddHHmmss");
				dateCleanParameter.Direction = ParameterDirection.Input;
				dateCleanParameter.Value = dateClean;

				OdbcParameter userTagParameter = new OdbcParameter("TAGUSERCLEAN", OdbcType.VarChar, 50);
				userTagParameter.Direction = ParameterDirection.Input;
				userTagParameter.Value = userTag;

				OdbcParameter cleanerParameter = new OdbcParameter("CLEANER", OdbcType.Int);
				cleanerParameter.Direction = ParameterDirection.Input;
				cleanerParameter.Value = cleaner;

				OdbcParameter oldStateParameter = new OdbcParameter("OLDSTATE", OdbcType.Int);
				oldStateParameter.Direction = ParameterDirection.Input;
				oldStateParameter.Value = oldState;

				OdbcParameter examParameter = new OdbcParameter("EXAM", OdbcType.Int);
				examParameter.Direction = ParameterDirection.Input;
				examParameter.Value = idExamToSave;

				Logger.Info("sp_updatedevnew: " + deviceTag + "," + state + "," + dateClean + "," + userTag + "," + cleaner + "," + oldState + "," + idExamToSave);

				ODBCCommand = new OdbcCommand("{call sp_updatedevnew (?,?,?,?,?,?,?,?)}", ODBCConnection);
				ODBCCommand.CommandType = CommandType.StoredProcedure;
				ODBCCommand.Parameters.Add(result);
				ODBCCommand.Parameters.Add(deviceTagParameter);
				ODBCCommand.Parameters.Add(stateParameter);
				ODBCCommand.Parameters.Add(dateCleanParameter);
				ODBCCommand.Parameters.Add(userTagParameter);
				ODBCCommand.Parameters.Add(cleanerParameter);
				ODBCCommand.Parameters.Add(oldStateParameter);
				ODBCCommand.Parameters.Add(examParameter);

				ODBCCommand.ExecuteNonQuery();

				Logger.Info("call sp_updatedevnew >> return value: " + result.Value.ToString());
				success = true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
			finally
			{
				ODBCConnection.Close();
				ODBCConnection.Dispose();
				if (ODBCCommand != null)
				{
					ODBCCommand.Parameters.Clear();
					ODBCCommand.Dispose();
				}
			}
			return success;
		}

		public static bool insertnewCycleEx(string deviceTag, string userTag, int cleaner, int state, int oldState, int idExamToSave, string connectionString, Dictionary<string, string> optionalField = null)
		{
			Logger.Info("DBUtil.insertnewCycleEx");
			Logger.Info("Previous state = " + oldState.ToString());
			Logger.Info("New state = " + state.ToString());
			bool result = false;

			if (oldState < 0)
			{
				Logger.Info("!!Previous state unidentified -> forcing state to dirty!!");
				oldState = (int)Types.State.Dirty;
			}

			if (deviceTag != null &&
				 deviceTag != "" &&
				 userTag != null &&
				 userTag != "" &&
				 state >= 0 &&
				 oldState >= 0)
			{
				try
				{
					if (!Device.Exist(deviceTag))
					{
						Logger.Info("Device not found!!!");
						return false;
					}

					if (!Operator.Exist(userTag))
					{
						Logger.Info("Operator not found!!!");
						return false;
					}

					var idDev = DBUtilities.getDevIdFromTag(deviceTag, connectionString);
					if (idDev < 0)
					{
						Logger.Info("idDev < 0");
						return false;
					}
					else
						Logger.Info("idDev = " + idDev.ToString());

					var idOp = DBUtilities.getOperatorIdFromTag(userTag, connectionString);
					if (idOp < 0)
					{
						Logger.Info("idop < 0");
						return false;
					}
					else
						Logger.Info("idDev = " + idOp.ToString());

					var currDate = DateTime.Now.ToString("yyyyMMddHHmmss");

					OdbcConnection conn = new OdbcConnection(connectionString);
					conn.Open();

					result = true;
					switch ((Types.State)state)
					{
						case Types.State.Dirty:
							{
								int idCycle = getLastValidCycleId(idDev, conn, Types.State.Dirty);

								if (idCycle > 0)
								{
									if (!setCycleDirtyInfo(idCycle, currDate, idOp, conn, optionalField))
									{
										Logger.Info("Unable setCycleDirtyInfo");
										result = false;
									}
								}
								else
								{
									Logger.Info("---> DIRTY STATE ");
									if (!insCycleStartingDirty(idDev, currDate, userTag, idExamToSave, conn, optionalField))
									{
										Logger.Info("Error inserting cycle starting dirty");
										result = false;
									}
								}

								if (result)
								{
									if (!setState(deviceTag, (int)Types.State.Dirty, conn))
									{
										Logger.Info("Error setting state deviceTag");
										result = false;
									}
								}
							}
							break;

						case Types.State.Clean:
							{
								Logger.Info("---> CLEAN STATE ");
								int idCycle = getLastValidCycleId(idDev, conn, Types.State.Clean);

								if (idCycle > 0)
								{
									if (!setCycleCleanInfo(idCycle, currDate, idOp, conn))
									{
										Logger.Info("Unable setcycleCleanInfo");
										result = false;
									}
								}
								else
								{
									Logger.Info("NO VALID CYCLE FOUND!....INSERTING NEW ONE (3)");

									if (!insCycleStartingClean(idDev, currDate, userTag, conn))
									{
										Logger.Info("Error inserting cycle starting dirty");
										result = false;
									}
								}

								if (result)
								{
									if (!setState(deviceTag, (int)Types.State.Clean, conn))
									{
										Logger.Info("Unable set state deviceTag");
										result = false;
									}
								}
							}
							break;

						case Types.State.Washing:
							{
								Logger.Info("---> WASHING STATE ");
								int idCycle = getLastValidCycleId(idDev, conn, Types.State.Washing);
								if (idCycle > 0)
								{
									if (!setCycleWashInfo(idCycle, currDate, idOp, cleaner, conn))
									{
										Logger.Info("Unable setCycleWashInfo");
										result = false;
									}
								}

								else
								{
									Logger.Info("NO VALID CYCLE FOUND!....INSERTING NEW ONE (4)");

									if (!insCycleStartingWash(idDev, currDate, userTag, cleaner, conn))
									{
										Logger.Info("Error inserting cycle starting dirty");
										result = false;
									}
								}

								if (result)
								{
									if (!setState(deviceTag, (int)Types.State.Washing, conn))
									{
										Logger.Info("Unable set state deviceTag");
										result = false;
									}
								}
							}
							break;

						case Types.State.PreWashing:
							{
								Logger.Info("---> PREWASHING STATE ");
								int idCycle = getLastValidCycleId(idDev, conn, Types.State.PreWashing);

								if (idCycle > 0)
								{
									if (!setCyclePreWashInfo(idCycle, currDate, idOp, conn))
									{
										Logger.Info("Unable setCyclePreWashInfo");
										result = false;
									}
								}
								else
								{
									Logger.Info("NO VALID CYCLE FOUND!....INSERTING NEW ONE (5)");

									if (!insCycleStartingPrewash(idDev, currDate, userTag, conn))
									{
										Logger.Info("Error inserting cycle starting dirty");
										result = false;
									}
								}

								if (result)
								{
									if (!setState(deviceTag, (int)Types.State.PreWashing, conn))
										Logger.Info("Unable set state deviceTag");
								}
							}
							break;

						case Types.State.Stored:
							{
								Logger.Info("---> STORED STATE ");
								int idCycle = getLastValidCycleId(idDev, conn, Types.State.Stored);

								if (idCycle > 0)
								{
									if (!setCycleStoredInfo(idCycle, currDate, idOp, conn))
									{
										Logger.Info("Unable setCycleStoredInfo");
										result = false;
									}
								}
								else
								{
									Logger.Info("NO VALID CYCLE FOUND!....INSERTING NEW ONE (6)");

									if (!insCycleStartingStored(idDev, currDate, userTag, conn))
										Logger.Info("Error inserting cycle starting dirty");

								}

								if (result)
								{
									if (!setState(deviceTag, (int)Types.State.Stored, conn))
										Logger.Info("Unable set state deviceTag");
								}
							}
							break;

						case Types.State.ConsignedForExam:
							{
								Logger.Info("---> CONSIGNED STATE ");
								int idCycle = getLastValidCycleId(idDev, conn, Types.State.ConsignedForExam);

								if (idCycle > 0)
									setCycleConsignedForExamInfo(idCycle, currDate, idOp, conn);
								else
								{
									Logger.Info("NO VALID CYCLE FOUND!....INSERTING NEW ONE (7)");

									if (!insCycleStartingConsignedForExam(idDev, currDate, userTag, conn))
										Logger.Info("Error inserting cycle starting dirty");

								}

								if (result)
								{
									if (!setState(deviceTag, (int)Types.State.ConsignedForExam, conn))
										Logger.Info("Unable set state deviceTag");
								}
							}
							break;

						case Types.State.InLeakTest:
							{
								Logger.Info("---> IN LEAK TEST STATE ");
								int idCycle = getLastValidCycleId(idDev, conn, Types.State.InLeakTest);

								if (idCycle > 0)
									setCycleInLeakTestInfo(idCycle, currDate, idOp, conn);
								else
								{
									Logger.Info("NO VALID CYCLE FOUND!....INSERTING NEW ONE (8)");

									if (!insCycleStartingInLeakTest(idDev, currDate, userTag, conn))
										Logger.Info("Error inserting cycle starting dirty");

								}

								if (result)
								{
									if (!setState(deviceTag, (int)Types.State.InLeakTest, conn))
										Logger.Info("Unable set state deviceTag");
								}
							}
							break;
						default:
							result = false;
							break;
					}

					conn.Close();


				}
				catch (Exception ex)
				{
					Logger.Error(ex);
				}
			}

			return result;
		}

		private static bool setCycleInLeakTestInfo(int idCycle, string currDate, int idOp, OdbcConnection conn)
		{
			Logger.Info("DBUtil.setCycleInLeakTestInfo...");
			bool result = false;

			try
			{
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = conn;
				cmdFCod.CommandText = "UPDATE CICLI SET DATATESTTENUTA = '" + currDate + "', IDOPERATORETESTTENUTA = " + idOp + " WHERE ID = " + idCycle;
				Logger.Info(cmdFCod.CommandText);
				cmdFCod.ExecuteNonQuery();

				result = true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);

				result = false;
			}

			Logger.Info("...DBUtil.setCycleInLeakTestInfo");
			return result;

		}

		private static bool setCycleConsignedForExamInfo(int idCycle, string currDate, int idOp, OdbcConnection conn)
		{
			Logger.Info("DBUtil.setCycleConsignedForExamInfo...");
			bool result = false;

			try
			{
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = conn;
				cmdFCod.CommandText = "UPDATE CICLI SET DATACONSEGNA = '" + currDate + "', IDOPERATORECONSEGNA = " + idOp + " WHERE ID = " + idCycle;
				Logger.Info(cmdFCod.CommandText);
				cmdFCod.ExecuteNonQuery();

				result = true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);

				result = false;
			}

			Logger.Info("...DBUtil.setCycleConsignedForExamInfo");
			return result;
		}

		private static bool setCycleStoredInfo(int idCycle, string currDate, int idOp, OdbcConnection conn)
		{
			Logger.Info("DBUtil.setCycleStoredInfo...");
			bool result = false;

			try
			{
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = conn;
				cmdFCod.CommandText = "UPDATE CICLI SET DATAARMADIO = '" + currDate + "', IDOPERATOREARMADIO = " + idOp + " WHERE ID = " + idCycle;
				Logger.Info(cmdFCod.CommandText);
				cmdFCod.ExecuteNonQuery();

				result = true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);

				result = false;
			}

			Logger.Info("...DBUtil.setCycleStoredInfo");
			return result;
		}

		private static bool setCyclePreWashInfo(int idCycle, string date, int idOp, OdbcConnection conn)
		{
			Logger.Info("DBUtil.setCyclePreWashInfo...");
			bool result = false;

			try
			{
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = conn;
				cmdFCod.CommandText = "UPDATE CICLI SET DataPrelavaggio = '" + date + "', IDOperatorePrelavaggio = " + idOp + " WHERE ID = " + idCycle;
				Logger.Info(cmdFCod.CommandText);
				cmdFCod.ExecuteNonQuery();

				result = true;
			}
			catch (Exception ex)
			{
				Logger.Info(ex);

				result = false;
			}
			Logger.Info("...DBUtil.setCyclePreWashInfo");
			return result;
		}

		public static bool setCycleWashInfo(int idCycle, string date, int idOp, int cleaner, OdbcConnection conn)
		{
			Logger.Info("DBUtil.setCycleWashInfo...");
			bool result = false;

			try
			{
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = conn;
				cmdFCod.CommandText = "UPDATE CICLI SET " + (cleaner > 0 ? "IDSTERILIZZATRICE = " + cleaner + "," : "") + " DATAINIZIOSTERILIZZAZIONE = '" + date + "', IDOPERATOREINIZIOSTERILIZZAZIO = " + idOp + " WHERE ID = " + idCycle;
				Logger.Info(cmdFCod.CommandText);
				cmdFCod.ExecuteNonQuery();

				result = true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);

				result = false;
			}

			Logger.Info("...DBUtil.setCycleWashInfo");
			return result;
		}

		public static bool setCycleCleanInfo(int idCycle, string date, int idOp, string odbcConn)
		{
			OdbcConnection conn = new OdbcConnection(odbcConn);
			conn.Open();
			bool result = setCycleCleanInfo(idCycle, date, idOp, conn);
			conn.Close();
			return result;
		}

		public static bool setCycleCleanInfo(int idCycle, string date, int idOp, OdbcConnection conn)
		{
			Logger.Info("DBUtil.setCycleCleanInfo...");
			bool result = false;

			try
			{
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = conn;
				cmdFCod.CommandText = "UPDATE CICLI SET DATAFINESTERILIZZAZIONE = '" + date + "', IDOPERATOREFINESTERILIZZAZIONE = " + idOp + " WHERE ID = " + idCycle;
				Logger.Info(cmdFCod.CommandText);
				cmdFCod.ExecuteNonQuery();

				result = true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);

				result = false;
			}

			Logger.Info("...DBUtil.setCycleCleanInfo");
			return result;
		}

		private static bool setCycleDirtyInfo(int idCycle, string date, int idOp, OdbcConnection conn, Dictionary<string, string> optionalField)
		{
			Logger.Info("DBUtil.setCycleCleanInfo...");
			bool result = false;

			try
			{
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = conn;
				var query = "UPDATE CICLI SET DATAESAME = '" + date + "', IDOPERATOREESAME = " + idOp;

				if (optionalField != null)
					foreach (var item in optionalField)
						query += ", " + item.Key + " = '" + item.Value + "' ";

				query += " WHERE ID = " + idCycle;
				cmdFCod.CommandText = query;

				Logger.Info(cmdFCod.CommandText);
				cmdFCod.ExecuteNonQuery();

				result = true;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);

				result = false;
			}

			Logger.Info("...DBUtil.setCycleCleanInfo");
			return result;
		}

		public static int getLastValidCycleId(int idDev, string odbcConn, Types.State stateAvailableFilter)
		{
			OdbcConnection conn = new OdbcConnection(odbcConn);
			conn.Open();
			int result = getLastValidCycleId(idDev, conn, stateAvailableFilter);
			conn.Close();
			return result;
		}

		public static int getLastValidCycleId(int idDev, OdbcConnection conn, Types.State stateAvailableFilter)
		{
			Logger.Info("DBUtil.getLastValidCycleId...");
			int id = -1;

			if (idDev > 0)
			{
				string query = "SELECT MAX(ID) AS MAXID FROM CICLI WHERE IDDISPOSITIVO = " + idDev.ToString();

				OdbcDataReader dr = null;
				OdbcCommand cmdFCod = new OdbcCommand();
				cmdFCod.Connection = conn;
				cmdFCod.CommandText = query;

				try
				{
					Logger.Info(cmdFCod.CommandText);
					dr = cmdFCod.ExecuteReader();
					if (dr.Read())
					{
						if (!dr.IsDBNull(0))
						{
							id = GetIntValue(dr, "MAXID");
							Logger.Info("getLastValidCycleId MAXID Found: " + id.ToString());
						}
						else
							Logger.Info("getLastValidCycleId not Found: ");
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
					id = -1;
				}
				finally
				{
					if (dr != null)
					{
						if (!dr.IsClosed)
							dr.Close();
						dr.Dispose();
					}
					cmdFCod.Dispose();
				}

				if (id > 0)
				{
					string queryNew = "SELECT * FROM CICLI WHERE ID = " + id.ToString();

					cmdFCod = new OdbcCommand();
					cmdFCod.Connection = conn;
					cmdFCod.CommandText = queryNew;

					try
					{
						Logger.Info(cmdFCod.CommandText);
						dr = cmdFCod.ExecuteReader();
						if (dr.Read())
						{
							switch (stateAvailableFilter)
							{
								case Types.State.ConsignedForExam:
									id = (dr.IsDBNull(dr.GetOrdinal("DATACONSEGNA")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATORECONSEGNA")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAESAME")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREESAME")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAPRELAVAGGIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREPRELAVAGGIO")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATATESTTENUTA")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATORETESTTENUTA")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAINIZIOSTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREINIZIOSTERILIZZAZIO")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAFINESTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREFINESTERILIZZAZIONE")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAARMADIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREARMADIO"))) ?
										  GetIntValue(dr, "ID") : -1;
									break;

								case Types.State.Dirty:
									id = (dr.IsDBNull(dr.GetOrdinal("DATAESAME")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREESAME")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAPRELAVAGGIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREPRELAVAGGIO")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATATESTTENUTA")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATORETESTTENUTA")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAINIZIOSTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREINIZIOSTERILIZZAZIO")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAFINESTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREFINESTERILIZZAZIONE")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAARMADIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREARMADIO"))) ?
										  GetIntValue(dr, "ID") : -1;
									break;

								case Types.State.InLeakTest:
									id = (dr.IsDBNull(dr.GetOrdinal("DATATESTTENUTA")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATORETESTTENUTA")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAPRELAVAGGIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREPRELAVAGGIO")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAINIZIOSTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREINIZIOSTERILIZZAZIO")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAFINESTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREFINESTERILIZZAZIONE")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAARMADIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREARMADIO"))) ?
										  GetIntValue(dr, "ID") : -1;
									break;

								case Types.State.PreWashing:
									id = (dr.IsDBNull(dr.GetOrdinal("DATAPRELAVAGGIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREPRELAVAGGIO")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAINIZIOSTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREINIZIOSTERILIZZAZIO")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAFINESTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREFINESTERILIZZAZIONE")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAARMADIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREARMADIO"))) ?
										  GetIntValue(dr, "ID") : -1;
									break;


								case Types.State.Washing:
									id = (dr.IsDBNull(dr.GetOrdinal("DATAINIZIOSTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREINIZIOSTERILIZZAZIO")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAFINESTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREFINESTERILIZZAZIONE")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAARMADIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREARMADIO"))) ?
										  GetIntValue(dr, "ID") : -1;
									break;

								case Types.State.Clean:
									id = (dr.IsDBNull(dr.GetOrdinal("DATAFINESTERILIZZAZIONE")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREFINESTERILIZZAZIONE")) &&
										  dr.IsDBNull(dr.GetOrdinal("DATAARMADIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREARMADIO"))) ?
										  GetIntValue(dr, "ID") : -1;
									break;

								case Types.State.Stored:
									id = (dr.IsDBNull(dr.GetOrdinal("DATAARMADIO")) && dr.IsDBNull(dr.GetOrdinal("IDOPERATOREARMADIO"))) ?
										  GetIntValue(dr, "ID") : -1;
									break;

								default:
									id = GetIntValue(dr, "ID");
									break;
							}
						}

						if (id > 0)
							Logger.Info("getLastValidCycleId valid ID Found: " + id.ToString());
						else
							Logger.Info("getLastValidCycleId not Found: ");
					}
					catch (Exception ex)
					{
						Logger.Error(ex);

						id = -1;
					}
					finally
					{
						if (dr != null)
						{
							if (!dr.IsClosed)
								dr.Close();
							dr.Dispose();
						}

						cmdFCod.Dispose();
					}
				}
			}

			Logger.Info("...DBUtil.getLastValidCycleId: result " + id.ToString());
			return id;
		}

		public static bool insertCleanerCycle(string deviceTag, string userTag, string cleaner, string startDate, string endDate, string connectionString)
		{
			Logger.Info("DBUtil.insertCleanerCycle...");
			bool result = false;

			if (deviceTag != null &&
				 deviceTag != "" &&
				 userTag != null &&
				 userTag != "")
			{
				try
				{
					if (!Device.Exist(deviceTag))
					{
						Logger.Info("Device not found!!!");
						return false;
					}

					if (!Operator.ExistFromAnagraphicData(userTag))
					{
						Logger.Info("Operator not found!!!");
						return false;
					}

					var idDev = DBUtilities.getDevIdFromTag(deviceTag, connectionString);
					if (idDev < 0)
					{
						Logger.Info("idDev < 0");
						return false;
					}
					else
						Logger.Info("idDev = " + idDev.ToString());

					var idOp = DBUtilities.getOperatorIdFromAnagraphicData(userTag, connectionString);
					if (idOp < 0)
					{
						Logger.Info("idop < 0");
						return false;
					}
					else
						Logger.Info("idop = " + idOp.ToString());

					OdbcConnection conn = new OdbcConnection(connectionString);
					conn.Open();

					Logger.Info("---> CLEAN STATE ");
					int idCycle = getLastValidCycleId(idDev, conn, Types.State.Clean);

					if (idCycle > 0)
					{
						setCycleCleanInfo(idCycle, endDate, idOp, conn);

						if (!setState(deviceTag, (int)Types.State.Clean, conn))
							Logger.Info("Unable set state deviceTag");
					}
					else
						Logger.Info("NO VALID CYCLE FOUND! (9)");

					conn.Close();

					result = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
				}
			}

			Logger.Info("...DBUtil.insertCleanerCycle");
			return result;
		}

		private static bool insCycleStartingInLeakTest(int idDev, string currDate, string userTag, OdbcConnection conn)
		{
			Logger.Info("DBUtil.insCycleStartingInLeakTest...");
			bool result = false;

			var idOperator = getOperatorIdFromTag(userTag, conn);
			if (idDev > 0 && idOperator > 0)
			{
				OdbcDataReader dr = null;

				try
				{
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = conn;
					cmdFCod.CommandText = "INSERT INTO CICLI (IDDISPOSITIVO, DATATESTTENUTA, IDOPERATORETESTTENUTA) VALUES (" +
						idDev.ToString() + ", '" + currDate + "', " + idOperator.ToString() + ")";
					Logger.Info(cmdFCod.CommandText);
					var rowAffected = cmdFCod.ExecuteNonQuery();
					Logger.Info("rows affected: " + rowAffected.ToString());

					result = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				//Davide System.Threading.Thread.Sleep(2000);
			}
			else
			{
				if (idDev > 0)
					Logger.Info("IDDevice not found!!!!");

				if (idOperator <= 0)
					Logger.Info("IDOperator not found!!!!");
			}

			Logger.Info("...DBUtil.insCycleStartingInLeakTest");
			return result;
		}

		private static bool insCycleStartingConsignedForExam(int idDev, string currDate, string userTag, OdbcConnection conn)
		{
			Logger.Info("DBUtil.insCycleStartingConsignedForExam...");
			bool result = false;

			var idOperator = getOperatorIdFromTag(userTag, conn);
			if (idDev > 0 && idOperator > 0)
			{
				OdbcDataReader dr = null;

				try
				{
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = conn;
					cmdFCod.CommandText = "INSERT INTO CICLI (IDDISPOSITIVO, DATACONSEGNA, IDOPERATORECONSEGNA) VALUES (" +
						idDev.ToString() + ", '" + currDate + "', " + idOperator.ToString() + ")";
					Logger.Info(cmdFCod.CommandText);
					var rowAffected = cmdFCod.ExecuteNonQuery();
					Logger.Info("rows affected: " + rowAffected.ToString());

					result = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				//Davide System.Threading.Thread.Sleep(2000);
			}
			else
			{
				if (idDev > 0)
					Logger.Info("IDDevice not found!!!!");

				if (idOperator <= 0)
					Logger.Info("IDOperator not found!!!!");
			}

			Logger.Info("...DBUtil.insCycleStartingConsignedForExam");
			return result;
		}

		private static bool insCycleStartingStored(int idDev, string currDate, string userTag, OdbcConnection conn)
		{
			Logger.Info("DBUtil.insCycleStartingStored...");
			bool result = false;

			var idOperator = getOperatorIdFromTag(userTag, conn);
			if (idDev > 0 && idOperator > 0)
			{
				OdbcDataReader dr = null;

				try
				{
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = conn;
					cmdFCod.CommandText = "INSERT INTO CICLI (IDDISPOSITIVO, DATAARMADIO, IDOPERATOREARMADIO) VALUES (" +
						idDev.ToString() + ", '" + currDate + "', " + idOperator.ToString() + ")";
					Logger.Info(cmdFCod.CommandText);
					var rowAffected = cmdFCod.ExecuteNonQuery();
					Logger.Info("rows affected: " + rowAffected.ToString());

					result = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				//Davide System.Threading.Thread.Sleep(2000);
			}
			else
			{
				if (idDev > 0)
					Logger.Info("IDDevice not found!!!!");

				if (idOperator <= 0)
					Logger.Info("IDOperator not found!!!!");
			}

			Logger.Info("...DBUtil.insCycleStartingStored");
			return result;
		}

		private static bool insCycleStartingDirty(int idDev, string date, string userTag, int idExamToSave, OdbcConnection conn, Dictionary<string, string> optionalField)
		{
			Logger.Info("DBUtil.insCycleStartingDirty...");
			bool result = false;

			var idOperator = getOperatorIdFromTag(userTag, conn);
			if (idDev > 0 && idOperator > 0)
			{
				OdbcDataReader dr = null;

				try
				{
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = conn;

					var query = "INSERT INTO CICLI (IDDISPOSITIVO, DATAESAME, IDOPERATOREESAME, IDESAME";
					if (optionalField != null)
						foreach (var item in optionalField)
							query += ", " + item.Key + " ";

					query += ") VALUES(" + idDev.ToString() + ", '" + date + "', " + idOperator.ToString() + ", " + idExamToSave.ToString();

					if (optionalField != null)
						foreach (var item in optionalField)
							query += ", '" + item.Value + "' ";

					query += ")";

					cmdFCod.CommandText = query;
					Logger.Info(cmdFCod.CommandText);
					var rowAffected = cmdFCod.ExecuteNonQuery();
					Logger.Info("rows affected: " + rowAffected.ToString());

					result = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				//Davide System.Threading.Thread.Sleep(2000);
			}
			else
			{
				if (idDev > 0)
					Logger.Info("IDDevice not found!!!!");

				if (idOperator <= 0)
					Logger.Info("IDOperator not found!!!!");
			}

			Logger.Info("...DBUtil.insCycleStartingDirty");
			return result;
		}

		public static bool insCycleStartingClean(int idDev, string date, string userTag, OdbcConnection conn)
		{
			Logger.Info("DBUtil.insCycleStartingClean...");
			bool result = false;

			var idOperator = getOperatorIdFromTag(userTag, conn);
			if (idDev > 0 && idOperator > 0)
			{
				OdbcDataReader dr = null;

				try
				{
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = conn;
					cmdFCod.CommandText = "INSERT INTO CICLI (IDDISPOSITIVO, DATAFINESTERILIZZAZIONE, IDOPERATOREFINESTERILIZZAZIONE) VALUES (" + idDev.ToString() + ", '" + date + "', " + idOperator.ToString() + ")";
					Logger.Info(cmdFCod.CommandText);
					var rowAffected = cmdFCod.ExecuteNonQuery();
					Logger.Info("rows affected: " + rowAffected.ToString());

					result = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				//Davide System.Threading.Thread.Sleep(2000);
			}
			else
			{
				if (idDev > 0)
					Logger.Info("IDDevice not found!!!!");

				if (idOperator <= 0)
					Logger.Info("IDOperator not found!!!!");
			}

			Logger.Info("...DBUtil.insCycleStartingClean");
			return result;
		}

		public static bool insCycleStartingWash(int idDev, string date, string userTag, int cleaner, OdbcConnection conn)
		{
			Logger.Info("DBUtil.insCycleStartingWash...");
			bool result = false;

			var idOperator = getOperatorIdFromTag(userTag, conn);
			if (idDev > 0 && idOperator > 0)
			{
				OdbcDataReader dr = null;

				try
				{
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = conn;
					cmdFCod.CommandText = "INSERT INTO CICLI (IDDISPOSITIVO, IDSTERILIZZATRICE, DATAINIZIOSTERILIZZAZIONE, IDOPERATOREINIZIOSTERILIZZAZIO) VALUES (" + idDev.ToString() + ", " + cleaner.ToString() + ", '" + date + "', " + idOperator.ToString() + ")";
					Logger.Info(cmdFCod.CommandText);
					var rowAffected = cmdFCod.ExecuteNonQuery();
					Logger.Info("rows affected: " + rowAffected.ToString());

					result = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				//Davide System.Threading.Thread.Sleep(2000);
			}
			else
			{
				if (idDev > 0)
					Logger.Info("IDDevice not found!!!!");

				if (idOperator <= 0)
					Logger.Info("IDOperator not found!!!!");
			}

			Logger.Info("...DBUtil.insCycleStartingWash");
			return result;
		}

		private static bool insCycleStartingPrewash(int idDev, string date, string userTag, OdbcConnection conn)
		{
			Logger.Info("DBUtil.insCycleStartingPrewash...");
			bool result = false;

			var idOperator = getOperatorIdFromTag(userTag, conn);
			if (idDev > 0 && idOperator > 0)
			{
				OdbcDataReader dr = null;

				try
				{
					OdbcCommand cmdFCod = new OdbcCommand();
					cmdFCod.Connection = conn;
					cmdFCod.CommandText = "INSERT INTO CICLI (IDDISPOSITIVO, DataPrelavaggio, IDOperatorePrelavaggio) VALUES (" + idDev.ToString() + ", '" + date + "', " + idOperator.ToString() + ")";
					Logger.Info(cmdFCod.CommandText);
					var rowAffected = cmdFCod.ExecuteNonQuery();
					Logger.Info("rows affected: " + rowAffected.ToString());

					result = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

				}

				if (dr != null && !dr.IsClosed)
					dr.Close();

				//Davide System.Threading.Thread.Sleep(2000);
			}
			else
			{
				if (idDev > 0)
					Logger.Info("IDDevice not found!!!!");

				if (idOperator <= 0)
					Logger.Info("IDOperator not found!!!!");
			}

			Logger.Info("...DBUtil.insCycleStartingPrewash");
			return result;
		}

		public static bool insertnewCycleWithDate(string deviceTag, string userTag, int cleaner, int state, int oldState, int idExamToSave, DateTime date, string connectionString)
		{
			Logger.Info("DBUtil.insertnewCycleWithDate");
			bool success = false;

			//trick forzastato
			if (oldState < 0)
			{
				Logger.Info("Trick ForceState!!!");
				oldState = 2;
			}


			if (deviceTag != null && deviceTag != "" && userTag != null && userTag != "" && state >= 0 && oldState >= 0)
			{
				OdbcConnection ODBCConnection = new OdbcConnection();

				try
				{
					ODBCConnection.ConnectionString = connectionString;
					ODBCConnection.Open();
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

					if (ODBCConnection != null)
						ODBCConnection.Dispose();

					return success;
				}

				try
				{
					OdbcParameter result = new OdbcParameter("RES", OdbcType.Int);
					result.Direction = ParameterDirection.Output;

					OdbcParameter deviceTagParameter = new OdbcParameter("TAGDEVICE", OdbcType.VarChar, 50);
					deviceTagParameter.Direction = ParameterDirection.Input;
					deviceTagParameter.Value = deviceTag;

					OdbcParameter stateParameter = new OdbcParameter("STATE", OdbcType.Int);
					stateParameter.Direction = ParameterDirection.Input;
					stateParameter.Value = state;

					OdbcParameter dateCleanParameter = new OdbcParameter("DATECLEAN", OdbcType.VarChar, 14);
					string dateClean = date.ToString("yyyyMMddHHmmss");
					dateCleanParameter.Direction = ParameterDirection.Input;
					dateCleanParameter.Value = dateClean;

					OdbcParameter userTagParameter = new OdbcParameter("TAGUSERCLEAN", OdbcType.VarChar, 50);
					userTagParameter.Direction = ParameterDirection.Input;
					userTagParameter.Value = userTag;

					OdbcParameter cleanerParameter = new OdbcParameter("CLEANER", OdbcType.Int);
					cleanerParameter.Direction = ParameterDirection.Input;
					cleanerParameter.Value = cleaner;

					OdbcParameter oldStateParameter = new OdbcParameter("OLDSTATE", OdbcType.Int);
					oldStateParameter.Direction = ParameterDirection.Input;
					oldStateParameter.Value = oldState;

					OdbcParameter examParameter = new OdbcParameter("EXAM", OdbcType.Int);
					examParameter.Direction = ParameterDirection.Input;
					examParameter.Value = idExamToSave;

					OdbcCommand ODBCCommand = new OdbcCommand("{call sp_updatedevnew (?,?,?,?,?,?,?,?)}", ODBCConnection);
					ODBCCommand.CommandType = CommandType.StoredProcedure;
					ODBCCommand.Parameters.Add(result);
					ODBCCommand.Parameters.Add(deviceTagParameter);
					ODBCCommand.Parameters.Add(stateParameter);
					ODBCCommand.Parameters.Add(dateCleanParameter);
					ODBCCommand.Parameters.Add(userTagParameter);
					ODBCCommand.Parameters.Add(cleanerParameter);
					ODBCCommand.Parameters.Add(oldStateParameter);
					ODBCCommand.Parameters.Add(examParameter);

					ODBCCommand.ExecuteNonQuery();
					success = true;
				}
				catch (Exception ex)
				{
					Logger.Error(ex);

				}

				ODBCConnection.Close();
				ODBCConnection.Dispose();
			}

			return success;

		}

		public static int insertnewCycleWithDateForReturn(string deviceTag, string userTag, int cleaner, int state, int oldState, int idExamToSave, DateTime date, string connectionString)
		{
			Logger.Info("DBUtil.insertnewCycleWithDateForReturn");
			//trick forzastato
			if (oldState < 0)
			{
				oldState = 2;
			}

			if (deviceTag != null && deviceTag != "" && userTag != null && userTag != "" && state >= 0 && oldState >= 0)
			{
				OdbcConnection ODBCConnection = new OdbcConnection();

				try
				{
					ODBCConnection.ConnectionString = connectionString;
					ODBCConnection.Open();
				}
				catch (Exception /*e*/)
				{
					if (ODBCConnection != null)
						ODBCConnection.Dispose();

					// Error Message

					return -10;
				}

				try
				{
					OdbcParameter result = new OdbcParameter("RES", OdbcType.Int);
					result.Direction = ParameterDirection.Output;

					OdbcParameter deviceTagParameter = new OdbcParameter("TAGDEVICE", OdbcType.VarChar, 50);
					deviceTagParameter.Direction = ParameterDirection.Input;
					deviceTagParameter.Value = deviceTag;

					OdbcParameter stateParameter = new OdbcParameter("STATE", OdbcType.Int);
					stateParameter.Direction = ParameterDirection.Input;
					stateParameter.Value = state;

					OdbcParameter dateCleanParameter = new OdbcParameter("DATECLEAN", OdbcType.VarChar, 14);
					string dateClean = date.ToString("yyyyMMddHHmmss");
					dateCleanParameter.Direction = ParameterDirection.Input;
					dateCleanParameter.Value = dateClean;

					OdbcParameter userTagParameter = new OdbcParameter("TAGUSERCLEAN", OdbcType.VarChar, 50);
					userTagParameter.Direction = ParameterDirection.Input;
					userTagParameter.Value = userTag;

					OdbcParameter cleanerParameter = new OdbcParameter("CLEANER", OdbcType.Int);
					cleanerParameter.Direction = ParameterDirection.Input;
					cleanerParameter.Value = cleaner;

					OdbcParameter oldStateParameter = new OdbcParameter("OLDSTATE", OdbcType.Int);
					oldStateParameter.Direction = ParameterDirection.Input;
					oldStateParameter.Value = oldState;

					OdbcParameter examParameter = new OdbcParameter("EXAM", OdbcType.Int);
					examParameter.Direction = ParameterDirection.Input;
					examParameter.Value = idExamToSave;

					OdbcCommand ODBCCommand = new OdbcCommand("{call sp_updatedevnew (?,?,?,?,?,?,?,?)}", ODBCConnection);
					ODBCCommand.CommandType = CommandType.StoredProcedure;
					ODBCCommand.Parameters.Add(result);
					ODBCCommand.Parameters.Add(deviceTagParameter);
					ODBCCommand.Parameters.Add(stateParameter);
					ODBCCommand.Parameters.Add(dateCleanParameter);
					ODBCCommand.Parameters.Add(userTagParameter);
					ODBCCommand.Parameters.Add(cleanerParameter);
					ODBCCommand.Parameters.Add(oldStateParameter);
					ODBCCommand.Parameters.Add(examParameter);

					ODBCCommand.ExecuteNonQuery();

					int res = -10;
					try
					{
						res = (int)result.Value;
					}
					catch
					{
						res = -10; ;
					}

					return res;
				}
				catch (Exception /*e*/)
				{
				}

				ODBCConnection.Close();
				ODBCConnection.Dispose();
			}

			return -10;
		}
	}
}
