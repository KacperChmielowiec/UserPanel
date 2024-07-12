using AutoMapper;
using UserPanel.Interfaces;
using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Adverts;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Models.User;
using UserPanel.References;
namespace UserPanel.Providers
{
    public class DataBaseProvider : IDataBaseProvider
    {
        private IConfiguration _configuration;
        private IMapper _mapper;
        private IHttpContextAccessor _contextAccessor;
        ISession _session;
        DataBase _database;

        public static string env_repo_type = AppReferences.EnvRepositoryType;
        public DataBaseProvider(
            IConfiguration configuration, 
            IMapper mapper, 
            IHttpContextAccessor accessor, 
            DataBase dataBase
        )
        {
            _configuration = configuration;
            _mapper = mapper;
            _contextAccessor = accessor;
            _session = _contextAccessor?.HttpContext?.Session;
            _database = dataBase;
        }
        public UserRepository<UserModel> GetUserRepository()
        {
            if (_configuration[env_repo_type]?.ToLower() == AppReferences.CONFIG_MOCK)
            {
                return new UserRepositoryMock(_session);
            }
            else
            {                                                                                                                                                                                                       
                return new UserRepositorySql(_database, _mapper);
            }
        }
        public CampaningRepository<Campaning> GetCampaningRepository()
        {
            if (_configuration[env_repo_type]?.ToLower() == AppReferences.CONFIG_MOCK)
            {
                return new MockRepositoryCampaning(_session,_mapper);                                                                          
            }
            else
            {
                throw new NotImplementedException("CampaningRepository is not implemented yet ( try mock type )");
            }
        }

        public GroupStatRepository<GroupStat> GetGroupStatRepository()
        {
            if (_configuration[env_repo_type]?.ToLower() == AppReferences.CONFIG_MOCK)
            {
                return new MockRepositoryGroupStat();
            }
            else
            {
                throw new NotImplementedException("GroupStatRepository repository is not implemented yet ( try mock type )");
            }
        }
        public GroupRepository<GroupModel> GetGroupRepository()
        {
            if (_configuration[env_repo_type]?.ToLower() == AppReferences.CONFIG_MOCK)
            {
                return new MockGroupRepository(_session, _mapper, _contextAccessor);
            }
            else
            {
                throw new NotImplementedException("GroupRepository repository is not implemented yet ( try mock type )");
            }
        }

        public AdvertRepository<Advert> GetAdvertRepository()
        {
            if (_configuration[env_repo_type]?.ToLower() == AppReferences.CONFIG_MOCK)
            {
                return new AdvertRepositoryMock(_session, _mapper, _contextAccessor);
            }
            else
            {
                throw new NotImplementedException("AdvertRepository repository is not implemented yet ( try mock type )");
            }
        }

        public FullContextRepository GetFullContextRepository()
        {
            if (_configuration[env_repo_type]?.ToLower() == AppReferences.CONFIG_MOCK)
            {
                return new FullContextRepositoryMock(_session, _mapper);
            }
            else
            {
                throw new NotImplementedException("FullContextRepository repository is not implemented yet ( try mock type )");
            }
        }
    }
}
