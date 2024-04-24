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
        private static PermissionContext _permissionContext;
        public PermissionContextProvider(IDataBaseProvider provider, IHttpContextAccessor accesor) { 
            _provider = provider;
            _contextAccessor = accesor;
        }
        public PermissionContext GetPermissionContext() {

            Console.WriteLine("TEST2");
            PermissionContext PermContext = new PermissionContext();
            _permissionContext = PermContext;
            PermContext.contextUserID = UserManager.getUserId(_contextAccessor);
            PermContext.IsLogin = UserManager.isLogin(_contextAccessor);
            if(PermContext.IsLogin)
            {

            }
            
            return PermContext;
        }
        public PermissionContextActions GetPermissionContextActions()
        {
            Console.WriteLine("TEST1");
            if (_permissionContext == null) throw new InvalidOperationException("");
            return new PermissionContextActions(_permissionContext,_provider);
        }
        public UserActionSubject GetUserActionSubject()
        {
            Console.WriteLine("TEST");
            var subject = new UserActionSubject();
            subject.attach(new UserActionObserver(subject, this));
            return subject;
        }
    

    }
}
