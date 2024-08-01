using UserPanel.Models.Messages;

namespace UserPanel.Filters
{
    public class AdvertMessagesFilterAction : MessageFilterAction
    {
        public AdvertMessagesFilterAction(AdvertMessages messages, bool inherit = false) : base(messages, inherit)
        {
        }
    }
}
