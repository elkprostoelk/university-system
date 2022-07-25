using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversitySystem.Data.Entities;

namespace UniversitySystem.Data.Interfaces
{
    public interface ILessonRepository
    {
        Task AddLessonAsync(Lesson lesson);
        Task<Lesson> GetLessonAsync(long lessonId);
        Task UpdateLessonAsync(Lesson lesson);
        Task RemoveLessonAsync(Lesson lesson);
        Task<ICollection<Lesson>> GetLessonsAsync(DateTime limitDate);
        Task<bool?> MarkPresenceAsync(Lesson lesson, Student student);
        Task<ICollection<Lesson>> GetLessonsAsync(DateTime startDate, DateTime endDate, long? userId = null);
        Task<ICollection<Lesson>> GetLessonsAsync();
    }
}