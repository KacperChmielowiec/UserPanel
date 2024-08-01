using Newtonsoft.Json;

namespace UserPanel.Models.Adverts
{
    public class Advert<T>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime Created {  get; set; }
        public bool IsActive { get; set; }

        [JsonProperty("template")]
        public AD_TEMPLATE Template { get; set; }
        public List<T> Formats { get; set; }
    }
}
