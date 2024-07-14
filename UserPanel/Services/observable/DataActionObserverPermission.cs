using UserPanel.Models;
using UserPanel.Types;
using UserPanel.Types;
using UserPanel.References;
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
        }

        public override void notify(DataActionMessage context)
        {
            Actions[context.actionType](context);
        }
        private void OnCreate(DataActionMessage context)
        {
            switch(context.dataType) {
                case DataType.Campaning:
                    OnCreateCamp(context); break;
                case DataType.Advert:
                    OnCreateAd(context); break;
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
    }
}
