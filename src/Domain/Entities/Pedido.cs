namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Pedido
/// </summary>
public class Pedido
{
    public Guid Id { get; set; }
    public Guid EstabelecimentoId { get; set; }
    public Guid ClienteId { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendente"; // Pendente, Confirmado, Preparando, Pronto, Entregue, Cancelado
    public decimal Total { get; set; }
    public string? Observacoes { get; set; }
    public string? MotivoCancelamento { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataEntrega { get; set; }

    // Relacionamentos
    public Estabelecimento? Estabelecimento { get; set; }
    public Cliente? Cliente { get; set; }
    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
}
