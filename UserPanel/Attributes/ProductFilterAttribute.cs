using Microsoft.AspNetCore.Mvc;
using UserPanel.Filters;

namespace UserPanel.Attributes
{
    public class ProductFilterAttribute : TypeFilterAttribute
    {
        public ProductFilterAttribute() : base(typeof(ProductMessageFilterAction))
        {
        }
    }
}
