# Camada de Infraestrutura - MenuAdminAPI

## 📋 Visão Geral

A camada de Infraestrutura é responsável por implementar os detalhes técnicos de acesso a dados, persistência e configuração de dependências. Utiliza **Dapper** para acesso ao banco de dados SQL Server.

## 📁 Estrutura de Diretórios

```
Infrastructure/
├── Repositories/              # Implementação dos repositórios com Dapper
│   ├── RepositoryBase.cs     # Classe base com operações CRUD genéricas
│   ├── EstabelecimentoRepository.cs
│   ├── ProdutoRepository.cs
│   ├── CategoriaRepository.cs
│   ├── SubcategoriaRepository.cs
│   ├── ClienteRepository.cs
│   ├── PedidoRepository.cs
│   ├── UsuarioRepository.cs
│   ├── VarianteRepository.cs
│   ├── AdicionalRepository.cs
│   └── EnderecoRepository.cs
├── Data/                      # Contexto de dados e Unit of Work
│   └── UnitOfWork.cs         # Gerenciamento de transações
├── Configuration/             # Configuração de injeção de dependência
│   └── DependencyInjection.cs
└── README.md                 # Este arquivo
```

## 🗄️ Repositórios Implementados

### 1. RepositoryBase<T>

Classe base genérica com operações CRUD padrão:

```csharp
public abstract class RepositoryBase<T> where T : class
{
    // Operações CRUD
    public virtual async Task<T?> ObterPorIdAsync(Guid id)
    public virtual async Task<IEnumerable<T>> ObterTodosAsync()
    public virtual async Task AdicionarAsync(T entidade)
    public virtual async Task AtualizarAsync(T entidade)
    public virtual async Task RemoverAsync(Guid id)
    
    // Operações auxiliares
    public virtual async Task<int> ContarAsync()
    public virtual async Task<bool> ExisteAsync(Guid id)
    public virtual async Task<(IEnumerable<T>, int)> ObterComPaginacaoAsync(int pagina, int tamanho)
}
```

### 2. EstabelecimentoRepository

Operações específicas de estabelecimentos:

- `ObterAtivosAsync()` - Listar estabelecimentos ativos
- `ObterPorPlanoAsync()` - Filtrar por plano
- `BuscarAsync()` - Busca por termo
- `EmailJaExisteAsync()` - Validar duplicação de email
- `CnpjJaExisteAsync()` - Validar duplicação de CNPJ
- `ObterEstatisticasAsync()` - Estatísticas completas
- `AtivarAsync()` / `DesativarAsync()` - Mudar status
- `ContarAtivosAsync()` - Contar ativos

### 3. ProdutoRepository

Operações específicas de produtos:

- `ObterPorSubcategoriaAsync()` - Listar por subcategoria
- `ObterAtivosPorSubcategoriaAsync()` - Apenas ativos
- `BuscarAsync()` - Busca por termo
- `ContarPorSubcategoriaAsync()` - Contar por subcategoria
- `NomeJaExisteAsync()` - Validar duplicação
- `AtivarAsync()` / `DesativarAsync()` - Mudar status
- `ObterMaisVendidosAsync()` - Ranking de produtos
- `DeletarComCascataAsync()` - Deletar com cascata

### 4. CategoriaRepository

Operações específicas de categorias:

- `ObterPorEstabelecimentoAsync()` - Listar por estabelecimento
- `ObterAtivasPorEstabelecimentoAsync()` - Apenas ativas
- `BuscarAsync()` - Busca por termo
- `ContarPorEstabelecimentoAsync()` - Contar por estabelecimento
- `NomeJaExisteAsync()` - Validar duplicação
- `AtivarAsync()` / `DesativarAsync()` - Mudar status
- `ObterComPaginacaoAsync()` - Com paginação
- `DeletarComCascataAsync()` - Deletar com cascata

### 5. SubcategoriaRepository

Operações específicas de subcategorias:

- `ObterPorCategoriaAsync()` - Listar por categoria
- `ObterAtivasPorCategoriaAsync()` - Apenas ativas
- `ObterPorNomeAsync()` - Buscar por nome
- `ContarPorCategoriaAsync()` - Contar por categoria
- `NomeJaExisteAsync()` - Validar duplicação
- `BuscarAsync()` - Busca por termo
- `ObterComPaginacaoAsync()` - Com paginação
- `AtivarAsync()` / `DesativarAsync()` - Mudar status
- `DeletarComCascataAsync()` - Deletar com cascata

### 6. ClienteRepository

Operações específicas de clientes:

- `ObterPorEstabelecimentoAsync()` - Listar por estabelecimento
- `BuscarAsync()` - Busca por termo
- `ContarPorEstabelecimentoAsync()` - Contar por estabelecimento
- `EmailJaExisteAsync()` - Validar duplicação de email
- `CpfJaExisteAsync()` - Validar duplicação de CPF
- `ObterEstatisticasAsync()` - Estatísticas do cliente
- `AtivarAsync()` / `DesativarAsync()` - Mudar status
- `DeletarComCascataAsync()` - Deletar com cascata
- `ObterComMaisPedidosAsync()` - Clientes mais ativos

### 7. PedidoRepository

Operações específicas de pedidos:

- `ObterPorEstabelecimentoAsync()` - Listar por estabelecimento
- `ObterPorClienteAsync()` - Listar por cliente
- `ObterPorPeriodoAsync()` - Filtrar por período
- `ObterPorStatusAsync()` - Filtrar por status
- `ContarPorEstabelecimentoAsync()` - Contar por estabelecimento
- `ObterEstatisticasAsync()` - Estatísticas completas
- `MudarStatusAsync()` - Mudar status
- `CancelarAsync()` - Cancelar com motivo
- `ObterFaturamentoPorPeriodoAsync()` - Faturamento por período
- `DeletarComCascataAsync()` - Deletar com cascata

### 8. UsuarioRepository

Operações específicas de usuários:

- `ObterPorEmailAsync()` - Buscar por email
- `ObterPorEstabelecimentoAsync()` - Listar por estabelecimento
- `ObterAtivosPorEstabelecimentoAsync()` - Apenas ativos
- `BuscarAsync()` - Busca por termo
- `ContarPorEstabelecimentoAsync()` - Contar por estabelecimento
- `EmailJaExisteAsync()` - Validar duplicação
- `AtivarAsync()` / `DesativarAsync()` - Mudar status
- `AtualizarSenhaAsync()` - Atualizar senha
- `ContarAtivosAsync()` - Contar ativos

### 9. VarianteRepository

Operações específicas de variantes:

- `ObterPorProdutoAsync()` - Listar por produto
- `ObterAtivasPorProdutoAsync()` - Apenas ativas
- `ObterPorNomeAsync()` - Buscar por nome
- `ContarPorProdutoAsync()` - Contar por produto
- `NomeJaExisteAsync()` - Validar duplicação
- `BuscarAsync()` - Busca por termo
- `AtivarAsync()` / `DesativarAsync()` - Mudar status

### 10. AdicionalRepository

Operações específicas de adicionais:

- `ObterPorProdutoAsync()` - Listar por produto
- `ObterAtivosPorProdutoAsync()` - Apenas ativos
- `ObterPorNomeAsync()` - Buscar por nome
- `ContarPorProdutoAsync()` - Contar por produto
- `NomeJaExisteAsync()` - Validar duplicação
- `BuscarAsync()` - Busca por termo
- `ObterMaisUsadosAsync()` - Ranking de adicionais
- `AtivarAsync()` / `DesativarAsync()` - Mudar status

### 11. EnderecoRepository

Operações específicas de endereços:

- `ObterPorClienteAsync()` - Listar por cliente
- `ObterPadraoAsync()` - Obter padrão
- `ContarPorClienteAsync()` - Contar por cliente
- `EnderecoJaExisteAsync()` - Validar duplicação
- `BuscarAsync()` - Busca por termo
- `ObterPorCidadeAsync()` - Filtrar por cidade
- `ObterPorBairroAsync()` - Filtrar por bairro
- `ObterCidadesUnicasAsync()` - Listar cidades
- `ObterBairrosUnicosAsync()` - Listar bairros
- `DefinirComoPadraoAsync()` - Definir como padrão
- `AtivarAsync()` / `DesativarAsync()` - Mudar status

## 🔄 Unit of Work Pattern

O padrão Unit of Work gerencia transações e coordena múltiplos repositórios:

```csharp
public class UnitOfWork : IUnitOfWork, IDisposable
{
    // Propriedades dos repositórios
    public IEstabelecimentoRepository Estabelecimentos { get; }
    public IProdutoRepository Produtos { get; }
    public ICategoriaRepository Categorias { get; }
    // ... outros repositórios
    
    // Métodos de transação
    public async Task BeginTransactionAsync()
    public async Task CommitAsync()
    public async Task RollbackAsync()
    public bool HasActiveTransaction { get; }
    public async Task<bool> SaveChangesAsync()
}
```

### Exemplo de Uso

```csharp
using var unitOfWork = new UnitOfWork(connectionString);

try
{
    await unitOfWork.BeginTransactionAsync();
    
    // Operações com múltiplos repositórios
    var categoria = await unitOfWork.Categorias.ObterPorIdAsync(categoriaId);
    var subcategorias = await unitOfWork.Subcategorias.ObterPorCategoriaAsync(categoriaId);
    
    // Modificações
    await unitOfWork.Produtos.AdicionarAsync(novoProduto);
    await unitOfWork.Variantes.AdicionarAsync(novaVariante);
    
    await unitOfWork.CommitAsync();
}
catch (Exception)
{
    await unitOfWork.RollbackAsync();
    throw;
}
```

## 🔧 Configuração de Injeção de Dependência

### Registrar Serviços

```csharp
// No Program.cs
services.AddInfrastructure(connectionString);
services.AddApplicationServices(jwtSecret, jwtIssuer, jwtAudience, jwtExpirationMinutes);
```

### Usar nos Controllers

```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class EstabelecimentosController : ControllerBase
{
    private readonly IEstabelecimentoService _service;

    public EstabelecimentosController(IEstabelecimentoService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var resultado = await _service.ObterPorIdAsync(id);
        return Ok(resultado);
    }
}
```

## 📊 Padrões Implementados

### 1. Repository Pattern
- Abstração de acesso a dados
- Operações CRUD genéricas
- Operações específicas de negócio

### 2. Unit of Work Pattern
- Gerenciamento centralizado de transações
- Coordenação de múltiplos repositórios
- Garantia de consistência de dados

### 3. Lazy Loading
- Repositórios criados sob demanda
- Otimização de recursos
- Inicialização eficiente

### 4. Dependency Injection
- Registro centralizado de serviços
- Desacoplamento de dependências
- Facilita testes e manutenção

## 🔐 Segurança

### Validações
- Verificação de null/empty
- Validação de duplicação
- Verificação de existência

### Transações
- Rollback automático em caso de erro
- Isolamento de operações
- Consistência de dados

### Acesso a Dados
- Parametrização de queries (prevenção de SQL Injection)
- Uso de Dapper para segurança
- Validação de entrada

## 📈 Performance

### Índices
- Índices em chaves estrangeiras
- Índices em campos de busca
- Índices em campos de filtro

### Queries Otimizadas
- SELECT apenas colunas necessárias
- ORDER BY apropriado
- OFFSET/FETCH para paginação

### Lazy Loading
- Repositórios criados sob demanda
- Conexão reutilizada
- Minimização de recursos

## 🧪 Testes

### Testes Unitários
- Mock de IDbConnection
- Mock de repositórios
- Testes de lógica de negócio

### Testes de Integração
- Testcontainers para SQL Server
- Testes com banco de dados real
- Testes de transações

## 📝 Boas Práticas

1. **Sempre usar Unit of Work para múltiplas operações**
2. **Validar entrada em todos os repositórios**
3. **Usar transações para operações críticas**
4. **Implementar paginação para grandes volumes**
5. **Criar índices para melhorar performance**
6. **Documentar queries complexas**
7. **Testar cascata de deletes**
8. **Usar soft delete quando apropriado**

## 🔗 Referências

- [Dapper Documentation](https://github.com/DapperLib/Dapper)
- [Repository Pattern](https://martinfowler.com/eaaCatalog/repository.html)
- [Unit of Work Pattern](https://martinfowler.com/eaaCatalog/unitOfWork.html)
- [SQL Server Best Practices](https://docs.microsoft.com/sql/relational-databases/sql-server-guides)
