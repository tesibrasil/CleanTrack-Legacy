// Decompiled with JetBrains decompiler
// Type: amrfidmgrex.DBUtilities
// Assembly: amrfidmgrex, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C64A34BB-7820-4D59-B5A9-AB5E58A7ECDA
// Assembly location: D:\Sorgenti\BRZ\AmRfidMgrEx.dll

using System;
using System.Data;
using System.Data.Odbc;
using System.Globalization;

namespace amrfidmgrex
{
  public class DBUtilities
  {
    public const string SEPARATOR = "$3P4R470R";

    public static int GetIntValue(OdbcDataReader reader, string field)
    {
      int num = -1;
      try
      {
        return reader.GetInt32(reader.GetOrdinal(field));
      }
      catch (InvalidCastException)
      {
      }
      try
      {
        return Convert.ToInt32(reader.GetDecimal(reader.GetOrdinal(field)));
      }
      catch (InvalidCastException)
      {
      }
      return num;
    }

    public static bool testConnection(string connectionString)
    {
      OdbcConnection odbcConnection = new OdbcConnection(connectionString);
      bool flag;
      try
      {
        odbcConnection.Open();
        flag = true;
      }
      catch
      {
        flag = false;
      }
      odbcConnection.Close();
      return flag;
    }

    public static int getStateFromTag(string Tag, string connectionString)
    {
      int num = -1;
      if (Tag != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT STATO FROM DISPOSITIVI WHERE TAG= '" + Tag + "' AND ELIMINATO<>0";
          reader = odbcCommand.ExecuteReader();
          reader.Read();
          if (!reader.IsDBNull(0))
            num = DBUtilities.GetIntValue(reader, "STATO");
        }
        catch (Exception)
        {
          num = -1;
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return num;
    }

    public static int getDispIdFromTag(string Tag, string connectionString)
    {
      int num = -1;
      if (Tag != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT ID FROM DISPOSITIVI WHERE TAG= '" + Tag + "' AND DISMESSO=0";
          reader = odbcCommand.ExecuteReader();
          reader.Read();
          if (!reader.IsDBNull(0))
            num = DBUtilities.GetIntValue(reader, "ID");
        }
        catch (Exception)
        {
          num = -1;
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return num;
    }

    public static int getOperatoreIdFromTag(string Tag, string connectionString)
    {
      int num = -1;
      if (Tag != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT ID FROM OPERATORI WHERE TAG= '" + Tag + "' AND DISATTIVATO=0";
          reader = odbcCommand.ExecuteReader();
          reader.Read();
          if (!reader.IsDBNull(0))
            num = DBUtilities.GetIntValue(reader, "ID");
        }
        catch (Exception)
        {
          num = -1;
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return num;
    }

    public static int getOperatoreIdFromMat(string Mat, string connectionString)
    {
      int num = -1;
      if (Mat != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT ID FROM OPERATORI WHERE MATRICOLA= '" + Mat + "' AND DISATTIVATO=0";
          reader = odbcCommand.ExecuteReader();
          reader.Read();
          if (!reader.IsDBNull(0))
            num = DBUtilities.GetIntValue(reader, "ID");
        }
        catch (Exception)
        {
          num = -1;
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return num;
    }

    public static string getDeviceDescFromTag(string tag, string connectionString)
    {
      string str = "";
      if (tag != null && tag != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT DESCRIZIONE FROM DISPOSITIVI WHERE TAG= '" + tag + "' AND DISMESSO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static string getDeviceDescFromId(int id, string connectionString)
    {
      string str = "";
      if (id > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT DESCRIZIONE FROM DISPOSITIVI WHERE ID= " + (object) id + " AND DISMESSO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static string getDeviceMatFromId(int id, string connectionString)
    {
      string str = "";
      if (id > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT MATRICOLA FROM DISPOSITIVI WHERE ID= " + (object) id + " AND DISMESSO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static string getSterDescFromId(int id, string connectionString)
    {
      string str = "";
      if (id > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT DESCRIZIONE FROM STERILIZZATRICI WHERE ID= " + (object) id + " AND DISMESSO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static string getOperatorNameFromTag(string Tag, string connectionString)
    {
      string str = "";
      if (Tag != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT NOME FROM OPERATORI WHERE TAG= '" + Tag + "' AND DISATTIVATO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static string getOperatorNameFromId(int id, string connectionString)
    {
      string str = "";
      if (id > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT NOME FROM OPERATORI WHERE ID= " + (object) id + " AND DISATTIVATO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static string getOperatorSurnameFromTag(string Tag, string connectionString)
    {
      string str = "";
      if (Tag != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT COGNOME FROM OPERATORI WHERE TAG= '" + Tag + "' AND DISATTIVATO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static string getOperatorSurnameFromId(int id, string connectionString)
    {
      string str = "";
      if (id > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT COGNOME FROM OPERATORI WHERE ID= " + (object) id + " AND DISATTIVATO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static string getOperatorTagFromId(int id, string connectionString)
    {
      string str = "";
      if (id > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT TAG FROM OPERATORI WHERE ID= " + (object) id + " AND DISATTIVATO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static bool checkUser(string id, string connectionString)
    {
      if (id != null && id != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT ID FROM OPERATORI WHERE MATRICOLA= '" + id + "' AND DISATTIVATO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            return true;
        }
        catch (Exception)
        {
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return false;
    }

    public static string getDeviceTagFromId(int id, string connectionString)
    {
      string str = "";
      if (id > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT TAG FROM DISPOSITIVI WHERE ID= " + (object) id + " AND DISMESSO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static string getDeviceTagFromMat(string Mat, string connectionString)
    {
      string str = "";
      if (Mat != null && Mat != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT TAG FROM DISPOSITIVI WHERE MATRICOLA= '" + Mat + "' AND DISMESSO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static string getOperatorTagFromMat(string Mat, string connectionString)
    {
      string str = "";
      if (Mat != null && Mat != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader odbcDataReader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT TAG FROM OPERATORI WHERE MATRICOLA= '" + Mat + "' AND DISATTIVATO=0";
          odbcDataReader = odbcCommand.ExecuteReader();
          odbcDataReader.Read();
          if (!odbcDataReader.IsDBNull(0))
            str = odbcDataReader.GetString(0);
        }
        catch (Exception)
        {
          str = "";
        }
        if (odbcDataReader != null && !odbcDataReader.IsClosed)
          odbcDataReader.Close();
        odbcConnection.Close();
      }
      return str;
    }

    public static int getDeviceIdFromTag(string Tag, string connectionString)
    {
      int num = -1;
      if (Tag != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT ID FROM DISPOSITIVI WHERE TAG= '" + Tag + "' AND DISATTIVATO=0";
          reader = odbcCommand.ExecuteReader();
          reader.Read();
          if (!reader.IsDBNull(0))
            num = DBUtilities.GetIntValue(reader, "ID");
        }
        catch (Exception)
        {
          num = -1;
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return num;
    }

    public static int getMachineIdFromMat(string matricola, string connectionString)
    {
      int num = -1;
      if (matricola != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT ID FROM STERILIZZATRICI WHERE MATRICOLA= '" + matricola + "' AND DISMESSO=0";
          reader = odbcCommand.ExecuteReader();
          reader.Read();
          if (!reader.IsDBNull(0))
            num = DBUtilities.GetIntValue(reader, "ID");
        }
        catch (Exception)
        {
          num = -1;
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return num;
    }

    public static int getDeviceIdFromMat(string Mat, string connectionString)
    {
      int num = -1;
      if (Mat != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT ID FROM DISPOSITIVI WHERE MATRICOLA= '" + Mat + "' AND DISMESSO=0";
          reader = odbcCommand.ExecuteReader();
          reader.Read();
          if (!reader.IsDBNull(0))
            num = DBUtilities.GetIntValue(reader, "ID");
        }
        catch (Exception)
        {
          num = -1;
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return num;
    }

    public static int getDeviceIdFromExam(int Exam, string connectionString)
    {
      int num = -1;
      if (Exam > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT IDDISPOSITIVO FROM CICLI WHERE EXAMID= " + (object) Exam;
          reader = odbcCommand.ExecuteReader();
          reader.Read();
          if (!reader.IsDBNull(0))
            num = DBUtilities.GetIntValue(reader, "IDDISPOSITIVO");
        }
        catch (Exception)
        {
          num = -1;
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return num;
    }

    public static int getPreviousMaxCycle(
      int deviceId,
      int currentCycleId,
      string connectionString)
    {
      int num = 0;
      OdbcConnection odbcConnection = new OdbcConnection(connectionString);
      odbcConnection.Open();
      OdbcCommand odbcCommand = new OdbcCommand();
      odbcCommand.Connection = odbcConnection;
      odbcCommand.CommandText = "SELECT MAX(ID) AS MAXVAL FROM CICLI WHERE DATASTERILIZZAZIONE IS NOT NULL AND IDDISPOSITIVO=" + (object) deviceId + " AND ID<" + (object) currentCycleId;
      OdbcDataReader reader = odbcCommand.ExecuteReader();
      reader.Read();
      try
      {
        if (!reader.IsDBNull(0))
          num = DBUtilities.GetIntValue(reader, "MAXVAL");
      }
      catch (Exception)
      {
      }
      reader.Close();
      odbcConnection.Close();
      return num;
    }

    public static int getMaxCycle(int deviceId, string connectionString)
    {
      int num = 0;
      OdbcConnection odbcConnection = new OdbcConnection(connectionString);
      odbcConnection.Open();
      OdbcCommand odbcCommand = new OdbcCommand();
      odbcCommand.Connection = odbcConnection;
      odbcCommand.CommandText = "SELECT MAX(ID) AS MAXVAL FROM CICLI WHERE IDDISPOSITIVO=" + (object) deviceId;
      OdbcDataReader reader = odbcCommand.ExecuteReader();
      reader.Read();
      try
      {
        if (!reader.IsDBNull(0))
          num = DBUtilities.GetIntValue(reader, "MAXVAL");
      }
      catch (Exception)
      {
      }
      reader.Close();
      odbcConnection.Close();
      return num;
    }

    public static int getCycleFromExam(int examId, string connectionString)
    {
      int num = 0;
      OdbcConnection odbcConnection = new OdbcConnection(connectionString);
      odbcConnection.Open();
      OdbcCommand odbcCommand = new OdbcCommand();
      odbcCommand.Connection = odbcConnection;
      odbcCommand.CommandText = "SELECT ID FROM CICLI WHERE EXAMID=" + (object) examId;
      OdbcDataReader reader = odbcCommand.ExecuteReader();
      reader.Read();
      try
      {
        if (!reader.IsDBNull(0))
          num = DBUtilities.GetIntValue(reader, "ID");
      }
      catch (Exception)
      {
      }
      reader.Close();
      odbcConnection.Close();
      return num;
    }

    public static RFIDCiclo GetCycleData(
      int iIDEsame,
      string sConnectionString,
      bool bPrevious)
    {
      int id1 = -1;
      int id2 = -1;
      int id3 = -1;
      int id4 = -1;
      int id5 = -1;
      int id6 = -1;
      int id7 = -1;
      string s1 = "";
      string s2 = "";
      string s3 = "";
      string s4 = "";
      string s5 = "";
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      if (iIDEsame > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(sConnectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          if (!bPrevious)
            odbcCommand.CommandText = "SELECT IDDISPOSITIVO,DATAESAME,IDOPERATOREESAME,DATASTERILIZZAZIONE,IDOPERATORESTERILIZZAZIONE,IDSTERILIZZATRICE,DATAARMADIO,IDOPERATOREARMADIO,MODIFICAMANUALE,DATAPERSONALIZZABILE1,DATAPERSONALIZZABILE2,IDOPERATOREPERSONALIZZABILE1,IDOPERATOREPERSONALIZZABILE2,IDVASCA,MACHINECYCLEID,DISINFECTANTCYCLEID FROM CICLI WHERE EXAMID=" + (object) iIDEsame + " ORDER BY ID DESC";
          else
            odbcCommand.CommandText = "SELECT IDDISPOSITIVO,DATAESAME,IDOPERATOREESAME,DATASTERILIZZAZIONE,IDOPERATORESTERILIZZAZIONE,IDSTERILIZZATRICE,DATAARMADIO,IDOPERATOREARMADIO,MODIFICAMANUALE,DATAPERSONALIZZABILE1,DATAPERSONALIZZABILE2,IDOPERATOREPERSONALIZZABILE1,IDOPERATOREPERSONALIZZABILE2,IDVASCA,MACHINECYCLEID,DISINFECTANTCYCLEID FROM CICLI WHERE ID=" + (object) DBUtilities.getPreviousMaxCycle(DBUtilities.getDeviceIdFromExam(iIDEsame, sConnectionString), DBUtilities.getCycleFromExam(iIDEsame, sConnectionString), sConnectionString);
          reader = odbcCommand.ExecuteReader();
          if (reader.Read())
          {
            try
            {
              id1 = DBUtilities.GetIntValue(reader, "IDDISPOSITIVO");
            }
            catch
            {
              id1 = -1;
            }
            try
            {
              id2 = DBUtilities.GetIntValue(reader, "IDOPERATOREESAME");
            }
            catch
            {
              id2 = -1;
            }
            try
            {
              id3 = DBUtilities.GetIntValue(reader, "IDOPERATORESTERILIZZAZIONE");
            }
            catch
            {
              id3 = -1;
            }
            try
            {
              id4 = DBUtilities.GetIntValue(reader, "IDOPERATOREARMADIO");
            }
            catch
            {
              id4 = -1;
            }
            try
            {
              id7 = DBUtilities.GetIntValue(reader, "IDSTERILIZZATRICE");
            }
            catch
            {
              id7 = -1;
            }
            try
            {
              s1 = reader.GetString(reader.GetOrdinal("DATAESAME"));
              s1 = DateTime.ParseExact(s1, "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              s1 = "";
            }
            try
            {
              s2 = reader.GetString(reader.GetOrdinal("DATASTERILIZZAZIONE"));
              s2 = DateTime.ParseExact(s2, "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              s2 = "";
            }
            try
            {
              s3 = reader.GetString(reader.GetOrdinal("DATAARMADIO"));
              s3 = DateTime.ParseExact(s3, "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              s3 = "";
            }
            try
            {
              id5 = DBUtilities.GetIntValue(reader, "IDOPERATOREPERSONALIZZABILE1");
            }
            catch
            {
              id5 = -1;
            }
            try
            {
              id6 = DBUtilities.GetIntValue(reader, "IDOPERATOREPERSONALIZZABILE2");
            }
            catch
            {
              id6 = -1;
            }
            try
            {
              s4 = reader.GetString(reader.GetOrdinal("DATAPERSONALIZZABILE1"));
              s4 = DateTime.ParseExact(s4, "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              s4 = "";
            }
            try
            {
              s5 = reader.GetString(reader.GetOrdinal("DATAPERSONALIZZABILE2"));
              s5 = DateTime.ParseExact(s5, "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              s5 = "";
            }
            try
            {
              str1 = reader.GetString(reader.GetOrdinal("IDVASCA"));
            }
            catch
            {
              str1 = "";
            }
            try
            {
              str2 = reader.GetString(reader.GetOrdinal("MACHINECYCLEID"));
            }
            catch
            {
              str2 = "";
            }
            try
            {
              str3 = reader.GetString(reader.GetOrdinal("DISINFECTANTCYCLEID"));
            }
            catch
            {
              str3 = "";
            }
            str4 = DBUtilities.getCycleAdditionalInfo(iIDEsame, sConnectionString, bPrevious);
          }
        }
        catch (Exception)
        {
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return new RFIDCiclo()
      {
        ExamDate = s1,
        SterilizationStartDate = s2,
        SterilizationEndDate = s3,
        ExamOperatorName = DBUtilities.getOperatorNameFromId(id2, sConnectionString),
        ExamOperatorSurname = DBUtilities.getOperatorSurnameFromId(id2, sConnectionString),
        SterilizationStartOperatorName = DBUtilities.getOperatorNameFromId(id3, sConnectionString),
        SterilizationStartOperatorSurname = DBUtilities.getOperatorSurnameFromId(id3, sConnectionString),
        SterilizationEndOperatorName = DBUtilities.getOperatorNameFromId(id4, sConnectionString),
        SterilizationEndOperatorSurname = DBUtilities.getOperatorSurnameFromId(id4, sConnectionString),
        DeviceIdNumber = DBUtilities.getDeviceMatFromId(id1, sConnectionString),
        DeviceDescription = DBUtilities.getDeviceDescFromId(id1, sConnectionString),
        SterilizerDescription = DBUtilities.getSterDescFromId(id7, sConnectionString),
        CustomTrackingOperatorName1 = DBUtilities.getOperatorNameFromId(id5, sConnectionString),
        CustomTrackingOperatorSurname1 = DBUtilities.getOperatorSurnameFromId(id5, sConnectionString),
        CustomTrackingOperatorName2 = DBUtilities.getOperatorNameFromId(id6, sConnectionString),
        CustomTrackingOperatorSurname2 = DBUtilities.getOperatorSurnameFromId(id6, sConnectionString),
        CustomTrackingDate1 = s4,
        CustomTrackingDate2 = s5,
        IdVasca = str1,
        MachineCycleId = str2,
        DisinfectantCycleId = str3,
        AdditionalInfo = str4
      };
    }

    public static string getCycleAdditionalInfo(int examId, string connectionString, bool previous)
    {
      string str = (string) null;
      int idCiclo = -1;
      if (examId > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          if (!previous)
            odbcCommand.CommandText = "SELECT ID FROM CICLI WHERE EXAMID=" + (object) examId + " ORDER BY ID DESC";
          else
            odbcCommand.CommandText = "SELECT ID FROM CICLI WHERE ID=" + (object) DBUtilities.getPreviousMaxCycle(DBUtilities.getDeviceIdFromExam(examId, connectionString), DBUtilities.getCycleFromExam(examId, connectionString), connectionString);
          reader = odbcCommand.ExecuteReader();
          if (reader.Read())
          {
            try
            {
              idCiclo = DBUtilities.GetIntValue(reader, "ID");
            }
            catch
            {
              idCiclo = -1;
            }
          }
        }
        catch (Exception)
        {
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      if (idCiclo > 0)
        str = DBUtilities.getCycleAdditionalInfoFromCycle(idCiclo, connectionString);
      return str;
    }

    public static RFIDCiclo GetCycleDataFromEsameDispositivo(
      int iIDEsame,
      int iIDDispositivo,
      string sConnectionString,
      bool bPrevious)
    {
      RFIDCiclo rfidCiclo = (RFIDCiclo) null;
      if (iIDEsame > 0 && iIDDispositivo > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(sConnectionString);
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          if (!bPrevious)
            odbcCommand.CommandText = "SELECT IDDISPOSITIVO, DATAESAME, IDOPERATOREESAME, DATASTERILIZZAZIONE, IDOPERATORESTERILIZZAZIONE, IDSTERILIZZATRICE, DATAARMADIO, IDOPERATOREARMADIO, MODIFICAMANUALE, DATAPERSONALIZZABILE1, DATAPERSONALIZZABILE2, IDOPERATOREPERSONALIZZABILE1, IDOPERATOREPERSONALIZZABILE2, IDVASCA, MACHINECYCLEID, DISINFECTANTCYCLEID FROM CICLI WHERE EXAMID=" + (object) iIDEsame + " AND IDDispositivo=" + (object) iIDDispositivo + "ORDER BY ID DESC";
          else
            odbcCommand.CommandText = "SELECT IDDISPOSITIVO, DATAESAME, IDOPERATOREESAME, DATASTERILIZZAZIONE, IDOPERATORESTERILIZZAZIONE, IDSTERILIZZATRICE, DATAARMADIO, IDOPERATOREARMADIO, MODIFICAMANUALE, DATAPERSONALIZZABILE1, DATAPERSONALIZZABILE2, IDOPERATOREPERSONALIZZABILE1, IDOPERATOREPERSONALIZZABILE2, IDVASCA, MACHINECYCLEID, DISINFECTANTCYCLEID FROM CICLI WHERE ID=" + (object) DBUtilities.getPreviousMaxCycle(iIDDispositivo, DBUtilities.GetCycleFromEsameDispositivo(iIDEsame, iIDDispositivo, sConnectionString), sConnectionString);
          OdbcDataReader reader = odbcCommand.ExecuteReader();
          if (reader.Read())
          {
            int id1;
            try
            {
              id1 = DBUtilities.GetIntValue(reader, "IDOPERATOREESAME");
            }
            catch
            {
              id1 = -1;
            }
            int id2;
            try
            {
              id2 = DBUtilities.GetIntValue(reader, "IDOPERATORESTERILIZZAZIONE");
            }
            catch
            {
              id2 = -1;
            }
            int id3;
            try
            {
              id3 = DBUtilities.GetIntValue(reader, "IDOPERATOREARMADIO");
            }
            catch
            {
              id3 = -1;
            }
            int id4;
            try
            {
              id4 = DBUtilities.GetIntValue(reader, "IDSTERILIZZATRICE");
            }
            catch
            {
              id4 = -1;
            }
            string str1;
            try
            {
              str1 = DateTime.ParseExact(reader.GetString(reader.GetOrdinal("DATAESAME")), "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              str1 = "";
            }
            string str2;
            try
            {
              str2 = DateTime.ParseExact(reader.GetString(reader.GetOrdinal("DATASTERILIZZAZIONE")), "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              str2 = "";
            }
            string str3;
            try
            {
              str3 = DateTime.ParseExact(reader.GetString(reader.GetOrdinal("DATAARMADIO")), "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              str3 = "";
            }
            int id5;
            try
            {
              id5 = DBUtilities.GetIntValue(reader, "IDOPERATOREPERSONALIZZABILE1");
            }
            catch
            {
              id5 = -1;
            }
            int id6;
            try
            {
              id6 = DBUtilities.GetIntValue(reader, "IDOPERATOREPERSONALIZZABILE2");
            }
            catch
            {
              id6 = -1;
            }
            string str4;
            try
            {
              str4 = DateTime.ParseExact(reader.GetString(reader.GetOrdinal("DATAPERSONALIZZABILE1")), "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              str4 = "";
            }
            string str5;
            try
            {
              str5 = DateTime.ParseExact(reader.GetString(reader.GetOrdinal("DATAPERSONALIZZABILE2")), "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              str5 = "";
            }
            string str6;
            try
            {
              str6 = reader.GetString(reader.GetOrdinal("IDVASCA"));
            }
            catch
            {
              str6 = "";
            }
            string str7;
            try
            {
              str7 = reader.GetString(reader.GetOrdinal("MACHINECYCLEID"));
            }
            catch
            {
              str7 = "";
            }
            string str8;
            try
            {
              str8 = reader.GetString(reader.GetOrdinal("DISINFECTANTCYCLEID"));
            }
            catch
            {
              str8 = "";
            }
            string esameDispositivo = DBUtilities.GetCycleAdditionalInfoFromEsameDispositivo(iIDEsame, iIDDispositivo, sConnectionString, bPrevious);
            rfidCiclo = new RFIDCiclo()
            {
              ExamDate = str1,
              SterilizationStartDate = str2,
              SterilizationEndDate = str3,
              ExamOperatorName = DBUtilities.getOperatorNameFromId(id1, sConnectionString),
              ExamOperatorSurname = DBUtilities.getOperatorSurnameFromId(id1, sConnectionString),
              SterilizationStartOperatorName = DBUtilities.getOperatorNameFromId(id2, sConnectionString),
              SterilizationStartOperatorSurname = DBUtilities.getOperatorSurnameFromId(id2, sConnectionString),
              SterilizationEndOperatorName = DBUtilities.getOperatorNameFromId(id3, sConnectionString),
              SterilizationEndOperatorSurname = DBUtilities.getOperatorSurnameFromId(id3, sConnectionString),
              DeviceIdNumber = DBUtilities.getDeviceMatFromId(iIDDispositivo, sConnectionString),
              DeviceDescription = DBUtilities.getDeviceDescFromId(iIDDispositivo, sConnectionString),
              SterilizerDescription = DBUtilities.getSterDescFromId(id4, sConnectionString),
              CustomTrackingOperatorName1 = DBUtilities.getOperatorNameFromId(id5, sConnectionString),
              CustomTrackingOperatorSurname1 = DBUtilities.getOperatorSurnameFromId(id5, sConnectionString),
              CustomTrackingOperatorName2 = DBUtilities.getOperatorNameFromId(id6, sConnectionString),
              CustomTrackingOperatorSurname2 = DBUtilities.getOperatorSurnameFromId(id6, sConnectionString),
              CustomTrackingDate1 = str4,
              CustomTrackingDate2 = str5,
              IdVasca = str6,
              MachineCycleId = str7,
              DisinfectantCycleId = str8,
              AdditionalInfo = esameDispositivo
            };
          }
          if (reader != null)
          {
            if (!reader.IsClosed)
              reader.Close();
          }
        }
        catch (Exception)
        {
        }
        odbcConnection.Close();
      }
      return rfidCiclo;
    }

    public static string GetCycleAdditionalInfoFromEsameDispositivo(
      int iIDEsame,
      int iIDDispositivo,
      string sConnectionString,
      bool bPrevious)
    {
      string str = (string) null;
      if (iIDEsame > 0 && iIDDispositivo > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(sConnectionString);
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          if (!bPrevious)
            odbcCommand.CommandText = "SELECT ID FROM Cicli WHERE ExamID=" + (object) iIDEsame + " AND IDDispositivo=" + (object) iIDDispositivo + " ORDER BY ID DESC";
          else
            odbcCommand.CommandText = "SELECT ID FROM Cicli WHERE ID=" + (object) DBUtilities.getPreviousMaxCycle(iIDDispositivo, DBUtilities.GetCycleFromEsameDispositivo(iIDEsame, iIDDispositivo, sConnectionString), sConnectionString);
          OdbcDataReader reader = odbcCommand.ExecuteReader();
          if (reader.Read())
          {
            try
            {
              str = DBUtilities.getCycleAdditionalInfoFromCycle(DBUtilities.GetIntValue(reader, "ID"), sConnectionString);
            }
            catch
            {
            }
          }
          if (reader != null)
          {
            if (!reader.IsClosed)
              reader.Close();
          }
        }
        catch (Exception)
        {
        }
        odbcConnection.Close();
      }
      return str;
    }

    public static int GetCycleFromEsameDispositivo(
      int iIDEsame,
      int iIDDispositivo,
      string sConnectionString)
    {
      int num = 0;
      OdbcConnection connection = new OdbcConnection(sConnectionString);
      try
      {
        connection.Open();
        OdbcDataReader reader = new OdbcCommand("SELECT ID FROM Cicli WHERE ExamID=" + (object) iIDEsame + " AND IDDispositivo=" + (object) iIDDispositivo + " ORDER BY ID DESC", connection).ExecuteReader();
        if (reader.Read())
          num = DBUtilities.GetIntValue(reader, "ID");
        if (reader != null)
        {
          if (!reader.IsClosed)
            reader.Close();
        }
      }
      catch (Exception)
      {
      }
      connection.Close();
      return num;
    }

    public static string getCycleAdditionalInfoFromCycle(int idCiclo, string connectionString)
    {
      string str1 = (string) null;
      if (idCiclo > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        OdbcDataReader reader = (OdbcDataReader) null;
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT DATA,DESCRIZIONE,VALORE,ERROR FROM CICLI_PLUS WHERE IDCICLO = " + (object) idCiclo + " ORDER BY ID DESC";
          reader = odbcCommand.ExecuteReader();
          while (reader.Read())
          {
            string str2;
            try
            {
              str2 = DateTime.ParseExact(reader.GetString(reader.GetOrdinal("DATA")), "yyyyMMddHHmmss", (IFormatProvider) CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch
            {
              str2 = "";
            }
            string str3;
            try
            {
              str3 = reader.GetString(reader.GetOrdinal("DESCRIZIONE"));
            }
            catch
            {
              str3 = "";
            }
            string str4;
            try
            {
              str4 = reader.GetString(reader.GetOrdinal("VALORE"));
            }
            catch
            {
              str4 = "";
            }
            string str5;
            try
            {
              str5 = string.Concat((object) DBUtilities.GetIntValue(reader, "ERROR"));
            }
            catch
            {
              str5 = "";
            }
            string str6 = str2 + "@@" + str3 + "@@" + str4 + "@@" + str5 + "$3P4R470R";
            str1 += str6;
          }
        }
        catch (Exception)
        {
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return str1;
    }

    private static bool setState(string Tag, int state, string connectionString)
    {
      bool flag = false;
      if (Tag != "")
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "UPDATE DISPOSITIVI SET STATO=" + (object) state + " WHERE TAG= '" + Tag + "'";
          odbcCommand.ExecuteNonQuery();
          flag = true;
        }
        catch
        {
          flag = false;
        }
        odbcConnection.Close();
      }
      return flag;
    }

    public static int getState(int id, string connectionString)
    {
      int num = -1;
      OdbcDataReader reader = (OdbcDataReader) null;
      if (id > 0)
      {
        OdbcConnection odbcConnection = new OdbcConnection(connectionString);
        try
        {
          odbcConnection.Open();
          OdbcCommand odbcCommand = new OdbcCommand();
          odbcCommand.Connection = odbcConnection;
          odbcCommand.CommandText = "SELECT STATO FROM DISPOSITIVI WHERE ID = " + (object) id;
          reader = odbcCommand.ExecuteReader();
          reader.Read();
          if (!reader.IsDBNull(0))
            num = DBUtilities.GetIntValue(reader, "STATO");
        }
        catch (Exception)
        {
          num = -1;
        }
        if (reader != null && !reader.IsClosed)
          reader.Close();
        odbcConnection.Close();
      }
      return num;
    }

    public static bool insertnewCycle(
      string deviceTag,
      string userTag,
      int cleaner,
      int state,
      int oldState,
      int idExamToSave,
      string connectionString)
    {
      bool flag = false;
      if (oldState < 0)
        oldState = 2;
      if (deviceTag != null && deviceTag != "" && (userTag != null && userTag != "") && (state >= 0 && oldState >= 0))
      {
        OdbcConnection connection = new OdbcConnection();
        try
        {
          connection.ConnectionString = connectionString;
          connection.Open();
        }
        catch (Exception)
        {
          connection.Dispose();
          return flag;
        }
        try
        {
          OdbcParameter odbcParameter1 = new OdbcParameter("RES", OdbcType.Int);
          odbcParameter1.Direction = ParameterDirection.Output;
          OdbcParameter odbcParameter2 = new OdbcParameter("TAGDEVICE", OdbcType.VarChar, 50);
          odbcParameter2.Direction = ParameterDirection.Input;
          odbcParameter2.Value = (object) deviceTag;
          OdbcParameter odbcParameter3 = new OdbcParameter("STATE", OdbcType.Int);
          odbcParameter3.Direction = ParameterDirection.Input;
          odbcParameter3.Value = (object) state;
          OdbcParameter odbcParameter4 = new OdbcParameter("DATECLEAN", OdbcType.VarChar, 14);
          string str = DateTime.Now.ToString("yyyyMMddHHmmss");
          odbcParameter4.Direction = ParameterDirection.Input;
          odbcParameter4.Value = (object) str;
          OdbcParameter odbcParameter5 = new OdbcParameter("TAGUSERCLEAN", OdbcType.VarChar, 50);
          odbcParameter5.Direction = ParameterDirection.Input;
          odbcParameter5.Value = (object) userTag;
          OdbcParameter odbcParameter6 = new OdbcParameter("CLEANER", OdbcType.Int);
          odbcParameter6.Direction = ParameterDirection.Input;
          odbcParameter6.Value = (object) cleaner;
          OdbcParameter odbcParameter7 = new OdbcParameter("OLDSTATE", OdbcType.Int);
          odbcParameter7.Direction = ParameterDirection.Input;
          odbcParameter7.Value = (object) oldState;
          OdbcParameter odbcParameter8 = new OdbcParameter("EXAM", OdbcType.Int);
          odbcParameter8.Direction = ParameterDirection.Input;
          odbcParameter8.Value = (object) idExamToSave;
          OdbcCommand odbcCommand = new OdbcCommand("{call sp_updatedevnew (?,?,?,?,?,?,?,?)}", connection);
          odbcCommand.CommandType = CommandType.StoredProcedure;
          odbcCommand.Parameters.Add(odbcParameter1);
          odbcCommand.Parameters.Add(odbcParameter2);
          odbcCommand.Parameters.Add(odbcParameter3);
          odbcCommand.Parameters.Add(odbcParameter4);
          odbcCommand.Parameters.Add(odbcParameter5);
          odbcCommand.Parameters.Add(odbcParameter6);
          odbcCommand.Parameters.Add(odbcParameter7);
          odbcCommand.Parameters.Add(odbcParameter8);
          odbcCommand.ExecuteNonQuery();
          flag = true;
        }
        catch (Exception)
        {
        }
        connection.Close();
        connection.Dispose();
      }
      return flag;
    }

    public static bool insertnewCycleWithDate(
      string deviceTag,
      string userTag,
      int cleaner,
      int state,
      int oldState,
      int idExamToSave,
      DateTime date,
      string connectionString)
    {
      bool flag = false;
      if (oldState < 0)
        oldState = 2;
      if (deviceTag != null && deviceTag != "" && (userTag != null && userTag != "") && (state >= 0 && oldState >= 0))
      {
        OdbcConnection connection = new OdbcConnection();
        try
        {
          connection.ConnectionString = connectionString;
          connection.Open();
        }
        catch (Exception)
        {
          connection.Dispose();
          return flag;
        }
        try
        {
          OdbcParameter odbcParameter1 = new OdbcParameter("RES", OdbcType.Int);
          odbcParameter1.Direction = ParameterDirection.Output;
          OdbcParameter odbcParameter2 = new OdbcParameter("TAGDEVICE", OdbcType.VarChar, 50);
          odbcParameter2.Direction = ParameterDirection.Input;
          odbcParameter2.Value = (object) deviceTag;
          OdbcParameter odbcParameter3 = new OdbcParameter("STATE", OdbcType.Int);
          odbcParameter3.Direction = ParameterDirection.Input;
          odbcParameter3.Value = (object) state;
          OdbcParameter odbcParameter4 = new OdbcParameter("DATECLEAN", OdbcType.VarChar, 14);
          string str = date.ToString("yyyyMMddHHmmss");
          odbcParameter4.Direction = ParameterDirection.Input;
          odbcParameter4.Value = (object) str;
          OdbcParameter odbcParameter5 = new OdbcParameter("TAGUSERCLEAN", OdbcType.VarChar, 50);
          odbcParameter5.Direction = ParameterDirection.Input;
          odbcParameter5.Value = (object) userTag;
          OdbcParameter odbcParameter6 = new OdbcParameter("CLEANER", OdbcType.Int);
          odbcParameter6.Direction = ParameterDirection.Input;
          odbcParameter6.Value = (object) cleaner;
          OdbcParameter odbcParameter7 = new OdbcParameter("OLDSTATE", OdbcType.Int);
          odbcParameter7.Direction = ParameterDirection.Input;
          odbcParameter7.Value = (object) oldState;
          OdbcParameter odbcParameter8 = new OdbcParameter("EXAM", OdbcType.Int);
          odbcParameter8.Direction = ParameterDirection.Input;
          odbcParameter8.Value = (object) idExamToSave;
          OdbcCommand odbcCommand = new OdbcCommand("{call sp_updatedevnew (?,?,?,?,?,?,?,?)}", connection);
          odbcCommand.CommandType = CommandType.StoredProcedure;
          odbcCommand.Parameters.Add(odbcParameter1);
          odbcCommand.Parameters.Add(odbcParameter2);
          odbcCommand.Parameters.Add(odbcParameter3);
          odbcCommand.Parameters.Add(odbcParameter4);
          odbcCommand.Parameters.Add(odbcParameter5);
          odbcCommand.Parameters.Add(odbcParameter6);
          odbcCommand.Parameters.Add(odbcParameter7);
          odbcCommand.Parameters.Add(odbcParameter8);
          odbcCommand.ExecuteNonQuery();
          flag = true;
        }
        catch (Exception)
        {
        }
        connection.Close();
        connection.Dispose();
      }
      return flag;
    }

    public static int insertnewCycleWithDateForReturn(
      string deviceTag,
      string userTag,
      int cleaner,
      int state,
      int oldState,
      int idExamToSave,
      DateTime date,
      string connectionString)
    {
      if (oldState < 0)
        oldState = 2;
      if (deviceTag != null && deviceTag != "" && (userTag != null && userTag != "") && (state >= 0 && oldState >= 0))
      {
        OdbcConnection connection = new OdbcConnection();
        try
        {
          connection.ConnectionString = connectionString;
          connection.Open();
        }
        catch (Exception)
        {
          connection.Dispose();
          return -10;
        }
        try
        {
          OdbcParameter odbcParameter1 = new OdbcParameter("RES", OdbcType.Int);
          odbcParameter1.Direction = ParameterDirection.Output;
          OdbcParameter odbcParameter2 = new OdbcParameter("TAGDEVICE", OdbcType.VarChar, 50);
          odbcParameter2.Direction = ParameterDirection.Input;
          odbcParameter2.Value = (object) deviceTag;
          OdbcParameter odbcParameter3 = new OdbcParameter("STATE", OdbcType.Int);
          odbcParameter3.Direction = ParameterDirection.Input;
          odbcParameter3.Value = (object) state;
          OdbcParameter odbcParameter4 = new OdbcParameter("DATECLEAN", OdbcType.VarChar, 14);
          string str = date.ToString("yyyyMMddHHmmss");
          odbcParameter4.Direction = ParameterDirection.Input;
          odbcParameter4.Value = (object) str;
          OdbcParameter odbcParameter5 = new OdbcParameter("TAGUSERCLEAN", OdbcType.VarChar, 50);
          odbcParameter5.Direction = ParameterDirection.Input;
          odbcParameter5.Value = (object) userTag;
          OdbcParameter odbcParameter6 = new OdbcParameter("CLEANER", OdbcType.Int);
          odbcParameter6.Direction = ParameterDirection.Input;
          odbcParameter6.Value = (object) cleaner;
          OdbcParameter odbcParameter7 = new OdbcParameter("OLDSTATE", OdbcType.Int);
          odbcParameter7.Direction = ParameterDirection.Input;
          odbcParameter7.Value = (object) oldState;
          OdbcParameter odbcParameter8 = new OdbcParameter("EXAM", OdbcType.Int);
          odbcParameter8.Direction = ParameterDirection.Input;
          odbcParameter8.Value = (object) idExamToSave;
          OdbcCommand odbcCommand = new OdbcCommand("{call sp_updatedevnew (?,?,?,?,?,?,?,?)}", connection);
          odbcCommand.CommandType = CommandType.StoredProcedure;
          odbcCommand.Parameters.Add(odbcParameter1);
          odbcCommand.Parameters.Add(odbcParameter2);
          odbcCommand.Parameters.Add(odbcParameter3);
          odbcCommand.Parameters.Add(odbcParameter4);
          odbcCommand.Parameters.Add(odbcParameter5);
          odbcCommand.Parameters.Add(odbcParameter6);
          odbcCommand.Parameters.Add(odbcParameter7);
          odbcCommand.Parameters.Add(odbcParameter8);
          odbcCommand.ExecuteNonQuery();
          int num;
          try
          {
            num = (int) odbcParameter1.Value;
          }
          catch
          {
            num = -10;
          }
          return num;
        }
        catch (Exception)
        {
        }
        connection.Close();
        connection.Dispose();
      }
      return -10;
    }

    public static int insertnewCycleForReturn(
      string deviceTag,
      string userTag,
      int cleaner,
      int state,
      int oldState,
      int idExamToSave,
      string connectionString)
    {
      if (oldState < 0)
        oldState = 2;
      if (deviceTag != null && deviceTag != "" && (userTag != null && userTag != "") && (state >= 0 && oldState >= 0))
      {
        OdbcConnection connection = new OdbcConnection();
        try
        {
          connection.ConnectionString = connectionString;
          connection.Open();
        }
        catch (Exception)
        {
          connection.Dispose();
          return -10;
        }
        try
        {
          OdbcParameter odbcParameter1 = new OdbcParameter("RES", OdbcType.Int);
          odbcParameter1.Direction = ParameterDirection.Output;
          OdbcParameter odbcParameter2 = new OdbcParameter("TAGDEVICE", OdbcType.VarChar, 50);
          odbcParameter2.Direction = ParameterDirection.Input;
          odbcParameter2.Value = (object) deviceTag;
          OdbcParameter odbcParameter3 = new OdbcParameter("STATE", OdbcType.Int);
          odbcParameter3.Direction = ParameterDirection.Input;
          odbcParameter3.Value = (object) state;
          OdbcParameter odbcParameter4 = new OdbcParameter("DATECLEAN", OdbcType.VarChar, 14);
          string str = DateTime.Now.ToString("yyyyMMddHHmmss");
          odbcParameter4.Direction = ParameterDirection.Input;
          odbcParameter4.Value = (object) str;
          OdbcParameter odbcParameter5 = new OdbcParameter("TAGUSERCLEAN", OdbcType.VarChar, 50);
          odbcParameter5.Direction = ParameterDirection.Input;
          odbcParameter5.Value = (object) userTag;
          OdbcParameter odbcParameter6 = new OdbcParameter("CLEANER", OdbcType.Int);
          odbcParameter6.Direction = ParameterDirection.Input;
          odbcParameter6.Value = (object) cleaner;
          OdbcParameter odbcParameter7 = new OdbcParameter("OLDSTATE", OdbcType.Int);
          odbcParameter7.Direction = ParameterDirection.Input;
          odbcParameter7.Value = (object) oldState;
          OdbcParameter odbcParameter8 = new OdbcParameter("EXAM", OdbcType.Int);
          odbcParameter8.Direction = ParameterDirection.Input;
          odbcParameter8.Value = (object) idExamToSave;
          OdbcCommand odbcCommand = new OdbcCommand("{call sp_updatedevnew (?,?,?,?,?,?,?,?)}", connection);
          odbcCommand.CommandType = CommandType.StoredProcedure;
          odbcCommand.Parameters.Add(odbcParameter1);
          odbcCommand.Parameters.Add(odbcParameter2);
          odbcCommand.Parameters.Add(odbcParameter3);
          odbcCommand.Parameters.Add(odbcParameter4);
          odbcCommand.Parameters.Add(odbcParameter5);
          odbcCommand.Parameters.Add(odbcParameter6);
          odbcCommand.Parameters.Add(odbcParameter7);
          odbcCommand.Parameters.Add(odbcParameter8);
          odbcCommand.ExecuteNonQuery();
          int num;
          try
          {
            num = (int) odbcParameter1.Value;
          }
          catch
          {
            num = -10;
          }
          return num;
        }
        catch (Exception)
        {
        }
        connection.Close();
        connection.Dispose();
      }
      return -10;
    }
  }
}
