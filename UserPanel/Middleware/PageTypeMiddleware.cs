using Microsoft.AspNetCore.Http.Features;
using UserPanel.References;
using UserPanel.Helpers;
namespace UserPanel.Middleware
{
    public class PageTypeMiddleware
    {
        private RequestDelegate next;
        public PageTypeMiddleware(RequestDelegate nextDelgate)
        {
            next = nextDelgate;
        }
        public async Task Invoke(HttpContext context)
        {

            // Context for sidebar class binding
            if (AppReferences.PathRoutes.Home.Contains(context.Request.Path.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                context.Items[AppReferences.CurrPageType] = PageTypes.HOME;
            }
            else if (context.GetRouteData().Values["controller"]?.ToString() == "Campaning")
            {
                context.Items[AppReferences.CurrPageType] = PageTypes.CAMP;
            }
            else if (context.GetRouteData().Values["controller"]?.ToString() == PageTypes.GROUP.GetStringValue())
            {
                context.Items[AppReferences.CurrPageType] = PageTypes.GROUP;
            }
            await next(context);

        }
    }
}
