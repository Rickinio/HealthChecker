using HealthChecker.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthChecker.Data
{
    public class HealthCheckerContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=HealthChecker;Trusted_Connection=True;Persist Security Info=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>()
                .Property(c => c.DateTimeCreated)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<HealthCheckerResult>()
                .Property(c => c.DateTimeCreated)
                .HasDefaultValueSql("GETUTCDATE()");
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<HealthCheckerResult> Results {get;set;}
        
    }
}
