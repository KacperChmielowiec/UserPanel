using Microsoft.EntityFrameworkCore;
using UserPanel.Interfaces.Abstract;
using UserPanel.Models.Logs;
using UserPanel.Models.Password;
using UserPanel.Providers;
using UserPanel.References;
using UserPanel.Services.observable;

namespace UserPanel.Models.User
{
    public class UserRepositoryORM : UserRepository<UserModel>
    {
        private readonly AppDbContext _context;
        private readonly IQueryable<UserModel> _users;
        public UserRepositoryORM(AppDbContext context) { 
            _context = context;
            _users = _context.Users
                .Include(u => u.PasswordHistories)
                .Include(u => u.States);
        }

        public override void AddPasswordToHistory(int id, string password)
        {
            if (GetModelById(id) != null)
            {
                _context.PasswordHistories.Add(new PasswordHistory() { UserId = id, Password = password, CreatedAt = DateTime.Now });
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"User with {id} does not exist in the database");
            }
            
        }

        public override void CreateUser(UserModel model)
        {

            if (model == null) throw new ArgumentNullException(nameof(model));

            _context.Users.Add(model); // Dodanie użytkownika do kontekstu
            _context.SaveChanges(); // Zapisanie zmian w bazie danych

            // Powiadomienie o akcji
            Subjects.userActionSubject.notify(new UserActionMessage() { id = model.Id, actionType = Types.UserActionType.Create });
        }

        public override void DeleteUser(int id)
        {
            var user = GetModelById(id);
            if (user != null)
            {
                _context.Users.Remove(user);  // Usunięcie użytkownika i kaskadowe usunięcie powiązań
                _context.SaveChanges();
            }
        }

        public override List<UserModel> GetAllUser()
        {
            return _users.ToList(); 
        }

        public override List<LogUserEntry> GetAllUserLogs()
        {
            return _context.LogUsers.ToList();
        }

        public override UserModel GetModelByEmail(string email)
        {
            return _users.FirstOrDefault(user => user.Email == email);
        }

        public override UserModel GetModelById(int id)
        {
            return _users.FirstOrDefault(user => user.Id == id);
        }

        public override UserModel GetModelByName(string name)
        {
            return _users.FirstOrDefault(user => user.Name == name);
        }

        public override PasswordRules GetPasswordRule(PasswordConstraintType type)
        {
            return _context.PasswordRules.Where(r => r.ConstraintType == type).FirstOrDefault();
        }

        public override List<PasswordHistory> GetPasswordsHistory(int id, int count = 0)
        {
            if(count <= 0) count = int.MaxValue;
            return _context.PasswordHistories.Where(pass => pass.UserId == id).Take(count).ToList();
        }

        public override List<PasswordRules> GetUserPasswordRules()
        {
            return _context.PasswordRules.ToList();
        }

        public override void UpdateModel(UserModel model)
        {

            if (model == null) throw new ArgumentNullException(nameof(model));

            var existingUser = _users.FirstOrDefault(m => m.Id == model.Id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with {model.Id} does not exist in the database");
            }

            // Aktualizacja właściwości istniejącego użytkownika
            _context.Entry(existingUser).CurrentValues.SetValues(model);

            _context.SaveChanges(); // Zapisanie zmian

            // Powiadomienie o akcji
            Subjects.userActionSubject.notify(new UserActionMessage() { id = model.Id, actionType = Types.UserActionType.Update });
        }

        public override void UpdatePasswordRule(PasswordConstraintType constraintType, PasswordRules rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            // Zakładamy, że klucz główny jest poprawnie ustawiony w obiekcie 'rule'
            var existingRule = _context.PasswordRules.Find(rule.Id); // Znalezienie istniejącej reguły w bazie

            if (existingRule != null)
            {
                // Zaktualizowanie wartości istniejącej reguły na podstawie nowego obiektu 'rule'
                _context.Entry(existingRule).CurrentValues.SetValues(rule);

                // Zapisanie zmian do bazy
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Password rule not found");
            }
        }

        public override void UpdateUserState(int userId, UserState state, bool remove = false)
        {
            var user = _users.First(m => m.Id == userId);

            if (user == null) return;

            bool UserHasState = user.States.Count(user => user.State == state) > 0;

            if (remove && UserHasState)
            {
                user.States = user.States.Where(st => st.State != state).ToList();
                UpdateModel(user);
                return;
            }
            else if (!remove && !UserHasState)
            {
                var stateModel = new UserModelState() { Description = "", UserId = userId, State = state };
                user.States.Add(stateModel);
                UpdateModel(user);
            }
        }

        public override void UpdateUserStates(int userId, UserState[] state, bool remove = false)
        {
            foreach (var item in state)
            {
                UpdateUserState(userId, item, remove);
            }
        }
    }
}
