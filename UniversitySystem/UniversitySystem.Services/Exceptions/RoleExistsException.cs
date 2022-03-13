using System;

namespace UniversitySystem.Services.Exceptions
{
    public class RoleExistsException : Exception
    {
        public override string Message => "Role with this name already exists!";
    }
}