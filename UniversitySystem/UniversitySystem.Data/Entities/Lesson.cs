using System;
using System.Collections.Generic;
using UniversitySystem.Data.Enums;

namespace UniversitySystem.Data.Entities
{
    public class Lesson
    {
        public long Id { get; set; }
        
        public string Name { get; set; }
        
        public LessonTypes LessonType { get; set; }
        
        public DateTime ScheduledOn { get; set; }
        
        public int TeacherId { get; set; }
        
        public Teacher Teacher { get; set; }
        
        public ICollection<Student> Participants { get; set; }
        
        public ICollection<LessonParticipant> LessonParticipants { get; set; }
    }
}