using Microsoft.AspNetCore.Mvc;
using MySql.Data;


using api.Models;
using System.Text.Json;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Prng;


namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        // POST api/LKW
        [HttpPost]
        public ActionResult<CreationReward> CreateUser(User user)
        {
            using(var connection = new MySqlConnection("Server=localhost;Database=truckit;Uid=root;Pwd=;")){
                connection.Open();
                using(var command = connection.CreateCommand()){
                    command.CommandText = "INSERT INTO user (uname, passwd) VALUES (@uname, @passwd)";
                    command.Parameters.AddWithValue("@uname", user.Name);
                    command.Parameters.AddWithValue("@passwd", user.Password);
                    try{
                        using(var reader = command.ExecuteReader())
                    {
                            Console.WriteLine(reader.Read());
                            return new CreationReward(true, 0);   
                    }
                    } catch (MySqlException e){
                        Console.WriteLine(e.Message);
                        return new CreationReward(false, 0);
                    }
                }
            }
        }
        
    }
}