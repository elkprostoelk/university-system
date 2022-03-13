using System;

namespace UniversitySystem.Services.Exceptions
{
    public class AdminRoleDeletingException : Exception
    {
        public override string Message => "Admin role cannot be deleted!";
    }
}