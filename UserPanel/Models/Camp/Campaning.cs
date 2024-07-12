using UserPanel.Models.Group;

namespace UserPanel.Models.Camp
{
    public class Campaning
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string website { get; set; }
        public List<GroupModel>? groups { get; set; }
        public DetailsCampaning? details { get; set; }
        public BudgetCampaning? budget {  get; set; }
        public bool status { get; set; }
    }
}
