namespace UserPanel.Models.Group
{
    public class GroupStat
    {
        public GroupStat(Guid id_camp, Guid id_group, UnitData<int>[] visit, UnitData<int>[] clicks, UnitData<decimal>[] budged)
        {
            Id_Camp = id_camp;
            Id_Group = id_group;
            Visit = visit;
            Clicks = clicks;
            Budged = budged;
        }

        public Guid Id_Camp { get; set; }

        public Guid Id_Group { get; set; }
        public UnitData<int>[] Visit { get; set; }

        public UnitData<int>[] Clicks { get; set; }

        public UnitData<decimal>[] Budged { get; set; }

    }
}
