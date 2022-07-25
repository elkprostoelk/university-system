using System;
using System.ComponentModel.DataAnnotations;
using UniversitySystem.Data.Enums;

namespace UniversitySystem.Api.Models
{
    public class NewStudentModel
    {
        [Required]
        public DateTime EnteredOn { get; set; }
        
        [Required]
        public EducationalLevels EducationalLevel { get; set; }
        
        [Required]
        public EducationForms EducationForm { get; set; }
        
        [Required]
        public int FacultyId { get; set; }
        
        [Required]
        public int SpecialtyId { get; set; }
        
        [Required]
        public long UserId { get; set; }
    }
}