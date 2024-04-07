namespace UserPanel.Models.Camp
{
    public class CampaningStat
    {
        public CampaningStat(Guid id_camp, UnitData<int>[] visit, UnitData<int>[] clicks, UnitData<decimal>[] budget, string name)
        {
            this.Id_Camp = id_camp;
            this.Visit = visit;
            this.Clicks = clicks;
            this.Budget = budget;
            Name = name;
        }

        public string Name { get; set; }
        public Guid Id_Camp { get; set; }

        public UnitData<int>[] Visit { get; set; }

        public UnitData<int>[] Clicks { get; set; }

        public UnitData<decimal>[] Budget { get; set; }
    }
}
