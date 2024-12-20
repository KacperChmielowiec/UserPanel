using System.ComponentModel.DataAnnotations;

namespace UserPanel.Models.User
{

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; } = "/";
        public bool TokenLogin { get; set; }
    }


    public class ResetPasswordUserModel
    {
        
        public string token { get; set; }

        [Required]
        public int idUser { get; set; }

     /*   [Required]
        [RegularExpression(@"^[A-Z](?=.*[0-9])(?=.*[a-z])(?=.*[#$@!%&*?]).*$", ErrorMessage = "First letter must be uppercase and include special character")]*/
        public string password { get; set; }

     /*   [Required]
        [RegularExpression(@"^[A-Z](?=.*[0-9])(?=.*[a-z])(?=.*[#$@!%&*?]).*$", ErrorMessage = "First letter must be uppercase and include special character")]
        [Compare("password", ErrorMessage = "Passwords do not match.")]*/
        public string cpassword { get; set; }
    }
}
