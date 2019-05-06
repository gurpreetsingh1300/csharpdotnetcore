using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;
using OdeToFood.Core;
using System.Linq;
using Microsoft.Azure.Documents;

namespace OdeToFood.Data
{
    public class CosmosRestaurantData : IRestaurantData
    {
        private DocumentClient client;
        private Uri restaurantsLink;
        private FeedOptions options;
        public CosmosRestaurantData()
        {
            var uri = new Uri("https://localhost:8081");
            var key = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
            client = new DocumentClient(uri , key);
            //restaurantsLink = UriFactory.CreateDocumentCollectionUri("Food","Restaurants");
            options = new FeedOptions { EnableCrossPartitionQuery = true };


            client.CreateDatabaseIfNotExistsAsync(new Database { Id = "OdeToFood" });
            var databaseLink = UriFactory.CreateDatabaseUri("OdeToFood");
            client.CreateDocumentCollectionIfNotExistsAsync(databaseLink, new DocumentCollection { Id = "Restaurants" });
            restaurantsLink = UriFactory.CreateDocumentCollectionUri("OdeToFood", "Restaurants");
        }
        public Restaurant AddRestaurant(Restaurant addNewRestaurant)
        {
            var addRestaurantObj = client.CreateDocumentAsync(restaurantsLink , addNewRestaurant).Result.Resource ;            
            Restaurant rest = GetRestaurantById(addRestaurantObj.Id);
            return rest;
        }

        public int Commit()
        {
            throw new System.NotImplementedException();
        }

        public Restaurant DeleteRestaurant(string Id)
        {
            var rest = client.CreateDocumentQuery<Restaurant>(restaurantsLink, options).Where(r => r.Id == Id);
            //Restaurant rest = GetRestaurantById(Id);

            Uri u = UriFactory.CreateDocumentUri("OdeToFood", "Restaurants", Id); //"a536942d-3758-dccb-6a6e-553bf717b0c9");
            client.DeleteDocumentAsync(u);// , new RequestOptions { PartitionKey = new PartitionKey(1) });
            return new Restaurant();
        }

        public IEnumerable<Restaurant> GetAll()
        {
            var restaurants = client.CreateDocumentQuery<Restaurant>(restaurantsLink);
            
            //throw new System.NotImplementedException();
            return restaurants;
        }

        public Restaurant GetRestaurantById(string Id)
        {
            var restaurants = client.CreateDocumentQuery<Restaurant>(restaurantsLink, options).Where(r => r.Id == Id);
            Restaurant[] restaurantArray = restaurants.ToArray<Restaurant>();
            Restaurant restaurant = restaurantArray[0];
            return restaurant;
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name)
        {

            var restaurants = client.CreateDocumentQuery<Restaurant>(restaurantsLink,options).Where( r => r.Name.StartsWith(name) || string.IsNullOrEmpty(name));           
            //var restaurants = client.CreateDocumentQuery<Restaurant>(restaurantsLink, options);
            //restaurants = from r in restaurants
            //              where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
            //              orderby r.Name
            //              select r;
            //throw new System.NotImplementedException();
            return restaurants;
        }

        public Restaurant UpdateRestaurant(Restaurant updatedRestaurant)
        {
            var updateRestaurantObj = client.UpsertDocumentAsync(restaurantsLink, updatedRestaurant).Result.Resource;
            Restaurant rest = GetRestaurantById(updatedRestaurant.Id);
            return rest;
        }
    }
}
