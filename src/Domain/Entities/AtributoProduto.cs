namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Atributo de produto (ex: Tamanho, Cor, Sabor)
/// </summary>
public class AtributoProduto
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public ICollection<AtributoProdutoValor> Valores { get; set; } = new List<AtributoProdutoValor>();
}
