using Newtonsoft.Json;

namespace UserPanel.Models
{
    public class EndpointMetaData
    {
        public string name {  get; set; }
        public bool permission { get; set; }

        public string method { get; set; }
        [JsonProperty("delegate")]
        public string Delegate { get; set; }

        public string fullName { get { return $"{name}:{method}"; }}
    }
}
