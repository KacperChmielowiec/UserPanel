using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.Adverts;
using UserPanel.Models.Group;
using UserPanel.References;
using UserPanel.Services;
using UserPanel.Types;
using UserPanel.Filters;
using UserPanel.Helpers;
using UserPanel.Attributes;

namespace UserPanel.Controllers
{
    [Authorize]
    [GroupFilter]
    public class GroupController : Controller
    {
        private GroupManager _groupManager;
        private IMapper _mapper;
        private CampaningManager _campaningManager;
        private IDataBaseProvider _dataBaseProvider;
        public GroupController(GroupManager groupManager, CampaningManager campaningManager, IMapper mapper, IDataBaseProvider dataBaseProvider)
        {
            _groupManager = groupManager;
            _campaningManager = campaningManager;
            _mapper = mapper;
            _dataBaseProvider = dataBaseProvider;
        }
        [Authorize]
        [HttpGet("/campaign/group/details/{id}")]
        [EndpointName(EndpointNames.GroupDetails)]
        public IActionResult Index(Guid id)
        {
            var group = _groupManager.GetGroupById(id, true);
            if (group == null) return BadRequest();
            return View(group);
        }
        [Authorize]
        [HttpGet("campaign/groups")]
        public IActionResult Groups([FromQuery(Name = "camp_id")] Guid id)
        {
            if (id == null || id == Guid.Empty) return NotFound();
            var groups = _groupManager.GetGroupsByCampID(id);
            ViewData["camp_id"] = id;
            return View(groups);
        }
        [Authorize]
        [HttpGet("campaign/group/create/{id}")]
        public IActionResult Create(Guid id)
        {
            var campaning = _campaningManager.GetCampaningById(id);
            return View(new CreateGroup() { id_camp = campaning.id, Utm_Source = campaning?.details?.Utm_Source ?? "" });
        }
        [Authorize]
        [HttpPost("/create/{id}")]
        public IActionResult Create(CreateGroup model)
        {
            if (model.id_camp == null || model.id_camp == Guid.Empty) return BadRequest();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            GroupModel groupModel = _mapper.Map<GroupModel>(model);

            _groupManager.CreateGroup(model.id_camp, groupModel);

            return RedirectToAction("groups", new { camp_id = model.id_camp });
        }
        [HttpGet("campaign/group/edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            return View(_mapper.Map<EditGroup>(_groupManager.GetGroupById(id)));
        }
        [HttpPost("campaign/group/edit/sent")]
        public IActionResult EditPost(EditGroup editGroup)
        {
            if (!ModelState.IsValid)
            {
                return View(editGroup);
            }
            var group = _mapper.Map<GroupModel>(editGroup);
            _groupManager.UpdateGroup(group);
            return RedirectToAction("Index", new { id = editGroup.id });
        }
        [HttpGet("campaign/group/edit-advertisements/{id}")]
        public IActionResult EditAdvertList(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id })) return StatusCode(401);

            Guid CampId = PermissionActionManager<Guid>.GetFullPath(id).Camp;

            List<Advert> Adverts = _dataBaseProvider
                .GetAdvertRepository()
                .GetAdvertsByCampId(CampId);

            GroupModel groupModel = _groupManager.GetGroupById(id);

            AdvertGroupListView ModelView = new AdvertGroupListView() { Id_Group = id, Name_Group = groupModel.name };

            Guid[] join_adverts = _dataBaseProvider
                .GetGroupRepository()
                .GroupJoinAdvert(groupModel.id)?.one_to_many?.ToArray() ?? new Guid[0];

            ModelView.AdvertGroups = Adverts.Select(a => new AdvertGroupEdit()
            {
                Id = a.Id,
                IsAttached = join_adverts.Contains(a.Id),
                Template = a.Template,
                Name = a.Name,
                ModifiedTime = DateTime.Now,
                isActive = a.IsActive,
                Formats = a.Formats.Select(f => f.Size).ToList()

            }).ToList();

            return View(ModelView);
        }

        [HttpPost("edit-advertisements/sent")]
        public IActionResult EditAdvertListPost([FromForm] AdvertGroupListView ModelView)
        {
            if (ModelState.IsValid)
            {
                var ids_attach = ModelView.AdvertGroups.Where(ad => ad.IsAttached).Select(ad => ad.Id).ToArray();
                var ids_detach = ModelView.AdvertGroups.Where(ad => !ad.IsAttached).Select(ad => ad.Id).ToArray();
                var id_group = ModelView.Id_Group;

                if(ids_detach.Length > 0)
                    _dataBaseProvider.GetAdvertRepository().ChangeAttachStateAdverts(ids_detach, id_group, false);
                if(ids_attach.Length > 0)
                    _dataBaseProvider.GetAdvertRepository().ChangeAttachStateAdverts(ids_attach, id_group, true); 
            }
            return RedirectToAction("Index", new { id = ModelView.Id_Group });
        }

        [Authorize]
        [HttpPost("campaign/group/delete")]
        public IActionResult Delete([FromForm] Guid id)
        {
            if(id == Guid.Empty) return View();
            if (!PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { id })) return StatusCode(401);

            try
            {
               _dataBaseProvider.GetGroupRepository().DeleteGroup(id);

                Guid camp = PermissionActionManager<Guid>.GetFullPath(id).Camp;
                return RedirectToAction("Groups", new { camp_id = camp, success = ErrorForm.suc_remove.GetStringValue() });

            }
            catch(Exception)
            {
                return RedirectToAction("Index", new { id = id, error = ErrorForm.err_remove.GetStringValue() });
            }
        }

        [Authorize]
        [HttpPost("campaign/group/switch")]
        public IActionResult Switch([FromForm] Guid id)
        {
            var group = _groupManager.GetGroupById(id);

            if (group == null) return StatusCode(404);

            group.status = !group.status;

            try
            {
                _groupManager.UpdateGroup(group);
                return RedirectToAction("Index", new { id = id });

            }
            catch(Exception e)
            {
                return StatusCode(500);
            }
        }

    }
}
