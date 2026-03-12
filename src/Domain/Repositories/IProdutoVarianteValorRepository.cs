using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface do repositório de ProdutoVariantesValores (ligação variante e atributo valor)
/// </summary>
public interface IProdutoVarianteValorRepository
{
    Task<IEnumerable<ProdutoVarianteValor>> ObterPorVarianteAsync(Guid produtoVarianteId);
    Task AdicionarAsync(ProdutoVarianteValor valor);
    Task DeletarPorVarianteAsync(Guid produtoVarianteId);
    Task DeletarAsync(Guid id);
}
