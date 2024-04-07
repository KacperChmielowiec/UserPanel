namespace UserPanel.Models.Camp
{
    public class GroupStat
    {
        public GroupStat(Guid id_camp,Guid id_group, UnitData<int>[] visit, UnitData<int>[] clicks, UnitData<decimal>[] budged) {
            this.Id_Camp = id_camp;
            this.Id_Group = id_group;
            this.Visit = visit;
            this.Clicks = clicks;
            this.Budged = budged;

        }

        public Guid Id_Camp { get; set; }

        public Guid Id_Group{ get; set; }
        public UnitData<int>[] Visit { get; set; }

        public UnitData<int>[] Clicks { get; set; }

        public UnitData<decimal>[] Budged { get; set; }

    }
}
