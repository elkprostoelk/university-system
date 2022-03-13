using System;

namespace UniversitySystem.Services.Dtos
{
    public class SelfDeletingException : Exception
    {
        public override string Message => "User cannot delete himself!";
    }
}