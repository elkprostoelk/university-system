using System;

namespace UniversitySystem.Services.Dtos
{
    public class UserForAdminPanelDto
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }
        
        public string FullName { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}