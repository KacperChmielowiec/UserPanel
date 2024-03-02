using System.Reflection;
using System.Runtime.CompilerServices;
using UserPanel.Interfaces;

namespace UserPanel.Installers
{
    public static class InstallerExtenstion
    {

        public static void InstallServices(this WebApplicationBuilder builder)
        {
            var collectionInterfaces = Assembly.GetExecutingAssembly()
                .GetExportedTypes()
                .Where(type => !type.IsAbstract && type.IsAssignableTo(typeof(Installer)))
                .Select(type => Activator.CreateInstance(type))
                .Cast<Installer>()
                .ToList();

            collectionInterfaces.ForEach(installer => installer.Install(builder));
        }
    }
}
