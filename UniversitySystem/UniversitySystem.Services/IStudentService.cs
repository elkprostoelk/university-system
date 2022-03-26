using System.Threading.Tasks;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Services
{
    public interface IStudentService
    {
        public Task CreateStudent(NewStudentDto newStudentDto);
    }
}