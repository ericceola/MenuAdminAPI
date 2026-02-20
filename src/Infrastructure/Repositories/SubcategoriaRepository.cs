using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de Subcategorias com Dapper
/// </summary>
public class SubcategoriaRepository : RepositoryBase<Subcategoria>, ISubcategoriaRepository
{
    public SubcategoriaRepository(IDbConnection connection)
        : base(connection, "Subcategorias")
    {
    }

    /// <summary>
    /// Obter subcategorias por categoria
    /// </summary>
    public async Task<IEnumerable<Subcategoria>> ObterPorCategoriaAsync(Guid categoriaId)
    {
        const string sql = @"
            SELECT Id, CategoriaId, Nome, Descricao, Ordem, Ativo, DataCriacao, DataAtualizacao
            FROM Subcategorias
            WHERE CategoriaId = @CategoriaId
            ORDER BY Ordem, Nome";

        return await _connection.QueryAsync<Subcategoria>(sql, new { CategoriaId = categoriaId });
    }

    /// <summary>
    /// Obter subcategorias ativas por categoria
    /// </summary>
    public async Task<IEnumerable<Subcategoria>> ObterAtivasPorCategoriaAsync(Guid categoriaId)
    {
        const string sql = @"
            SELECT Id, CategoriaId, Nome, Descricao, Ordem, Ativo, DataCriacao, DataAtualizacao
            FROM Subcategorias
            WHERE CategoriaId = @CategoriaId AND Ativo = 1
            ORDER BY Ordem, Nome";

        return await _connection.QueryAsync<Subcategoria>(sql, new { CategoriaId = categoriaId });
    }

    /// <summary>
    /// Obter subcategoria por nome
    /// </summary>
    public async Task<Subcategoria?> ObterPorNomeAsync(string nome, Guid categoriaId)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return null;

        const string sql = @"
            SELECT Id, CategoriaId, Nome, Descricao, Ordem, Ativo, DataCriacao, DataAtualizacao
            FROM Subcategorias
            WHERE Nome = @Nome AND CategoriaId = @CategoriaId";

        return await _connection.QueryFirstOrDefaultAsync<Subcategoria>(sql, new { Nome = nome, CategoriaId = categoriaId });
    }

    /// <summary>
    /// Contar subcategorias por categoria
    /// </summary>
    public async Task<int> ContarPorCategoriaAsync(Guid categoriaId)
    {
        const string sql = @"
            SELECT COUNT(*) FROM Subcategorias
            WHERE CategoriaId = @CategoriaId AND Ativo = 1";

        return await _connection.QueryFirstAsync<int>(sql, new { CategoriaId = categoriaId });
    }

    /// <summary>
    /// Verificar se nome já existe
    /// </summary>
    public async Task<bool> NomeJaExisteAsync(string nome, Guid categoriaId, Guid? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return false;

        const string sql = @"
            SELECT COUNT(*) FROM Subcategorias
            WHERE Nome = @Nome AND CategoriaId = @CategoriaId AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { Nome = nome, CategoriaId = categoriaId, IdExcluir = idExcluir });
        return count > 0;
    }

    /// <summary>
    /// Buscar subcategorias por termo (retorna todas se termo vazio)
    /// </summary>
    public async Task<IEnumerable<Subcategoria>> BuscarAsync(string termo)
    {
        // Se termo vazio, busca todas as subcategorias ativas
        var termoLike = string.IsNullOrWhiteSpace(termo) ? "%" : $"%{termo}%";

        const string sql = @"
            SELECT Id, CategoriaId, Nome, Descricao, Ordem, Ativo, DataCriacao, DataAtualizacao
            FROM Subcategorias
            WHERE Ativo = 1 AND (Nome LIKE @Termo OR Descricao LIKE @Termo)
            ORDER BY Ordem, Nome";

        return await _connection.QueryAsync<Subcategoria>(sql, new { Termo = termoLike });
    }

    /// <summary>
    /// Obter com paginação
    /// </summary>
    public async Task<(IEnumerable<Subcategoria> Itens, int Total)> ObterComPaginacaoAsync(Guid categoriaId, int pagina, int tamanho)
    {
        if (pagina < 1) pagina = 1;
        if (tamanho < 1) tamanho = 10;

        var offset = (pagina - 1) * tamanho;

        const string sqlTotal = @"
            SELECT COUNT(*) FROM Subcategorias
            WHERE CategoriaId = @CategoriaId AND Ativo = 1";

        var total = await _connection.QueryFirstAsync<int>(sqlTotal, new { CategoriaId = categoriaId });

        const string sql = @"
            SELECT Id, CategoriaId, Nome, Descricao, Ordem, Ativo, DataCriacao, DataAtualizacao
            FROM Subcategorias
            WHERE CategoriaId = @CategoriaId AND Ativo = 1
            ORDER BY Ordem, Nome
            OFFSET @Offset ROWS FETCH NEXT @Tamanho ROWS ONLY";

        var itens = await _connection.QueryAsync<Subcategoria>(sql, new { CategoriaId = categoriaId, Offset = offset, Tamanho = tamanho });

        return (itens, total);
    }

    /// <summary>
    /// Ativar subcategoria
    /// </summary>
    public async Task AtivarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Subcategorias
            SET Ativo = 1, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Desativar subcategoria
    /// </summary>
    public async Task DesativarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Subcategorias
            SET Ativo = 0, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Deletar subcategoria com cascata
    /// </summary>
    public async Task DeletarComCascataAsync(Guid id)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            try
            {
                // Obter todos os produtos
                const string sqlProdutos = "SELECT Id FROM Produtos WHERE SubcategoriaId = @SubcategoriaId";
                var produtos = await _connection.QueryAsync<Guid>(sqlProdutos, new { SubcategoriaId = id }, transaction);

                // Deletar adicionais e variantes de cada produto
                foreach (var produtoId in produtos)
                {
                    await _connection.ExecuteAsync(
                        "DELETE FROM Adicionais WHERE ProdutoId = @ProdutoId",
                        new { ProdutoId = produtoId },
                        transaction);

                    await _connection.ExecuteAsync(
                        "DELETE FROM Variantes WHERE ProdutoId = @ProdutoId",
                        new { ProdutoId = produtoId },
                        transaction);
                }

                // Deletar produtos
                await _connection.ExecuteAsync(
                    "DELETE FROM Produtos WHERE SubcategoriaId = @SubcategoriaId",
                    new { SubcategoriaId = id },
                    transaction);

                // Deletar subcategoria
                await _connection.ExecuteAsync(
                    "DELETE FROM Subcategorias WHERE Id = @Id",
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
