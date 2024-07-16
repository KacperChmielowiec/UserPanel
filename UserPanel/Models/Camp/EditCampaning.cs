using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using UserPanel.Attributes;

namespace UserPanel.Models.Camp
{
    public class EditCampaning
    {

        [Required]
        [RegularExpression(@"^[A-Za-z0-9]{4,25}$", ErrorMessage = "Niepoprawny format")]
        public string name {  get; set; }
        [Required]
        [RegularExpression(@"^((http://|https://)|www.)[A-Za-z0-9]{3,15}(.pl|.com])", ErrorMessage = "Niepoprawny format")]
        public string website { get; set; }
        [Required]
        public bool status { get; set; }
        

        [FileTypeValidation(".jpg", ".png", ErrorMessage = "Dozwolone są tylko pliki typu: JPG lub PNG.")]
        public IFormFile? logo { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string currency { get; set; }
        [Required]
        public bool notify { get; set; }
        public decimal totalBudget { get; set; }
        public decimal dayBudget { get; set; }
        public string? Utm_Source { get; set; } = "";
        public string? Utm_Medium { get; set; } = "";
    }
}
