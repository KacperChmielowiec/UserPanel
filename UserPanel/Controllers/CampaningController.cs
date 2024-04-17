using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Helpers;
using UserPanel.Interfaces.Abstract;
using UserPanel.Models;
using UserPanel.Models.Camp;
using UserPanel.Models.User;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    [Authorize]
    [Route("/campaning")]
    public class CampaningController : Controller
    {
        private readonly CampaningManager _campaningManager;
        public CampaningController(CampaningManager campaningManager)
        {
            _campaningManager = campaningManager;
        }

        [Authorize]
        [HttpGet("/campaning/details/{id}")]
        public IActionResult Index(Guid id)
        {
            _campaningManager.SetCampaningSession(id);
            var list = _campaningManager.GetCampanings().Where(camp => camp.id == id).FirstOrDefault();
            return View(list);
        }
        [Authorize]
        [HttpGet("/campaning")]
        public IActionResult Campaning()
        {
            return View(_campaningManager.GetCampanings());
        }
        [Authorize]
        [HttpPost("switch")]
        public IActionResult Switch([FromForm] bool state, [FromForm] Guid id)
        {

            var list = _campaningManager
                .GetCampanings()
                .Where(camp => camp.id == id)
                .FirstOrDefault();

            list.status = !list.status;

            _campaningManager.UpdateCampaning(list);

            return RedirectToAction("Index", new { id = list.id });
        }
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost("create")]
        public IActionResult Create(CreateCampaning form)
        {
            if (!ModelState.IsValid)
                return View();

            _campaningManager.CreateCampaning(form);

            return RedirectToAction("Campaning");
        }
        [HttpPost("delete")]
        public IActionResult Delete([FromForm] Guid id)
        {
            _campaningManager.DeleteCampaning(id);
            return RedirectToAction("Campaning");
        }
    }
}
