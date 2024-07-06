using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Adverts;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Models.User;

namespace UserPanel.Interfaces
{
    public interface IDataBaseProvider
    {
        public UserRepository<UserModel> GetUserRepository();
        public CampaningRepository<Campaning> GetCampaningRepository();
        public GroupStatRepository<GroupStat> GetGroupStatRepository();
        public GroupRepository<GroupModel> GetGroupRepository();
        public AdvertRepository<Advert> GetAdvertRepository();
    }
}
