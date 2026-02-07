using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface de Repositório para Adicionais
/// </summary>
public interface IAdicionalRepository
{
    Task<Adicional?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Adicional>> ObterPorProdutoAsync(Guid produtoId);
    Task<IEnumerable<Adicional>> ObterAtivosPorProdutoAsync(Guid produtoId);
    Task<IEnumerable<Adicional>> BuscarAsync(string termo);
    Task AdicionarAsync(Adicional adicional);
    Task AtualizarAsync(Adicional adicional);
    Task RemoverAsync(Guid id);
    Task<bool> NomeJaExisteAsync(Guid produtoId, string nome, Guid? idExcluir = null);
    Task<bool> ExisteAsync(Guid id);
    Task<int> ContarPorProdutoAsync(Guid produtoId);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
}
