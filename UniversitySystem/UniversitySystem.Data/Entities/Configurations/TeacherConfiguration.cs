using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniversitySystem.Data.Entities.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.HiredOn)
                .IsRequired();

            builder.HasOne(t => t.User)
                .WithOne(u => u.TeacherRole)
                .HasForeignKey<User>(u => u.TeacherId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.Chair)
                .WithMany(c => c.Teachers)
                .HasForeignKey(t => t.ChairId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}