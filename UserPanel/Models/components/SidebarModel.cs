using UserPanel.Models.Camp;
using UserPanel.References;
namespace UserPanel.Models.components
{
    public class SidebarModel
    {
        public List<Campaning> campaningList {  get; set; }
        public Guid activeCamp {  get; set; } = Guid.Empty;
       
        public List<PageTypes> pageTypes { get; set; }
    }
}
