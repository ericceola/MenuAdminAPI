using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de Adicionais com Dapper
/// </summary>
public class AdicionalRepository : RepositoryBase<Adicional>, IAdicionalRepository
{
    public AdicionalRepository(IDbConnection connection)
        : base(connection, "Adicionais")
    {
    }

    /// <summary>
    /// Obter adicionais por produto
    /// </summary>
    public async Task<IEnumerable<Adicional>> ObterPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            SELECT Id, ProdutoId, Nome, Preco, Ativo, DataCriacao, DataAtualizacao
            FROM Adicionais
            WHERE ProdutoId = @ProdutoId
            ORDER BY Nome";

        return await _connection.QueryAsync<Adicional>(sql, new { ProdutoId = produtoId });
    }

    /// <summary>
    /// Obter adicionais ativos por produto
    /// </summary>
    public async Task<IEnumerable<Adicional>> ObterAtivosPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            SELECT Id, ProdutoId, Nome, Preco, Ativo, DataCriacao, DataAtualizacao
            FROM Adicionais
            WHERE ProdutoId = @ProdutoId AND Ativo = 1
            ORDER BY Nome";

        return await _connection.QueryAsync<Adicional>(sql, new { ProdutoId = produtoId });
    }

    /// <summary>
    /// Obter adicional por nome
    /// </summary>
    public async Task<Adicional?> ObterPorNomeAsync(string nome, Guid produtoId)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return null;

        const string sql = @"
            SELECT Id, ProdutoId, Nome, Preco, Ativo, DataCriacao, DataAtualizacao
            FROM Adicionais
            WHERE Nome = @Nome AND ProdutoId = @ProdutoId";

        return await _connection.QueryFirstOrDefaultAsync<Adicional>(sql, new { Nome = nome, ProdutoId = produtoId });
    }

    /// <summary>
    /// Contar adicionais por produto
    /// </summary>
    public async Task<int> ContarPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            SELECT COUNT(*) FROM Adicionais
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
            SELECT COUNT(*) FROM Adicionais
            WHERE Nome = @Nome AND ProdutoId = @ProdutoId AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { Nome = nome, ProdutoId = produtoId, IdExcluir = idExcluir });
        return count > 0;
    }

    /// <summary>
    /// Buscar adicionais por termo
    /// </summary>
    public async Task<IEnumerable<Adicional>> BuscarAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return Enumerable.Empty<Adicional>();

        const string sql = @"
            SELECT Id, ProdutoId, Nome, Preco, Ativo, DataCriacao, DataAtualizacao
            FROM Adicionais
            WHERE Ativo = 1 AND Nome LIKE @Termo
            ORDER BY Nome";

        var termoLike = $"%{termo}%";
        return await _connection.QueryAsync<Adicional>(sql, new { Termo = termoLike });
    }

    /// <summary>
    /// Obter adicionais mais usados
    /// </summary>
    public async Task<IEnumerable<AdicionalMaisUsado>> ObterMaisUsadosAsync(Guid estabelecimentoId, int top = 10)
    {
        const string sql = @"
            SELECT TOP (@Top)
                a.Id,
                a.Nome,
                SUM(ap.Quantidade) AS TotalUsado,
                SUM(ap.PrecoTotal) AS Faturamento
            FROM AdicionalPedidos ap
            INNER JOIN Adicionais a ON ap.AdicionalId = a.Id
            INNER JOIN ItemPedidos ip ON ap.ItemPedidoId = ip.Id
            INNER JOIN Pedidos p ON ip.PedidoId = p.Id
            WHERE p.EstabelecimentoId = @EstabelecimentoId AND p.Status IN (1, 2, 3)
            GROUP BY a.Id, a.Nome
            ORDER BY TotalUsado DESC";

        return await _connection.QueryAsync<AdicionalMaisUsado>(sql, new { EstabelecimentoId = estabelecimentoId, Top = top });
    }

    /// <summary>
    /// Ativar adicional
    /// </summary>
    public async Task AtivarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Adicionais
            SET Ativo = 1, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Desativar adicional
    /// </summary>
    public async Task DesativarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Adicionais
            SET Ativo = 0, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }
}

/// <summary>
/// DTO para adicionais mais usados
/// </summary>
public class AdicionalMaisUsado
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TotalUsado { get; set; }
    public decimal Faturamento { get; set; }
}
