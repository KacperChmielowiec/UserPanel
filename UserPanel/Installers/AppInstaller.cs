using Microsoft.AspNetCore.Authentication.Cookies;
using UserPanel.Interfaces;
using UserPanel.Providers;
using UserPanel.Services.database;
using UserPanel.Services;
using UserPanel.Models.Config;
using System.Security.Cryptography;
using Microsoft.OpenApi.Models;
using UserPanel.Models;
using UserPanel.Services.observable;
namespace UserPanel.Installers
{
    public class AppInstaller : Installer
    {
        public AppInstaller() { }

        public override void Install(WebApplicationBuilder builder)
        {
            // Add services to the container.
            builder.Services.AddSwaggerGen(c =>{c.SwaggerDoc("v1", new OpenApiInfo { Title = "AddingStuffAndCheckingI", Version = "v1" });});
            builder.Services.AddScoped<EmailService, EmailService>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<DataBase, SqlDataBase>();
            builder.Services.AddSingleton<IDataBaseProvider, DataBaseProvider>();
            builder.Services.AddScoped<PermissionContextProvider>();
            builder.Services.AddScoped(ctx => ctx
                .GetRequiredService<PermissionContextProvider>()
                .GetPermissionContext()
            );
            builder.Services.BuildServiceProvider().GetRequiredService<PermissionContext>();
            builder.Services.AddScoped(ctx => ctx
                .GetRequiredService<PermissionContextProvider>()
                .GetPermissionContextActions()
            );
            builder.Services.AddScoped(ctx => ctx
                .GetRequiredService<PermissionContextProvider>()
                .GetUserActionSubject()
            );
            builder.Services.AddScoped<PermissionContextActions>();
            builder.Services.AddScoped<GroupManager, GroupManager>();
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


            builder.Services.Configure<UserPanel.Models.PasswordHashOptions>(options =>
            {
                options.passwordHasherAlgorithms = HashAlgorithmName.SHA1;
                options.SaltSize = 16;
                options.Iterations = 8192;
                options.HashSize = 256;

            });
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {

            });
            builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("STMP_CONFIG"));
   
        }

    }
}
