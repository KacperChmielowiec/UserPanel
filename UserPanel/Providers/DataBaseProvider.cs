using AutoMapper;
using UserPanel.Interfaces;
using UserPanel.Interfaces.Abstract;
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
        private DataBase DataBase { get; set; }
        public DataBaseProvider(IConfiguration configuration, DataBase dataBase, IMapper mapper, IHttpContextAccessor accessor) {
            _configuration = configuration;
            _mapper = mapper;
            _contextAccessor = accessor;
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
                return new MockGroupRepository(_contextAccessor.HttpContext.Session);
            }
            else
            {
                throw new AccessViolationException("");
            }
        }
    }
}
