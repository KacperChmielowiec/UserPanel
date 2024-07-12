using System.Security.Policy;

namespace UserPanel.Models.User
{
    public class FinishPageModel
    {
        public string Email { get; set; }
        public string HashLink { get; set; }

        public bool Development { get; set; }
    }
}
