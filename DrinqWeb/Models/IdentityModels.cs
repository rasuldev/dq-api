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
        public string Description { get; set; }
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
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<UserAssignment> UserAssignments { get; set; }
        public DbSet<UserQuest> UserQuests { get; set; }
        public DbSet<VerificationItem> VerificationItems { get; set; }
        public DbSet<Media> Media { get; set; }

        public ApplicationDbContext()
            : base("DataEntities", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserQuest>()
                .HasRequired(q => q.Quest);
            modelBuilder.Entity<UserQuest>()
                .HasRequired(q => q.User);

            modelBuilder.Entity<UserAssignment>()
                .HasRequired(a => a.Assignment);
            modelBuilder.Entity<UserAssignment>()
                .HasRequired(q => q.User);

            modelBuilder.Entity<VerificationItem>()
                .HasRequired(vi => vi.UserAssignment);
            modelBuilder.Entity<VerificationItem>()
                .HasRequired(vi => vi.Media);

            modelBuilder.Entity<Media>()
                .HasOptional(m => m.Assignment);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<DrinqWeb.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}