using System.Collections.Generic;
using System.Threading.Tasks;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ServiceResult> LoginUser(LoginDto loginDto);

        public Task<ServiceResult> RegisterUser(RegisterDto registerDto);
        
        public Task<ServiceResult> DeleteUser(long id);
        
        public Task<ServiceResult> ChangePassword(long id, ChangePasswordDto changePasswordDto);
        
        public Task<ServiceResult> AddToRole(long userId, int roleId);
        
        public Task<ServiceResult> DeleteFromRole(long userId, int roleId);
        
        public Task<ServiceResult> EditUser(long id, EditUserDto editUserDto);
        public Task<ICollection<RoleDto>> GetRoles(string login);
        public Task<ServiceResult> GetMainUserInfo(long userId);
        public Task<ICollection<UserForAdminPanelDto>> GetAllUsers();
        public Task<ServiceResult> ReloginUser(ReloginDto reloginDto);
    }
}
