using System;
using System.ComponentModel.DataAnnotations;
using UniversitySystem.Data.Enums;

namespace UniversitySystem.Api.Models
{
    public class NewTeacherModel
    {
        [Required]
        public DateTime HiredOn { get; set; }
        
        public AcademicTitles? AcademicTitle { get; set; }
        
        public ScienceDegrees? ScienceDegree { get; set; }

        [Required]
        public int ChairId { get; set; }
        
        [Required]
        public int UserId { get; set; }
    }
}