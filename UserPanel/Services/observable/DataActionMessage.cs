using UserPanel.Types;
using UserPanel.References;
namespace UserPanel.Services.observable
{
    public class DataActionMessage
    {
        public Guid id;
        public Guid Parent;
        public DataActionType actionType;
        public DataType dataType;
    }
}
