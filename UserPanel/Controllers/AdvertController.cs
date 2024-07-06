using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.Adverts;
using UserPanel.References;
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


        [HttpGet("/advertisement/create-form")]
        public IActionResult Index([FromQuery] Guid id_group)
        {
            var result = Directory.Exists("static/advertisement/96e898ff-754d-45c8-9704-93326e8f0c21/300x300");

            if(id_group == Guid.Empty)
            {
                return BadRequest();
            }
            if(!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] {id_group}))
            {
                return StatusCode(403);
            }

            return View( new AdvertForm() { id_group = id_group } );
        }

        [HttpPost("/advertisement/create")]
        public IActionResult Create(AdvertForm advert)
        {
          

            if(ModelState.IsValid)
            {

                Advert advertModel = _mapper.Map<Advert>(advert);
                advertModel.Id = Guid.NewGuid();

                if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { advertModel.Parent }))
                {
                    return StatusCode(403);
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

                    advertModel.Formats[i].Url = PathGenerate.ShrinkRoot(FormFileService.GettFullSavedPath());
                    try
                    {
                        _dataBaseProvider.GetAdvertRepository().CreateAdvert(advertModel);

                    }catch(Exception ex)
                    {
                        //TODO revert saved photo in specified directory.
                    }
                }

                return Redirect("/");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/advertisement/update-form/{id}")]
        public IActionResult Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id }))
            {
                return StatusCode(403);
            }

            Advert Curr_Ad = _dataBaseProvider.GetAdvertRepository().GetAdvertById(id);

            if (Curr_Ad == null)
            {
                return NotFound();
            }
            
            AdvertForm Curr_Ad_Form = _mapper.Map<AdvertForm>(Curr_Ad);
            

            return View(Curr_Ad_Form);
        }

    }
}
