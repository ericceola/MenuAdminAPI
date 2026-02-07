using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface de Repositório para Estabelecimentos
/// </summary>
public interface IEstabelecimentoRepository
{
    Task<Estabelecimento?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Estabelecimento>> ObterTodosAsync();
    Task<IEnumerable<Estabelecimento>> ObterAtivosAsync();
    Task<IEnumerable<Estabelecimento>> ObterPorPlanoAsync(string plano);
    Task<IEnumerable<Estabelecimento>> BuscarAsync(string termo);
    Task AdicionarAsync(Estabelecimento estabelecimento);
    Task AtualizarAsync(Estabelecimento estabelecimento);
    Task RemoverAsync(Guid id);
    Task<bool> EmailJaExisteAsync(string email, Guid? idExcluir = null);
    Task<bool> CnpjJaExisteAsync(string cnpj, Guid? idExcluir = null);
    Task<bool> ExisteAsync(Guid id);
    Task<int> ContarAsync();
    Task<int> ContarAtivosAsync();
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
}
