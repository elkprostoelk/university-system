using System;
using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Api.Models
{
    public class RegisterModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username should be 3-20 symbols!")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "First name should be less than 20 symbols!")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Last name should be less than 20 symbols!")]
        public string LastName { get; set; }

        [StringLength(20, ErrorMessage = "Second name should be less than 20 symbols!")]
        public string SecondName { get; set; }

        [Required]
        public byte Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password should be 8-20 symbols!")]
        public string Password { get; set; }

        [Required]
        public string PassportNumber { get; set; }
    }
}
