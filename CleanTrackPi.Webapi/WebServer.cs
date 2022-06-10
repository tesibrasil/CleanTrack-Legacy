using System;
using System.IO;
using System.Threading.Tasks;
using KleanTrak.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CleantrackPi.Webapi { 
    
    public delegate bool DelegateConfig(string json);
    
    public class WebServer {
        public static Task Run(int port, DelegateConfig callback = null) {
            return Task.Run(async () => {
                try
                {
                    var app = WebApplication.CreateBuilder().Build();
                    app.MapPost("/AcceptMessage", async (context) =>
                    {
                        var val = await new StreamReader(context.Request.Body).ReadToEndAsync();
                        if ((val != null) &&
                            (callback != null))
                            await context.Response.WriteAsync(
                                new Response() { Successed = callback(val) }.SaveObjectToXml());
                    });
                    app.Run("http://*:" + port);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            });
        }

        public static bool callback_test(string val) => true;

        public static async Task Main() =>
            await Run(8090, callback_test);
    }
}
