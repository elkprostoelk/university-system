using System;

namespace UniversitySystem.Services.Exceptions
{
    public class UserHasRoleException : Exception
    {
        public override string Message => "This user already has this role!";
    }
}