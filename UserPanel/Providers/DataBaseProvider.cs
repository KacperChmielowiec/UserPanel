using AutoMapper;
using UserPanel.Interfaces;
using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Adverts;
using UserPanel.Models.Camp;
using UserPanel.Models.Group;
using UserPanel.Models.User;
using UserPanel.References;
using UserPanel.Services.observable;
namespace UserPanel.Providers
{
    public class DataBaseProvider : IDataBaseProvider
    {
        private IConfiguration _configuration;
        private IMapper _mapper;
        private IHttpContextAccessor _contextAccessor;
        IServiceProvider _serviceProvider;
        ISession _session;
        private DataBase DataBase { get; set; }
        public DataBaseProvider(
            IConfiguration configuration, 
            DataBase dataBase, 
            IMapper mapper, 
            IHttpContextAccessor accessor, 
            IServiceProvider serviceProvider
        )
        {
            _configuration = configuration;
            _mapper = mapper;
            _contextAccessor = accessor;
            _serviceProvider = serviceProvider;
            _session = _contextAccessor?.HttpContext?.Session;
        }
        public UserRepository<UserModel> GetUserRepository()
        {
            if (_configuration["ENVIROMENT:UserRepositoryType"]?.ToLower() == AppReferences.CONFIG_MOCK)
            {
                return new MockRepository();
            }
            else
            {
                return new UserRepositorySql(DataBase, _mapper);
            }
        }
        public CampaningRepository<Campaning> GetCampaningRepository()
        {
            if (_configuration["ENVIROMENT:UserRepositoryType"]?.ToLower() == AppReferences.CONFIG_MOCK)
            {
                return new MockRepositoryCampaning(_contextAccessor.HttpContext.Session);
            }
            else
            {
                throw new AccessViolationException("");
            }
        }

        public GroupStatRepository<GroupStat> GetGroupStatRepository()
        {
            if (_configuration["ENVIROMENT:UserRepositoryType"]?.ToLower() == AppReferences.CONFIG_MOCK)
            {
                return new MockRepositoryGroupStat();
            }
            else
            {
                throw new AccessViolationException("");
            }
        }
        public GroupRepository<GroupModel> GetGroupRepository()
        {
            if (_configuration["ENVIROMENT:UserRepositoryType"]?.ToLower() == AppReferences.CONFIG_MOCK)
            {
                return new MockGroupRepository(_session, _mapper, _contextAccessor);
            }
            else
            {
                throw new AccessViolationException("");
            }
        }

        AdvertRepository<Advert> IDataBaseProvider.GetAdvertRepository()
        {
            return new AdvertRepositoryMock(_session, _mapper, _contextAccessor);
        }
    }
}
