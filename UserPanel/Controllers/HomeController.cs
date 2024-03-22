using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserPanel.Models;
using UserPanel.Models.Home;
using UserPanel.Models.User;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index([FromQuery] ButtonFilterRate rate = ButtonFilterRate.RATE_1)
        {
            HomeModel model = new HomeModel();
            FilterParametr filterParametr = new FilterParametr() { rate = rate };
            model.FilterParametr = filterParametr;
            return View(model);
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}