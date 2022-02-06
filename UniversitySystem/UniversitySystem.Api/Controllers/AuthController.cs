using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UniversitySystem.Api.Models;
using UniversitySystem.Data.Exceptions;
using UniversitySystem.Services;
using UniversitySystem.Services.Dtos;

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
                return Ok(new { jwt = token });
            }
            catch (UserNotFoundException e)
            {
                return NotFound();
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized();
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            try
            {
                var registerDto = _mapper.Map<RegisterDto>(registerModel);
                var user = await _userService.RegisterUser(registerDto);
                string token = GenerateToken(user.Login, user.Id, user.Role);
                return Created(nameof(Register), new { jwt = token });
            }
            catch (UserExistsException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
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
