using System;

namespace UniversitySystem.Services.Exceptions
{
    public class WrongPasswordException : Exception
    {
        public override string Message { get; } = "Wrong password!";
    }
}