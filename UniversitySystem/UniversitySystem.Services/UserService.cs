using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UniversitySystem.Data;
using UniversitySystem.Data.Exceptions;
using UniversitySystem.Data.Repositories;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> LoginUser(LoginDto loginDto)
        {
            var user = await _userRepository.GetUser(loginDto.Login);
            var hashedPassword = HashPassword(user, loginDto.Password);
            if (user is null)
            {
                throw new UserNotFoundException();
            }
            if (user.PasswordHash == hashedPassword)
            {
                return new UserDto
                {
                    Id = user.Id,
                    Login = user.UserName,
                    Role = String.Empty
                };
            }
            throw new UnauthorizedAccessException();
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
                Login = user.UserName,
                Role = String.Empty
            };
        }

        public async Task DeleteUser(int id)
        {
            var user = await _userRepository.GetUser(id);
            if (user is null)
            {
                throw new UserNotFoundException();
            }

            await _userRepository.DeleteUser(user);
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
