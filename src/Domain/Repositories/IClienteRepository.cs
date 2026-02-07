using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface de Repositório para Clientes
/// </summary>
public interface IClienteRepository
{
    Task<Cliente?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Cliente>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<IEnumerable<Cliente>> BuscarAsync(string termo);
    Task AdicionarAsync(Cliente cliente);
    Task AtualizarAsync(Cliente cliente);
    Task RemoverAsync(Guid id);
    Task<bool> EmailJaExisteAsync(Guid estabelecimentoId, string email, Guid? idExcluir = null);
    Task<bool> CpfJaExisteAsync(Guid estabelecimentoId, string cpf, Guid? idExcluir = null);
    Task<bool> ExisteAsync(Guid id);
    Task<int> ContarPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task DeletarComCascataAsync(Guid id);
}
