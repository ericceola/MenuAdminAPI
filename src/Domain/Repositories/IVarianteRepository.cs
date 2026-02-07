using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface de Repositório para Variantes
/// </summary>
public interface IVarianteRepository
{
    Task<Variante?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Variante>> ObterPorProdutoAsync(Guid produtoId);
    Task<IEnumerable<Variante>> ObterAtivasPorProdutoAsync(Guid produtoId);
    Task<IEnumerable<Variante>> BuscarAsync(string termo);
    Task AdicionarAsync(Variante variante);
    Task AtualizarAsync(Variante variante);
    Task RemoverAsync(Guid id);
    Task<bool> NomeJaExisteAsync(string nome, Guid produtoId, Guid? idExcluir = null);
    Task<bool> ExisteAsync(Guid id);
    Task<int> ContarPorProdutoAsync(Guid produtoId);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
}
