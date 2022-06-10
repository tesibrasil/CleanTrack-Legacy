// Decompiled with JetBrains decompiler
// Type: amrfidmgrex.Operator
// Assembly: amrfidmgrex, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C64A34BB-7820-4D59-B5A9-AB5E58A7ECDA
// Assembly location: D:\Sorgenti\BRZ\AmRfidMgrEx.dll

using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace amrfidmgrex
{
  public class Operator : DBObject
  {
    private static List<Operator> Operators = new List<Operator>();

    public int Id { get; set; }

    public string RegNumber { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Tag { get; set; }

    public static string Refresh()
    {
      string str = "";
      OdbcConnection connection = new OdbcConnection(DBObject.ODBCConnectionString);
      OdbcDataReader reader = (OdbcDataReader) null;
      try
      {
        connection.Open();
        reader = new OdbcCommand("select id, matricola, cognome, nome, tag from operatori where disattivato = 0", connection).ExecuteReader();
        lock (DBObject.Locker)
        {
          Operator.Operators.Clear();
          while (reader.Read())
            Operator.Operators.Add(new Operator()
            {
              Id = !reader.IsDBNull(0) ? DBUtilities.GetIntValue(reader, "ID") : -1,
              RegNumber = !reader.IsDBNull(1) ? reader.GetString(1) : "",
              FirstName = !reader.IsDBNull(2) ? reader.GetString(2) : "",
              LastName = !reader.IsDBNull(3) ? reader.GetString(3) : "",
              Tag = !reader.IsDBNull(4) ? reader.GetString(4) : ""
            });
        }
      }
      catch (Exception ex)
      {
        str = ex.ToString();
      }
      reader.Close();
      connection.Close();
      return str;
    }

    public static bool Exist(string badgeID)
    {
      int num = -1;
      lock (DBObject.Locker)
        num = Operator.Operators.FindIndex((Predicate<Operator>) (op => op.Tag == badgeID));
      return num >= 0;
    }

    public static Operator Get(string badgeID)
    {
      lock (DBObject.Locker)
        return Operator.Operators.Find((Predicate<Operator>) (op => op.Tag == badgeID));
    }
  }
}
