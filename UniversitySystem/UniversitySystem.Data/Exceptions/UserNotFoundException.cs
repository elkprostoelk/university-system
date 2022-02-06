using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversitySystem.Data.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public override string Message => "User doesn't exist!";
    }
}
