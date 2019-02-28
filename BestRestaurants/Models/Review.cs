using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BestRestaurants.Models
{
  public class Review
  {
    private int _star;
    private string _review;
    private int _id;
    private int _restaurantId;

    public Review(int star, string review, int restaurantId, int id = 0)
    {
      _star = star;
      _review = review;
      _restaurantId = restaurantId;
    }

    public int GetStar(){return _star;}
    public void SetStar(int star){ _star = star;}
    public string GetReview(){return _review;}
    public void SetReview(string review){ _review = review;}
    public int GetId(){return _id;}
    public int GetRestaurantId(){return _restaurantId;}

    public static List<Review> GetAll()
    {
      List<Review> allReviews = new List<Review>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM reviews;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
      int star = rdr.GetInt32(0);
      string review = rdr.GetString(1);
      int restaurantId = rdr.GetInt32(2);
      int id = rdr.GetInt32(3);
      Review newReview = new Review(star, review, restaurantId, id);
      allReviews.Add(newReview);
      }
      conn.Close();
      if (conn != null)
      {
       conn.Dispose();
      }
      return allReviews;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO reviews (star, review, restaurant_id) VALUES (@star, @review, @restaurant_id);";
      MySqlParameter star = new MySqlParameter();
      star.ParameterName = "@star";
      star.Value = this._star;
      cmd.Parameters.Add(star);
      MySqlParameter review = new MySqlParameter();
      review.ParameterName = "@review";
      review.Value = this._review;
      cmd.Parameters.Add(review);
      MySqlParameter restaurantId = new MySqlParameter();
      restaurantId.ParameterName = "@restaurant_id";
      restaurantId.Value = this._restaurantId;
      cmd.Parameters.Add(restaurantId);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }





  }
}
