using System.Threading.Tasks;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Services.Interfaces
{
    public interface IStudentService
    {
        public Task CreateStudent(NewStudentDto newStudentDto);
    }
}