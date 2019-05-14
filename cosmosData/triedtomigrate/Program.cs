using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;

namespace JsonAdd
{
    class Program
    {
        private Uri restaurantsLink;
        private DocumentClient client;
        static void Main(string[] args)
        {
            List<dynamic> objs = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText("rest.json"));
            JsonAddClass jsonObj = new JsonAddClass();
            jsonObj.Add(objs);


        }
        
    }
}
