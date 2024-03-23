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
        [ConfigurationKeyName("TypeAccess")]
        public string TypeAccess { get; set; }
        [ConfigurationKeyName("Auth")]
        public bool Auth{ get; set; }
        [ConfigurationKeyName("Roles")]
        public UserRole[] Roles { get; set; }

        [ConfigurationKeyName("Pages")]
        public string[] Pages { get; set; }
    }
}
