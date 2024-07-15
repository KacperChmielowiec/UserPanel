using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Helpers;
using UserPanel.Models.components;
using UserPanel.Models.Camp;
using UserPanel.References;
using Microsoft.Extensions.Primitives;
using UserPanel.Models;
namespace UserPanel.ViewComponents
{
    [Authorize]
    [ViewComponent(Name = "Sidebar")]
    public class SidebarViewComponent : ViewComponent
    {
        private delegate bool RouteIDDelegate(HttpContext context, out Guid result);
        private RouteIDDelegate[] strategy;
        public SidebarViewComponent() {
            strategy = new RouteIDDelegate[] { GetIDFromRoute, GetIDFromQueryCamp, GetIDFromQueryGroup };
        }
        
        private bool GetIDFromRoute(HttpContext context, out Guid result)
        {
            result = Guid.Empty;
            if(context.Request.RouteValues.TryGetValue("id", out object id))
            {
                string value = (string)id;
                if(Guid.TryParse(value, out Guid r))
                {
                    result = r;
                    return true;
                }
            }
            return false;
        }
        private bool GetIDFromQueryCamp(HttpContext context, out Guid result)
        {
            result = Guid.Empty;
            if(context.Request.Query.TryGetValue("camp_id", out StringValues value))
            {
                if(Guid.TryParse(value, out Guid r))
                {
                    result = r; 
                    return true;
                }
            }
            return false;
        }
        private bool GetIDFromQueryGroup(HttpContext context, out Guid result)
        {
            result = Guid.Empty;
            return false;
        }

        private Guid TryFetchIDFromRequest()
        {
            foreach(var _strategy in strategy) {

                bool result = _strategy.Invoke(HttpContext, out Guid result_guid);
                if(result)
                {
                    if( PermissionActionManager<Guid>.GetNodeType(result_guid) == ContextNodeType.Camp)
                    {
                        return result_guid;
                    }
                    else
                    {
                        return PermissionActionManager<Guid>.GetFullPath(result_guid).Camp;
                    }
                }

            }
            return Guid.Empty;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = HttpContext.Session.GetJson<List<Campaning>>(AppReferences.SessionCamp);

            if(list == null)
            {
                list = new List<Campaning>();
            }

            SidebarModel sidebarModel = new SidebarModel() { campaningList = list };

            if (HttpContext.Items.ContainsKey(AppReferences.CurrPageType))
            {
                if (HttpContext.Items[AppReferences.CurrPageType] is PageTypes[] type)
                {
                    var PageType = (PageTypes[])HttpContext.Items[AppReferences.CurrPageType];
                    foreach (var (pageType,i) in PageType.Select((x,y) => (x,y)))
                    {
                        if(i == 0)
                        {
                            sidebarModel.PageMain = pageType;

                        }
                        if(i == 1)
                        {
                            sidebarModel.PageSub1 = pageType;
                        }
                    }
                }
            }

            sidebarModel.activeCamp = TryFetchIDFromRequest();

            return View(sidebarModel);
        }
    }
}
