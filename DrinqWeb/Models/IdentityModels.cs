using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DrinqWeb.Models.CodeFirstModels;

namespace DrinqWeb.Models
{
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Assignment> Assignments;
        public DbSet<Quest> Quests;
        public DbSet<UserAssignment> UserAssignments;
        public DbSet<UserQuest> UserQuests;
        public DbSet<VerificationItem> VerificationItems;
        public DbSet<Media> Media;

        public ApplicationDbContext()
            : base("DataEntities", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserQuest>()
                .HasRequired(q => q.Quest);

            modelBuilder.Entity<UserAssignment>()
                .HasRequired(a => a.Assignment);

            modelBuilder.Entity<VerificationItem>()
                .HasRequired(vi => vi.UserAssignment);

            modelBuilder.Entity<Media>()
                .HasRequired(m => m.Assignment);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}