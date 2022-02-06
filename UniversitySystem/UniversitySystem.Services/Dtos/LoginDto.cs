using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Services.Dtos
{
    public class LoginDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Login should be 3-20 symbols!")]
        public string Login { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Login should be 8-20 symbols!")]
        public string Password { get; set; }
    }
}
