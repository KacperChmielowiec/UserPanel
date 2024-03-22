using UserPanel.Controllers;
using UserPanel.Helpers;
using UserPanel.Models;
namespace UserPanel.References
{
    public class AppReferences
    {
        public static string BASE_APP_PATH = "";
        public static string CONFIG_APP_PATH = "";
        public static string USER_MOCK_PATH = "";


        public static Dictionary<string, UserRole> RoleMap = new Dictionary<string, UserRole>()
        {
            { "ADMIN", UserRole.ADMIN },
            { "USER", UserRole.USER },
            { "EMPLOYEE", UserRole.EMPLOYEE },
        };

        public static class PathRoutes
        {
            public static string Login = "Login";
            public static string Register = "Register";
            public static string Home = "/";
        }

        public static class ViewComponentsNames
        { 
            public static string Sidebar = "Sidebar";
            public static string Nav = "navigation";
        }
    }
}
