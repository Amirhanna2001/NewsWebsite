using Microsoft.EntityFrameworkCore;

namespace NewsApp.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<News> News { get; set; }
    }
}
