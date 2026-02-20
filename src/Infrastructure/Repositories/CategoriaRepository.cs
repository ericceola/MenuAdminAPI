using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de Categorias com Dapper
/// </summary>
public class CategoriaRepository : RepositoryBase<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(IDbConnection connection)
        : base(connection, "Categorias")
    {
    }

    /// <summary>
    /// Obter categorias por estabelecimento
    /// </summary>
    public async Task<IEnumerable<Categoria>> ObterPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT Id, EstabelecimentoId, Nome, Descricao, Ordem, Ativo, DataCriacao, DataAtualizacao
            FROM Categorias
            WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1
            ORDER BY Ordem, Nome";

        return await _connection.QueryAsync<Categoria>(sql, new { EstabelecimentoId = estabelecimentoId });
    }

    /// <summary>
    /// Obter categorias ativas por estabelecimento
    /// </summary>
    public async Task<IEnumerable<Categoria>> ObterAtivasPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT Id, EstabelecimentoId, Nome, Descricao, Ordem, Ativo, DataCriacao, DataAtualizacao
            FROM Categorias
            WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1
            ORDER BY Ordem, Nome";

        return await _connection.QueryAsync<Categoria>(sql, new { EstabelecimentoId = estabelecimentoId });
    }

    /// <summary>
    /// Buscar categorias por termo (retorna todas se termo vazio)
    /// </summary>
    public async Task<IEnumerable<Categoria>> BuscarAsync(string termo)
    {
        // Se termo vazio, busca todas as categorias ativas
        var termoLike = string.IsNullOrWhiteSpace(termo) ? "%" : $"%{termo}%";

        const string sql = @"
            SELECT Id, EstabelecimentoId, Nome, Descricao, Ordem, Ativo, DataCriacao, DataAtualizacao
            FROM Categorias
            WHERE Ativo = 1 AND (Nome LIKE @Termo OR Descricao LIKE @Termo)
            ORDER BY Ordem, Nome";

        return await _connection.QueryAsync<Categoria>(sql, new { Termo = termoLike });
    }

    /// <summary>
    /// Contar categorias por estabelecimento
    /// </summary>
    public async Task<int> ContarPorEstabelecimentoAsync(Guid estabelecimentoId)
    {
        const string sql = @"
            SELECT COUNT(*) FROM Categorias
            WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1";

        return await _connection.QueryFirstAsync<int>(sql, new { EstabelecimentoId = estabelecimentoId });
    }

    /// <summary>
    /// Verificar se nome já existe
    /// </summary>
    public async Task<bool> NomeJaExisteAsync(string nome, Guid estabelecimentoId, Guid? idExcluir = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return false;

        const string sql = @"
            SELECT COUNT(*) FROM Categorias
            WHERE Nome = @Nome AND EstabelecimentoId = @EstabelecimentoId AND (@IdExcluir IS NULL OR Id != @IdExcluir)";

        var count = await _connection.QueryFirstAsync<int>(sql, new { Nome = nome, EstabelecimentoId = estabelecimentoId, IdExcluir = idExcluir });
        return count > 0;
    }

    /// <summary>
    /// Ativar categoria
    /// </summary>
    public async Task AtivarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Categorias
            SET Ativo = 1, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Desativar categoria
    /// </summary>
    public async Task DesativarAsync(Guid id)
    {
        const string sql = @"
            UPDATE Categorias
            SET Ativo = 0, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";

        await _connection.ExecuteAsync(sql, new { Id = id });
    }

    /// <summary>
    /// Obter com paginação
    /// </summary>
    public async Task<(IEnumerable<Categoria> Itens, int Total)> ObterComPaginacaoAsync(Guid estabelecimentoId, int pagina, int tamanho)
    {
        if (pagina < 1) pagina = 1;
        if (tamanho < 1) tamanho = 10;

        var offset = (pagina - 1) * tamanho;

        const string sqlTotal = @"
            SELECT COUNT(*) FROM Categorias
            WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1";

        var total = await _connection.QueryFirstAsync<int>(sqlTotal, new { EstabelecimentoId = estabelecimentoId });

        const string sql = @"
            SELECT Id, EstabelecimentoId, Nome, Descricao, Ordem, Ativo, DataCriacao, DataAtualizacao
            FROM Categorias
            WHERE EstabelecimentoId = @EstabelecimentoId AND Ativo = 1
            ORDER BY Ordem, Nome
            OFFSET @Offset ROWS FETCH NEXT @Tamanho ROWS ONLY";

        var itens = await _connection.QueryAsync<Categoria>(sql, new { EstabelecimentoId = estabelecimentoId, Offset = offset, Tamanho = tamanho });

        return (itens, total);
    }

    /// <summary>
    /// Deletar categoria com cascata
    /// </summary>
    public async Task DeletarComCascataAsync(Guid id)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            try
            {
                // Obter todas as subcategorias
                const string sqlSubcategorias = "SELECT Id FROM Subcategorias WHERE CategoriaId = @CategoriaId";
                var subcategorias = await _connection.QueryAsync<Guid>(sqlSubcategorias, new { CategoriaId = id }, transaction);

                // Deletar produtos, variantes e adicionais de cada subcategoria
                foreach (var subcategoriaId in subcategorias)
                {
                    const string sqlProdutos = "SELECT Id FROM Produtos WHERE SubcategoriaId = @SubcategoriaId";
                    var produtos = await _connection.QueryAsync<Guid>(sqlProdutos, new { SubcategoriaId = subcategoriaId }, transaction);

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

                    await _connection.ExecuteAsync(
                        "DELETE FROM Produtos WHERE SubcategoriaId = @SubcategoriaId",
                        new { SubcategoriaId = subcategoriaId },
                        transaction);
                }

                // Deletar subcategorias
                await _connection.ExecuteAsync(
                    "DELETE FROM Subcategorias WHERE CategoriaId = @CategoriaId",
                    new { CategoriaId = id },
                    transaction);

                // Deletar categoria
                await _connection.ExecuteAsync(
                    "DELETE FROM Categorias WHERE Id = @Id",
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
