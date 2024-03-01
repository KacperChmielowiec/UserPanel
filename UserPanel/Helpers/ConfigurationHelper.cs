using UserPanel.References;

namespace UserPanel.Helpers
{
    public static class ConfigurationHelper
    {
        public static IConfiguration config;
        public static void Initialize(IConfiguration Configuration)
        {
            config = Configuration;
            AppReferences.BASE_APP_PATH = config.GetValue<string>("BASE_PATH")?.Replace("//","/") ?? "";
            AppReferences.CONFIG_APP_PATH = config.GetValue<string>("CONFIG_PATH")?.Replace("//", "/") ?? "";
            AppReferences.USER_MOCK_PATH = "appConfig.database.mock.users";

        }
    }
}
