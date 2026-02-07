using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface de Repositório para Pedidos
/// </summary>
public interface IPedidoRepository
{
    Task<Pedido?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Pedido>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<IEnumerable<Pedido>> ObterPorClienteAsync(Guid clienteId);
    Task<IEnumerable<Pedido>> ObterPorPeriodoAsync(Guid estabelecimentoId, DateTime dataInicio, DateTime dataFim);
    Task<IEnumerable<Pedido>> ObterPorStatusAsync(Guid estabelecimentoId, int status);
    Task AdicionarAsync(Pedido pedido);
    Task AtualizarAsync(Pedido pedido);
    Task RemoverAsync(Guid id);
    Task<bool> ExisteAsync(Guid id);
    Task<int> ContarPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task MudarStatusAsync(Guid id, int novoStatus);
    Task CancelarAsync(Guid id, string motivo);
    Task DeletarComCascataAsync(Guid id);
}
