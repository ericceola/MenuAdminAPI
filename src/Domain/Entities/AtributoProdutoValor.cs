namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Valor de um atributo de produto (ex: Pequeno, Médio, Grande para o atributo Tamanho)
/// </summary>
public class AtributoProdutoValor
{
    public Guid Id { get; set; }
    public Guid AtributoProdutoId { get; set; }
    public string Valor { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public AtributoProduto? Atributo { get; set; }
    public ICollection<ProdutoVarianteValor> VariantesValores { get; set; } = new List<ProdutoVarianteValor>();
}
