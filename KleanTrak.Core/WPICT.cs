using KleanTrak.Model;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KleanTrak.Core
{
	/// <summary>
	/// Parser utilizzato per la lettura dei dati da tabelle di frontiera esposte da sw ICT GROUP
	/// </summary>
	/// <remarks>
	/// NON SI TRATTA LAVA ENDOSCOPI, MA DI TABELLE SU SW DI TERZE PARTI.
	/// IL CAMPO BARCODE DELLA TABELLA DATI ICT DEVE CORRISPONDERE AL CAMPO TAG DELLA TABELLA DISPOSITIVI.
	/// IL RICONOSCIMENTO DEL DEVICE VIENE FATTO SEMPRE TRAMITE IL BARCODE, IL CAMPO SERIALE DELLA TABELLA DISPOSITIVI
	/// NON SI USA.
	/// ENDOX RICONOSCE TRAMITE MATRICOLA PERTANTO LA MATRICOLA DEVE ESSERE UGUALE AL TAG.
	/// RIASSUMENDO: 
	/// - ISFRS_DAT.BARCODE = DISPOSITIVI.TAG
	/// - DISPOSITIVI.MATRICOLA DEVE ESSERE LO STESSO DI DISPOSITIVI.TAG
	/// </remarks>
	class WPICT : WPBase
	{
		public override List<WasherCycle> GetCycles(Washer washer, DateTime lastDate)
		{
			var cycles = new List<WasherCycle>();
			var clean_state = StateTransactions.GetStateId(FixedStates.End_wash);
			SqlCommand cmd = null;
			SqlDataReader rdr = null;
			SqlCommand update_cmd = null;
			SqlConnection conn = null;
			SqlConnection update_conn = null;
			try
			{
				string in_list = GetTagList();
				if (in_list.Length == 0)
				{
					Logger.Debug("WasherParserICT no device tags present");
					return cycles;
				}
				//recupera i cicli con flag used = null (non ancora gestiti)
				string query = $"SELECT * FROM ISFRS_DATI LEFT OUTER JOIN ISFRS_UTENTE " +
					$"ON ISFRS_DATI.CODICEUTENTE = ISFRS_UTENTE.CODICE " +
					$"WHERE FLGUSED IS NULL AND BARCODE IN ({in_list}) " +
					$"ORDER BY barcode, DataOraCiclo";
				conn = new SqlConnection(FolderOrFileName);
				conn.Open();
				update_conn = new SqlConnection(FolderOrFileName);
				update_conn.Open();
				cmd = new SqlCommand(query, conn);
				rdr = cmd.ExecuteReader();
				update_cmd = new SqlCommand("UPDATE ISFRS_DATI SET FLGUSED = @now WHERE PK = @pk", update_conn);
				var pnow = new SqlParameter("now", System.Data.SqlDbType.DateTime);
				var ppk = new SqlParameter("pk", System.Data.SqlDbType.Int);
				update_cmd.Parameters.Add(pnow);
				update_cmd.Parameters.Add(ppk);
				while (rdr.Read())
				{
					var device_ext_id = GetDbString(rdr, "Barcode"); //si recupera il device dal barcode
					var operator_ext_id = GetDbString(rdr, "CodiceUtente");
					var riprocessatore = GetDbString(rdr, "Riprocessatore");
					var data_ora_ciclo = GetDbDateTime(rdr, "DataOraCiclo");
					var data_ora_fine_ciclo = GetDbDateTime(rdr, "Scadenza");
					var pk = GetDbInt(rdr, "PK");
					Logger.Debug($"read cycle for PK: {pk}{Environment.NewLine}" +
						$"Seriale: {device_ext_id}{Environment.NewLine}" +
						$"Riprocessatore: {riprocessatore}{Environment.NewLine}" +
						$"CodiceUtente: {operator_ext_id}{Environment.NewLine}" +
						$"DataOraCiclo: {data_ora_ciclo}{Environment.NewLine}");
					var new_cycle = new WasherCycle
					{
						Completed = true, //verificare come capire codici di errore 
						Failed = false,
						WasherID = washer.ID,
						WasherExternalID = riprocessatore,
						DeviceID = TranscodeDevice(device_ext_id),
						DeviceExternalID = device_ext_id,
						OperatorStartExternalID = "", //nessuna info su operatore inizio ciclo
						OperatorStartID = -1, //nessuna info su operatore inizio ciclo
						OperatorEndExternalID = operator_ext_id,
						OperatorEndID = TranscodeOperator(operator_ext_id),
						// nessuna info su ora inizio ciclo, 
						// deve avere un valore valido per cleantrack 
						// altrimenti non passa la transazione
						StartTimestamp = data_ora_ciclo, 
						EndTimestamp = data_ora_fine_ciclo, 
						StationName = riprocessatore,
						CycleCount = GetDbString(rdr, "NumCiclo"),
						CycleType = "",
						Filename = $"tabella dati pk: {pk}",
						FileDatetime = GetDbDateTime(rdr, "DataOraCiclo"),
						FileContent = GetRecordAsString(rdr),
						// il device viene sempre settato come pulito
						DesiredDestinationState = clean_state 
					};
					//seriale endoscopio
					new_cycle.AdditionalInfoList.Add(new WasherCycleInfo 
					{
						Date = data_ora_ciclo,
						Description = "BarCode",
						Value = GetDbString(rdr, "Barcode")
					});
					new_cycle.AdditionalInfoList.Add(new WasherCycleInfo
					{
						Date = data_ora_ciclo,
						Description = "Operatore (tabella utente)",
						Value = $"codice: {GetDbString(rdr, "Codice")}, " +
							$"cognome nome: {GetDbString(rdr, "Cognome")} {GetDbString(rdr, "Nome")}"
					}); ;
					cycles.Add(new_cycle);
					pnow.Value = DateTime.Now;
					ppk.Value = pk;
					if (1 != update_cmd.ExecuteNonQuery())
						Logger.Error($"UPDATE FAILED FOR FLGUSED ON PK {pk}");
				}
				return cycles;
			}
			catch (Exception e)
			{
				Logger.Error($"washer: {washer}, lastdate: {lastDate}", e);
				return cycles;
			}
			finally 
			{
				if (rdr != null && !rdr.IsClosed)
					rdr.Close();
				if (cmd != null)
					cmd.Dispose();
				if (update_cmd != null)
					update_cmd.Dispose();
				if (conn != null)
					conn.Close();
				if (update_conn != null)
					update_conn.Close();
			}
		}
		private string GetTagList()
		{
			OdbcConnection conn = null;
			OdbcCommand dev_cmd = null;
			OdbcDataReader dev_rdr = null;
			try
			{
				conn = new OdbcConnection(DbConnection.ConnectionString);
				conn.Open();
				string dev_query = "SELECT TAG FROM DISPOSITIVI WHERE DISMESSO = 0 AND ELIMINATO = 0";
				dev_cmd = new OdbcCommand(dev_query, conn);
				dev_rdr = dev_cmd.ExecuteReader();
				string in_list = "";
				while (dev_rdr.Read())
				{
					if (dev_rdr.IsDBNull(0))
						continue;
					in_list += $"'{dev_rdr.GetString(0)}',";
				}
				return (in_list.Length > 0) ? in_list.Substring(0, in_list.Length - 1) : "";
			}
			catch (Exception e)
			{
				Logger.Error("GetTagList", e);
				return "";
			}
			finally
			{
				if (dev_rdr != null && !dev_rdr.IsClosed)
					dev_rdr.Close();
				if (dev_cmd != null)
					dev_cmd.Dispose();
				if (conn != null)
					conn.Close();
			}
		}
		private string GetDbString(SqlDataReader rdr, string field_name, string str_for_null = "")
		{
			int ordinal = rdr.GetOrdinal(field_name);
			if (rdr.IsDBNull(ordinal))
				return str_for_null;
			return rdr.GetString(ordinal);
		}
		private int GetDbInt(SqlDataReader rdr, string field_name, int int_for_null = 0)
		{
			int ordinal = rdr.GetOrdinal(field_name);
			if (rdr.IsDBNull(ordinal))
				return int_for_null;
			return rdr.GetInt32(ordinal);
		}
		private long GetDbLong(SqlDataReader rdr, string field_name, long long_for_null = 0)
		{
			int ordinal = rdr.GetOrdinal(field_name);
			if (rdr.IsDBNull(ordinal))
				return long_for_null;
			return rdr.GetInt64(ordinal);
		}
		private DateTime GetDbDateTime(SqlDataReader rdr, string field_name)
		{
			int ordinal = rdr.GetOrdinal(field_name);
			if (rdr.IsDBNull(ordinal))
				return DateTime.MinValue;
			return rdr.GetDateTime(ordinal);
		}
		private string GetRecordAsString(SqlDataReader rdr)
		{
			return $"TABELLA DATI: {Environment.NewLine}" +
				$"PK: {GetDbInt(rdr, "PK")}{Environment.NewLine}" +
				$"Barcode: {GetDbString(rdr, "Barcode")}{Environment.NewLine}" +
				$"Endoscopio: {GetDbString(rdr, "Endoscopio")}{Environment.NewLine}" +
				$"Tipologia: {GetDbString(rdr, "Tipologia")}{Environment.NewLine}" +
				$"Seriale: {GetDbString(rdr, "Seriale")}{Environment.NewLine}" +
				$"Riprocessatore: {GetDbString(rdr, "Riprocessatore")}{Environment.NewLine}" +
				$"NumCiclo: {GetDbString(rdr, "NumCiclo")}{Environment.NewLine}" +
				$"DataOraCiclo{GetDbDateTime(rdr, "DataOraCiclo")}{Environment.NewLine}" +
				$"Scadenza: {GetDbDateTime(rdr, "Scadenza")}{Environment.NewLine}" +
				$"Tmst: {GetDbDateTime(rdr, "Tmst")}{Environment.NewLine}" +
				$"FlgUsed: {GetDbDateTime(rdr, "FlgUsed")}{Environment.NewLine}" +
				Environment.NewLine +
				$"TABELLA UTENTE: {Environment.NewLine}" +
				$"Codice: {GetDbString(rdr, "Codice")}{Environment.NewLine}" +
				$"Cognome: {GetDbString(rdr, "Cognome")}{Environment.NewLine}" +
				$"Nome: {GetDbString(rdr, "Nome")}{Environment.NewLine}";
		}
		/// <summary>
		/// Prova la connessione al db
		/// </summary>
		public override bool Start()
		{
			SqlConnection conn = null;
			try
			{
				conn = new SqlConnection(FolderOrFileName);
				conn.Open();
				return true;
			}
			catch (Exception e)
			{
				Logger.Error("WasherParserICT.Start error", e);
				return false;
			}
			finally
			{
				if (conn != null)
					conn.Close();
			}
		}
	}
}
