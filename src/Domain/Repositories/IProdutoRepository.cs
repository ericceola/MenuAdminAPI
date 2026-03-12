using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface de Repositório para Produtos
/// </summary>
public interface IProdutoRepository
{
    Task<Produto?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Produto>> ObterPorSubcategoriaAsync(Guid subcategoriaId);
    Task<IEnumerable<Produto>> ObterAtivosPorSubcategoriaAsync(Guid subcategoriaId);
    Task<IEnumerable<Produto>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<IEnumerable<Produto>> BuscarAsync(string termo);
    Task AdicionarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
    Task RemoverAsync(Guid id);
    Task<bool> NomeJaExisteAsync(string nome, Guid subcategoriaId, Guid? idExcluir = null);
    Task<bool> ExisteAsync(Guid id);
    Task<int> ContarPorSubcategoriaAsync(Guid subcategoriaId);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task DeletarComCascataAsync(Guid id);
}
