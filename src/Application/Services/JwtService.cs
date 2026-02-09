using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MenuAdminAPI.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace MenuAdminAPI.Application.Services;

/// <summary>
/// Interface para serviço de JWT
/// </summary>
public interface IJwtService
{
    string GerarToken(Usuario usuario);
    string GerarRefreshToken();
    ClaimsPrincipal? ValidarToken(string token);
}

/// <summary>
/// Serviço para geração e validação de tokens JWT
/// </summary>
public class JwtService : IJwtService
{
    private readonly string _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationMinutes;

    public JwtService(IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        _secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret não configurado");
        _issuer = jwtSettings["Issuer"] ?? "MenuAdminAPI";
        _audience = jwtSettings["Audience"] ?? "MenuAdminAPI";
        _expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");
    }

    /// <summary>
    /// Gera um token JWT para o usuário
    /// </summary>
    public string GerarToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secret);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim("Perfil", usuario.Perfil),
            new Claim("EstabelecimentoId", usuario.EstabelecimentoId.ToString()),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationMinutes),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Gera um refresh token (string aleatória)
    /// </summary>
    public string GerarRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    /// <summary>
    /// Valida um token JWT e retorna as claims
    /// </summary>
    public ClaimsPrincipal? ValidarToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }
}
