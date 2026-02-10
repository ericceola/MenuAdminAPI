using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Application.Services;

namespace MenuAdminAPI.Presentation.Controllers;

/// <summary>
/// Controller para gerenciar Subcategorias
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class SubcategoriasController : BaseController
{
    private readonly ISubcategoriaService _subcategoriaService;
    private readonly ILogger<SubcategoriasController> _logger;

    public SubcategoriasController(ISubcategoriaService subcategoriaService, ILogger<SubcategoriasController> logger)
    {
        _subcategoriaService = subcategoriaService ?? throw new ArgumentNullException(nameof(subcategoriaService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obter subcategoria por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SubcategoriaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            _logger.LogInformation("Obtendo subcategoria {SubcategoriaId}", id);
            
            // TODO: Implementar chamada ao service
            return OkResponse(new SubcategoriaResponse());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter subcategoria {SubcategoriaId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar subcategorias por categoria
    /// </summary>
    [HttpGet("categoria/{categoriaId}")]
    [ProducesResponseType(typeof(IEnumerable<SubcategoriaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorCategoria(Guid categoriaId)
    {
        try
        {
            _logger.LogInformation("Listando subcategorias da categoria {CategoriaId}", categoriaId);
            
            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<SubcategoriaResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar subcategorias da categoria {CategoriaId}", categoriaId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar apenas subcategorias ativas por categoria
    /// </summary>
    [HttpGet("categoria/{categoriaId}/ativas")]
    [ProducesResponseType(typeof(IEnumerable<SubcategoriaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterAtivasPorCategoria(Guid categoriaId)
    {
        try
        {
            _logger.LogInformation("Listando subcategorias ativas da categoria {CategoriaId}", categoriaId);
            
            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<SubcategoriaResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar subcategorias ativas da categoria {CategoriaId}", categoriaId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Buscar subcategorias por termo
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<SubcategoriaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar([FromQuery] string termo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termo))
                return BadRequestResponse();

            _logger.LogInformation("Buscando subcategorias com termo: {Termo}", termo);
            
            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<SubcategoriaResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar subcategorias com termo: {Termo}", termo);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar subcategorias com paginação
    /// </summary>
    [HttpGet("paginado")]
    [ProducesResponseType(typeof(PaginatedResponse<SubcategoriaResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterComPaginacao([FromQuery] int pagina = 1, [FromQuery] int tamanho = 10)
    {
        try
        {
            if (pagina < 1 || tamanho < 1)
                return BadRequestResponse();

            _logger.LogInformation("Listando subcategorias com paginação - Página: {Pagina}, Tamanho: {Tamanho}", pagina, tamanho);
            
            // TODO: Implementar chamada ao service
            return OkResponse(new PaginatedResponse<SubcategoriaResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar subcategorias com paginação");
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Criar nova subcategoria
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(SubcategoriaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] CriarSubcategoriaRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Criando nova subcategoria: {SubcategoriaNome}", request.Nome);
            
            // TODO: Implementar chamada ao service
            return CreatedResponse(new SubcategoriaResponse());
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar subcategoria");
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflito ao criar subcategoria");
            return ConflictResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar subcategoria");
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Atualizar subcategoria
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarSubcategoriaRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Atualizando subcategoria {SubcategoriaId}", id);
            
            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar subcategoria {SubcategoriaId}", id);
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Subcategoria {SubcategoriaId} não encontrada", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar subcategoria {SubcategoriaId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Ativar subcategoria
    /// </summary>
    [HttpPatch("{id}/ativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ativar(Guid id)
    {
        try
        {
            _logger.LogInformation("Ativando subcategoria {SubcategoriaId}", id);
            
            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Subcategoria {SubcategoriaId} não encontrada", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ativar subcategoria {SubcategoriaId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Desativar subcategoria
    /// </summary>
    [HttpPatch("{id}/desativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id)
    {
        try
        {
            _logger.LogInformation("Desativando subcategoria {SubcategoriaId}", id);
            
            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Subcategoria {SubcategoriaId} não encontrada", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao desativar subcategoria {SubcategoriaId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Deletar subcategoria
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            _logger.LogInformation("Deletando subcategoria {SubcategoriaId}", id);
            
            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Subcategoria {SubcategoriaId} não encontrada", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar subcategoria {SubcategoriaId}", id);
            return InternalErrorResponse();
        }
    }
}
