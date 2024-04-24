using Newtonsoft.Json;

namespace UserPanel.Models.Group
{
    public class EditGroup
    {
        public Guid? id { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
        public string? description { get; set; } = "";
        public decimal? totalBudget { get; set; }
        public decimal? dayBudget { get; set; }
        public BillingModel Billing { get; set; }
        public DevicesModel[] Devices { get; set; }
        public string? startTime { get; set; }
        public string? endTime { get; set; }
        public string? Utm_Source { get; set; }
        public string? Utm_Medium { get; set; }
        public string? Utm_Camp { get; set; }
    }
}
