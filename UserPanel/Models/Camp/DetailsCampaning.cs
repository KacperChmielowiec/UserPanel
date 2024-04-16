using Newtonsoft.Json;
using System.Text.Json;

namespace UserPanel.Models.Camp
{
    public class DetailsCampaning
    {
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("emailNotify")]
        public bool EmailNotify { get; set; }
        [JsonProperty("utm_source")]
        public string Utm_Source { get; set; }
        [JsonProperty("utm_medium")]
        public string Utm_Medium { get; set; }
        [JsonProperty("flags")]
        public CampaningFlags CampaningFlags { get; set; }

        public string logo { get; set; }
    }
}
