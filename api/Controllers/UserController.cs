using Microsoft.AspNetCore.Mvc;
using MySql.Data;
using System.Security.Cryptography;
using System;
using System.Text;
using api.Models;
using System.Text.Json;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Prng;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Collections;


namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        // POST api/LKW
        [HttpPost("signup")]
        public ActionResult<CreationReward> CreateUser(User user)
        {
            user.Password = ComputeSha256Hash(user.Password);
            using(var connection = new MySqlConnection("Server=localhost;Database=truckit;Uid=root;Pwd=;")){
                connection.Open();
                using(var command = connection.CreateCommand()){
                    command.CommandText = "INSERT INTO user (uname, passwd) VALUES (@uname, @passwd)";
                    command.Parameters.AddWithValue("@uname", user.Name);
                    command.Parameters.AddWithValue("@passwd", user.Password);
                    try{
                        using(var reader = command.ExecuteReader())
                    {
                            return Ok();   
                    }
                    } catch (MySqlException e){
                        Console.WriteLine(e.Message);
                        return new CreationReward(false, 0);
                    }
                }
            }
        }

        [HttpPost("login")]
        public ActionResult<CreationReward> LoginUser(User user)
        {
            user.Password = ComputeSha256Hash(user.Password);
            using(var connection = new MySqlConnection("Server=localhost;Database=truckit;Uid=root;Pwd=;")){
                connection.Open();
                using(var command = connection.CreateCommand()){
                    command.CommandText = "SELECT * FROM user WHERE uname = @uname AND passwd = @passwd";
                    command.Parameters.AddWithValue("@uname", user.Name);
                    command.Parameters.AddWithValue("@passwd", user.Password);
                    try{
                        using(var reader = command.ExecuteReader())
                    {
                            if (reader.Read()){
                                return Ok();
                            }
                            return Unauthorized();   
                    }
                    } catch (MySqlException e){
                        Console.WriteLine(e.Message);
                        return Unauthorized();
                    }
                }
            }
        }
    static string ComputeSha256Hash(string rawData)
    {
        // Create a SHA256   
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
            builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
    static string extractPhoneNumber(string phone){
        const string MatchPhonePattern = ".*[^0-9-( )] ([0-9-( )]+)$";
        Regex rx = new Regex(MatchPhonePattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        MatchCollection matches = rx.Matches(phone);
        return matches[0].Value.ToString();
    }
    [HttpPost("isNear")]
    public Boolean isNear(double tlat1, double tlon1, double tlat2, double tlon2){
        if(Math.Abs(tlat1-tlat2) < 0.01 && Math.Abs(tlon1-tlon2) < 0.01){
            return true;
        }
        return false;
    }

    [HttpPost("getNearLocations")]
    public ActionResult<string> getNearLocations(int tlat1, int tlon1){
        var result = new Hashtable();
        using(var connection = new MySqlConnection("Server=localhost;Database=truckit;Uid=root;Pwd=;")){
                connection.Open();
                using(var command = connection.CreateCommand()){
                    command.CommandText = "SELECT user.cid, COUNT(*) FROM trucklocations,  user WHERE tlat BETWEEN @tlat1-0.1 AND @tlat1+0.1 AND tlon BETWEEN @tlon1-0.1 AND @tlon1+0.1 AND tdate > SUBDATE(now(), 30) AND trucklocations.uid = user.uid GROUP BY user.cid ORDER BY COUNT(*) DESC" ;
                    command.Parameters.AddWithValue("@tlat1", tlat1);
                    command.Parameters.AddWithValue("@tlon1", tlon1);
                    using(var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Console.WriteLine(reader.GetString(0));
                            //result.Add(reader.GetString(0),reader.GetInt32(1));
                        }
                    }
                   
                }
    }
    Console.WriteLine(JsonSerializer.Serialize(result));
    return JsonSerializer.Serialize(result);
    }


    
    }
    
}