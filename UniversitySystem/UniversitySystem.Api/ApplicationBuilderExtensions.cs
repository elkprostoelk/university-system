using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using UniversitySystem.Data;
using UniversitySystem.Data.Repositories;
using UniversitySystem.Services;
using UniversitySystem.Services.Dtos;

namespace UniversitySystem.Api
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task DatabaseEnsureCreated(this IApplicationBuilder app)
        {
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using var serviceScope = serviceScopeFactory.CreateScope();
            await using var dbContext = serviceScope.ServiceProvider.GetService<UniversitySystemDbContext>();
            if (dbContext is not null)
            {
                await dbContext.Database.EnsureCreatedAsync();
                var httpAccessor = serviceScope.ServiceProvider.GetService<IHttpContextAccessor>();
                var claimDecorator = new ClaimDecorator(httpAccessor);
                var userRepository = serviceScope.ServiceProvider.GetService<IUserRepository>();
                var userService = serviceScope.ServiceProvider.GetService<IUserService>();
                if (userRepository is not null &&
                    userService is not null &&
                    !(await userRepository.UserExists("admin")))
                {
                    var adminUserDto = new RegisterDto()
                    {   
                        UserName = "admin",
                        BirthDate = new DateTime(1990, 01, 01),
                        Email = "admin@admin.com",
                        FirstName = "Адміністратор",
                        Gender = 0,
                        LastName = "Адміністратор",
                        PassportNumber = "000000000",
                        Password = "THINKY7teeth"
                    };
                    var admin = (UserDto)(await userService.RegisterUser(adminUserDto)).ResultObject;
                    await userService.AddToRole(admin.Id, 1);
                    Log.Information("User account \"admin\" was successfully initialized");
                }
            }
        }
    }
}
