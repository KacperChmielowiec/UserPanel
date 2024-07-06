using UserPanel.Models.Adverts;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
namespace UserPanel.Models
{
    public class FullUserContext : FullContext
    {
        public int UserId { get; set; }

        public List<Campaning> Campanings { get; set; }
        public List<GroupModel> Groups { get; set; }

        public List<Advert> Adverts { get; set; }
    }
}
