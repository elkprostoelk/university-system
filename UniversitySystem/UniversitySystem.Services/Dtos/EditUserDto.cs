using System;

namespace UniversitySystem.Services.Dtos
{
    public class EditUserDto
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SecondName { get; set; }

        public byte Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string PassportNumber { get; set; }
    }
}