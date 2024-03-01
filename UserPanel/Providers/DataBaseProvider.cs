using AutoMapper;
using UserPanel.Interfaces;
using UserPanel.Models.User;
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
            if (_configuration["ENVIROMENT:UserRepositoryType"]?.ToLower() == "mock")
            {
                return new MockRepository();
            }
            else
            {
                return new UserRepositorySql(DataBase,_mapper);
            }
        }

    }
}
