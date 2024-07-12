using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace UserPanel.Models
{
    public class EnviromentSettings
    {
        [ConfigurationKeyName("UserRepositoryType")]
        public string EnvMock {  get; set; }
        [ConfigurationKeyName("DevelopmentEmail")]
        public bool EnvDevEmail { get; set; }
    }
}
