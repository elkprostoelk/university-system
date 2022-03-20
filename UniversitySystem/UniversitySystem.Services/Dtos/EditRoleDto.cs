using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.Dtos
{
    public class EditRoleDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string FullRoleName { get; set; }
    }
}