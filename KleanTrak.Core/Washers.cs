using KleanTrack.License;
using KleanTrak.Model;
using log4net.Repository.Hierarchy;
using OdbcExtensions;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;

namespace KleanTrak.Core
{
	public class Washers
	{
		protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public static List<Washer> Get()
		{
			List<Washer> ret = new List<Washer>();
			DbConnection db = new DbConnection();
			var query = "SELECT ARMADI_LAVATRICI.*, UO_SEDI.IDUO " +
				"FROM ARMADI_LAVATRICI " +
				"INNER JOIN UO_SEDI ON UO_SEDI.IDSEDE = ARMADI_LAVATRICI.IDSEDE " +
				"WHERE DISMESSO = 0 " +
				"ORDER BY ID";
			DbRecordset dataset = db.ExecuteReader(query);
			foreach (var record in dataset)
			{
				if (!Enum.IsDefined(typeof(WasherStorageTypes), record.GetInt("TIPO")))
					continue;
				ret.Add(new Washer()
				{
					ID = record.GetInt("ID").Value,
					IDUO = record.GetInt("IDUO").Value,
					IDSede = record.GetInt("IDSEDE").Value,
					Code = record.GetString("MATRICOLA"),
					Description = record.GetString("DESCRIZIONE"),
					SerialNumber = record.GetString("SERIALE"),
					TimeToClean = record.GetInt("TEMPOLAVAGGIO"),
					Type = (WasherStorageTypes)record.GetInt("TIPO").Value,
					PollingTime = record.GetInt("POLLINGTIME").Value,
					FolderOrFileName = record.GetString("PERCORSO"),
					User = record.GetString("UTENTE"),
					Password = record.GetString("PASSWORD")
				});
			}

			return ret;
		}
		public static Washer FromId(int washer_id)
		{

			OdbcCommand cmd = null;
			OdbcDataReader rdr = null;
			try
			{
				string query = $"SELECT * FROM ARMADI_LAVATRICI WHERE ID = {washer_id}";
				var conn = new OdbcConnection(DbConnection.ConnectionString);
				conn.Open();
				cmd = new OdbcCommand(query, conn);
				rdr = cmd.ExecuteReader();
				if (rdr.Read())
				{
					return new Washer
					{
						ID = rdr.GetIntEx("ID"),
						IDSede = rdr.GetIntEx("IDSEDE"),
						Code = rdr.GetStringEx("MATRICOLA"),
						Description = rdr.GetStringEx("DESCRIZIONE"),
						SerialNumber = rdr.GetStringEx("SERIALE"),
						TimeToClean = rdr.GetIntEx("TEMPOLAVAGGIO"),
						Type = (WasherStorageTypes)rdr.GetIntEx("TIPO"),
						PollingTime = rdr.GetIntEx("POLLINGTIME"),
						FolderOrFileName = rdr.GetStringEx("PERCORSO")
					};
				}
				throw new ApplicationException($"washer with id {washer_id} not found");
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (rdr != null && !rdr.IsClosed)
					rdr.Close();
				if (cmd != null)
					cmd.Dispose();
			}
		}
		public static Washer FromMatr(string matr)
		{
			try
			{
				DbConnection db = new DbConnection();
				DbRecordset dataset = db.ExecuteReader(string.Format("SELECT * FROM ARMADI_LAVATRICI WHERE DISMESSO = 0 AND MATRICOLA = '{0:s}'", matr));
				if (dataset.Count() == 0)
					throw new ApplicationException($"washer not found with MATRICOLA = '{matr}'");
				return new Washer
				{
					ID = dataset[0].GetInt("ID").Value,
					IDSede = dataset[0].GetInt("IDSEDE").Value,
					Code = dataset[0].GetString("MATRICOLA"),
					Description = dataset[0].GetString("DESCRIZIONE"),
					SerialNumber = dataset[0].GetString("SERIALE"),
					TimeToClean = dataset[0].GetInt("TEMPOLAVAGGIO"),
					Type = (WasherStorageTypes)dataset[0].GetInt("TIPO").Value,
					PollingTime = dataset[0].GetInt("POLLINGTIME").Value,
					FolderOrFileName = dataset[0].GetString("PERCORSO"),
					User = dataset[0].GetString("UTENTE"),
					Password = dataset[0].GetString("PASSWORD")
				};
			}
			catch (Exception e)
			{
				Logger.Error(matr, e);
				return null;
			}
		}
	}
}
