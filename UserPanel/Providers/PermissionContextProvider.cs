using System.Runtime.CompilerServices;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Services;
using UserPanel.Services.observable;
namespace UserPanel.Providers
{
    public class PermissionContextProvider
    {
        private IDataBaseProvider _provider;
        private IHttpContextAccessor _contextAccessor;
        public PermissionContextProvider(IDataBaseProvider provider, IHttpContextAccessor accesor) { 
            _provider = provider;
            _contextAccessor = accesor;
        }
        public PermissionContext GetPermissionContext() {
            return new PermissionContext() { 
                IsLoad = false, 
                IsLogin = false, 
                contextUserID = 0, 
                CampsContext = new List<CampContext>(), 
                GroupsContext = new List<GroupContext>() 
            };
        }
        public PermissionContextActions GetPermissionContextActions(PermissionContext _permissionContext)
        {
            return new PermissionContextActions(_permissionContext,_provider);
        }
        public UserActionSubject GetUserActionSubject(PermissionContextActions permissionContextActions)
        {
            var subject = new UserActionSubject();
            subject.attach(new UserActionObserver(subject, permissionContextActions));
            return subject;
        }
        public GroupActionSubject GetGroupActionSubject(PermissionContextActions permissionContextActions)
        {
            var subject = new GroupActionSubject();
            subject.attach(new GroupActionObserver(subject,permissionContextActions));
            return subject;
        }



    }
}
