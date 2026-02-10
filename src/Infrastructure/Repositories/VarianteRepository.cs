using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de Variantes com Dapper
/// </summary>
public class VarianteRepository : RepositoryBase<Variante>, IVarianteRepository
{
    public VarianteRepository(IDbConnection connection)
        : base(connection, "Variantes")
    {
    }

    /// <summary>
    /// Obter variantes por produto
    /// </summary>
    public async Task<IEnumerable<Variante>> ObterPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            SELECT Id, ProdutoId, Nome, Descricao, PrecoAdicional, Ativo, DataCriacao, DataAtualizacao
            FROM Variantes
            WHERE ProdutoId = @ProdutoId
            ORDER BY Nome";

        return await _connection.QueryAsync<Variante>(sql, new { ProdutoId = produtoId });
    }

    /// <summary>
    /// Obter variantes ativas por produto
    /// </summary>
    public async Task<IEnumerable<Variante>> ObterAtivasPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            SELECT Id, ProdutoId, Nome, Descricao, PrecoAdicional, Ativo, DataCriacao, DataAtualizacao
            FROM Variantes
            WHERE ProdutoId = @ProdutoId AND Ativo = 1
            ORDER BY Nome";

        return await _connection.QueryAsync<Variante>(sql, new { ProdutoId = produtoId });
    }

    /// <summary>
    /// Obter variante por nome
    /// </summary>
    public async Task<Variante?> ObterPorNomeAsync(string nome, Guid produtoId)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return null;

        const string sql = @"
            SELECT Id, ProdutoId, Nome, Descricao, PrecoAdicional, Ativo, DataCriacao, DataAtualizacao
            FROM Variantes
            WHERE Nome = @Nome AND ProdutoId = @ProdutoId";

        return await _connection.QueryFirstOrDefaultAsync<Variante>(sql, new { Nome = nome, ProdutoId = produtoId });
    }

    /// <summary>
    /// Contar variantes por produto
    /// </summary>
    public async Task<int> ContarPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            SELECT COUNT(*) FROM Variantes
            WHERE ProdutoId = @ProdutoId AND Ativo = 1";

        return await _connection.QueryFirstAsync<int>(sql, new { ProdutoId = produtoId });
    }

    /// <summary>
    /// Verificar se nome já existe
    /// </summary>
    public async Task<bool> NomeJaExisteAsync(string nome, Guid produtoId, Guid? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return false;

        const string sql = @"
            SELECT COUNT(*) FROM Variantes
            WHERE Nome = @Nome AND ProdutoId = @ProdutoId AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { Nome = nome, ProdutoId = produtoId, IdExcluir = idExcluir });
        return count > 0;
    }

    /// <summary>
    /// Buscar variantes por termo
    /// </summary>
    public async Task<IEnumerable<Variante>> BuscarAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return Enumerable.Empty<Variante>();

        const string sql = @"
            SELECT Id, ProdutoId, Nome, Descricao, PrecoAdicional, Ativo, DataCriacao, DataAtualizacao
            FROM Variantes
            WHERE Ativo = 1 AND (Nome LIKE @Termo OR Descricao LIKE @Termo)
            ORDER BY Nome";

        var termoLike = $"%{termo}%";
        return await _connection.QueryAsync<Variante>(sql, new { Termo = termoLike });
    }

    /// <summary>
    /// Ativar variante
    /// </summary>
    public async Task AtivarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Variantes
            SET Ativo = 1, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Desativar variante
    /// </summary>
    public async Task DesativarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Variantes
            SET Ativo = 0, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }
}
