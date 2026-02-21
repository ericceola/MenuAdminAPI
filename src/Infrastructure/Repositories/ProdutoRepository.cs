using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de Produtos com Dapper
/// </summary>
public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
{
    public ProdutoRepository(IDbConnection connection)
        : base(connection, "Produtos")
    {
    }

    /// <summary>
    /// Obter produtos por subcategoria
    /// </summary>
    public async Task<IEnumerable<Produto>> ObterPorSubcategoriaAsync(Guid subcategoriaId)
    {
        const string sql = @"
            SELECT Id, SubcategoriaId, Nome, Descricao, Preco, ImagemUrl, Ativo, DataCriacao, DataAtualizacao
            FROM Produtos
            WHERE SubcategoriaId = @SubcategoriaId AND Ativo = 1
            ORDER BY Nome";

        return await _connection.QueryAsync<Produto>(sql, new { SubcategoriaId = subcategoriaId });
    }

    /// <summary>
    /// Obter produtos ativos por subcategoria
    /// </summary>
    public async Task<IEnumerable<Produto>> ObterAtivosPorSubcategoriaAsync(Guid subcategoriaId)
    {
        const string sql = @"
            SELECT Id, SubcategoriaId, Nome, Descricao, Preco, ImagemUrl, Ativo, DataCriacao, DataAtualizacao
            FROM Produtos
            WHERE SubcategoriaId = @SubcategoriaId AND Ativo = 1
            ORDER BY Nome";

        return await _connection.QueryAsync<Produto>(sql, new { SubcategoriaId = subcategoriaId });
    }

    /// <summary>
    /// Buscar produtos por termo (retorna todos se termo vazio)
    /// </summary>
    public async Task<IEnumerable<Produto>> BuscarAsync(string termo)
    {
        // Se termo vazio, busca todos os produtos ativos
        var termoLike = string.IsNullOrWhiteSpace(termo) ? "%" : $"%{termo}%";

        const string sql = @"
            SELECT Id, SubcategoriaId, Nome, Descricao, Preco, ImagemUrl, Ativo, DataCriacao, DataAtualizacao
            FROM Produtos
            WHERE Ativo = 1 AND (Nome LIKE @Termo OR Descricao LIKE @Termo)
            ORDER BY Nome";

        return await _connection.QueryAsync<Produto>(sql, new { Termo = termoLike });
    }

    /// <summary>
    /// Contar produtos por subcategoria
    /// </summary>
    public async Task<int> ContarPorSubcategoriaAsync(Guid subcategoriaId)
    {
        const string sql = @"
            SELECT COUNT(*) FROM Produtos
            WHERE SubcategoriaId = @SubcategoriaId AND Ativo = 1";

        return await _connection.QueryFirstAsync<int>(sql, new { SubcategoriaId = subcategoriaId });
    }

    /// <summary>
    /// Verificar se nome já existe
    /// </summary>
    public async Task<bool> NomeJaExisteAsync(string nome, Guid subcategoriaId, Guid? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return false;

        const string sql = @"
            SELECT COUNT(*) FROM Produtos
            WHERE Nome = @Nome AND SubcategoriaId = @SubcategoriaId AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { Nome = nome, SubcategoriaId = subcategoriaId, IdExcluir = idExcluir });
        return count > 0;
    }

    /// <summary>
    /// Ativar produto
    /// </summary>
    public async Task AtivarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Produtos
            SET Ativo = 1, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Desativar produto
    /// </summary>
    public async Task DesativarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Produtos
            SET Ativo = 0, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Obter produtos mais vendidos
    /// </summary>
    public async Task<IEnumerable<ProdutoMaisVendido>> ObterMaisVendidosAsync(Guid estabelecimentoId, int top = 10)
    {
        const string sql = @"
            SELECT TOP (@Top)
                p.Id,
                p.Nome,
                SUM(ip.Quantidade) AS TotalVendido,
                SUM(ip.PrecoTotal) AS Faturamento,
                AVG(ip.PrecoUnitario) AS PrecoMedio
            FROM ItemPedidos ip
            INNER JOIN Produtos p ON ip.ProdutoId = p.Id
            INNER JOIN Pedidos ped ON ip.PedidoId = ped.Id
            INNER JOIN Subcategorias sc ON p.SubcategoriaId = sc.Id
            INNER JOIN Categorias c ON sc.CategoriaId = c.Id
            WHERE c.EstabelecimentoId = @EstabelecimentoId AND ped.Status IN (1, 2, 3)
            GROUP BY p.Id, p.Nome
            ORDER BY TotalVendido DESC";

        return await _connection.QueryAsync<ProdutoMaisVendido>(sql, new { EstabelecimentoId = estabelecimentoId, Top = top });
    }

    /// <summary>
    /// Deletar produto com cascata
    /// </summary>
    public async Task DeletarComCascataAsync(Guid id)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            try
            {
                // Deletar adicionais
                await _connection.ExecuteAsync(
                    "DELETE FROM Adicionais WHERE ProdutoId = @Id",
                    new { Id = id },
                    transaction);

                // Deletar variantes
                await _connection.ExecuteAsync(
                    "DELETE FROM Variantes WHERE ProdutoId = @Id",
                    new { Id = id },
                    transaction);

                // Deletar produto
                await _connection.ExecuteAsync(
                    "DELETE FROM Produtos WHERE Id = @Id",
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
/// DTO para produtos mais vendidos
/// </summary>
public class ProdutoMaisVendido
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int TotalVendido { get; set; }
    public decimal Faturamento { get; set; }
    public decimal PrecoMedio { get; set; }
}
