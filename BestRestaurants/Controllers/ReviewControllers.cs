using Microsoft.AspNetCore.Mvc;
using BestRestaurants.Models;
using System.Collections.Generic;

namespace BestRestaurants.Controllers
{
  public class ReviewController : Controller
  {
    [HttpGet("/restaurants/{restaurantId}/reviews/new")]
    public ActionResult New(int restaurantId)
    {
     Restaurant restaurant = Restaurant.Find(restaurantId);
     return View(restaurant);
    }

    [HttpPost("/restaurants/{restaurantId}/reviews")]
    public ActionResult Index(int restaurantId, string review, int star)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Restaurant foundRestaurant = Restaurant.Find(restaurantId);
      Review newReview = new Review(star, review, restaurantId);
      newReview.Save();
      List<Review> reviews = foundRestaurant.GetReviews();
      model.Add("restaurants", foundRestaurant);
      model.Add("review", reviews);
      return View(model);
    }

  }
}
