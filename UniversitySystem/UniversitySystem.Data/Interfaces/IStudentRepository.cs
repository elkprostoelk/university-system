using System.Collections.Generic;
using System.Threading.Tasks;
using UniversitySystem.Data.Entities;

namespace UniversitySystem.Data.Interfaces
{
    public interface IStudentRepository
    {
        public Task RemoveAllByUserId(long userId);
        public Task AddStudent(Student newStudent);
        Task<ICollection<Student>> GetStudents(ICollection<int> studentsIds);
    }
}