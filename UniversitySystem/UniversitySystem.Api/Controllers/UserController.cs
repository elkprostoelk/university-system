using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Api.Models;
using UniversitySystem.Services;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        
        public UserController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterModel registerModel)
        {
            var registerDto = _mapper.Map<RegisterDto>(registerModel);
            var result = await _userService.RegisterUser(registerDto);
            if (result.IsSuccessful)
            {
                return Created(nameof(RegisterUser), result.ResultObject);
            }
            foreach (var (key, value) in result.Errors)
            {
                ModelState.AddModelError(key, value);
            }
            return BadRequest();
        }

        [Authorize(Roles = "admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _userService.GetAllUsers();
            return Ok(allUsers);
        }

        [Authorize]
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetMainUserInfo(int userId)
        {
            var result = await _userService.GetMainUserInfo(userId);
            if (result.IsSuccessful)
            {
                return Ok(result.ResultObject);
            }
            foreach (var (key, value) in result.Errors)
            {
                ModelState.AddModelError(key, value);
            }
            return BadRequest();
        }

        [HttpGet("roles/{login}")]
        public async Task<IActionResult> GetUserRoles(string login)
        {
            var roles = await _userService.GetRoles(login);
            return Ok(roles);
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("add-to-role/{userId:int}/{roleId:int}")]
        public async Task<IActionResult> AddToRole(int userId, int roleId)
        {
            var result = await _userService.AddToRole(userId, roleId);
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

        [Authorize(Roles = "admin")]
        [HttpPatch("delete-from-role/{userId:int}/{roleId:int}")]
        public async Task<IActionResult> DeleteFromRole(int userId, int roleId)
        {
            var result = await _userService.DeleteFromRole(userId, roleId);
            if (result.IsSuccessful)
            {
                return NoContent();
            }
            foreach (var (key, value) in result.Errors)
            {
                ModelState.AddModelError(key, value);
            }
            return BadRequest();
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditUserInfo(int id, EditUserModel editUserModel)
        {
            var editUserDto = _mapper.Map<EditUserDto>(editUserModel);
            var result = await _userService.EditUser(id, editUserDto);
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

        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (result.IsSuccessful)
            {
                return NoContent();
            }
            foreach (var (key, value) in result.Errors)
            {
                ModelState.AddModelError(key, value);
            }
            return BadRequest();
        }
    }
}