using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.Storage.Streams;
using KleanTrak.Model;

namespace KleanTrak.Pi
{
    public class HttpServer : IDisposable
    {
        private const uint BufferSize = 8192;
        private readonly StreamSocketListener listener = new StreamSocketListener();

        public delegate Task<Response> RequestReceivedHandler(Request cmd);

        public event RequestReceivedHandler RequestReceived;

        public HttpServer(int port)
        {
            Init(port);
        }
        protected async void Init(int port)
        {
            listener.ConnectionReceived += (s, e) => ProcessRequestAsync(e.Socket);
            await listener.BindServiceNameAsync(port.ToString());
        }

        public void Dispose()
        {
            listener.Dispose();
        }

        private async void ProcessRequestAsync(StreamSocket socket)
        {
            // this works for text only
            string strRequest = "", strMethod = "", strUri = "", strPayload = "";

            using (IInputStream input = socket.InputStream)
            {
                uint payloadSize = 0;
                int bytesReceived = 0;

                while (true)
                {
                    byte[] data = new byte[BufferSize];
                    IBuffer buffer = data.AsBuffer();
                    await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                    if (buffer.Length == 0)
                        break;

                    bytesReceived += (int)buffer.Length;
                    strRequest += Encoding.UTF8.GetString(data, 0, (int)buffer.Length);

                    // Provo a splittare l'header http
                    if (strMethod == "" || strUri == "" || payloadSize == 0)
                    {
                        string[] requestHeader = strRequest.Split('\n');
                        for (int i = 0; i < requestHeader.Length; i++)
                        {
                            string[] requestRowParts = requestHeader[i].Split(' ');
                            if (requestRowParts.Length < 2)
                                continue;

                            if (i == 0)
                            {
                                if (requestRowParts.Length != 3)
                                    continue;

                                strMethod = requestRowParts[0];
                                strUri = requestRowParts[1];
                            }
                            else
                            {
                                // Ricavo dimensione payload
                                if (requestRowParts[0].ToUpper().CompareTo("CONTENT-LENGTH:") == 0)
                                    payloadSize = Convert.ToUInt32(requestRowParts[1]);
                            }
                        }
                    }


                    if (payloadSize > 0)
                    {
                        int posPayload = strRequest.IndexOf("\r\n\r\n");
                        if (posPayload > 0 && bytesReceived - posPayload - 4 >= payloadSize)
                        {
                            strPayload = strRequest.Substring(posPayload + 4);
                            break;
                        }
                    }
                }
            }

            using (IOutputStream output = socket.OutputStream)
            {
                if (strMethod == "POST" && strUri == "/AcceptMessage")
                {
                    await WriteResponseAsync(strPayload, output);
                }
                else
                {
                    byte[] headerArray = Encoding.UTF8.GetBytes(
                                          "HTTP/1.1 404 Not Found\r\n" +
                                          "Content-Length:0\r\n" +
                                          "Connection: close\r\n\r\n");

                    await output.AsStreamForWrite().WriteAsync(headerArray, 0, headerArray.Length);
                }
            }
        }

        private async Task WriteResponseAsync(string content, IOutputStream os)
        {
            string payload, result, error;

            using (Stream response = os.AsStreamForWrite())
            {
                try
                {
                    Request req = (Request)Request.ReadObjectFromXml(content);
                    if (req == null)
                    {
                        payload = "Input object not recognized";
                        result = "400";
                        error = "Bad request";
                    }
                    else
                    {
                        if (RequestReceived != null)
                        {
                            Response resp = await RequestReceived(req);
                            if (resp != null)
                            {
                                payload = resp.SaveObjectToXml();
                                result = "200";
                                error = "OK";
                            }
                            else
                            {
                                payload = "Error managing request";
                                result = "500";
                                error = "Internal error";
                            }
                        }
                        else
                        {
                            payload = "No listener";
                            result = "500";
                            error = "Internal error";
                        }
                    }
                }
                catch (Exception ex)
                {
                    payload = ex.Message + " - " + ex.StackTrace.ToString();
                    result = "500";
                    error = "Internal error";
                }

                Stream stream = ToStream(payload);
                string header = String.Format("HTTP/1.1 " + result + " " + error + "\r\n" +
                                "Content-Length: {0}\r\n" +
                                "Connection: close\r\n\r\n",
                                stream.Length);

                byte[] headerArray = Encoding.UTF8.GetBytes(header);
                await response.WriteAsync(headerArray, 0, headerArray.Length);
                await stream.CopyToAsync(response);
                await response.FlushAsync();
            }
        }

        private static MemoryStream ToStream(string text)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
