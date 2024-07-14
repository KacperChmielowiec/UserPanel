using System.Runtime.CompilerServices;

namespace UserPanel.Helpers
{
    public static class FetchRouteInfoHelper
    {
        public static string CampaignFetchIdAction(HttpContext context)
        {
            return context.Request.RouteValues["id"].ToString();
        }
    }
}
