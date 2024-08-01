using System.ComponentModel.DataAnnotations;

namespace UserPanel.Services
{
    public class ValidatorModel
    {
        public static List<ValidationResult> ValidateModel<T>(T model)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, context, validationResults, validateAllProperties: true);
            return validationResults;
        }

    }
}
