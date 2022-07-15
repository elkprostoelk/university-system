using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using UniversitySystem.Data;
using UniversitySystem.Data.Interfaces;
using UniversitySystem.Services.Dtos;
using UniversitySystem.Services.Interfaces;
using UniversitySystem.Services.ServiceImplementations;

namespace UniversitySystem.Api
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureApp(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(b => b
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin());
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => 
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UniversitySystem.Api v1"));
            }

            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature!.Error;
                Log.Fatal(exception!, "An exception occured while processing the request");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new {error = exception.Message});
            }));
            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        
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
