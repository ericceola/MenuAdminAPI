namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Adicional de Item de Pedido
/// </summary>
public class AdicionalPedido
{
    public Guid Id { get; set; }
    public Guid ItemPedidoId { get; set; }
    public Guid AdicionalId { get; set; }
    public int Quantidade { get; set; }
    public decimal Preco { get; set; }
    public decimal Subtotal { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public ItemPedido? ItemPedido { get; set; }
    public Adicional? Adicional { get; set; }
}
