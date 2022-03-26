using System.Threading.Tasks;
using UniversitySystem.Data.Entities;

namespace UniversitySystem.Data.Repositories
{
    public interface ITeacherRepository
    {
        public Task RemoveAllByUserId(int userId);
        public Task AddTeacher(Teacher newTeacher);
    }
}