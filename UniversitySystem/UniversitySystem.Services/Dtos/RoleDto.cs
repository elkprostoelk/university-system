using System.Collections.Generic;

namespace UniversitySystem.Services.Dtos
{
    public class RoleDto
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string FullName { get; set; }
        
        public ICollection<UserDto> Users { get; set; }
    }
}