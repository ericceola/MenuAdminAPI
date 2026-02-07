namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Cliente
/// </summary>
public class Cliente
{
    public Guid Id { get; set; }
    public Guid EstabelecimentoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string? CPF { get; set; }
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    public Estabelecimento? Estabelecimento { get; set; }
    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
