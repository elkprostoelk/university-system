using System.Threading.Tasks;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Services.Interfaces
{
    public interface ITeacherService
    {
        public Task CreateTeacher(NewTeacherDto newTeacherDto);
    }
}