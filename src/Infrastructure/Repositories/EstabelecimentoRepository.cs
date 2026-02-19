using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de Estabelecimentos com Dapper
/// </summary>
public class EstabelecimentoRepository : RepositoryBase<Estabelecimento>, IEstabelecimentoRepository
{
    public EstabelecimentoRepository(IDbConnection connection)
        : base(connection, "Estabelecimentos")
    {
    }

    /// <summary>
    /// Obter estabelecimentos ativos
    /// </summary>
    public async Task<IEnumerable<Estabelecimento>> ObterAtivosAsync()
    {
        const string sql = @"
            SELECT Id, Nome, Email, Telefone, CNPJ, Endereco, Cidade, Estado, CEP, Plano, Ativo, DataCriacao, DataAtualizacao
            FROM Estabelecimentos
            WHERE Ativo = 1
            ORDER BY Nome";

        return await _connection.QueryAsync<Estabelecimento>(sql, transaction: _transaction);
    }

    /// <summary>
    /// Obter estabelecimentos por plano
    /// </summary>
    public async Task<IEnumerable<Estabelecimento>> ObterPorPlanoAsync(int plano)
    {
        const string sql = @"
            SELECT Id, Nome, Email, Telefone, CNPJ, Endereco, Cidade, Estado, CEP, Plano, Ativo, DataCriacao, DataAtualizacao
            FROM Estabelecimentos
            WHERE Plano = @Plano AND Ativo = 1
            ORDER BY Nome";

        return await _connection.QueryAsync<Estabelecimento>(sql, new { Plano = plano }, transaction: _transaction);
    }

    /// <summary>
    /// Buscar estabelecimentos por termo
    /// </summary>
    public async Task<IEnumerable<Estabelecimento>> BuscarAsync(string termo)
    {
        if (string.IsNullOrWhiteSpace(termo))
            return await ObterAtivosAsync();

        const string sql = @"
            SELECT Id, Nome, Email, Telefone, CNPJ, Endereco, Cidade, Estado, CEP, Plano, Ativo, DataCriacao, DataAtualizacao
            FROM Estabelecimentos
            WHERE Ativo = 1 AND (Nome LIKE @Termo OR Email LIKE @Termo OR CNPJ LIKE @Termo)
            ORDER BY Nome";

        var termoLike = $"%{termo}%";
        return await _connection.QueryAsync<Estabelecimento>(sql, new { Termo = termoLike }, transaction: _transaction);
    }

    /// <summary>
    /// Verificar se email já existe
    /// </summary>
    public async Task<bool> EmailJaExisteAsync(string email, Guid? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        const string sql = @"
            SELECT COUNT(*) FROM Estabelecimentos
            WHERE Email = @Email AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { Email = email, IdExcluir = idExcluir }, transaction: _transaction);
        return count > 0;
    }

    /// <summary>
    /// Verificar se CNPJ já existe
    /// </summary>
    public async Task<bool> CnpjJaExisteAsync(string cnpj, Guid? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            return false;

        const string sql = @"
            SELECT COUNT(*) FROM Estabelecimentos
            WHERE CNPJ = @Cnpj AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { Cnpj = cnpj, IdExcluir = idExcluir }, transaction: _transaction);
        return count > 0;
    }

    /// <summary>
    /// Obter estatísticas do estabelecimento
    /// </summary>
    public async Task<EstabelecimentoEstatisticas> ObterEstatisticasAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT
                (SELECT COUNT(*) FROM Usuarios WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1) AS TotalUsuarios,
                (SELECT COUNT(*) FROM Produtos p 
                    INNER JOIN Subcategorias sc ON p.SubcategoriaId = sc.Id
                    INNER JOIN Categorias c ON sc.CategoriaId = c.Id
                    WHERE c.EstabelecimentoId = @EstabelecimentoId AND p.Ativo = 1) AS TotalProdutos,
                (SELECT COUNT(*) FROM Clientes WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1) AS TotalClientes,
                (SELECT COUNT(*) FROM Pedidos WHERE EstabelecimentoId = @EstabelecimentoId) AS TotalPedidos,
                (SELECT ISNULL(SUM(ValorFinal), 0) FROM Pedidos WHERE EstabelecimentoId = @EstabelecimentoId AND Status IN (1, 2, 3)) AS ReceitaTotal";

        return await _connection.QueryFirstAsync<EstabelecimentoEstatisticas>(sql, new { EstabelecimentoId = estabelecimentoId }, transaction: _transaction);
    }

    /// <summary>
    /// Ativar estabelecimento
    /// </summary>
    public async Task AtivarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Estabelecimentos
            SET Ativo = 1, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }

    /// <summary>
    /// Desativar estabelecimento
    /// </summary>
    public async Task DesativarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Estabelecimentos
            SET Ativo = 0, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }

    /// <summary>
    /// Contar estabelecimentos ativos
    /// </summary>
    public async Task<int> ContarAtivosAsync()
    {
        const string sql = "SELECT COUNT(*) FROM Estabelecimentos WHERE Ativo = 1";
        return await _connection.QueryFirstAsync<int>(sql, transaction: _transaction);
    }

    /// <summary>
    /// Obter estabelecimentos com hierarquia de filiais
    /// </summary>
    public async Task<IEnumerable<Estabelecimento>> ObterComHierarquiaAsync()
    {
        // Obter todos os estabelecimentos que não são filiais (matrizes e independentes)
        const string sqlPrincipais = @"
            SELECT Id, Nome, Email, Telefone, CNPJ, Endereco, Numero, Complemento, Bairro, Cidade, Estado, CEP, 
                   RazaoSocial, NomeResponsavel, TelefoneResponsavel, Plano, EhMatriz, TemFiliais, MatrizId, Ativo, DataCriacao, DataAtualizacao
            FROM Estabelecimentos
            WHERE MatrizId IS NULL
            ORDER BY Nome";

        var estabelecimentosPrincipais = (await _connection.QueryAsync<Estabelecimento>(sqlPrincipais, transaction: _transaction)).ToList();

        // Para cada estabelecimento que é matriz, obter suas filiais
        const string sqlFiliais = @"
            SELECT Id, Nome, Email, Telefone, CNPJ, Endereco, Numero, Complemento, Bairro, Cidade, Estado, CEP, 
                   RazaoSocial, NomeResponsavel, TelefoneResponsavel, Plano, EhMatriz, TemFiliais, MatrizId, Ativo, DataCriacao, DataAtualizacao
            FROM Estabelecimentos
            WHERE MatrizId = @MatrizId AND Ativo = 1
            ORDER BY Nome";

        foreach (var estabelecimento in estabelecimentosPrincipais)
        {
            // Buscar filiais para todos os estabelecimentos
            var filiais = (await _connection.QueryAsync<Estabelecimento>(sqlFiliais, new { MatrizId = estabelecimento.Id }, transaction: _transaction)).ToList();
            if (filiais.Any())
            {
                estabelecimento.Filiais = filiais;
            }
        }

        return estabelecimentosPrincipais;
    }
}

/// <summary>
/// DTO para estatísticas do estabelecimento
/// </summary>
public class EstabelecimentoEstatisticas
{
    public int TotalUsuarios { get; set; }
    public int TotalProdutos { get; set; }
    public int TotalClientes { get; set; }
    public int TotalPedidos { get; set; }
    public decimal ReceitaTotal { get; set; }
}
