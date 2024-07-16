using Microsoft.AspNetCore.Mvc;
using UserPanel.Filters;
using UserPanel.Models.Messages;

namespace UserPanel.Attributes
{
    public class GroupFilterAttribute : TypeFilterAttribute
    {
        public GroupFilterAttribute() : base(typeof(GroupMessageFilterAction))
        {
        }
    }
}
