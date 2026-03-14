using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Application.Services;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Presentation.Controllers;

/// <summary>
/// Controller para gerenciar Produtos
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ProdutosController : BaseController
{
    private readonly IProdutoService _produtoService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(IProdutoService produtoService, IUnitOfWork unitOfWork, ILogger<ProdutosController> logger)
    {
        _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Listar todos os produtos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProdutoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodos()
    {
        try
        {
            _logger.LogInformation("Listando todos os produtos");

            var produtos = await _produtoService.ObterTodosAsync();
            return OkResponse(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos");
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Obter produto por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            _logger.LogInformation("Obtendo produto {ProdutoId}", id);

            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null)
                return NotFoundResponse();

            return OkResponse(produto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter produto {ProdutoId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar produtos por subcategoria
    /// </summary>
    [HttpGet("subcategoria/{subcategoriaId}")]
    [ProducesResponseType(typeof(IEnumerable<ProdutoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorSubcategoria(Guid subcategoriaId)
    {
        try
        {
            _logger.LogInformation("Listando produtos da subcategoria {SubcategoriaId}", subcategoriaId);

            var produtos = await _produtoService.ObterPorSubcategoriaAsync(subcategoriaId);
            return OkResponse(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos da subcategoria {SubcategoriaId}", subcategoriaId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar produtos por estabelecimento
    /// </summary>
    [HttpGet("estabelecimento/{estabelecimentoId}")]
    [ProducesResponseType(typeof(IEnumerable<ProdutoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterPorEstabelecimento(Guid estabelecimentoId)
    {
        try
        {
            _logger.LogInformation("Listando produtos do estabelecimento {EstabelecimentoId}", estabelecimentoId);

            var produtos = await _produtoService.ObterPorEstabelecimentoAsync(estabelecimentoId);
            return OkResponse(produtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos do estabelecimento {EstabelecimentoId}", estabelecimentoId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar apenas produtos ativos por subcategoria
    /// </summary>
    [HttpGet("subcategoria/{subcategoriaId}/ativos")]
    [ProducesResponseType(typeof(IEnumerable<ProdutoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterAtivosPorSubcategoria(Guid subcategoriaId)
    {
        try
        {
            _logger.LogInformation("Listando produtos ativos da subcategoria {SubcategoriaId}", subcategoriaId);

            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<ProdutoResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar produtos ativos da subcategoria {SubcategoriaId}", subcategoriaId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Buscar produtos por termo
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<ProdutoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar([FromQuery] string termo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(termo))
                return BadRequestResponse();

            _logger.LogInformation("Buscando produtos com termo: {Termo}", termo);

            // TODO: Implementar chamada ao service
            return OkResponse(Enumerable.Empty<ProdutoResponse>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar produtos com termo: {Termo}", termo);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Criar novo produto
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Criando novo produto: {ProdutoNome} para estabelecimento {EstabelecimentoId}",
                request.Nome, request.EstabelecimentoId);

            var produto = await _produtoService.CriarAsync(request);
            return CreatedResponse(produto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao criar produto");
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflito ao criar produto");
            return ConflictResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar produto");
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Atualizar produto
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarProdutoRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Atualizando produto {ProdutoId}", id);

            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao atualizar produto {ProdutoId}", id);
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Produto {ProdutoId} não encontrado", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar produto {ProdutoId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Ativar produto
    /// </summary>
    [HttpPatch("{id}/ativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ativar(Guid id)
    {
        try
        {
            _logger.LogInformation("Ativando produto {ProdutoId}", id);

            await _produtoService.AtivarAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Produto {ProdutoId} não encontrado", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao ativar produto {ProdutoId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Desativar produto
    /// </summary>
    [HttpPatch("{id}/desativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id)
    {
        try
        {
            _logger.LogInformation("Desativando produto {ProdutoId}", id);

            await _produtoService.DesativarAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Produto {ProdutoId} não encontrado", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao desativar produto {ProdutoId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Deletar produto
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            _logger.LogInformation("Deletando produto {ProdutoId}", id);

            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Produto {ProdutoId} não encontrado", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao deletar produto {ProdutoId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Adicionar variante ao produto
    /// </summary>
    [HttpPost("{id}/variantes")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdicionarVariante(Guid id, [FromBody] CriarVarianteRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Adicionando variante ao produto {ProdutoId}", id);

            // TODO: Implementar chamada ao service
            return CreatedResponse(new VarianteResponse());
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao adicionar variante");
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Produto {ProdutoId} não encontrado", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar variante ao produto {ProdutoId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Remover variante do produto
    /// </summary>
    [HttpDelete("variantes/{varianteId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverVariante(Guid varianteId)
    {
        try
        {
            _logger.LogInformation("Removendo variante {VarianteId}", varianteId);

            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Variante {VarianteId} não encontrada", varianteId);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover variante {VarianteId}", varianteId);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Listar adicionais de um produto
    /// </summary>
    [HttpGet("{id}/adicionais")]
    [ProducesResponseType(typeof(IEnumerable<AdicionalResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarAdicionais(Guid id)
    {
        try
        {
            _logger.LogInformation("Listando adicionais do produto {ProdutoId}", id);
            var adicionais = await _unitOfWork.Adicionais.ObterPorProdutoAsync(id);
            var response = adicionais.Select(a => new AdicionalResponse(
                Id: a.Id,
                ProdutoId: a.ProdutoId,
                Nome: a.Nome,
                Preco: a.Preco,
                Ativo: a.Ativo
            ));
            return OkResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar adicionais do produto {ProdutoId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Adicionar adicional ao produto
    /// </summary>
    [HttpPost("{id}/adicionais")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdicionarAdicional(Guid id, [FromBody] CriarAdicionalRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequestResponse();

            _logger.LogInformation("Adicionando adicional ao produto {ProdutoId}", id);

            // TODO: Implementar chamada ao service
            return CreatedResponse(new AdicionalResponse());
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Erro de validação ao adicionar adicional");
            return BadRequestResponse();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Produto {ProdutoId} não encontrado", id);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao adicionar adicional ao produto {ProdutoId}", id);
            return InternalErrorResponse();
        }
    }

    /// <summary>
    /// Remover adicional do produto
    /// </summary>
    [HttpDelete("adicionais/{adicionalId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverAdicional(Guid adicionalId)
    {
        try
        {
            _logger.LogInformation("Removendo adicional {AdicionalId}", adicionalId);

            // TODO: Implementar chamada ao service
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Adicional {AdicionalId} não encontrado", adicionalId);
            return NotFoundResponse();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover adicional {AdicionalId}", adicionalId);
            return InternalErrorResponse();
        }
    }
}
