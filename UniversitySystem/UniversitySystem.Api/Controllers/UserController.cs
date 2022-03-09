using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UniversitySystem.Api.Models;
using UniversitySystem.Data.Exceptions;
using UniversitySystem.Services;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Api.Controllers
{
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterModel registerModel)
        {
            try
            {
                var registerDto = _mapper.Map<RegisterDto>(registerModel);
                var user = await _userService.RegisterUser(registerDto);
                return Created(nameof(RegisterUser), user);
            }
            catch (UserExistsException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An exception occured while processing the request");
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpGet("roles/{login}")]
        public async Task<IActionResult> GetUserRoles(string login)
        {
            try
            {
                var roles = await _userService.GetRoles(login);
                return Ok(roles);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An exception occured while processing the request");
                return StatusCode(500);
            }
        }

        [HttpPatch("add-to-role/{userId:int}/{roleId:int}")]
        public async Task<IActionResult> AddToRole(int userId, int roleId)
        {
            try
            {
                await _userService.AddToRole(userId, roleId);
                return Ok();
            }
            catch (UserNotFoundException e)
            {
                return NotFound(new { e.Message });
            }
            catch (RoleNotFoundException e)
            {
                return NotFound(new { e.Message });
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An exception occured while processing the request");
                return StatusCode(500);
            }
        }

        [HttpPatch("delete-from-role/{userId:int}/{roleId:int}")]
        public async Task<IActionResult> DeleteFromRole(int userId, int roleId)
        {
            try
            {
                await _userService.DeleteFromRole(userId, roleId);
                return NoContent();
            }
            catch (UserNotFoundException e)
            {
                return NotFound(new { e.Message });
            }
            catch (RoleNotFoundException e)
            {
                return NotFound(new { e.Message });
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An exception occured while processing the request");
                return StatusCode(500);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditUserInfo(int id, EditUserModel editUserModel)
        {
            try
            {
                var editUserDto = _mapper.Map<EditUserDto>(editUserModel);
                await _userService.EditUser(id, editUserDto);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An exception occured while processing the request");
                return StatusCode(500);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return NoContent();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An exception occured while processing the request");
                return StatusCode(500);
            }
        }
    }
}