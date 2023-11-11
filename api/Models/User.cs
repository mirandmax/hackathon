using System.ComponentModel.DataAnnotations;

namespace api.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

    }
}