using UserPanel.Services;
using Microsoft.AspNetCore.Mvc;
using UserPanel.Models.User;
using UserPanel.Interfaces;
using System.Text.RegularExpressions;
using UserPanel.Models;
using UserPanel.Models.Password;
using UserPanel.Models.Logs;
using System.Text.Json;

namespace UserPanel.Controllers
{
    public class LoginController : Controller
    {   
        private IDataBaseProvider _dataBaseProvider { get; set; }
        private UserManager _userManager;
        private PasswordHasher _hasher;
        private IConfiguration _configuration;
        private ILogger<LogUserEntry> _logger;

        string invalidPasswordMessage = "";
        string notFoundMessage = "";
        string notActiveMessage = "";
        public LoginController(IDataBaseProvider dataBase, UserManager userManager, PasswordHasher hasher, IConfiguration configuration, ILogger<LogUserEntry> logger)
        {
            _dataBaseProvider = dataBase;
            _userManager = userManager;
            _hasher = hasher;
            _configuration = configuration;
            _logger = logger;

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

            var UserRepo = _dataBaseProvider
                  .GetUserRepository();

            UserModel userModel = UserRepo.GetModelByEmail(loginModel.Email);

            if (userModel == null)
            {
                ModelState.AddModelError("LoginError", notFoundMessage);
                return View(loginModel);
            }
            string timeOut = HttpContext.Session.GetString("TimeOutLogin");
            string tryCount = HttpContext.Session.GetString("LoginTimes");

            if (!string.IsNullOrEmpty(timeOut))
            {
                var lastActivityTime = DateTime.Parse(timeOut);
                if (lastActivityTime > DateTime.Now)
                {
                    ModelState.AddModelError("LoginError", "To many tries of login. Try later...");
                    return View(loginModel);
                }
                else
                {
                    HttpContext.Session.Remove("TimeOutLogin");
                }
            }

            if (!loginModel.TokenLogin && !_hasher.VerifyHashedPassword(userModel?.Password ?? "", loginModel.Password))
            {
                int tryCountParsed = 0;
                bool parsedResult = int.TryParse(tryCount, out tryCountParsed);
                if (string.IsNullOrEmpty(tryCount) && !parsedResult)
                {
                    HttpContext.Session.SetString("LoginTimes", "1");

                }
                else if(tryCountParsed > 3)
                {


                    HttpContext.Session.SetString("TimeOutLogin", DateTime.Now.AddMinutes(2).ToString());
                    _logger.LogInformation(new EventId(0, "LogUserEntry"), JsonSerializer.Serialize(new LogUserEntry() { Action = "TimeOut", Timestamp = DateTime.Now, UserName = userModel.Name, UserId = userModel.Id }));
                    ModelState.AddModelError("LoginError", "To many tries of login. Try later...");
                    return View(loginModel);

                }
                else
                {
                    HttpContext.Session.SetString("LoginTimes", $"{tryCountParsed + 1}");
                }



                ModelState.AddModelError("LoginError", invalidPasswordMessage);
                return View(loginModel);
            }

            if(loginModel.TokenLogin && ( userModel?.Token == null || !_hasher.VerifyOneTimeToken(loginModel.Password, userModel.Token)))
            {
                ModelState.AddModelError("LoginError", "Incorrect user token");
                return View(loginModel);
            }

            if (userModel.IsBlocked())
            {
                ModelState.AddModelError("LoginError", notActiveMessage);
                return View(loginModel);
            }

            if (loginModel.TokenLogin && userModel.Token != null)
            {
                userModel.Token = null;
                _dataBaseProvider.GetUserRepository().UpdateModel(userModel);
            }

            if (userModel.Role == UserRole.USER)
            {
                Func<ResetPasswordUserModel> GetResetModelWithToken = () =>
                {
                    var token = _userManager.GenerateAndSetResetTokenWichCheck(userModel, 60 * 60, false);
                    return new ResetPasswordUserModel() { idUser = userModel.Id, token = token };
                };


                if (userModel.isNewUser())
                {
                    ModelState.AddModelError("ResetError", "Change your default password.");
                    return View("ResetFormConfirm", GetResetModelWithToken.Invoke());
                }

                var PasswordTimeValue = UserRepo.GetPasswordRule(PasswordConstraintType.DaysToPasswordReset);

                if (userModel.isExpiredPass() && PasswordTimeValue?.IsEnabled == true)
                {
                    ModelState.AddModelError("ResetError", "Change your default password.");
                    return View("ResetFormConfirm", GetResetModelWithToken.Invoke());
                }

                var LastPassword = userModel
                    .PasswordHistories
                    .OrderBy(p => p.CreatedAt)
                    .LastOrDefault();

                bool timeExpired = PasswordTimeValue?.IsEnabled == true
                    && LastPassword?.CreatedAt != null
                    && (DateTime.Now - LastPassword.CreatedAt).Days > PasswordTimeValue.Value;

                if (timeExpired)
                {
                    ModelState.AddModelError("ResetError", "Yout password expired.");
                    UserRepo.UpdateUserState(userModel.Id, UserState.ExpiredPassword);
                    _dataBaseProvider.GetUserRepository().UpdateModel(userModel);
                    return View("ResetFormConfirm", GetResetModelWithToken.Invoke());
                }

            }

            await _userManager.SignIn(userModel);
            _logger.LogInformation(new EventId(0,"LogUserEntry"), JsonSerializer.Serialize(new LogUserEntry() { Action = "Login", Timestamp = DateTime.Now, UserName = userModel.Name, UserId = userModel.Id }));

            return Redirect(loginModel.ReturnUrl);
            
            
        }
        [HttpGet("/reset-password")]
        public IActionResult Reset()
        {
            return View();
        }
        [HttpPost("/reset-password-post")]
        public IActionResult ResetPost([FromForm] string email)
        {
            try
            {
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    ModelState.AddModelError("email", "Niepoprawny adres e-mail.");
                    return View("Reset");
                }

                var user = _dataBaseProvider.GetUserRepository().GetModelByEmail(email);

                if (user != null)
                {
                    _userManager.GenerateAndSetResetToken(user, 60 * 60);
                }

                TempData["Success"] = "Token weryfikacyjny został wysłany na podany email, jeśli istnieje. Sprawdź swoją poczte.";

                return View("Reset");

            }
            catch (Exception)
            {
                ModelState.AddModelError("ResetError", "Problem z przetwarzaniem żądania");
                return View("Reset");
            }
        }

        [HttpGet("/reset-passsword-update")]
        public IActionResult ResetUpdate([FromQuery] string uid, [FromQuery] string key, [FromQuery] string token)
        {
            try
            {
                string text_id = "";
                int id = 0;

                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(key))
                {
                    return BadRequest("The request not comprise all needed parameters");
                }
                try
                {
                   text_id = AesHasher.Decrypt(uid, key);

                } catch (Exception e)
                {
                    throw new InvalidDataException("");
                }

                if (!int.TryParse(text_id, out id))
                {
                    return Unauthorized();
                }
                if(!_userManager.VerifyResetToken(uid, token, key))
                {
                    return Unauthorized();
                }

                return View("ResetFormConfirm", new ResetPasswordUserModel() { idUser = id, token = token});

            }
            catch(InvalidDataException)
            {
                return BadRequest("The request not comprise all correct parrameters");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [HttpPost("reset-password-new-post")]
        public IActionResult ResetUpdatePost([FromForm] ResetPasswordUserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("ResetFormConfirm", model);
                }

                var UserRepo = _dataBaseProvider.GetUserRepository();

                UserModel modelUser = UserRepo.GetModelById(model.idUser);

                if (modelUser == null)
                {
                    return Unauthorized();
                }

                if (modelUser?.ResetToken != model.token)
                {
                    return Unauthorized();
                }

                if (modelUser.ResetTokenExpiry < DateTime.Now)
                {
                    return Unauthorized();
                }
                
                if(_hasher.VerifyHashedPassword(modelUser.Password,model.password)) {
                    ModelState.AddModelError("ResetError", "The password have to be different than current");
                    return View("ResetFormConfirm", model);
                }

                if(_userManager.CheckPasswordInHistory(model.password,model.idUser))
                {
                    ModelState.AddModelError("ResetError", "The password already have been set previously");
                    return View("ResetFormConfirm", model);
                }

                string oldPassword = modelUser.Password;

                modelUser.ResetTokenExpiry = null;
                modelUser.ResetToken = null;
                modelUser.Password = _hasher.HashPassword(model.password);

                if (modelUser.isNewUser() || modelUser.isExpiredPass())
                {
                    UserRepo.UpdateUserStates(modelUser.Id,new UserState[] { UserState.New, UserState.ExpiredPassword }, true);
                }

                UserRepo.UpdateModel(modelUser);
                UserRepo.AddPasswordToHistory(model.idUser, oldPassword);

                TempData["Success"] = "The password have been reset successfull";
                _logger.LogInformation(new EventId(0, "LogUserEntry"), JsonSerializer.Serialize(new LogUserEntry() { Action = "ResetPassword", Timestamp = DateTime.Now, UserName = modelUser.Name, UserId = modelUser.Id}));
                return View("ResetFormConfirm", model);
            }
            catch(Exception)
            {
                return StatusCode(500);
            }
        }


    }
    
}