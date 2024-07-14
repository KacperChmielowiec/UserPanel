using Microsoft.AspNetCore.Authentication.Cookies;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Interfaces.Abstract;
namespace UserPanel.Helpers
{
    public static class PermissionExtension
    {
        public static Func<FullContext> LoadContext(IDataBaseProvider _provider, int id)
        {
            return () =>
            {
                return _provider.GetFullContextRepository().GetContext(id);
            };
        }
       

    }
    public static class PermissionUtils
    {
        public static void LoadContext(HttpContext context, int id)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            PermissionContext<Guid> permissionContext = context.RequestServices.GetRequiredService<PermissionContext<Guid>>();
            IDataBaseProvider provider = context.RequestServices.GetRequiredService<IDataBaseProvider>();

            if (permissionContext.IsLoad == false)
            {
                permissionContext.SetupContext(PermissionExtension.LoadContext(provider, id));
                PermissionActionManager<Guid>.SetupInstance(permissionContext);
                permissionContext.IsLoad = true;
                permissionContext.IsLogin = true;
                
            }

        }
        public static async Task OnSignOutValidate(CookieSigningOutContext context)
        {
            PermissionActionManager<Guid>.ClearContext();
        }
    }

}
