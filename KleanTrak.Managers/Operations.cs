using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KleanTrak.Models;

namespace KleanTrak.Managers
{
    public class Operations
    {
        public static Operation FromBarcode(string barcode)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT ID, DESCRIZIONE, DESCRIZIONEAZIONE FROM STATO WHERE ELIMINATO = 0 AND BARCODE = '" + barcode + "'");

            if (dataset.Count() >= 1)
            {
                return new Operation()
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Description = dataset.First().GetString("DESCRIZIONE"),
                    ActionDescription = dataset.First().GetString("DESCRIZIONEAZIONE")
                };
            }

            return null;
        }

        public static Operation FromID(string id)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT ID, DESCRIZIONE, DESCRIZIONEAZIONE FROM STATO WHERE ELIMINATO = 0 AND ID = " + id.ToString());

            if (dataset.Count() >= 1)
            {
                return new Operation()
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Description = dataset.First().GetString("DESCRIZIONE"),
                    ActionDescription = dataset.First().GetString("DESCRIZIONEAZIONE")
                };
            }

            return null;
        }

		public static bool TryToFindOperationBarcode(string barcode, ref string description)
		{
			// Logger.Get().Write("", "Service.TryToFindOperationBarcode input: ", barcode, null, Logger.LogLevel.Info);

			try
			{
				Operation op = FromBarcode(barcode);
				if (op != null)
				{
					description = op.ActionDescription;
					// Logger.Get().Write("", "Service.TryToFindOperationBarcode return: ", description, null, Logger.LogLevel.Info);
					return true;
				}
			}
			catch (Exception /*ex*/)
			{
				// Logger.Get().Write("", "Service.TryToFindOperationBarcode exception ", ex.Message + " " + ex.StackTrace, null, Logger.LogLevel.Error);
			}

			// Logger.Get().Write("", "Service.TryToFindOperationBarcode return: ", "false", null, Logger.LogLevel.Info);
			return false;
		}
	}
}
