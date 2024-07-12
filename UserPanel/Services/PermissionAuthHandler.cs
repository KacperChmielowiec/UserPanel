using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using UserPanel.Helpers;
using System.Text.Encodings.Web;
using UserPanel.Models;
using System.Threading.Tasks;
using UserPanel.Interfaces;
using Microsoft.AspNetCore.Http;
using UserPanel.References;
using System.Linq;

namespace UserPanel.Services
{
    public class PermissionAuthHandler : CookieAuthenticationHandler
    {
        private IHttpContextAccessor _contextAccessor;
        private List<EndpointMetaData> _endpoints;
        public PermissionAuthHandler(
            IOptionsMonitor<CookieAuthenticationOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IHttpContextAccessor httpContextAccessor,
            IOptions<List<EndpointMetaData>> optionsEndpoint
        ) : base(options, logger, encoder, clock)
        {
            _contextAccessor = httpContextAccessor;
            _endpoints = optionsEndpoint.Value;
           
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var context = _contextAccessor.HttpContext;
            var result = await base.HandleAuthenticateAsync();

            if (result.Succeeded)
            {
                if(!PermissionActionManager<Guid>.Inited)
                {
                    var userIdClaim = result.Ticket.Principal.Claims
                        .Where(p => p.Type == AppReferences.UserIdClaim)
                        .Select(p => p.Value)
                        .DefaultIfEmpty("")
                        .FirstOrDefault();

                    if (int.TryParse(userIdClaim, out int id))
                    {
                        PermissionUtils.LoadContext(context, id);
                    }
                }

                var endpoint = context.GetEndpoint();
               
                EndpointNameAttribute endpointNameAttribute = endpoint?.Metadata?.GetMetadata<EndpointNameAttribute>();
                if (endpointNameAttribute != null)
                {
                    EndpointMetaData matched = _endpoints?.Where(end => end.permission == true)
                        ?.Where(e => e.name + ":" + e.method.ToLower() == endpointNameAttribute.EndpointName).FirstOrDefault();

                    if (matched != null)
                    {
                        bool res = context.ControlAccess<Guid>(matched);
                        if (!res)
                        {
                            return AuthenticateResult.Fail("");
                        }
                    }
                }
            }
            return result;
        }
   
    }
}
