using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Models.Dashboard;

namespace UserPanel.ViewComponents
{

    [Authorize(policy: "admin")]
    [ViewComponent(Name = "SessionSettings")]
    public class SessionSettingsViewComponent : ViewComponent
    {
        IConfiguration _configuration;
        public SessionSettingsViewComponent(IConfiguration configuration) { 
            _configuration = configuration;
        }
        public IViewComponentResult Invoke()
        {
            var model = new SessionSettingsModel();
            var value = (int)_configuration.GetSection("ENVIROMENT").GetValue(typeof(int), "MaxInActiveTimeSession");
            model.MaxTimeSession = value;
            return View(model);
        }
    }
}
