using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

namespace UniversitySystem.Api
{
    public static class ServiceProviderExtensions
    {
        public static void AddRsaAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(provider =>
            {
                RSA rsa = RSA.Create();
                rsa.ImportRSAPublicKey(
                    source: Convert.FromBase64String(configuration["Jwt:PublicKey"]),
                    bytesRead: out _);
                return new RsaSecurityKey(rsa);
            });
            services.AddAuthentication()
                .AddJwtBearer("Asymmetric", options =>
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
    }
}
