using System.Collections.Generic;
using OdeToFood.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OdeToFood.Data
{
    public class SqlRestuarantData : IRestaurantData
    {
        private readonly OdeToFoodDbContext db;

        public SqlRestuarantData(OdeToFoodDbContext db)
        {
            this.db = db;
        }
        public Restaurant AddRestaurant(Restaurant addNewRestaurant)
        {
            db.Add(addNewRestaurant);
            return addNewRestaurant;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Restaurant DeleteRestaurant(string Id)
        {
            var restaurant = GetRestaurantById(Id);
            if(restaurant != null)
            {
                db.Restaurants.Remove(restaurant);
            }
            return restaurant;
        }

        public IEnumerable<Restaurant> GetAll()
        {
            var restaurants = from r in db.Restaurants
                              orderby r.Name
                              select r;
            return restaurants;
        }

        public Restaurant GetRestaurantById(string Id)
        {
            return db.Restaurants.Find(Id);
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name)
        {
            var restaurants = from r in db.Restaurants
                              where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                              orderby r.Name
                              select r;
            return restaurants;
        }

        public Restaurant UpdateRestaurant(Restaurant updatedRestaurant)
        {
            var entity = db.Restaurants.Attach(updatedRestaurant);
            entity.State = EntityState.Modified;
            return updatedRestaurant;
        }
    }
}
