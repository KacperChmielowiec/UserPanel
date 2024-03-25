namespace UserPanel.Models.Campaning
{
    public class CampaningFilterModel
    {
        public Campaning Campaning { get; set; }
        public bool Selected { get; set; }
        public CampaningFilterModel(Campaning campaning, bool selected = false) {
            Campaning = campaning;
            Selected = selected;
        }
    }
}
