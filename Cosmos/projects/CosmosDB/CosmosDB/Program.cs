using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace CosmosDB
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                Debugger.Break();

                var endpoint = ConfigurationManager.AppSettings["CosmosDbEndpoint"];
                var masterKey = ConfigurationManager.AppSettings["CosmosDbMasterKey"];

                using (var client = new DocumentClient(new Uri(endpoint), masterKey))
                {
                    ViewDatabases(client);

                    await CreateDatabase(client);
                    ViewDatabases(client);

                    await DeleteDatabase(client);
                }


            }
            ).Wait();
        }
        private static void ViewDatabases(DocumentClient client)
        {
            Console.WriteLine();
            Console.WriteLine(">>> View Databases <<<");

            var databases = client.CreateDatabaseQuery().ToList();
            foreach (var database in databases)
            {
                Console.WriteLine($" Database Id: {database.Id}; Rid: {database.ResourceId}");
            }

            Console.WriteLine();
            Console.WriteLine($"Total databases: {databases.Count}");
        }

        private async static Task CreateDatabase(DocumentClient client)
        {
            Console.WriteLine();
            Console.WriteLine(">>> Create Database <<<");

            var databaseDefinition = new Database { Id = "MyNewDatabase" };
            var result = await client.CreateDatabaseAsync(databaseDefinition);
            var database = result.Resource;

            Console.WriteLine($" Database Id: {database.Id}; Rid: {database.ResourceId}");
        }

        private async static Task DeleteDatabase(DocumentClient client)
        {
            Console.WriteLine();
            Console.WriteLine(">>> Delete Database <<<");

            var databaseUri = UriFactory.CreateDatabaseUri("MyNewDatabase");
            await client.DeleteDatabaseAsync(databaseUri);
        }
    }
}
