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
        [HttpGet("/groups/details/{id}")]
        public IActionResult Index(Guid id)
        {
          return View();
        }
        [Authorize]
        [HttpGet("/groups")]
        public IActionResult Groups([FromQuery] Guid id)
        {
          var groups = _groupManager.GetGroupsByCampID(id);

          return View();
        }

    }
}
