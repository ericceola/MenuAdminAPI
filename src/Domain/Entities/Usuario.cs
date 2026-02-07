namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Usuário do Sistema
/// </summary>
public class Usuario
{
    public Guid Id { get; set; }
    public Guid EstabelecimentoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Perfil { get; set; } = "Gerente"; // Admin, Gerente, Operador
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
    public DateTime? UltimoAcesso { get; set; }

    // Relacionamentos
    public Estabelecimento? Estabelecimento { get; set; }
}
