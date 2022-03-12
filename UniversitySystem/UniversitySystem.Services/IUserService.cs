using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Services
{
    public interface IUserService
    {
        public Task<UserDto> LoginUser(LoginDto loginDto);

        public Task<UserDto> RegisterUser(RegisterDto registerDto);
        
        public Task DeleteUser(int id);
        
        public Task ChangePassword(int id, ChangePasswordDto changePasswordDto);
        
        public Task AddToRole(int userId, int roleId);
        
        public Task DeleteFromRole(int userId, int roleId);
        
        public Task EditUser(int id, EditUserDto editUserDto);
        public Task<ICollection<RoleDto>> GetRoles(string login);
        public Task<MainUserInfoDto> GetMainUserInfo(int userId);
        public Task<ICollection<UserForAdminPanelDto>> GetAllUsers();
    }
}
