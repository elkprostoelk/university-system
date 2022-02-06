using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UniversitySystemDbContext _dbContext;

        public UserRepository(UniversitySystemDbContext dbContext)
        {
            _dbContext = dbContext;
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
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == login);
            return user;
        }

        public async Task<bool> UserExists(string userName)
        {
            var exists = await _dbContext.Users.AnyAsync(u => u.UserName == userName);
            return exists;
        }
    }
}
