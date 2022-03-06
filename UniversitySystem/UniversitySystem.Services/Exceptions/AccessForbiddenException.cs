using System;

namespace UniversitySystem.Services.Exceptions
{
    public class AccessForbiddenException : Exception
    {
        public override string Message => "Access forbidden!";
    }
}