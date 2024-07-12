using Newtonsoft.Json;
using UserPanel.References;
using static UserPanel.References.AppReferences;
namespace UserPanel.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        [JsonProperty("State")]
        public bool IsActive { get; set; }
    }
}
