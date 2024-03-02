using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UserPanel.Models.User;
using UserPanel.Providers;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    public class RegisterController : Controller
    {
        UserManager UserManager;
        IConfiguration _configuration;
        public RegisterController(UserManager userManager, IConfiguration configuration) { 
        
            this.UserManager = userManager;
            this._configuration = configuration;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegisterModel { ReturnUrl = "/" });
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] RegisterModel registerModel)
        {
            if (!ModelState.IsValid) return View(registerModel);

            var EmailExists = this.UserManager.UserExistsByEmail(registerModel.Email);
            var PhoneExists = this.UserManager.UserExistsByPhone(registerModel.PhoneNumber);


            if (EmailExists || PhoneExists)
            {
            
                if(EmailExists)
                {
                    ModelState.AddModelError("RegisterError", "User with giving Email already exists");
                }

                if (PhoneExists)
                {
                    ModelState.AddModelError("RegisterError", "User with giving Phone number already exists");
                }
                
                return View(registerModel);
            }



            return Redirect(registerModel.ReturnUrl);

        }

    }
}

