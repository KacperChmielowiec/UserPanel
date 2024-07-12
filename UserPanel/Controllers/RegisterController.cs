using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.User;
using UserPanel.Services;

namespace UserPanel.Controllers
{
    public class RegisterController : Controller
    {
        UserManager _userManager;
        IConfiguration _configuration;
        IMapper _mapper;
        IDataBaseProvider _dataBaseProvider;
        EnviromentSettings _enviromentSettings;

        private string PhoneMessage = "";
        private string EmailMessage = "";
        public RegisterController(UserManager userManager, IConfiguration configuration, IMapper mapper, IDataBaseProvider dataBaseProvider, IOptions<EnviromentSettings> options) { 
        
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _dataBaseProvider = dataBaseProvider;
            _enviromentSettings = options.Value;

            PhoneMessage = _configuration["messages:RegisterMessages:PhoneExists"];
            EmailMessage = _configuration["messages:RegisterMessages:EmailExists"];
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View(new RegisterModel { ReturnUrl = "/" });
        }
        [HttpGet("Register/Confirmation")]
        public IActionResult Confirm([FromQuery(Name = "code")] string? code, [FromQuery(Name = "email")] string? email)
        {
            if (code == null || email == null) return BadRequest();
            if (_userManager.VerifyEmailToken(email,code)){

                try
                {
                    _userManager.ChangeUserState(email, true);

                }catch (KeyNotFoundException)
                {
                    return NotFound();

                }catch(ArgumentNullException)
                {
                    return NotFound();
                }
                catch(Exception)
                {
                    return StatusCode(500);
                }

                return View("Confirm", true);
            }

            return View("Confirm", false);
        }

        [HttpGet("Register/finish")]
        public IActionResult Finish([FromQuery] string email)
        {
            var FinishViewModel = new FinishPageModel();

            if (email == null)
            {
                return BadRequest();
            }
            string token = _userManager.GetVierifyEmailForUser(email);

            if(token.Length == 0) 
            {
                return NotFound();
            }
            
            FinishViewModel.HashLink = token;
            FinishViewModel.Email = email;
            FinishViewModel.Development = _enviromentSettings.EnvDevEmail;


            return View("Finish", FinishViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromForm] RegisterModel registerModel)
        {
            if (!ModelState.IsValid) return View(registerModel);

            var EmailExists = _userManager.UserExistsByEmail(registerModel.Email);
            var PhoneExists = _userManager.UserExistsByPhone(registerModel.PhoneNumber);


            if (EmailExists || PhoneExists)
            {

                if (EmailExists)
                {
                    ModelState.AddModelError("RegisterError", EmailMessage);
                }
                if (PhoneExists)
                {
                    ModelState.AddModelError("RegisterError", PhoneMessage);
                }
                
                return View(registerModel);
            }

            UserModel user = _mapper.Map<UserModel>(registerModel);
            user.Role = UserRole.USER;

            _userManager.CreateUser(user);

            _userManager.SendEmailVerify(user);

            return RedirectToAction("Finish", new { email = registerModel.Email });

        }
        [HttpGet("/users/get-all")]
        public string GetListUser()
        {
            return JsonSerializer.Serialize(_dataBaseProvider.GetUserRepository().GetAllUser());
        }
    }
}

