using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Interfaces;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    [Authorize]
    [Route("/groups")]
    public class GroupController : Controller
    {
        private GroupManager _groupManager;
        public GroupController(GroupManager groupManager) 
        {
            _groupManager = groupManager;
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

    }
}
