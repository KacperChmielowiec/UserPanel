using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using UserPanel.Helpers;
using UserPanel.Installers;
using UserPanel.Interfaces;
using UserPanel.Models.Config;
using UserPanel.Providers;
using UserPanel.Services;
using UserPanel.Services.database;

var builder = WebApplication.CreateBuilder(args);
// Mo¿esz dodaæ wiêcej ni¿ jeden Swagger dokument
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AddingStuffAndCheckingI", Version = "v1" });
});

builder.InstallServices();
builder.Services.AddScoped<EmailService, EmailService>();
builder.Services.AddScoped<GroupManager, GroupManager>();
ConfigurationHelper.Initialize(builder.Configuration);
ConfigManager.LoadConfig();

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
 
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AddingStuffAndChecking v1");
});
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "dashboard",
    pattern: "{Dashboard:regex(^(?i)dashboard$)}",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "home",
    pattern: "{controller=Home}/{action=Index}",
    defaults: new { controller = "Home", action = "Index" });


app.UseStatusCodePagesWithRedirects("/Code/{0}");
app.Run();
