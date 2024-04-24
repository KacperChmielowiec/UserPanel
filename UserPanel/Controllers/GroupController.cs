using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Interfaces;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    [Authorize]
    [Route("/groups")]
    public class GroupController : Controller
    {
        private GroupManager _groupManager;
        private IMapper _mapper;
        public GroupController(GroupManager groupManager, IMapper mapper) 
        {
            _groupManager = groupManager;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet("/group/details/{id}")]
        public IActionResult Index(Guid id)
        {
          var group = _groupManager.GetGroupsByID(id);
         
          return View(group);
        }
        [Authorize]
        [HttpGet("/groups")]
        public IActionResult Groups([FromQuery(Name="camp_id")] Guid id)
        {
            var groups = _groupManager.GetGroupsByCampID(id);
            return View(groups);
        }
        [HttpGet("edit/{id}")]
        public IActionResult Edit(Guid id)
        {
            return View(_mapper.Map<EditGroup>(_groupManager.GetGroupsByID(id)));
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
    }
}
