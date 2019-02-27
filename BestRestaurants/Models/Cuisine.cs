using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace BestRestaurants.Models
{
  public class Cuisine
  {
    private string _type;
    private int _id;

    public Cuisine(string CuisineType, int id = 0)
    {
      _type = CuisineType;
      _id = id;
    }

    public string GetType{return _type;}
    public string GetId{return _id;}

    public static void ClearAll()
    {
      MySqlConnection conn = BD.Connection();
      conn.Open;
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE  FROM cuisines;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Cuisine> GetAll()
    {
      List<Cuisine> allCuisine = new List<Cuisine>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        string CuisineType = rdr.GetString(0);
        int CuisineId = rdr.GetInt32(1);
        Cuisine newCuisine = new Cuisine(CuisineType, CuisineId);
        allCuisine.Add(newCuisine);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCuisine;
    }

    public static Cuisine Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cuisines WHERE id = (@serchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int CuisineId = 0;
      string CuisineType = "";
      while(rdr.Read())
      {
        CuisineType = rdr.GetString(0);
        CuisineId = rdr.GetInt32(1);
      }
      Cuisine newCuisine = new Cuisine(CuisineType, CuisineId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newCuisine;
    }

    public List<Restaurant> GetRestaurant()
    {
      List<Restaurant> allCuisineRestaurants = new List<Restaurant>{};
      MySqlConnection conn = DB.Connection();
     conn.Open();
     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = "SELECT * FROM restaurants WHERE cuisine_id = @cuisine_id;";
     MySqlParameter cuisineId = new MySqlParameter();
      cuisineId.ParameterName = "@cuisine_id";
      cuisineId.Value = this._id;
      cmd.Parameters.Add(cuisineId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int restaurantId = rdr.GetInt32(1);
        string restaurantName = rdr.GetString(0);
        int cuisineId = rdr.GetInt32(2);
        Restaurant newRestaurant= new Restaurant(restaurantName, cuisineId, restaurantId);
        allCuisineRestaurants.Add(newRestaurant);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCuisineRestaurants;
    }

    public override bool Equals(System.Object otherCuisine)
    {
      if (!(otherCuisine is Cuisine))
      {
        return false;
      }
      else
      {
        Cuisine newCuisine = (Cuisine) otherCuisine;
        bool idEquality = this.GetId().Equals(newCuisine.GetId());
        bool typeEquality = this.GetType().Equals(newCuisine.GetName());
        return (idEquality && typeEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cuisines (type) VALUES (@type)";
      MySqlParameter type = new MySqlParameter();
      type.ParameterName = "@type";
      type.Value = this._type;
      cmd.Parameters.Add(type);
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
