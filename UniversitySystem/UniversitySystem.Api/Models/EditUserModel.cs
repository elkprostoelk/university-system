using System;
using System.ComponentModel.DataAnnotations;

namespace UniversitySystem.Api.Models
{
    public class EditUserModel
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Username should be 3-20 symbols!")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First name should be less than 20 symbols!")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Last name should be less than 20 symbols!")]
        public string LastName { get; set; }

        [StringLength(100, ErrorMessage = "Second name should be less than 20 symbols!")]
        public string SecondName { get; set; }

        [Required]
        public byte Gender { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PassportNumber { get; set; }
    }
}