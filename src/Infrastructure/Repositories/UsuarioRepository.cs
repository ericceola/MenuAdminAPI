using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de Usuários com Dapper
/// </summary>
public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(IDbConnection connection)
        : base(connection, "Usuarios")
    {
    }

    /// <summary>
    /// Obter usuário por email
    /// </summary>
    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        const string sql = @"
            SELECT Id, EstabelecimentoId, Nome, Email, Senha, Perfil, Status, Ativo, DataCriacao, DataAtualizacao
            FROM Usuarios
            WHERE Email = @Email AND Ativo = 1";

        return await _connection.QueryFirstOrDefaultAsync<Usuario>(sql, new { Email = email });
    }

    /// <summary>
    /// Obter usuários por estabelecimento
    /// </summary>
    public async Task<IEnumerable<Usuario>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT Id, EstabelecimentoId, Nome, Email, Senha AS SenhaHash, Perfil, Status, Ativo, DataCriacao, DataAtualizacao
            FROM Usuarios
            WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1
            ORDER BY Nome";

        return await _connection.QueryAsync<Usuario>(sql, new { EstabelecimentoId = estabelecimentoId });
    }

    /// <summary>
    /// Obter usuários ativos por estabelecimento
    /// </summary>
    public async Task<IEnumerable<Usuario>> ObterAtivosPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT Id, EstabelecimentoId, Nome, Email, Senha AS SenhaHash, Perfil, Status, Ativo, DataCriacao, DataAtualizacao
            FROM Usuarios
            WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1 AND Status = 0
            ORDER BY Nome";

        return await _connection.QueryAsync<Usuario>(sql, new { EstabelecimentoId = estabelecimentoId });
    }

    /// <summary>
    /// Buscar usuários por termo
    /// </summary>
    public async Task<IEnumerable<Usuario>> BuscarAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return Enumerable.Empty<Usuario>();

        const string sql = @"
            SELECT Id, EstabelecimentoId, Nome, Email, Senha AS SenhaHash, Perfil, Status, Ativo, DataCriacao, DataAtualizacao
            FROM Usuarios
            WHERE Ativo = 1 AND (Nome LIKE @Termo OR Email LIKE @Termo)
            ORDER BY Nome";

        var termoLike = $"%{termo}%";
        return await _connection.QueryAsync<Usuario>(sql, new { Termo = termoLike });
    }

    /// <summary>
    /// Contar usuários por estabelecimento
    /// </summary>
    public async Task<int> ContarPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT COUNT(*) FROM Usuarios
            WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1";

        return await _connection.QueryFirstAsync<int>(sql, new { EstabelecimentoId = estabelecimentoId });
    }

    /// <summary>
    /// Verificar se email já existe
    /// </summary>
    public async Task<bool> EmailJaExisteAsync(string email, Guid? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        const string sql = @"
            SELECT COUNT(*) FROM Usuarios
            WHERE Email = @Email AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { Email = email, IdExcluir = idExcluir });
        return count > 0;
    }

    /// <summary>
    /// Ativar usuário
    /// </summary>
    public async Task AtivarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Usuarios
            SET Ativo = 1, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Desativar usuário
    /// </summary>
    public async Task DesativarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Usuarios
            SET Ativo = 0, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Atualizar senha
    /// </summary>
    public async Task AtualizarSenhaAsync(Guid id, string novaSenhaHash)
    {
        const string sql = @"
            UPDATE Usuarios
            SET Senha = @SenhaHash, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id, SenhaHash = novaSenhaHash });
    }

    /// <summary>
    /// Atualizar último acesso
    /// </summary>
    public async Task AtualizarUltimoAcessoAsync(Guid id)
    {
        const string sql = @"
            UPDATE Usuarios
            SET DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Contar usuários ativos
    /// </summary>
    public async Task<int> ContarAtivosAsync()
    {
        const string sql = "SELECT COUNT(*) FROM Usuarios WHERE Ativo = 1";
        return await _connection.QueryFirstAsync<int>(sql);
    }
}
