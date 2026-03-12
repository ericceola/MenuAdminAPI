namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Produto do Cardápio
/// </summary>
public class Produto
{
    public Guid Id { get; set; }
    public Guid SubcategoriaId { get; set; }
    public Guid EstabelecimentoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
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
    public Subcategoria? Subcategoria { get; set; }
    public ICollection<Variante> Variantes { get; set; } = new List<Variante>();
    public ICollection<Adicional> Adicionais { get; set; } = new List<Adicional>();
    public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
}
