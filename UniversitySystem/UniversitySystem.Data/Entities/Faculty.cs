using System.Collections.Generic;

namespace UniversitySystem.Data.Entities
{
    public class Faculty
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public ICollection<Specialty> Specialties { get; set; }

        public ICollection<Student> Students { get; set; }
        
        public ICollection<Chair> Chairs { get; set; }
    }
}