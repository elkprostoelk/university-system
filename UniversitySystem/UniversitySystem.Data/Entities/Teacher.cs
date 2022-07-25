using System;
using System.Collections.Generic;
using UniversitySystem.Data.Enums;

namespace UniversitySystem.Data.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        
        public DateTime HiredOn { get; set; }
        
        public AcademicTitles? AcademicTitle { get; set; }
        
        public ScienceDegrees? ScienceDegree { get; set; }

        public int ChairId { get; set; }
        
        public Chair Chair { get; set; }
        
        public long UserId { get; set; }
        
        public User User { get; set; }
        
        public ICollection<Lesson> Lessons { get; set; }
    }
}