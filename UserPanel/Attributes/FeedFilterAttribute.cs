using Microsoft.AspNetCore.Mvc;
using UserPanel.Filters;

namespace UserPanel.Attributes
{
    public class FeedFilterAttribute : TypeFilterAttribute
    {
        public FeedFilterAttribute() : base(typeof(FeedMessageFilterAction))
        {
        }
    }
}
