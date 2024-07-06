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
            AppReferences.CAMP_LOGO_PATH = config.GetValue<string>("LOGO_PATH")?.Replace("//", "/") ?? "";
            AppReferences.CAMP_LOGO_PATH_FETCH = config.GetValue<string>("LOGO_PATH_FETCH")?.Replace("//", "/") ?? "";
            AppReferences.BASE_APP_HOST = config.GetValue<string>("APP_HOST") ?? "";
            AppReferences.RepoType = config["ENVIROMENT:UserRepositoryType"]?.ToLower();
            AppReferences.ADVERT_PATH = config.GetValue<string>("ADVERT_PATH")?.Replace("//", "/") ?? "";

        }
    }
}
