using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserPanel.Helpers;
using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Campaning;
using UserPanel.Models.components;
using UserPanel.Providers;
namespace UserPanel.ViewComponents
{
    [Authorize]
    [ViewComponent(Name = "Sidebar")]
    public class SidebarViewComponent : ViewComponent
    {
        private DataBaseProvider Provider;
        public SidebarViewComponent(DataBaseProvider provider ) { 
            this.Provider = provider;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        { 
            SidebarModel sidebarModel = new SidebarModel() { campaningList = HttpContext.Session.GetJson<List<Campaning>>("sessionCamp") };
            return View(sidebarModel);
        }
    }
}
