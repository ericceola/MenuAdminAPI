using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Application.Services;

namespace MenuAdminAPI.Presentation.Controllers;

/// <summary>
/// Controller para gerenciamento de Variantes de Produto e Endpoints Compostos
/// </summary>
[Authorize]
public class ProdutoVariantesController : BaseController
{
    private readonly IProdutoVarianteService _service;

    public ProdutoVariantesController(IProdutoVarianteService service)
    {
        _service = service;
    }

    // =====================================================================
    // Endpoints Compostos (produto completo)
    // =====================================================================

    /// <summary>
    /// Obter produto completo com variantes e atributos
    /// GET /api/v1/ProdutoVariantes/{produtoId}/completo
    /// </summary>
    [HttpGet("{produtoId:guid}/completo")]
    public async Task<IActionResult> ObterCompleto(Guid produtoId)
    {
        try
        {
            var produto = await _service.ObterProdutoCompletoAsync(produtoId);
            if (produto == null) return NotFoundResponse($"Produto com ID {produtoId} não encontrado.");
            return OkResponse(produto);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Criar produto completo com variantes em uma única requisição
    /// POST /api/v1/ProdutoVariantes/completo
    /// </summary>
    [HttpPost("completo")]
    public async Task<IActionResult> CriarCompleto([FromBody] CriarProdutoCompletoRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Nome))
                return BadRequestResponse("Nome do produto é obrigatório.");

            var produto = await _service.CriarProdutoCompletoAsync(request);
            return CreatedResponse($"/api/v1/ProdutoVariantes/{produto.Id}/completo", produto, "Produto criado com sucesso.");
        }
        catch (InvalidOperationException ex)
        {
            return ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Atualizar produto completo com variantes
    /// PUT /api/v1/ProdutoVariantes/{produtoId}/completo
    /// </summary>
    [HttpPut("{produtoId:guid}/completo")]
    public async Task<IActionResult> AtualizarCompleto(Guid produtoId, [FromBody] AtualizarProdutoCompletoRequest request)
    {
        try
        {
            var produto = await _service.AtualizarProdutoCompletoAsync(produtoId, request);
            return OkResponse(produto, "Produto atualizado com sucesso.");
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("não encontrado"))
        {
            return NotFoundResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    // =====================================================================
    // CRUD de Variantes
    // =====================================================================

    /// <summary>
    /// Listar variantes de um produto
    /// GET /api/v1/ProdutoVariantes/{produtoId}/variantes
    /// </summary>
    [HttpGet("{produtoId:guid}/variantes")]
    public async Task<IActionResult> Listar(Guid produtoId)
    {
        try
        {
            var variantes = await _service.ObterPorProdutoAsync(produtoId);
            return OkResponse(variantes);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Obter variante específica de um produto
    /// GET /api/v1/ProdutoVariantes/{produtoId}/variantes/{varianteId}
    /// </summary>
    [HttpGet("{produtoId:guid}/variantes/{varianteId:guid}")]
    public async Task<IActionResult> ObterPorId(Guid produtoId, Guid varianteId)
    {
        try
        {
            var variante = await _service.ObterPorIdAsync(produtoId, varianteId);
            if (variante == null) return NotFoundResponse($"Variante com ID {varianteId} não encontrada.");
            return OkResponse(variante);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Criar variante para um produto
    /// POST /api/v1/ProdutoVariantes/{produtoId}/variantes
    /// </summary>
    [HttpPost("{produtoId:guid}/variantes")]
    public async Task<IActionResult> Criar(Guid produtoId, [FromBody] CriarProdutoVarianteRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Nome))
                return BadRequestResponse("Nome da variante é obrigatório.");

            var variante = await _service.CriarAsync(produtoId, request);
            return CreatedResponse(variante, "Variante criada com sucesso.");
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("não encontrado"))
        {
            return NotFoundResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Atualizar variante de um produto
    /// PUT /api/v1/ProdutoVariantes/{produtoId}/variantes/{varianteId}
    /// </summary>
    [HttpPut("{produtoId:guid}/variantes/{varianteId:guid}")]
    public async Task<IActionResult> Atualizar(Guid produtoId, Guid varianteId, [FromBody] AtualizarProdutoVarianteRequest request)
    {
        try
        {
            var variante = await _service.AtualizarAsync(produtoId, varianteId, request);
            return OkResponse(variante, "Variante atualizada com sucesso.");
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("não encontrada"))
        {
            return NotFoundResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Deletar variante (soft delete)
    /// DELETE /api/v1/ProdutoVariantes/{produtoId}/variantes/{varianteId}
    /// </summary>
    [HttpDelete("{produtoId:guid}/variantes/{varianteId:guid}")]
    public async Task<IActionResult> Deletar(Guid produtoId, Guid varianteId)
    {
        try
        {
            await _service.DeletarAsync(produtoId, varianteId);
            return OkResponse<object?>(null, "Variante removida com sucesso.");
        }
        catch (InvalidOperationException ex)
        {
            return NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Ativar variante
    /// PATCH /api/v1/ProdutoVariantes/{produtoId}/variantes/{varianteId}/ativar
    /// </summary>
    [HttpPatch("{produtoId:guid}/variantes/{varianteId:guid}/ativar")]
    public async Task<IActionResult> Ativar(Guid produtoId, Guid varianteId)
    {
        try
        {
            await _service.AtivarAsync(produtoId, varianteId);
            return OkResponse<object?>(null, "Variante ativada com sucesso.");
        }
        catch (InvalidOperationException ex)
        {
            return NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Desativar variante
    /// PATCH /api/v1/ProdutoVariantes/{produtoId}/variantes/{varianteId}/desativar
    /// </summary>
    [HttpPatch("{produtoId:guid}/variantes/{varianteId:guid}/desativar")]
    public async Task<IActionResult> Desativar(Guid produtoId, Guid varianteId)
    {
        try
        {
            await _service.DesativarAsync(produtoId, varianteId);
            return OkResponse<object?>(null, "Variante desativada com sucesso.");
        }
        catch (InvalidOperationException ex)
        {
            return NotFoundResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Associar/substituir atributos de uma variante
    /// PUT /api/v1/ProdutoVariantes/{produtoId}/variantes/{varianteId}/atributos
    /// </summary>
    [HttpPut("{produtoId:guid}/variantes/{varianteId:guid}/atributos")]
    [HttpPost("{produtoId:guid}/variantes/{varianteId:guid}/atributos")]
    public async Task<IActionResult> AssociarAtributos(Guid produtoId, Guid varianteId, [FromBody] AssociarAtributosVarianteRequest request)
    {
        try
        {
            var variante = await _service.AssociarAtributosAsync(produtoId, varianteId, request);
            return OkResponse(variante, "Atributos associados com sucesso.");
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("não encontrada"))
        {
            return NotFoundResponse(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return ConflictResponse(ex.Message);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Listar atributos de uma variante
    /// GET /api/v1/ProdutoVariantes/{produtoId}/variantes/{varianteId}/atributos
    /// </summary>
    [HttpGet("{produtoId:guid}/variantes/{varianteId:guid}/atributos")]
    public async Task<IActionResult> ListarAtributos(Guid produtoId, Guid varianteId)
    {
        try
        {
            var variante = await _service.ObterPorIdAsync(produtoId, varianteId);
            if (variante == null) return NotFoundResponse($"Variante com ID {varianteId} não encontrada.");
            return OkResponse(variante.Atributos);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }
}
