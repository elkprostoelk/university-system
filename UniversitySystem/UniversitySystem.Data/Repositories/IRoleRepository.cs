using System.Collections.Generic;
using System.Threading.Tasks;
using UniversitySystem.Data.Entities;

namespace UniversitySystem.Data.Repositories
{
    public interface IRoleRepository
    {
        public Task<Role> GetRole(int id);
        
        public Task<ICollection<Role>> GetRoles();
        
        public Task DeleteRole(Role role);
    }
}