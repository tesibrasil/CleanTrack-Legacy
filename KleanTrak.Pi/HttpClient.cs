using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using KleanTrak.Model;
using Windows.Storage.Streams;

namespace KleanTrak.Pi
{
    public class HttpClient
    {
        public static async Task<Response> Send(string url, Request req)
        {
            Response ret = null;

            try
            {
                HttpStringContent content = new HttpStringContent(req.SaveObjectToXml());

                //Create an HTTP client object
                Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

                var myRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
                myRequest.Content = content;

                //Send the GET request asynchronously and retrieve the response as a string.
                Windows.Web.Http.HttpResponseMessage httpResponse = await httpClient.SendRequestAsync(myRequest);
                httpResponse.EnsureSuccessStatusCode();

                IBuffer buffer = await httpResponse.Content.ReadAsBufferAsync();
                DataReader dataReader = DataReader.FromBuffer(buffer);

                byte[] bytes = new byte[buffer.Length];
                dataReader.ReadBytes(bytes);
                string httpResponseBody = System.Text.Encoding.UTF8.GetString(bytes);
                dataReader.Dispose();
                
                ret = (Response)Response.ReadObjectFromXml(httpResponseBody);
            }
            catch (Exception ex)
            {
                ret = new Response();
                ret.Successed = false;
                ret.ErrorMessage = ex.Message + " " + ex.StackTrace.ToString();
            }

            return ret;
        }
    }
}
