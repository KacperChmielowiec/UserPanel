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
           if (ConfigurationHelper.config == null) return false;

           var section = ConfigurationHelper.config.GetSection($"SectionsAccess:{components}");
           if(section == null || !section.Exists()) return true;

           ComponentsDescriptor descriptor = section.Get<ComponentsDescriptor>(); 

           if(descriptor == null) { return true; }

           if (context == null) return false;

           descriptor.name = components;

           if (descriptor.Auth)
           {
                visible = context.User?.Identity?.IsAuthenticated ?? false;
                if (!visible) return false;

                if (context.User == null) return false;

                visible = Enumerable.Range(0, descriptor.Roles.Length)
                    .Select(x => context.User.IsInRole(descriptor.Roles[x].GetStringValue()))
                    .FirstOrDefault(x => x == true) ? true : false;
                
                if (!visible) return false;
            }

           if(descriptor.TypeAccess == AppReferences.TypeAccessForbidden)
           {
                visible = !descriptor.Pages.Any(page => context.Request.Path.Value.Contains(page));
                
           }
           else
           {
                visible = descriptor.Pages.Any(page => context.Request.Path.Value.Contains(page));
           }

           return visible;
       }

        
        
    }
}
