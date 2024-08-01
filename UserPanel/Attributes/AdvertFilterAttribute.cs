using Microsoft.AspNetCore.Mvc;
using UserPanel.Filters;


namespace UserPanel.Attributes
{
    public class AdvertFilterAttribute : TypeFilterAttribute
    {
        public AdvertFilterAttribute() : base(typeof(AdvertMessagesFilterAction))
        {
        }
    }
}
