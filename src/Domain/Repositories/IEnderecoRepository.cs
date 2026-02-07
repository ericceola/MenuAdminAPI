using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface de Repositório para Endereços
/// </summary>
public interface IEnderecoRepository
{
    Task<Endereco?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Endereco>> ObterPorClienteAsync(Guid clienteId);
    Task<Endereco?> ObterPadraoAsync(Guid clienteId);
    Task<IEnumerable<Endereco>> BuscarAsync(string termo);
    Task AdicionarAsync(Endereco endereco);
    Task AtualizarAsync(Endereco endereco);
    Task RemoverAsync(Guid id);
    Task<bool> EnderecoJaExisteAsync(Guid clienteId, string rua, string numero, string bairro, Guid? idExcluir = null);
    Task<bool> ExisteAsync(Guid id);
    Task<int> ContarPorClienteAsync(Guid clienteId);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task DefinirComoPadraoAsync(Guid clienteId, Guid enderecoId);
}
