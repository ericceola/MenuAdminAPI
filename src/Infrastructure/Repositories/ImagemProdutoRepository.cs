using Dapper;
using MenuAdminAPI.Domain.Entities;
using MySql.Data.MySqlClient;

namespace MenuAdminAPI.Infrastructure.Repositories;

public interface IImagemProdutoRepository
{
    Task<ImagemProduto> CriarAsync(ImagemProduto imagem);
    Task<IEnumerable<ImagemProduto>> ObterPorProdutoAsync(Guid produtoId);
    Task<ImagemProduto?> ObterPorIdAsync(Guid id);
}

public class ImagemProdutoRepository : IImagemProdutoRepository
{
    private readonly string _connectionString;

    public ImagemProdutoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<ImagemProduto> CriarAsync(ImagemProduto imagem)
    {
        const string sql = @"
            INSERT INTO ImagensProduto (Id, ProdutoId, BlobOriginal, BlobThumb, ContentType, CreatedAt)
            VALUES (@Id, @ProdutoId, @BlobOriginal, @BlobThumb, @ContentType, @CreatedAt);
        ";

        using var connection = new MySqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, imagem);
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

        using var connection = new MySqlConnection(_connectionString);
        return await connection.QueryAsync<ImagemProduto>(sql, new { ProdutoId = produtoId });
    }

    public async Task<ImagemProduto?> ObterPorIdAsync(Guid id)
    {
        const string sql = @"
            SELECT Id, ProdutoId, BlobOriginal, BlobThumb, ContentType, CreatedAt
            FROM ImagensProduto
            WHERE Id = @Id;
        ";

        using var connection = new MySqlConnection(_connectionString);
        return await connection.QueryFirstOrDefaultAsync<ImagemProduto>(sql, new { Id = id });
    }
}
