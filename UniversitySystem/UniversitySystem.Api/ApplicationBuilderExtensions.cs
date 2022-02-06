using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Data;

namespace UniversitySystem.Api
{
    public static class ApplicationBuilderExtensions
    {
        public static void DatabaseEnsureCreated(this IApplicationBuilder app)
        {
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using var serviceScope = serviceScopeFactory.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<UniversitySystemDbContext>();
            dbContext.Database.EnsureCreated();
        }
    }
}
