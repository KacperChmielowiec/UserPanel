using Microsoft.AspNetCore.Authentication.Cookies;
using UserPanel.Interfaces;
using UserPanel.Providers;
using UserPanel.Services.database;
using UserPanel.Services;
using UserPanel.Models.Config;
using System.Security.Cryptography;
using Microsoft.OpenApi.Models;
using UserPanel.Models;
using UserPanel.References;
using UserPanel.Helpers;
using UserPanel.Models.Messages;
using UserPanel.Filters;
using Microsoft.Extensions.Options;
using UserPanel.Models.Product;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;

namespace UserPanel.Installers
{
    public class AppInstaller : Installer
    {
        public AppInstaller() { }

        public override void Install(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("panel")));
            // Add services to the container.
            builder.Services.AddSwaggerGen(c =>{c.SwaggerDoc("v1", new OpenApiInfo { Title = "AddingStuffAndCheckingI", Version = "v1" });});
            builder.Services.AddScoped<EmailService, EmailService>();
            builder.Services.AddControllersWithViews();
           
            builder.Services.AddSession();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<DataBase, SqlDataBase>();
            builder.Services.AddSingleton<IDataBaseProvider, DataBaseProvider>();
            builder.Services.AddSingleton<PermissionContext<Guid>>( p => new PermissionContextUserPanel());
            builder.Services.AddScoped<GroupManager, GroupManager>();
            builder.Services.AddScoped<AdvertManager, AdvertManager>();
            builder.Services.AddScoped<UserManager, UserManager>();
            builder.Services.AddScoped<CampaningManager, CampaningManager>();
            builder.Services.AddScoped<SignInService>();
            builder.Services.AddScoped<PasswordHasher>();
            builder.Services.AddAuthentication(AppReferences.PERMISSION_SCHEME)
            .AddCookie(options =>
            {
                options.Cookie.Name = "User";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Login";
            })
            .AddScheme<CookieAuthenticationOptions, PermissionAuthHandler>(AppReferences.PERMISSION_SCHEME, (options) => {
                options.Cookie.Name = "User";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Login";
                options.Events.OnSigningOut  = PermissionUtils.OnSignOutValidate;
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("basic", options =>
                {
                    options.AddAuthenticationSchemes(AppReferences.PERMISSION_SCHEME)
                    .RequireAuthenticatedUser();
                });
                options.AddPolicy("admin", options =>
                {
                    options.AddAuthenticationSchemes(AppReferences.PERMISSION_SCHEME)
                    .RequireRole(UserRole.ADMIN.GetStringValue());
                });
            });
            builder.Configuration.AddJsonFile("Config.json");

            builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ResetUserPasswordValidatorEmail>());
            builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ResetUserPasswordValidatorDashboard>());

            builder.Services.Configure<PasswordHashOptions>(options =>
            {
                options.passwordHasherAlgorithms = HashAlgorithmName.SHA1;
                options.SaltSize = 16;
                options.Iterations = 8192;
                options.HashSize = 128;

            });
           
            builder.Services.Configure<List<EndpointMetaData>>(opt => builder.Configuration.GetSection("Endpoints").Bind(opt));

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {

            });

            builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("STMP_CONFIG"));
            builder.Services.Configure<EnviromentSettings>(builder.Configuration.GetSection("ENVIROMENT"));
            builder.Services.Configure<GroupFormMessages>(builder.Configuration.GetSection("messages:Group"));
            builder.Services.Configure<CampFormMessages>(builder.Configuration.GetSection("messages:Campaign"));
            builder.Services.Configure<FeedFormMessage>(builder.Configuration.GetSection("messages:Feed"));
            builder.Services.Configure<ProductMessage>(builder.Configuration.GetSection("messages:Product"));
            builder.Services.Configure<AdvertMessages>(builder.Configuration.GetSection("messages:Advert"));
            builder.Services.Configure<RenderModel>(builder.Configuration.GetSection("Render:default"));

            builder.Services.AddScoped(p => p.GetRequiredService<IOptions<GroupFormMessages>>().Value);
            builder.Services.AddScoped(p => p.GetRequiredService<IOptions<CampFormMessages>>().Value);
            builder.Services.AddScoped(p => p.GetRequiredService<IOptions<FeedFormMessage>>().Value);
            builder.Services.AddScoped(p => p.GetRequiredService<IOptions<ProductMessage>>().Value);
            builder.Services.AddScoped(p => p.GetRequiredService<IOptions<AdvertMessages>>().Value);

            builder.Services.AddScoped<GroupMessageFilterAction>();
            builder.Services.AddScoped<CampMessageFilterAction>();
            builder.Services.AddScoped<FeedMessageFilterAction>();
            builder.Services.AddScoped<ProductMessageFilterAction>();
            builder.Services.AddScoped<AdvertMessagesFilterAction>();
            
        }

    }
}
