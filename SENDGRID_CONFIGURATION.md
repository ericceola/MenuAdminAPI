# Configuração SendGrid

Documentação sobre como configurar o SendGrid para envio de emails na API MenuAdmin.

## 📋 Visão Geral

A API MenuAdmin usa o **SendGrid** para enviar emails automaticamente quando:
- Um novo usuário é criado (credenciais de acesso)
- Uma senha é resetada
- Um email de boas-vindas é enviado

## 🔧 Arquitetura de Configuração

### 1. **Classe SendGridOptions** (`src/Application/Options/SendGridOptions.cs`)

Classe tipada para armazenar as configurações:

```csharp
public class SendGridOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string FromEmail { get; set; } = "noreply@menuadminapi.com";
    public string FromName { get; set; } = "Menu Admin";
}
```

### 2. **Registro no DI** (`src/Presentation/Program.cs`)

```csharp
// Configurar SendGrid Options
builder.Services.Configure<MenuAdminAPI.Application.Options.SendGridOptions>(
    builder.Configuration.GetSection("SendGrid"));
```

O .NET automaticamente:
- Lê a seção "SendGrid" do `appsettings.json`
- Converte variáveis de ambiente com `__` (double underscore) para `:` (colon)
- Injeta as opções no `EmailService` via `IOptions<SendGridOptions>`

### 3. **Uso no EmailService** (`src/Application/Services/EmailService.cs`)

```csharp
public EmailService(IOptions<SendGridOptions> options, ILogger<EmailService> logger)
{
    var sendGridOptions = options.Value;
    
    if (string.IsNullOrEmpty(sendGridOptions.ApiKey))
    {
        throw new InvalidOperationException("SendGrid API Key não configurada...");
    }
    
    _sendGridClient = new SendGridClient(sendGridOptions.ApiKey);
    _fromEmail = sendGridOptions.FromEmail ?? "noreply@menuadminapi.com";
    _fromName = sendGridOptions.FromName ?? "Menu Admin";
}
```

## 📁 Arquivos de Configuração

### `appsettings.json` (Padrão)

```json
{
  "SendGrid": {
    "ApiKey": "",
    "FromEmail": "noreply@menuadminapi.com",
    "FromName": "Menu Admin"
  }
}
```

### `appsettings.Development.json` (Desenvolvimento)

```json
{
  "SendGrid": {
    "ApiKey": "SG.YOUR_API_KEY_HERE",
    "FromEmail": "noreply@cafesenador.com.br",
    "FromName": "Menu Admin Platform"
  }
}
```

### `appsettings.Production.json` (Produção)

```json
{
  "SendGrid": {
    "ApiKey": "SG.YOUR_API_KEY_HERE",
    "FromEmail": "noreply@cafesenador.com.br",
    "FromName": "Menu Admin Platform"
  }
}
```

> **Nota:** Em produção, a chave de API deve ser configurada via **variáveis de ambiente** ou **Azure Key Vault**, nunca hardcoded.

## 🌍 Configuração em Diferentes Ambientes

### Desenvolvimento Local

#### Opção 1: Variáveis de Ambiente do Sistema

```bash
# Windows (PowerShell)
\[Environment\]::SetEnvironmentVariable("SendGrid__ApiKey", "SG.YOUR_API_KEY_HERE", "User")
\[Environment\]::SetEnvironmentVariable("SendGrid__FromEmail", "noreply@cafesenador.com.br", "User")
\[Environment\]::SetEnvironmentVariable("SendGrid__FromName", "Menu Admin Platform", "User")

# Linux/macOS
export SendGrid__ApiKey="SG.YOUR_API_KEY_HERE"
export SendGrid__FromEmail="noreply@cafesenador.com.br"
export SendGrid__FromName="Menu Admin Platform"
```

#### Opção 2: Arquivo `.env.local`

```
SendGrid__ApiKey=SG.YOUR_API_KEY_HERE
SendGrid__FromEmail=noreply@cafesenador.com.br
SendGrid__FromName=Menu Admin Platform
```

Adicione o código ao `Program.cs` para carregar o arquivo:

```csharp
var envFile = Path.Combine(AppContext.BaseDirectory, "..", "..", ".env.local");
if (File.Exists(envFile))
{
    foreach (var line in File.ReadAllLines(envFile))
    {
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            continue;

        var parts = line.Split('=', 2);
        if (parts.Length == 2)
        {
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
}
```

#### Opção 3: Arquivo `appsettings.Development.json`

O arquivo já está configurado com as credenciais de desenvolvimento.

### Produção (Azure App Service)

#### Passo 1: Configurar Variáveis no Azure Portal

1. Acesse: **Azure Portal** → **App Service** → **menu-api** → **Settings** → **Configuration**
2. Clique em **+ New application setting**
3. Adicione as variáveis:

| Name | Value |
|------|-------|
| `SendGrid:ApiKey` | `SG.YOUR_API_KEY_HERE` |
| `SendGrid:FromEmail` | `noreply@cafesenador.com.br` |
| `SendGrid:FromName` | `Menu Admin Platform` |

#### Passo 2: Usar Azure CLI

```bash
az webapp config appsettings set \
    --resource-group menu-admin-rg \
    --name menu-api \
    --settings SendGrid:ApiKey="SG.YOUR_API_KEY_HERE" \
                SendGrid:FromEmail="noreply@cafesenador.com.br" \
                SendGrid:FromName="Menu Admin Platform"
```

#### Passo 3: Reiniciar o App Service

```bash
az webapp restart --resource-group menu-admin-rg --name menu-api
```

## 🔄 Conversão Automática de Variáveis

O .NET Core converte automaticamente entre formatos:

| Formato | Contexto | Exemplo |
|---------|----------|---------|
| `__` (double underscore) | Variáveis de ambiente | `SendGrid__ApiKey` |
| `:` (colon) | Código C# e Azure Portal | `SendGrid:ApiKey` |
| Estrutura hierárquica | JSON | `"SendGrid": { "ApiKey": "..." }` |

**Fluxo de Conversão:**
```
Variável de Ambiente (SendGrid__ApiKey)
    ↓
.NET Configuration Provider
    ↓
Seção JSON (SendGrid:ApiKey)
    ↓
Código C# (configuration["SendGrid:ApiKey"])
```

## ✅ Verificar Configuração

### Desenvolvimento Local

```bash
# Verificar variáveis de ambiente
echo $SendGrid__ApiKey  # Linux/macOS
$env:SendGrid__ApiKey  # Windows PowerShell
```

### Produção (Azure)

```bash
# Listar variáveis configuradas
az webapp config appsettings list \
    --resource-group menu-admin-rg \
    --name menu-api \
    --query "[?name=='SendGrid:ApiKey' || name=='SendGrid:FromEmail' || name=='SendGrid:FromName']"
```

## 🧪 Testar Envio de Email

### 1. Iniciar a API

```bash
cd src/Presentation
dotnet run
```

### 2. Acessar Swagger

Abra: `http://localhost:5000/swagger`

### 3. Criar um Novo Usuário

1. Vá para **Usuarios** → **POST /api/usuarios**
2. Envie:

```json
{
  "nome": "João Silva",
  "email": "joao@example.com",
  "estabelecimentoId": 1,
  "role": "admin-estabelecimento"
}
```

### 4. Verificar Email

Verifique se o email foi recebido em `joao@example.com` com as credenciais de acesso.

## 🐛 Solucionar Problemas

### Erro: "SendGrid API Key não configurada"

**Causa:** A chave de API não foi configurada em nenhum dos ambientes.

**Solução:**
1. Verifique se a variável de ambiente foi configurada corretamente
2. Confirme se o formato é `SendGrid__ApiKey` (com `__`)
3. Reinicie a aplicação
4. Verifique os logs para mais detalhes

### Erro: "Failed to send email"

**Causa:** A chave de API é inválida ou o email de origem não está verificado.

**Solução:**
1. Verifique a chave de API em: https://app.sendgrid.com/settings/api_keys
2. Confirme se `noreply@cafesenador.com.br` está verificado como sender
3. Verifique os logs da aplicação

### Emails não são recebidos

**Causa:** O email está sendo bloqueado ou marcado como spam.

**Solução:**
1. Verifique a pasta de spam
2. Confirme se o email de origem está verificado no SendGrid
3. Verifique os logs de envio no SendGrid Dashboard

## 📚 Referências

- [SendGrid Documentation](https://docs.sendgrid.com/)
- [SendGrid C# Library](https://github.com/sendgrid/sendgrid-csharp)
- [Configuration in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration)
- [Options Pattern in .NET](https://learn.microsoft.com/en-us/dotnet/core/extensions/options)
- [Azure App Service Configuration](https://learn.microsoft.com/en-us/azure/app-service/configure-common)

---

**Última atualização:** 2026-02-11
