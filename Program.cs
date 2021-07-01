using CommandLine;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MyAppConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(o =>
            {
                var returnObject = new ReturnObject();
                try
                {
                    Console.WriteLine("API import starting...");
                    var results = GetDataFromAPI(o.ApiKey, o.Symbol);
                    // var values = JsonConvert.DeserializeObject<TypeResults>(results);
                    // SaveValuesInDatabase(values);
                    Console.WriteLine("API import ended successfully");
                } catch (Exception e)
                {
                    returnObject.Error = -1;
                    returnObject.ErrorMessage = e.Message;
                    returnObject.StackTrace = e.StackTrace;
                    Console.Write(JsonConvert.SerializeObject(returnObject));
                }
            });
        }

        private static string GetDataFromAPI(string apiKey, string symbol)
        {
            var parameters = new Dictionary<string, string>() {
                { "symbol", symbol },
                { "interval", "1d" }
            };

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("APIKEYPARAM", apiKey);

                var baseUrl = "https://api.binance.com/api/v3/klines";
                var url = baseUrl + "?" + string.Join("&", parameters.Select(x => string.Format("{0}={1}", x.Key, x.Value)));
                var task = client.GetAsync(url);
                task.Wait();

                if (!task.Result.IsSuccessStatusCode)
                {
                    throw new Exception(task.Result.StatusCode.ToString());
                }

                var readTask = task.Result.Content.ReadAsStringAsync();
                readTask.Wait();
                return readTask.Result;
            }
        }
    }
}
