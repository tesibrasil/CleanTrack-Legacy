using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CleanTrackPi.Helpers
{
    public class Listener
    {
        public static Task recv = null;

        public static int Port { get; } = 11000;

        public delegate void SetBarcodeTextDelegate(string text);

        public static void ReceiveBarcode(SetBarcodeTextDelegate callback)
        {
            recv = Task.Run(async () =>
            {
                try
                {
                    using (var cli = new UdpClient(Port))
                    {
                        string received = String.Empty;
                        bool completed = false;
                        do
                        {
                            var tmp = Encoding.UTF8.GetString((await cli.ReceiveAsync()).Buffer);
                            if (tmp.Contains("\r"))
                            {
                                received += tmp.Replace("\r", String.Empty);
                                received = received.Trim('\0');
                                callback(received);
                                received = string.Empty;
                            }
                            tmp = String.Empty;
                        } while (!completed);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
            });
        }

        public static void SendToken(string code)
        {
            UdpClient udpClient = new UdpClient();
            try
            {
                Byte[] sendBytes = Encoding.ASCII.GetBytes(code);
                udpClient.Send(sendBytes,
                    sendBytes.Length, "127.0.0.1", Listener.Port);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
