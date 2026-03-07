using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Infrastructure.Repositories;

public interface IImagemProdutoRepository
{
    Task<ImagemProduto> CriarAsync(ImagemProduto imagem);
    Task<IEnumerable<ImagemProduto>> ObterPorProdutoAsync(Guid produtoId);
    Task<ImagemProduto?> ObterPorIdAsync(Guid id);
}

public class ImagemProdutoRepository : IImagemProdutoRepository
{
    private readonly IDbConnection _connection;

    public ImagemProdutoRepository(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<ImagemProduto> CriarAsync(ImagemProduto imagem)
    {
        const string sql = @"
            INSERT INTO ImagensProduto (Id, ProdutoId, BlobOriginal, BlobThumb, ContentType, CreatedAt)
            VALUES (@Id, @ProdutoId, @BlobOriginal, @BlobThumb, @ContentType, @CreatedAt);
        ";

        await _connection.ExecuteAsync(sql, imagem);
        return imagem;
    }

    public async Task<IEnumerable<ImagemProduto>> ObterPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            SELECT Id, ProdutoId, BlobOriginal, BlobThumb, ContentType, CreatedAt
            FROM ImagensProduto
            WHERE ProdutoId = @ProdutoId
            ORDER BY CreatedAt DESC;
        ";

        return await _connection.QueryAsync<ImagemProduto>(sql, new { ProdutoId = produtoId });
    }

    public async Task<ImagemProduto?> ObterPorIdAsync(Guid id)
    {
        const string sql = @"
            SELECT Id, ProdutoId, BlobOriginal, BlobThumb, ContentType, CreatedAt
            FROM ImagensProduto
            WHERE Id = @Id;
        ";

        return await _connection.QueryFirstOrDefaultAsync<ImagemProduto>(sql, new { Id = id });
    }
}
