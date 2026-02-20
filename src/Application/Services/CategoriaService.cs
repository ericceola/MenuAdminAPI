using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaResponse>> ObterTodasAsync();
    Task<CategoriaResponse> ObterPorIdAsync(Guid id);
    Task<IEnumerable<CategoriaResponse>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<IEnumerable<CategoriaResponse>> ObterAtivasPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<IEnumerable<CategoriaResponse>> BuscarAsync(string termo);
    Task<CategoriaResponse> CriarAsync(CriarCategoriaRequest request);
    Task AtualizarAsync(Guid id, AtualizarCategoriaRequest request);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task DeletarAsync(Guid id);
}

public class CategoriaService : ICategoriaService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoriaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoriaResponse>> ObterTodasAsync()
    {
        // Retorna categorias ativas de todos os estabelecimentos
        var categorias = await _unitOfWork.Categorias.BuscarAsync("");
        return categorias.Select(MapToCategoriaResponse);
    }

    public async Task<IEnumerable<CategoriaResponse>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        var categorias = await _unitOfWork.Categorias.ObterPorEstabelecimentoAsync(estabelecimentoId);
        return categorias.Select(MapToCategoriaResponse);
    }

    public async Task<IEnumerable<CategoriaResponse>> ObterAtivasPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        var categorias = await _unitOfWork.Categorias.ObterAtivasPorEstabelecimentoAsync(estabelecimentoId);
        return categorias.Select(MapToCategoriaResponse);
    }

    public async Task<CategoriaResponse> ObterPorIdAsync(Guid id)
    {
        var categoria = await _unitOfWork.Categorias.ObterPorIdAsync(id);
        if (categoria == null)
            throw new InvalidOperationException($"Categoria com ID {id} não encontrada");
        
        return MapToCategoriaResponse(categoria);
    }

    public async Task<IEnumerable<CategoriaResponse>> BuscarAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            throw new ArgumentException("Termo de busca não pode estar vazio");

        var categorias = await _unitOfWork.Categorias.BuscarAsync(termo);
        return categorias.Select(MapToCategoriaResponse);
    }

    public async Task<CategoriaResponse> CriarAsync(CriarCategoriaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("Nome da categoria é obrigatório");

        // TODO: Implementar validação de duplicatas
        
        var categoria = new Domain.Entities.Categoria
        {
            Id = Guid.NewGuid(),
            EstabelecimentoId = request.EstabelecimentoId,
            Nome = request.Nome.Trim(),
            Descricao = request.Descricao?.Trim() ?? "",
            Ordem = request.Ordem,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };

        await _unitOfWork.Categorias.AdicionarAsync(categoria);
        await _unitOfWork.SaveChangesAsync();

        return MapToCategoriaResponse(categoria);
    }

    public async Task AtualizarAsync(Guid id, AtualizarCategoriaRequest request)
    {
        var categoria = await _unitOfWork.Categorias.ObterPorIdAsync(id);
        if (categoria == null)
            throw new InvalidOperationException($"Categoria com ID {id} não encontrada");

        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("Nome da categoria é obrigatório");

        categoria.Nome = request.Nome.Trim();
        categoria.Descricao = request.Descricao?.Trim() ?? "";
        categoria.Ordem = request.Ordem;

        await _unitOfWork.Categorias.AtualizarAsync(categoria);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AtivarAsync(Guid id)
    {
        var categoria = await _unitOfWork.Categorias.ObterPorIdAsync(id);
        if (categoria == null)
            throw new InvalidOperationException($"Categoria com ID {id} não encontrada");

        categoria.Ativo = true;
        await _unitOfWork.Categorias.AtualizarAsync(categoria);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DesativarAsync(Guid id)
    {
        var categoria = await _unitOfWork.Categorias.ObterPorIdAsync(id);
        if (categoria == null)
            throw new InvalidOperationException($"Categoria com ID {id} não encontrada");

        categoria.Ativo = false;
        await _unitOfWork.Categorias.AtualizarAsync(categoria);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeletarAsync(Guid id)
    {
        var categoria = await _unitOfWork.Categorias.ObterPorIdAsync(id);
        if (categoria == null)
            throw new InvalidOperationException($"Categoria com ID {id} não encontrada");

        await _unitOfWork.Categorias.RemoverAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    private static CategoriaResponse MapToCategoriaResponse(Domain.Entities.Categoria categoria)
    {
        return new CategoriaResponse(
            Id: categoria.Id,
            EstabelecimentoId: categoria.EstabelecimentoId,
            Nome: categoria.Nome,
            Descricao: categoria.Descricao,
            Ordem: categoria.Ordem,
            Ativo: categoria.Ativo,
            DataCriacao: categoria.DataCriacao
        );
    }
}
