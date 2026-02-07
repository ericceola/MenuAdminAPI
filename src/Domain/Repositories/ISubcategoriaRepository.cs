using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface de Repositório para Subcategorias
/// </summary>
public interface ISubcategoriaRepository
{
    Task<Subcategoria?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Subcategoria>> ObterPorCategoriaAsync(Guid categoriaId);
    Task<IEnumerable<Subcategoria>> ObterAtivasPorCategoriaAsync(Guid categoriaId);
    Task<IEnumerable<Subcategoria>> BuscarAsync(string termo);
    Task AdicionarAsync(Subcategoria subcategoria);
    Task AtualizarAsync(Subcategoria subcategoria);
    Task RemoverAsync(Guid id);
    Task<bool> NomeJaExisteAsync(Guid categoriaId, string nome, Guid? idExcluir = null);
    Task<bool> ExisteAsync(Guid id);
    Task<int> ContarPorCategoriaAsync(Guid categoriaId);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task DeletarComCascataAsync(Guid id);
}
