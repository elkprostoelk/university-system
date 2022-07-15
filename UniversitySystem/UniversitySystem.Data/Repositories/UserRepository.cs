using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversitySystem.Data.Entities;
using UniversitySystem.Data.Interfaces;

namespace UniversitySystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UniversitySystemDbContext _dbContext;

        public UserRepository(UniversitySystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _dbContext.Users
                .Include(u => u.Roles)
                .Include(u => u.StudentRoles)
                .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task AddUser(User user)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<User> GetUser(string login)
        {
            var user = await _dbContext.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserName == login);
            return user;
        }

        public async Task<bool> UserExists(string userName)
        {
            var exists = await _dbContext.Users.AnyAsync(u => u.UserName == userName);
            return exists;
        }

        public async Task DeleteUser(User user)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task UpdateUser(User user)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }

        public async Task<ICollection<User>> GetAllUsers()
        {
            var users = await _dbContext.Users
                .Include(u => u.Roles)
                .ToListAsync();
            return users;
        }
    }
}
