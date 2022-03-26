using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniversitySystem.Data.Entities.Configurations
{
    public class ChairConfiguration : IEntityTypeConfiguration<Chair>
    {
        public void Configure(EntityTypeBuilder<Chair> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired();

            builder.HasIndex(c => c.Name)
                .IsUnique();

            builder.HasOne(c => c.Faculty)
                .WithMany(f => f.Chairs)
                .HasForeignKey(c => c.FacultyId);

            builder.HasMany(c => c.Specialties)
                .WithOne(s => s.Chair);

            builder.HasMany(c => c.Teachers)
                .WithOne(t => t.Chair);
        }
    }
}