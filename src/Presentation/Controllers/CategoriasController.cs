using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Application.Services;

namespace MenuAdminAPI.Presentation.Controllers;

/// <summary>
/// Controller para gerenciar Categorias
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class CategoriasController : BaseController
{
    private readonly ICategoriaService _categoriaService;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(ICategoriaService categoriaService, ILogger<CategoriasController> logger)
    {
        _categoriaService = categoriaService ?? throw new ArgumentNullException(nameof(categoriaService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Listar todas as categorias
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoriaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodas()
        {
            try
            {
                _logger.LogInformation("Listando todas as categorias");
                var categorias = await _categoriaService.ObterTodasAsync();
                return OkResponse(categorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar categorias");
                return InternalErrorResponse();
            }
        }

    /// <summary>
    /// Listar categorias por estabelecimento
    /// </summary>
    [HttpGet("estabelecimento/{estabelecimentoId}")]
    [ProducesResponseType(typeof(IEnumerable<CategoriaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorEstabelecimento(Guid estabelecimentoId)
    {
        try
        {
            _logger.LogInformation("Listando categorias do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            var categorias = await _categoriaService.ObterPorEstabelecimentoAsync(estabelecimentoId);
            return OkResponse(categorias);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar categorias do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Obter categoria por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            _logger.LogInformation("Obtendo categoria {CategoriaId}", id);
            
            // TODO: Implementar chamada ao service
            return OkResponse(new CategoriaResponse());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter categoria {CategoriaId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar categorias por estabelecimento
    /// </summary>
    [HttpGet("estabelecimento/{estabelecimentoId}")]
    [ProducesResponseType(typeof(IEnumerable<CategoriaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorEstabelecimento(Guid estabelecimentoId)
    {
        try
        {
            _logger.LogInformation("Listando categorias do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            
            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<CategoriaResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar categorias do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar apenas categorias ativas por estabelecimento
    /// </summary>
    [HttpGet("estabelecimento/{estabelecimentoId}/ativas")]
    [ProducesResponseType(typeof(IEnumerable<CategoriaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterAtivasPorEstabelecimento(Guid estabelecimentoId)
    {
        try
        {
            _logger.LogInformation("Listando categorias ativas do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            
            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<CategoriaResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar categorias ativas do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Buscar categorias por termo
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<CategoriaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar([FromQuery] string termo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termo))
                return BadRequestResponse();

            _logger.LogInformation("Buscando categorias com termo: {Termo}", termo);
            
            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<CategoriaResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar categorias com termo: {Termo}", termo);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Criar nova categoria
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CategoriaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] CriarCategoriaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequestResponse();

                _logger.LogInformation("Criando nova categoria: {CategoriaNome}", request.Nome);
                var novaCategoria = await _categoriaService.CriarAsync(request);
                return CreatedResponse(novaCategoria);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao criar categoria");
                return BadRequestResponse();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Conflito ao criar categoria");
                return ConflictResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar categoria");
                return InternalErrorResponse();
            }
        }

    /// <summary>
    /// Atualizar categoria
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarCategoriaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequestResponse();

                _logger.LogInformation("Atualizando categoria {CategoriaId}", id);
                await _categoriaService.AtualizarAsync(id, request);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Erro de validação ao atualizar categoria {CategoriaId}", id);
                return BadRequestResponse();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Categoria {CategoriaId} não encontrada", id);
                return NotFoundResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar categoria {CategoriaId}", id);
                return InternalErrorResponse();
            }
        }

    /// <summary>
    /// Ativar categoria
    /// </summary>
    [HttpPatch("{id}/ativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ativar(Guid id)
    {
        try
        {
            _logger.LogInformation("Ativando categoria {CategoriaId}", id);
            
            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Categoria {CategoriaId} não encontrada", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ativar categoria {CategoriaId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Desativar categoria
    /// </summary>
    [HttpPatch("{id}/desativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id)
    {
        try
        {
            _logger.LogInformation("Desativando categoria {CategoriaId}", id);
            
            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Categoria {CategoriaId} não encontrada", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao desativar categoria {CategoriaId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Deletar categoria
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                _logger.LogInformation("Deletando categoria {CategoriaId}", id);
                await _categoriaService.DeletarAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Categoria {CategoriaId} não encontrada", id);
                return NotFoundResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar categoria {CategoriaId}", id);
                return InternalErrorResponse();
            }
        }
}
