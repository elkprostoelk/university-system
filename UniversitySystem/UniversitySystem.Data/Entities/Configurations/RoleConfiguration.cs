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

            builder.HasData(
                new Role { Id = 1, Name = "admin" },
                new Role { Id = 2, Name = "student" },
                new Role { Id = 3, Name = "teacher" }
                );
        }
    }
}