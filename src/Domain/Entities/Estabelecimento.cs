using System.ComponentModel.DataAnnotations.Schema;

namespace MenuAdminAPI.Domain.Entities;

/// <summary>
/// Entidade de Estabelecimento (Restaurante, Pizzaria, etc)
/// </summary>
public class Estabelecimento
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    
    [NotMapped]
    public string Numero { get; set; } = string.Empty;
    
    [NotMapped]
    public string Complemento { get; set; } = string.Empty;
    
    [NotMapped]
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string CEP { get; set; } = string.Empty;
    public string RazaoSocial { get; set; } = string.Empty;
    public string NomeResponsavel { get; set; } = string.Empty;
    public string TelefoneResponsavel { get; set; } = string.Empty;
    public bool EhMatriz { get; set; } = false;
    public bool TemFiliais { get; set; } = false;
    public Guid? MatrizId { get; set; } // ID da matriz se for filial
    public string Plano { get; set; } = "Básico";
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;

    // Relacionamentos
    [NotMapped]
    public Estabelecimento? Matriz { get; set; } // Referência à matriz
    
    [NotMapped]
    public ICollection<Estabelecimento> Filiais { get; set; } = new List<Estabelecimento>(); // Filiais deste estabelecimento
    
    [NotMapped]
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    
    [NotMapped]
    public ICollection<Categoria> Categorias { get; set; } = new List<Categoria>();
    
    [NotMapped]
    public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    
    [NotMapped]
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
