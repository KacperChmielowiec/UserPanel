using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    [Route("/campaning")]
    public class CampaningController : Controller
    {
        private readonly CampaningManager _campaningManager;

        public CampaningController(CampaningManager campaningManager)
        {
            _campaningManager = campaningManager;
        }

        [HttpGet("/campaning/details/{id}")]
        public IActionResult Index(Guid id)
        {
            _campaningManager.SetCampaningSession(id);
            return View();
        }
        [Authorize]
        [HttpGet("/campaning")]
        public IActionResult Campaning()
        {
            return View(_campaningManager.GetCampanings());
        }

    }
}
