# Documentação de Arquitetura - Menu Admin API

## 📐 Visão Geral da Arquitetura

A Menu Admin API segue os princípios de **Domain-Driven Design (DDD)** com uma arquitetura em camadas bem definidas. Cada camada tem responsabilidades específicas e dependências controladas.

## 🏗️ Camadas da Arquitetura

### 1. Domain Layer (Camada de Domínio)

**Localização**: `src/Domain/`

**Responsabilidade**: Encapsular a lógica de negócio pura, independente de qualquer framework ou tecnologia.

**Componentes**:

#### Entities (Entidades)
Objetos com identidade única que representam conceitos do domínio.

```csharp
public class Estabelecimento
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public ICollection<Produto> Produtos { get; set; }
    public ICollection<Pedido> Pedidos { get; set; }
}
```

#### Value Objects
Objetos sem identidade que representam valores.

```csharp
public record Moeda(decimal Valor, string Moeda = "BRL")
{
    public static Moeda Zero => new(0);
    public Moeda Adicionar(Moeda outra) => new(Valor + outra.Valor);
}
```

#### Repositories (Interfaces)
Abstrações para acesso a dados.

```csharp
public interface IEstabelecimentoRepository
{
    Task<Estabelecimento> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Estabelecimento>> ObterTodosAsync();
    Task AdicionarAsync(Estabelecimento estabelecimento);
    Task AtualizarAsync(Estabelecimento estabelecimento);
    Task RemoverAsync(Guid id);
}
```

#### Interfaces de Domínio
Contratos que definem comportamentos esperados.

```csharp
public interface IUnitOfWork
{
    IEstabelecimentoRepository Estabelecimentos { get; }
    IProdutoRepository Produtos { get; }
    Task<bool> CommitAsync();
}
```

**Características**:
- ✅ Sem dependências externas
- ✅ Lógica de negócio centralizada
- ✅ Fácil de testar
- ✅ Reutilizável

---

### 2. Application Layer (Camada de Aplicação)

**Localização**: `src/Application/`

**Responsabilidade**: Orquestrar casos de uso e coordenar entre Domain e Infrastructure.

**Componentes**:

#### Application Services
Serviços que implementam casos de uso.

```csharp
public class EstabelecimentoService : IEstabelecimentoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EstabelecimentoService> _logger;

    public async Task<EstabelecimentoResponse> ObterPorIdAsync(Guid id)
    {
        var estabelecimento = await _unitOfWork.Estabelecimentos.ObterPorIdAsync(id);
        
        if (estabelecimento == null)
            throw new InvalidOperationException("Estabelecimento não encontrado");
        
        return estabelecimento.ToResponse();
    }
}
```

#### DTOs (Data Transfer Objects)
Objetos para transferência de dados entre camadas.

```csharp
public record CriarEstabelecimentoRequest(
    string Nome,
    string Email,
    string Telefone,
    string CNPJ
);

public record EstabelecimentoResponse(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    int Plano,
    bool Ativo
);
```

#### Mappings (Mapeadores)
Conversão entre entidades e DTOs.

```csharp
public static class MappingProfile
{
    public static EstabelecimentoResponse ToResponse(this Estabelecimento entity)
    {
        return new EstabelecimentoResponse(
            entity.Id,
            entity.Nome,
            entity.Email,
            entity.Telefone,
            entity.Plano,
            entity.Ativo
        );
    }
}
```

#### Validators (Validadores)
Validação de entrada de dados.

```csharp
public class CriarEstabelecimentoValidator : AbstractValidator<CriarEstabelecimentoRequest>
{
    public CriarEstabelecimentoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(255).WithMessage("Nome deve ter no máximo 255 caracteres");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email inválido");
    }
}
```

**Características**:
- ✅ Orquestração de casos de uso
- ✅ Validação centralizada
- ✅ Mapeamento de dados
- ✅ Depende apenas de Domain

---

### 3. Infrastructure Layer (Camada de Infraestrutura)

**Localização**: `src/Infrastructure/`

**Responsabilidade**: Implementar detalhes técnicos como acesso a banco de dados, logging, etc.

**Componentes**:

#### Repositories (Implementações)
Implementação concreta de acesso a dados com Dapper.

```csharp
public class EstabelecimentoRepository : RepositoryBase<Estabelecimento>, IEstabelecimentoRepository
{
    public EstabelecimentoRepository(IDbConnection connection) : base(connection) { }

    public async Task<Estabelecimento> ObterPorIdAsync(Guid id)
    {
        const string sql = @"
            SELECT Id, Nome, Email, Telefone, CNPJ, Endereco, Cidade, Estado, CEP, Plano, Ativo, DataCriacao, DataAtualizacao
            FROM Estabelecimentos
            WHERE Id = @Id AND Ativo = 1";
        
        return await _connection.QueryFirstOrDefaultAsync<Estabelecimento>(sql, new { Id = id });
    }
}
```

#### Unit of Work Pattern
Gerenciamento de transações e coordenação de repositórios.

```csharp
public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly string _connectionString;
    private SqlConnection _connection;
    private SqlTransaction _transaction;

    public IEstabelecimentoRepository Estabelecimentos => 
        _estabelecimentos ??= new EstabelecimentoRepository(_connection);

    public async Task<bool> CommitAsync()
    {
        try
        {
            _transaction?.Commit();
            return true;
        }
        catch
        {
            _transaction?.Rollback();
            throw;
        }
    }
}
```

#### Dependency Injection Configuration
Registro de serviços.

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddMenuAdminAPI(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddScoped<IUnitOfWork>(_ => new UnitOfWork(connectionString));
        services.AddScoped<IEstabelecimentoService, EstabelecimentoService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        
        return services;
    }
}
```

**Características**:
- ✅ Implementação de repositórios
- ✅ Acesso a banco de dados
- ✅ Configuração de injeção de dependência
- ✅ Gerenciamento de transações

---

### 4. Presentation Layer (Camada de Apresentação)

**Localização**: `src/Presentation/`

**Responsabilidade**: Expor a API REST e gerenciar requisições HTTP.

**Componentes**:

#### Controllers
Endpoints REST.

```csharp
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class EstabelecimentosController : BaseController
{
    private readonly IEstabelecimentoService _service;

    public EstabelecimentosController(IEstabelecimentoService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var resultado = await _service.ObterPorIdAsync(id);
            return OkResponse(resultado);
        }
        catch (InvalidOperationException ex)
        {
            return NotFoundResponse();
        }
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Criar([FromBody] CriarEstabelecimentoRequest request)
    {
        var resultado = await _service.CriarAsync(request);
        return CreatedResponse(resultado);
    }
}
```

#### Middleware
Processamento de requisições.

```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new ErrorResponse("Erro interno do servidor"));
        }
    }
}
```

#### Configuration
Configuração de Swagger, JWT, CORS.

```csharp
public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Menu Admin API",
                Version = "1.0.0",
                Description = "API para gerenciamento de cardápios"
            });
            
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });
        });

        return services;
    }
}
```

#### Program.cs
Startup da aplicação.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerConfiguration(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddMenuAdminAPI(connectionString);

var app = builder.Build();

app.UseSwaggerConfiguration();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

**Características**:
- ✅ Endpoints REST
- ✅ Middleware customizado
- ✅ Configuração de segurança
- ✅ Documentação com Swagger

---

## 🔄 Fluxo de Dados

### Requisição HTTP

```
1. Cliente HTTP
   ↓
2. Middleware (CORS, Autenticação, Logging)
   ↓
3. Controller (Validação de entrada)
   ↓
4. Application Service (Orquestração)
   ↓
5. Domain Entities (Lógica de negócio)
   ↓
6. Infrastructure Repository (Acesso a dados)
   ↓
7. Banco de Dados
   ↓
8. Resposta HTTP
```

### Exemplo Prático: Criar Estabelecimento

```
POST /api/v1/estabelecimentos
{
  "nome": "Pizzaria do João",
  "email": "contato@pizzariadojoao.com.br",
  "telefone": "(11) 3000-0001",
  "cnpj": "12.345.678/0001-90"
}

↓ Presentation Layer
EstabelecimentosController.Criar()
├── Validação de entrada
└── Chamada de IEstabelecimentoService.CriarAsync()

↓ Application Layer
EstabelecimentoService.CriarAsync()
├── Validação de negócio
├── Mapeamento de DTO para Entidade
└── Chamada de IUnitOfWork.Estabelecimentos.AdicionarAsync()

↓ Domain Layer
Estabelecimento (Entity)
└── Validações de domínio

↓ Infrastructure Layer
EstabelecimentoRepository.AdicionarAsync()
├── Geração de SQL INSERT
└── Execução no banco de dados

↓ Resposta
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "nome": "Pizzaria do João",
  "email": "contato@pizzariadojoao.com.br",
  "telefone": "(11) 3000-0001",
  "plano": 1,
  "ativo": true
}
```

---

## 🧪 Testes

### Estrutura de Testes

```
tests/
├── Domain.Tests/
│   └── Entities/
│       └── EstabelecimentoTests.cs
├── Application.Tests/
│   └── Services/
│       └── EstabelecimentoServiceTests.cs
├── Infrastructure.Tests/
│   └── Repositories/
│       └── EstabelecimentoRepositoryTests.cs
└── Presentation.Tests/
    └── Controllers/
        └── EstabelecimentosControllerTests.cs
```

### Exemplo de Teste Unitário

```csharp
public class EstabelecimentoServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly EstabelecimentoService _service;

    public EstabelecimentoServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _service = new EstabelecimentoService(_unitOfWorkMock.Object, new Mock<ILogger<EstabelecimentoService>>().Object);
    }

    [Fact]
    public async Task ObterPorIdAsync_ComIdValido_RetornaEstabelecimento()
    {
        // Arrange
        var id = Guid.NewGuid();
        var estabelecimento = new Estabelecimento { Id = id, Nome = "Pizzaria" };
        _unitOfWorkMock.Setup(x => x.Estabelecimentos.ObterPorIdAsync(id))
            .ReturnsAsync(estabelecimento);

        // Act
        var resultado = await _service.ObterPorIdAsync(id);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be("Pizzaria");
    }
}
```

---

## 📦 Dependências Entre Projetos

```
MenuAdminAPI.Domain
    ↑
    └─ Sem dependências externas

MenuAdminAPI.Application
    ↑
    ├─ MenuAdminAPI.Domain
    └─ FluentValidation

MenuAdminAPI.Infrastructure
    ↑
    ├─ MenuAdminAPI.Domain
    ├─ MenuAdminAPI.Application
    ├─ Dapper
    ├─ System.Data.SqlClient
    └─ Serilog

MenuAdminAPI.Presentation
    ↑
    ├─ MenuAdminAPI.Domain
    ├─ MenuAdminAPI.Application
    ├─ MenuAdminAPI.Infrastructure
    ├─ Microsoft.AspNetCore
    ├─ Swashbuckle.AspNetCore
    └─ System.IdentityModel.Tokens.Jwt
```

---

## 🔐 Padrões de Segurança

### Autenticação JWT

```csharp
// Login
POST /api/v1/auth/login
{
  "email": "usuario@example.com",
  "senha": "senha123"
}

// Resposta
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiracaoToken": "2024-01-15T10:30:00Z"
}

// Usar token
GET /api/v1/estabelecimentos
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### CORS

```csharp
// Configuração
{
  "Cors": {
    "AllowedOrigins": ["http://localhost:3000", "https://menuadmin.com"],
    "AllowedMethods": ["GET", "POST", "PUT", "DELETE"],
    "AllowedHeaders": ["Content-Type", "Authorization"]
  }
}
```

---

## 📊 Banco de Dados

### Modelo Entidade-Relacionamento

```
Estabelecimentos
├── Usuarios (1:N)
├── Categorias (1:N)
│   └── Subcategorias (1:N)
│       └── Produtos (1:N)
│           ├── Variantes (1:N)
│           └── Adicionais (1:N)
├── Clientes (1:N)
│   ├── Enderecos (1:N)
│   └── Pedidos (1:N)
│       └── ItemPedidos (1:N)
│           └── AdicionalPedidos (1:N)
```

---

## 🚀 Performance

### Índices

- `Usuarios.Email` - Busca rápida de usuários
- `Pedidos.EstabelecimentoId` - Filtro por estabelecimento
- `Pedidos.DataCriacao` - Ordenação por data
- `Produtos.SubcategoriaId` - Filtro por subcategoria

### Caching

```csharp
// Implementar cache em Application Service
public async Task<EstabelecimentoResponse> ObterPorIdAsync(Guid id)
{
    var cacheKey = $"estabelecimento_{id}";
    
    if (_cache.TryGetValue(cacheKey, out EstabelecimentoResponse resultado))
        return resultado;
    
    var estabelecimento = await _unitOfWork.Estabelecimentos.ObterPorIdAsync(id);
    _cache.Set(cacheKey, estabelecimento.ToResponse(), TimeSpan.FromHours(1));
    
    return estabelecimento.ToResponse();
}
```

---

## 📝 Boas Práticas

1. **Separação de Responsabilidades**: Cada camada tem um propósito específico
2. **Injeção de Dependência**: Usar DI para desacoplamento
3. **Validação em Múltiplas Camadas**: Domain, Application e Presentation
4. **Logging Estruturado**: Usar Serilog para logs
5. **Testes Automatizados**: Cobertura mínima de 80%
6. **Documentação de Código**: XML comments para métodos públicos
7. **Versionamento de API**: Usar `/api/v1/` nos endpoints
8. **Tratamento de Erros**: Centralizado com middleware

---

## 🔗 Referências

- [Domain-Driven Design - Eric Evans](https://www.domainlanguage.com/ddd/)
- [Clean Architecture - Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Dapper Documentation](https://github.com/DapperLib/Dapper)
