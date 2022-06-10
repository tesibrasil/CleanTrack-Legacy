using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CleanTrackPi.Helpers
{
    public class Util
    {
        public static string LogFolder =
        Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) +
            Path.DirectorySeparatorChar + "log" + Path.DirectorySeparatorChar;
        
        public static void Wait(int s) => Task.Delay(s * 1000).Wait();

        public static Task WaitAsync(int s) => Task.Delay(s * 1000);

        public static string GetLocalIPAddress()
        {
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                if ((ip.AddressFamily == AddressFamily.InterNetwork) &&
                    (!IPAddress.IsLoopback(ip)))
                     return ip.ToString();

            return String.Empty;
        }

        public static void WriteLog(string content)
        {
            try
            {
                if (!Directory.Exists(LogFolder))
                    Directory.CreateDirectory(LogFolder);
                
                string filePath = LogFolder + String.Format("{0}.log", DateTime.Now.ToString("yyyyMMdd"));               
                using (StreamWriter outputFile = new StreamWriter(filePath, File.Exists(filePath)))
                    outputFile.WriteLine(DateTime.Now.ToShortTimeString() + " - " + content);
            }
            catch (Exception exc)
            {
                
            }
        }

        public static bool CheckServerStatus(string url) //265 MainPage.xaml.cs
        {           
            try
            {
                PingReply r = new Ping().Send(Dns.GetHostAddresses(new Uri(url).Host)[0]);
                if (r.Status == IPStatus.Success)
                {
                    WriteLog($"IP Status: Success - Address: {r.Address}");
                    return true;
                }
                else
                    WriteLog($"Error at ping method:\n{r.Status}");

            }
            catch (Exception ex)
            {
                WriteLog($"Error at ping method:\n{ex}");
                WriteLog(ex.StackTrace);
            }

            return false;
        }
    }
}
