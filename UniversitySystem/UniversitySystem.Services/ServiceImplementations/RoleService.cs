using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UniversitySystem.Data.Entities;
using UniversitySystem.Data.Exceptions;
using UniversitySystem.Data.Repositories;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Exceptions;
using UniversitySystem.Services.Interfaces;

namespace UniversitySystem.Services.ServiceImplementations
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
                Name = newRoleDto.RoleName,
                FullName = newRoleDto.FullRoleName
            };
            if (await _roleRepository.RoleExists(newRole.Name))
            {
                throw new RoleExistsException();
            }
            await _roleRepository.AddRole(newRole);
        }

        public async Task EditRole(int id, EditRoleDto editRoleDto)
        {
            var role = await _roleRepository.GetRole(id);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }

            role.Name = editRoleDto.Name;
            role.FullName = editRoleDto.FullRoleName;
            await _roleRepository.UpdateRole(role);
        }

        public async Task<RoleDto> GetRole(int roleId)
        {
            var role = await _roleRepository.GetRole(roleId);
            var roleDto = _mapper.Map<RoleDto>(role);
            roleDto.Users = role.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.UserName,
                Role = roleDto.Name,
                FullRoleName = role.FullName
            }).ToList();
            return roleDto;
        }
    }
}