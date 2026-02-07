using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de Clientes com Dapper
/// </summary>
public class ClienteRepository : RepositoryBase<Cliente>, IClienteRepository
{
    public ClienteRepository(IDbConnection connection)
        : base(connection, "Clientes")
    {
    }

    /// <summary>
    /// Obter clientes por estabelecimento
    /// </summary>
    public async Task<IEnumerable<Cliente>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT Id, EstabelecimentoId, Nome, Email, Telefone, CPF, Ativo, DataCriacao, DataAtualizacao
            FROM Clientes
            WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1
            ORDER BY Nome";

        return await _connection.QueryAsync<Cliente>(sql, new { EstabelecimentoId = estabelecimentoId });
    }

    /// <summary>
    /// Buscar clientes por termo
    /// </summary>
    public async Task<IEnumerable<Cliente>> BuscarAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return Enumerable.Empty<Cliente>();

        const string sql = @"
            SELECT Id, EstabelecimentoId, Nome, Email, Telefone, CPF, Ativo, DataCriacao, DataAtualizacao
            FROM Clientes
            WHERE Ativo = 1 AND (Nome LIKE @Termo OR Email LIKE @Termo OR Telefone LIKE @Termo OR CPF LIKE @Termo)
            ORDER BY Nome";

        var termoLike = $"%{termo}%";
        return await _connection.QueryAsync<Cliente>(sql, new { Termo = termoLike });
    }

    /// <summary>
    /// Contar clientes por estabelecimento
    /// </summary>
    public async Task<int> ContarPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT COUNT(*) FROM Clientes
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
            SELECT COUNT(*) FROM Clientes
            WHERE Email = @Email AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { Email = email, IdExcluir = idExcluir });
        return count > 0;
    }

    /// <summary>
    /// Verificar se CPF já existe
    /// </summary>
    public async Task<bool> CpfJaExisteAsync(string cpf, Guid? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        const string sql = @"
            SELECT COUNT(*) FROM Clientes
            WHERE CPF = @Cpf AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { Cpf = cpf, IdExcluir = idExcluir });
        return count > 0;
    }

    /// <summary>
    /// Obter estatísticas do cliente
    /// </summary>
    public async Task<ClienteEstatisticas> ObterEstatisticasAsync(Guid clienteId)
    {
        const string sql = @"
            SELECT
                COUNT(p.Id) AS TotalPedidos,
                ISNULL(SUM(p.ValorFinal), 0) AS ValorTotalGasto,
                ISNULL(AVG(p.ValorFinal), 0) AS TicketMedio,
                MAX(p.DataCriacao) AS UltimoPedido
            FROM Clientes c
            LEFT JOIN Pedidos p ON c.Id = p.ClienteId AND p.Status IN (1, 2, 3)
            WHERE c.Id = @ClienteId";

        return await _connection.QueryFirstAsync<ClienteEstatisticas>(sql, new { ClienteId = clienteId });
    }

    /// <summary>
    /// Ativar cliente
    /// </summary>
    public async Task AtivarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Clientes
            SET Ativo = 1, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Desativar cliente
    /// </summary>
    public async Task DesativarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Clientes
            SET Ativo = 0, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Deletar cliente com cascata
    /// </summary>
    public async Task DeletarComCascataAsync(Guid id)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            try
            {
                // Deletar adicionais de pedidos
                const string sqlAdicionalPedidos = @"
                    DELETE FROM AdicionalPedidos
                    WHERE ItemPedidoId IN (
                        SELECT ip.Id FROM ItemPedidos ip
                        INNER JOIN Pedidos p ON ip.PedidoId = p.Id
                        WHERE p.ClienteId = @ClienteId
                    )";
                await _connection.ExecuteAsync(sqlAdicionalPedidos, new { ClienteId = id }, transaction);

                // Deletar itens de pedidos
                const string sqlItemPedidos = @"
                    DELETE FROM ItemPedidos
                    WHERE PedidoId IN (SELECT Id FROM Pedidos WHERE ClienteId = @ClienteId)";
                await _connection.ExecuteAsync(sqlItemPedidos, new { ClienteId = id }, transaction);

                // Deletar pedidos
                await _connection.ExecuteAsync(
                    "DELETE FROM Pedidos WHERE ClienteId = @ClienteId",
                    new { ClienteId = id },
                    transaction);

                // Deletar endereços
                await _connection.ExecuteAsync(
                    "DELETE FROM Enderecos WHERE ClienteId = @ClienteId",
                    new { ClienteId = id },
                    transaction);

                // Deletar cliente
                await _connection.ExecuteAsync(
                    "DELETE FROM Clientes WHERE Id = @Id",
                    new { Id = id },
                    transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    /// <summary>
    /// Obter clientes com mais pedidos
    /// </summary>
    public async Task<IEnumerable<ClienteMaisPedidos>> ObterComMaisPedidosAsync(Guid estabelecimentoId, int top = 10)
    {
        const string sql = @"
            SELECT TOP (@Top)
                c.Id,
                c.Nome,
                COUNT(p.Id) AS TotalPedidos,
                SUM(p.ValorFinal) AS ValorTotalGasto,
                MAX(p.DataCriacao) AS UltimoPedido
            FROM Clientes c
            LEFT JOIN Pedidos p ON c.Id = p.ClienteId AND p.Status IN (1, 2, 3)
            WHERE c.EstabelecimentoId = @EstabelecimentoId AND c.Ativo = 1
            GROUP BY c.Id, c.Nome
            ORDER BY TotalPedidos DESC";

        return await _connection.QueryAsync<ClienteMaisPedidos>(sql, new { EstabelecimentoId = estabelecimentoId, Top = top });
    }
}

/// <summary>
/// DTO para estatísticas do cliente
/// </summary>
public class ClienteEstatisticas
{
    public int TotalPedidos { get; set; }
    public decimal ValorTotalGasto { get; set; }
    public decimal TicketMedio { get; set; }
    public DateTime? UltimoPedido { get; set; }
}

/// <summary>
/// DTO para clientes com mais pedidos
/// </summary>
public class ClienteMaisPedidos
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public int TotalPedidos { get; set; }
    public decimal ValorTotalGasto { get; set; }
    public DateTime? UltimoPedido { get; set; }
}
