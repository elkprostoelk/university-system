using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Api.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Enter a current password!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password should be 8-20 symbols!")]
        public string CurrentPassword { get; set; }
        
        [Required(ErrorMessage = "Enter a new password!")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password should be 8-20 symbols!")]
        public string NewPassword { get; set; }
    }
}