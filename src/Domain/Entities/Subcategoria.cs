namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Subcategoria de Produtos
/// </summary>
public class Subcategoria
{
    public Guid Id { get; set; }
    public Guid CategoriaId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int Ordem { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public Categoria? Categoria { get; set; }
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
