using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniversitySystem.Data.Entities.Configurations
{
    public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                .IsRequired();

            builder.HasIndex(f => f.Name)
                .IsUnique();

            builder.HasMany(f => f.Chairs)
                .WithOne(c => c.Faculty);

            builder.HasMany(f => f.Students)
                .WithOne(s => s.Faculty);

            builder.HasMany(f => f.Specialties)
                .WithOne(s => s.Faculty);
        }
    }
}