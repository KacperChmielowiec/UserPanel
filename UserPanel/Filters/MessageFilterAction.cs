using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using UserPanel.Helpers;
using UserPanel.Models.Messages;
using UserPanel.Types;

namespace UserPanel.Filters
{
    public class MessageFilterAction : Attribute, IActionFilter
    {
        FormMessages FormMessages { get; set; }
        public MessageFilterAction(FormMessages messages) {

            FormMessages = messages;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
          
            if (context.HttpContext.Request.Query.TryGetValue("error", out StringValues error))
            {
                if (context.Controller is Controller controller)
                {
                    controller.ViewData["error"] = Enum.TryParse(typeof(ErrorForm), error, out _) ?
                        FormMessages.GetValue((ErrorForm)Enum.Parse(typeof(ErrorForm), error))
                        : FormMessages.GetValue(ErrorForm.err_default);
                }
            }
            else if(context.HttpContext.Request.Query.TryGetValue("success", out StringValues success))
            {
                if (context.Controller is Controller controller)
                {
                    if (Enum.TryParse(typeof(ErrorForm), success, out object succes_enum))
                    {
                        controller.ViewData["success"] = FormMessages.GetValue((ErrorForm)succes_enum);
                    }
                }
            }
        } 
    }
}
