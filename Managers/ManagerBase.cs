using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
namespace Managers
{
    class ManagerBase
    {
        protected OdbcConnection db = null;
        public ManagerBase(OdbcConnection db_connection)
        {
            db = db_connection;
        }
    }
}
