using Microsoft.EntityFrameworkCore;
using UserPanel.Helpers;
using UserPanel.Installers;
using UserPanel.Middleware;
using UserPanel.Providers;
using UserPanel.References;
using UserPanel.Services;


var builder = WebApplication.CreateBuilder(args);

builder.InstallServices();
builder.Logging.AddProvider(new EfLoggerProvider(builder.Services.BuildServiceProvider().GetRequiredService<AppDbContext>()));
builder.Services.AddScoped<ConfigService, ConfigService>();
ConfigurationHelper.Initialize(builder.Configuration);
ConfigManager.LoadConfig();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AddingStuffAndChecking v1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

if(AppReferences.RepoType == AppReferences.CONFIG_MOCK)
{
    app.UseMiddleware<SessionLoadMockMiddleware>();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<PageTypeMiddleware>();
app.UseMiddleware<InactivityTimeoutMiddleware>();
app.MapControllerRoute(
    name: "home",
    pattern: "{Dashboard:regex(^(?i)dashboard$)}",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}",
    defaults: new { controller = "Home", action = "Index" });


app.UseStatusCodePagesWithRedirects("/Code/{0}");
app.EnsurePopulate();
app.Run();
