using UserPanel.Models;
using UserPanel.Types;
using UserPanel.Types;
namespace UserPanel.Services.observable
{
    public class GroupActionObserver : Observer<GroupActionMessage>
    {
        private PermissionContextActions permissionContextActions;
        public Dictionary<GroupActionType, Action<GroupActionMessage>> Actions { get; private set; } = new Dictionary<GroupActionType, Action<GroupActionMessage>>();
        public GroupActionObserver(Subject<GroupActionMessage> _subject, PermissionContextActions permissionContextActions) : base(_subject)
        {
            this.permissionContextActions = permissionContextActions;
            Actions[GroupActionType.ADD] = OnCreate;
        }

        public override void notify(GroupActionMessage context)
        {
            Actions[context.actionType](context);
        }
        private void OnCreate(GroupActionMessage context)
        {
            permissionContextActions.UpdatePermissionContextGroup(new GroupContext() { id = context.id });
        }
    }
}
