using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using UserPanel.Helpers;
using UserPanel.Models;
namespace UserPanel.Middleware
{
   

    public class InactivityTimeoutMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _maxInactivityTime;

        public InactivityTimeoutMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            // Pobranie ustawień czasu braku aktywności z appsettings.json
            _maxInactivityTime = configuration.GetValue<int>("ENVIROMENT:MaxInActiveTimeSession");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Sprawdzenie, czy użytkownik jest zalogowany
            if (context.User.Identity.IsAuthenticated && context.User.IsInRole(UserRole.USER.GetStringValue()))
            {
                // Pobranie ostatniego czasu aktywności z sesji
                var lastActivity = context.Session.GetString("LastActivity");

                if (!string.IsNullOrEmpty(lastActivity))
                {
                    var lastActivityTime = DateTime.Parse(lastActivity);

                    // Sprawdzenie, czy czas braku aktywności przekracza maksymalny czas
                    if ((DateTime.UtcNow - lastActivityTime).TotalSeconds > _maxInactivityTime)
                    {
                        // Wylogowanie użytkownika
                        context.Session.Clear(); // Wyczyść sesję
                        await context.SignOutAsync(); // Wyloguj użytkownika

                        // Przekierowanie na stronę logowania (lub inną stronę)
                        context.Response.Redirect("/");
                        return;
                    }
                }

                // Aktualizacja ostatniego czasu aktywności w sesji
                context.Session.SetString("LastActivity", DateTime.UtcNow.ToString());
            }

            // Kontynuowanie przetwarzania żądania
            await _next(context);
        }
    }

}
