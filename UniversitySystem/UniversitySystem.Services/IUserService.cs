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
    }
}
