using Microsoft.EntityFrameworkCore;
using UserPanel.Models.User;
using UserPanel.Models.Password;
using Microsoft.AspNetCore.Identity;
using UserPanel.Models.Logs;
namespace UserPanel.Providers
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<UserModel> Users { get; set; } // DbSet dla UserModel
        public DbSet<PasswordHistory> PasswordHistories { get; set; }  // DbSet dla PasswordHistory
        public DbSet<PasswordRules> PasswordRules { get; set; }
        public DbSet<UserModelState> UserModelStates { get; set; }
        public DbSet<LogUserEntry> LogUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja relacji pomiędzy UserModel i PasswordHistory
            modelBuilder.Entity<PasswordHistory>()
                .HasOne<UserModel>() // Brak nawigacyjnej właściwości
                .WithMany(u => u.PasswordHistories) // Relacja "wiele" z `UserModel`
                .HasForeignKey(ph => ph.UserId) // Klucz obcy
                .OnDelete(DeleteBehavior.Cascade); // Kaskadowe usuwanie

            modelBuilder.Entity<UserModelState>()
                .HasOne<UserModel>()
                .WithMany(u => u.States)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LogUserEntry>(e =>
            {
                e.HasKey(u => u.Id);
                e.Property(e => e.UserId).IsRequired();
                e.Property(e => e.UserName).IsRequired();
                e.Property(e => e.Timestamp).IsRequired();  
                e.Property(e => e.Action).IsRequired();
            });
                

        }
    }
}   
