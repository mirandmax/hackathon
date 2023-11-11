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
            CreationReward reward = new CreationReward(false, 1);

            using(var connection = new MySqlConnection("Server=localhost;Database=truckit;Uid=root;Pwd=;"))
            {
                connection.Open();

                using(var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM trucklocations WHERE tid = (SELECT tid FROM trucks WHERE tplate = @plate) AND tdate = CURDATE()";
                    command.Parameters.AddWithValue("@plate", lkw.LicensePlate);

                    using(var reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return new CreationReward(false, 5);
                        }
                    }
                }

                using(var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM trucks WHERE tplate = @plate";
                    command.Parameters.AddWithValue("@plate", lkw.LicensePlate);

                    using(var reader = command.ExecuteReader())
                    {
                        if(!reader.Read())
                        {
                            using (var command2 = connection.CreateCommand())
                            {
                                command2.CommandText = "SELECT cid FROM companies WHERE cemail = @mail";
                                command2.Parameters.AddWithValue("@mail", lkw.CompanyMail);
                                reader.Close();
                                command2.ExecuteNonQuery();

                                using (var reader2 = command2.ExecuteReader())
                                {
                                    if (reader2.Read())
                                    {
                                        using (var command3 = connection.CreateCommand())
                                        {
                                            command3.CommandText = "INSERT INTO trucks (tplate, cid) VALUES (@plate, @cid)";
                                            command3.Parameters.AddWithValue("@plate", lkw.LicensePlate);
                                            command3.Parameters.AddWithValue("@cid", reader2.GetInt32(0));
                                            reader2.Close();
                                            command3.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        reader2.Close();
                                        using (var command3 = connection.CreateCommand())
                                        {
                                            command3.CommandText = "INSERT INTO companies (cemail, cphone) VALUES (@mail, @phone)";
                                            command3.Parameters.AddWithValue("@mail", lkw.CompanyMail);
                                            command3.Parameters.AddWithValue("@phone", lkw.CompanyPhone);

                                            command3.ExecuteNonQuery();
                                        }

                                        using (var command3 = connection.CreateCommand())
                                        {
                                            command3.CommandText = "SELECT cid FROM companies WHERE cemail = @mail";
                                            command3.Parameters.AddWithValue("@mail", lkw.CompanyMail);

                                            command3.ExecuteNonQuery();

                                            using (var reader3 = command3.ExecuteReader())
                                            {
                                                if (reader3.Read())
                                                {
                                                    using (var command4 = connection.CreateCommand())
                                                    {
                                                        command4.CommandText = "INSERT INTO trucks (tplate, cid) VALUES (@plate, @cid)";
                                                        command4.Parameters.AddWithValue("@plate", lkw.LicensePlate);
                                                        command4.Parameters.AddWithValue("@cid", reader3.GetInt32(0));
                                                        reader3.Close();
                                                        command4.ExecuteNonQuery();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                using(var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT tid FROM trucks WHERE tplate = @plate";
                    command.Parameters.AddWithValue("@plate", lkw.LicensePlate); 

                    using(var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        var tid = reader.GetInt32(0);
                        reader.Close();

                        using(var command1 = connection.CreateCommand())
                        {
                            command1.CommandText = "SELECT uid FROM user WHERE uname = @name";
                            command1.Parameters.AddWithValue("@name", lkw.UserName);

                            using(var reader1 = command1.ExecuteReader())
                            {
                                if(reader1.Read())
                                {
                                    using(var command2 = connection.CreateCommand())
                                    {
                                        command2.CommandText = "INSERT INTO trucklocations (tid, tdate, tlat, tlon, uid) VALUES (@tid, CURDATE(), @tlat, @tlon, @uid)";
                                        command2.Parameters.AddWithValue("@tid", tid);
                                        command2.Parameters.AddWithValue("@tlat", lkw.Latitude);
                                        command2.Parameters.AddWithValue("@tlon", lkw.Longitude);
                                        var temp = reader1.GetInt32(0);
                                        command2.Parameters.AddWithValue("@uid", temp);
                                        reader1.Close();
                                        command2.ExecuteNonQuery();

                                        reward = new CreationReward(true, 10);
                                    }

                                } 
                                
                            }
                        }

                    }

                }
       
            }

            return Ok(reward);
        }

    }
}