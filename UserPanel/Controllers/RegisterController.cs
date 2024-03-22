using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using UserPanel.Helpers;
using UserPanel.Models;
using UserPanel.Models.User;
using UserPanel.Providers;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    public class RegisterController : Controller
    {
        UserManager UserManager;
        IConfiguration _configuration;
        EmailService _emailService;
        IMapper _mapper;
        public RegisterController(UserManager userManager, IConfiguration configuration, IMapper mapper) { 
        
            this.UserManager = userManager;
            this._configuration = configuration;
            this._mapper = mapper;  
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegisterModel { ReturnUrl = "/" });
        }
        [HttpGet("Register/Confirmation")]
        public IActionResult Confirm([FromQuery(Name = "code")] string? code, [FromQuery(Name = "email")] string? email)
        {
            if (code == null || email == null) return NotFound();
            string password = HttpContext.Session.GetString("pass") ?? "";
            if (UserManager.VerifyEmailToken(email,password, code)){
                return View("Confirm", true);
            }

            return View("Confirm", false);
        }

        [HttpGet("Register/finish")]
        public IActionResult Finish()
        {
            return View("Finish",HttpContext.Request.Query["email"].ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] RegisterModel registerModel)
        {
            if (!ModelState.IsValid) return View(registerModel);

            var EmailExists = this.UserManager.UserExistsByEmail(registerModel.Email);
            var PhoneExists = this.UserManager.UserExistsByPhone(registerModel.PhoneNumber);


            if (EmailExists || PhoneExists)
            {

                if (EmailExists)
                {
                    ModelState.AddModelError("RegisterError", "User with giving Email already exists");
                }
                if (PhoneExists)
                {
                    ModelState.AddModelError("RegisterError", "User with giving Phone number already exists");
                }
                
                return View(registerModel);
            }

            UserManager.SendEmailVerify(_mapper.Map<UserModel>(registerModel));

            return RedirectToAction("Finish", new { email = registerModel.Email });

        }

    }
}

