using UserPanel.Types;
using UserPanel.Models;
namespace UserPanel.Services.observable
{
    public class CampActionObserver : Observer<CampActionMessage>
    {
        public PermissionContextActions _permissionContextActions { get; set; }
        public Dictionary<CampAction, Action<CampActionMessage>> Actions { get; private set; } = new Dictionary<CampAction, Action<CampActionMessage>>();
        public CampActionObserver(Subject<CampActionMessage> _subject, PermissionContextActions permissionContextActions) : base(_subject)
        {
            _permissionContextActions = permissionContextActions;
            Actions[CampAction.Update] = UpdateActionCamp;
        }

        public override void notify(CampActionMessage context)
        {
            Actions[context.CampAction](context);
        }
        private void UpdateActionCamp(CampActionMessage context)
        {
            _permissionContextActions.UpdatePermissionContextCamp(context.CampContext);
        }
    }
}
