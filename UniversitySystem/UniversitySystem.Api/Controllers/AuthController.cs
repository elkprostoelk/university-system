using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using UniversitySystem.Api.Models;
using UniversitySystem.Data.Exceptions;
using UniversitySystem.Services;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Exceptions;

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
            try
            {
                var loginDto = _mapper.Map<LoginDto>(loginModel);
                var user = await _userService.LoginUser(loginDto);
                string token = GenerateToken(user.Login, user.Id, user.Role);
                return Ok(new {jwt = token});
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (AccessForbiddenException)
            {
                return Forbid();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An exception occured while processing the request");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPut("change-password/{id:int}")]
        public async Task<IActionResult> ChangePassword(int id, ChangePasswordModel changePasswordModel)
        {
            try
            {
                var changePasswordDto = _mapper.Map<ChangePasswordDto>(changePasswordModel);
                await _userService.ChangePassword(id, changePasswordDto);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                ModelState.AddModelError("", "Only admins can change any user's password!");
                return BadRequest();
            }
            catch (WrongPasswordException e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An exception occured while processing the request");
                return StatusCode(500);
            }
        }

        private string GenerateToken(string userName, int id, string role)
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
                    new Claim(ClaimTypes.Role, role)
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
