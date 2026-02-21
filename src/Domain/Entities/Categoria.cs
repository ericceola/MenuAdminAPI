using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Categoria de Produtos
/// </summary>
public class Categoria
{
    public Guid Id { get; set; }
    public Guid EstabelecimentoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Emoji { get; set; } = "📦";
    public string Descricao { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    [NotMapped]
    public Estabelecimento? Estabelecimento { get; set; }
    
    [NotMapped]
    public ICollection<Subcategoria> Subcategorias { get; set; } = new List<Subcategoria>();
}
