using System.Collections.Generic;
using System.Threading.Tasks;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Services.Interfaces
{
    public interface IRoleService
    {
        public Task<ICollection<RoleDto>> GetAllRoles();
        
        public Task DeleteRole(int roleId);
        
        public Task CreateRole(NewRoleDto newRoleDto);
        
        public Task EditRole(int id, EditRoleDto editRoleDto);
        
        public Task<RoleDto> GetRole(int roleId);
    }
}