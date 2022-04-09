using System.Threading.Tasks;
using UniversitySystem.Data.Entities;

namespace UniversitySystem.Data.Interfaces
{
    public interface ITeacherRepository
    {
        public Task RemoveAllByUserId(int userId);
        public Task AddTeacher(Teacher newTeacher);
    }
}