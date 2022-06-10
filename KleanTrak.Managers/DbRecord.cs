using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KleanTrak.Managers
{
    public class DbRecord : Dictionary<string, object>
    {
        public int? GetInt(string fieldName)
        {
            if ((from s in Keys where s == fieldName.ToUpper() select s).Count() == 0)
                return null;

            if (this[fieldName.ToUpper()] == null)
                return null;

            return Convert.ToInt32(this[fieldName.ToUpper()]);
        }

        public string GetString(string fieldName)
        {
            if ((from s in Keys where s == fieldName.ToUpper() select s).Count() == 0)
                return null;

            if (this[fieldName.ToUpper()] == null)
                return null;

            return this[fieldName.ToUpper()].ToString();
        }

        public bool? GetBoolean(string fieldName)
        {
            if ((from s in Keys where s == fieldName.ToUpper() select s).Count() == 0)
                return null;

            if (this[fieldName.ToUpper()] == null)
                return null;

            return Convert.ToInt32(this[fieldName.ToUpper()]) == 0 ? false : true;
        }
    }
}
