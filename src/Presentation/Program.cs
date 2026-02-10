using MenuAdminAPI.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// ---------- Serilog (Linux-safe) ----------
var isLinux = OperatingSystem.IsLinux();
var logFilePath = isLinux
    ? "/home/LogFiles/MenuAdminAPI-.txt"     // App Service Linux
    : "logs/MenuAdminAPI-.txt";              // local windows/dev

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    // ---------- Services ----------
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // ---------- Connection string (App Service friendly) ----------
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        // Prefer the standard .NET way (App Settings): ConnectionStrings__DefaultConnection
        connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection") ??
            // App Service specific prefixes
            Environment.GetEnvironmentVariable("SQLCONNSTR_DefaultConnection") ??
            Environment.GetEnvironmentVariable("MYSQLCONNSTR_DefaultConnection") ??
            Environment.GetEnvironmentVariable("CUSTOMCONNSTR_DefaultConnection") ??
            // legacy fallback (if you insist on custom names)
            Environment.GetEnvironmentVariable("ConnectionString") ??
            Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTIONSTRING");
    }

    if (string.IsNullOrWhiteSpace(connectionString))
        throw new InvalidOperationException("Connection string 'DefaultConnection' não foi encontrada. Configure em App Service > Configuration > Connection strings.");

    Log.Information("Connection string 'DefaultConnection' carregada com sucesso.");

    builder.Services.AddInfrastructure(connectionString);

    // ---------- Application services ----------
    builder.Services.AddApplicationServices();

    // ---------- JWT ----------
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");
    var jwtSecret = jwtSettings["Secret"];

    if (string.IsNullOrWhiteSpace(jwtSecret))
    {
        jwtSecret = Environment.GetEnvironmentVariable("JwtSecret")
            ?? "your-super-secret-key-that-must-be-at-least-32-characters-long-for-security";
    }

    var jwtIssuer = jwtSettings["Issuer"] ?? Environment.GetEnvironmentVariable("JwtIssuer") ?? "MenuAdminAPI";
    var jwtAudience = jwtSettings["Audience"] ?? Environment.GetEnvironmentVariable("JwtAudience") ?? "MenuAdminAPI";

    // Evitar exception por valor inválido
    var jwtExpirationMinutesRaw = jwtSettings["ExpirationMinutes"] ?? Environment.GetEnvironmentVariable("JwtExpirationMinutes") ?? "60";
    if (!int.TryParse(jwtExpirationMinutesRaw, out var jwtExpirationMinutes))
        jwtExpirationMinutes = 60;

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

    Log.Information("JWT configurado com sucesso. Issuer={Issuer} Audience={Audience} ExpMinutes={ExpMinutes}",
        jwtIssuer, jwtAudience, jwtExpirationMinutes);

    // ---------- CORS ----------
    builder.Services.AddCors(options =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "*" };

        Log.Information("CORS Allowed Origins: {Origins}", string.Join(", ", allowedOrigins));

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

    // ---------- Global error handling (must be early) ----------
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

    // ---------- HTTP pipeline ----------
   
        app.UseSwagger();
        app.UseSwaggerUI();
   

    // Em App Service Linux, o TLS termina no front-end; UseHttpsRedirection é ok, mas não obrigatório.
    app.UseHttpsRedirection();

    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Iniciando MenuAdminAPI. Environment={Env} OS={OS}", app.Environment.EnvironmentName, isLinux ? "Linux" : "Windows");
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