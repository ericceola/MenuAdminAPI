using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface do repositório de AtributosProduto
/// </summary>
public interface IAtributoProdutoRepository
{
    Task<AtributoProduto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<AtributoProduto>> ObterTodosAsync();
    Task<IEnumerable<AtributoProduto>> ObterAtivosAsync();
    Task<AtributoProduto?> ObterComValoresAsync(Guid id);
    Task<IEnumerable<AtributoProduto>> ObterTodosComValoresAsync();
    Task AdicionarAsync(AtributoProduto atributo);
    Task AtualizarAsync(AtributoProduto atributo);
    Task DeletarAsync(Guid id);
    Task<bool> NomeJaExisteAsync(string nome, Guid? idExcluir = null);
}
