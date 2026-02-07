using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Application.Services;

namespace MenuAdminAPI.Presentation.Controllers;

/// <summary>
/// Controller para gerenciar Usuários
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UsuariosController : BaseController
{
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
    {
        _usuarioService = usuarioService ?? throw new ArgumentNullException(nameof(usuarioService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obter usuário por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            _logger.LogInformation("Obtendo usuário {UsuarioId}", id);
            
            // TODO: Implementar chamada ao service
            return OkResponse(new UsuarioResponse());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter usuário {UsuarioId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar usuários por estabelecimento
    /// </summary>
    [HttpGet("estabelecimento/{estabelecimentoId}")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorEstabelecimento(Guid estabelecimentoId)
    {
        try
        {
            _logger.LogInformation("Listando usuários do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            
            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<UsuarioResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuários do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar apenas usuários ativos por estabelecimento
    /// </summary>
    [HttpGet("estabelecimento/{estabelecimentoId}/ativos")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterAtivosPorEstabelecimento(Guid estabelecimentoId)
    {
        try
        {
            _logger.LogInformation("Listando usuários ativos do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            
            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<UsuarioResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar usuários ativos do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Buscar usuários por termo
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar([FromQuery] string termo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termo))
                return BadRequestResponse();

            _logger.LogInformation("Buscando usuários com termo: {Termo}", termo);
            
            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<UsuarioResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuários com termo: {Termo}", termo);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Criar novo usuário
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Criando novo usuário: {UsuarioEmail}", request.Email);
            
            // TODO: Implementar chamada ao service
            return CreatedResponse(new UsuarioResponse());
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar usuário");
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflito ao criar usuário");
            return ConflictResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário");
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Atualizar usuário
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarUsuarioRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Atualizando usuário {UsuarioId}", id);
            
            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar usuário {UsuarioId}", id);
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Usuário {UsuarioId} não encontrado", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário {UsuarioId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Ativar usuário
    /// </summary>
    [HttpPatch("{id}/ativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ativar(Guid id)
    {
        try
        {
            _logger.LogInformation("Ativando usuário {UsuarioId}", id);
            
            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Usuário {UsuarioId} não encontrado", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ativar usuário {UsuarioId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Desativar usuário
    /// </summary>
    [HttpPatch("{id}/desativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id)
    {
        try
        {
            _logger.LogInformation("Desativando usuário {UsuarioId}", id);
            
            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Usuário {UsuarioId} não encontrado", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao desativar usuário {UsuarioId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Deletar usuário
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            _logger.LogInformation("Deletando usuário {UsuarioId}", id);
            
            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Usuário {UsuarioId} não encontrado", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar usuário {UsuarioId}", id);
            return InternalErrorResponse();
        }
    }
}
