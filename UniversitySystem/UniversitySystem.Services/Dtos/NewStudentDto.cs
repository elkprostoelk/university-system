using System;
using UniversitySystem.Data.Enums;

namespace UniversitySystem.Services.Dtos
{
    public class NewStudentDto
    {
        public DateTime EnteredOn { get; set; }
        
        public EducationalLevels EducationalLevel { get; set; }
        
        public EducationForms EducationForm { get; set; }
        
        public int FacultyId { get; set; }
        
        public int SpecialtyId { get; set; }
        
        public long UserId { get; set; }
    }
}