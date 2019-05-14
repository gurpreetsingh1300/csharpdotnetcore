using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;

namespace JsonAdd
{
    class JsonAddClass
    {
        private DocumentClient client;
        private Uri restaurantsLink;
        public JsonAddClass()
        {
            var uri = new Uri("https://localhost:8081");
            var key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            client = new DocumentClient(uri, key);
            client.CreateDatabaseIfNotExistsAsync(new Database { Id = "Jobs" });
            var databaseLink = UriFactory.CreateDatabaseUri("Jobs");
            client.CreateDocumentCollectionIfNotExistsAsync(databaseLink, new DocumentCollection { Id = "Jobs" });
            restaurantsLink = UriFactory.CreateDocumentCollectionUri("Jobs", "Jobs");
        }
        public void Add(List<dynamic> objs)
        {
            foreach(var obj in objs)
            {
                var addRestaurantObj = client.CreateDocumentAsync(restaurantsLink, objs);
            }

            
        }

    }
}
