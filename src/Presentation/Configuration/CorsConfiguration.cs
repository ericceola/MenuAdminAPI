namespace MenuAdminAPI.Presentation.Configuration;

/// <summary>
/// Configuração de CORS (Cross-Origin Resource Sharing)
/// </summary>
public static class CorsConfiguration
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
            ?? new[] { "http://localhost:3000", "http://localhost:5173", "https://localhost:5173" };

        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy
                    .WithOrigins(corsOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition", "X-Total-Count", "X-Page-Number", "X-Page-Size");
            });

            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app, bool allowAll = false)
    {
        var policyName = allowAll ? "AllowAll" : "AllowSpecificOrigins";
        app.UseCors(policyName);
        return app;
    }
}
