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
    }
}
