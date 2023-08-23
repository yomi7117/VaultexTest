using ExcelReader.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ExcelReader.Domain.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

   
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Organisation> Organisations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
