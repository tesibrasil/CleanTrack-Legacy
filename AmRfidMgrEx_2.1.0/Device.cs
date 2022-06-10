// Decompiled with JetBrains decompiler
// Type: amrfidmgrex.Device
// Assembly: amrfidmgrex, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C64A34BB-7820-4D59-B5A9-AB5E58A7ECDA
// Assembly location: D:\Sorgenti\BRZ\AmRfidMgrEx.dll

using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.IO;

namespace amrfidmgrex
{
  public class Device : DBObject
  {
    private static List<Device> Devices = new List<Device>();

    public int Id { get; set; }

    public string RegNumber { get; set; }

    public string Description { get; set; }

    public string Tag { get; set; }

    public int Stato { get; set; }

    public static string Refresh()
    {
      string text = "";
      OdbcConnection connection = new OdbcConnection(DBObject.ODBCConnectionString);
      OdbcDataReader reader = (OdbcDataReader) null;
      try
      {
        connection.Open();
        reader = new OdbcCommand("select id, matricola, descrizione, tag,stato from dispositivi where dismesso = 0 and dispositivi.eliminato = 0", connection).ExecuteReader();
        lock (DBObject.Locker)
        {
          Device.Devices.Clear();
          while (reader.Read())
            Device.Devices.Add(new Device()
            {
              Id = !reader.IsDBNull(0) ? DBUtilities.GetIntValue(reader, "ID") : -1,
              RegNumber = !reader.IsDBNull(1) ? reader.GetString(1) : "",
              Description = !reader.IsDBNull(2) ? reader.GetString(2) : "",
              Tag = !reader.IsDBNull(3) ? reader.GetString(3) : "",
              Stato = !reader.IsDBNull(4) ? DBUtilities.GetIntValue(reader, "STATO") : -1
            });
        }
      }
      catch (Exception ex)
      {
        text = ex.ToString();
        Device.writeLog(text);
      }
      reader.Close();
      connection.Close();
      return text;
    }

    public static bool Exist(string badgeID)
    {
      int num = -1;
      lock (DBObject.Locker)
        num = Device.Devices.FindIndex((Predicate<Device>) (dev => dev.Tag == badgeID));
      return num >= 0;
    }

    public static Device Get(string badgeID)
    {
      lock (DBObject.Locker)
        return Device.Devices.Find((Predicate<Device>) (dev => dev.Tag == badgeID));
    }

    public static void writeLog(string text)
    {
      try
      {
        StreamWriter streamWriter = new StreamWriter("C:\\TESILOG\\" + DateTime.Now.ToString("yyyyMMdd") + ".log", true);
        streamWriter.WriteLine(DateTime.Now.ToString("HH:mm") + " - " + text);
        streamWriter.Close();
      }
      catch
      {
      }
    }
  }
}
