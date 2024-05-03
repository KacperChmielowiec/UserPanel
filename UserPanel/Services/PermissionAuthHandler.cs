using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using UserPanel.Helpers;
using System.Text.Encodings.Web;
using UserPanel.Models;
using System.Threading.Tasks;
using UserPanel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace UserPanel.Services
{
    public class PermissionAuthHandler : CookieAuthenticationHandler
    {
        private IHttpContextAccessor _contextAccessor;
        private List<EndpointMetaData> _endpoints;
        private PermissionContext _permissionContext;
        private IDataBaseProvider _dataBaseProvider;
        public PermissionAuthHandler(
            IOptionsMonitor<CookieAuthenticationOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock,
            IHttpContextAccessor httpContextAccessor,
            IOptions<List<EndpointMetaData>> optionsEndpoint,
            PermissionContext permissionContext,
            IDataBaseProvider dataBaseProvider
    
        ) : base(options, logger, encoder, clock)
        {
            _contextAccessor = httpContextAccessor;
            _endpoints = optionsEndpoint.Value;
            _permissionContext = permissionContext;
            _dataBaseProvider = dataBaseProvider;
           
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var context = _contextAccessor.HttpContext;
            var result = await base.HandleAuthenticateAsync();

            if (result.Succeeded)
            {
                if (!_permissionContext.IsLoad) _permissionContext.LoadContext(_dataBaseProvider, result.Ticket);
                var endpoint = context.GetEndpoint();
               
                EndpointNameAttribute endpointNameAttribute = endpoint?.Metadata?.GetMetadata<EndpointNameAttribute>();
                if (endpointNameAttribute != null)
                {
                    EndpointMetaData matched = _endpoints?.Where(end => end.permission == true)
                        ?.Where(e => e.name + ":" + e.method.ToLower() == endpointNameAttribute.EndpointName).FirstOrDefault();

                    if (matched != null)
                    {
                        bool res = context.ControlAccess(matched, _permissionContext);
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
