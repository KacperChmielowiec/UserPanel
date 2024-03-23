using UserPanel.Models.User;
using System.Security.Claims;
using UserPanel.References;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using UserPanel.Interfaces;
using UserPanel.Providers;
using UserPanel.Helpers;
using UserPanel.Models;
using NETCore.MailKit.Core;
namespace UserPanel.Services
{
    public class UserManager
    {
     
        private SignInService _signInService;
        private DataBaseProvider _provider;
        private IConfiguration _configuration;
        EmailService _emailService;
        IHttpContextAccessor HttpContextAccessor;
        public UserManager(SignInService signInService, DataBaseProvider provider, IConfiguration configuration, EmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            this._signInService = signInService;
            this._provider = provider;
            this._emailService = emailService;
            InitStrategy();
            _configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
        }


        private Dictionary<UserRole, Func<UserModel, string, Task>> strategy = new Dictionary<UserRole, Func<UserModel, string, Task>>();

        private void InitStrategy()
        {
            strategy.Add(UserRole.USER, async (userModel, scheme) => await SignInAsUser(userModel, scheme));
            strategy.Add(UserRole.ADMIN, async (userModel, scheme) => await SignInAsAdmin(userModel, scheme));
        }
        private async Task SignInAsUser(UserModel userModel, string scheme)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(UserRole), userModel.Role)));
            claims.Add(new Claim("Id", userModel.Id.ToString()));
            ClaimsIdentity identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);
            await _signInService.SignIn(principal);

        }
        public bool VerifyEmailToken(UserModel userModel,string token)
        {
            return TokenHasher.HashToken(_configuration["EmailSalt"], userModel.Email + ":" + userModel.Password) == token;
        }
        public bool VerifyEmailToken(string email, string pass, string token)
        {
            return TokenHasher.HashToken(_configuration["EmailSalt"], email + ":" + pass) == token;
        }
        public bool UserExistsByEmail(string email)
        {
            return this._provider.GetUserRepository().GetModelByEmail(email) != null ? true : false;
        }

        public bool UserExistsByPhone(string phone)
        {
            return false;
        }
        public void SendEmailVerify(UserModel registerModel)
        {
            string token = TokenHasher.HashToken(ConfigurationHelper.config["EmailSalt"], registerModel.Email + ":" + registerModel.Password);
            string link = new LinkBuilder(HttpContextAccessor.HttpContext).GenerateConfirmEmailLink(token, registerModel.Email);
            Email email = new Email(new List<string>() { registerModel.Email }, "CONFIRM YOUR ACCOUNT", link);
            _emailService.SendEmail(email);
            HttpContextAccessor.HttpContext.Session.SetString("pass", registerModel.Password);
        }
        public async Task SignIn(UserModel userModel, string scheme = CookieAuthenticationDefaults.AuthenticationScheme)
        {
            await strategy[userModel.Role](userModel,scheme);
        }
        public async Task SignOut()
        {
            await _signInService.SignOut();
        }

        private async Task SignInAsAdmin(UserModel userModel, string scheme)
        {
            throw new NotImplementedException();
        }



    }
}
