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

        private List<PageTypes> breadcrumbsbreadcrumbs;

        private Dictionary<string, PageTypes> MapPageType = new Dictionary<string, PageTypes>()
        {
            {"", PageTypes.HOME },
            {"/", PageTypes.HOME },
            {"admin:dashboard", PageTypes.DASHBOARD },
            {"user:dashboard", PageTypes.DASHBOARD },
            {"admin:dashboard:create-user", PageTypes.DASHBOARD_CREATE_USER },
            {"admin:dashboard:settings", PageTypes.DASHBOARD_SETTINGS },
            {"home", PageTypes.HOME },
            {"campaigns", PageTypes.CAMPS },
            {"campaigns:list", PageTypes.LIST_CAMPS },
            {"campaign", PageTypes.CAMP},
            {"campaign:product:list", PageTypes.PRODUCTS},
            {"campaign:details", PageTypes.CAMP_DETAILS},
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
          
            breadcrumbsbreadcrumbs = new List<PageTypes>();
            for(int i = 0; i < path.Length; i++)
            {
                
                if (MapPageType.ContainsKey(path[i].ToLower()))
                {
                    breadcrumbsbreadcrumbs.Add(MapPageType[path[i].ToLower()]);

                }
                else
                {
                    string index = String.Join(":", path.Take(i + 1)).ToLower();
                    if (MapPageType.ContainsKey(index))
                    {
                        breadcrumbsbreadcrumbs.Add(MapPageType[index]);
                    }
                }
                
            }
            

            if(breadcrumbsbreadcrumbs.Count == 0) { 
                breadcrumbsbreadcrumbs.Add(PageTypes.HOME);
            }

            context.Items[AppReferences.CurrPageType] = breadcrumbsbreadcrumbs.ToArray();

            await next(context);

        }
    }
}
