using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Providers;
using UserPanel.Services;
using UserPanel.Services.database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<DataBase,SqlDataBase>();
builder.Services.AddScoped<DataBaseProvider>();
builder.Services.AddScoped<UserManager, UserManager>();
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

ConfigurationHelper.Initialize(builder.Configuration);
ConfigManager.LoadConfig();

builder.Services.Configure<UserPanel.Models.PasswordHashOptions>(options =>
{
    options.passwordHasherAlgorithms = HashAlgorithmName.SHA1;
    options.SaltSize = 16;
    options.Iterations = 8192;
    options.HashSize = 256;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/testAuth", (HttpContext ctx) =>
{
    return "It is work";
}).RequireAuthorization("basic");

app.MapGet("/show", (HttpContext ctx) =>
{
    var user = ctx.User;
    return "";
});

app.MapControllerRoute(
    name: "dashboard",
    pattern: "{Dashboard:regex(^(?i)dashboard$)}",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "home",
    pattern: "{controller=Home}/{action=Index}",
    defaults: new { controller = "Home", action = "Index" });



app.Run();
