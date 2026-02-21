using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Produto do Cardápio
/// </summary>
public class Produto
{
    public Guid Id { get; set; }
    public Guid SubcategoriaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    [NotMapped]
    public Subcategoria? Subcategoria { get; set; }
    
    [NotMapped]
    public ICollection<Variante> Variantes { get; set; } = new List<Variante>();
    
    [NotMapped]
    public ICollection<Adicional> Adicionais { get; set; } = new List<Adicional>();
    
    [NotMapped]
    public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
}
