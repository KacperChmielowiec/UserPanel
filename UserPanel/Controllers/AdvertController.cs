using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using UserPanel.Attributes;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.Adverts;
using UserPanel.Services;
using UserPanel.Types;

namespace UserPanel.Controllers
{
    [AdvertFilter]
    public class AdvertController : Controller
    {
        private IMapper _mapper;
        private IDataBaseProvider _dataBaseProvider;
        private AdvertManager _advertManager;
        public AdvertController(IMapper mapper, IDataBaseProvider dataBaseProvider, AdvertManager advertManager)
        {
            _mapper = mapper;
            _dataBaseProvider = dataBaseProvider;
            _advertManager = advertManager;
        }

        [Authorize]
        [HttpGet("campaign/advertisement/create-form-static")]
        public IActionResult CreateStatic([FromQuery] Guid id_camp)
        {
            
            if(id_camp == Guid.Empty)
            {
                return BadRequest();
            }
            if(!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] {id_camp}))
            {
                return StatusCode(401);
            }

            var advert = new AdvertForm<AdvertFormatFormStatic>() { id_camp = id_camp };

            AdvertFormatHelper.SortAndFill(advert.Formats, 2);

            return View(advert);
        }

        [Authorize]
        [HttpGet("campaign/advertisement/create-form-dynamic")]
        public IActionResult CreateDynamic([FromQuery] Guid id_camp)
        {
            if (id_camp == Guid.Empty)
            {
                return BadRequest();
            }
            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id_camp }))
            {
                return StatusCode(401);
            }

            var advert = new AdvertForm<AdvertFormatFormDynamic>() { id_camp = id_camp };
            AdvertFormatHelper.SortAndFill(advert.Formats, 2);

            return View(advert);
        }


        [Authorize]
        [HttpPost("campaign/advertisement/create-template")]
        public IActionResult Create( [FromForm] Guid id, [FromForm] string template )
        {
            AD_TEMPLATE template_enum;

            if(Enum.TryParse(typeof(AD_TEMPLATE), template, out object result))
            {
                template_enum = (AD_TEMPLATE)result;
                switch(template_enum)
                {
                    case AD_TEMPLATE.Static:
                        return RedirectToAction("CreateStatic", new { id_camp = id } );
                    case AD_TEMPLATE.Dynamic: 
                        return RedirectToAction("CreateDynamic", new { id_camp = id } );
                    default: break;
                }
            }

            return RedirectToAction("List", new { id = id });
        }

        [Authorize]
        [HttpPost("campaign/advertisement/static/create")]
        public IActionResult CreateStaticPost(AdvertForm<AdvertFormatFormStatic> advert)
        {
            if(ModelState.IsValid)
            {

                Advert<AdvertFormat> advertModel = _mapper.Map<Advert<AdvertFormat>>(advert);
                advertModel.Id = Guid.NewGuid();
                advertModel.Template = AD_TEMPLATE.Static;
                advertModel.Created = DateTime.Now;

                if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { advert.id_camp }))
                {
                    return StatusCode(401);
                }
                

                foreach(var (format,i) in advert.Formats.Select((x,y) => (x, y)))
                {
                    
                    
                    string path = _advertManager.UpdateAdvertImage(advertModel.Id, format.Size, format.StaticImg);
                    advertModel.Formats[i].Src = path;
                   
                }
                try
                {
                    _dataBaseProvider.GetAdvertRepository().CreateAdvert(advertModel, advert.id_camp);

                }
                catch (Exception ex)
                {
                    return RedirectToAction("List", new { id = advert.id_camp, error = ErrorForm.err_create });
                }

                return RedirectToAction("List", new { id = advert.id_camp , success = ErrorForm.suc_create});
            }

            return RedirectToAction("CreateStatic", advert);
        }
        [Authorize]
        [HttpPost("campaign/advertisement/dynamic/create")]
        public IActionResult CreateDynamicPost(AdvertForm<AdvertFormatFormDynamic> advert)
        {
            if (ModelState.IsValid)
            {

                Advert<AdvertFormatDynamic> advertModel = _mapper.Map<Advert<AdvertFormatDynamic>>(advert);

                advertModel.Id = Guid.NewGuid();
                advertModel.Template = AD_TEMPLATE.Dynamic;
                advertModel.Created = DateTime.Now;

                if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { advert.id_camp }))
                {
                    return StatusCode(401);
                }


                foreach (var (format, i) in advertModel.Formats.Select((x, y) => (x, y)))
                {

                    format.Id = Guid.NewGuid();
                    advertModel.Formats[i].Src = $"/campaign/advert/render?ad={advertModel.Id}&ad_f={format.Id}";
                    
                }
                try
                {
                    _dataBaseProvider.GetAdvertDyRepository().CreateAdvert(advertModel, advert.id_camp);

                }
                catch (Exception ex)
                {
                    return RedirectToAction("List", new { id = advert.id_camp, error = ErrorForm.err_create });
                }
                return RedirectToAction("List", new { id = advert.id_camp, success = ErrorForm.suc_create });
            }
            return RedirectToAction("CreateStatic", advert);
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

            Advert<AdvertFormat> Curr_Ad = _dataBaseProvider.GetAdvertRepository().GetAdvertById(id);

            if (Curr_Ad == null)
            {
                return NotFound();
            }

            if (Curr_Ad.Template == AD_TEMPLATE.Static)
            {
                AdvertForm<AdvertFormatFormStatic> Curr_Ad_Form = _mapper.Map<AdvertForm<AdvertFormatFormStatic>>(Curr_Ad);

                Curr_Ad_Form.id_camp = PermissionActionManager<Guid>.GetFullPath(id).Camp;

                AdvertFormatHelper.SortAndFill(Curr_Ad_Form.Formats, 2);

                return View("EditStatic",Curr_Ad_Form);
            }
            else
            {
                AdvertForm<AdvertFormatFormDynamic> Curr_Ad_Form = _mapper.Map<AdvertForm<AdvertFormatFormDynamic>>(Curr_Ad);

                Curr_Ad_Form.id_camp = PermissionActionManager<Guid>.GetFullPath(id).Camp;

                AdvertFormatHelper.SortAndFill(Curr_Ad_Form.Formats, 2);

                return View("EditDynamic", Curr_Ad_Form);
            }
        }

        [Authorize]
        [HttpPost("/advertisement/update-form/static/sent")]
        public IActionResult EditStaticPost(AdvertForm<AdvertFormatFormStatic> form)
        { 
            if (ModelState.IsValid)
            {

                foreach (var (format, i) in form.Formats.Select((x, y) => (x, y)))
                {
                    if(format.StaticImg != null)
                    {
                        string path = _advertManager.UpdateAdvertImage(form.Id, format.Size, format.StaticImg);
                        format.Src = path;
                    }
                }

                _dataBaseProvider.GetAdvertRepository().UpdateAdvert(_mapper.Map<Advert<AdvertFormat>>(form));

                return RedirectToAction("List", new { id = form.id_camp, success = ErrorForm.suc_edit });
            }
            return View("EditStatic",form);
        }

        [Authorize]
        [HttpPost("/advertisement/update-form/dynamic/sent")]
        public IActionResult EditDynamicPost(AdvertForm<AdvertFormatFormDynamic> form)
        {
            if (ModelState.IsValid)
            {
                _dataBaseProvider.GetAdvertDyRepository().UpdateAdvert(_mapper.Map<Advert<AdvertFormatDynamic>>(form));
                return RedirectToAction("List", new { id = form.id_camp, success = ErrorForm.suc_edit });
            }
            return View("EditStatic", form);
        }

        [Authorize]
        [HttpPost("/advertisement/remove/{id}")]
        public IActionResult Remove(Guid id, [FromQuery] Guid camp_id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest();
            }

            if(!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id }))
            {
                return StatusCode(401);
            }

            try
            {
                _dataBaseProvider.GetAdvertRepository().DeleteAdvertsById(id);

            }catch(Exception) { 
                
                return RedirectToAction("List", new {id = camp_id, error = ErrorForm.err_remove});
            }

            return RedirectToAction("List", new { id = camp_id, success = ErrorForm.suc_remove });

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

            Advert<AdvertFormat> advert = _dataBaseProvider.GetAdvertRepository().GetAdvertById(id);

            if (advert == null || advert.Template == AD_TEMPLATE.Dynamic) return NotFound();

            return View(advert);

        }

        [Authorize]
        [HttpGet("campaign/advertisement/preview-dynamic/{id}")]
        public IActionResult PreviewDynamic(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id }))
            {
                return StatusCode(401);
            }

            Advert<AdvertFormatDynamic> advert = _dataBaseProvider.GetAdvertDyRepository().GetAdvertById(id);

            if (advert == null || advert.Template == AD_TEMPLATE.Static) return NotFound();

            return View(advert);

        }

        [Authorize]
        [HttpGet("campaign/advertisements-list/{id}")]
        public IActionResult List(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id }))
            {
                return StatusCode(401);
            }
            
            List<Advert<AdvertFormat>> AdvertsList = _dataBaseProvider.GetAdvertRepository().GetAdvertsByCampId(id);

            AdvertsList.Sort((a, b) => a.Created.CompareTo(b.Created));

            return View(AdvertsList);
        }



        [Authorize]
        [HttpPost("/advertisements/switch/{id}")]
        public IActionResult Switch(Guid id)
        {
            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id }))
            {
                return StatusCode(401);
            }

            Guid CampId = PermissionActionManager<Guid>.GetFullPath(id).Camp;

            try
            {
              var Advert =  _dataBaseProvider.GetAdvertRepository().GetAdvertById(id);
              Advert.IsActive = !Advert.IsActive;
              _dataBaseProvider.GetAdvertRepository().UpdateAdvert(Advert);

            }catch (Exception) {

                return RedirectToAction("List", new { id = CampId, error = ErrorForm.err_edit });

            }

            return RedirectToAction("List", new { id = CampId });
        }



        [Authorize]
        [HttpGet("/list-all/{id}")]
        public string ListAll(int id)
        {
            return JsonSerializer.Serialize(_dataBaseProvider.GetAdvertRepository().GetAdvertByUserId(id));
        }



    }
}
