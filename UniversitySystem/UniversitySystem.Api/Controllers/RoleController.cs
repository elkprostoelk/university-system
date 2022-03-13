using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UniversitySystem.Data.Exceptions;
using UniversitySystem.Services;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Exceptions;

namespace UniversitySystem.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roleDtos = await _roleService.GetAllRoles();
                return Ok(roleDtos);
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An exception occured while processing the request");
                return StatusCode(500);
            }
        }

        [HttpDelete("{roleId:int}")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            try
            {
                await _roleService.DeleteRole(roleId);
                return NoContent();
            }
            catch (RoleNotFoundException e)
            {
                return BadRequest(new {e.Message});
            }
            catch (AdminRoleDeletingException e)
            {
                return BadRequest(new {e.Message});
            }
            catch (Exception e)
            {
                Log.Fatal(e, "An exception occured while processing the request");
                return StatusCode(500);
            }
        }
    }
}