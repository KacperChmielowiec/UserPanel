using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Attributes;
using UserPanel.Models.Camp;
using UserPanel.References;
using UserPanel.Services;
using UserPanel.Types;
namespace UserPanel.Controllers
{
    [Authorize]
    [CampFilter]
    public class CampaignController : Controller
    {
        private readonly CampaningManager _campaningManager;
        private IMapper _mapper;
        public CampaignController(CampaningManager campaningManager, IMapper mapper)
        {
            _campaningManager = campaningManager;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("/campaign/details/{id}")]
        [EndpointName(EndpointNames.CampaningDetails)]
        public IActionResult Index(Guid id)
        {
            _campaningManager.SetCampaignSession(id);
            var camp = _campaningManager.GetCampanings().Where(camp => camp.id == id).FirstOrDefault();
            return View(camp);
        }
        [Authorize]
        [HttpGet("/campaigns/list")]
        public IActionResult Campaigns()
        {
            var camps = _campaningManager.GetCampanings();
            return View(camps);
        }
        [Authorize]
        [HttpPost("campaign/switch")]
        public IActionResult Switch([FromForm] Guid id)
        {

            var list = _campaningManager
                .GetCampanings()
                .Where(camp => camp.id == id)
                .FirstOrDefault();

            list.status = !list.status;

            _campaningManager.UpdateCampaning(list);

            return RedirectToAction("Index", new { id = list.id });
        }
        [HttpGet("campaign/create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost("campaign/create")]
        public IActionResult Create(CreateCampaning form)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                _campaningManager.CreateCampaning(form);
                return RedirectToAction("Campaigns", new { success = ErrorForm.suc_create });
            }
            catch(Exception) {
                return RedirectToAction("Campaigns", new { error = ErrorForm.err_create });
            }

            
        }
        [HttpPost("campaign/delete")]
        public IActionResult Delete([FromForm] Guid id)
        {
            try
            {
                _campaningManager.DeleteCampaning(id);
                _campaningManager.RemoveCampaignSession(id);
                return RedirectToAction("Campaigns", new { success = ErrorForm.suc_remove });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { id = id, error = ErrorForm.err_remove });
            }
        }
        [HttpGet("campaign/edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            return View(_mapper.Map<EditCampaning>(_campaningManager.GetCampanings()
                .Where(c => c.id == id).FirstOrDefault()));
        }
        [HttpPost("campaign/edit/{id}")]
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
