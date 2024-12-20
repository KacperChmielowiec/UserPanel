using UserPanel.Attributes;

namespace UserPanel.Models
{
    public enum UserRole
    {
        [StringValue("Admin")]
        ADMIN = 0,
        [StringValue("User")]
        USER = 1,
        EMPLOYEE = 2,
    }
}
