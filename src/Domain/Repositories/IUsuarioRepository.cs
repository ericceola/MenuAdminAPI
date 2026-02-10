using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface de Repositório para Usuários
/// </summary>
public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorIdAsync(Guid id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<IEnumerable<Usuario>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<IEnumerable<Usuario>> ObterAtivosPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<IEnumerable<Usuario>> BuscarAsync(string termo);
    Task AdicionarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task RemoverAsync(Guid id);
    Task<bool> EmailJaExisteAsync(string email, Guid? idExcluir = null);
    Task<bool> ExisteAsync(Guid id);
    Task<int> ContarPorEstabelecimentoAsync(Guid estabelecimentoId);
    Task<int> ContarAtivosAsync();
    Task AtivarAsync(Guid id);
    Task DesativarAsync(Guid id);
    Task AtualizarSenhaAsync(Guid id, string novaSenhaHash);
    Task AtualizarUltimoAcessoAsync(Guid id);
}
