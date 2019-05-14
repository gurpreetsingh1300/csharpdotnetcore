using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace GlobalDistribution
{
	public static class CreateFamiliesCollection
	{
		public static async Task Run()
		{
			var endpoint = ConfigurationManager.AppSettings["WusEndpoint"];
			var masterKey = ConfigurationManager.AppSettings["WusMasterKey"];

			using (var client = new DocumentClient(new Uri(endpoint), masterKey))
			{
				var db = new Database { Id = "Families" };
				await client.CreateDatabaseAsync(db);

				var coll = new DocumentCollection { Id = "Families" };
				coll.PartitionKey.Paths.Add("/address/zipCode");

				var dbUri = UriFactory.CreateDatabaseUri("Families");
				await client.CreateDocumentCollectionAsync(dbUri, coll, new RequestOptions { OfferThroughput = 1000 });

				var collUri = UriFactory.CreateDocumentCollectionUri("Families", "Families");
				await AddSmithFamilyDocument(client, collUri);
				await AddJonesFamilyDocument(client, collUri);
			}
		}

		private static async Task AddSmithFamilyDocument(DocumentClient client, Uri collUri)
		{
			dynamic documentDef = new
			{
				familyName = "Smith",
				address = new
				{
					addressLine = "123 Main Street",
					city = "Chicago",
					state = "IL",
					zipCode = "60601"
				},
				parents = new string[]
				{
					"Peter",
					"Alice"
				},
				kids = new string[]
				{
					"Adam",
					"Jacqueline",
					"Joshua"
				}
			};

			await client.CreateDocumentAsync(collUri, documentDef);
		}

		private static async Task AddJonesFamilyDocument(DocumentClient client, Uri collUri)
		{
			dynamic documentDef = new
			{
				familyName = "Jones",
				address = new
				{
					addressLine = "456 Harbor Boulevard",
					city = "Chicago",
					state = "IL",
					zipCode = "60603"
				},
				parents = new string[]
				{
					"David",
					"Diana"
				},
				kids = new string[]
				{
					"Evan"
				},
				pets = new string[]
				{
					"Lint"
				}
			};

			await client.CreateDocumentAsync(collUri, documentDef);
		}

	}
}
