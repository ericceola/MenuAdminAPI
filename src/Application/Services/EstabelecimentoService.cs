using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Application.Mappings;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

/// <summary>
/// Serviço de Aplicação para Estabelecimentos
/// </summary>
public class EstabelecimentoService : IEstabelecimentoService
{
    private readonly IUnitOfWork _unitOfWork;

    public EstabelecimentoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EstabelecimentoResponse?> ObterPorIdAsync(Guid id)
    {
        var estabelecimento = await _unitOfWork.Estabelecimentos.ObterPorIdAsync(id);
        return estabelecimento?.ToResponse();
    }

    public async Task<IEnumerable<EstabelecimentoResponse>> ObterTodosAsync()
    {
        var estabelecimentos = await _unitOfWork.Estabelecimentos.ObterTodosAsync();
        return estabelecimentos.Select(e => e.ToResponse());
    }

    public async Task<IEnumerable<EstabelecimentoResponse>> ObterAtivosAsync()
    {
        var estabelecimentos = await _unitOfWork.Estabelecimentos.ObterAtivosAsync();
        return estabelecimentos.Select(e => e.ToResponse());
    }

    public async Task<IEnumerable<EstabelecimentoResponse>> BuscarAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            throw new ArgumentException("Termo não pode estar vazio");

        var estabelecimentos = await _unitOfWork.Estabelecimentos.BuscarAsync(termo);
        return estabelecimentos.Select(e => e.ToResponse());
    }

    public async Task<EstabelecimentoResponse> CriarAsync(CriarEstabelecimentoRequest request)
    {
        if (await _unitOfWork.Estabelecimentos.EmailJaExisteAsync(request.Email))
            throw new InvalidOperationException("Email já existe");

        if (await _unitOfWork.Estabelecimentos.CnpjJaExisteAsync(request.CNPJ))
            throw new InvalidOperationException("CNPJ já existe");

        var estabelecimento = request.ToEntity();

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Estabelecimentos.AdicionarAsync(estabelecimento);
            await _unitOfWork.CommitAsync();

            return estabelecimento.ToResponse();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<EstabelecimentoResponse> AtualizarAsync(Guid id, AtualizarEstabelecimentoRequest request)
    {
        var estabelecimento = await _unitOfWork.Estabelecimentos.ObterPorIdAsync(id);
        if (estabelecimento == null)
            throw new KeyNotFoundException("Estabelecimento não encontrado");

        if (estabelecimento.Email != request.Email && await _unitOfWork.Estabelecimentos.EmailJaExisteAsync(request.Email, id))
            throw new InvalidOperationException("Email já existe");

        if (estabelecimento.CNPJ != request.CNPJ && await _unitOfWork.Estabelecimentos.CnpjJaExisteAsync(request.CNPJ, id))
            throw new InvalidOperationException("CNPJ já existe");

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

            return estabelecimento.ToResponse();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task AtivarAsync(Guid id)
    {
        if (!await _unitOfWork.Estabelecimentos.ExisteAsync(id))
            throw new KeyNotFoundException("Estabelecimento não encontrado");

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Estabelecimentos.AtivarAsync(id);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DesativarAsync(Guid id)
    {
        if (!await _unitOfWork.Estabelecimentos.ExisteAsync(id))
            throw new KeyNotFoundException("Estabelecimento não encontrado");

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Estabelecimentos.DesativarAsync(id);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DeletarAsync(Guid id)
    {
        if (!await _unitOfWork.Estabelecimentos.ExisteAsync(id))
            throw new KeyNotFoundException("Estabelecimento não encontrado");

        try
        {
            await _unitOfWork.BeginTransactionAsync();
            await _unitOfWork.Estabelecimentos.RemoverAsync(id);
            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
