using Newtonsoft.Json.Linq;

public class ConfigService
{
    private readonly string _filePath = "appsettings.json";
    IWebHostEnvironment _environment;
    IConfiguration _configuration;
    public ConfigService(IWebHostEnvironment environment, IConfiguration configuration) { 
        
        _environment = environment;
        _configuration = configuration;
        _filePath = Path.Combine(_environment.ContentRootPath, "appsettings.json");

    }
    public void UpdateSetting(string[] section, string key, int newValue)
    {
        var json = File.ReadAllText(_filePath);
        var jObject = JObject.Parse(json);

        JToken partial_section = jObject;

        section.ToList().ForEach(item =>
        {
            partial_section = partial_section[item];
        });

        partial_section[key] = newValue;

        File.WriteAllText(_filePath, jObject.ToString());

        // Opcjonalnie: Wymuszenie odświeżenia konfiguracji, jeśli używasz IConfigurationRoot
        if (_configuration is IConfigurationRoot configurationRoot)
        {
            configurationRoot.Reload();
        }
    }
}