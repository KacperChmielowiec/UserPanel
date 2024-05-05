using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace UserPanel.Models.Group
{
    public class CreateGroup
    {
        [Required]
        public Guid id_camp { get; set; }
        [Required]
        public string name { get; set; }
        public string? description { get; set; }
        public decimal? totalBudget { get; set; }
        public decimal? dayBudget { get; set; }
        [Required]
        public BillingModel Billing { get; set; }
        [Required]
        public DevicesModel[] Devices { get; set; }
        public string? startTime { get; set; }
        public string? endTime { get; set; }
        [Required]
        public string Utm_Source { get; set; }
        public string? Utm_Medium { get; set; }
        public string? Utm_Camp { get; set; }
    }
}
