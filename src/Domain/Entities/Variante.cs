namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Variante de Produto (Tamanho, Cor, etc)
/// </summary>
public class Variante
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoAdicional { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public Produto? Produto { get; set; }
}
