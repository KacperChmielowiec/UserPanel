using AutoMapper;
using UserPanel.Interfaces;
using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Campaning;
using UserPanel.Models.User;
using UserPanel.References;
namespace UserPanel.Providers
{
    public class DataBaseProvider
    {
        private IConfiguration _configuration;
        private IMapper _mapper;
        private DataBase DataBase { get; set; }
        public DataBaseProvider(IConfiguration configuration, DataBase dataBase, IMapper mapper) {
            _configuration = configuration;
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
                return new MockRepositoryCampaning();
            }
            else
            {
                throw new AccessViolationException("");
            }
        }

    }
}
