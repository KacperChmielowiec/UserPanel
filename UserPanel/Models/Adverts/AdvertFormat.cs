using Newtonsoft.Json;

namespace UserPanel.Models.Adverts
{
    public class AdvertFormat
    {
        public Guid Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("src")]
        public string Src { get; set; }
    }
}
