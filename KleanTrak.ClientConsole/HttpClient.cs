using System;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Xml.Serialization;
using KleanTrak.Model;

namespace KleanTrak.ClientConsole
{
	public class HttpClient
    {
        public static Response Send(string url, Request req)
        {
			Stream dataStream = null;
			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(HttpWebRequest));
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
				request.Method = "POST";
				request.ContentType = "text/xml";
				Console.WriteLine("request:");
				serializer.Serialize(Console.OpenStandardOutput(), request);
				dataStream = request.GetRequestStream();
				serializer.Serialize(dataStream, req);
				dataStream.Flush();
				dataStream.Close();
				dataStream = request.GetResponse().GetResponseStream();
				string responseFromServer = new StreamReader(dataStream).ReadToEnd();
				Console.WriteLine("server response:");
				Console.Write(responseFromServer);
				return (Response)(new XmlSerializer(typeof(Response)).Deserialize(dataStream));
			}
			catch (Exception e)
			{
				return new Response
				{
					Successed = false,
					ErrorMessage = e.ToString()
				};
			}
			finally
			{
				if(dataStream != null)
					dataStream.Close();
			}
		}
    }
}
