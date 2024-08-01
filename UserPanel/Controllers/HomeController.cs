using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using UserPanel.Models;
using UserPanel.Models.Home;
using UserPanel.References;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    [Authorize]
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly CampaningManager _campaningManager;
        public HomeController(ILogger<HomeController> logger,IConfiguration configuration, CampaningManager campaningManager, IWebHostEnvironment env)
        {
            _logger = logger;
            _configuration = configuration;
            _campaningManager = campaningManager;
            var p = PermissionActionManager<Guid>.InstanceContext;
            var e = env;

        }
        [EndpointName(EndpointNames.DashboardHomeGet)]
        public IActionResult Index([FromQuery] int timerate = 7)
        {
            HomeModel model = new HomeModel();
            model.FilterParametr = new FilterParametr() {
                rate = (ButtonFilterRate)Enum.Parse(typeof(ButtonFilterRate), timerate.ToString()),
                FilterCampanings = _campaningManager.GetCampanings()
            };
            return View(model);
        }
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("/Code/{code}")]
        public int Code(int code)
        {
            return code;
        }

        
    }
}