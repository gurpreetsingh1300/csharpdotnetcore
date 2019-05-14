using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CrossPartitionQueries
{
	public static class CrossPartitionQueriesDemo
	{
		public async static Task Run()
		{
			Debugger.Break();

			var endpoint = ConfigurationManager.AppSettings["EmulatorEndpoint"];
			var masterKey = ConfigurationManager.AppSettings["EmulatorMasterKey"];

			using (var client = new DocumentClient(new Uri(endpoint), masterKey))
			{
				var db = new Database { Id = "Families" };
				await client.CreateDatabaseAsync(db);

				var coll = new DocumentCollection { Id = "Families" };
				coll.PartitionKey.Paths.Add("/address/zipCode");

				var dbUri = UriFactory.CreateDatabaseUri("Families");
				await client.CreateDocumentCollectionAsync(dbUri, coll, new RequestOptions { OfferThroughput = 20000 });

				var collUri = UriFactory.CreateDocumentCollectionUri("Families", "Families");
				await AddSmithFamilyDocument(client, collUri);
				await AddJonesFamilyDocument(client, collUri);

				var sql = "SELECT * FROM c WHERE c.address.zipCode = '60603'";
				var query = client.CreateDocumentQuery<Document>(collUri, sql);
				var result = query.ToList();

				sql = "SELECT * FROM c WHERE c.address.city = 'Chicago'";
				query = client.CreateDocumentQuery<Document>(collUri, sql);
				try
				{
					result = query.ToList();
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.InnerException.Message);
				}

				sql = "SELECT * FROM c WHERE c.address.city = 'Chicago'";
				var options = new FeedOptions { EnableCrossPartitionQuery = true, MaxDegreeOfParallelism = -1 };
				query = client.CreateDocumentQuery<Document>(collUri, sql, options);
				result = query.ToList();

				await client.DeleteDatabaseAsync(dbUri);
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

			ResourceResponse<Document> result = await client.CreateDocumentAsync(collUri, documentDef);
			var document = result.Resource;
			Console.WriteLine($"Created Smith document with ID: {document.Id}");
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

			ResourceResponse<Document> result = await client.CreateDocumentAsync(collUri, documentDef);
			var document = result.Resource;
			Console.WriteLine($"Created Jones document with ID: {document.Id}");
		}

	}
}
