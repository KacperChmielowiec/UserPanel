using UserPanel.Models.Logs;
using UserPanel.Models.Password;
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
        public abstract void DeleteUser(int id);
        public abstract List<PasswordHistory> GetPasswordsHistory(int id, int count = 0);
        public abstract void AddPasswordToHistory(int id, string password);
        public abstract List<PasswordRules> GetUserPasswordRules();

        public abstract List<LogUserEntry> GetAllUserLogs();
        public abstract PasswordRules GetPasswordRule(PasswordConstraintType type);
        public abstract void UpdateUserState(int userId, UserState state, bool remove = false);
        public abstract void UpdateUserStates(int userId, UserState[] state, bool remove = false);

        public abstract void UpdatePasswordRule(PasswordConstraintType constraintType, PasswordRules rule);
    }
}
