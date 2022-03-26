using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Data.Entities;

namespace UniversitySystem.Data.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly UniversitySystemDbContext _dbContext;
        
        public TeacherRepository(UniversitySystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task RemoveAllByUserId(int userId)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            var teacherEntitiesToRemove = await _dbContext.Teachers
                .Where(t => t.UserId == userId).ToListAsync();
            _dbContext.Teachers.RemoveRange(teacherEntitiesToRemove);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task AddTeacher(Teacher newTeacher)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            await _dbContext.Teachers.AddAsync(newTeacher);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}