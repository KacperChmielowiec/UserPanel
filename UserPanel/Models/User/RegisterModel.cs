using System.ComponentModel.DataAnnotations;

namespace UserPanel.Models.User
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Length of chars must be between 3 - 12")]
        [RegularExpression(@"^[A-Z](?=.*[0-9])(?=.*[a-z])(?=.*[#$@!%&*?]).*$", ErrorMessage = "First letter must be uppercase and include special character")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Length of chars must be between 3 - 12")]
        [RegularExpression(@"^(http:\/\/|https:\/\/)[A-Za-z0-9]{3,15}(\.[A-Za-z0-9]{2,})(\.pl|\.com)$", ErrorMessage = "First letter must be uppercase and include special character")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string CPassword { get; set;}

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^[\w]{4,14}$",ErrorMessage = "Name have to include between 4 to 14 chars (not allowed special chars)")]
        public string Name { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
