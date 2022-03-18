using System;

namespace UniversitySystem.Services.Exceptions
{
    public class SingleRoleException : Exception
    {
        public override string Message => "You cannot delete the only user role!";
    }
}