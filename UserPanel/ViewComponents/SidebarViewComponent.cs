using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            Claim claim = HttpContext.User.Claims.ToArray().Where(claim => claim.Type == "Id").FirstOrDefault();
            List<Campaning> campaning = Provider.GetCampaningRepository().getCampaningsByUser(int.Parse(claim.Value));
            SidebarModel sidebarModel = new SidebarModel() { campaningList = campaning };
            return View(sidebarModel);
        }
    }
}
