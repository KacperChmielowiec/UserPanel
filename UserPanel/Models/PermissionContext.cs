using UserPanel.Models.Camp;

namespace UserPanel.Models
{
    public class PermissionContext
    {
        public int contextUserID { get; set; }
        public bool IsLogin { get; set; }

        public bool IsLoad { get; set; }
        public List<CampContext> CampsContext { get; set; }
        public List<GroupContext> GroupsContext { get; set; }
    }
}
