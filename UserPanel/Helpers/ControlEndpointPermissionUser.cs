using Microsoft.AspNetCore.Http;
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
        public static bool CampaingDetailsControl(HttpContext httpContext)
        {
            return CheckPermissionByGuidStandard(httpContext);
        }
        [DisplayName(EndpointNames.GroupDetails)]
        public static bool GroupDetailsControl(HttpContext httpContext)
        {
          return CheckPermissionByGuidStandard(httpContext);
        }

        private static bool CheckPermissionByGuidStandard(HttpContext httpContext)
        {
            if (httpContext.Request.RouteValues.TryGetValue("id", out var id))
            {
                if (Guid.TryParse(id.ToString(), out Guid result))
                {
                    return PermissionActionManager<Guid>.CheckPermisionAccess(new Guid[] { result });
                }
            }
            return false;
        }
        public static bool ControlAccess<T>(this HttpContext httpContext, EndpointMetaData endpointMeta) where T : IComparable { 

            var method = typeof(ControlEndpointPermissionUser)
                 .GetMethods()
                 .Where(info => info.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName == $"{endpointMeta.Delegate}:{endpointMeta.method}")
                 .FirstOrDefault();

            if (method != null)
            {
                Func<HttpContext, bool> func = (Func<HttpContext, bool>)Delegate.CreateDelegate(typeof(Func<HttpContext, bool>), method);

                return func.Invoke(httpContext);
            }

            return true;
        }
    }
}
