using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de ProdutoVariantesValores com Dapper
/// </summary>
public class ProdutoVarianteValorRepository : RepositoryBase<ProdutoVarianteValor>, IProdutoVarianteValorRepository
{
    public ProdutoVarianteValorRepository(IDbConnection connection)
        : base(connection, "ProdutoVariantesValores")
    {
    }

    public async Task<IEnumerable<ProdutoVarianteValor>> ObterPorVarianteAsync(Guid produtoVarianteId)
    {
        const string sql = @"
            SELECT pvv.Id, pvv.ProdutoVarianteId, pvv.AtributoProdutoValorId, pvv.DataCriacao
            FROM ProdutoVariantesValores pvv
            WHERE pvv.ProdutoVarianteId = @ProdutoVarianteId";
        return await _connection.QueryAsync<ProdutoVarianteValor>(sql,
            new { ProdutoVarianteId = produtoVarianteId },
            transaction: _transaction);
    }

    public async Task DeletarPorVarianteAsync(Guid produtoVarianteId)
    {
        const string sql = "DELETE FROM ProdutoVariantesValores WHERE ProdutoVarianteId = @ProdutoVarianteId";
        await _connection.ExecuteAsync(sql, new { ProdutoVarianteId = produtoVarianteId }, transaction: _transaction);
    }

    public override async Task DeletarAsync(Guid id)
    {
        const string sql = "DELETE FROM ProdutoVariantesValores WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }
}
