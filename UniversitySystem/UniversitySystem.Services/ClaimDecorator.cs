using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace UniversitySystem.Services
{
    public class ClaimDecorator : IClaimDecorator
    {
        public int Id => Convert.ToInt32(_user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

        public string Role => _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        private readonly ClaimsPrincipal _user;
        
        public ClaimDecorator(IHttpContextAccessor httpContextAccessor)
        {
            _user = httpContextAccessor.HttpContext.User;
        }
    }
}