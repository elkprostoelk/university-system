using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Api.Models
{
    public class EditRoleModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string FullRoleName { get; set; }
    }
}