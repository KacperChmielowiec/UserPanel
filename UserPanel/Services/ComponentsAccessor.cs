using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Reflection;
using UserPanel.Helpers;
using UserPanel.Models;
using UserPanel.References;

namespace UserPanel.Services
{
    public class ComponentsAccessor
    {
       private static ComponentsAccessor Instance;
       private ComponentsAccessor() {
       }
       public static ComponentsAccessor GetInstance() {

            if (Instance == null)
            {
                return new ComponentsAccessor();
            }
            else
            {
                return Instance;
            }
       }
       public bool isVisible(HttpContext context, string components)
       {
           bool visible = true;
           ComponentsDescriptor descriptor = ConfigurationHelper.config.GetSection("components:" + components).Get<ComponentsDescriptor>();
           if(descriptor == null) { return false; }
           if (context == null) return false;

           descriptor.name = components;
           if (descriptor.isAuth)
            { 
                visible = context.User.Identity.IsAuthenticated ? true : false;
                visible = Enumerable.Range(0, descriptor.Roles.Length)
                    .Select(x => context.User.IsInRole(Enum.GetName(typeof(UserRole),descriptor.Roles[x])))
                    .FirstOrDefault(x => x == true) ? true : false;
           }
           if(descriptor.forbiddenPath.Contains("*"))
                visible = descriptor.forbiddenPath.Contains(context.Request.Path.Value.ToLower());
           else
                visible = !descriptor.forbiddenPath.Contains(context.Request.Path.Value.ToLower());

            return visible;
       }
        
    }
}
