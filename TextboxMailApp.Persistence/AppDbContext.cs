using Microsoft.EntityFrameworkCore;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
