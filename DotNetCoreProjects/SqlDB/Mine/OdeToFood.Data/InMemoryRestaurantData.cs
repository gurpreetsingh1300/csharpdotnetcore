﻿using System.Linq;
using OdeToFood.Core;
using System.Collections.Generic;


namespace OdeToFood.Data
{
    public class InMemoryRestaurantData : IRestaurantData
    {
        readonly List<Restaurant> restaurants;

        public InMemoryRestaurantData()
        {
            restaurants = new List<Restaurant>()
            {
                new Restaurant { Id = 1, Name = "Scott's Pizza", Location="Maryland", Cuisine=CuisineType.Italian},
                new Restaurant { Id = 2, Name = "Cinnamon Club", Location="London", Cuisine=CuisineType.Italian},
                new Restaurant { Id = 3, Name = "La Costa", Location = "California", Cuisine=CuisineType.Mexican}
            };
        }
        public Restaurant GetRestaurantById(int Id)
        {
            return restaurants.SingleOrDefault(r => r.Id == Id);
        }
        public Restaurant AddRestaurant(Restaurant newRestaurant)
        {
            newRestaurant.Id = restaurants.Max(r => r.Id) + 1;
            restaurants.Add(newRestaurant);
            return newRestaurant;
        }
        public Restaurant UpdateRestaurant(Restaurant updatedRestaurant)
        {
            var restaurant = restaurants.SingleOrDefault(r => r.Id == updatedRestaurant.Id);
            if (restaurant != null)
            {
                restaurant.Name = updatedRestaurant.Name;
                restaurant.Location = updatedRestaurant.Location;
                restaurant.Cuisine = updatedRestaurant.Cuisine;
            }
            return restaurant;
        }
        public IEnumerable<Restaurant> GetAll()
        {
            return from r in restaurants
                   orderby r.Name
                   select r;
        }
        public IEnumerable<Restaurant> GetRestaurantsByName(string name = null)
        {
            return from r in restaurants
                   where string.IsNullOrEmpty(name) || r.Name.StartsWith(name)
                   orderby r.Name
                   select r;
        }

        public int Commit()
        {
            return 0;
        }

        public Restaurant DeleteRestaurant(int Id)
        {
            var restaurant = restaurants.FirstOrDefault(r => r.Id == Id);
            if(restaurant != null)
            {
                restaurants.Remove(restaurant);
            }
            return restaurant;
        }
    }
}
