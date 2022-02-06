using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Data.Exceptions
{
    public class UserExistsException : Exception
    {
        public override string Message => "User already exists!";
    }
}
