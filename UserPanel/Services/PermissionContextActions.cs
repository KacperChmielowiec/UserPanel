using Microsoft.DotNet.Scaffolding.Shared;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Providers;
namespace UserPanel.Services
{
    public class PermissionContextActions
    {
        private PermissionContext _permissionContext;
        private IDataBaseProvider _provider;
        public PermissionContextActions(PermissionContext permissionContext, IDataBaseProvider dataBaseProvider)
        {
            _permissionContext = permissionContext;
            _provider = dataBaseProvider;
        }

        public void UpdatePermissionContextUser(PermissionContext context)
        {
            _permissionContext.IsLogin = context.IsLogin;
            _permissionContext.contextUserID = context.contextUserID;
            if (context.IsLogin)
            {
                _permissionContext.CampsContext = _provider.GetCampaningRepository()
                        .getCampaningsByUser(context.contextUserID)
                        .Select(camp => new CampContext() { id = camp.id })
                        .ToList();
                _permissionContext.GroupsContext = _provider.GetGroupRepository()
                        .GetGroupsByUserId(context.contextUserID)
                        .Select(g => new GroupContext() {id = g.id})
                        .ToList();
            }
            else
            {
                _permissionContext.GroupsContext?.Clear();
                _permissionContext.CampsContext?.Clear();
            }
        }
        public void UpdatePermissionContextCamp(CampContext context)
        {
            if(_permissionContext.IsLogin) {

                _permissionContext.CampsContext.RemoveAll(g => g.id == context.id);
                _permissionContext.CampsContext.Add(context);

            }
        }
        public void UpdatePermissionContextGroup(GroupContext context)
        {
            if (_permissionContext.IsLogin)
            {

                _permissionContext.GroupsContext.RemoveAll(g => g.id == context.id);
                _permissionContext.GroupsContext.Add(context);

            }
        }
    }
}
