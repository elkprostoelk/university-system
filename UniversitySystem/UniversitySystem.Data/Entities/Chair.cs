using System.Collections.Generic;

namespace UniversitySystem.Data.Entities
{
    public class Chair
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int FacultyId { get; set; }
        
        public Faculty Faculty { get; set; }

        public ICollection<Specialty> Specialties { get; set; }
        
        public ICollection<Teacher> Teachers { get; set; }
    }
}