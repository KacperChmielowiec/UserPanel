namespace UserPanel.Models.Camp
{
    public class CampaningStat
    {
        public CampaningStat(Guid id_camp, int[] visit, int[] clicks, decimal[] budged, string name)
        {
            this.Id_Camp = id_camp;
            this.Visit = visit;
            this.Clicks = clicks;
            this.Budged = budged;
            Name = name;
        }

        public string Name { get; set; }
        public Guid Id_Camp { get; set; }

        public int[] Visit { get; set; }

        public int[] Clicks { get; set; }

        public decimal[] Budged { get; set; }
    }
}
