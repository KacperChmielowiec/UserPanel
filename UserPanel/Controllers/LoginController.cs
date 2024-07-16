using UserPanel.Services;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Models.User;
using UserPanel.Interfaces;

namespace UserPanel.Controllers
{
    public class LoginController : Controller
    {   
        private IDataBaseProvider _dataBaseProvider { get; set; }
        private UserManager _userManager;
        private PasswordHasher _hasher;
        private IConfiguration _configuration;

        string invalidPasswordMessage = "";
        string notFoundMessage = "";
        string notActiveMessage = "";
        public LoginController(IDataBaseProvider dataBase, UserManager userManager, PasswordHasher hasher, IConfiguration configuration)
        {
            _dataBaseProvider = dataBase;
            _userManager = userManager;
            _hasher = hasher;
            _configuration = configuration;

            invalidPasswordMessage = _configuration["messages:loginMessages:InvalidPassword"];
            notFoundMessage = _configuration["messages:loginMessages:NotFound"];
            notActiveMessage = _configuration["messages:loginMessages:NotActive"];
        }

        
        [HttpGet]
        public IActionResult Index([FromQuery(Name = "ReturnUrl")] string? ReturnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated) return Redirect("/");
            return View(new LoginModel() { ReturnUrl = ReturnUrl ?? "/"});
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await _userManager.SignOut();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Index([FromForm] LoginModel loginModel)
        {
            if (!ModelState.IsValid) return View(loginModel);
           
            var result = true;

            UserModel userModel = _dataBaseProvider
                .GetUserRepository()
                .GetModelByEmail(loginModel.Email);

            if (userModel == null)
            {
                ModelState.AddModelError("LoginError", notFoundMessage);
                return View(loginModel);
            }
            if (!_hasher.VerifyHashedPassword(userModel?.Password ?? "",loginModel.Password) && result)
            {
                ModelState.AddModelError("LoginError", invalidPasswordMessage);
                result = false;
            }
            if(!userModel.IsActive && result)
            {
                ModelState.AddModelError("LoginError", notActiveMessage);
                result = false;
            }
            if (result) {

                await _userManager.SignIn(userModel);
                return Redirect(loginModel.ReturnUrl);

            }
                
            return View(loginModel);
            
        }

    }
}