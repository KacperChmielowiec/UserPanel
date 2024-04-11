using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using static System.Net.Mime.MediaTypeNames;
using UserPanel.Models.Camp;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Text.Encodings.Web;
namespace UserPanel.Helpers
{
    [HtmlTargetElement("span", Attributes = "flag-state")]
    public class FlagsTagHelper : TagHelper
    {
        private Dictionary<CampaningFlagState, string> mapState = new Dictionary<CampaningFlagState, string>()
        {
            { CampaningFlagState.Active, "icon-ok" },
            { CampaningFlagState.Waiting, "icon-ok" },
            { CampaningFlagState.Inactive, "icon-warning-sign" }
        };
        private Dictionary<CampaningFlagState, string> mapStyle = new Dictionary<CampaningFlagState, string>()
        {
            { CampaningFlagState.Active, "success" },
            { CampaningFlagState.Waiting, "warn" },
            { CampaningFlagState.Inactive, "danger" }
        };
        public CampaningFlagState FlagState { get; set; }
        public override void Process(TagHelperContext context,TagHelperOutput output)
        {
            TagBuilder result = new TagBuilder("i");
            result.Attributes["class"] = mapState[FlagState];
            output.AddClass(mapStyle[FlagState], HtmlEncoder.Default);
            output.Content.AppendHtml(result);
        }
    }
}
