using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GlobalDistribution
{
	public static class GlobalDistributionDemo
	{
		public static async Task Run()
		{
			var endpoint = ConfigurationManager.AppSettings["WusEndpoint"];
			var masterKey = ConfigurationManager.AppSettings["WusMasterKey"];

			var collUri = UriFactory.CreateDocumentCollectionUri("Families", "Families");
			var sql = "SELECT * FROM c WHERE c.address.zipCode = '60603'";

			var connectionPolicy = new ConnectionPolicy();
			connectionPolicy.PreferredLocations.Add(ConfigurationManager.AppSettings["PreferredRegion1"]);
			connectionPolicy.PreferredLocations.Add(ConfigurationManager.AppSettings["PreferredRegion2"]);

			using (var client = new DocumentClient(new Uri(endpoint), masterKey, connectionPolicy))
			{
				for (var i = 0; i < 100; i++)
				{
					var startedAt = DateTime.Now;
					var query = client.CreateDocumentQuery(collUri, sql).AsDocumentQuery();
					var result = await query.ExecuteNextAsync();
					Console.WriteLine($"{i + 1}. Elapsed: {DateTime.Now.Subtract(startedAt).TotalMilliseconds} ms; Cost: {result.RequestCharge} RUs");
				}
			}

			Console.ReadKey();
		}

	}
}
