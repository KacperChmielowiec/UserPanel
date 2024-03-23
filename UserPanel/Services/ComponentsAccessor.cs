using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Org.BouncyCastle.Asn1.Cms.Ecc;
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
           ComponentsDescriptor descriptor = ConfigurationHelper.config.GetSection($"SectionsAccess:{components}").Get<ComponentsDescriptor>();
           if(descriptor == null) { return false; }
           if (context == null) return false;

           descriptor.name = components;
           if (descriptor.Auth)
           { 
                visible = context.User.Identity.IsAuthenticated && Enumerable.Range(0, descriptor.Roles.Length)
                    .Select(x => context.User.IsInRole(Enum.GetName(typeof(UserRole), descriptor.Roles[x])))
                    .FirstOrDefault(x => x == true) ? true : false;

           }
           if(descriptor.TypeAccess == AppReferences.TypeAccessForbidden)
                visible = !descriptor.Pages.Contains(context.Request.Path.Value.ToLower()) && visible;
           else
                visible = descriptor.Pages.Contains(context.Request.Path.Value.ToLower()) && visible;

            return visible;
       }
        
    }
}
