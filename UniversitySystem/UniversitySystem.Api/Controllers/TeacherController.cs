using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Api.Models;
using UniversitySystem.Services;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ITeacherService _teacherService;

        public TeacherController(
            IMapper mapper,
            ITeacherService teacherService)
        {
            _mapper = mapper;
            _teacherService = teacherService;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateTeacher(NewTeacherModel newTeacherModel)
        {
            var newTeacherDto = _mapper.Map<NewTeacherDto>(newTeacherModel);
            await _teacherService.CreateTeacher(newTeacherDto);
            return Created(nameof(CreateTeacher), newTeacherDto);
        }
    }
}