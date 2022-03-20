using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Api.Models
{
    public class NewRoleModel
    {
        [Required]
        public string RoleName { get; set; }
        
        [Required]
        public string FullRoleName { get; set; }
    }
}