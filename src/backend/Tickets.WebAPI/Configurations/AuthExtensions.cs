using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Tickets.Infrastructure.Security.Jwt;

namespace Tickets.WebAPI.Configuration;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(
            configuration.GetSection("Jwt"));

        var jwtSettings = configuration
            .GetSection("Jwt")
            .Get<JwtSettings>();

        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    //RoleClaimType = ClaimTypes.Role
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized; 
                        context.Response.ContentType = "application/json"; 
                        var response = new 
                        { 
                            success = false, 
                            errors = new[] 
                            { 
                                "Authentication is required." 
                            } 
                        }; 
                        
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden; 
                        context.Response.ContentType = "application/json"; 
                        var response = new 
                        { 
                            success = false, 
                            errors = new[] 
                            { 
                                "You do not have permission to access this resource." 
                            } 
                        }; 
                        
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                };
            });

        return services;
    }
}