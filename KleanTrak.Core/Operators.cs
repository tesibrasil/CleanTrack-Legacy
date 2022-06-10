using System;
using System.Linq;
using KleanTrak.Model;

namespace KleanTrak.Core
{
	public class Operators
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Operator FromBarcode(string sTag)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT ID, MATRICOLA, NOME, COGNOME, TAG FROM OPERATORI WHERE DISATTIVATO = 0 AND TAG = '" + sTag.Replace("'", "''").ToUpper() + "'");
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

        public static Operator GetUnknownOperator()
        {

			try
			{
                DbConnection db = new DbConnection();
                DbRecordset dataset = db.ExecuteReader($"SELECT ID, MATRICOLA, NOME, COGNOME, TAG FROM OPERATORI WHERE DISATTIVATO = 0 AND TAG = '{WasherManager._UNKNOWN_OPERATOR}'");
                if (dataset.Count == 0)
                    return null;
                return new Operator()
                {
                    ID = dataset.First().GetInt("ID").Value,
                    Code = dataset.First().GetString("MATRICOLA"),
                    FirstName = dataset.First().GetString("NOME"),
                    LastName = dataset.First().GetString("COGNOME"),
                    Tag = dataset.First().GetString("TAG")
                };
            }
            catch (Exception e)
			{
				Logger.Error(e);
                return null;
			}
        }

        public static Operator FromSerial(string sMatricola)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT ID, MATRICOLA, NOME, COGNOME, TAG FROM OPERATORI WHERE DISATTIVATO = 0 AND MATRICOLA = '" + sMatricola.Replace("'", "''").ToUpper() + "'");
            if (dataset.Count() == 0)
                return null;
            return new Operator
            {
                ID = dataset.First().GetInt("ID").Value,
                Code = dataset.First().GetString("MATRICOLA"),
                FirstName = dataset.First().GetString("NOME"),
                LastName = dataset.First().GetString("COGNOME"),
                Tag = dataset.First().GetString("TAG")
            };
        }

        public static Operator FromID(int id)
        {
            DbConnection db = new DbConnection();
            DbRecordset dataset = db.ExecuteReader("SELECT ID, MATRICOLA, NOME, COGNOME, TAG FROM OPERATORI WHERE DISATTIVATO = 0 AND ID = " + id);

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

        public static Operator FindOperator(string code)
        {
            try
            {
                if (code == null || code == "")
                    return null;
                Operator user = null;
                user = Operators.FromBarcode(code);
                if (user == null) // Sandro 09/06/2017 // modifica fatta al volo per Petardo a Pordenone //
                    user = Operators.FromSerial(code);
                if (user == null)
                    user = Operators.GetUnknownOperator();
                return user;
            }
            catch (Exception e)
            {
                Logger.Error($"code {code}", e);
                return null;
            }
        }

        public static bool TryToFindUserBarcode(string sTag, ref string sNominativo)
		{
			Logger.Info("input: " + sTag);
			try
			{
				Operator user = FromBarcode(sTag);
                if (user == null)
                    user = FromSerial(sTag);
                if (user == null)
                    user = GetUnknownOperator();
				if (user == null)
				{
                    // operatore non trovato
                    Logger.Error($"operator not found sTag {sTag}");
                    return false;
                }
                sNominativo = user.FirstName + " " + user.LastName;
                Logger.Info("return: " + sNominativo);
                return true;
            }
            catch (Exception ex)
			{
                Logger.Error(ex);
                return false;
            }
		}
	}
}
