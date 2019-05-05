using OdeToFood.Core;
using System.Collections.Generic;


namespace OdeToFood.Data
{
    public interface IRestaurantData
    {
        IEnumerable<Restaurant> GetAll();
        IEnumerable<Restaurant> GetRestaurantsByName(string name);
        Restaurant GetRestaurantById(string Id);
        Restaurant AddRestaurant(Restaurant addNewRestaurant);
        Restaurant UpdateRestaurant(Restaurant updatedRestaurant);
        Restaurant DeleteRestaurant(string Id);
        int Commit();
    }
}
