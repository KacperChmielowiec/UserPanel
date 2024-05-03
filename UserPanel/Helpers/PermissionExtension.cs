using Microsoft.AspNetCore.Authentication;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Services;

namespace UserPanel.Helpers
{
    public static class PermissionExtension
    {
        public static void LoadContext(this PermissionContext context, IDataBaseProvider _provider, AuthenticationTicket ticket)
        {
            context.IsLoad = true;
            context.IsLogin = ticket.Principal?.Identity?.IsAuthenticated ?? false;
            var id = ticket.Principal.Claims.Where(x => x.Type.ToLower() == "id")?.Select(x => x.Value).FirstOrDefault();
            if (id != null)
            {
                int ID = 0;
                int.TryParse(id, out ID);
                context.contextUserID = ID;
            }
            context.CampsContext = _provider.GetCampaningRepository()
                        .getCampaningsByUser(context.contextUserID)
                        .Select(camp => new CampContext() { id = camp.id, UserId = camp.FK_User })
                        .ToList();
            context.GroupsContext = _provider.GetGroupRepository()
                        .GetGroupsByUserId(context.contextUserID)
                        .Select(g => new GroupContext() { CampId = g.FK_Camp, Id = g.id, UserId = context.contextUserID })
                        .ToList();
             
        }

        public static bool CampIsAllowed(this PermissionContext context, Guid guid)
        {
             return context.CampsContext.Select(camp => camp.id)?.Contains(guid) ?? false;
        }
    }
}
