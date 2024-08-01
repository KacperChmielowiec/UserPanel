using System.ComponentModel.DataAnnotations;
using UserPanel.Attributes;

namespace UserPanel.Models.Adverts
{
    public class AdvertFormatFormStatic : AdvertFormatForm
    {
        [DataType(DataType.Upload)]
        [FileTypeValidationAdvertForm(requiredError: "Nie wybrano zdjecia dla tego formatu.", ".jpg", ".png", ErrorMessage = "Dozwolone są tylko pliki typu: JPG lub PNG.")]
        public IFormFile? StaticImg { get; set; }

    }
}
