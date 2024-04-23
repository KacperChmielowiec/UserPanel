using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        private IMapper _mapper;
        public CampaningController(CampaningManager campaningManager, IMapper mapper)
        {
            _campaningManager = campaningManager;
            _mapper = mapper;
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
            var camp = _campaningManager.GetCampanings();
            return View(camp);
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
        [HttpGet("edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            return View(_mapper.Map<EditCampaning>(_campaningManager.GetCampanings()
                .Where(c => c.id == id).FirstOrDefault()));
        }
        [HttpPost("edit/{id}")]
        public IActionResult Edit([FromForm] EditCampaning editCampaning, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View(editCampaning);
            }
            Campaning campaning = _mapper.Map<Campaning>(editCampaning);
            campaning.id = id;
            if(editCampaning.logo != null)
            {
                campaning.details.logo = editCampaning.logo.FileName;
                _campaningManager.WriteLogoCampaning(editCampaning.logo, id.ToString());
            }
            _campaningManager.UpdateCampaning(campaning);
            return RedirectToAction("Index", new { id = id });
        }
    }
}
