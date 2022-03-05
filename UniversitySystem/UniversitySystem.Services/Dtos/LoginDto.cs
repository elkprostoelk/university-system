using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.Dtos
{
    public class LoginDto
    {
        public string Login { get; set; }

        public string Password { get; set; }
        
        public int RoleId { get; set; }
    }
}
