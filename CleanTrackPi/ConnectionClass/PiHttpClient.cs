using KleanTrak.Model;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace CleanTrackPi.ConnectionClass
{
    public class PiHttpClient
    {
        public static async Task<Response> Send(string url, Request req)
        {
            Response ret = null;

            try
            {
                System.Net.Http.StringContent content = new StringContent(req.SaveObjectToXml());
                HttpClient httpClient = new HttpClient();
                var myRequest = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, new Uri(url));
                myRequest.Content = content;

                //Send the GET request asynchronously and retrieve the response as a string.
                System.Net.Http.HttpResponseMessage httpResponse = await httpClient.SendAsync(myRequest);
                httpResponse.EnsureSuccessStatusCode();

                var buffer = await httpResponse.Content.ReadAsStreamAsync();
                StreamReader streamReader = new StreamReader(buffer);
                string httpResponseBody = await streamReader.ReadToEndAsync();
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
