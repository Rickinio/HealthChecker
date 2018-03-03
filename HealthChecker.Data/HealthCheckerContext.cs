using HealthChecker.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthChecker.Data
{
    public class HealthCheckerContext : DbContext
    {
        public DbSet<Application> Applications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=HealthChecker;Trusted_Connection=True;Persist Security Info=False");
        }
    }
}
