using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using UniversitySystem.Data;
using UniversitySystem.Data.Entities;
using UniversitySystem.Data.Exceptions;
using UniversitySystem.Data.Repositories;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Exceptions;

namespace UniversitySystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClaimDecorator _claimDecorator;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UserService(
            IMapper mapper,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IClaimDecorator claimDecorator)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _claimDecorator = claimDecorator;
        }

        public async Task<UserDto> LoginUser(LoginDto loginDto)
        {
            var user = await _userRepository.GetUser(loginDto.Login);
            var hashedPassword = HashPassword(user, loginDto.Password);
            if (user is null)
            {
                throw new UserNotFoundException();
            }
            
            var role = await _roleRepository.GetRole(loginDto.RoleId);
            if (!user.Roles.Contains(role) || user.PasswordHash != hashedPassword)
            {
                throw new AccessForbiddenException();
            }
            
            return new UserDto
            {
                Id = user.Id,
                Name = user.UserName,
                Role = role.Name
            };
        }

        public async Task<UserDto> RegisterUser(RegisterDto registerDto)
        {
            var userExists = await _userRepository.UserExists(registerDto.UserName);
            if (userExists)
            {
                throw new UserExistsException();
            }
            var user = new User
            {
                UserName = registerDto.UserName,
                FirstName = registerDto.FirstName,
                SecondName = registerDto.SecondName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                BirthDate = registerDto.BirthDate,
                CreatedDate = DateTime.Now,
                Gender = registerDto.Gender,
                PassportNumber = registerDto.PassportNumber
            };
            var hashedPassword = HashPassword(user, registerDto.Password);
            user.PasswordHash = hashedPassword;
            await _userRepository.AddUser(user);
            return new UserDto
            {
                Id = user.Id,
                Name = user.UserName,
                Role = String.Empty
            };
        }

        public async Task DeleteUser(int id)
        {
            if (_claimDecorator.Id == id)
            {
                throw new SelfDeletingException();
            }
            var user = await _userRepository.GetUser(id);
            if (user is null)
            {
                throw new UserNotFoundException();
            }

            await _userRepository.DeleteUser(user);
        }

        public async Task ChangePassword(int id, ChangePasswordDto changePasswordDto)
        {
            var user = await _userRepository.GetUser(id);
            if (_claimDecorator.Role != "admin" && user.Id != id)
            {
                throw new UnauthorizedAccessException();
            }
            var currentPassword = HashPassword(user, changePasswordDto.CurrentPassword);
            if (currentPassword != user.PasswordHash)
            {
                throw new WrongPasswordException();
            }

            var newPasswordHash = HashPassword(user, changePasswordDto.NewPassword);
            user.PasswordHash = newPasswordHash;
            await _userRepository.UpdateUser(user);
        }

        public async Task AddToRole(int userId, int roleId)
        {
            var user = await _userRepository.GetUser(userId);
            if (user is null)
            {
                throw new UserNotFoundException();
            }
            var role = await _roleRepository.GetRole(roleId);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }

            if (user.Roles.Contains(role))
            {
                throw new UserHasRoleException();
            }
            user.Roles.Add(role);
            await _userRepository.UpdateUser(user);
        }

        public async Task DeleteFromRole(int userId, int roleId)
        {
            var user = await _userRepository.GetUser(userId);
            if (user is null)
            {
                throw new UserNotFoundException();
            }
            var role = await _roleRepository.GetRole(roleId);
            if (role is null)
            {
                throw new RoleNotFoundException();
            }

            if (user.Roles.Count > 1)
            {
                user.Roles.Remove(role);
                await _userRepository.UpdateUser(user);
            }
            else
            {
                throw new SingleRoleException();
            }
        }

        public async Task EditUser(int id, EditUserDto editUserDto)
        {
            var user = await _userRepository.GetUser(id);
            if (user is null)
            {
                throw new UserNotFoundException();
            }

            user = _mapper.Map<User>(editUserDto);
            await _userRepository.UpdateUser(user);
        }

        public async Task<ICollection<RoleDto>> GetRoles(string login)
        {
            var user = await _userRepository.GetUser(login);
            ICollection<RoleDto> roles = new List<RoleDto>();
            if (user is not null)
            {
                roles = user.Roles.Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToList();
            }
            return roles;
        }

        public async Task<MainUserInfoDto> GetMainUserInfo(int userId)
        {
            var user = await _userRepository.GetUser(userId);
            var mainUserInfoDto = _mapper.Map<MainUserInfoDto>(user);
            mainUserInfoDto.FullName = string.Join(' ', user.FirstName, user.SecondName, user.LastName);
            mainUserInfoDto.Gender = user.Gender switch
            {
                0 => "Male",
                1 => "Female",
                _ => "n/a"
            };
            return mainUserInfoDto;
        }

        public async Task<ICollection<UserForAdminPanelDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            var userForAdminPanelDtos = users.Select(u =>
            {
                var dto = _mapper.Map<UserForAdminPanelDto>(u);
                dto.FullName = string.Join(' ', u.LastName, u.FirstName, u.SecondName);
                dto.Roles = u.Roles.Select(r => r.Name).ToList();
                return dto;
            }).ToList();
            return userForAdminPanelDtos;
        }

        public async Task<UserDto> ReloginUser(ReloginDto reloginDto)
        {
            var user = await _userRepository.GetUser(_claimDecorator.Id);
            var role = user.Roles.SingleOrDefault(r => r.Id == reloginDto.ReloginRole);
            if (role is null)
            {
                throw new AccessForbiddenException();
            }
            return new UserDto
            {
                Id = _claimDecorator.Id,
                Name = _claimDecorator.Name,
                Role = role.Name
            };
        }

        private string HashPassword(User user, string password)
        {
            const int hashSize = 256 / 8;
            byte[] salt;
            var isRegistered = user.Id != 0;
            if (isRegistered)
            {
                salt = Convert.FromBase64String(user.HashingSalt);
            }
            else
            {
                salt = new byte[hashSize];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetNonZeroBytes(salt);
                }
                user.HashingSalt = Convert.ToBase64String(salt);
            }
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: hashSize));
            return hashed;
        }
    }
}
