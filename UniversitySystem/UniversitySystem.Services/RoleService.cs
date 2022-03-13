using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UniversitySystem.Data.Entities;
using UniversitySystem.Data.Exceptions;
using UniversitySystem.Data.Repositories;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Exceptions;

namespace UniversitySystem.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(
            IRoleRepository roleRepository,
            IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<RoleDto>> GetAllRoles()
        {
            var roles = await _roleRepository.GetRoles();
            ICollection<RoleDto> roleDtos = roles.Select(r => _mapper.Map<RoleDto>(r)).ToList();
            return roleDtos;
        }

        public async Task DeleteRole(int roleId)
        {
            var role = await _roleRepository.GetRole(roleId);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }

            if (role.Name == "admin")
            {
                throw new AdminRoleDeletingException();
            }

            await _roleRepository.DeleteRole(role);
        }

        public async Task CreateRole(NewRoleDto newRoleDto)
        {
            var newRole = new Role
            {
                Name = newRoleDto.RoleName
            };
            if (await _roleRepository.RoleExists(newRole.Name))
            {
                throw new RoleExistsException();
            }
            await _roleRepository.AddRole(newRole);
        }
    }
}