using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace UserPanel.Services
{
    public class SignInService
    {
        private IHttpContextAccessor _httpContextAccessor;
        public SignInService(IHttpContextAccessor httpContextAccessor) { 
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task SignIn(ClaimsPrincipal principal, string scheme = CookieAuthenticationDefaults.AuthenticationScheme)
        {
            await _httpContextAccessor.HttpContext.SignInAsync(scheme, principal);
        }
    }
}
