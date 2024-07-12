using UserPanel.Models.User;

namespace UserPanel.Interfaces.Abstract
{
    public abstract class UserRepository<T>
    {
        public abstract List<T> GetAllUser();
        public abstract T GetModelById(int id);
        public abstract T GetModelByName(string name);
        public abstract T GetModelByEmail(string name);
        public abstract void UpdateModel(T model);
        public abstract void CreateUser(T model);
    }
}
