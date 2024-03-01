using AutoMapper;
using System.Data;
using UserPanel.Interfaces;

namespace UserPanel.Models.User
{
    public class UserRepositorySql : UserRepository<UserModel>
    {
        private DataBase DataBase;
        private readonly IMapper _mapper;
        public UserRepositorySql(DataBase dataBase, IMapper mapper) { 
            DataBase = dataBase;
            _mapper = mapper;
        }
        public override UserModel GetModelById(int id)
        {
            string query = $"SELECT * FROM dbo.Users WHERE Users.UserID = {id}";

            DataTable table = DataBase.query(query,"User");
            return _mapper.Map<UserModel>(table);

        }

        public override UserModel GetModelByName(string name)
        {
            throw new NotImplementedException();
        }

        public override UserModel GetModelByEmail(string name)
        {
            throw new NotImplementedException();
        }
    }
}
