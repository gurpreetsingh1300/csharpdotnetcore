using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace functions
{
    public static class OnPaymentReceived
    {
        [FunctionName("OnPaymentReceived")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Queue("orders")] IAsyncCollector<Order> orderQueue,
            ILogger log)
        {
            log.LogInformation("Payment Received");

            //string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var order = JsonConvert.DeserializeObject<Order>(requestBody);
            await orderQueue.AddAsync(order);
            log.LogInformation($"Order {order.OrderId} received from {order.Email} for product {order.ProductId}");
            //name = name ?? data?.name;

            //return name != null
            //    ? (ActionResult)new OkObjectResult($"Hello, {name}")
            //    : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
            return new OkObjectResult($"Thank you for purchase") ;
        }
    }
    public class Order
    {
        public string OrderId { get; set;}
        public string ProductId { get; set;}
        public string Email { get; set;}
        public decimal Price { get; set; }
    }
}
