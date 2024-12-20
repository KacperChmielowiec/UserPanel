using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace UserPanel.Models.User
{
    public enum UserState {
        Active,
        ExpiredPassword,
        New,
        Banned,
    }
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; }
        public UserRole Role { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public ICollection<PasswordHistory> PasswordHistories { get; set; }
        public ICollection<UserModelState> States { get; set; } = new List<UserModelState>();


    }
    public static class UserHelperModel
    {
        public static bool IsBlocked(this UserModel model)
        {
            return !model.IsActive || model.States.Select(sm => sm.State).Contains(UserState.Banned);
        }
        public static bool isNewUser(this UserModel model)
        {
            return model.States.Select(sm => sm.State).Contains(UserState.New);
        }
        public static bool isExpiredPass(this UserModel model)
        {
            return model.States.Select(sm => sm.State).Contains(UserState.ExpiredPassword);
        }
    }
    public class PasswordHistory
    {
        [Key]
        public int PasswordId { get; set; }
        public int UserId { get; set; }    
        public string Password { get; set; } 
        public DateTime CreatedAt { get; set; }  
    }

    public class UserModelState
    {
        [Key]
        public int Id { get; set; }
        public UserState State { get; set; }
        public string? Description {  get; set; }

        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }
    }

}
