using System.ComponentModel.DataAnnotations;
using UserPanel.Attributes;

namespace UserPanel.Models.Adverts
{
    public class AdvertFormatForm
    {
        public Guid id { get; set; }
        [Required]
        public bool isEdit { get; set; }

        [RegularExpression(@"^(http:\/\/|https:\/\/)[A-Za-z0-9]{3,15}(\.[A-Za-z0-9]{2,})*(\.pl|\.com)$", ErrorMessage = "Niepoprawny format")]
        [Required]
        public string Url { get; set; }
        [Required]
        public string Size { get; set; }

        public string? Src { get; set; } = "";
    }
}
