using System.Threading.Tasks;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Services
{
    public interface ITeacherService
    {
        public Task CreateTeacher(NewTeacherDto newTeacherDto);
    }
}