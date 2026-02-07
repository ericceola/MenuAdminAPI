using System.Data;
using System.Data.SqlClient;
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

    public UnitOfWork(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    /// <summary>
    /// Repositório de Estabelecimentos
    /// </summary>
    public IEstabelecimentoRepository Estabelecimentos =>
        _estabelecimentos ??= new EstabelecimentoRepository(GetConnection());

    /// <summary>
    /// Repositório de Produtos
    /// </summary>
    public IProdutoRepository Produtos =>
        _produtos ??= new ProdutoRepository(GetConnection());

    /// <summary>
    /// Repositório de Categorias
    /// </summary>
    public ICategoriaRepository Categorias =>
        _categorias ??= new CategoriaRepository(GetConnection());

    /// <summary>
    /// Repositório de Subcategorias
    /// </summary>
    public ISubcategoriaRepository Subcategorias =>
        _subcategorias ??= new SubcategoriaRepository(GetConnection());

    /// <summary>
    /// Repositório de Clientes
    /// </summary>
    public IClienteRepository Clientes =>
        _clientes ??= new ClienteRepository(GetConnection());

    /// <summary>
    /// Repositório de Pedidos
    /// </summary>
    public IPedidoRepository Pedidos =>
        _pedidos ??= new PedidoRepository(GetConnection());

    /// <summary>
    /// Repositório de Usuários
    /// </summary>
    public IUsuarioRepository Usuarios =>
        _usuarios ??= new UsuarioRepository(GetConnection());

    /// <summary>
    /// Repositório de Variantes
    /// </summary>
    public IVarianteRepository Variantes =>
        _variantes ??= new VarianteRepository(GetConnection());

    /// <summary>
    /// Repositório de Adicionais
    /// </summary>
    public IAdicionalRepository Adicionais =>
        _adicionais ??= new AdicionalRepository(GetConnection());

    /// <summary>
    /// Repositório de Endereços
    /// </summary>
    public IEnderecoRepository Enderecos =>
        _enderecos ??= new EnderecoRepository(GetConnection());

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
            _transaction?.Dispose();
            _transaction = null;
        }
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
