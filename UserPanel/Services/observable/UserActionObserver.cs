using UserPanel.Models;
using UserPanel.Providers;
using UserPanel.Types;

namespace UserPanel.Services.observable
{
    public class UserActionObserver : Observer<UserActionMessage>
    {
        public Dictionary<UserActionType,Action<UserActionMessage>> Actions { get; private set; } = new Dictionary<UserActionType, Action<UserActionMessage>>();
        private PermissionContextActions _permissionContextActions;
        public UserActionObserver(Subject<UserActionMessage> _subject, PermissionContextActions permissionContextActions) : base(_subject)
        {
            _permissionContextActions= permissionContextActions;
            Actions[UserActionType.Login] = UserLoginNotify;
        }

        public override void notify(UserActionMessage context)
        {
            Actions[context.ActionType](context);
        }

        private void UserLoginNotify(UserActionMessage context)
        {
          _permissionContextActions
                .UpdatePermissionContextUser(new PermissionContext() { contextUserID = context.ID, IsLogin = context.IsLogin });
        }
    }
}
