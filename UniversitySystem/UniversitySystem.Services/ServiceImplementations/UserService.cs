using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using UniversitySystem.Data.Entities;
using UniversitySystem.Data.Repositories;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Interfaces;

namespace UniversitySystem.Services.ServiceImplementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IClaimDecorator _claimDecorator;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;

        public UserService(
            IMapper mapper,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IClaimDecorator claimDecorator)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _claimDecorator = claimDecorator;
        }

        public async Task<ServiceResult> LoginUser(LoginDto loginDto)
        {
            var user = await _userRepository.GetUser(loginDto.Login);
            var hashedPassword = HashPassword(user, loginDto.Password);
            var result = new ServiceResult();
            if (user is null)
            {
                result.IsSuccessful = false;
                result.Errors.Add("UserNotExists", $"User {loginDto.Login} doesn't exist!");
            }
            else
            {
                if (user.PasswordHash != hashedPassword)
                {
                    result.IsSuccessful = false;
                    result.Errors.Add("WrongPassword", "Wrong password!");
                }
                else
                {
                    var role = await _roleRepository.GetRole(loginDto.RoleId);
                    if (!user.Roles.Contains(role))
                    {
                        result.IsSuccessful = false;
                        result.Errors.Add("NotInRole", $"User {loginDto.Login} does not have this role!");
                    }
                    else
                    {
                        result.IsSuccessful = true;
                        result.ResultObject = new UserDto
                        {
                            Id = user.Id,
                            Name = user.UserName,
                            Role = role.Name,
                            FullRoleName = role.FullName
                        };
                    }
                }
            }
            return result;
        }

        public async Task<ServiceResult> RegisterUser(RegisterDto registerDto)
        {
            var result = new ServiceResult();
            var userExists = await _userRepository.UserExists(registerDto.UserName);
            if (userExists)
            {
                result.IsSuccessful = false;
                result.Errors.Add("UserExists", $"User {registerDto.UserName} already exists!");
                return result;
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
            result.ResultObject = new UserDto
            {
                Id = user.Id,
                Name = user.UserName,
                Role = String.Empty,
                FullRoleName = String.Empty
            };
            return result;
        }

        public async Task<ServiceResult> DeleteUser(int id)
        {
            var result = new ServiceResult();
            if (_claimDecorator.Id == id)
            {
                result.IsSuccessful = false;
                result.Errors.Add("SelfDelete", "User cannot delete himself");
            }
            else
            {
                var user = await _userRepository.GetUser(id);
                if (user is null)
                {
                    result.IsSuccessful = false;
                    result.Errors.Add("UserNotExists", "User doesn't exist!");
                }
                else
                {
                    await _userRepository.DeleteUser(user);
                }
            }

            return result;
        }

        public async Task<ServiceResult> ChangePassword(int id, ChangePasswordDto changePasswordDto)
        {
            var result = new ServiceResult();
            var user = await _userRepository.GetUser(id);
            if (_claimDecorator.Role != "admin" && user.Id != id)
            {
                result.IsSuccessful = false;
                result.Errors.Add("ForbiddenAccess", "Only administrators can change other users' passwords!");
                var currentPassword = HashPassword(user, changePasswordDto.CurrentPassword);
                if (currentPassword != user.PasswordHash)
                {
                    result.IsSuccessful = false;
                    result.Errors.Add("WrongPassword", "The current password is wrong!");
                }
                else
                {
                    var newPasswordHash = HashPassword(user, changePasswordDto.NewPassword);
                    user.PasswordHash = newPasswordHash;
                    await _userRepository.UpdateUser(user);
                }
            }
            return result;
        }

        public async Task<ServiceResult> AddToRole(int userId, int roleId)
        {
            var result = new ServiceResult();
            var user = await _userRepository.GetUser(userId);
            if (user is null)
            {
                result.IsSuccessful = false;
                result.Errors.Add("UserNotFound", "User was not found!");
            }
            else
            {
                var role = await _roleRepository.GetRole(roleId);
                if (role is null)
                {
                    result.IsSuccessful = false;
                    result.Errors.Add("RoleNotFound", "Role was not found!");
                }
                else
                {
                    if (user.Roles.Contains(role))
                    {
                        result.IsSuccessful = false;
                        result.Errors.Add("UserHasRole", "User has already got this role!");
                    }
                    else
                    {
                        user.Roles.Add(role);
                        await _userRepository.UpdateUser(user);
                    }
                }
            }
            return result;
        }

        public async Task<ServiceResult> DeleteFromRole(int userId, int roleId)
        {
            var result = new ServiceResult();
            var user = await _userRepository.GetUser(userId);
            if (user is null)
            {
                result.IsSuccessful = false;
                result.Errors.Add("UserNotFound", "User was not found");
            }
            else
            {
                var role = await _roleRepository.GetRole(roleId);
                if (role is null)
                {
                    result.IsSuccessful = false;
                    result.Errors.Add("RoleNotFound", "Role was not found");
                }
                else
                {
                    if (user.Roles.Count > 1)
                    {
                        user.Roles.Remove(role);
                        switch (role.Name)
                        {
                            case "student":
                            {
                                await _studentRepository.RemoveAllByUserId(user.Id);
                                break;
                            }
                            case "teacher":
                            {
                                await _teacherRepository.RemoveAllByUserId(user.Id);
                                break;
                            }
                        }
                        await _userRepository.UpdateUser(user);
                    }
                    else
                    {
                        result.IsSuccessful = false;
                        result.Errors.Add("SingleRole", "Role could not be deleted because it's the single role of this user");
                    }
                }
            }
            return result;
        }

        public async Task<ServiceResult> EditUser(int id, EditUserDto editUserDto)
        {
            var result = new ServiceResult();
            var user = await _userRepository.GetUser(id);
            if (user is null)
            {
                result.IsSuccessful = false;
                result.Errors.Add("UserNotFound", "User was not found");
            }
            else
            {
                user = _mapper.Map<User>(editUserDto);
                await _userRepository.UpdateUser(user);
            }
            return result;
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
                    Name = r.Name,
                    FullName = r.FullName
                }).ToList();
            }
            return roles;
        }

        public async Task<ServiceResult> GetMainUserInfo(int userId)
        {
            var result = new ServiceResult();
            var user = await _userRepository.GetUser(userId);
            if (user is null)
            {
                result.IsSuccessful = false;
                result.Errors.Add("UserNotExists", "User does not exist!");
            }
            else
            {
                var mainUserInfoDto = _mapper.Map<MainUserInfoDto>(user);
                mainUserInfoDto.FullName = string.Join(' ', user.FirstName, user.SecondName, user.LastName);
                mainUserInfoDto.Gender = user.Gender switch
                {
                    0 => "Male",
                    1 => "Female",
                    _ => "n/a"
                };
                result.ResultObject = mainUserInfoDto;
            }
            return result;
        }

        public async Task<ICollection<UserForAdminPanelDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            var userForAdminPanelDtos = users.Select(u =>
            {
                var dto = _mapper.Map<UserForAdminPanelDto>(u);
                dto.FullName = string.Join(' ', u.LastName, u.FirstName, u.SecondName);
                dto.Roles = u.Roles.Select(r => r.FullName).ToList();
                return dto;
            }).ToList();
            return userForAdminPanelDtos;
        }

        public async Task<ServiceResult> ReloginUser(ReloginDto reloginDto)
        {
            var user = await _userRepository.GetUser(_claimDecorator.Id);
            var role = user.Roles.SingleOrDefault(r => r.Id == reloginDto.ReloginRole);
            var result = new ServiceResult();
            if (role is null)
            {
                result.IsSuccessful = false;
                result.Errors.Add("NotInRole", $"User {user.UserName} does not have this role!");
                return result;
            }
            result.ResultObject = new UserDto
            {
                Id = _claimDecorator.Id,
                Name = _claimDecorator.Name,
                Role = role.Name,
                FullRoleName = role.FullName
            };
            return result;
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
