using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UniversitySystem.Data.Entities;

namespace UniversitySystem.Data
{
    public class UniversitySystemDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<Role> Roles { get; set; }

        public DbSet<Student> Students { get; set; }
        
        public DbSet<Teacher> Teachers { get; set; }
        
        public DbSet<Specialty> Specialties { get; set; }
        
        public DbSet<Faculty> Faculties { get; set; }
        
        public DbSet<Chair> Chairs { get; set; }

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
