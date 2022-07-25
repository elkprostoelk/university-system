using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UniversitySystem.Services.Interfaces;

namespace UniversitySystem.Services.ServiceImplementations
{
    public class ClaimDecorator : IClaimDecorator
    {
        public long Id => Convert.ToInt64(_user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

        public string Name => _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        
        public string Role => _user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        private readonly ClaimsPrincipal _user;
        
        public ClaimDecorator(IHttpContextAccessor httpContextAccessor)
        {
            _user = httpContextAccessor?.HttpContext?.User;
        }
    }
}