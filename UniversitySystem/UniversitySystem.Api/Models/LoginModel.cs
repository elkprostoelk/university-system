using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Api.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Login should be 3-20 symbols!")]
        public string Login { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password should be 8-20 symbols!")]
        public string Password { get; set; }
        
        public int RoleId { get; set; }
    }
}
