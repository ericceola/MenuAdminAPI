using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

public interface ISubcategoriaService
{
    Task<IEnumerable<SubcategoriaResponse>> ObterTodasAsync();
    Task<SubcategoriaResponse> ObterPorIdAsync(Guid id);
    Task<IEnumerable<SubcategoriaResponse>> ObterPorCategoriaAsync(Guid categoriaId);
    Task<IEnumerable<SubcategoriaResponse>> ObterAtivasPorCategoriaAsync(Guid categoriaId);
    Task<IEnumerable<SubcategoriaResponse>> BuscarAsync(string termo);
    Task<SubcategoriaResponse> CriarAsync(CriarSubcategoriaRequest request);
    Task AtualizarAsync(Guid id, AtualizarSubcategoriaRequest request);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task DeletarAsync(Guid id);
}

public class SubcategoriaService : ISubcategoriaService
{
    private readonly IUnitOfWork _unitOfWork;

    public SubcategoriaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SubcategoriaResponse>> ObterTodasAsync()
    {
        // Retorna todas as subcategorias ativas
        var subcategorias = await _unitOfWork.Subcategorias.BuscarAsync("");
        return subcategorias.Select(MapToSubcategoriaResponse);
    }

    public async Task<SubcategoriaResponse> ObterPorIdAsync(Guid id)
    {
        var subcategoria = await _unitOfWork.Subcategorias.ObterPorIdAsync(id);
        if (subcategoria == null)
            throw new InvalidOperationException($"Subcategoria com ID {id} não encontrada");
        
        return MapToSubcategoriaResponse(subcategoria);
    }

    public async Task<IEnumerable<SubcategoriaResponse>> ObterPorCategoriaAsync(Guid categoriaId)
    {
        var subcategorias = await _unitOfWork.Subcategorias.ObterPorCategoriaAsync(categoriaId);
        return subcategorias.Select(MapToSubcategoriaResponse);
    }

    public async Task<IEnumerable<SubcategoriaResponse>> ObterAtivasPorCategoriaAsync(Guid categoriaId)
    {
        var subcategorias = await _unitOfWork.Subcategorias.ObterAtivasPorCategoriaAsync(categoriaId);
        return subcategorias.Select(MapToSubcategoriaResponse);
    }

    public async Task<IEnumerable<SubcategoriaResponse>> BuscarAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            throw new ArgumentException("Termo de busca não pode estar vazio");

        var subcategorias = await _unitOfWork.Subcategorias.BuscarAsync(termo);
        return subcategorias.Select(MapToSubcategoriaResponse);
    }

    public async Task<SubcategoriaResponse> CriarAsync(CriarSubcategoriaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("Nome da subcategoria é obrigatório");

        // Validar se categoria existe
        var categoria = await _unitOfWork.Categorias.ObterPorIdAsync(request.CategoriaId);
        if (categoria == null)
            throw new InvalidOperationException($"Categoria com ID {request.CategoriaId} não encontrada");

        var subcategoria = new Domain.Entities.Subcategoria
        {
            Id = Guid.NewGuid(),
            CategoriaId = request.CategoriaId,
            EstabelecimentoId = categoria.EstabelecimentoId,
            Nome = request.Nome.Trim(),
            Descricao = request.Descricao?.Trim() ?? "",
            Ordem = request.Ordem,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };

        await _unitOfWork.Subcategorias.AdicionarAsync(subcategoria);
        await _unitOfWork.SaveChangesAsync();

        return MapToSubcategoriaResponse(subcategoria);
    }

    public async Task AtualizarAsync(Guid id, AtualizarSubcategoriaRequest request)
    {
        var subcategoria = await _unitOfWork.Subcategorias.ObterPorIdAsync(id);
        if (subcategoria == null)
            throw new InvalidOperationException($"Subcategoria com ID {id} não encontrada");

        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("Nome da subcategoria é obrigatório");

        subcategoria.Nome = request.Nome.Trim();
        subcategoria.Descricao = request.Descricao?.Trim() ?? "";
        subcategoria.Ordem = request.Ordem;

        await _unitOfWork.Subcategorias.AtualizarAsync(subcategoria);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AtivarAsync(Guid id)
    {
        var subcategoria = await _unitOfWork.Subcategorias.ObterPorIdAsync(id);
        if (subcategoria == null)
            throw new InvalidOperationException($"Subcategoria com ID {id} não encontrada");

        subcategoria.Ativo = true;
        await _unitOfWork.Subcategorias.AtualizarAsync(subcategoria);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DesativarAsync(Guid id)
    {
        var subcategoria = await _unitOfWork.Subcategorias.ObterPorIdAsync(id);
        if (subcategoria == null)
            throw new InvalidOperationException($"Subcategoria com ID {id} não encontrada");

        subcategoria.Ativo = false;
        await _unitOfWork.Subcategorias.AtualizarAsync(subcategoria);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeletarAsync(Guid id)
    {
        var subcategoria = await _unitOfWork.Subcategorias.ObterPorIdAsync(id);
        if (subcategoria == null)
            throw new InvalidOperationException($"Subcategoria com ID {id} não encontrada");

        await _unitOfWork.Subcategorias.RemoverAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    private static SubcategoriaResponse MapToSubcategoriaResponse(Domain.Entities.Subcategoria subcategoria)
    {
        return new SubcategoriaResponse(
            Id: subcategoria.Id,
            CategoriaId: subcategoria.CategoriaId,
            Nome: subcategoria.Nome,
            Descricao: subcategoria.Descricao,
            Ordem: subcategoria.Ordem,
            Ativo: subcategoria.Ativo,
            DataCriacao: subcategoria.DataCriacao
        );
    }
}
