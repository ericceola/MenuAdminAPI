using MenuAdminAPI.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/MenuAdminAPI-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    // Adicionar serviços
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Configurar Infraestrutura
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    // Se não encontrar em appsettings, tentar variável de ambiente
    if (string.IsNullOrEmpty(connectionString))
    {
        connectionString = Environment.GetEnvironmentVariable("ConnectionString") 
            ?? Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTIONSTRING")
            ?? throw new InvalidOperationException("Connection string não foi encontrada em appsettings ou variáveis de ambiente.");
    }

    Log.Information($"Usando connection string: {(connectionString.Length > 50 ? connectionString.Substring(0, 50) + "..." : connectionString)}");

    builder.Services.AddInfrastructure(connectionString);

    // Configurar JWT
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var jwtSecret = jwtSettings["Secret"];

    if (string.IsNullOrEmpty(jwtSecret))
    {
        jwtSecret = Environment.GetEnvironmentVariable("JwtSecret") 
            ?? "your-super-secret-key-that-must-be-at-least-32-characters-long-for-security";
    }

    Log.Information("JWT Secret configurado com sucesso");

    var jwtIssuer = jwtSettings["Issuer"] ?? "MenuAdminAPI";
    var jwtAudience = jwtSettings["Audience"] ?? "MenuAdminAPI";
    var jwtExpirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

    builder.Services.AddApplicationServices();

    // Configurar SendGrid Options
    builder.Services.Configure<MenuAdminAPI.Application.Options.SendGridOptions>(
        builder.Configuration.GetSection("SendGrid"));

    Log.Information("SendGrid Options configurado com sucesso");

    // Configurar Autenticação JWT
    var key = Encoding.ASCII.GetBytes(jwtSecret);
    builder.Services.AddAuthentication(options =>
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
    });

    // Configurar CORS
    builder.Services.AddCors(options =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "*" };
        
        Log.Information($"CORS Allowed Origins: {string.Join(", ", allowedOrigins)}");
        
        options.AddPolicy("AllowAll", policy =>
        {
            if (allowedOrigins.Contains("*"))
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            }
            else
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            }
        });
    });

    var app = builder.Build();

    // Configurar pipeline HTTP
   
        app.UseSwagger();
        app.UseSwaggerUI();
    

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Adicionar middleware de tratamento de erros
    app.Use(async (context, next) =>
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Erro não tratado na requisição");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                sucesso = false,
                mensagem = "Erro interno do servidor",
                erro = app.Environment.IsDevelopment() ? ex.Message : null
            });
        }
    });

    Log.Information("Iniciando MenuAdminAPI");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Erro ao iniciar MenuAdminAPI");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
