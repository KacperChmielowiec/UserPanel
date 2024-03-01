using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UserPanel.Models.User;
using UserPanel.Providers;

namespace UserPanel.Controllers
{
    public class RegisterController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginModel() { ReturnUrl = "/" });
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] LoginModel loginModel)
        {
             return View();
        }

    }
}

