using System.Threading.Tasks;
using AutoMapper;
using UniversitySystem.Data.Entities;
using UniversitySystem.Data.Repositories;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Interfaces;

namespace UniversitySystem.Services.ServiceImplementations
{
    public class TeacherService : ITeacherService
    {
        private readonly IMapper _mapper;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IUserRepository _userRepository;
        
        public TeacherService(
            IMapper mapper,
            ITeacherRepository teacherRepository,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _teacherRepository = teacherRepository;
            _userRepository = userRepository;
        }
        
        public async Task CreateTeacher(NewTeacherDto newTeacherDto)
        {
            var newTeacher = _mapper.Map<Teacher>(newTeacherDto);
            var user = await _userRepository.GetUser(newTeacherDto.UserId);
            await _teacherRepository.AddTeacher(newTeacher);
            user.TeacherId = newTeacher.Id;
            await _userRepository.UpdateUser(user);
        }
    }
}