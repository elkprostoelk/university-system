using System;
using System.Collections.Generic;
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
        
        public long UserId { get; set; }
        
        public User User { get; set; }
        
        public ICollection<Lesson> Lessons { get; set; }
        
        public ICollection<LessonParticipant> LessonParticipants { get; set; }
    }
}