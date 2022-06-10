using System;
using System.Runtime.InteropServices;

namespace amrfidmgrex
{
    [ComVisible(true)]
    [Guid("242D3762-3E23-3B6C-AE68-5C601FBF4B45")]
    public class DBObject
    {
        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ODBCConnectionString { get; set; }
    }
}
