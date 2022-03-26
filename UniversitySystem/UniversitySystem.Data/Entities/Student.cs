using System;
using UniversitySystem.Data.Enums;

namespace UniversitySystem.Data.Entities
{
    public class Student
    {
        public int Id { get; set; }
        
        public DateTime EnteredOn { get; set; }
        
        public EducationalLevels EducationalLevel { get; set; }
        
        public EducationForms EducationForm { get; set; }
        
        public int FacultyId { get; set; }
        
        public Faculty Faculty { get; set; }
        
        public int SpecialtyId { get; set; }
        
        public Specialty Specialty { get; set; }
        
        public int UserId { get; set; }
        
        public User User { get; set; }
    }
}