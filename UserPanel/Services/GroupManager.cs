using System.Reflection;
using UserPanel.Interfaces;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;

namespace UserPanel.Services
{
    public class GroupManager
    {
        private IDataBaseProvider _provider;
        private IConfiguration _configuration;
        private UserManager _userManager;

        public GroupManager(IDataBaseProvider provider, IConfiguration configuration, UserManager userManager)
        {
            this._provider = provider;
            _configuration = configuration;
            _userManager = userManager;
        }
        public List<GroupModel> GetGroupsByCampID(Guid idCamp)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return new List<GroupModel>();
            int id = _userManager.getUserId();

            if (_provider.GetCampaningRepository().getCampaningsByUser(id).Where(camp => camp.id == idCamp).FirstOrDefault() != null)
            {
              return  _provider.GetGroupRepository().GetGroupsByCampId(idCamp);
            }
            return new List<GroupModel>();
        }

        public GroupModel? GetGroupsByID(Guid idGroup)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return new GroupModel();
            int id = _userManager.getUserId();

            var group = _provider.GetGroupRepository().GetGroupById(idGroup);

            if(_provider.GetCampaningRepository().getCampaningsByUser(id)
                .Select(camp => camp.id).Contains(group.FK_Camp))
            {
                return group;
            }
            
            return null;
        }
        public void UpdateGroup(GroupModel model)
        {

            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return;
            if (model.id == Guid.Empty) return;

            int id = _userManager.getUserId();

            if (!_provider.GetCampaningRepository().getCampaningsByUser(id)
                .Select(camp => camp.id).Contains(model.FK_Camp))
            {
                return;
            }

            _provider.GetGroupRepository().UpdateGroup(model.id, model);
        }
    }
}
