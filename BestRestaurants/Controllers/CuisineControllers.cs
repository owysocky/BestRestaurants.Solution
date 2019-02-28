using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using BestRestaurants.Models;

namespace BestRestaurants.Controllers
{
  public class CuisineController : Controller
  {
    [HttpGet("/cuisines")]
    public ActionResult Index()
    {
      List<Cuisine> allcuisines = Cuisine.GetAll();
      return View(allcuisines);
    }

    [HttpGet("/cuisines/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/cuisines")]
    public ActionResult Create(string cuisineType)
    {
      Cuisine newCuisine = new Cuisine(cuisineType);
      newCuisine.Save();
      List<Cuisine> allcuisines = Cuisine.GetAll();
      return View("Index", allcuisines);
    }

    [HttpGet("/cuisines/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Cuisine selectedCuisine = Cuisine.Find(id);
      List<Restaurant> cuisineRestaurants = selectedCuisine.GetRestaurants();
      model.Add("cuisine", selectedCuisine);
      model.Add("restaurants", cuisineRestaurants);
      return View(model);
    }

    // This one creates new Restaurants within a given Cuisine, not new cuisines:
    [HttpPost("/cuisines/{cuisineId}/restaurants")]
    public ActionResult Create(int cuisineId, string restaurantName)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Cuisine foundCuisine = Cuisine.Find(cuisineId);
      Restaurant newRestaurant = new Restaurant(restaurantName, cuisineId);
      newRestaurant.Save();
      List<Restaurant> cuisineRestaurants = foundCuisine.GetRestaurants();
      model.Add("restaurants", cuisineRestaurants);
      model.Add("cuisine", foundCuisine);
      return View("Show", model);
    }

    
  }
}
