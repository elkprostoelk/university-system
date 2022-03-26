using System;
using UniversitySystem.Data.Enums;

namespace UniversitySystem.Services.Dtos
{
    public class NewTeacherDto
    {
        public DateTime HiredOn { get; set; }
        
        public AcademicTitles? AcademicTitle { get; set; }
        
        public ScienceDegrees? ScienceDegree { get; set; }

        public int ChairId { get; set; }
        
        public int UserId { get; set; }
    }
}