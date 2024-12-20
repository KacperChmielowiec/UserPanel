using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using UserPanel.Models.Password;
using UserPanel.Models.User;

namespace UserPanel.Models.Dashboard
{
    public class CreateUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public bool IsActive { get; set; } = false;

        public bool IsToken { get; set; }
        public int? Token { get; set; }
        
    }
}
