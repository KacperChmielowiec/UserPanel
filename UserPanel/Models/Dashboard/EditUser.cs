using System.ComponentModel.DataAnnotations;

namespace UserPanel.Models.Dashboard
{
    public class EditUser
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? Password { get; set; }
        [Required]
        public UserRole Role { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public bool IsToken { get; set; }
        public int? Token { get; set; }
    }
}
