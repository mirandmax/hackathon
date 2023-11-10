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
        public double Longitude { get; set; }

        [Required]
        public double Latitude { get; set; }
    }
}