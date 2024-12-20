using UserPanel.Models.User;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using UserPanel.Interfaces;
using UserPanel.Helpers;
using UserPanel.Models;
using UserPanel.References;
using Microsoft.Extensions.Options;
namespace UserPanel.Services
{
    public class UserManager
    {

        private SignInService _signInService;
        private IDataBaseProvider _provider;
        private IConfiguration _configuration;
        EmailService _emailService;
        HttpContext _context;
        EnviromentSettings _enviromentSettings;
        PasswordHasher _passwordHasher;
        public UserManager(
            SignInService signInService,
            IDataBaseProvider provider,
            IConfiguration configuration,
            EmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            IOptions<EnviromentSettings> enviroment,
            PasswordHasher passwordHasher
        )
        {
            _signInService = signInService;
            _provider = provider;
            _emailService = emailService;
            _configuration = configuration;
            _context = httpContextAccessor?.HttpContext;
            _enviromentSettings = enviroment.Value;
            _passwordHasher = passwordHasher;
            InitStrategy();
        }

        private Dictionary<UserRole, Func<UserModel, string, Task>> strategy = new Dictionary<UserRole, Func<UserModel, string, Task>>();

        private void InitStrategy()
        {
            strategy.Add(UserRole.USER, async (userModel, scheme) => await SignInAsUser(userModel, scheme));
            strategy.Add(UserRole.ADMIN, async (userModel, scheme) => await SignInAsUser(userModel, scheme));
        }
        private async Task SignInAsUser(UserModel userModel, string scheme)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role,userModel.Role.GetStringValue()),
                new Claim(AppReferences.UserIdClaim, userModel.Id.ToString())
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);
            await _signInService.SignIn(principal);

        }
        public bool VerifyEmailToken(UserModel userModel, string token)
        {
            return TokenHasher.HashToken(_configuration[AppReferences.EmailSalt], userModel.Email + ":" + userModel.Name) == token;
        }
        public bool VerifyEmailToken(string email, string token)
        {
            UserModel user = _provider.GetUserRepository().GetModelByEmail(email);
            if (user == null) return false;

            return TokenHasher.HashToken(_configuration[AppReferences.EmailSalt], email + ":" + user.Name) == token;
        }
        public bool CheckTokenExpiry(UserModel userModel)
        {
            if(userModel == null) return false;
            if(userModel.ResetToken == null) return true;
            if(userModel.ResetTokenExpiry == null) return true;
            if(userModel.ResetTokenExpiry < DateTime.Now) return true;
            return false;
        }
        public string GenerateAndSetResetTokenWichCheck(UserModel userModel, int expirte_time_s, bool email = true)
        {
            if(CheckTokenExpiry(userModel))
            {
                return GenerateAndSetResetToken(userModel,expirte_time_s, email);
            }
            else
            {
                return userModel.ResetToken;
            }
        }
        public string GenerateAndSetResetToken(UserModel userModel, int expirte_time_s, bool email = true)
        {

            try
            {
                if (userModel == null || userModel.Password == null) throw new ArgumentNullException();

                string salt = _passwordHasher.GetSaltUser(userModel.Password);

                string token = Convert.ToBase64String(TokenHasher.GenerateRandomBytes(15));

                token = TokenHasher.HashToken(salt, token);

                DateTime expireTime = DateTime.Now.AddSeconds(expirte_time_s);

                userModel.ResetToken = token;

                userModel.ResetTokenExpiry = expireTime;

                _provider.GetUserRepository().UpdateModel(userModel);

                if(email == true)
                    this.SendResetTokenEmail(userModel);
                
                return token;

            } catch (Exception) {

                //TODO reverse action effects 
                throw;
            }

        }

        public bool VerifyResetToken(string hashId, string token, string key)
        {
            int id = -1;
            if (int.TryParse(AesHasher.Decrypt(hashId, key), out id))
            {
                UserModel userModel = _provider.GetUserRepository().GetModelById(id);
                if (userModel == null) return false;

                if (userModel.ResetToken == null) return false;
                if (userModel.ResetToken != token) return false;
                if (userModel.ResetTokenExpiry == null) return false;
                if (userModel.ResetTokenExpiry < DateTime.Now) return false;

                return true;
            }
            else
            {
                throw new UnauthorizedAccessException("");
            }

        }

        public void SendResetTokenEmail(UserModel userModel)
        {

            if (
                userModel == null ||
                userModel.Password == null ||
                userModel.Id == 0 ||
                userModel.Email == null ||
                userModel.ResetToken == null
            )
            {
                throw new ArgumentNullException();
            }

            try
            {
                string salt = Convert.ToBase64String(AesHasher.GenerateRandomPublicKey());

                string hasedID = AesHasher.Encrypt(userModel.Id.ToString(), salt);

                string resetURL = new LinkBuilder(_context).GenerateResetPasswordLink(userModel.ResetToken, salt, hasedID);

                _emailService.SendEmail(new Email(new string[] { userModel.Email }, "reset-password-user-panel", resetURL));

            }
            catch (Exception)
            {
                throw;
            }


        }

        public bool UserExistsByEmail(string email)
        {
            return _provider.GetUserRepository().GetModelByEmail(email) != null ? true : false;
        }

        public bool UserExistsByPhone(string phone)
        {
            return false;
        }
        public void SendEmailVerify(UserModel registerModel)
        {
            string token = TokenHasher.HashToken(ConfigurationHelper.config[AppReferences.EmailSalt], registerModel.Email + ":" + registerModel.Name);
            string link = new LinkBuilder(_context).GenerateConfirmEmailLink(token, registerModel.Email);
            if (!_enviromentSettings.EnvDevEmail)
            {
                Email email = new Email(new List<string>() { registerModel.Email }, "CONFIRM YOUR ACCOUNT", link);
                _emailService.SendEmail(email);
            }

        }
        public async Task SignIn(UserModel userModel, string scheme = CookieAuthenticationDefaults.AuthenticationScheme)
        {
            await strategy[userModel.Role](userModel, scheme);
        }
        public async Task SignOut()
        {
            await _signInService.SignOut();
        }


        public bool isLogin()
        {
            return _context.User.Identity.IsAuthenticated;
        }
        public static bool isLogin(IHttpContextAccessor accessor)
        {
            return accessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        }
        public int getUserId()
        {
            int id = -1;
            int.TryParse(_context.User.FindFirst(AppReferences.UserIdClaim)?.Value, out id);
            return id;
        }
        public static int getUserId(IHttpContextAccessor accessor)
        {
            int id = -1;
            int.TryParse(accessor.HttpContext?.User?.FindFirst(AppReferences.UserIdClaim)?.Value, out id);
            return id;
        }

        public void CreateUser(UserModel user)
        {
            if (user.Password.Length < 4) throw new ArgumentNullException(nameof(user.Password));
            user.Password = _passwordHasher.HashPassword(user.Password);
            _provider.GetUserRepository().CreateUser(user);
        }

        public string GetVierifyEmailForUser(string email)
        {
            UserModel user = _provider.GetUserRepository().GetModelByEmail(email);
            if (user != null)
            {
                string token = TokenHasher.HashToken(ConfigurationHelper.config[AppReferences.EmailSalt], user.Email + ":" + user.Name);
                return new LinkBuilder(_context).GenerateConfirmEmailLink(token, email);
            }
            return "";
        }

        public void ChangeUserState(string email, bool state)
        {
            var user = _provider.GetUserRepository().GetModelByEmail(email);
            if (user == null)
            {
                throw new KeyNotFoundException(nameof(user));
            }

            user.IsActive = state;

            _provider.GetUserRepository().UpdateModel(user);
        }


        public bool CheckPasswordInHistory(string password, int userId)
        {
            if(string.IsNullOrEmpty(password))
            {
                return false;
            }

            var passwords = _provider.GetUserRepository().GetPasswordsHistory(userId);

            if (passwords is not { Count: > 0 }) { 

                return false;
            }

            return passwords.Where((p) => _passwordHasher.VerifyHashedPassword(p.Password, password)).Count() > 0;
        }
    }
}
