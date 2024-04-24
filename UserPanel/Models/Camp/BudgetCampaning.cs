namespace UserPanel.Models.Camp
{
    public class BudgetCampaning
    {
        public Guid id { get; set; }

        public decimal totalBudget { get; set; }
        public decimal totalBudgetLeft { get; set; }
        public decimal dayBudget { get; set; }
        public decimal dayBudgetLeft { get; set; }
    }
}
