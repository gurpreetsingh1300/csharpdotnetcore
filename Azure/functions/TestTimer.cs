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



            Random random = new Random();


            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:7071/api/OnPaymentReceived");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            Order obj = new Order();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                obj.Email = "v-guseth@microsoft.com";
                obj.OrderId = "400" + random.Next(10000);
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
