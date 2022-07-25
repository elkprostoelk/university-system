using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniversitySystem.Data.Entities.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.UserName)
                .IsUnique();

            builder.HasIndex(u => new {u.LastName, u.FirstName, u.SecondName})
                .HasDatabaseName("FullName");
            
            builder.Property(u => u.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.SecondName)
                .HasMaxLength(100);

            builder.Property(u => u.PassportNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(255);

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("UserRoles"));

            builder.HasOne(u => u.TeacherRole)
                .WithOne(t => t.User)
                .HasForeignKey<Teacher>(t => t.UserId);

            builder.HasOne(u => u.StudentRole)
                .WithOne(s => s.User)
                .HasForeignKey<User>(u => u.StudentId);
        }
    }
}