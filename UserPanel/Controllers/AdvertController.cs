using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.Adverts;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    public class AdvertController : Controller
    {
        private IMapper _mapper;
        private IDataBaseProvider _dataBaseProvider;
        public AdvertController(IMapper mapper, IDataBaseProvider dataBaseProvider)
        {
            _mapper = mapper;
            _dataBaseProvider = dataBaseProvider;
        }

        [Authorize]
        [HttpGet("/advertisement/create-form")]
        public IActionResult Index([FromQuery] Guid id_group)
        {
            
            if(id_group == Guid.Empty)
            {
                return BadRequest();
            }
            if(!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] {id_group}))
            {
                return StatusCode(401);
            }

            return View( new AdvertForm() { id_group = id_group } );
        }
        [Authorize]
        [HttpPost("/advertisement/create")]
        public IActionResult Create(AdvertForm advert)
        {
          

            if(ModelState.IsValid)
            {

                Advert advertModel = _mapper.Map<Advert>(advert);
                advertModel.Id = Guid.NewGuid();

                if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { advert.id_group }))
                {
                    return StatusCode(401);
                }
                

                foreach(var (format,i) in advert.Formats.Select((x,y) => (x, y)))
                {
                    string path = PathGenerate.GetAdvertPath(advertModel.Id, format.Size);
                    var FormFileService = new FormFileService(format.StaticImg);
                    bool result = FormFileService.WriteFile(path);
                    if(!result)
                    {
                        return BadRequest("Cannot save the photo");
                    }

                    advertModel.Formats[i].Src = PathGenerate.ShrinkRoot(FormFileService.GettFullSavedPath());
                    try
                    {
                        _dataBaseProvider.GetAdvertRepository().CreateAdvert(advertModel,advert.id_group);

                    }catch(Exception ex)
                    {
                        //TODO revert saved photo in specified directory.
                    }
                }

                return Redirect("/");
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpGet("/advertisement/update-form/{id}")]
        public IActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id }))
            {
                return StatusCode(401);
            }

            Advert Curr_Ad = _dataBaseProvider.GetAdvertRepository().GetAdvertById(id);

            if (Curr_Ad == null)
            {
                return NotFound();
            }
            
            AdvertForm Curr_Ad_Form = _mapper.Map<AdvertForm>(Curr_Ad);
            

            return View(Curr_Ad_Form);
        }
        [Authorize]
        [HttpPost("/advertisement/update-form/sent")]
        public IActionResult Edit(AdvertForm form)
        {
            if(ModelState.IsValid)
            {
                _dataBaseProvider.GetAdvertRepository().UpdateAdvert(_mapper.Map<Advert>(form));
                return Redirect("/");
            }

            ViewData["Edit"] = true;
            return View();
        }

        [Authorize]
        [HttpPost("/advertisement/remove/{id}")]
        public IActionResult Delete(Guid id, [FromQuery] string ReturnUrl = "")
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }
            if(!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id }))
            {
                return StatusCode(401);
            }

            _dataBaseProvider.GetAdvertRepository().DeleteAdvertsById(id);

            if(ReturnUrl.Length > 0)
            {
                return Redirect(ReturnUrl);
            }

            return Redirect("/");

        }
        [Authorize]
        [HttpPost("/advertisement/detach/{id_group}/{id}")]
        public IActionResult Detach(Guid id_group, Guid id, [FromQuery] string ReturnUrl = "")
        {
            if (id == Guid.Empty || id_group == Guid.Empty)
            {
                return BadRequest();
            }
            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id_group, id }))
            {
                return StatusCode(401);
            }

            _dataBaseProvider.GetAdvertRepository().DettachAdvertFromGroup(id,id_group);

            if (ReturnUrl.Length > 0)
            {
                return Redirect(ReturnUrl);
            }

            return Redirect("/");

        }

        [Authorize]
        [HttpGet("campaign/advertisement/preview/{id}")]
        public IActionResult Preview(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id }))
            {
                return StatusCode(401);
            }

            Advert advert = _dataBaseProvider.GetAdvertRepository().GetAdvertById(id);

            if (advert == null) return NotFound();

            return View(advert);

        }

        [Authorize]
        [HttpGet("/list-all/{id}")]
        public string ListAll(int id)
        {
            return JsonSerializer.Serialize(_dataBaseProvider.GetAdvertRepository().GetAdvertByUserId(id));
        }

    }
}
