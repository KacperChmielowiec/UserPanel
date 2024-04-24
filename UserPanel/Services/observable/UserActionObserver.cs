using UserPanel.Models;
using UserPanel.Providers;
using UserPanel.Types;

namespace UserPanel.Services.observable
{
    public class UserActionObserver : Observer<UserActionMessage>
    {
        public Dictionary<UserActionType,Action<UserActionMessage>> Actions { get; private set; } = new Dictionary<UserActionType, Action<UserActionMessage>>();
        private PermissionContextProvider _permissionContextProvider;
        public UserActionObserver(Subject<UserActionMessage> _subject, PermissionContextProvider permissionContextProvider) : base(_subject)
        {
            _permissionContextProvider = permissionContextProvider;
            Actions[UserActionType.Login] = UserLoginNotify;
        }

        public override void notify(UserActionMessage context)
        {
            Actions[context.ActionType](context);
        }

        private void UserLoginNotify(UserActionMessage context)
        {
          _permissionContextProvider
                .GetPermissionContextActions()
                .UpdatePermissionContextUser(new PermissionContext() { contextUserID = context.ID, IsLogin = context.IsLogin });
        }
    }
}
