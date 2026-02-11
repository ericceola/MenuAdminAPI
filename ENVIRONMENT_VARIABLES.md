# Convenção de Nomes para Variáveis de Ambiente

Documentação sobre como usar variáveis de ambiente corretamente em diferentes contextos.

## 🔑 Formatos de Nomes

### 1. **Variáveis de Ambiente do Sistema** (Recomendado)

Use **`__` (double underscore)** para separar níveis de configuração:

```
SendGrid__ApiKey
SendGrid__FromEmail
SendGrid__FromName
ConnectionStrings__DefaultConnection
Jwt__Secret
Jwt__Issuer
Logging__LogLevel__Default
```

**Por quê?**
- ✅ Compatível com todos os ambientes (Windows, Linux, macOS)
- ✅ .NET converte automaticamente `__` em `:`
- ✅ Funciona em Docker, Kubernetes, CI/CD
- ✅ Recomendado pela Microsoft

**Onde usar:**
- Variáveis de ambiente do sistema
- Arquivos `.env` / `.env.local`
- Docker `ENV` e `--env`
- GitHub Actions secrets
- Azure App Service Configuration

---

### 2. **Azure Portal (Configuration)**

Use **`:` (colon)** para separar níveis de configuração:

```
SendGrid:ApiKey
SendGrid:FromEmail
SendGrid:FromName
ConnectionStrings:DefaultConnection
Jwt:Secret
Jwt:Issuer
Logging:LogLevel:Default
```

**Por quê?**
- ✅ Interface visual mais limpa
- ✅ Azure converte automaticamente para `__` internamente
- ✅ Mais legível no portal

**Onde usar:**
- Azure Portal → App Service → Configuration
- Azure Portal → App Service → Application Settings
- Azure Key Vault (referências)

---

### 3. **Arquivos de Configuração JSON** (`appsettings.json`)

Use **`:` (colon)** na estrutura JSON:

```json
{
  "SendGrid": {
    "ApiKey": "...",
    "FromEmail": "...",
    "FromName": "..."
  },
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "Jwt": {
    "Secret": "...",
    "Issuer": "...",
    "Audience": "..."
  }
}
```

**Por quê?**
- ✅ Estrutura hierárquica natural do JSON
- ✅ Mais fácil de ler e manter
- ✅ Suporta tipos complexos

**Onde usar:**
- `appsettings.json`
- `appsettings.Development.json`
- `appsettings.Production.json`

---

## 🔄 Conversão Automática do .NET

O .NET Core converte automaticamente entre formatos:

```
Variável de Ambiente    →    Chave de Configuração    →    JSON
SendGrid__ApiKey        →    SendGrid:ApiKey          →    SendGrid.ApiKey
SendGrid__FromEmail     →    SendGrid:FromEmail       →    SendGrid.FromEmail
Logging__LogLevel__Default → Logging:LogLevel:Default → Logging.LogLevel.Default
```

### Exemplo Prático

**Variável de Ambiente:**
```bash
export SendGrid__ApiKey="SG.YOUR_API_KEY_HERE"
```

**Arquivo `appsettings.json`:**
```json
{
  "SendGrid": {
    "ApiKey": "SG.YOUR_API_KEY_HERE"
  }
}
```

**Código C#:**
```csharp
var apiKey = configuration["SendGrid:ApiKey"];
// Resultado: "SG.YOUR_API_KEY_HERE"
```

---

## 📊 Tabela de Referência

| Contexto | Formato | Exemplo |
|----------|---------|---------|
| **Variáveis de Ambiente** | `__` (double underscore) | `SendGrid__ApiKey` |
| **Azure Portal** | `:` (colon) | `SendGrid:ApiKey` |
| **Arquivo JSON** | Estrutura hierárquica | `"SendGrid": { "ApiKey": "..." }` |
| **Arquivo .env** | `__` (double underscore) | `SendGrid__ApiKey=...` |
| **Docker ENV** | `__` (double underscore) | `ENV SendGrid__ApiKey=...` |
| **GitHub Actions** | `__` (double underscore) | `SendGrid__ApiKey: ${{ secrets.SENDGRID_API_KEY }}` |
| **Código C#** | `:` (colon) | `configuration["SendGrid:ApiKey"]` |

---

## ✅ Checklist de Configuração

### Desenvolvimento Local

- [ ] Arquivo `.env.local` usa formato `__`
- [ ] Arquivo `appsettings.Development.json` usa estrutura JSON
- [ ] Variáveis de ambiente do sistema usam formato `__`
- [ ] Código C# acessa com `:` (colon)

### Produção (Azure)

- [ ] Azure Portal Configuration usa formato `:` (colon)
- [ ] Arquivo `appsettings.Production.json` usa estrutura JSON
- [ ] Código C# acessa com `:` (colon)

### CI/CD (GitHub Actions)

- [ ] Secrets usam formato `__` (double underscore)
- [ ] Variáveis de ambiente usam formato `__`

---

## 🔗 Referências

- [Configuration in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration)
- [Environment Variables in .NET](https://learn.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariable)
- [Azure App Service Configuration](https://learn.microsoft.com/en-us/azure/app-service/configure-common)
- [Options Pattern in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/options)

---

## 📝 Exemplos Práticos

### Exemplo 1: Desenvolvimento Local com `.env.local`

**Arquivo `.env.local`:**
```
SendGrid__ApiKey=SG.YOUR_API_KEY_HERE
SendGrid__FromEmail=noreply@cafesenador.com.br
SendGrid__FromName=Menu Admin Platform
```

**Código C#:**
```csharp
var apiKey = configuration["SendGrid:ApiKey"];
var fromEmail = configuration["SendGrid:FromEmail"];
var fromName = configuration["SendGrid:FromName"];
```

---

### Exemplo 2: Produção no Azure

**Azure Portal → Configuration:**
- Name: `SendGrid:ApiKey`
- Value: `SG.YOUR_API_KEY_HERE`

**Código C# (mesmo):**
```csharp
var apiKey = configuration["SendGrid:ApiKey"];
```

---

### Exemplo 3: Docker

**Dockerfile:**
```dockerfile
ENV SendGrid__ApiKey=SG.YOUR_API_KEY_HERE
ENV SendGrid__FromEmail=noreply@cafesenador.com.br
ENV SendGrid__FromName=Menu Admin Platform
```

**Código C# (mesmo):**
```csharp
var apiKey = configuration["SendGrid:ApiKey"];
```

---

## 🚨 Erros Comuns

### ❌ Erro 1: Usar `:` em Variáveis de Ambiente

```bash
# ❌ ERRADO - Não funciona em todos os ambientes
export SendGrid:ApiKey="SG.YOUR_API_KEY_HERE"

# ✅ CORRETO
export SendGrid__ApiKey="SG.YOUR_API_KEY_HERE"
```

### ❌ Erro 2: Usar `__` no Azure Portal

```
# ❌ ERRADO
Name: SendGrid__ApiKey

# ✅ CORRETO
Name: SendGrid:ApiKey
```

### ❌ Erro 3: Usar `:` em Arquivo `.env`

```
# ❌ ERRADO
SendGrid:ApiKey=SG.YOUR_API_KEY_HERE

# ✅ CORRETO
SendGrid__ApiKey=SG.YOUR_API_KEY_HERE
```

---

**Última atualização:** 2026-02-11
