namespace UserPanel.Models.Camp
{
    public class Campaning
    {
        public Guid id { get; set; }
        public int FK_User { get; set; }
        public string name { get; set; }
        public string website { get; set; }
        public DetailsCampaning details { get; set; }
        public BudgetCampaning budget {  get; set; }
        public bool status { get; set; }
    }
}
