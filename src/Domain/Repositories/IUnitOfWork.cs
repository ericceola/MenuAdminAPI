namespace MenuAdminAPI.Domain.Repositories;

/// <summary>
/// Interface do padrão Unit of Work para gerenciar transações
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IEstabelecimentoRepository Estabelecimentos { get; }
    IUsuarioRepository Usuarios { get; }
    ICategoriaRepository Categorias { get; }
    ISubcategoriaRepository Subcategorias { get; }
    IProdutoRepository Produtos { get; }
    IVarianteRepository Variantes { get; }
    IAdicionalRepository Adicionais { get; }
    IClienteRepository Clientes { get; }
    IEnderecoRepository Enderecos { get; }
    IPedidoRepository Pedidos { get; }

    // Variantes e Atributos de Produto
    IAtributoProdutoRepository AtributosProduto { get; }
    IAtributoProdutoValorRepository AtributosProdutoValores { get; }
    IProdutoVarianteRepository ProdutoVariantes { get; }
    IProdutoVarianteValorRepository ProdutoVariantesValores { get; }

    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    Task<bool> SaveChangesAsync();
    bool HasActiveTransaction { get; }
}
