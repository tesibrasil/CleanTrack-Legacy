namespace amrfidmgrex
{
  public class DBObject
  {
    protected static readonly object Locker = new object();

    protected static string ODBCConnectionString { get; set; }
  }
}
