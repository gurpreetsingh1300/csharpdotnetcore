﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OdeToFood.Data;
using OdeToFood.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OdeToFood.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData restaurantData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public Restaurant Restaurant { get; set; }
        public IEnumerable<SelectListItem> Cuisines { get; set; }
        public EditModel(IRestaurantData restaurantData,IHtmlHelper htmlHelper)
        {
            this.restaurantData = restaurantData;
            this.htmlHelper = htmlHelper;
        }
        public IActionResult OnGet(int? restaurantId)
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>();
            if (restaurantId.HasValue)
            {
                Restaurant = restaurantData.GetRestaurantById(restaurantId.Value);
            }
            else
            {
                Restaurant = new Restaurant();
            }
            if (Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            Cuisines = htmlHelper.GetEnumSelectList<CuisineType>(); // need again in post method ... for populating during post operation
            if (ModelState.IsValid)
            {
                if (Restaurant.Id > 0)
                {
                    //Restaurant = restaurantData.UpdateRestaurant(Restaurant); this also works
                    restaurantData.UpdateRestaurant(Restaurant); //this is able to work due to model binding 

                }
                else
                {
                    restaurantData.AddRestaurant(Restaurant);
                }
                TempData["Message"] = "Restaurant saved!";
                restaurantData.Commit();
                return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
                //return RedirectToPage("./Detail/" + Restaurant.Id);
            }
            
            return Page();
        }
    }
}