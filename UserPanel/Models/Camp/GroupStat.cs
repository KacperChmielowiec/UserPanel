namespace UserPanel.Models.Camp
{
    public class GroupStat
    {
        public GroupStat(Guid id_camp,Guid id_group, int[] visit, int[] clicks, decimal[] budged) {
            this.Id_Camp = id_camp;
            this.Id_Group = id_group;
            this.Visit = visit;
            this.Clicks = clicks;
            this.Budged = budged;

        }

        public Guid Id_Camp { get; set; }

        public Guid Id_Group{ get; set; }
        public int[] Visit { get; set; }

        public int[] Clicks { get; set; }

        public decimal[] Budged{ get; set; }

    }
}
