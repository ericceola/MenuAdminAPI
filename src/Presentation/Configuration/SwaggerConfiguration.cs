using Microsoft.OpenApi.Models;

namespace MenuAdminAPI.Presentation.Configuration;

/// <summary>
/// Configuração do Swagger/OpenAPI
/// </summary>
public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MenuAdminAPI",
                Version = "v1",
                Description = "API de Gestão de Cardápio para Múltiplos Estabelecimentos",
                Contact = new OpenApiContact
                {
                    Name = "Menu Admin Team",
                    Email = "support@menuadmin.com"
                },
                License = new OpenApiLicense
                {
                    Name = "MIT"
                }
            });

            // Configurar segurança JWT
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    new string[] { }
                }
            });

            // Incluir comentários XML
            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }

            // Ordenar operações por tag
            options.OrderActionsBy(x => x.RelativePath);
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "MenuAdminAPI v1");
            options.RoutePrefix = "swagger";
            options.DisplayOperationId();
            options.EnableDeepLinking();
            options.EnableFilter();
            options.ShowExtensions();
            options.DefaultModelsExpandDepth(2);
            options.DefaultModelExpandDepth(2);
        });

        return app;
    }
}
