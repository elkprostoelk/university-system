using System.Collections.Generic;

namespace UniversitySystem.Data.Entities
{
    public class Specialty
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public int FacultyId { get; set; }
        
        public Faculty Faculty { get; set; }
        
        public int ChairId { get; set; }
        
        public Chair Chair { get; set; }
        
        public ICollection<Student> Students { get; set; }
    }
}