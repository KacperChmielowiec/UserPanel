using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using UserPanel.Attributes;
namespace UserPanel.Models.Camp
{
    public class CreateCampaning
    {
        [Required]
        [RegularExpression(@"^[A-Za-z0-9]{4,25}$", ErrorMessage = "Niepoprawny format")]
        public string name { get; set; }

        [DataType(DataType.Upload)]
        [FileTypeValidation(".jpg", ".png", ErrorMessage = "Dozwolone są tylko pliki typu: JPG lub PNG.")]
        public IFormFile logo {  get; set; }

        [Required]
        [RegularExpression(@"^(http://|https://)[A-Za-z0-9]{3,15}(.pl|.com])", ErrorMessage = "Niepoprawny format")]
        public string url { get; set; }

        [Required]
        public string currency {  get; set; }

        [Required]
        public string country { get; set; }
    }
}
