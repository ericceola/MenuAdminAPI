using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MenuAdminAPI.Presentation.Controllers;

/// <summary>
/// Classe base para todos os controllers
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Obter ID do usuário autenticado do token JWT
    /// </summary>
    protected Guid ObterUsuarioId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null && Guid.TryParse(claim.Value, out var userId) ? userId : Guid.Empty;
    }

    /// <summary>
    /// Obter ID do estabelecimento do token JWT
    /// </summary>
    protected Guid ObterEstabelecimentoId()
    {
        var claim = User.FindFirst("EstabelecimentoId");
        return claim != null && Guid.TryParse(claim.Value, out var estabelecimentoId) ? estabelecimentoId : Guid.Empty;
    }

    /// <summary>
    /// Obter perfil do usuário do token JWT
    /// </summary>
    protected string ObterPerfil()
    {
        return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
    }

    /// <summary>
    /// Verificar se o usuário tem um perfil específico
    /// </summary>
    protected bool TemPerfil(string perfil)
    {
        return ObterPerfil().Equals(perfil, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Verificar se o usuário tem algum dos perfis especificados
    /// </summary>
    protected bool TemAlgumPerfil(params string[] perfis)
    {
        var perfilUsuario = ObterPerfil();
        return perfis.Any(p => p.Equals(perfilUsuario, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Retornar resposta OK com dados
    /// </summary>
    protected IActionResult OkResponse<T>(T dados, string mensagem = "Operação realizada com sucesso")
    {
        return Ok(new { sucesso = true, mensagem, dados });
    }

    /// <summary>
    /// Retornar resposta Created
    /// </summary>
    protected IActionResult CreatedResponse<T>(string location, T dados, string mensagem = "Recurso criado com sucesso")
    {
        return Created(location, new { sucesso = true, mensagem, dados });
    }

    /// <summary>
    /// Retornar resposta BadRequest
    /// </summary>
    protected IActionResult BadRequestResponse(string mensagem, IEnumerable<string>? erros = null)
    {
        return BadRequest(new { sucesso = false, mensagem, erros });
    }

    /// <summary>
    /// Retornar resposta NotFound
    /// </summary>
    protected IActionResult NotFoundResponse(string mensagem = "Recurso não encontrado")
    {
        return NotFound(new { sucesso = false, mensagem });
    }

    /// <summary>
    /// Retornar resposta Conflict
    /// </summary>
    protected IActionResult ConflictResponse(string mensagem)
    {
        return Conflict(new { sucesso = false, mensagem });
    }

    /// <summary>
    /// Retornar resposta Unauthorized
    /// </summary>
    protected IActionResult UnauthorizedResponse(string mensagem = "Não autorizado")
    {
        return Unauthorized(new { sucesso = false, mensagem });
    }

    /// <summary>
    /// Retornar resposta Forbidden
    /// </summary>
    protected IActionResult ForbiddenResponse(string mensagem = "Acesso proibido")
    {
        return Forbid();
    }

    /// <summary>
    /// Retornar resposta InternalServerError
    /// </summary>
    protected IActionResult InternalErrorResponse(string mensagem = "Erro interno do servidor")
    {
        return StatusCode(StatusCodes.Status500InternalServerError, new { sucesso = false, mensagem });
    }
}
