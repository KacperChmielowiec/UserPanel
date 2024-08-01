using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using UserPanel.Models.Messages;
using UserPanel.Types;

namespace UserPanel.Filters
{
    public class FeedMessageFilterAction : MessageFilterAction
    {
        FeedFormMessage _message;
        public FeedMessageFilterAction(FeedFormMessage messages) : base(messages,true)
        {
            _message = messages;
            _OnActionExecuting = OnActionExecuting;
        }

        public new void OnActionExecuting(ActionExecutingContext context)
        {


            if(IsError == false && IsSucces == false)
            {
                if (context.HttpContext.Request.Query.TryGetValue("error", out StringValues error))
                {
                    if (context.Controller is Controller controller)
                    {
                        if (Enum.TryParse(typeof(ErrorForm), error, out object error_enum))
                        {
                            controller.ViewData["error"] = _message.GetValue((ErrorForm)Enum.Parse(typeof(ErrorForm), error));
                        }

                    }
                }
                else if (context.HttpContext.Request.Query.TryGetValue("success", out StringValues success))
                {
                    if (context.Controller is Controller controller)
                    {
                        if (Enum.TryParse(typeof(ErrorForm), success, out object succes_enum))
                        {
                            controller.ViewData["success"] = _message.GetValue((ErrorForm)succes_enum);
                        }
                    }
                }
            }
        }
    }
}
