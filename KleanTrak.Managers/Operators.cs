using KleanTrak.Models;
using System;
using System.Linq;

namespace KleanTrak.Managers
{
	public class Operators
    {
        public static Operator FromBarcode(string barcode)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT ID, MATRICOLA, NOME, COGNOME, TAG FROM OPERATORI WHERE DISATTIVATO = 0 AND TAG = '" + barcode + "'");

            if (dataset.Count() >= 1)
            {
                return new Operator()
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Code = dataset.First().GetString("MATRICOLA"),
                    FirstName = dataset.First().GetString("NOME"),
                    LastName = dataset.First().GetString("COGNOME"),
                    Tag = dataset.First().GetString("TAG")
                };
            }

            return null;
        }

        public static Operator FromSerial(string serial)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT ID, MATRICOLA, NOME, COGNOME, TAG FROM OPERATORI WHERE DISATTIVATO = 0 AND MATRICOLA = '" + serial + "'");

            if (dataset.Count() >= 1)
            {
                return new Operator()
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Code = dataset.First().GetString("MATRICOLA"),
                    FirstName = dataset.First().GetString("NOME"),
                    LastName = dataset.First().GetString("COGNOME"),
                    Tag = dataset.First().GetString("TAG")
                };
            }

            return null;
        }

		public static bool TryToFindUserBarcode(string barcode, ref string description)
		{
			// Logger.Get().Write("", "Service.TryToFindUserBarcode input: ", barcode, null, Logger.LogLevel.Info);

			try
			{
				Operator user = FromBarcode(barcode);
				if (user != null)
				{
					description = user.FirstName + " " + user.LastName;
					// Logger.Get().Write("", "Service.TryToFindUserBarcode return: ", description, null, Logger.LogLevel.Info);
					return true;
				}
			}
			catch (Exception /*ex*/)
			{
				// Logger.Get().Write("", "Service.TryToFindUserBarcode exception ", ex.Message + " " + ex.StackTrace, null, Logger.LogLevel.Error);
			}

			// Logger.Get().Write("", "Service.TryToFindUserBarcode return: ", "false", null, Logger.LogLevel.Info);
			return false;
		}
	}
}
