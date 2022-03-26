using System.Threading.Tasks;
using UniversitySystem.Data.Entities;

namespace UniversitySystem.Data.Repositories
{
    public interface IStudentRepository
    {
        public Task RemoveAllByUserId(int userId);
        public Task AddStudent(Student newStudent);
    }
}