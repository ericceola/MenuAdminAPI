using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MenuAdminAPI.Presentation.Configuration;

/// <summary>
/// Configuração de Autenticação JWT
/// </summary>
public static class AuthenticationConfiguration
{
    public static IServiceCollection AddAuthenticationConfiguration(
        this IServiceCollection services,
        string jwtSecret,
        string jwtIssuer,
        string jwtAudience)
    {
        var key = Encoding.ASCII.GetBytes(jwtSecret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception is SecurityTokenExpiredException)
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    // Aqui você pode adicionar lógica adicional de validação
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }
}
