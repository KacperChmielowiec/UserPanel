using UserPanel.Helpers;
using UserPanel.Installers;
using UserPanel.Middleware;
using UserPanel.Services;


var builder = WebApplication.CreateBuilder(args);

builder.InstallServices();

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
app.UseRouting();

app.UseMiddleware<PageTypeMiddleware>();

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
