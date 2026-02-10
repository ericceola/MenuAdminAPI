namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Adicional/Extra de Produto
/// </summary>
public class Adicional
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public Produto? Produto { get; set; }
    public ICollection<AdicionalPedido> AdicionalPedidos { get; set; } = new List<AdicionalPedido>();
}
