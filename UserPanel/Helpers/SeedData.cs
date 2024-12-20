using Microsoft.EntityFrameworkCore;
using UserPanel.Providers;
using UserPanel.Models.User;
using UserPanel.Models.Password;
using UserPanel.Services;
namespace UserPanel.Helpers
{
    public static class SeedData
    {

        public static void EnsurePopulate(this WebApplication app)
        {
            AppDbContext dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            PasswordHasher passwordHasher = app.Services.CreateScope().ServiceProvider.GetRequiredService<PasswordHasher>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                dbContext.Database.Migrate();
            }
            if(!dbContext.PasswordRules.Any()) {

                dbContext.PasswordRules.AddRange(new PasswordRules[]
                {
                    new PasswordRules()
                    {
                        Description = "",
                        ConstraintType = PasswordConstraintType.ContainsSpecialCharacter,
                        Regex = @"[#$@!%&*?]",
                        IsEnabled = true,
                        ErrorMessage = "The Password doesn't contain at least 1 special char"
                    },
                    new PasswordRules()
                    {
                        Description = "",
                        ConstraintType = PasswordConstraintType.NoRepeatingCharacters,
                        Regex = "",
                        IsEnabled = true,
                        ErrorMessage = "The Password contain reapeted characters"
                    },
                    new PasswordRules()
                    {
                        Description = "",
                        ConstraintType = PasswordConstraintType.ContainsDigitNotAtStart,
                        Regex = @"^(?!\d)[A-Za-z\d]",
                        IsEnabled = true,
                        ErrorMessage = "The Password shouldn't start from Number value"

                    },
                    new PasswordRules()
                    {
                        Description = "",
                        ConstraintType = PasswordConstraintType.LengthBetween,
                        Regex = "",
                        IsEnabled = true,
                        MinValue = 5,
                        MaxValue = 16,
                        ErrorMessage = $"The Password must be between 5 and 15 chars"
                    },
                    new PasswordRules()
                    {
                        Description = "",
                        ConstraintType = PasswordConstraintType.ContainsUppercaseLetter,
                        Regex = @"^(?=.*[A-Z]).+$",
                        IsEnabled = true,
                        ErrorMessage = $"The Password must contain upper case letter"
                    },
                       new PasswordRules()
                    {
                        Description = "",
                        ConstraintType = PasswordConstraintType.DaysToPasswordReset,
                        Value = 10,
                        IsEnabled = true,
                        ErrorMessage = ""
                    }
                }) ; 
            
            }
            if(!dbContext.Users.Any())
            {
                dbContext.Users.Add(new UserModel()
                {
                    Name = "Kacper",
                    Email = "kacperc318@gmail.com",
                    Password = passwordHasher.HashPassword("admin123"),
                    Role = Models.UserRole.ADMIN,
                    Phone = "111-111-111",
                    Company = "UBB",
                    Address = "UBB",
                    IsActive = false,
                });

                dbContext.Users.AddRange(new UserModel[] {
                    new UserModel()
                    {
                        Name = "Marek",
                        Email = "kacperc317@gmail.com",
                        Password = passwordHasher.HashPassword("user123"),
                        Role = Models.UserRole.USER,
                        Phone = "111-111-111",
                        Company = "UBB",
                        Address = "UBB",
                        IsActive = true,
                    },
                    new UserModel()
                    {
                        Name = "Adam",
                        Email = "kacperc319@gmail.com",
                        Password = passwordHasher.HashPassword("user123"),
                        Role = Models.UserRole.USER,
                        Phone = "111-111-111",
                        Company = "UBB",
                        Address = "UBB",
                        IsActive = false,
                    }
                });

                dbContext.SaveChanges(); 


                foreach (var item in dbContext.Users)
                {
                    dbContext.UserModelStates.Add(new UserModelState()
                    { 
                        UserId = item.Id,
                        State = UserState.New,
                        Description = "",
                    }); 
                }

                dbContext.SaveChanges(); 

            }
        }
    }
}
