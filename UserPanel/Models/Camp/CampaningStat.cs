namespace UserPanel.Models.Camp
{
    public class CampaningStat
    {
        public CampaningStat(Guid id_camp, int[] visit, int[] clicks, decimal[] budged)
        {
            this.Id_Camp = id_camp;
            this.Visit = visit;
            this.Clicks = clicks;
            this.Budged = budged;
        }

        public Guid Id_Camp { get; set; }

        public int[] Visit { get; set; }

        public int[] Clicks { get; set; }

        public decimal[] Budged { get; set; }
    }
}
