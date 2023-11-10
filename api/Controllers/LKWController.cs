using Microsoft.AspNetCore.Mvc;
using MySql.Data;


using api.Models;
using System.Text.Json;
using MySql.Data.MySqlClient;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LKWController : ControllerBase
    {
        private class LeaderboardRecord
        {
            public string Name { get; set; } = string.Empty;

            public int Credits { get; set; }
        }
        // GET api/LKW
        [HttpGet]
        public ActionResult<string> GetLeaderboard()
        {
            var result = new List<LeaderboardRecord>();
            using(var connection = new MySqlConnection("Server=localhost;Database=truckit;Uid=root;Pwd=;"))
            {
                connection.Open();

                using(var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT uname, ucredits FROM user ORDER BY ucredits DESC LIMIT 25";

                    using(var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            result.Add(new LeaderboardRecord
                            {
                                Name = reader.GetString(0),
                                Credits = reader.GetInt32(1)
                            });
                        }
                    }
                }
            }

            return JsonSerializer.Serialize(result);
        }

        // POST api/LKW
        [HttpPost]
        public ActionResult<CreationReward> Post(LKWCreationDto lkw)
        {
            CreationReward reward = new CreationReward(true, 10);

            using(var connection = new MySqlConnection("Server=localhost;Database=truckit;Uid=root;Pwd=;"))
            {
                connection.Open();

                using(var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT FROM ";
                    command.Parameters.AddWithValue("@name", lkw.CompanyName);
                    command.Parameters.AddWithValue("@credits", reward.Reward);

                    command.ExecuteNonQuery();
                }
            }

            using(var connection = new MySqlConnection("Server=localhost;Database=truckit;Uid=root;Pwd=;"))
            {
                connection.Open();

                using(var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO user (name, credits) VALUES (@name, @credits)";
                    command.Parameters.AddWithValue("@name", lkw.CompanyName);
                    command.Parameters.AddWithValue("@credits", reward.Reward);

                    command.ExecuteNonQuery();
                }
            }

            return Ok(reward);
        }

    }
}