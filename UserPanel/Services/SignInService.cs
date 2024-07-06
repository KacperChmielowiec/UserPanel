using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using UserPanel.Models;
using UserPanel.Types;
using UserPanel.Services.observable;
using UserPanel.References;

namespace UserPanel.Services
{
    public class SignInService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public SignInService(IHttpContextAccessor httpContextAccessor) { 
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SignIn(ClaimsPrincipal principal, string scheme = AppReferences.PERMISSION_SCHEME)
        {
            await _httpContextAccessor.HttpContext.SignInAsync(scheme, principal);
          
        }
        public async Task SignOut()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync();
        }

    }
}
