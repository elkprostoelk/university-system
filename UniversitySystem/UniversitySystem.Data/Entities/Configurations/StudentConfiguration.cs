using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniversitySystem.Data.Entities.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.Id);
            
            builder.Property(s => s.EnteredOn)
                .IsRequired();

            builder.Property(s => s.EducationalLevel)
                .IsRequired();

            builder.Property(s => s.EducationForm)
                .IsRequired();

            builder.HasOne(s => s.Faculty)
                .WithMany(f => f.Students)
                .HasForeignKey(s => s.FacultyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s => s.Specialty)
                .WithMany(s => s.Students)
                .HasForeignKey(s => s.SpecialtyId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s => s.User)
                .WithOne(u => u.StudentRole)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}