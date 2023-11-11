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
    }
}