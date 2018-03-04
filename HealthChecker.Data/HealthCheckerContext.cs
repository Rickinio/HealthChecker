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

        public DbSet<Application> Applications { get; set; }
        public DbSet<HealthCheckerResult> Results {get;set;}
        
    }
}
