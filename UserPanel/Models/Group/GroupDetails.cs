using UserPanel.Models.Camp;
using Newtonsoft.Json;
namespace UserPanel.Models.Group
{
    public class GroupDetails
    {
        [JsonProperty("billing")]
        public BillingModel Billing { get; set; }
        [JsonProperty("devices")]
        public DevicesModel[] Devices { get; set; }

        public string startTime { get; set; }
        public string endTime { get; set; }

        [JsonProperty("utm_source")]
        public string Utm_Source { get; set; }
        [JsonProperty("utm_medium")]
        public string Utm_Medium { get; set; }
        [JsonProperty("utm_camp")]
        public string Utm_Camp { get; set;}
        [JsonProperty("flags")]
        public CampaningFlags CampaningFlags { get; set; }
    }
}
