using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Services.Dtos
{
    public class RegisterDto
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SecondName { get; set; }

        public byte Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PassportNumber { get; set; }
    }
}
