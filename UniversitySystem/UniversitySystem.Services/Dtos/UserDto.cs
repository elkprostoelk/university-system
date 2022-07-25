using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Services.Dtos
{
    public class UserDto
    {
        public long Id { get; set; }
        
        public string Name { get; set; }

        public string Role { get; set; }
        
        public string FullRoleName { get; set; }
    }
}
