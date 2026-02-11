# Setup Ambiente de Desenvolvimento Local

Guia completo para configurar o ambiente de desenvolvimento local com SendGrid e outras dependências.

## 📋 Variáveis de Ambiente

**Formato para Variáveis de Ambiente (Recomendado):**

| Variável             | Valor                                                                |
| -------------------- | -------------------------------------------------------------------- |
| `SendGrid__ApiKey`   | `SG.YOUR_API_KEY_HERE` (veja SENDGRID_CONFIGURATION.md) |
| `SendGrid__FromEmail` | `noreply@cafesenador.com.br`                                         |
| `SendGrid__FromName` | `Menu Admin Platform`                                                |

**Formato para Azure Portal (Configuration):**

| Variável             | Valor                                                                |
| -------------------- | -------------------------------------------------------------------- |
| `SendGrid:ApiKey`    | `SG.YOUR_API_KEY_HERE` |
| `SendGrid:FromEmail` | `noreply@cafesenador.com.br`                                         |
| `SendGrid:FromName`  | `Menu Admin Platform`                                                |

> **Nota:** O .NET converte automaticamente `__` (double underscore) em `:` (colon) ao ler variáveis de ambiente. Use `__` para variáveis de ambiente padrão, e `:` apenas no Azure Portal.

---

## 🚀 Opção 1: Usando Arquivo `.env.local` (Recomendado)

### Passo 1: Arquivo Já Existe

O arquivo `.env.local` já foi criado na raiz do projeto com todas as variáveis necessárias.

### Passo 2: Configurar o Projeto para Ler `.env.local`

Abra `src/Presentation/Program.cs` e adicione o seguinte código **antes** de `builder.Build()`:

```csharp
// Carregar variáveis de ambiente do arquivo .env.local
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

### Passo 3: Executar a Aplicação

```bash
cd src/Presentation
dotnet run
```

---

## 🖥️ Opção 2: Variáveis de Ambiente do Sistema

### Windows (PowerShell como Administrador)

```powershell
# Executar o script de setup
.\scripts\setup-dev-env-windows.ps1
```

Ou configurar manualmente:

```powershell
[Environment]::SetEnvironmentVariable("SendGrid__ApiKey", "SG.YOUR_API_KEY_HERE", "User")
[Environment]::SetEnvironmentVariable("SendGrid__FromEmail", "noreply@cafesenador.com.br", "User")
[Environment]::SetEnvironmentVariable("SendGrid__FromName", "Menu Admin Platform", "User")
```

### Linux / macOS (Terminal)

```bash
# Executar o script de setup
./scripts/setup-dev-env-linux.sh

# Recarregar o shell
source ~/.bashrc  # ou ~/.zshrc
```

Ou configurar manualmente:

```bash
export SendGrid__ApiKey="SG.YOUR_API_KEY_HERE"
export SendGrid__FromEmail="noreply@cafesenador.com.br"
export SendGrid__FromName="Menu Admin Platform"
```

---

## 🔧 Opção 3: Arquivo `appsettings.Development.json`

### Passo 1: Editar Arquivo

Abra `src/Presentation/appsettings.Development.json` e adicione:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.AspNetCore": "Information"
    }
  },
  "SendGrid": {
    "ApiKey": "SG.YOUR_API_KEY_HERE",
    "FromEmail": "noreply@cafesenador.com.br",
    "FromName": "Menu Admin Platform"
  },
  "Jwt": {
    "Secret": "your-super-secret-key-that-must-be-at-least-32-characters-long-for-security",
    "Issuer": "MenuAdminAPI",
    "Audience": "MenuAdminAPI",
    "ExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

### ⚠️ Importante

**NUNCA commite `appsettings.Development.json` com dados sensíveis!**

Adicione ao `.gitignore`:

```
appsettings.Development.json
.env.local
```

---

## 📦 Pré-requisitos

### Instalações Necessárias

1. **.NET 8.0 SDK**
   ```bash
   # Windows
   choco install dotnet-sdk-8.0
   
   # macOS
   brew install dotnet-sdk@8.0
   
   # Linux
   # Seguir instruções em: https://learn.microsoft.com/en-us/dotnet/core/install/linux
   ```

2. **SQL Server / Azure SQL Database**
   - Já configurado: `cafe-senador.database.windows.net`
   - Credenciais: `CoffeeAdmin` / `CoffeeCeola@123`

3. **SendGrid Account**
   - Conta já criada e configurada
   - API Key: `SG.YOUR_API_KEY_HERE`

---

## 🧪 Testando a Configuração

### 1. Verificar Variáveis de Ambiente

**Windows (PowerShell):**
```powershell
$env:SendGrid__ApiKey
$env:SendGrid__FromEmail
$env:SendGrid__FromName
```

**Linux / macOS:**
```bash
echo $SendGrid__ApiKey
echo $SendGrid__FromEmail
echo $SendGrid__FromName
```

### 2. Executar a Aplicação

```bash
cd src/Presentation
dotnet run
```

Você deve ver:
```
info: MenuAdminAPI.Presentation.Program[0]
      Application started. Press Ctrl+C to exit.
```

### 3. Testar Envio de Email

1. Acesse: `http://localhost:5000/swagger`
2. Vá para **Auth** → **POST /api/auth/login**
3. Faça login com:
   - Email: `admin@menuplatform.com`
   - Password: `admin123`
4. Crie um novo usuário via **Usuarios** → **POST /api/usuarios**
5. Verifique se o email foi enviado para o endereço configurado

---

## 🐛 Solucionar Problemas

### Erro: "SendGrid API Key não configurada"

**Causa:** Variáveis de ambiente não foram carregadas.

**Solução:**
1. Verifique se o arquivo `.env.local` existe na raiz do projeto
2. Confirme se as variáveis foram adicionadas ao `Program.cs`
3. Reinicie a aplicação
4. Se usar PowerShell, confirme se é Administrador

### Erro: "Failed to send email"

**Causa:** Credenciais SendGrid inválidas ou email não verificado.

**Solução:**
1. Verifique a chave de API em: https://app.sendgrid.com/settings/api_keys
2. Confirme se `noreply@cafesenador.com.br` está verificado como sender
3. Verifique os logs da aplicação para mais detalhes

### Erro: "Connection timeout"

**Causa:** Banco de dados não acessível.

**Solução:**
1. Verifique a string de conexão em `appsettings.json`
2. Confirme se você tem acesso ao Azure SQL Database
3. Teste a conexão com: `sqlcmd -S cafe-senador.database.windows.net -U CoffeeAdmin -P CoffeeCeola@123 -d MenuDB`

---

## 📚 Estrutura de Arquivos

```
MenuAdminAPI_Solution/
├── .env.local                          ← Variáveis de ambiente
├── .env.example                        ← Exemplo de arquivo .env
├── SETUP_LOCAL_DEV.md                  ← Este arquivo
├── scripts/
│   ├── setup-dev-env-windows.ps1       ← Script para Windows (PowerShell)
│   ├── setup-dev-env-windows.bat       ← Script para Windows (CMD)
│   ├── setup-dev-env-linux.sh          ← Script para Linux/macOS
│   └── README.md                       ← Documentação dos scripts
├── src/
│   ├── Presentation/
│   │   ├── Program.cs                  ← Adicionar carregamento de .env.local
│   │   ├── appsettings.json            ← Configurações gerais
│   │   └── appsettings.Development.json ← Configurações de desenvolvimento
│   ├── Application/
│   │   └── Services/
│   │       └── EmailService.cs         ← Serviço que usa SendGrid
│   └── ...
└── ...
```

---

## ✅ Checklist de Setup

- [ ] .NET 8.0 SDK instalado
- [ ] Arquivo `.env.local` criado na raiz do projeto
- [ ] Variáveis de ambiente configuradas (via script ou manualmente)
- [ ] Acesso ao Azure SQL Database verificado
- [ ] Chave de API SendGrid validada
- [ ] Aplicação iniciada com `dotnet run`
- [ ] Swagger acessível em `http://localhost:5000/swagger`
- [ ] Email de teste enviado com sucesso

---

## 🔗 Referências

- [.NET 8.0 Documentation](https://learn.microsoft.com/en-us/dotnet/core/)
- [SendGrid Documentation](https://docs.sendgrid.com/)
- [Azure SQL Database](https://learn.microsoft.com/en-us/azure/azure-sql/database/)
- [Environment Variables in .NET](https://learn.microsoft.com/en-us/dotnet/api/system.environment.getenvironmentvariable)
- [Configuration in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration)
