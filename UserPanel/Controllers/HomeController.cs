using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using UserPanel.Models;
using UserPanel.Models.Home;
using UserPanel.Models.Campaning;
using UserPanel.Models.User;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly CampaningManager _campaningManager;
        public HomeController(ILogger<HomeController> logger,IConfiguration configuration, CampaningManager campaningManager)
        {
            _logger = logger;
            _configuration = configuration;
            _campaningManager = campaningManager;
        }

        public IActionResult Index(IFormCollection form,[FromQuery] int timerate = 7)
        {
            HomeModel model = new HomeModel();
            model.FilterParametr = new FilterParametr() {
                rate = (ButtonFilterRate)Enum.Parse(typeof(ButtonFilterRate), timerate.ToString()),
                FilterCampanings = _campaningManager.GetCampanings().Select(c => new CampaningFilterModel(c, form["campaning"].Contains(c.id.ToString()))).ToList()
            };
            return View(model);
        }
        [Authorize]
        [HttpGet("campaning")]
        public IActionResult Campaning()
        { 
            return View(_campaningManager.GetCampanings());
        }
        [Authorize]
        [HttpGet("SetCampaning")]
        public IActionResult SetCampaning([FromQuery] Guid cmp_id)
        {
            _campaningManager.SetCampaningSession(cmp_id);
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
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