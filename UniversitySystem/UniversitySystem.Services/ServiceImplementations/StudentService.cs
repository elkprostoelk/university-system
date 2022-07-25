using System.Threading.Tasks;
using AutoMapper;
using UniversitySystem.Data.Entities;
using UniversitySystem.Data.Interfaces;
using UniversitySystem.Data.Repositories;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Interfaces;

namespace UniversitySystem.Services.ServiceImplementations
{
    public class StudentService : IStudentService
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        
        public StudentService(
            IMapper mapper,
            IStudentRepository studentRepository,
            IUserRepository userRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _userRepository = userRepository;
        }
        
        public async Task CreateStudent(NewStudentDto newStudentDto)
        {
            var newStudent = _mapper.Map<Student>(newStudentDto);
            var user = await _userRepository.GetUser(newStudentDto.UserId);
            await _studentRepository.AddStudent(newStudent);
            user.StudentRole = newStudent;
            await _userRepository.UpdateUser(user);
        }
    }
}