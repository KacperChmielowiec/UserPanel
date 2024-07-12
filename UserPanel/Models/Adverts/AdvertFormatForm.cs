using System.ComponentModel.DataAnnotations;
using UserPanel.Attributes;

namespace UserPanel.Models.Adverts
{
    public class AdvertFormatForm
    {
        [RegularExpression(@"^(http://|https://)[A-Za-z0-9]{3,15}(.pl|.com])", ErrorMessage = "Niepoprawny format")]
        [Required]
        public string Url { get; set; }
        [Required]
        public string Size { get; set; }

        [DataType(DataType.Upload)]
        [FileTypeValidation(".jpg", ".png", ErrorMessage = "Dozwolone są tylko pliki typu: JPG lub PNG.")]
        public IFormFile StaticImg { get; set; }

        public string Src { get; set; } = "";

    }
}
