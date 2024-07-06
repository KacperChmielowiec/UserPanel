using Newtonsoft.Json;
using UserPanel.Models.Camp;
using UserPanel.Models.Adverts;
namespace UserPanel.Models.Group
{
    public class GroupModel
    {
        public Guid id { get; set; }
        public Guid Parent {  get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public BudgetCampaning budget { get; set; }
        public GroupDetails details { get; set; }
        public bool status { get; set; }
        [JsonProperty("advertisements")]
        public Advert[] advertisementsList { get; set; }
        [JsonProperty("lists")]
        public GroupLists[] Lists { get; set; }
    }
}
