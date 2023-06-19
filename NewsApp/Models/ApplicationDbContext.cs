using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NewsApp.Models
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("Users");

            // Ignore other Identity tables
            modelBuilder.Ignore<IdentityRole>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityRoleClaim<string>>();
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<News> News { get; set; }
    }
}
