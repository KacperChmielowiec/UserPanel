using UserPanel.Models;
using UserPanel.Providers;
using UserPanel.Types;

namespace UserPanel.Services.observable
{
    public class UserActionObserver : Observer<UserActionMessage>
    {
        public Dictionary<UserActionType,Action<UserActionMessage>> Actions { get; private set; } = new Dictionary<UserActionType, Action<UserActionMessage>>();
   
        public UserActionObserver(Subject<UserActionMessage> _subject) : base(_subject)
        {
     
            Actions[UserActionType.Login] = UserLoginNotification;
            Actions[UserActionType.Logout] = UserLogoutNotification;
            Actions[UserActionType.Create] = UserCreateNotification;
            Actions[UserActionType.Update] = UserUpdateNotification;
        }

        public override void notify(UserActionMessage context)
        {
            Actions[context.actionType](context);
        }

        private void UserLoginNotification(UserActionMessage context)
        {
          
        }
        private void UserLogoutNotification(UserActionMessage context)
        {
            
        }
        private void UserCreateNotification(UserActionMessage context)
        {

        }
        private void UserUpdateNotification(UserActionMessage context)
        {

        }
    }
}
