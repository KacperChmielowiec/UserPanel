using System.ComponentModel.DataAnnotations;
using UserPanel.Models.Adverts;

namespace UserPanel.Attributes
{
    public class FileTypeValidationAdvertFormAttribute : ValidationAttribute
    {
        private readonly string[] _validTypes;
        private string _requiredError;
        public FileTypeValidationAdvertFormAttribute(string requiredError,params string[] validTypes)
        {
            _validTypes = validTypes;
            _requiredError = requiredError;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(!(validationContext.ObjectInstance is AdvertFormatForm))
            {
                return ValidationResult.Success;
            }

            AdvertFormatForm advertFormatForm = (AdvertFormatForm)validationContext.ObjectInstance;
            if(advertFormatForm.isEdit)
            {
                if(value == null)
                {
                    return ValidationResult.Success;
                }
            }
            if(!advertFormatForm.isEdit && value == null)
            {
                return new ValidationResult(_requiredError);
            }

            return new FileTypeValidationAttribute(_validTypes).IsValid(value) ? ValidationResult.Success : new ValidationResult($"Dozwolone są tylko pliki typu: {string.Join(", ", _validTypes)}.");
        }
    }
}

