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

        private PageTypes[] breadcrumbsbreadcrumbs;

        private Dictionary<string, PageTypes> MapPageType = new Dictionary<string, PageTypes>()
        {
            {"", PageTypes.HOME },
            {"/", PageTypes.HOME },
            {"home", PageTypes.HOME },
            {"campaigns", PageTypes.CAMPS },
            {"list", PageTypes.LIST_CAMPS },
            {"details", PageTypes.CAMP_DETAILS},
            {"groups", PageTypes.GROUPS},
            {"group:details", PageTypes.GROUP_DETAILS },
            {"advertisements-list", PageTypes.ADVERT_LIST },
            {"feed:list", PageTypes.FEEDS },
            {"product:list", PageTypes.PRODUCTS},
        };

        public async Task Invoke(HttpContext context)
        {
            string[] path = context.Request.Path.Value.Split("/").Skip(1).ToArray();

            path = path.Where(p => !string.IsNullOrWhiteSpace(p) && !Guid.TryParse(p, out _)).ToArray();

            if(path.Length > 2)
            {
                breadcrumbsbreadcrumbs = new PageTypes[2];
                breadcrumbsbreadcrumbs[0] = MapPageType.ContainsKey(path[0].ToLower()) ? MapPageType[path[0].ToLower()] : PageTypes.OTHER;
                breadcrumbsbreadcrumbs[1] = MapPageType.ContainsKey($"{path[1].ToLower()}:{path[2].ToLower()}") ? MapPageType[$"{path[1].ToLower()}:{path[2].ToLower()}"] : PageTypes.OTHER;
            }
            else
            {
                breadcrumbsbreadcrumbs = new PageTypes[path.Length];
                foreach (var (p,i) in path.Select((x,y) => (x, y)))
                {
                    breadcrumbsbreadcrumbs[i] = MapPageType.ContainsKey(p.ToLower()) ? MapPageType[p.ToLower()] : PageTypes.OTHER;
                }
            }

            context.Items[AppReferences.CurrPageType] = breadcrumbsbreadcrumbs;

            await next(context);

        }
    }
}
