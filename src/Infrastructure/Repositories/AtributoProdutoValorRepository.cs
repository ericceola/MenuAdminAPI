using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de AtributosProdutoValores com Dapper
/// </summary>
public class AtributoProdutoValorRepository : RepositoryBase<AtributoProdutoValor>, IAtributoProdutoValorRepository
{
    public AtributoProdutoValorRepository(IDbConnection connection)
        : base(connection, "AtributosProdutoValores")
    {
    }

    public async Task<IEnumerable<AtributoProdutoValor>> ObterPorAtributoAsync(Guid atributoProdutoId)
    {
        const string sql = @"
            SELECT Id, AtributoProdutoId, Valor, Ativo, DataCriacao, DataAtualizacao
            FROM AtributosProdutoValores
            WHERE AtributoProdutoId = @AtributoProdutoId
            ORDER BY Valor";
        return await _connection.QueryAsync<AtributoProdutoValor>(sql, new { AtributoProdutoId = atributoProdutoId }, transaction: _transaction);
    }

    public async Task<IEnumerable<AtributoProdutoValor>> ObterAtivosPorAtributoAsync(Guid atributoProdutoId)
    {
        const string sql = @"
            SELECT Id, AtributoProdutoId, Valor, Ativo, DataCriacao, DataAtualizacao
            FROM AtributosProdutoValores
            WHERE AtributoProdutoId = @AtributoProdutoId AND Ativo = 1
            ORDER BY Valor";
        return await _connection.QueryAsync<AtributoProdutoValor>(sql, new { AtributoProdutoId = atributoProdutoId }, transaction: _transaction);
    }

    public async Task<bool> ValorJaExisteAsync(string valor, Guid atributoProdutoId, Guid? idExcluir = null)
    {
        const string sql = @"
            SELECT COUNT(*) FROM AtributosProdutoValores
            WHERE Valor = @Valor AND AtributoProdutoId = @AtributoProdutoId
            AND (@IdExcluir IS NULL OR Id != @IdExcluir)";
        var count = await _connection.QueryFirstAsync<int>(sql,
            new { Valor = valor, AtributoProdutoId = atributoProdutoId, IdExcluir = idExcluir },
            transaction: _transaction);
        return count > 0;
    }

    public async Task DeletarAsync(Guid id)
    {
        await RemoverAsync(id);
    }
}
