using Newtonsoft.Json;

namespace UserPanel.Models.Group
{
    public class GroupListMock
    {
        public int id_user {  get; set; }
        public Guid id_camp { get; set; }
        public Guid id_group { get; set; }
        public Guid id { get; set; }
        [JsonProperty("status")]
        public ListStatus Status { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public ListType Type { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("bid_rate")]
        public double BidRate { get; set; }
        [JsonProperty("cap")]
        public int Cappping { get; set; }
        public int start { get; set; }
        public int end { get; set; }
    }
}
