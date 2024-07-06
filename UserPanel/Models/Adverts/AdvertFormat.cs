using Newtonsoft.Json;

namespace UserPanel.Models.Adverts
{
    public class AdvertFormat
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }
    }
}
