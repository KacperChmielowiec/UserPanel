using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using UserPanel.Models;
using UserPanel.Types;
using UserPanel.Services.observable;

namespace UserPanel.Services
{
    public class SignInService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private UserActionSubject _userActionObserver;
        public SignInService(IHttpContextAccessor httpContextAccessor, UserActionSubject UserSubject) { 
            _httpContextAccessor = httpContextAccessor;
            _userActionObserver = UserSubject;
        }

        public async Task SignIn(ClaimsPrincipal principal, string scheme = CookieAuthenticationDefaults.AuthenticationScheme)
        {
            await _httpContextAccessor.HttpContext.SignInAsync(scheme, principal);
            _userActionObserver.notify(new UserActionMessage() 
            { 
                ActionType = UserActionType.Login, 
                IsLogin = true, 
                ID =  int.Parse(principal.Claims.First(c => c.Type == "Id")?.Value ?? "-1" )
            });
        }
        public async Task SignOut()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
            _userActionObserver.notify(new UserActionMessage()
            {
                ActionType = UserActionType.Login,
                IsLogin = false,
            });
        }

    }
}
