namespace UniversitySystem.Data.Entities
{
    public class LessonParticipant
    {
        public long LessonId { get; set; }
        
        public Lesson Lesson { get; set; }
        
        public long ParticipantId { get; set; }
        
        public Student Participant { get; set; }
        
        public bool Present { get; set; }
    }
}