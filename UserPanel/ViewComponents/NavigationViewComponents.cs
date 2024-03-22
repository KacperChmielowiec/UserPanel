using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared;
using System.Security.Claims;
using UserPanel.Models.Campaning;
using UserPanel.Models.components;
namespace UserPanel.ViewComponents
{
    [ViewComponent(Name = "navigation")]
    public class NavigationViewComponents : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        { 
            return View();
        }
    }
}
