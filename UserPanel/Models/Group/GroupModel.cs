using Newtonsoft.Json;
using UserPanel.Models.Camp;

namespace UserPanel.Models.Group
{
    public class GroupModel
    {
        public Guid id { get; set; }
        [JsonProperty("id_camp")]
        public Guid FK_Camp { get; set; }
        public string name { get; set; }
        public BudgetCampaning budget { get; set; }
        public GroupDetails details { get; set; }
        public bool status { get; set; }
        [JsonProperty("advertisements")]
        public GroupAdvertisements advertisements { get; set; }
        [JsonProperty("lists")]
        public GroupLists[] Lists { get; set; }
    }
}
