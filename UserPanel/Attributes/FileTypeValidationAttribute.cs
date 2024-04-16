namespace UserPanel.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using System.IO;

    public class FileTypeValidationAttribute : ValidationAttribute
    {
        private readonly string[] _validTypes;

        public FileTypeValidationAttribute(params string[] validTypes)
        {
            _validTypes = validTypes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var file = value as IFormFile;
            if (file == null)
                return new ValidationResult("Nieprawidłowy typ pliku.");

            var extension = Path.GetExtension(file.FileName);
            if (!_validTypes.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                return new ValidationResult($"Dozwolone są tylko pliki typu: {string.Join(", ", _validTypes)}.");
            }

            return ValidationResult.Success;
        }
    }
}
