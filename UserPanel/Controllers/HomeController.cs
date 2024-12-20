using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.Dashboard;
using UserPanel.Models.Home;
using UserPanel.Models.Logs;
using UserPanel.Models.Password;
using UserPanel.Models.User;
using UserPanel.Services;


namespace UserPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager _userManger;
        private readonly CampaningManager _campaningManager;
        private readonly IDataBaseProvider _provider;
        private readonly PasswordHasher _passwordHasher;
        private readonly ConfigService _configService;
        private ILogger<LogUserEntry> _logger;
        public HomeController(IConfiguration configuration, CampaningManager campaningManager, UserManager userManager, IDataBaseProvider provider, PasswordHasher passwordHasher, ConfigService configService, ILogger<LogUserEntry> logger)
        {
            _campaningManager = campaningManager;
            _userManger = userManager;
            _provider = provider;
            _passwordHasher = passwordHasher;
            _configService = configService;
            _logger = logger;
            var p = PermissionActionManager<Guid>.InstanceContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index([FromQuery] int timerate = 7)
        {
            HomeModel model = new HomeModel();
            model.FilterParametr = new FilterParametr() {
                rate = (ButtonFilterRate)Enum.Parse(typeof(ButtonFilterRate), timerate.ToString()),
                FilterCampanings = _campaningManager.GetCampanings()
            };
            return View(model);
        }
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("/Code/{code}")]
        public int Code(int code)
        {
            return code;
        }

        [Authorize]
        [HttpGet("/create-user")]
        public IActionResult AddUser()
        {
            return Ok();
        }

        [Authorize(Policy = "admin")]
        [HttpGet("/admin/dashboard")]
        public IActionResult AdminDashboard()
        {
            List<UserModel> users = _provider
                .GetUserRepository()
                .GetAllUser()
                .FindAll(user => user.Role != UserRole.ADMIN);

            return View(users);
        }

        [Authorize]
        [HttpGet("/user/dashboard")]
        public IActionResult UserDashboard()
        {
            return View(new ResetUserPassword() {  Id = _userManger.getUserId()});
        }

        [Authorize(Policy = "admin")]
        [HttpPost("/admin/remove-user")]
        public IActionResult RemoveUser(int id)
        {
            try
            {
                _provider.GetUserRepository().DeleteUser(id);

                List<UserModel> users = _provider
                  .GetUserRepository()
                  .GetAllUser()
                  .FindAll(user => user.Role != UserRole.ADMIN);

                TempData["success"] = "the user has been removed";

                return View("AdminDashboard", users);

            }catch (Exception)
            {
                TempData["error"] = "Something have been gone wrong";

                return View("AdminDashboard", new List<UserModel>());
            }
        }

        [Authorize(Policy = "admin")]
        [HttpPost("/admin/update-state")]
        public IActionResult UpdateState(int id)
        {
            try
            {
                UserModel userModel = _provider.GetUserRepository().GetModelById(id);

                userModel.IsActive = !userModel.IsActive;

                _provider.GetUserRepository().UpdateModel(userModel);

                List<UserModel> users = _provider
                  .GetUserRepository()
                  .GetAllUser()
                  .FindAll(user => user.Role != UserRole.ADMIN);

                TempData["success"] = "the user has been update";

                return View("AdminDashboard", users);

            }catch (Exception) {

                TempData["error"] = "Something have been gone wrong";

                return View("AdminDashboard", new List<UserModel>());
            }
        }

        [Authorize(Policy = "admin")]
        [HttpGet("/admin/dashboard/create-user")]
        public IActionResult CreateUser()
        {
            return View("DashboardAddUserForm");
        }

        [Authorize(Policy = "admin")]
        [HttpPost("/admin/dashboard/create-user-form")]
        public IActionResult CreateUserPost(CreateUser user)
        {
            if (!ModelState.IsValid)
            {
                return View("DashboardAddUserForm", user);
            }
            
            UserModel model = _provider.GetUserRepository().GetModelByEmail(user.Email);

            if(model != null)
            {
                ModelState.AddModelError("ErrorCreate", "User with given email already exists");
                return View("DashboardAddUserForm", user);
            }

            string token = string.Empty;

            if(user.Token > 0 && user.IsToken)
            {
                token = _passwordHasher.HashOneTimeToken((int)user.Token, user.Name.Length);
            }

            UserModel userModel = new UserModel()
            {
                Name = user.Name,
                Email = user.Email,
                Password = _passwordHasher.HashPassword(user.Password),
                Phone = user.Phone,
                Address = "",
                Company = "",
                Role = user.Role,
                IsActive = user.IsActive,
                Token = token
            };

            userModel.States.Add(new UserModelState() { Description = "", State = UserState.New });

            _provider.GetUserRepository().CreateUser(userModel);

            _logger.LogInformation(new EventId(0, "LogUserEntry"), JsonSerializer.Serialize(new LogUserEntry() { Action = "Create User", Timestamp = DateTime.Now, UserName = userModel.Name, UserId = userModel.Id }));
            var models = _provider.GetUserRepository().GetAllUser();

            TempData["success"] = "The user has been added";
            return View("AdminDashboard", models);
        }

        [Authorize(Policy = "admin")]
        [HttpGet("/admin/dashboard/edit-user/{id}")]
        public IActionResult EditUser([FromRoute] int id)
        {
            var user = _provider.GetUserRepository().GetModelById(id);

            if (user == null) {
                return StatusCode(404);
            }

            return View("DashboardEditUserForm", new EditUser() { 
                Id = user.Id, 
                Name = user.Name, 
                Email = user.Email, 
                IsActive = user.IsActive, 
                Password = user.Password, 
                Phone = user.Phone, 
                Role = user.Role,
                IsToken = false,
                Token = null
            });
        }

        [Authorize(Policy = "admin")]
        [HttpPost("/admin/dashboard/edit-user/post")]
        public IActionResult EditUserPost(EditUser User)
        {
            if (!ModelState.IsValid)
            {
                return View("DashboardEditUserForm", User);
            }

            var user = _provider.GetUserRepository().GetModelById(User.Id);

            if (user == null)
            {
                return StatusCode(500);
            }

            user.Email = User.Email;
            user.IsActive = User.IsActive;
            user.Password = User?.Password?.Length > 0 ? User.Password : user.Password;
            user.Phone = User.Phone;
            user.Role = User.Role;
            user.Name = User.Name;
            user.Token = User.IsToken ? (User.Token != null ? _passwordHasher.HashOneTimeToken((int)User.Token, user.Name.Length) : null) : null;

            _provider.GetUserRepository().UpdateModel(user);
            _logger.LogInformation(new EventId(0, "LogUserEntry"), JsonSerializer.Serialize(new LogUserEntry() { Action = "Edit User", Timestamp = DateTime.Now, UserName = user.Name, UserId = user.Id }));
            return RedirectToAction("AdminDashboard");   
        }

        [Authorize(Policy = "admin")]
        [HttpGet("/admin/dashboard/settings")]
        public IActionResult Settings()
        {
            int idAdmin = _userManger.getUserId();

            return View("DashboardAdminSettings", new ResetAdminPassword() { Id = idAdmin});
        }

        [Authorize(Policy = "admin")]
        [HttpPost("/admin/dashboard/settings/post")]
        public IActionResult SettingsAdminPost([FromForm] ResetAdminPassword resetAdminPassword)
        {

            if (!ModelState.IsValid)
            {
                return View("DashboardAdminSettings", resetAdminPassword);
            }

            int idAdmin = _userManger.getUserId();

            if(idAdmin != resetAdminPassword.Id) return Unauthorized();

            var admin = _provider.GetUserRepository().GetModelById(idAdmin);

            if(admin == null) {

                return StatusCode(500);
            }

            if(!_passwordHasher.VerifyHashedPassword(admin.Password,resetAdminPassword.OldPassword))
            {
                ModelState.AddModelError("Error", "The Old Password is incorrect");
                return View("DashboardAdminSettings", resetAdminPassword);
            }

            admin.Password = _passwordHasher.HashPassword(resetAdminPassword.NewPassword);

            _provider.GetUserRepository().UpdateModel(admin);

            return RedirectToAction("AdminDashboard");
        }

        [Authorize]
        [HttpPost("/user/dashboard/settings/post")]
        public IActionResult SettingsUserPost([FromForm] ResetUserPassword resetUserPassword)
        {

            if (!ModelState.IsValid)
            {
                return View("UserDashboard", resetUserPassword);
            }

            int idUser = _userManger.getUserId();

            if (idUser != resetUserPassword.Id) return Unauthorized();

            var user = _provider.GetUserRepository().GetModelById(idUser);

            if (user == null)
            {
                return StatusCode(500);
            }


            if (!_passwordHasher.VerifyHashedPassword(user.Password, resetUserPassword.OldPassword))
            {
                ModelState.AddModelError("Error", "The Old Password is incorrect");
                return View("DashboardAdminSettings", resetUserPassword);
            }

            if (_userManger.CheckPasswordInHistory(resetUserPassword.NewPassword, resetUserPassword.Id))
            {
                ModelState.AddModelError("Error", "The password already have been set previously");
                return View("UserDashboard", resetUserPassword);
            }

            var UserRepo = _provider.GetUserRepository();
            string oldPassword = user.Password;

            user.ResetTokenExpiry = null;
            user.ResetToken = null;
            user.Password = _passwordHasher.HashPassword(resetUserPassword.NewPassword);

            if (user.isNewUser() || user.isExpiredPass())
            {
                UserRepo.UpdateUserStates(user.Id, new UserState[] { UserState.New, UserState.ExpiredPassword }, true);
            }

            UserRepo.UpdateModel(user);
            UserRepo.AddPasswordToHistory(user.Id, oldPassword);

            TempData["Success"] = "The password have been reset successfull";

            _provider.GetUserRepository().UpdateModel(user);

            return RedirectToAction("UserDashboard");
        }

        [Authorize(Policy = "admin")]
        [HttpPost("/set/protection-form")]
        public IActionResult ProtectionPost(PasswordProtectionForm form)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Settings");
            }

            ProtectionFormWorker.HandleProtectionForm(form, _provider);

            int idAdmin = _userManger.getUserId();

            TempData["success"] = "The Password protection have been changed";

            return View("DashboardAdminSettings", new ResetAdminPassword() { Id = idAdmin });
        }

        [HttpGet("/generate/one-time-token")]
        public IActionResult GenerateOneTimeToken([FromQuery] string returnUrl = "/", [FromQuery] int len = 6 )
        {
            int token = _passwordHasher.OneTimeTokenGenerate(len);

            TempData["token"] = token;

            return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult ExportLogs()
        {
            // Pobieranie danych z DbContext (np. listy użytkowników)
            var data = _provider.GetUserRepository().GetAllUserLogs()
                .Select(log => new
                {
                    Name = log.UserName,
                    Date = log.Timestamp,
                    Action = log.Action
                });
            // Konwersja do CSV
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Name,Date,Action"); // Nagłówki

            foreach (var item in data)
            {
                csvBuilder.AppendLine($"{item.Name},{item.Date},{item.Action}");
            }

            // Konwersja CSV na strumień
            var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            var csvStream = new System.IO.MemoryStream(csvBytes);

            // Zwracanie pliku CSV
            return File(csvStream, "text/csv", "export.csv");
        }

        [HttpPost]
        public IActionResult UpdateMaxTimeInActiveUser([FromForm] SessionSettingsModel model)
        {
            _configService.UpdateSetting(new string[] { "ENVIROMENT" }, "MaxInActiveTimeSession", model.MaxTimeSession);
            return RedirectToAction("Settings");
        }

    }
}