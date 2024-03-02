using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Web;
using UserPanel.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Security.Claims;
using UserPanel.Models;
using UserPanel.Models.User;
using UserPanel.Providers;
using UserPanel.References;
using UserPanel.Helpers;

namespace UserPanel.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataBaseProvider DataBaseProvider { get; set; }
        private UserManager UserManager;
        private PasswordHasher Hasher;
        private EmailService EmailService;

        private static string INVALID_USER = ConfigManager.GetConfig("appConfig.messages.loginMessages.InvalidPassword").ToString();
        private static string NOT_FOUND = ConfigManager.GetConfig("appConfig.messages.loginMessages.NotFound").ToString();

        public LoginController(ILogger<HomeController> logger, DataBaseProvider dataBase, UserManager userManager, PasswordHasher hasher, EmailService emailService)
        {
            _logger = logger;
            DataBaseProvider = dataBase;
            UserManager = userManager;
            Hasher = hasher;
            EmailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginModel() { ReturnUrl = "/"});
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] LoginModel loginModel)
        {
            if (!ModelState.IsValid) return View(loginModel);
           
            var result = true;



            UserModel userModel = DataBaseProvider
                .GetUserRepository()
                .GetModelByEmail(loginModel.Email);

            if (userModel == null)
            {
                ModelState.AddModelError("LoginError", NOT_FOUND);
                result = false;
            }
            if (!Hasher.VerifyHashedPassword(userModel?.Password ?? "",loginModel.Password) && result)
            {
                ModelState.AddModelError("LoginError", INVALID_USER);
                result = false;
            }
            if (result) {

                string token = TokenHasher.HashToken(ConfigurationHelper.config["EmailSalt"], loginModel.Email);
                string link = new LinkBuilder(HttpContext).GenerateConfirmEmailLink(token,loginModel.Email);
                Email email = new Email(new List<string>() { "kacperc317@gmail.com" },"TEST",link);
                EmailService.SendEmail(email);

                await UserManager.SignIn(userModel);
                return Redirect(loginModel.ReturnUrl);

            }
       
            return View(loginModel);
            
        }

    }
}