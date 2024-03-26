using Microsoft.AspNetCore.Authentication.Cookies;
using UserPanel.Interfaces;
using UserPanel.Providers;
using UserPanel.Services.database;
using UserPanel.Services;
namespace UserPanel.Installers
{
    public class AppInstaller : Installer
    {
        public AppInstaller() { }

        public override void Install(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<DataBase, SqlDataBase>();
            builder.Services.AddScoped<IDataBaseProvider, DataBaseProvider>();
            builder.Services.AddScoped<UserManager, UserManager>();
            builder.Services.AddScoped<CampaningManager, CampaningManager>();
            builder.Services.AddScoped<SignInService>();
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                    .AddCookie(options =>
                    {
                        options.Cookie.Name = "User";
                        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                        options.LoginPath = "/Login";
                        options.AccessDeniedPath = "/Forbidden";

                    });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("basic", options =>
                {
                    options.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser();
                });
            });
            builder.Configuration.AddJsonFile("Config.json");
        }

    }
}
