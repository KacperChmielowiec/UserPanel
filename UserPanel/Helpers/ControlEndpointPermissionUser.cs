using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel;
using System.Reflection;
using UserPanel.Models;
using UserPanel.References;

namespace UserPanel.Helpers
{
    public static class ControlEndpointPermissionUser
    {
        [DisplayName(EndpointNames.CampaningDetails)]
        public static bool CampaingDetailsControl(HttpContext httpContext, PermissionContext permission)
        {
            if(httpContext.Request.RouteValues.TryGetValue("id", out var id))
            {
                if(Guid.TryParse(id.ToString(),out Guid result))
                {
                    return permission.CampIsAllowed(result);
                }
            }
            return false;
        }
        [DisplayName(EndpointNames.GroupDetails)]
        public static bool GroupDetailsControl(HttpContext httpContext, PermissionContext permission)
        {
            if (httpContext.Request.RouteValues.TryGetValue("id", out var id))
            {
                if (Guid.TryParse(id.ToString(), out Guid result))
                {
                    return permission.GroupIsAllowed(result);
                }
            }
            return false;
        }
        public static bool ControlAccess(this HttpContext httpContext, EndpointMetaData endpointMeta, PermissionContext context)
       {
           
           var method = typeof(ControlEndpointPermissionUser)
                .GetMethods()
                .Where(info => info.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName == $"{endpointMeta.Delegate}:{endpointMeta.method}")
                .FirstOrDefault();

            if (method != null)
            {
                Func<HttpContext, PermissionContext, bool> func = 
                    (Func<HttpContext, PermissionContext, bool>)Delegate
                    .CreateDelegate(typeof(Func<HttpContext, PermissionContext, bool>), method);

                return func.Invoke(httpContext, context);
            }

            return true;
       }
    }
}
