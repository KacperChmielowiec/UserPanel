using AutoMapper;
using System.Data;
using UserPanel.Services;
using UserPanel.Interfaces.Abstract;
namespace UserPanel.Models.User
{
    public class MockRepository : UserRepository<UserModel>
    {
        private string UserPath = "appConfig.database.mock.users";
        public MockRepository()
        {
            
        }
        public override UserModel? GetModelById(int id)
        {
            return ConfigManager.GetConfig(UserPath).Parse<List<UserModel>>().Where(user => user.Id == id).FirstOrDefault();
        }

        public override UserModel? GetModelByName(string name)
        {
            return ConfigManager.GetConfig(UserPath).Parse<List<UserModel>>().Where(user => user.Name == name).FirstOrDefault(); 
        }
        public override UserModel? GetModelByEmail(string email)
        {
            return ConfigManager.GetConfig(UserPath).Parse<List<UserModel>>().Where(user => user.Email == email).FirstOrDefault();
        }
    }
}
