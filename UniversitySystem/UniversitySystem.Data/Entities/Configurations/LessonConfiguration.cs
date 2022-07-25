using UniversitySystem.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniversitySystem.Data.Entities.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(l => l.Name);

            builder.Property(l => l.ScheduledOn)
                .IsRequired();

            builder.Property(l => l.LessonType)
                .IsRequired()
                .HasDefaultValue(LessonTypes.Lecture);

            builder.HasOne(l => l.Teacher)
                .WithMany(t => t.Lessons)
                .HasForeignKey(l => l.TeacherId);

            builder.HasMany(l => l.Participants)
                .WithMany(s => s.Lessons)
                .UsingEntity<LessonParticipant>(j =>
                        j.HasOne(lp => lp.Participant)
                            .WithMany(l => l.LessonParticipants)
                            .HasForeignKey(lp => lp.ParticipantId),

                    j =>
                        j.HasOne(lp => lp.Lesson)
                            .WithMany(p => p.LessonParticipants)
                            .HasForeignKey(lp => lp.LessonId),
                    
                    j =>
                    {
                        j.HasKey(lp => new {lp.LessonId, lp.ParticipantId});
                        j.Property(lp => lp.Present)
                            .IsRequired()
                            .HasDefaultValue(false);
                        j.ToTable("LessonParticipants");
                    }
                );
        }
    }
}