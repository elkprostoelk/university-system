using System;

namespace UniversitySystem.Services.Dtos
{
    public class MainUserInfoDto
    {
        public string FirstName { get; set; }
        
        public string SecondName { get; set; }
        
        public string LastName { get; set; }
        
        public string FullName { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public string Gender { get; set; }
        
        public string Email { get; set; }
    }
}