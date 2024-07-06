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
     
            Actions[UserActionType.Login] = UserLoginNotify;
            Actions[UserActionType.Logout] = UserLogoutNotify;
        }

        public override void notify(UserActionMessage context)
        {
            Actions[context.ActionType](context);
        }

        private void UserLoginNotify(UserActionMessage context)
        {
          
        }
        private void UserLogoutNotify(UserActionMessage context)
        {
            
        }
    }
}
