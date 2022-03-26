using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniversitySystem.Data.Entities.Configurations
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired();

            builder.HasIndex(s => s.Name)
                .IsUnique();

            builder.HasOne(s => s.Chair)
                .WithMany(c => c.Specialties)
                .HasForeignKey(s => s.ChairId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s => s.Faculty)
                .WithMany(f => f.Specialties)
                .HasForeignKey(s => s.FacultyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(s => s.Students)
                .WithOne(s => s.Specialty)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}