using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Data.Entities;
using UniversitySystem.Data.Interfaces;

namespace UniversitySystem.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UniversitySystemDbContext _dbContext;

        public StudentRepository(UniversitySystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task RemoveAllByUserId(int userId)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            var studentEntitiesToRemove = await _dbContext.Students
                .Where(s => s.UserId == userId).ToListAsync();
            _dbContext.Students.RemoveRange(studentEntitiesToRemove);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task AddStudent(Student newStudent)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            await _dbContext.Students.AddAsync(newStudent);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}