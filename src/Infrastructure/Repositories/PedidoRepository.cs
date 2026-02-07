using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de Pedidos com Dapper
/// </summary>
public class PedidoRepository : RepositoryBase<Pedido>, IPedidoRepository
{
    public PedidoRepository(IDbConnection connection)
        : base(connection, "Pedidos")
    {
    }

    /// <summary>
    /// Obter pedidos por estabelecimento
    /// </summary>
    public async Task<IEnumerable<Pedido>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT Id, EstabelecimentoId, ClienteId, EnderecoId, NumeroNota, Status, ValorTotal, ValorDesconto, ValorFinal, Observacoes, MotivoCancelamento, DataCriacao, DataAtualizacao
            FROM Pedidos
            WHERE EstabelecimentoId = @EstabelecimentoId
            ORDER BY DataCriacao DESC";

        return await _connection.QueryAsync<Pedido>(sql, new { EstabelecimentoId = estabelecimentoId });
    }

    /// <summary>
    /// Obter pedidos por cliente
    /// </summary>
    public async Task<IEnumerable<Pedido>> ObterPorClienteAsync(Guid clienteId)
    {
        const string sql = @"
            SELECT Id, EstabelecimentoId, ClienteId, EnderecoId, NumeroNota, Status, ValorTotal, ValorDesconto, ValorFinal, Observacoes, MotivoCancelamento, DataCriacao, DataAtualizacao
            FROM Pedidos
            WHERE ClienteId = @ClienteId
            ORDER BY DataCriacao DESC";

        return await _connection.QueryAsync<Pedido>(sql, new { ClienteId = clienteId });
    }

    /// <summary>
    /// Obter pedidos por período
    /// </summary>
    public async Task<IEnumerable<Pedido>> ObterPorPeriodoAsync(Guid estabelecimentoId, DateTime dataInicio, DateTime dataFim)
    {
        const string sql = @"
            SELECT Id, EstabelecimentoId, ClienteId, EnderecoId, NumeroNota, Status, ValorTotal, ValorDesconto, ValorFinal, Observacoes, MotivoCancelamento, DataCriacao, DataAtualizacao
            FROM Pedidos
            WHERE EstabelecimentoId = @EstabelecimentoId AND DataCriacao >= @DataInicio AND DataCriacao <= @DataFim
            ORDER BY DataCriacao DESC";

        return await _connection.QueryAsync<Pedido>(sql, new { EstabelecimentoId = estabelecimentoId, DataInicio = dataInicio, DataFim = dataFim });
    }

    /// <summary>
    /// Obter pedidos por status
    /// </summary>
    public async Task<IEnumerable<Pedido>> ObterPorStatusAsync(Guid estabelecimentoId, int status)
    {
        const string sql = @"
            SELECT Id, EstabelecimentoId, ClienteId, EnderecoId, NumeroNota, Status, ValorTotal, ValorDesconto, ValorFinal, Observacoes, MotivoCancelamento, DataCriacao, DataAtualizacao
            FROM Pedidos
            WHERE EstabelecimentoId = @EstabelecimentoId AND Status = @Status
            ORDER BY DataCriacao DESC";

        return await _connection.QueryAsync<Pedido>(sql, new { EstabelecimentoId = estabelecimentoId, Status = status });
    }

    /// <summary>
    /// Contar pedidos por estabelecimento
    /// </summary>
    public async Task<int> ContarPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        const string sql = "SELECT COUNT(*) FROM Pedidos WHERE EstabelecimentoId = @EstabelecimentoId";
        return await _connection.QueryFirstAsync<int>(sql, new { EstabelecimentoId = estabelecimentoId });
    }

    /// <summary>
    /// Obter estatísticas de pedidos
    /// </summary>
    public async Task<PedidoEstatisticas> ObterEstatisticasAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT
                COUNT(*) AS TotalPedidos,
                (SELECT COUNT(*) FROM Pedidos WHERE EstabelecimentoId = @EstabelecimentoId AND Status = 0) AS Pendentes,
                (SELECT COUNT(*) FROM Pedidos WHERE EstabelecimentoId = @EstabelecimentoId AND Status = 1) AS Confirmados,
                (SELECT COUNT(*) FROM Pedidos WHERE EstabelecimentoId = @EstabelecimentoId AND Status = 2) AS EmPreparacao,
                (SELECT COUNT(*) FROM Pedidos WHERE EstabelecimentoId = @EstabelecimentoId AND Status = 3) AS Entregues,
                (SELECT COUNT(*) FROM Pedidos WHERE EstabelecimentoId = @EstabelecimentoId AND Status = 4) AS Cancelados,
                (SELECT ISNULL(SUM(ValorFinal), 0) FROM Pedidos WHERE EstabelecimentoId = @EstabelecimentoId AND Status IN (1, 2, 3)) AS Faturamento,
                (SELECT ISNULL(AVG(ValorFinal), 0) FROM Pedidos WHERE EstabelecimentoId = @EstabelecimentoId AND Status IN (1, 2, 3)) AS TicketMedio
            FROM Pedidos
            WHERE EstabelecimentoId = @EstabelecimentoId";

        return await _connection.QueryFirstAsync<PedidoEstatisticas>(sql, new { EstabelecimentoId = estabelecimentoId });
    }

    /// <summary>
    /// Mudar status do pedido
    /// </summary>
    public async Task MudarStatusAsync(Guid id, int novoStatus)
    {
        const string sql = @"
            UPDATE Pedidos
            SET Status = @Status, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id, Status = novoStatus });
    }

    /// <summary>
    /// Cancelar pedido com motivo
    /// </summary>
    public async Task CancelarAsync(Guid id, string motivo)
    {
        const string sql = @"
            UPDATE Pedidos
            SET Status = 4, MotivoCancelamento = @Motivo, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id, Motivo = motivo });
    }

    /// <summary>
    /// Obter faturamento por período
    /// </summary>
    public async Task<IEnumerable<FaturamentoPeriodo>> ObterFaturamentoPorPeriodoAsync(Guid estabelecimentoId, DateTime dataInicio, DateTime dataFim)
    {
        const string sql = @"
            SELECT
                CONVERT(DATE, DataCriacao) AS Data,
                COUNT(*) AS TotalPedidos,
                SUM(ValorFinal) AS Faturamento,
                AVG(ValorFinal) AS TicketMedio
            FROM Pedidos
            WHERE EstabelecimentoId = @EstabelecimentoId AND Status IN (1, 2, 3) AND DataCriacao >= @DataInicio AND DataCriacao <= @DataFim
            GROUP BY CONVERT(DATE, DataCriacao)
            ORDER BY Data DESC";

        return await _connection.QueryAsync<FaturamentoPeriodo>(sql, new { EstabelecimentoId = estabelecimentoId, DataInicio = dataInicio, DataFim = dataFim });
    }

    /// <summary>
    /// Deletar pedido com cascata
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
                    WHERE ItemPedidoId IN (SELECT Id FROM ItemPedidos WHERE PedidoId = @PedidoId)";
                await _connection.ExecuteAsync(sqlAdicionalPedidos, new { PedidoId = id }, transaction);

                // Deletar itens de pedidos
                await _connection.ExecuteAsync(
                    "DELETE FROM ItemPedidos WHERE PedidoId = @PedidoId",
                    new { PedidoId = id },
                    transaction);

                // Deletar pedido
                await _connection.ExecuteAsync(
                    "DELETE FROM Pedidos WHERE Id = @Id",
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
}

/// <summary>
/// DTO para estatísticas de pedidos
/// </summary>
public class PedidoEstatisticas
{
    public int TotalPedidos { get; set; }
    public int Pendentes { get; set; }
    public int Confirmados { get; set; }
    public int EmPreparacao { get; set; }
    public int Entregues { get; set; }
    public int Cancelados { get; set; }
    public decimal Faturamento { get; set; }
    public decimal TicketMedio { get; set; }
}

/// <summary>
/// DTO para faturamento por período
/// </summary>
public class FaturamentoPeriodo
{
    public DateTime Data { get; set; }
    public int TotalPedidos { get; set; }
    public decimal Faturamento { get; set; }
    public decimal TicketMedio { get; set; }
}
