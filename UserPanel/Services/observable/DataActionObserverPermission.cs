using UserPanel.Models;
using UserPanel.Types;
using UserPanel.References;
using System.Diagnostics.Metrics;
namespace UserPanel.Services.observable
{
    public class DataActionObserverPermission : Observer<DataActionMessage>
    {

        public Dictionary<DataActionType, Action<DataActionMessage>> Actions { get; private set; } = new Dictionary<DataActionType, Action<DataActionMessage>>();
        public DataActionObserverPermission(Subject<DataActionMessage> _subject) : base(_subject)
        {
            Actions[DataActionType.ADD] = OnCreate;
            Actions[DataActionType.REMOVE] = OnDelete;
            Actions[DataActionType.UPDATE] = OnUpdate;
            Actions[DataActionType.DETACH] = OnDetachAdvert;
            Actions[DataActionType.ATTACH] = OnAttachAdvert;
        }

        public override void notify(DataActionMessage context)
        {
            Actions[context.actionType](context);
        }
        private void OnCreate(DataActionMessage context)
        {
            switch (context.dataType)
            {
                case DataType.Campaning:
                    OnCreateCamp(context); break;
                case DataType.Advert:
                    OnCreateAd(context); break;
                case DataType.Group:
                    OnCreateGroup(context); break;
                default:
                    break;
            }
        }
        private void OnDelete(DataActionMessage context)
        {
            switch (context.dataType)
            {
                case DataType.Campaning:
                    OnDeleteCamp(context);
                    break;
                case DataType.Group:
                    OnDeleteGroup(context);
                    break;
                default:
                    break;
            }
        }
        private void OnUpdate(DataActionMessage context)
        {

        }
        private void OnCreateCamp(DataActionMessage contex)
        {
            PermissionActionManager<Guid>.AddNode(contex.id, ContextNodeType.Camp);
        }
        private void OnCreateAd(DataActionMessage contex)
        {
            PermissionActionManager<Guid>.AddNode(contex.id, contex.Parent, ContextNodeType.Advert);
        }
        private void OnDeleteCamp(DataActionMessage contex)
        {
            PermissionActionManager<Guid>.RemoveNode(contex.id);
        }
        private void OnCreateGroup(DataActionMessage contex)
        {
            PermissionActionManager<Guid>.AddNode(contex.id, contex.Parent, ContextNodeType.Group);
        }
        private void OnDeleteGroup(DataActionMessage contex)
        {
            PermissionActionManager<Guid>.RemoveNode(contex.id);

        }
        private void OnAttachAdvert(DataActionMessage contex)
        {
            PermissionActionManager<Guid>.AttachNode(contex.id, contex.Parent);
        }
        private void OnDetachAdvert(DataActionMessage context)
        {
            PermissionActionManager<Guid>.DetachNode(context.id, context.Parent);
        }
    }
}
