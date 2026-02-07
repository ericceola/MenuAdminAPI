using MenuAdminAPI.Application.DTOs;

namespace MenuAdminAPI.Application.Services;

/// <summary>
/// Interface de Serviço para Estabelecimentos
/// </summary>
public interface IEstabelecimentoService
{
    Task<EstabelecimentoResponse?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<EstabelecimentoResponse>> ObterTodosAsync();
    Task<IEnumerable<EstabelecimentoResponse>> ObterAtivosAsync();
    Task<IEnumerable<EstabelecimentoResponse>> BuscarAsync(string termo);
    Task<EstabelecimentoResponse> CriarAsync(CriarEstabelecimentoRequest request);
    Task<EstabelecimentoResponse> AtualizarAsync(Guid id, AtualizarEstabelecimentoRequest request);
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task DeletarAsync(Guid id);
}
