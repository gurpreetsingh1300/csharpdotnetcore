using System;
using System.Collections.Specialized;
using System.Net;
//using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace functions
{
    public static class TestTimer
    {
        private static readonly HttpClient client = new HttpClient();

        [FunctionName("TestTimer")]
        public static void Run([TimerTrigger("*/40 * * * * *")]TimerInfo myTimer, ILogger log)
        {

            //log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            //using (var client = new WebClient())
            //{
            //    Random random = new Random();
            //    var values = new NameValueCollection();
            //    values["OrderId"] = "3007" + random.Next(10000);
            //    values["Email"] = "v-guseth@microsoft.com";
            //    values["ProductId"] = "P155";

            //    var response = client.UploadValues("http://localhost:7071/api/OnPaymentReceived", values);

            //    var responseString = Encoding.Default.GetString(response);
            //}

            Random random = new Random();
            //var values = new Dictionary<string, string>
            //{
            //   { "OrderId", "400"+ random.Next(10000) },
            //   { "Email", "v-guseth@microsoft.com" },
            //   { "ProductId", "P155" }
            //};

            //var content = new FormUrlEncodedContent(values);
            
            //var response = await client.PostAsync("http://localhost:7071/api/OnPaymentReceived", content);

            //var responseString = await response.Content.ReadAsStringAsync();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:7071/api/OnPaymentReceived");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            Order obj = new Order();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                obj.Email = "v-guseth@microsoft.com";
                obj.OrderId = "400" + random.Next(10000) ;
                obj.ProductId = "P155";


                string json = JsonConvert.SerializeObject(obj);
                
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
}
