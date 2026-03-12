namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Variante de produto (ex: Café Pequeno, Café Grande)
/// </summary>
public class ProdutoVariante
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? SKU { get; set; }
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public string? ImagemBlobName { get; set; }
    public int Ordem { get; set; } = 0;
    public string Status { get; set; } = "ativo";
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataExclusao { get; set; }

    // Relacionamentos
    public Produto? Produto { get; set; }
    public ICollection<ProdutoVarianteValor> Valores { get; set; } = new List<ProdutoVarianteValor>();
}
