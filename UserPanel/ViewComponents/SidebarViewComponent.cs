using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Helpers;
using UserPanel.Models.components;
using UserPanel.Providers;
using UserPanel.Interfaces;
using UserPanel.Models.Camp;
using UserPanel.References;
namespace UserPanel.ViewComponents
{
    [Authorize]
    [ViewComponent(Name = "Sidebar")]
    public class SidebarViewComponent : ViewComponent
    {
        private IDataBaseProvider Provider;
        public SidebarViewComponent(IDataBaseProvider provider ) { 
            this.Provider = provider;
        }
        
        private Guid getCampId()
        {
            if(HttpContext.Request.RouteValues.ContainsKey("id") && HttpContext.Request.Path.ToString().Contains("/details"))
            {
               return new Guid(HttpContext.Request.RouteValues["id"].ToString());
            }
            if(HttpContext.Request.Query.ContainsKey(AppReferences.QueryCamp))
            {
                return new Guid(HttpContext.Request.Query[AppReferences.QueryCamp].ToString());
            }
            return Guid.Empty;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            SidebarModel sidebarModel = new SidebarModel() { campaningList = HttpContext.Session.GetJson<List<Campaning>>("sessionCamp") };
            if (HttpContext.Items.ContainsKey(AppReferences.CurrPageType))
            {
                if (HttpContext.Items[AppReferences.CurrPageType] is PageTypes type)
                {
                    var PageType = (PageTypes)HttpContext.Items[AppReferences.CurrPageType];
                    switch (PageType)
                    {
                        case PageTypes.HOME:
                            sidebarModel.Page = PageTypes.HOME;
                            break;
                        case PageTypes.CAMP:
                            sidebarModel.activeCamp = getCampId();
                            sidebarModel.Page = PageTypes.CAMP;
                            break;
                        case PageTypes.GROUP:
                            sidebarModel.activeCamp = getCampId();
                            sidebarModel.Page = PageTypes.GROUP;
                            break;
                    }
                }
            }
            return View(sidebarModel);
        }
    }
}
