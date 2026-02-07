using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface de Repositório para Categorias
/// </summary>
public interface ICategoriaRepository
{
    Task<Categoria?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Categoria>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<IEnumerable<Categoria>> ObterAtivasPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<IEnumerable<Categoria>> BuscarAsync(string termo);
    Task AdicionarAsync(Categoria categoria);
    Task AtualizarAsync(Categoria categoria);
    Task RemoverAsync(Guid id);
    Task<bool> NomeJaExisteAsync(string nome, Guid estabelecimentoId, Guid? idExcluir = null);
    Task<bool> ExisteAsync(Guid id);
    Task<int> ContarPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task DeletarComCascataAsync(Guid id);
}
