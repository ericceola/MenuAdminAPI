using System.Data;
using Microsoft.Data.SqlClient;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;
using MenuAdminAPI.Infrastructure.Repositories;

namespace MenuAdminAPI.Infrastructure.Data;

/// <summary>
/// Implementação do padrão Unit of Work para gerenciar transações e repositórios
/// </summary>
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly string _connectionString;
    private SqlConnection? _connection;
    private SqlTransaction? _transaction;
    private bool _disposed;

    // Repositórios lazy-loaded
    private IEstabelecimentoRepository? _estabelecimentos;
    private IProdutoRepository? _produtos;
    private ICategoriaRepository? _categorias;
    private ISubcategoriaRepository? _subcategorias;
    private IClienteRepository? _clientes;
    private IPedidoRepository? _pedidos;
    private IUsuarioRepository? _usuarios;
    private IVarianteRepository? _variantes;
    private IAdicionalRepository? _adicionais;
    private IEnderecoRepository? _enderecos;
    private IAtributoProdutoRepository? _atributosProduto;
    private IAtributoProdutoValorRepository? _atributosProdutoValores;
    private IProdutoVarianteRepository? _produtoVariantes;
    private IProdutoVarianteValorRepository? _produtoVariantesValores;

    public UnitOfWork(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    /// <summary>
    /// Repositório de Estabelecimentos
    /// </summary>
    public IEstabelecimentoRepository Estabelecimentos
    {
        get
        {
            if (_estabelecimentos == null)
                _estabelecimentos = new EstabelecimentoRepository(GetConnection());
            
            // Sempre propagar transação ao acessar repositório
            if (_estabelecimentos is RepositoryBase<Estabelecimento> repo)
                repo.SetTransaction(_transaction);
            
            return _estabelecimentos;
        }
    }

    /// <summary>
    /// Repositório de Produtos
    /// </summary>
    public IProdutoRepository Produtos
    {
        get
        {
            if (_produtos == null)
                _produtos = new ProdutoRepository(GetConnection());
            
            if (_produtos is RepositoryBase<Produto> repo)
                repo.SetTransaction(_transaction);
            
            return _produtos;
        }
    }

    /// <summary>
    /// Repositório de Categorias
    /// </summary>
    public ICategoriaRepository Categorias
    {
        get
        {
            if (_categorias == null)
                _categorias = new CategoriaRepository(GetConnection());
            
            if (_categorias is RepositoryBase<Categoria> repo)
                repo.SetTransaction(_transaction);
            
            return _categorias;
        }
    }

    /// <summary>
    /// Repositório de Subcategorias
    /// </summary>
    public ISubcategoriaRepository Subcategorias
    {
        get
        {
            if (_subcategorias == null)
                _subcategorias = new SubcategoriaRepository(GetConnection());
            
            if (_subcategorias is RepositoryBase<Subcategoria> repo)
                repo.SetTransaction(_transaction);
            
            return _subcategorias;
        }
    }

    /// <summary>
    /// Repositório de Clientes
    /// </summary>
    public IClienteRepository Clientes
    {
        get
        {
            if (_clientes == null)
                _clientes = new ClienteRepository(GetConnection());
            
            if (_clientes is RepositoryBase<Cliente> repo)
                repo.SetTransaction(_transaction);
            
            return _clientes;
        }
    }

    /// <summary>
    /// Repositório de Pedidos
    /// </summary>
    public IPedidoRepository Pedidos
    {
        get
        {
            if (_pedidos == null)
                _pedidos = new PedidoRepository(GetConnection());
            
            if (_pedidos is RepositoryBase<Pedido> repo)
                repo.SetTransaction(_transaction);
            
            return _pedidos;
        }
    }

    /// <summary>
    /// Repositório de Usuários
    /// </summary>
    public IUsuarioRepository Usuarios
    {
        get
        {
            if (_usuarios == null)
                _usuarios = new UsuarioRepository(GetConnection());
            
            if (_usuarios is RepositoryBase<Usuario> repo)
                repo.SetTransaction(_transaction);
            
            return _usuarios;
        }
    }

    /// <summary>
    /// Repositório de Variantes
    /// </summary>
    public IVarianteRepository Variantes
    {
        get
        {
            if (_variantes == null)
                _variantes = new VarianteRepository(GetConnection());
            
            if (_variantes is RepositoryBase<Variante> repo)
                repo.SetTransaction(_transaction);
            
            return _variantes;
        }
    }

    /// <summary>
    /// Repositório de Adicionais
    /// </summary>
    public IAdicionalRepository Adicionais
    {
        get
        {
            if (_adicionais == null)
                _adicionais = new AdicionalRepository(GetConnection());
            
            if (_adicionais is RepositoryBase<Adicional> repo)
                repo.SetTransaction(_transaction);
            
            return _adicionais;
        }
    }

    /// <summary>
    /// Repositório de Endereços
    /// </summary>
    public IEnderecoRepository Enderecos
    {
        get
        {
            if (_enderecos == null)
                _enderecos = new EnderecoRepository(GetConnection());
            
            if (_enderecos is RepositoryBase<Endereco> repo)
                repo.SetTransaction(_transaction);
            
            return _enderecos;
        }
    }

    /// <summary>
    /// Repositório de Atributos de Produto
    /// </summary>
    public IAtributoProdutoRepository AtributosProduto
    {
        get
        {
            if (_atributosProduto == null)
                _atributosProduto = new AtributoProdutoRepository(GetConnection());
            
            if (_atributosProduto is RepositoryBase<AtributoProduto> repo)
                repo.SetTransaction(_transaction);
            
            return _atributosProduto;
        }
    }

    /// <summary>
    /// Repositório de Valores de Atributos de Produto
    /// </summary>
    public IAtributoProdutoValorRepository AtributosProdutoValores
    {
        get
        {
            if (_atributosProdutoValores == null)
                _atributosProdutoValores = new AtributoProdutoValorRepository(GetConnection());
            
            if (_atributosProdutoValores is RepositoryBase<AtributoProdutoValor> repo)
                repo.SetTransaction(_transaction);
            
            return _atributosProdutoValores;
        }
    }

    /// <summary>
    /// Repositório de Variantes de Produto
    /// </summary>
    public IProdutoVarianteRepository ProdutoVariantes
    {
        get
        {
            if (_produtoVariantes == null)
                _produtoVariantes = new ProdutoVarianteRepository(GetConnection());
            
            if (_produtoVariantes is RepositoryBase<ProdutoVariante> repo)
                repo.SetTransaction(_transaction);
            
            return _produtoVariantes;
        }
    }

    /// <summary>
    /// Repositório de Valores de Variantes de Produto
    /// </summary>
    public IProdutoVarianteValorRepository ProdutoVariantesValores
    {
        get
        {
            if (_produtoVariantesValores == null)
                _produtoVariantesValores = new ProdutoVarianteValorRepository(GetConnection());
            
            if (_produtoVariantesValores is RepositoryBase<ProdutoVarianteValor> repo)
                repo.SetTransaction(_transaction);
            
            return _produtoVariantesValores;
        }
    }

    /// <summary>
    /// Obter ou criar conexão
    /// </summary>
    private IDbConnection GetConnection()
    {
        if (_connection == null)
        {
            _connection = new SqlConnection(_connectionString);
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        return _connection;
    }

    /// <summary>
    /// Iniciar transação
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        ThrowIfDisposed();

        if (_transaction != null)
            throw new InvalidOperationException("Uma transação já está em andamento");

        var connection = GetConnection() as SqlConnection;
        _transaction = connection?.BeginTransaction();
        
        // Propagar transação a todos os repositórios
        PropagateTransactionToRepositories();
    }
    
    /// <summary>
    /// Propagar transação para todos os repositórios
    /// </summary>
    private void PropagateTransactionToRepositories()
    {
        if (_estabelecimentos is RepositoryBase<Estabelecimento> repo)
            repo.SetTransaction(_transaction);
        if (_produtos is RepositoryBase<Produto> repoProd)
            repoProd.SetTransaction(_transaction);
        if (_categorias is RepositoryBase<Categoria> repoCat)
            repoCat.SetTransaction(_transaction);
        if (_subcategorias is RepositoryBase<Subcategoria> repoSubcat)
            repoSubcat.SetTransaction(_transaction);
        if (_clientes is RepositoryBase<Cliente> repoCli)
            repoCli.SetTransaction(_transaction);
        if (_pedidos is RepositoryBase<Pedido> repoPed)
            repoPed.SetTransaction(_transaction);
        if (_usuarios is RepositoryBase<Usuario> repoUsu)
            repoUsu.SetTransaction(_transaction);
        if (_variantes is RepositoryBase<Variante> repoVar)
            repoVar.SetTransaction(_transaction);
        if (_adicionais is RepositoryBase<Adicional> repoAdi)
            repoAdi.SetTransaction(_transaction);
        if (_enderecos is RepositoryBase<Endereco> repoEnd)
            repoEnd.SetTransaction(_transaction);
        if (_atributosProduto is RepositoryBase<AtributoProduto> repoAtrib)
            repoAtrib.SetTransaction(_transaction);
        if (_atributosProdutoValores is RepositoryBase<AtributoProdutoValor> repoAtribVal)
            repoAtribVal.SetTransaction(_transaction);
        if (_produtoVariantes is RepositoryBase<ProdutoVariante> repoProdVar)
            repoProdVar.SetTransaction(_transaction);
        if (_produtoVariantesValores is RepositoryBase<ProdutoVarianteValor> repoProdVarVal)
            repoProdVarVal.SetTransaction(_transaction);
    }

    /// <summary>
    /// Confirmar transação
    /// </summary>
    public async Task CommitAsync()
    {
        ThrowIfDisposed();

        try
        {
            _transaction?.Commit();
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
        finally
        {
            ClearTransactionFromRepositories();
            _transaction?.Dispose();
            _transaction = null;
        }
    }
    
    /// <summary>
    /// Limpar transação de todos os repositórios
    /// </summary>
    private void ClearTransactionFromRepositories()
    {
        if (_estabelecimentos is RepositoryBase<Estabelecimento> repo)
            repo.SetTransaction(null);
        if (_produtos is RepositoryBase<Produto> repoProd)
            repoProd.SetTransaction(null);
        if (_categorias is RepositoryBase<Categoria> repoCat)
            repoCat.SetTransaction(null);
        if (_subcategorias is RepositoryBase<Subcategoria> repoSubcat)
            repoSubcat.SetTransaction(null);
        if (_clientes is RepositoryBase<Cliente> repoCli)
            repoCli.SetTransaction(null);
        if (_pedidos is RepositoryBase<Pedido> repoPed)
            repoPed.SetTransaction(null);
        if (_usuarios is RepositoryBase<Usuario> repoUsu)
            repoUsu.SetTransaction(null);
        if (_variantes is RepositoryBase<Variante> repoVar)
            repoVar.SetTransaction(null);
        if (_adicionais is RepositoryBase<Adicional> repoAdi)
            repoAdi.SetTransaction(null);
        if (_enderecos is RepositoryBase<Endereco> repoEnd)
            repoEnd.SetTransaction(null);
        if (_atributosProduto is RepositoryBase<AtributoProduto> repoAtrib)
            repoAtrib.SetTransaction(null);
        if (_atributosProdutoValores is RepositoryBase<AtributoProdutoValor> repoAtribVal)
            repoAtribVal.SetTransaction(null);
        if (_produtoVariantes is RepositoryBase<ProdutoVariante> repoProdVar)
            repoProdVar.SetTransaction(null);
        if (_produtoVariantesValores is RepositoryBase<ProdutoVarianteValor> repoProdVarVal)
            repoProdVarVal.SetTransaction(null);
    }

    /// <summary>
    /// Desfazer transação
    /// </summary>
    public async Task RollbackAsync()
    {
        ThrowIfDisposed();

        try
        {
            _transaction?.Rollback();
        }
        finally
        {
            ClearTransactionFromRepositories();
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    /// <summary>
    /// Verificar se há transação ativa
    /// </summary>
    public bool HasActiveTransaction => _transaction != null;

    /// <summary>
    /// Salvar mudanças (para compatibilidade com padrão)
    /// </summary>
    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            await CommitAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Verificar se foi descartado
    /// </summary>
    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(UnitOfWork));
    }

    /// <summary>
    /// Descartar recursos
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
            return;

        _transaction?.Dispose();
        _connection?.Dispose();
        _disposed = true;

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Destrutor
    /// </summary>
    ~UnitOfWork()
    {
        Dispose();
    }
}
