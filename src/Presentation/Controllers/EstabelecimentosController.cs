using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Application.Mappings;
using MenuAdminAPI.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MenuAdminAPI.Presentation.Controllers;

/// <summary>
/// Controller para gerenciar Estabelecimentos
/// </summary>
[Authorize]
public class EstabelecimentosController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;

    public EstabelecimentosController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Obter estabelecimento por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EstabelecimentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var estabelecimento = await _unitOfWork.Estabelecimentos.ObterPorIdAsync(id);
        if (estabelecimento == null)
            return NotFoundResponse("Estabelecimento não encontrado");

        return OkResponse(estabelecimento.ToResponse());
    }

    /// <summary>
    /// Listar todos os estabelecimentos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EstabelecimentoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodos()
    {
        var estabelecimentos = await _unitOfWork.Estabelecimentos.ObterTodosAsync();
        return OkResponse(estabelecimentos.Select(e => e.ToResponse()));
    }

    /// <summary>
    /// Listar estabelecimentos ativos
    /// </summary>
    [HttpGet("ativos")]
    [ProducesResponseType(typeof(IEnumerable<EstabelecimentoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterAtivos()
    {
        var estabelecimentos = await _unitOfWork.Estabelecimentos.ObterAtivosAsync();
        return OkResponse(estabelecimentos.Select(e => e.ToResponse()));
    }

    /// <summary>
    /// Buscar estabelecimentos por termo
    /// </summary>
    [HttpGet("buscar")]
    [ProducesResponseType(typeof(IEnumerable<EstabelecimentoResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Buscar([FromQuery] string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return BadRequestResponse("Termo de busca não pode estar vazio");

        var estabelecimentos = await _unitOfWork.Estabelecimentos.BuscarAsync(termo);
        return OkResponse(estabelecimentos.Select(e => e.ToResponse()));
    }

    /// <summary>
    /// Criar novo estabelecimento
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(EstabelecimentoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarEstabelecimentoRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequestResponse("Dados inválidos", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));

        var estabelecimento = request.ToEntity();

        try
        {
            // Validações DENTRO da transação
            await _unitOfWork.BeginTransactionAsync();
            
            if (await _unitOfWork.Estabelecimentos.EmailJaExisteAsync(request.Email))
            {
                await _unitOfWork.RollbackAsync();
                return ConflictResponse("Email já existe");
            }

            if (await _unitOfWork.Estabelecimentos.CnpjJaExisteAsync(request.CNPJ))
            {
                await _unitOfWork.RollbackAsync();
                return ConflictResponse("CNPJ já existe");
            }
            
            await _unitOfWork.Estabelecimentos.AdicionarAsync(estabelecimento);
            await _unitOfWork.CommitAsync();

            return CreatedResponse($"/api/v1/estabelecimentos/{estabelecimento.Id}", estabelecimento.ToResponse());
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return InternalErrorResponse($"Erro ao criar estabelecimento: {ex.Message}");
        }
    }

    /// <summary>
    /// Atualizar estabelecimento
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EstabelecimentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarEstabelecimentoRequest request)
    {
        var estabelecimento = await _unitOfWork.Estabelecimentos.ObterPorIdAsync(id);
        if (estabelecimento == null)
            return NotFoundResponse("Estabelecimento não encontrado");

        if (estabelecimento.Email != request.Email && await _unitOfWork.Estabelecimentos.EmailJaExisteAsync(request.Email, id))
            return ConflictResponse("Email já existe");

        if (estabelecimento.CNPJ != request.CNPJ && await _unitOfWork.Estabelecimentos.CnpjJaExisteAsync(request.CNPJ, id))
            return ConflictResponse("CNPJ já existe");

        estabelecimento.Nome = request.Nome;
        estabelecimento.Email = request.Email;
        estabelecimento.Telefone = request.Telefone;
        estabelecimento.CNPJ = request.CNPJ;
        estabelecimento.Endereco = request.Endereco;
        estabelecimento.Numero = request.Numero;
        estabelecimento.Complemento = request.Complemento;
        estabelecimento.Bairro = request.Bairro;
        estabelecimento.Cidade = request.Cidade;
        estabelecimento.Estado = request.Estado;
        estabelecimento.CEP = request.CEP;
        estabelecimento.Plano = request.Plano;
        estabelecimento.DataAtualizacao = DateTime.UtcNow;

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Estabelecimentos.AtualizarAsync(estabelecimento);
            await _unitOfWork.CommitAsync();

            return OkResponse(estabelecimento.ToResponse());
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return InternalErrorResponse($"Erro ao atualizar estabelecimento: {ex.Message}");
        }
    }

    /// <summary>
    /// Ativar estabelecimento
    /// </summary>
    [HttpPatch("{id}/ativar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ativar(Guid id)
    {
        if (!await _unitOfWork.Estabelecimentos.ExisteAsync(id))
            return NotFoundResponse("Estabelecimento não encontrado");

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Estabelecimentos.AtivarAsync(id);
            await _unitOfWork.CommitAsync();

            return OkResponse(new { mensagem = "Estabelecimento ativado com sucesso" });
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return InternalErrorResponse($"Erro ao ativar estabelecimento: {ex.Message}");
        }
    }

    /// <summary>
    /// Desativar estabelecimento
    /// </summary>
    [HttpPatch("{id}/desativar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Desativar(Guid id)
    {
        if (!await _unitOfWork.Estabelecimentos.ExisteAsync(id))
            return NotFoundResponse("Estabelecimento não encontrado");

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Estabelecimentos.DesativarAsync(id);
            await _unitOfWork.CommitAsync();

            return OkResponse(new { mensagem = "Estabelecimento desativado com sucesso" });
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return InternalErrorResponse($"Erro ao desativar estabelecimento: {ex.Message}");
        }
    }

    /// <summary>
    /// Deletar estabelecimento
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(Guid id)
    {
        if (!await _unitOfWork.Estabelecimentos.ExisteAsync(id))
            return NotFoundResponse("Estabelecimento não encontrado");

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Estabelecimentos.RemoverAsync(id);
            await _unitOfWork.CommitAsync();

            return OkResponse(new { mensagem = "Estabelecimento deletado com sucesso" });
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return InternalErrorResponse($"Erro ao deletar estabelecimento: {ex.Message}");
        }
    }
}
