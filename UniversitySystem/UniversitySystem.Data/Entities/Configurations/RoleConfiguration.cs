using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniversitySystem.Data.Entities.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired();

            builder.Property(r => r.FullName)
                .IsRequired();

            builder.HasData(
                new Role { Id = 1, Name = "admin", FullName = "Administrator" },
                new Role { Id = 2, Name = "student", FullName = "Student" },
                new Role { Id = 3, Name = "teacher", FullName = "Teacher" }
                );
        }
    }
}