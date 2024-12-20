using Microsoft.AspNetCore.Mvc;
using UserPanel.Helpers;
using UserPanel.Models.Camp;
using UserPanel.Models.components;
using UserPanel.References;

namespace UserPanel.ViewComponents
{
    public class DashboardSidebarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        { 

            SidebarModel sidebarModel = new SidebarModel() { };

            if (HttpContext.Items.ContainsKey(AppReferences.CurrPageType))
            {
                if (HttpContext.Items.TryGetValue(AppReferences.CurrPageType, out var pageTypeItem) && pageTypeItem is IEnumerable<PageTypes> pageTypes)
                {
                    sidebarModel.pageTypes = pageTypes.ToList();
                }
            }


            return View(sidebarModel);
        }
    }
}
