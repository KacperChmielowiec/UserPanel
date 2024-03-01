using Newtonsoft.Json;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using UserPanel.Helpers;
using UserPanel.Models;
using UserPanel.References;

namespace UserPanel.Services
{
    public static class ConfigManager
    {
        public static ConfigurationCore Core = new ConfigurationCore();
        private static string BASE_PATH = AppReferences.BASE_APP_PATH;
        private static string CONFIG_PATH = AppReferences.CONFIG_APP_PATH;
        public static void LoadConfig()
        {
            Core.Configuration = new Dictionary<string, object>();
            var files = Directory.GetFiles(BASE_PATH + "configs");
            StringFileService stringFileService = new StringFileService();
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    string name = Path.GetFileName(file);
                    string test = stringFileService.ReadFile("configs\\" + name);
                    JsonDocument jsonDocument = JsonDocument.Parse(stringFileService.ReadFile("configs\\" + name));
                    Core.Configuration[name.Split(".")[0]+"Config"] = ToDynamic(jsonDocument.RootElement);
                    
                }
            }
        }
        public static T Parse<T>(this object obj) where T : new()
        {
            string json = JsonConvert.SerializeObject(obj);
            return JsonConvert.DeserializeObject<T>(json);
        }
        
    
        public static object GetConfig(string path)
        {
            var pathArray = path.Split(".");
            Dictionary<string, object> dict = Core.Configuration;
            foreach(var item in pathArray)
            {
                bool result = dict.Keys.Contains(item);
                if (result)
                {
                    var temp = dict[item] as Dictionary<string, object>;
                    var tempList = dict[item] as List<object>;
                    var tempString = dict[item] as string;
                    if(temp != null)
                    {
                        dict = temp;
                    }
                    else if(tempList != null)
                    {
                        return tempList;
                    }
                    else
                    {
                        return tempString;
                    }
                }
                else
                {
                    throw new KeyNotFoundException("Bad Path");
                }

            }
            return dict;
        }
        static dynamic ToDynamic(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
              
                var expandoDictionary = new Dictionary<string, object>();

                foreach (var property in element.EnumerateObject())
                {
                    expandoDictionary[property.Name] = ToDynamic(property.Value);
                }

                return expandoDictionary;
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                var list = new List<object>();
                foreach (var arrayElement in element.EnumerateArray())
                {
                    list.Add(ToDynamic(arrayElement));
                }
                return list;
            }
            else
            {
                return element.GetRawText().Trim('"');
            }
        }
    }
}
