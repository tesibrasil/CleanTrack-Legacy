using System;
using System.Data.Odbc;
using System.Net;
using System.Net.Mail;

namespace amrfidmgrex
{
  internal class NetworkUtility
  {
    private static bool SendMail(string text, string SMTPHost, bool SMTPLogin, string SMTPUser, string SMTPPassword, string OdbcConnectionString)
    {
      bool flag = false;
      OdbcConnection connection = new OdbcConnection(OdbcConnectionString);
      try
      {
        connection.Open();
        OdbcDataReader odbcDataReader = new OdbcCommand("SELECT MAIL FROM MAIL WHERE ELIMINATO=0", connection).ExecuteReader();
        while (odbcDataReader.Read())
        {
          if (!odbcDataReader.IsDBNull(odbcDataReader.GetOrdinal("MAIL")))
          {
            MailMessage message = new MailMessage("CLEANTRACK-NO_REPLY@tesi.mi.it", odbcDataReader.GetString(odbcDataReader.GetOrdinal("MAIL")), "INVALID OPERATION", text);
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = SMTPHost;
            if (SMTPLogin)
              smtpClient.Credentials = (ICredentialsByHost) new NetworkCredential(SMTPUser, SMTPPassword);
            try
            {
              smtpClient.Send(message);
              flag = true;
            }
            catch (Exception ex)
            {
              flag = false;
            }
          }
        }
        odbcDataReader.Close();
      }
      catch
      {
        flag = false;
      }
      return flag;
    }
  }
}
