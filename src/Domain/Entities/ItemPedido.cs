namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Item de Pedido
/// </summary>
public class ItemPedido
{
    public Guid Id { get; set; }
    public Guid PedidoId { get; set; }
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
    public string? Observacoes { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public Pedido? Pedido { get; set; }
    public Produto? Produto { get; set; }
    public ICollection<AdicionalPedido> Adicionais { get; set; } = new List<AdicionalPedido>();
}
