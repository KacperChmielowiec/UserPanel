using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MimeKit.Text;
using NuGet.Packaging;
using System.Text;
using UserPanel.Models.Password;

namespace UserPanel.Helpers
{
    [HtmlTargetElement("input-protection", Attributes = "protection-type, text-class, check-class")]
    public class PasswordProtectionInputHelper : TagHelper
    {

        public delegate void RendererDelegate(TagBuilder tagBuilder, PasswordProtectionInputHelper Instance);

        [HtmlAttributeName("text-class")]
        public string TextClass { get; set; }

        [HtmlAttributeName("check-class")]
        public string CheckClass { get; set; }

        [HtmlAttributeName("protection-type")]
        public PasswordConstraintType ProtectionType { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "value-")]
        public Dictionary<string, string> ValueAttributes { get; set; } = new Dictionary<string, string>();


        static RendererDelegate TextCheckRenderer = (builder, Instance) =>
        {

            var meta =  Instance.ProtectionType.GetPasswordConstraintMetadata();

            var wrapper = new TagBuilder("div");
            var inputTag = new TagBuilder("input");
           
            inputTag.Attributes["type"] = meta.InputTypes[0];
            inputTag.Attributes["name"] = meta.InputNames[0];

            if (meta.InputTypes[0] == "checkbox")
            {
                if (Instance.ValueAttributes.Values.FirstOrDefault()?.ToLower() == "true")
                {
                    inputTag.Attributes["checked"] = "checked";
                }
                inputTag.Attributes["data-role"] = "checkbox";
                inputTag.Attributes["data-caption"] = meta.InputLabels[0];

                wrapper.AddCssClass(Instance.CheckClass);
            }
            else
            {
                inputTag.Attributes["value"] = Instance.ValueAttributes.Values.FirstOrDefault() ?? "";
                inputTag.AddCssClass(Instance.TextClass);
            }



            wrapper.InnerHtml.AppendHtml(inputTag);

            builder.InnerHtml.AppendHtml(wrapper);

        };

        static RendererDelegate LengthRenderer = (builderTag, Instance) =>
        {
            var meta = Instance.ProtectionType.GetPasswordConstraintMetadata();

            var builder = new StringBuilder();

            builder.Append("<div>");
            builder.Append("<p class='py-2 font-semibold text-lg'>Password Length constraint</p>");
            // Generowanie minimalnego inputu
            builder.Append("<div class='flex gap-x-4'>");

            builder.Append("<div>");
            builder.AppendFormat("<label>{0}</label>", meta.InputLabels[0]);
            builder.AppendFormat("<input type='{0}' name='{1}' value='{2}' class='{3}' />",
                                 meta.InputTypes[0],
                                 meta.InputNames[0],
                                 "",
                                 Instance.TextClass
                                 );
            builder.Append("</div>");

            // Generowanie maksymalnego inputu
            builder.Append("<div>");
            builder.AppendFormat("<label>{0}</label>", meta.InputLabels[1]);
            builder.AppendFormat("<input type='{0}' name='{1}' value='{2}' class='{3}' />",
                                 meta.InputTypes[1],
                                 meta.InputNames[1],
                                 "",
                                 Instance.TextClass);
            builder.Append("</div>");
            builder.Append("</div>");
            builder.Append("</div>");
            // Konwersja na string, gotowy HTML
            var finalHtml = builder.ToString();
            builderTag.InnerHtml.AppendHtml(finalHtml);

        };

        Dictionary<PasswordConstraintType, RendererDelegate> Renderer = new Dictionary<PasswordConstraintType, RendererDelegate>()
        {
            { PasswordConstraintType.DaysToPasswordReset, TextCheckRenderer },
            { PasswordConstraintType.ContainsSpecialCharacter, TextCheckRenderer },
            { PasswordConstraintType.ContainsUppercaseLetter, TextCheckRenderer },
            { PasswordConstraintType.ContainsDigitNotAtStart, TextCheckRenderer },
            { PasswordConstraintType.NoRepeatingCharacters, TextCheckRenderer  },
            { PasswordConstraintType.LengthBetween, LengthRenderer}
        };


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var BuilderHtml = new TagBuilder("div");

            if(Renderer.ContainsKey(ProtectionType))
            {
                Renderer[ProtectionType].Invoke(BuilderHtml,this);
            }
            output.TagName = null;
            output.Content.AppendHtml(BuilderHtml.InnerHtml);
        }
    }
}
