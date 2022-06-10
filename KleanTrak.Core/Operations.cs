using KleanTrak.Model;
using System;
using System.Linq;

namespace KleanTrak.Core
{
	public class Operations
    {
        public static Operation FromBarcode(string barcode)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT ID, DESCRIZIONE, DESCRIZIONEAZIONE, BARCODE FROM STATO WHERE ELIMINATO = 0 AND BARCODE = '" + barcode.ToUpper().Replace("'", "''") + "'");

            if (dataset.Count() >= 1)
            {
                return new Operation()
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Description = dataset.First().GetString("DESCRIZIONE"),
                    ActionDescription = dataset.First().GetString("DESCRIZIONEAZIONE"),
                    Barcode = dataset.First().GetString("BARCODE")
                };
            }

            return null;
        }

        public static Operation FromID(string id)
        {
			Operation opReturn = null;

            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT ID, DESCRIZIONE, DESCRIZIONEAZIONE, BARCODE FROM STATO WHERE ELIMINATO = 0 AND ID = " + id.ToString());

            if (dataset.Count() >= 1)
            {
				opReturn = new Operation();

				opReturn.ID = dataset.First().GetInt("ID").Value;
				opReturn.Description = dataset.First().GetString("DESCRIZIONE");
				opReturn.ActionDescription = dataset.First().GetString("DESCRIZIONEAZIONE");
                opReturn.Barcode = dataset.First().GetString("BARCODE");
            }

            return opReturn;
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

        public static CmdGetStartCycleBarcodeResponse GetStartCycleBarcode()
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT BARCODE FROM STATO WHERE ELIMINATO = 0 AND INIZIOCICLO = 1");

            if (dataset.Count() >= 1)
            {
                return new CmdGetStartCycleBarcodeResponse()
                {
                    Barcode = dataset.First().GetString("BARCODE"),
                    Successed = true
                };
            }

            return new CmdGetStartCycleBarcodeResponse()
            {
                Barcode = null,
                Successed = false,
                ErrorMessage = "StartCycleBarcode NOT FOUND"
            };
        }
    }
}
