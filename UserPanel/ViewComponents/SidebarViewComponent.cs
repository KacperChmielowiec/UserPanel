using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Helpers;
using UserPanel.Models.components;
using UserPanel.Providers;
using UserPanel.Interfaces;
using UserPanel.Models.Camp;
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
        public async Task<IViewComponentResult> InvokeAsync()
        { 
            SidebarModel sidebarModel = new SidebarModel() { campaningList = HttpContext.Session.GetJson<List<Campaning>>("sessionCamp") };
            return View(sidebarModel);
        }
    }
}
