using UserPanel.Helpers;

namespace UserPanel.References
{
    public class AppReferences
    {
        public static string BASE_APP_PATH = "";
        public static string CONFIG_APP_PATH = "";
        public static string USER_MOCK_PATH = "";
        public  enum UserRole
        {
           ADMIN = 0,
           USER = 1,
           EMPLOYEE = 2,
        }

        public static Dictionary<string, UserRole> RoleMap = new Dictionary<string, UserRole>()
        {
            { "Admin", UserRole.ADMIN },
            { "User", UserRole.USER },
            { "Employee", UserRole.EMPLOYEE },
        };
    }
}
