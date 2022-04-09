using System.Collections.Generic;
using System.Threading.Tasks;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ServiceResult> LoginUser(LoginDto loginDto);

        public Task<ServiceResult> RegisterUser(RegisterDto registerDto);
        
        public Task<ServiceResult> DeleteUser(int id);
        
        public Task<ServiceResult> ChangePassword(int id, ChangePasswordDto changePasswordDto);
        
        public Task<ServiceResult> AddToRole(int userId, int roleId);
        
        public Task<ServiceResult> DeleteFromRole(int userId, int roleId);
        
        public Task<ServiceResult> EditUser(int id, EditUserDto editUserDto);
        public Task<ICollection<RoleDto>> GetRoles(string login);
        public Task<ServiceResult> GetMainUserInfo(int userId);
        public Task<ICollection<UserForAdminPanelDto>> GetAllUsers();
        public Task<ServiceResult> ReloginUser(ReloginDto reloginDto);
    }
}
