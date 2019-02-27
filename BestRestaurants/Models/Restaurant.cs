using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BestRestaurants.Models
{
  public class Restaurant
  {
    private string _name;
    private int _id;
    private int _cuisineId;

    public Restaurant(string restaurantName, int cuisineId, int id = 0)
    {
      _name = restaurantName;
      _id = id;
      _cuisineId = cuisineId;
    }

    public string GetName(){return _name;}
    public void SetName(string name){ _name = name;}
    public int GetId(){return _id;}
    public int GetCuisineId(){return _cuisineId;}

    public static List<Restaurant> GetAll()
    {
      List<Restaurant> allRestaurants = new List<Restaurant>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurants;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
       int restaurantId = rdr.GetInt32(1);
       string restaurantName = rdr.GetString(0);
       int restaurantCuisineId = rdr.GetInt32(2);
       Restaurant newRestaurant = new Restaurant(restaurantName, restaurantCuisineId, restaurantId);
       allRestaurants.Add(newRestaurant);
      }
      conn.Close();
      if (conn != null)
      {
       conn.Dispose();
      }
      return allRestaurants;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM restaurants;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
       conn.Dispose();
      }
    }

    public static Restaurant Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM restaurants WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int restaurantId = 0;
      string restaurantName = "";
      int restaurantCuisineId = 0;
      while(rdr.Read())
      {
        restaurantId = rdr.GetInt32(1);
        restaurantName = rdr.GetString(0);
        restaurantCuisineId = rdr.GetInt32(2);
      }
      Restaurant newRestaurant = new Restaurant(restaurantName, restaurantCuisineId, restaurantId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newRestaurant;
    }

    public override bool Equals(System.Object otherRestaurant)
    {
      if (!(otherRestaurant is Restaurant))
      {
        return false;
      }
      else
      {
         Restaurant newRestaurant = (Restaurant) otherRestaurant;
         bool idEquality = this.GetId() == newRestaurant.GetId();
         bool nameEquality = this.GetName() == newRestaurant.GetName();
         bool cuisineEquality = this.GetCuisineId() == newRestaurant.GetCuisineId();
         return (idEquality && nameEquality && cuisineEquality);
       }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO restaurants (name, cuisine_id) VALUES (@name, @cuisine_id);";
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);
      MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@cuisine_id";
      cuisineId.Value = this._cuisineId;
      cmd.Parameters.Add(cuisineId);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Edit(string newName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE restaurants SET name = @newName WHERE id = @searchId;";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@newName";
      name.Value = newName;
      cmd.Parameters.Add(name);
      cmd.ExecuteNonQuery();
      _name = newName;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

  }
}
