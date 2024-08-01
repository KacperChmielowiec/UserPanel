using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Adverts;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Models.Product;
using UserPanel.Models.User;
using UserPanel.Models.Feed;

namespace UserPanel.Interfaces
{
    public interface IDataBaseProvider
    {
        public UserRepository<UserModel> GetUserRepository();
        public CampaningRepository<Campaning> GetCampaningRepository();
        public GroupStatRepository<GroupStat> GetGroupStatRepository();
        public GroupRepository<GroupModel> GetGroupRepository();
        public AdvertRepositoryStatic GetAdvertStRepository();
        public AdvertRepositoryDynamic GetAdvertDyRepository();
        public AdvertRepositoryFull GetAdvertRepository();
        public ProductRepository<Product> GetProductRepository();
        public FullContextRepository GetFullContextRepository();
        public FeedRepository<Feed> GetFeedRepository();
    }
}
