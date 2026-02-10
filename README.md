# Menu Admin API - Solução Completa

Solução profissional de API REST para gerenciamento de cardápios, produtos, clientes e pedidos. Desenvolvida em C# .NET 8 com arquitetura em camadas (DDD - Domain-Driven Design).

## 📋 Estrutura da Solução

```
MenuAdminAPI_Solution/
├── src/                                    # Código-fonte da aplicação
│   ├── Domain/                             # Camada de Domínio (Entidades, Value Objects)
│   │   ├── Entities/                       # Entidades do domínio
│   │   ├── ValueObjects/                   # Value Objects
│   │   ├── Repositories/                   # Interfaces de repositórios
│   │   ├── Interfaces/                     # Interfaces do domínio
│   │   └── MenuAdminAPI.Domain.csproj      # Projeto Domain
│   │
│   ├── Application/                        # Camada de Aplicação (Services, DTOs)
│   │   ├── Services/                       # Serviços de aplicação
│   │   ├── DTOs/                           # Data Transfer Objects
│   │   ├── Mappings/                       # Mapeadores de entidades
│   │   ├── Validators/                     # Validadores de entrada
│   │   └── MenuAdminAPI.Application.csproj # Projeto Application
│   │
│   ├── Infrastructure/                     # Camada de Infraestrutura (Repositories, Database)
│   │   ├── Repositories/                   # Implementação de repositórios
│   │   ├── Data/                           # Contexto de dados
│   │   ├── Configuration/                  # Configuração de injeção de dependência
│   │   ├── Persistence/                    # Persistência de dados
│   │   └── MenuAdminAPI.Infrastructure.csproj # Projeto Infrastructure
│   │
│   └── Presentation/                       # Camada de Apresentação (Controllers, API)
│       ├── Controllers/                    # Controllers REST
│       ├── Middleware/                     # Middleware customizado
│       ├── Configuration/                  # Configuração de Swagger, JWT, CORS
│       ├── Program.cs                      # Startup da aplicação
│       └── MenuAdminAPI.Presentation.csproj # Projeto Presentation
│
├── tests/                                  # Testes automatizados
│   ├── Domain.Tests/                       # Testes unitários do Domain
│   ├── Application.Tests/                  # Testes unitários da Application
│   ├── Infrastructure.Tests/               # Testes de integração da Infrastructure
│   └── Presentation.Tests/                 # Testes de integração da Presentation
│
├── Database/                               # Scripts SQL
│   ├── 01-CreateDatabase.sql               # Criação do banco de dados
│   ├── 02-SeedData.sql                     # Dados iniciais
│   ├── 03-UsefulQueries.sql                # Consultas úteis
│   └── 04-BackupRestore.sql                # Backup e restore
│
├── docs/                                   # Documentação
│   └── ARCHITECTURE.md                     # Documentação de arquitetura
│
├── MenuAdminAPI.sln                        # Arquivo de solução
└── README.md                               # Este arquivo

```

## 🏗️ Arquitetura DDD (Domain-Driven Design)

### **Domain Layer (MenuAdminAPI.Domain)**
- **Responsabilidade**: Lógica de negócio pura
- **Dependências**: Nenhuma (independente)
- **Contém**: 
  - Entidades (Estabelecimento, Produto, Pedido, etc)
  - Value Objects (Moeda, Email, Endereço)
  - Interfaces de Repositórios
  - Exceções de domínio

### **Application Layer (MenuAdminAPI.Application)**
- **Responsabilidade**: Orquestração de casos de uso
- **Dependências**: Domain
- **Contém**:
  - Application Services
  - DTOs (Data Transfer Objects)
  - Mapeadores (Mappings)
  - Validadores

### **Infrastructure Layer (MenuAdminAPI.Infrastructure)**
- **Responsabilidade**: Implementação técnica
- **Dependências**: Domain, Application
- **Contém**:
  - Implementação de Repositórios (Dapper)
  - Unit of Work Pattern
  - Configuração de Injeção de Dependência
  - Acesso a banco de dados

### **Presentation Layer (MenuAdminAPI.Presentation)**
- **Responsabilidade**: Exposição de APIs
- **Dependências**: Domain, Application, Infrastructure
- **Contém**:
  - Controllers REST
  - Middleware (Autenticação, Logging, Tratamento de Erros)
  - Configuração de Swagger, JWT, CORS
  - Program.cs (Startup)

## 🚀 Como Começar

### Pré-requisitos

- **.NET 8.0** ou superior
- **Visual Studio 2022** ou **Visual Studio Code**
- **SQL Server 2019** ou superior (ou SQL Server Express)
- **Git**

### 1. Clonar o Repositório

```bash
git clone <seu-repositorio>
cd MenuAdminAPI_Solution
```

### 2. Abrir a Solução

```bash
# Com Visual Studio
start MenuAdminAPI.sln

# Ou com Visual Studio Code
code .
```

### 3. Restaurar Dependências

```bash
dotnet restore
```

### 4. Configurar Banco de Dados

#### Opção A: Usar SQL Server Management Studio

1. Abrir `Database/01-CreateDatabase.sql`
2. Executar o script
3. Executar `Database/02-SeedData.sql` para dados iniciais

#### Opção B: Usar dotnet CLI

```bash
# Restaurar dependências
dotnet restore

# Executar migrations (quando implementadas)
dotnet ef database update
```

### 5. Configurar Secrets

Editar `appsettings.json` com suas configurações:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MenuAdminDB;User Id=sa;Password=YourPassword123!;Encrypt=false;TrustServerCertificate=true;"
  },
  "Jwt": {
    "Secret": "your-super-secret-key-that-is-at-least-32-characters-long",
    "Issuer": "MenuAdminAPI",
    "Audience": "MenuAdminClients",
    "ExpirationMinutes": 60
  }
}
```

### 6. Executar a Aplicação

```bash
cd src/Presentation
dotnet run
```

A API estará disponível em:
- **HTTP**: http://localhost:5000
- **HTTPS**: https://localhost:5001
- **Swagger**: http://localhost:5000/swagger

## 📚 Referências de Projetos

### Dependências Entre Projetos

```
Domain (sem dependências)
  ↑
Application (depende de Domain)
  ↑
Infrastructure (depende de Domain e Application)
  ↑
Presentation (depende de Domain, Application e Infrastructure)

Tests (cada um depende de seu projeto correspondente)
```

### Fluxo de Requisição

```
1. HTTP Request
   ↓
2. Presentation Layer (Controllers)
   ├── Validação de entrada
   ├── Chamada de Application Service
   └── Retorno de resposta
   ↓
3. Application Layer (Services)
   ├── Orquestração de negócio
   ├── Chamada de Infrastructure
   └── Mapeamento de DTOs
   ↓
4. Infrastructure Layer (Repositories)
   ├── Acesso ao banco de dados
   ├── Unit of Work
   └── Retorno de entidades
   ↓
5. Domain Layer (Entities)
   └── Lógica de negócio pura
```

## 🧪 Testes

### Executar Todos os Testes

```bash
dotnet test
```

### Executar Testes de um Projeto

```bash
# Testes do Domain
dotnet test tests/Domain.Tests/MenuAdminAPI.Domain.Tests.csproj

# Testes da Application
dotnet test tests/Application.Tests/MenuAdminAPI.Application.Tests.csproj

# Testes da Infrastructure
dotnet test tests/Infrastructure.Tests/MenuAdminAPI.Infrastructure.Tests.csproj

# Testes da Presentation
dotnet test tests/Presentation.Tests/MenuAdminAPI.Presentation.Tests.csproj
```

### Cobertura de Testes

```bash
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

## 📊 Endpoints da API

### Autenticação

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/v1/auth/login` | Login |
| POST | `/api/v1/auth/refresh-token` | Renovar token |
| GET | `/api/v1/auth/me` | Usuário autenticado |

### Estabelecimentos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/v1/estabelecimentos` | Listar todos |
| GET | `/api/v1/estabelecimentos/{id}` | Obter por ID |
| POST | `/api/v1/estabelecimentos` | Criar novo |
| PUT | `/api/v1/estabelecimentos/{id}` | Atualizar |
| DELETE | `/api/v1/estabelecimentos/{id}` | Deletar |

### Produtos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/v1/produtos/{id}` | Obter com variantes |
| GET | `/api/v1/produtos/subcategoria/{id}` | Listar por subcategoria |
| POST | `/api/v1/produtos` | Criar novo |
| POST | `/api/v1/produtos/{id}/variantes` | Adicionar variante |
| POST | `/api/v1/produtos/{id}/adicionais` | Adicionar adicional |

### Clientes

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/v1/clientes/{id}` | Obter com endereços |
| GET | `/api/v1/clientes/estabelecimento/{id}` | Listar por estabelecimento |
| POST | `/api/v1/clientes` | Criar novo |
| POST | `/api/v1/clientes/{id}/enderecos` | Adicionar endereço |

### Pedidos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/v1/pedidos/{id}` | Obter com itens |
| GET | `/api/v1/pedidos/estabelecimento/{id}` | Listar por estabelecimento |
| POST | `/api/v1/pedidos` | Criar novo |
| PATCH | `/api/v1/pedidos/{id}/confirmar` | Confirmar |
| PATCH | `/api/v1/pedidos/{id}/cancelar` | Cancelar |

## 🔧 Build e Deploy

### Build para Release

```bash
dotnet build -c Release
```

### Publicar para Produção

```bash
dotnet publish -c Release -o ./publish
```

### Docker

```bash
# Build da imagem
docker build -t menuadminapi:latest .

# Executar container
docker run -p 5000:5000 -p 5001:5001 menuadminapi:latest
```

## 📝 Padrões de Código

### Naming Conventions

- **Namespaces**: `MenuAdminAPI.{Layer}.{Feature}`
- **Classes**: PascalCase (ex: `EstabelecimentoService`)
- **Métodos**: PascalCase (ex: `ObterPorIdAsync`)
- **Variáveis**: camelCase (ex: `estabelecimentoId`)
- **Constantes**: UPPER_CASE (ex: `MAX_ITEMS_PER_PAGE`)

### Estrutura de Classe

```csharp
namespace MenuAdminAPI.Application.Services;

public interface IEstabelecimentoService
{
    Task<EstabelecimentoResponse> ObterPorIdAsync(Guid id);
    Task<IEnumerable<EstabelecimentoResponse>> ObterTodosAsync();
    Task<EstabelecimentoResponse> CriarAsync(CriarEstabelecimentoRequest request);
}

public class EstabelecimentoService : IEstabelecimentoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EstabelecimentoService> _logger;

    public EstabelecimentoService(IUnitOfWork unitOfWork, ILogger<EstabelecimentoService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<EstabelecimentoResponse> ObterPorIdAsync(Guid id)
    {
        _logger.LogInformation("Obtendo estabelecimento {EstabelecimentoId}", id);
        
        var estabelecimento = await _unitOfWork.Estabelecimentos.ObterPorIdAsync(id);
        
        if (estabelecimento == null)
            throw new InvalidOperationException($"Estabelecimento {id} não encontrado");
        
        return estabelecimento.ToResponse();
    }
}
```

## 🔐 Segurança

- **Autenticação JWT**: Tokens com expiração
- **CORS**: Configurado para origens específicas
- **Hash de Senha**: SHA256 com salt
- **Validação de Entrada**: Em todos os endpoints
- **Tratamento de Exceções**: Centralizado

## 📞 Suporte

Para dúvidas ou problemas:

1. Verificar documentação em `/docs`
2. Abrir issue no repositório
3. Contatar o time de desenvolvimento

## 📄 Licença

MIT License - veja LICENSE.md para detalhes

## 👥 Contribuindo

1. Fork o projeto
2. Criar branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abrir Pull Request

## 🎯 Roadmap

- [ ] Integração com payment gateway (Stripe)
- [ ] Notificações em tempo real (SignalR)
- [ ] Cache distribuído (Redis)
- [ ] Autenticação OAuth2
- [ ] Relatórios avançados
- [ ] Mobile App
- [ ] Integração com sistemas de entrega
