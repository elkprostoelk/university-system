using System;
using UniversitySystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UniversitySystem.Api
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<UniversitySystemDbContext>
    {
        public UniversitySystemDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{environment}.json", optional: false)
                .Build();
            var builder = new DbContextOptionsBuilder<UniversitySystemDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new UniversitySystemDbContext(builder.Options);
        }
    }
}