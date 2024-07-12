using AutoMapper;
using MimeKit;
using NuGet.Packaging;
using System.Reflection;
using System.Security.Cryptography;
using UserPanel.Helpers;
using UserPanel.Interfaces;
using UserPanel.Models;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;

namespace UserPanel.Services
{
    public class GroupManager
    {
        private IDataBaseProvider _provider;
        private IConfiguration _configuration;
        private UserManager _userManager;
        public GroupManager(
            IDataBaseProvider provider, 
            IConfiguration configuration, 
            UserManager userManager
         )
        {
            this._provider = provider;
            this._configuration = configuration;
            this._userManager = userManager;
        }
        public List<GroupModel> GetGroupsByCampID(Guid idCamp)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return new List<GroupModel>();
            int id = _userManager.getUserId();

            if (_provider.GetCampaningRepository().GetCampaningsByUser(id).Where(camp => camp.id == idCamp).FirstOrDefault() != null)
            {
              return  _provider.GetGroupRepository().GetGroupsByCampId(idCamp);
            }
            return new List<GroupModel>();
        }

        public List<GroupModel> GetGroupsByUserID(int idUser)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return new List<GroupModel>();
            int id = _userManager.getUserId();
            if (idUser != id) return new List<GroupModel>();

            return _provider.GetGroupRepository().GetGroupsByUserId(idUser);
        }

        public GroupModel? GetGroupById(Guid idGroup, bool deep = false)
        {
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return new GroupModel();
        

            var group = _provider.GetGroupRepository().GetGroupById(idGroup);

            if (deep && group != null) {
              var ad = _provider.GetAdvertRepository().GetAdvertGroupId(idGroup);
              group.advertisementsList = ad.ToArray();
            }


            return group;
        }
        public void UpdateGroup(GroupModel model)
        {
            if (model == null || model.id == Guid.Empty) throw new ArgumentNullException(); 
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return;
         


            var group = _provider.GetGroupRepository().GetGroupById(model.id);

            model.advertisementsList = group.advertisementsList;
            model.Lists = group.Lists;
            model.details.CampaningFlags = group.details.CampaningFlags;

            _provider.GetGroupRepository().UpdateGroup(model.id, model);
        }
        public void CreateGroup(Guid camp_id,GroupModel model)
        {
            if (model == null) throw new ArgumentException();
            if (!_userManager.isLogin() || _userManager.getUserId() == -1) return;
            int id = _userManager.getUserId();

          ;

            if(model.id == Guid.Empty) model.id = Guid.NewGuid();

            model.details.CampaningFlags = new CampaningFlags();
            model.details.CampaningFlags.Advert = CampaningFlagState.Waiting;
            model.details.CampaningFlags.Products = CampaningFlagState.Waiting;
            model.details.CampaningFlags.Budget = CampaningFlagState.Waiting;
            model.details.CampaningFlags.Display = CampaningFlagState.Waiting;

            _provider.GetGroupRepository().CreateGroup(camp_id, model);
        }
    }
}
