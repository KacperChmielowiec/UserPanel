using AutoMapper;
using System.Data;
using UserPanel.Interfaces;
using UserPanel.Interfaces.Abstract;
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
            string query = $"SELECT * FROM dbo.Users WHERE Users.Id = '{id}'";

            DataTable table = DataBase.query(query,"User");
            return _mapper.Map<UserModel>(table);

        }

        public override UserModel GetModelByName(string name)
        {
            throw new NotImplementedException();
        }

        public override UserModel GetModelByEmail(string email)
        {
            string query = $"SELECT * FROM dbo.Users WHERE Users.Email = '{email}'";

            DataTable table = DataBase.query(query, "User");
            if(table.Rows.Count > 0)
                return _mapper.Map<UserModel>(table.Rows[0]);
            return null;

        }

        public override void CreateUser(UserModel model)
        {
            throw new NotImplementedException();
        }

        public override List<UserModel> GetAllUser()
        {
            throw new NotImplementedException();
        }

        public override void UpdateModel(UserModel model)
        {
            throw new NotImplementedException();
        }
    }
}
