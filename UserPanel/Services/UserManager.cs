using UserPanel.Models.User;
using System.Security.Claims;
using UserPanel.References;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using UserPanel.Interfaces;
using UserPanel.Providers;
namespace UserPanel.Services
{
    public class UserManager
    {
     
        private SignInService _signInService;
        private DataBaseProvider _provider;
        public UserManager(SignInService signInService, DataBaseProvider provider) {
            this._signInService = signInService;
            this._provider = provider;
            InitStrategy();
        }


        private Dictionary<AppReferences.UserRole, Func<UserModel, string, Task>> strategy = new Dictionary<AppReferences.UserRole, Func<UserModel, string, Task>>();

        private void InitStrategy()
        {
            strategy.Add(AppReferences.UserRole.USER, async (userModel, scheme) => await SignInAsUser(userModel, scheme));
            strategy.Add(AppReferences.UserRole.ADMIN, async (userModel, scheme) => await SignInAsAdmin(userModel, scheme));
        }
        private async Task SignInAsUser(UserModel userModel, string scheme)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(AppReferences.UserRole), userModel.Role)));
            claims.Add(new Claim("Id", userModel.Id.ToString()));
            ClaimsIdentity identity = new ClaimsIdentity(claims, scheme);
            var principal = new ClaimsPrincipal(identity);
            await _signInService.SignIn(principal);

        }

        public bool UserExistsByEmail(string email)
        {
            return this._provider.GetUserRepository().GetModelByEmail(email) != null ? true : false;
        }

        public bool UserExistsByPhone(string phone)
        {
            return false;
        }

        public async Task SignIn(UserModel userModel, string scheme = CookieAuthenticationDefaults.AuthenticationScheme)
        {
            await strategy[userModel.Role](userModel,scheme);
        }

        private async Task SignInAsAdmin(UserModel userModel, string scheme)
        {

        }



    }
}
