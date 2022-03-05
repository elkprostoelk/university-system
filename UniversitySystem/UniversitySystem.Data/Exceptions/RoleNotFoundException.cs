using System;

namespace UniversitySystem.Data.Exceptions
{
    public class RoleNotFoundException : Exception
    {
        public override string Message { get; } = "Role doesn't exist!";
    }
}