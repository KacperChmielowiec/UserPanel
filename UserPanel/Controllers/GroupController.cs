using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.Adverts;
using UserPanel.Models.Group;
using UserPanel.References;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    [Authorize]
    [Route("/groups")]
    public class GroupController : Controller
    {
        private GroupManager _groupManager;
        private IMapper _mapper;
        private CampaningManager _campaningManager;
        private IDataBaseProvider _dataBaseProvider;
        public GroupController(GroupManager groupManager,CampaningManager campaningManager, IMapper mapper, IDataBaseProvider dataBaseProvider) 
        {
            _groupManager = groupManager;
            _campaningManager = campaningManager;
            _mapper = mapper;
            _dataBaseProvider = dataBaseProvider;
        }
        [Authorize]
        [HttpGet("/group/details/{id}")]
        [EndpointName(EndpointNames.GroupDetails)]
        public IActionResult Index(Guid id)
        {
            var group = _groupManager.GetGroupById(id,true);
            if (group == null) return BadRequest();
            return View(group);
        }
        [Authorize]
        [HttpGet("/groups")]
        public IActionResult Groups([FromQuery(Name="camp_id")] Guid id)
        {
            if(id == null || id == Guid.Empty) return NotFound();
            var groups = _groupManager.GetGroupsByCampID(id);
            ViewData["camp_id"] = id;
            return View(groups);
        }
        [Authorize]
        [HttpGet("create/{id}")]
        public IActionResult Create(Guid id)
        {
            var campaning = _campaningManager.GetCampaningById(id);
            return View(new CreateGroup() { id_camp = campaning.id, Utm_Source = campaning?.details?.Utm_Source ?? ""});
        }
        [Authorize]
        [HttpPost("create/{id}")]
        public IActionResult Create(CreateGroup model)
        {
            if (model.id_camp == null || model.id_camp == Guid.Empty) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            GroupModel groupModel =  _mapper.Map<GroupModel>(model);

            _groupManager.CreateGroup(model.id_camp, groupModel);

            return RedirectToAction("groups", new { camp_id = model.id_camp});   
        }
        [HttpGet("edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            return View(_mapper.Map<EditGroup>(_groupManager.GetGroupById(id)));
        }
        [HttpPost("edit")]
        public IActionResult Edit(EditGroup editGroup)
        {
            if(!ModelState.IsValid)
            {
                return View(editGroup);
            }
            var group = _mapper.Map<GroupModel>(editGroup);
            _groupManager.UpdateGroup(group);
            return RedirectToAction("Index", new { id = editGroup.id });   
        }
        [HttpGet("edit-advertisements/{groupId}")]
        public IActionResult EditAdvertList(Guid groupId)
        {
            if(groupId == Guid.Empty) return BadRequest();
            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { groupId })) return StatusCode(401);

            List<Advert> Adverts = _dataBaseProvider.GetAdvertRepository().GetAdvertGroupId(groupId);
            GroupModel groupModel = _groupManager.GetGroupById(groupId);

            AdvertGroupListView ModelView = new AdvertGroupListView() { Id_Group = groupId, Name_Group = groupModel.name};

            ModelView.AdvertGroups = Adverts.Select(a => new AdvertGroupEdit()
            {
                Id = a.Id,
                IsAttached = true,
                Template = a.Template,
                Name = a.Name,
                ModifiedTime = DateTime.Now,
                isActive = a.IsActive,
                Formats = a.Formats.Select(f => f.Size).ToList()

            }).ToList();

            return View(ModelView);
        }

        [HttpPost("edit-advertisements/sent")]
        public IActionResult EditAdvertList([FromForm] AdvertGroupListView ModelView)
        {
            if(ModelState.IsValid)
            {
                foreach(var ad in ModelView.AdvertGroups)
                {
                    if (ad.IsAttached) continue;
                }
            }
            return View(ModelView);
        }
    }
}
