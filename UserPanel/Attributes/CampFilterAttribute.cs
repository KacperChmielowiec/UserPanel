using Microsoft.AspNetCore.Mvc;
using UserPanel.Filters;

namespace UserPanel.Attributes
{
    public class CampFilterAttribute : TypeFilterAttribute
    {
        public CampFilterAttribute() : base(typeof(CampMessageFilterAction))
        {
        }
    }
}
