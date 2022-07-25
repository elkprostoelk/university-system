using System;
using System.Collections.Generic;
using UniversitySystem.Data.Enums;

namespace UniversitySystem.Services.Dtos
{
    public class NewLessonDTO
    {
        public string Name { get; set; }
        
        public DateTime ScheduledOn { get; set; }
        
        public LessonTypes LessonType { get; set; }
        
        public ICollection<int> ParticipantIds { get; set; }
        
        public int TeacherId { get; set; }
    }
}