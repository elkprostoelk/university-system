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
    }
}