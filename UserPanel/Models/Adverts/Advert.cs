using Newtonsoft.Json;

namespace UserPanel.Models.Adverts
{
    public class Advert
    {
        public Guid Parent {  get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        [JsonProperty("template")]
        public AD_TEMPLATE Template { get; set; }
        public List<AdvertFormat> Formats { get; set; }
    }
}
