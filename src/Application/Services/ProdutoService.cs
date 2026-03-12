using MenuAdminAPI.Domain.Repositories;
using MenuAdminAPI.Application.DTOs;

namespace MenuAdminAPI.Application.Services;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponse>> ObterTodosAsync();
    Task<ProdutoResponse?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ProdutoResponse>> ObterPorSubcategoriaAsync(Guid subcategoriaId);
    Task<IEnumerable<ProdutoResponse>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<ProdutoResponse> CriarAsync(CriarProdutoRequest request);
    Task<ProdutoResponse> AtualizarAsync(Guid id, AtualizarProdutoRequest request);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task DeletarAsync(Guid id);
}

public class ProdutoService : IProdutoService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private static ProdutoResponse MapToResponse(Domain.Entities.Produto p) => new(
        Id: p.Id,
        SubcategoriaId: p.SubcategoriaId,
        EstabelecimentoId: p.EstabelecimentoId,
        Nome: p.Nome,
        Descricao: p.Descricao ?? string.Empty,
        Preco: p.Preco,
        Ordem: p.Ordem,
        Status: p.Status,
        ImagemUrl: p.ImagemUrl,
        ImagemBlobName: p.ImagemBlobName,
        Ativo: p.Ativo,
        DataCriacao: p.DataCriacao
    );

    public async Task<IEnumerable<ProdutoResponse>> ObterTodosAsync()
    {
        // TODO: Implementar busca de todos os produtos
        return Enumerable.Empty<ProdutoResponse>();
    }

    public async Task<ProdutoResponse?> ObterPorIdAsync(Guid id)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(id);
        if (produto == null) return null;
        return MapToResponse(produto);
    }

    public async Task<IEnumerable<ProdutoResponse>> ObterPorSubcategoriaAsync(Guid subcategoriaId)
    {
        var produtos = await _unitOfWork.Produtos.ObterPorSubcategoriaAsync(subcategoriaId);
        return produtos.Select(MapToResponse);
    }

    public async Task<IEnumerable<ProdutoResponse>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        var produtos = await _unitOfWork.Produtos.ObterPorEstabelecimentoAsync(estabelecimentoId);
        return produtos.Select(MapToResponse);
    }

    public async Task<ProdutoResponse> CriarAsync(CriarProdutoRequest request)
    {
        var produto = new Domain.Entities.Produto
        {
            Id = Guid.NewGuid(),
            SubcategoriaId = request.SubcategoriaId,
            EstabelecimentoId = request.EstabelecimentoId,
            Nome = request.Nome.Trim(),
            Descricao = request.Descricao?.Trim() ?? "",
            Preco = request.Preco,
            Ordem = request.Ordem,
            Status = request.Status,
            ImagemUrl = request.ImagemUrl?.Trim(),
            ImagemBlobName = request.ImagemBlobName?.Trim(),
            Ativo = true,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        await _unitOfWork.Produtos.AdicionarAsync(produto);

        // Criar variantes se fornecidas
        if (request.Variantes != null && request.Variantes.Any())
        {
            foreach (var varianteDto in request.Variantes)
            {
                var variante = new Domain.Entities.Variante
                {
                    Id = Guid.NewGuid(),
                    ProdutoId = produto.Id,
                    Nome = varianteDto.Nome.Trim(),
                    PrecoAdicional = varianteDto.PrecoAdicional,
                    Ativo = true
                };
                await _unitOfWork.Variantes.AdicionarAsync(variante);
            }
        }

        // Criar adicionais se fornecidos
        if (request.Adicionais != null && request.Adicionais.Any())
        {
            foreach (var adicionalDto in request.Adicionais)
            {
                var adicional = new Domain.Entities.Adicional
                {
                    Id = Guid.NewGuid(),
                    ProdutoId = produto.Id,
                    Nome = adicionalDto.Nome.Trim(),
                    Preco = adicionalDto.Preco,
                    Ativo = true
                };
                await _unitOfWork.Adicionais.AdicionarAsync(adicional);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(produto);
    }

    public async Task<ProdutoResponse> AtualizarAsync(Guid id, AtualizarProdutoRequest request)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(id);
        if (produto == null)
            throw new InvalidOperationException($"Produto com ID {id} não encontrado");

        produto.Nome = request.Nome.Trim();
        produto.Descricao = request.Descricao?.Trim() ?? "";
        produto.Preco = request.Preco;
        produto.Ordem = request.Ordem;
        produto.Status = request.Status;
        produto.ImagemUrl = request.ImagemUrl?.Trim();
        produto.ImagemBlobName = request.ImagemBlobName?.Trim();
        produto.DataAtualizacao = DateTime.UtcNow;

        await _unitOfWork.Produtos.AtualizarAsync(produto);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(produto);
    }

    public async Task AtivarAsync(Guid id)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(id);
        if (produto == null)
            throw new InvalidOperationException($"Produto com ID {id} não encontrado");

        produto.Ativo = true;
        produto.Status = "ativo";
        await _unitOfWork.Produtos.AtualizarAsync(produto);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DesativarAsync(Guid id)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(id);
        if (produto == null)
            throw new InvalidOperationException($"Produto com ID {id} não encontrado");

        produto.Ativo = false;
        produto.Status = "inativo";
        await _unitOfWork.Produtos.AtualizarAsync(produto);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeletarAsync(Guid id)
    {
        await _unitOfWork.Produtos.DeletarComCascataAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}
