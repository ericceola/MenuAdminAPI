using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de AtributosProduto com Dapper
/// </summary>
public class AtributoProdutoRepository : RepositoryBase<AtributoProduto>, IAtributoProdutoRepository
{
    public AtributoProdutoRepository(IDbConnection connection)
        : base(connection, "AtributosProduto")
    {
    }

    public async Task<IEnumerable<AtributoProduto>> ObterAtivosAsync()
    {
        const string sql = @"
            SELECT Id, Nome, Ativo, DataCriacao, DataAtualizacao
            FROM AtributosProduto
            WHERE Ativo = 1
            ORDER BY Nome";
        return await _connection.QueryAsync<AtributoProduto>(sql, transaction: _transaction);
    }

    public async Task<AtributoProduto?> ObterComValoresAsync(Guid id)
    {
        const string sql = @"
            SELECT a.Id, a.Nome, a.Ativo, a.DataCriacao, a.DataAtualizacao,
                   v.Id, v.AtributoProdutoId, v.Valor, v.Ativo, v.DataCriacao, v.DataAtualizacao
            FROM AtributosProduto a
            LEFT JOIN AtributosProdutoValores v ON v.AtributoProdutoId = a.Id AND v.Ativo = 1
            WHERE a.Id = @Id";

        var atributoDict = new Dictionary<Guid, AtributoProduto>();

        await _connection.QueryAsync<AtributoProduto, AtributoProdutoValor, AtributoProduto>(
            sql,
            (atributo, valor) =>
            {
                if (!atributoDict.TryGetValue(atributo.Id, out var atributoEntry))
                {
                    atributoEntry = atributo;
                    atributoEntry.Valores = new List<AtributoProdutoValor>();
                    atributoDict[atributo.Id] = atributoEntry;
                }
                if (valor?.Id != Guid.Empty && valor != null)
                    ((List<AtributoProdutoValor>)atributoEntry.Valores).Add(valor);
                return atributoEntry;
            },
            new { Id = id },
            splitOn: "Id",
            transaction: _transaction
        );

        return atributoDict.Values.FirstOrDefault();
    }

    public async Task<IEnumerable<AtributoProduto>> ObterTodosComValoresAsync()
    {
        const string sql = @"
            SELECT a.Id, a.Nome, a.Ativo, a.DataCriacao, a.DataAtualizacao,
                   v.Id, v.AtributoProdutoId, v.Valor, v.Ativo, v.DataCriacao, v.DataAtualizacao
            FROM AtributosProduto a
            LEFT JOIN AtributosProdutoValores v ON v.AtributoProdutoId = a.Id AND v.Ativo = 1
            WHERE a.Ativo = 1
            ORDER BY a.Nome, v.Valor";

        var atributoDict = new Dictionary<Guid, AtributoProduto>();

        await _connection.QueryAsync<AtributoProduto, AtributoProdutoValor, AtributoProduto>(
            sql,
            (atributo, valor) =>
            {
                if (!atributoDict.TryGetValue(atributo.Id, out var atributoEntry))
                {
                    atributoEntry = atributo;
                    atributoEntry.Valores = new List<AtributoProdutoValor>();
                    atributoDict[atributo.Id] = atributoEntry;
                }
                if (valor?.Id != Guid.Empty && valor != null)
                    ((List<AtributoProdutoValor>)atributoEntry.Valores).Add(valor);
                return atributoEntry;
            },
            splitOn: "Id",
            transaction: _transaction
        );

        return atributoDict.Values;
    }

    public async Task<bool> NomeJaExisteAsync(string nome, Guid? idExcluir = null)
    {
        const string sql = @"
            SELECT COUNT(*) FROM AtributosProduto
            WHERE Nome = @Nome AND (@IdExcluir IS NULL OR Id != @IdExcluir)";
        var count = await _connection.QueryFirstAsync<int>(sql, new { Nome = nome, IdExcluir = idExcluir }, transaction: _transaction);
        return count > 0;
    }

    public async Task DeletarAsync(Guid id)
    {
        await RemoverAsync(id);
    }
}
