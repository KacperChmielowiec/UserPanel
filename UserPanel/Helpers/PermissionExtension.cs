using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Services;
using UserPanel.References;

namespace UserPanel.Helpers
{
    public static class PermissionExtension
    {
        public static Func<FullUserContext> LoadContext(IDataBaseProvider _provider, int id)
        {
            return () =>
            {
                FullUserContext fullUserContext = new FullUserContext();

                fullUserContext.UserId = id;
                fullUserContext.Campanings = _provider.GetCampaningRepository().getCampaningsByUser(fullUserContext.UserId);
                fullUserContext.Groups = _provider.GetGroupRepository().GetGroupsByUserId(fullUserContext.UserId);
                fullUserContext.Adverts = _provider.GetAdvertRepository().GetAdvertByUserId(fullUserContext.UserId);

                return fullUserContext;

            };
        }
       

    }
    public static class PrincipalValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            PermissionContext<Guid> permissionContext = context.HttpContext.RequestServices.GetRequiredService<PermissionContext<Guid>>();
            IDataBaseProvider provider = context.HttpContext.RequestServices.GetRequiredService<IDataBaseProvider>();
            var userIdClaim = context.Principal.Claims.FirstOrDefault(c => c.Type == AppReferences.UserIdClaim);

            if (userIdClaim != null)
            { 
                if(int.TryParse(userIdClaim.Value, out var userId))
                {
                    if (permissionContext.IsLoad == false)
                    {
                        permissionContext.SetupContext(PermissionExtension.LoadContext(provider, userId));
                        PermissionActionManager<Guid>.SetupInstance(permissionContext);
                        permissionContext.IsLoad = true;
                        permissionContext.IsLogin = true;
                    }
                }
            }
          

        }
        public static async Task OnSignOutValidate(CookieSigningOutContext context)
        {
            PermissionActionManager<Guid>.InstanceContext.ClearContext();
        }
    }

}
