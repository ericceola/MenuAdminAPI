using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface do repositório de AtributosProdutoValores
/// </summary>
public interface IAtributoProdutoValorRepository
{
    Task<AtributoProdutoValor?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<AtributoProdutoValor>> ObterPorAtributoAsync(Guid atributoProdutoId);
    Task<IEnumerable<AtributoProdutoValor>> ObterAtivosPorAtributoAsync(Guid atributoProdutoId);
    Task AdicionarAsync(AtributoProdutoValor valor);
    Task AtualizarAsync(AtributoProdutoValor valor);
    Task DeletarAsync(Guid id);
    Task<bool> ValorJaExisteAsync(string valor, Guid atributoProdutoId, Guid? idExcluir = null);
}
