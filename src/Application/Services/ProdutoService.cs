using MenuAdminAPI.Domain.Repositories;
using MenuAdminAPI.Application.DTOs;

namespace MenuAdminAPI.Application.Services;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponse>> ObterTodosAsync();
    Task<ProdutoResponse?> ObterPorIdAsync(Guid id);
    Task<ProdutoResponse> CriarAsync(CriarProdutoRequest request);
    Task<ProdutoResponse> AtualizarAsync(Guid id, AtualizarProdutoRequest request);
    Task DeletarAsync(Guid id);
}

public class ProdutoService : IProdutoService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProdutoResponse>> ObterTodosAsync()
    {
        // TODO: Implementar busca de produtos do repositório
        return Enumerable.Empty<ProdutoResponse>();
    }

    public async Task<ProdutoResponse?> ObterPorIdAsync(Guid id)
    {
        // TODO: Implementar busca de produto por ID
        return null;
    }

    public async Task<ProdutoResponse> CriarAsync(CriarProdutoRequest request)
    {
        // TODO: Implementar criação de produto
        throw new NotImplementedException();
    }

    public async Task<ProdutoResponse> AtualizarAsync(Guid id, AtualizarProdutoRequest request)
    {
        // TODO: Implementar atualização de produto
        throw new NotImplementedException();
    }

    public async Task DeletarAsync(Guid id)
    {
        // TODO: Implementar deleção de produto
    }
}
