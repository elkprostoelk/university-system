using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace UniversitySystem.Data
{
    public class UniversitySystemDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UniversitySystemDbContext(
            DbContextOptions<UniversitySystemDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
