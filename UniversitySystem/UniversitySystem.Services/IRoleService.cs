using System.Collections.Generic;
using System.Threading.Tasks;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Services
{
    public interface IRoleService
    {
        public Task<ICollection<RoleDto>> GetAllRoles();
        
        public Task DeleteRole(int roleId);
    }
}