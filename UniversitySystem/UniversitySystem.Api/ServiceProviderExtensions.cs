using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UniversitySystem.Data;
using UniversitySystem.Data.Interfaces;
using UniversitySystem.Data.Repositories;
using UniversitySystem.Services.Interfaces;
using UniversitySystem.Services.ServiceImplementations;

namespace UniversitySystem.Api
{
    public static class ServiceProviderExtensions
    {
        public static void AddRsaAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(provider =>
            {
                var rsa = RSA.Create();
                rsa.ImportRSAPublicKey(
                    source: Convert.FromBase64String(configuration["Jwt:PublicKey"]),
                    bytesRead: out _);
                return new RsaSecurityKey(rsa);
            });
            services.AddAuthentication(auth=>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer("Bearer", options =>
                {
                    SecurityKey rsa = services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();
                    options.IncludeErrorDetails = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = rsa,
                        ValidAudience = "university-system",
                        ValidIssuer = "university-system",
                        RequireSignedTokens = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true
                    };
                });
        }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddRouting(options => options.LowercaseUrls = true);
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddDbContext<UniversitySystemDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();

            services.AddScoped<IClaimDecorator, ClaimDecorator>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();

            services.AddCors();
            services.AddMvc();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "University System", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter your token here",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
    }
}
