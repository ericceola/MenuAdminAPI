namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Ligação entre uma variante de produto e um valor de atributo
/// Exemplo: Variante "Café Grande" -> AtributoValor "Grande" (do atributo "Tamanho")
/// </summary>
public class ProdutoVarianteValor
{
    public Guid Id { get; set; }
    public Guid ProdutoVarianteId { get; set; }
    public Guid AtributoProdutoValorId { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public ProdutoVariante? Variante { get; set; }
    public AtributoProdutoValor? AtributoValor { get; set; }
}
