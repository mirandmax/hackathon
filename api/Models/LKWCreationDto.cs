using System.ComponentModel.DataAnnotations;

namespace api.Models {

    public class LKWCreationDto{

        [Required]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public string CompanyMail { get; set; } = string.Empty;

        [Required]
        public string CompanyPhone { get; set; } = string.Empty;

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;
    }
}