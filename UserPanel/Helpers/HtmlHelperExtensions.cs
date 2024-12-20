using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;
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

        public static IHtmlContent ToggleClassByState(this IHtmlHelper helper, bool state, string active, string inactive)
        {
            return state ? new HtmlString(active) : new HtmlString(inactive);
        }

    }
    public static class ViewDataHelper
    {
     
        public static string ParseToString(this ViewDataDictionary viewData, string key, string defaultValue = "")
        {
            if (viewData == null)
            {
                throw new ArgumentNullException(nameof(viewData));
            }

            if (string.IsNullOrEmpty(key))
            {
                return "";
            }

            if (viewData.ContainsKey(key))
            {
                try
                {
                    var value = viewData[key];
                    return value?.ToString() ?? defaultValue;
                }
                catch
                {
                    // Log exception if needed
                    return defaultValue;
                }
            }

            return defaultValue;
        }
    }
}