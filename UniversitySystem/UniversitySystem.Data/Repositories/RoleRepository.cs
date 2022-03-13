using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Data.Entities;

namespace UniversitySystem.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UniversitySystemDbContext _dbContext;
        
        public RoleRepository(UniversitySystemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<Role> GetRole(int id)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
            return role;
        }

        public async Task<ICollection<Role>> GetRoles()
        {
            var roles = await _dbContext.Roles.ToListAsync();
            return roles;
        }

        public async Task DeleteRole(Role role)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
    }
}