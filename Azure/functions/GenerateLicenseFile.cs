using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace functions
{
    public static class GenerateLicenseFile
    {
        [FunctionName("GenerateLicenseFile")]
        public static void Run([QueueTrigger("orders", Connection = "AzureWebJobsStorage")]Order order, [Blob("licenses/{rand-guid}.lic")]TextWriter outputBlob, ILogger log)
        {
            outputBlob.WriteLine($"OrderId: {order.OrderId}");
            outputBlob.WriteLine($"Email: {order.Email}");
            outputBlob.WriteLine($"ProductId: {order.ProductId}");
            outputBlob.WriteLine($"PurchaseDate: {DateTime.UtcNow}");
            log.LogInformation($"C# Queue trigger function processed: {order}");
        }
    }
}
