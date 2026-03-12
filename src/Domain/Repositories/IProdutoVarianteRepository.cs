using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface do repositório de ProdutoVariantes
/// </summary>
public interface IProdutoVarianteRepository
{
    Task<ProdutoVariante?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<ProdutoVariante>> ObterPorProdutoAsync(Guid produtoId);
    Task<IEnumerable<ProdutoVariante>> ObterAtivosPorProdutoAsync(Guid produtoId);
    Task<ProdutoVariante?> ObterComAtributosAsync(Guid id);
    Task<IEnumerable<ProdutoVariante>> ObterTodosComAtributosPorProdutoAsync(Guid produtoId);
    Task AdicionarAsync(ProdutoVariante variante);
    Task AtualizarAsync(ProdutoVariante variante);
    Task DeletarAsync(Guid id);
    Task DeletarPorProdutoAsync(Guid produtoId);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task<bool> CombinacaoAtributosJaExisteAsync(Guid produtoId, IEnumerable<Guid> atributoValorIds, Guid? varianteIdExcluir = null);
}
