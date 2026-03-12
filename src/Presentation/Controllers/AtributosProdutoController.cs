using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Application.Services;

namespace MenuAdminAPI.Presentation.Controllers;

/// <summary>
/// Controller para gerenciamento de Atributos de Produto (ex: Tamanho, Cor, Sabor)
/// </summary>
[Authorize]
public class AtributosProdutoController : BaseController
{
    private readonly IAtributoProdutoService _service;

    public AtributosProdutoController(IAtributoProdutoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Listar todos os atributos
    /// GET /api/v1/AtributosProduto?comValores=true
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] bool comValores = false)
    {
        try
        {
            var atributos = await _service.ObterTodosAsync(comValores);
            return OkResponse(atributos);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Obter atributo por ID
    /// GET /api/v1/AtributosProduto/{id}?comValores=true
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, [FromQuery] bool comValores = true)
    {
        try
        {
            var atributo = await _service.ObterPorIdAsync(id, comValores);
            if (atributo == null) return NotFoundResponse($"Atributo com ID {id} não encontrado.");
            return OkResponse(atributo);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Criar novo atributo
    /// POST /api/v1/AtributosProduto
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarAtributoProdutoRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Nome))
                return BadRequestResponse("Nome do atributo é obrigatório.");

            var atributo = await _service.CriarAsync(request);
            return CreatedResponse($"/api/v1/AtributosProduto/{atributo.Id}", atributo, "Atributo criado com sucesso.");
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
    /// Atualizar atributo
    /// PUT /api/v1/AtributosProduto/{id}
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarAtributoProdutoRequest request)
    {
        try
        {
            var atributo = await _service.AtualizarAsync(id, request);
            return OkResponse(atributo, "Atributo atualizado com sucesso.");
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
    /// Deletar atributo (soft delete)
    /// DELETE /api/v1/AtributosProduto/{id}
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            await _service.DeletarAsync(id);
            return OkResponse<object?>(null, "Atributo removido com sucesso.");
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

    // =====================================================================
    // Valores dos Atributos
    // =====================================================================

    /// <summary>
    /// Listar valores de um atributo
    /// GET /api/v1/AtributosProduto/{atributoId}/valores
    /// </summary>
    [HttpGet("{atributoId:guid}/valores")]
    public async Task<IActionResult> ListarValores(Guid atributoId)
    {
        try
        {
            var valores = await _service.ObterValoresPorAtributoAsync(atributoId);
            return OkResponse(valores);
        }
        catch (Exception ex)
        {
            return InternalErrorResponse(ex.Message);
        }
    }

    /// <summary>
    /// Criar valor para um atributo
    /// POST /api/v1/AtributosProduto/{atributoId}/valores
    /// </summary>
    [HttpPost("{atributoId:guid}/valores")]
    public async Task<IActionResult> CriarValor(Guid atributoId, [FromBody] CriarAtributoProdutoValorRequest request)
    {
        try
        {
            // Garantir que o atributoId da rota seja usado
            var req = request with { AtributoProdutoId = atributoId };
            var valor = await _service.CriarValorAsync(req);
            return CreatedResponse(valor, "Valor criado com sucesso.");
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
    /// Atualizar valor de atributo
    /// PUT /api/v1/AtributosProduto/valores/{valorId}
    /// </summary>
    [HttpPut("valores/{valorId:guid}")]
    public async Task<IActionResult> AtualizarValor(Guid valorId, [FromBody] AtualizarAtributoProdutoValorRequest request)
    {
        try
        {
            var valor = await _service.AtualizarValorAsync(valorId, request);
            return OkResponse(valor, "Valor atualizado com sucesso.");
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
    /// Deletar valor de atributo (soft delete)
    /// DELETE /api/v1/AtributosProduto/valores/{valorId}
    /// </summary>
    [HttpDelete("valores/{valorId:guid}")]
    public async Task<IActionResult> DeletarValor(Guid valorId)
    {
        try
        {
            await _service.DeletarValorAsync(valorId);
            return OkResponse<object?>(null, "Valor removido com sucesso.");
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
}
