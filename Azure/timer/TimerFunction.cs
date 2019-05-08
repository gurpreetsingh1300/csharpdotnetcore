using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

using System.Net;
using System.Collections.Specialized;
using System.Text;

namespace timer
{
    public static class TimerFunction
    {
        [FunctionName("TimerFunction")]
        public static void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["OrderId"] = "3007";
                values["Email"] = "v-guseth@microsoft.com";
                values["ProductId"] = "P155";

                var response = client.UploadValues("http://localhost:7071/api/OnPaymentReceived", values);

                var responseString = Encoding.Default.GetString(response);
            }
        }
    }
}
