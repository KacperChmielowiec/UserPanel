using UserPanel.Helpers;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Models.User;
using UserPanel.References;
using UserPanel.Services;
using UserPanel.Helpers;
namespace UserPanel.Middleware
{
    public class SessionLoadMockMiddleware
    {
        private RequestDelegate next;
        public SessionLoadMockMiddleware(RequestDelegate nextDelgate)
        {
            next = nextDelgate;
        }
        public async Task Invoke(HttpContext context)
        {
            ISession _session = context.Session;
            if (!_session.Keys.Contains(SessionKeysReferences.readyKey))
            {
                var users = ConfigManager.GetConfig(MockPathsReferences.usersPath)
                    .Parse<List<UserModel>>() ?? new List<UserModel>();
                var camps = ConfigManager.GetConfig(MockPathsReferences.campaningsPath)
                    .Parse<List<Campaning>>() ?? new List<Campaning>();
                var groups = ConfigManager.GetConfig(MockPathsReferences.groupsPath)
                    .Parse<List<GroupModelMock>>() ?? new List<GroupModelMock>();
                var lists = ConfigManager.GetConfig(MockPathsReferences.listsPath)
                    .Parse<List<GroupListMock>>() ?? new List<GroupListMock>();
                var adverts = ConfigManager.GetConfig(MockPathsReferences.advertPath)
                    .Parse<List<AdvertisementMock>>() ?? new List<AdvertisementMock>();

                _session.SetJson(SessionKeysReferences.usersKey, users);
                _session.SetJson(SessionKeysReferences.campsKey, camps);
                _session.SetJson(SessionKeysReferences.groupsKey, groups);
                _session.SetJson(SessionKeysReferences.listsKey, lists);
                _session.SetJson(SessionKeysReferences.advertKey, adverts);
                _session.Set(SessionKeysReferences.readyKey, new byte[] { 0 });

            }

            await next(context);

        }
    }
}
