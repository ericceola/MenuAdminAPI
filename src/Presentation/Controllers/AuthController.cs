using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Application.Services;

namespace MenuAdminAPI.Presentation.Controllers;

/// <summary>
/// Controller para autenticação e autorização
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Fazer login com email e senha
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Tentativa de login para usuário: {Email}", request.Email);
            
            var response = await _authService.LoginAsync(request);
            return OkResponse(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao fazer login");
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Falha na autenticação");
            return UnauthorizedResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao fazer login");
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Renovar token JWT
    /// </summary>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RefreshTokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Renovando token JWT");
            
            var response = await _authService.RefreshTokenAsync(request);
            return OkResponse(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao renovar token");
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Token inválido ou expirado");
            return UnauthorizedResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao renovar token");
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Obter usuário autenticado
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ObterUsuarioAutenticado()
    {
        try
        {
            var usuarioId = ObterUsuarioId();
            if (usuarioId == Guid.Empty)
            {
                _logger.LogWarning("Usuário não autenticado");
                return UnauthorizedResponse();
            }

            _logger.LogInformation("Obtendo dados do usuário autenticado: {UsuarioId}", usuarioId);
            
            var response = await _authService.ObterUsuarioAutenticadoAsync(usuarioId);
            return OkResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário autenticado");
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Alterar senha do usuário autenticado
    /// </summary>
    [HttpPost("alterar-senha")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AlterarSenha([FromBody] AlterarSenhaRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            var usuarioId = ObterUsuarioId();
            if (usuarioId == Guid.Empty)
            {
                _logger.LogWarning("Usuário não autenticado");
                return UnauthorizedResponse();
            }

            _logger.LogInformation("Alterando senha do usuário: {UsuarioId}", usuarioId);
            
            await _authService.AlterarSenhaAsync(usuarioId, request);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao alterar senha");
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Erro ao alterar senha");
            return BadRequestResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao alterar senha");
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Resetar senha (gera nova senha temporária)
    /// </summary>
    [HttpPost("resetar-senha")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetarSenha([FromBody] ResetarSenhaRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Resetando senha para usuário: {Email}", request.Email);
            
            await _authService.ResetarSenhaAsync(request);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao resetar senha");
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Usuário não encontrado");
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao resetar senha");
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Validar token JWT
    /// </summary>
    [HttpPost("validar-token")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ValidarToken([FromBody] ValidarTokenRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Validando token JWT");
            
            // TODO: Implementar validação de token com IJwtService
            return OkResponse(new { valido = true });
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao validar token");
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Token inválido");
            return UnauthorizedResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar token");
            return InternalErrorResponse();
        }
    }
}

/// <summary>
/// Request para validar token
/// </summary>
public class ValidarTokenRequest
{
    public string? Token { get; set; }
}
