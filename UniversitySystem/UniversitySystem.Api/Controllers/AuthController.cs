using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UniversitySystem.Api.Models;
using UniversitySystem.Services;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Interfaces;

namespace UniversitySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthController(
            IUserService userService, 
            IMapper mapper,
            IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var loginDto = _mapper.Map<LoginDto>(loginModel);
            var result = await _userService.LoginUser(loginDto);
            if (result.IsSuccessful && result.ResultObject is UserDto user)
            {
                var token = GenerateToken(user.Name, user.Id, user.Role, user.FullRoleName);
                return Ok(new {jwt = token});
            }

            foreach (var (key, value) in result.Errors)
            {
                ModelState.AddModelError(key, value);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPost("relogin")]
        public async Task<IActionResult> Relogin(ReloginModel reloginModel)
        {
            var reloginDto = _mapper.Map<ReloginDto>(reloginModel);
            var result = await _userService.ReloginUser(reloginDto);
            if (result.IsSuccessful && result.ResultObject is UserDto userDto)
            {
                var token = GenerateToken(userDto.Name, userDto.Id, userDto.Role, userDto.FullRoleName);
                return Ok(new {jwt = token});
            }

            foreach (var (key, value) in result.Errors)
            {
                ModelState.AddModelError(key, value);
            }
            return BadRequest();
        }

        [Authorize]
        [HttpPut("change-password/{id:int}")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordModel changePasswordModel)
        {
            var changePasswordDto = _mapper.Map<ChangePasswordDto>(changePasswordModel);
            var result = await _userService.ChangePassword(id, changePasswordDto);
            if (result.IsSuccessful)
            {
                return Ok();
            }
            foreach (var (key, value) in result.Errors)
            {
                ModelState.AddModelError(key, value);
            }
            return BadRequest();
        }

        private string GenerateToken(string userName, long id, string role, string roleName)
        {
            RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(
                source: Convert.FromBase64String(_configuration["Jwt:PrivateKey"]),
                bytesRead: out _); 

            var signingCredentials = new SigningCredentials(
                key: new RsaSecurityKey(rsa),
                algorithm: SecurityAlgorithms.RsaSha256
            );
            DateTime jwtDate = DateTime.Now;

            var jwt = new JwtSecurityToken(
                audience: "university-system",
                issuer: "university-system",
                claims: new Claim[] 
                { 
                    new Claim(ClaimTypes.NameIdentifier, id.ToString(), ClaimValueTypes.Integer64),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(ClaimTypes.UserData, roleName)
                },
                notBefore: jwtDate,
                expires: jwtDate.AddHours(24),
                signingCredentials: signingCredentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }
    }
}
