using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Text.Json.Serialization;

namespace UserPanel.Models
{
    public class ComponentsDescriptor
    {
        [BindNever]
        public string name { get; set;}
        [ConfigurationKeyName("auth")]
        public bool isAuth{ get; set; }
        [ConfigurationKeyName("roles")]
        public UserRole[] Roles { get; set; }

        [ConfigurationKeyName("routesForbidden")]
        public string[] forbiddenPath { get; set; }
    }
}
