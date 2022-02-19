using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UniversitySystem.Services;

namespace UniversitySystem.Api.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        
        public AdminController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpDelete("delete-user/{id:int}")]
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