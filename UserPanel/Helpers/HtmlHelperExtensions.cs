using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;
using UserPanel.Models;
using UserPanel.Services;

namespace UserPanel.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent AddClassIfPropertyInError<TModel, TValue>(
            this IHtmlHelper<TModel> htmlHelper,
            string Error,
            string Success,
            Expression<Func<TModel, TValue>> expression
         )
        {
            var expressionProvider = htmlHelper.ViewContext.HttpContext.RequestServices
            .GetService(typeof(ModelExpressionProvider)) as ModelExpressionProvider;

            var expressionText = expressionProvider.GetExpressionText(expression);
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData
                .TemplateInfo.GetFullHtmlFieldName(expressionText);
            var state = htmlHelper.ViewData.ModelState[fullHtmlFieldName];
            if (state == null)
            {
                return new HtmlString(Success);
            }

            if (state.Errors.Count == 0)
            {
                return new HtmlString(Success);
            }

            return new HtmlString(Error);
        }

        public static bool isVisible(this IHtmlHelper htmlHelper, string components)
        {
            return ComponentsAccessor.GetInstance().isVisible(htmlHelper.ViewContext.HttpContext, components);
        }

    }

}