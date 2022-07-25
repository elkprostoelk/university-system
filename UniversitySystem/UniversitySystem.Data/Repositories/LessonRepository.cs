using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversitySystem.Data.Entities;
using UniversitySystem.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UniversitySystem.Data.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly UniversitySystemDbContext _context;

        public LessonRepository(
            UniversitySystemDbContext context)
        {
            _context = context;
        }
        
        public async Task AddLessonAsync(Lesson lesson)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<Lesson> GetLessonAsync(long lessonId) =>
            await _context.Lessons
                .Include(l => l.Participants)
                .Include(l => l.LessonParticipants)
                .Include(l => l.Teacher)
                .SingleOrDefaultAsync(l => l.Id == lessonId);

        public async Task UpdateLessonAsync(Lesson lesson)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task RemoveLessonAsync(Lesson lesson)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<ICollection<Lesson>> GetLessonsAsync(DateTime limitDate) => 
            await _context.Lessons
                .Include(l => l.LessonParticipants)
                    .ThenInclude(lp => lp.Participant)
                .Where(l =>
                    l.ScheduledOn <= DateTime.Now
                    && l.ScheduledOn >= limitDate)
                .ToListAsync();

        public async Task<bool?> MarkPresenceAsync(Lesson lesson, Student student)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            lesson.Participants.Add(student);
            await _context.SaveChangesAsync();

            var lessonParticipant = lesson.LessonParticipants
                .SingleOrDefault(lp => lp.ParticipantId == student.Id);
            if (lessonParticipant is null)
            {
                return null;
            }
            lessonParticipant.Present = !lessonParticipant.Present;
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return lessonParticipant.Present;
        }

        public async Task<ICollection<Lesson>> GetLessonsAsync(DateTime startDate, DateTime endDate, long? userId = null)
        {
            var lessons = await _context.Lessons
                .Include(l => l.Participants)
                    .ThenInclude(s => s.User)
                .Include(l => l.LessonParticipants)
                .Include(l => l.Teacher)
                    .ThenInclude(t => t.User)
                .Where(l => l.ScheduledOn >= startDate
                     && l.ScheduledOn <= endDate).ToListAsync();
            if (userId.HasValue)
            {
                lessons = lessons.Where(l => l.Participants.Any(p => p.Id == userId)).ToList();
            }

            return lessons;
        }

        public async Task<ICollection<Lesson>> GetLessonsAsync() =>
            await _context.Lessons
                .Include(l => l.Participants)
                .ToListAsync();
    }
}