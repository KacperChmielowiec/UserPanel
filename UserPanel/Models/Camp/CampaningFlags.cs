using Newtonsoft.Json;

namespace UserPanel.Models.Camp
{
    public class CampaningFlags
    {
        [JsonProperty("budget")]
        public CampaningFlagState Budget {  get; set; }
        [JsonProperty("products")]
        public CampaningFlagState Products { get; set; }
        [JsonProperty("lists")]
        public CampaningFlagState Lists { get; set; }
        [JsonProperty("advert")]
        public CampaningFlagState Advert { get; set; }
        [JsonProperty("display")]
        public CampaningFlagState Display { get; set; }
    }
}
