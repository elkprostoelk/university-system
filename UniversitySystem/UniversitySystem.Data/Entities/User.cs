using System;
using System.Collections.Generic;

namespace UniversitySystem.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SecondName { get; set; }

        public byte Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Email { get; set; }

        public string HashingSalt { get; set; }

        public string PasswordHash { get; set; }

        public string PassportNumber { get; set; }
        
        public ICollection<Role> Roles { get; set; }
    }
}
