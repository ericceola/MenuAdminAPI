namespace MenuAdminAPI.Domain.Entities;

public class ImagemProduto
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public string BlobOriginal { get; set; } = string.Empty;
    public string BlobThumb { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public virtual Produto? Produto { get; set; }
}
