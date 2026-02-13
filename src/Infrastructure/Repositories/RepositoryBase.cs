using System.Data;
using Dapper;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Classe base para todos os repositórios com Dapper
/// </summary>
/// <typeparam name="T">Tipo da entidade</typeparam>
public abstract class RepositoryBase<T> where T : class
{
    protected readonly IDbConnection _connection;
    protected readonly string _tableName;
    protected IDbTransaction? _transaction;

    protected RepositoryBase(IDbConnection connection, string tableName)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
    }

    /// <summary>
    /// Definir transação para operações
    /// </summary>
    public void SetTransaction(IDbTransaction? transaction)
    {
        _transaction = transaction;
    }

    /// <summary>
    /// Obter entidade por ID
    /// </summary>
    public virtual async Task<T?> ObterPorIdAsync(Guid id)
    {
        const string sql = "SELECT * FROM {0} WHERE Id = @Id";
        var query = string.Format(sql, _tableName);
        
        return await _connection.QueryFirstOrDefaultAsync<T>(query, new { Id = id }, transaction: _transaction);
    }

    /// <summary>
    /// Obter todas as entidades
    /// </summary>
    public virtual async Task<IEnumerable<T>> ObterTodosAsync()
    {
        var sql = $"SELECT * FROM {_tableName}";
        return await _connection.QueryAsync<T>(sql, transaction: _transaction);
    }

    /// <summary>
    /// Adicionar nova entidade
    /// </summary>
    public virtual async Task AdicionarAsync(T entidade)
    {
        if (entidade == null)
            throw new ArgumentNullException(nameof(entidade));

        var propriedades = typeof(T).GetProperties()
            .Where(p => p.CanRead && p.GetValue(entidade) != null)
            .ToList();

        var colunas = string.Join(", ", propriedades.Select(p => p.Name));
        var parametros = string.Join(", ", propriedades.Select(p => $"@{p.Name}"));
        
        var sql = $"INSERT INTO {_tableName} ({colunas}) VALUES ({parametros})";

        var parametrosDict = propriedades.ToDictionary(p => p.Name, p => p.GetValue(entidade));
        
        await _connection.ExecuteAsync(sql, parametrosDict, transaction: _transaction);
    }

    /// <summary>
    /// Atualizar entidade existente
    /// </summary>
    public virtual async Task AtualizarAsync(T entidade)
    {
        if (entidade == null)
            throw new ArgumentNullException(nameof(entidade));

        var propriedades = typeof(T).GetProperties()
            .Where(p => p.CanRead && p.Name != "Id")
            .ToList();

        var atualizacoes = string.Join(", ", propriedades.Select(p => $"{p.Name} = @{p.Name}"));
        var sql = $"UPDATE {_tableName} SET {atualizacoes} WHERE Id = @Id";

        var parametrosDict = propriedades.ToDictionary(p => p.Name, p => p.GetValue(entidade));
        parametrosDict["Id"] = typeof(T).GetProperty("Id")?.GetValue(entidade);

        await _connection.ExecuteAsync(sql, parametrosDict, transaction: _transaction);
    }

    /// <summary>
    /// Remover entidade por ID
    /// </summary>
    public virtual async Task RemoverAsync(Guid id)
    {
        var sql = $"DELETE FROM {_tableName} WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }

    /// <summary>
    /// Contar total de registros
    /// </summary>
    public virtual async Task<int> ContarAsync()
    {
        var sql = $"SELECT COUNT(*) FROM {_tableName}";
        return await _connection.QueryFirstAsync<int>(sql, transaction: _transaction);
    }

    /// <summary>
    /// Verificar se existe registro com ID
    /// </summary>
    public virtual async Task<bool> ExisteAsync(Guid id)
    {
        var sql = $"SELECT COUNT(*) FROM {_tableName} WHERE Id = @Id";
        var count = await _connection.QueryFirstAsync<int>(sql, new { Id = id }, transaction: _transaction);
        return count > 0;
    }

    /// <summary>
    /// Obter com paginação
    /// </summary>
    public virtual async Task<(IEnumerable<T> Itens, int Total)> ObterComPaginacaoAsync(int pagina, int tamanho)
    {
        if (pagina < 1) pagina = 1;
        if (tamanho < 1) tamanho = 10;

        var offset = (pagina - 1) * tamanho;

        var sqlTotal = $"SELECT COUNT(*) FROM {_tableName}";
        var total = await _connection.QueryFirstAsync<int>(sqlTotal, transaction: _transaction);

        var sql = $"SELECT * FROM {_tableName} ORDER BY Id OFFSET {offset} ROWS FETCH NEXT {tamanho} ROWS ONLY";
        var itens = await _connection.QueryAsync<T>(sql, transaction: _transaction);

        return (itens, total);
    }
}
