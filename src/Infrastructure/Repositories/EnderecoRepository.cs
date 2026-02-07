using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de Endereços com Dapper
/// </summary>
public class EnderecoRepository : RepositoryBase<Endereco>, IEnderecoRepository
{
    public EnderecoRepository(IDbConnection connection)
        : base(connection, "Enderecos")
    {
    }

    /// <summary>
    /// Obter endereços por cliente
    /// </summary>
    public async Task<IEnumerable<Endereco>> ObterPorClienteAsync(Guid clienteId)
    {
        const string sql = @"
            SELECT Id, ClienteId, Rua, Numero, Complemento, Bairro, Cidade, Estado, CEP, Padrao, Ativo, DataCriacao, DataAtualizacao
            FROM Enderecos
            WHERE ClienteId = @ClienteId AND Ativo = 1
            ORDER BY Padrao DESC, Rua";

        return await _connection.QueryAsync<Endereco>(sql, new { ClienteId = clienteId });
    }

    /// <summary>
    /// Obter endereço padrão do cliente
    /// </summary>
    public async Task<Endereco?> ObterPadraoAsync(Guid clienteId)
    {
        const string sql = @"
            SELECT Id, ClienteId, Rua, Numero, Complemento, Bairro, Cidade, Estado, CEP, Padrao, Ativo, DataCriacao, DataAtualizacao
            FROM Enderecos
            WHERE ClienteId = @ClienteId AND Padrao = 1 AND Ativo = 1";

        return await _connection.QueryFirstOrDefaultAsync<Endereco>(sql, new { ClienteId = clienteId });
    }

    /// <summary>
    /// Contar endereços por cliente
    /// </summary>
    public async Task<int> ContarPorClienteAsync(Guid clienteId)
    {
        const string sql = @"
            SELECT COUNT(*) FROM Enderecos
            WHERE ClienteId = @ClienteId AND Ativo = 1";

        return await _connection.QueryFirstAsync<int>(sql, new { ClienteId = clienteId });
    }

    /// <summary>
    /// Verificar se endereço já existe
    /// </summary>
    public async Task<bool> EnderecoJaExisteAsync(Guid clienteId, string rua, string numero, string bairro, Guid? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(rua) || string.IsNullOrWhiteSpace(numero) || string.IsNullOrWhiteSpace(bairro))
            return false;

        const string sql = @"
            SELECT COUNT(*) FROM Enderecos
            WHERE ClienteId = @ClienteId AND Rua = @Rua AND Numero = @Numero AND Bairro = @Bairro 
            AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { ClienteId = clienteId, Rua = rua, Numero = numero, Bairro = bairro, IdExcluir = idExcluir });
        return count > 0;
    }

    /// <summary>
    /// Buscar endereços por termo
    /// </summary>
    public async Task<IEnumerable<Endereco>> BuscarAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return Enumerable.Empty<Endereco>();

        const string sql = @"
            SELECT Id, ClienteId, Rua, Numero, Complemento, Bairro, Cidade, Estado, CEP, Padrao, Ativo, DataCriacao, DataAtualizacao
            FROM Enderecos
            WHERE Ativo = 1 AND (Rua LIKE @Termo OR Numero LIKE @Termo OR Bairro LIKE @Termo OR Cidade LIKE @Termo OR CEP LIKE @Termo)
            ORDER BY Rua";

        var termoLike = $"%{termo}%";
        return await _connection.QueryAsync<Endereco>(sql, new { Termo = termoLike });
    }

    /// <summary>
    /// Obter endereços por cidade
    /// </summary>
    public async Task<IEnumerable<Endereco>> ObterPorCidadeAsync(Guid clienteId, string cidade)
    {
        if (string.IsNullOrWhiteSpace(cidade))
            return Enumerable.Empty<Endereco>();

        const string sql = @"
            SELECT Id, ClienteId, Rua, Numero, Complemento, Bairro, Cidade, Estado, CEP, Padrao, Ativo, DataCriacao, DataAtualizacao
            FROM Enderecos
            WHERE ClienteId = @ClienteId AND Cidade = @Cidade AND Ativo = 1
            ORDER BY Rua";

        return await _connection.QueryAsync<Endereco>(sql, new { ClienteId = clienteId, Cidade = cidade });
    }

    /// <summary>
    /// Obter endereços por bairro
    /// </summary>
    public async Task<IEnumerable<Endereco>> ObterPorBairroAsync(Guid clienteId, string bairro)
    {
        if (string.IsNullOrWhiteSpace(bairro))
            return Enumerable.Empty<Endereco>();

        const string sql = @"
            SELECT Id, ClienteId, Rua, Numero, Complemento, Bairro, Cidade, Estado, CEP, Padrao, Ativo, DataCriacao, DataAtualizacao
            FROM Enderecos
            WHERE ClienteId = @ClienteId AND Bairro = @Bairro AND Ativo = 1
            ORDER BY Rua";

        return await _connection.QueryAsync<Endereco>(sql, new { ClienteId = clienteId, Bairro = bairro });
    }

    /// <summary>
    /// Obter cidades únicas do cliente
    /// </summary>
    public async Task<IEnumerable<string>> ObterCidadesUnicasAsync(Guid clienteId)
    {
        const string sql = @"
            SELECT DISTINCT Cidade
            FROM Enderecos
            WHERE ClienteId = @ClienteId AND Ativo = 1
            ORDER BY Cidade";

        return await _connection.QueryAsync<string>(sql, new { ClienteId = clienteId });
    }

    /// <summary>
    /// Obter bairros únicos do cliente
    /// </summary>
    public async Task<IEnumerable<string>> ObterBairrosUnicosAsync(Guid clienteId)
    {
        const string sql = @"
            SELECT DISTINCT Bairro
            FROM Enderecos
            WHERE ClienteId = @ClienteId AND Ativo = 1
            ORDER BY Bairro";

        return await _connection.QueryAsync<string>(sql, new { ClienteId = clienteId });
    }

    /// <summary>
    /// Definir endereço como padrão
    /// </summary>
    public async Task DefinirComoPadraoAsync(Guid clienteId, Guid enderecoId)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            try
            {
                // Remover padrão dos outros endereços
                const string sqlRemover = @"
                    UPDATE Enderecos
                    SET Padrao = 0, DataAtualizacao = GETUTCDATE()
                    WHERE ClienteId = @ClienteId AND Padrao = 1";
                await _connection.ExecuteAsync(sqlRemover, new { ClienteId = clienteId }, transaction);

                // Definir como padrão
                const string sqlDefinir = @"
                    UPDATE Enderecos
                    SET Padrao = 1, DataAtualizacao = GETUTCDATE()
                    WHERE Id = @Id";
                await _connection.ExecuteAsync(sqlDefinir, new { Id = enderecoId }, transaction);

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
    /// Ativar endereço
    /// </summary>
    public async Task AtivarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Enderecos
            SET Ativo = 1, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Desativar endereço
    /// </summary>
    public async Task DesativarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Enderecos
            SET Ativo = 0, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }
}
