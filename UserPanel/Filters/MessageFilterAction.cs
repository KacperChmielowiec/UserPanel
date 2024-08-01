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
        protected bool IsError { get; set; }
        protected bool IsSucces {  get; set; }
        protected bool IsInherited { get; set; }
        protected Action<ActionExecutingContext> _OnActionExecuting { get; set; }
        public MessageFilterAction(FormMessages messages, bool inherit = false)
        {

            FormMessages = messages;
            IsInherited = inherit;
            _OnActionExecuting = OnActionExecuting;
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
                    if (Enum.TryParse(typeof(ErrorForm), error, out object error_enum))
                    {
                        controller.ViewData["error"] = FormMessages.GetValue((ErrorForm)Enum.Parse(typeof(ErrorForm), error));
                    }
                    else if(IsInherited)
                    {
                        controller.ViewData["error"] = FormMessages.GetValue(ErrorForm.err_default);
                    }
                          
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
            if(IsInherited)
            {
                _OnActionExecuting(context);
            }
          
        } 
    }
}
