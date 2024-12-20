﻿using Newtonsoft.Json.Serialization;
using System.Security.Policy;
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
        public static string CAMP_LOGO_PATH = "";
        public static string CAMP_LOGO_PATH_FETCH = "";
        public static string ADVERT_PATH = "";
        public static string BASE_APP_HOST = "";
        public static string CONFIG_MOCK = "mock";
        public static string RepoType = "";

        public const string UserIdClaim = "Id";
        public const string PERMISSION_SCHEME = "permission";
        public const string EmailSalt = "EmailSalt";
        public static string TypeAccessForbidden = "Forbidden";
        public static string SessionCamp = "sessionCamp";
        public static string ActiveCampKey = "activeCamp";
        public static string EnvRepositoryType = "ENVIROMENT:UserRepositoryType";

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
            public static string[] Home = new string[] {"/", "home" };
            public static string Camp = "campaning";
        }

        public static class ViewComponentsNames
        { 
            public static string Sidebar = "Sidebar";
            public static string Nav = "navigation";
            public static string DasboardSidebar = "DashboardSidebar";
        }
        public static string CurrPageType = "PageType";
        public static string QueryCamp = "camp_id";
    }
}
