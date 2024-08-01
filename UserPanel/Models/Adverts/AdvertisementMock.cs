using Newtonsoft.Json;
using UserPanel.Models.Group;

namespace UserPanel.Models.Adverts
{
    public class AdvertisementMock
    {
        public int id_user { get; set; }
        public Guid id_camp { get; set; }
        public Guid[] id_groups { get; set; }
        public Guid id { get; set; }
        public string name { get; set; }

        [JsonProperty("created")]
        public DateTime Created { get; set; }
        public AD_TEMPLATE template { get; set; }
        public List<AdvertFormat> formats { get; set; }
        [JsonProperty("status")]
        public bool IsActive { get; set; }
        public string Description {  get; set; }
    }
}
