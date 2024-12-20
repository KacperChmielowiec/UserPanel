using System.ComponentModel.DataAnnotations;
using UserPanel.Models.User;
using static UserPanel.Models.Password.PasswordConstraintMetadataAttribute;
namespace UserPanel.Models.Password
{


    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class PasswordConstraintMetadataAttribute : Attribute
    {

        public string[] InputNames { get; }
        public string[] InputTypes { get; }

        public string[] InputLabels { get; }
        public int InputCount { get; }
        public PasswordConstraintMetadataAttribute(int inputCount, string[] names, string[] types, string[] inputLabels)
        {
            InputCount = inputCount;
            InputNames = names;
            InputTypes = types;
            InputLabels = inputLabels;

        }
    }

    public static class EnumExtensions
    {
        // Funkcja rozszerzeniowa dla typu Enum, aby uzyskać atrybut PasswordConstraintMetadataAttribute
        public static PasswordConstraintMetadataAttribute GetPasswordConstraintMetadata(this Enum enumValue)
        {
            // Pobieranie typu enuma
            var type = enumValue.GetType();

            // Pobieranie informacji o konkretnym elemencie enuma (np. LengthBetween)
            var memberInfo = type.GetMember(enumValue.ToString()).FirstOrDefault();

            // Pobieranie atrybutu przypisanego do elementu enuma
            var attribute = memberInfo?.GetCustomAttributes(typeof(PasswordConstraintMetadataAttribute), false)
                                       .FirstOrDefault() as PasswordConstraintMetadataAttribute;

            return attribute;
        }
    }


    public enum PasswordConstraintType
    {
        [PasswordConstraintMetadata(2,new string[] {"MinLength", "MaxLength"}, new string[] {"text","text"}, new string[] { "Min.", "Max." })]
        LengthBetween = 1,
        [PasswordConstraintMetadata(1, new string[] { "UpperCases" }, new string[] { "checkbox" }, new string[] { "Must Upper Case."})]
        ContainsUppercaseLetter = 2,
        [PasswordConstraintMetadata(1, new string[] { "Digits" }, new string[] { "checkbox" }, new string[] { "Must Digits."})]
        ContainsDigitNotAtStart = 3,
        [PasswordConstraintMetadata(1, new string[] { "SpecialChar" }, new string[] { "checkbox" }, new string[] { "Must Special Char." })]
        ContainsSpecialCharacter = 4,
        [PasswordConstraintMetadata(1, new string[] { "NoRepeat" }, new string[] { "checkbox" }, new string[] { "Must Only Unique Chars." } )]
        NoRepeatingCharacters = 5,
        [PasswordConstraintMetadata(1, new string[] { "DaysLimit" }, new string[] { "text" }, new string[] { "Days Limit to Reset"})]
        DaysToPasswordReset = 6,
    }

    public class PasswordRules
    {
        [Key]
        public int Id { get; set; } 
        [Required]
        public PasswordConstraintType ConstraintType { get; set; } 
        public int Value { get; set; }
        public int MinValue { get; set; } 
        public int MaxValue { get; set; }
        public string Regex { get; set; } = "";
        public string Description { get; set; } = "";
        public string ErrorMessage { get; set; } = "";
        public bool IsEnabled { get; set; }
    }
}
