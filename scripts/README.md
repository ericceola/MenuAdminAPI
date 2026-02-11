# Scripts de Setup do Ambiente de Desenvolvimento

Este diretório contém scripts para configurar as variáveis de ambiente necessárias para desenvolvimento local.

## 📋 Arquivos Disponíveis

### 1. `setup-dev-env-windows.ps1` (PowerShell)

**Plataforma:** Windows (PowerShell)

**Requisitos:**
- PowerShell 5.0+
- Executar como Administrador

**Uso:**
```powershell
# Abra PowerShell como Administrador e execute:
.\setup-dev-env-windows.ps1
```

**O que faz:**
- Configura variáveis de ambiente do sistema (User scope)
- Valida permissões de administrador
- Exibe confirmação de cada variável configurada

---

### 2. `setup-dev-env-windows.bat` (CMD)

**Plataforma:** Windows (Command Prompt)

**Requisitos:**
- Windows 7+
- Executar como Administrador

**Uso:**
```cmd
REM Abra CMD como Administrador e execute:
setup-dev-env-windows.bat
```

**O que faz:**
- Configura variáveis de ambiente do sistema (User scope)
- Valida permissões de administrador
- Exibe confirmação de cada variável configurada

---

### 3. `setup-dev-env-linux.sh` (Bash/Zsh)

**Plataforma:** Linux / macOS

**Requisitos:**
- Bash ou Zsh
- Permissão de escrita no diretório home

**Uso:**
```bash
# Torne o script executável (primeira vez)
chmod +x setup-dev-env-linux.sh

# Execute o script
./setup-dev-env-linux.sh

# Recarregue o shell
source ~/.bashrc  # ou ~/.zshrc
```

**O que faz:**
- Detecta o shell em uso (bash, zsh, sh)
- Cria backup do arquivo de configuração
- Adiciona variáveis de ambiente ao arquivo de configuração
- Exporta variáveis para a sessão atual

---

## 🚀 Guia Rápido

### Windows (PowerShell)

```powershell
# 1. Abra PowerShell como Administrador
# 2. Navegue até o diretório scripts
cd C:\caminho\para\MenuAdminAPI_Solution\scripts

# 3. Execute o script
.\setup-dev-env-windows.ps1

# 4. Feche e reabra o Visual Studio ou Terminal
```

### Windows (CMD)

```cmd
REM 1. Abra CMD como Administrador
REM 2. Navegue até o diretório scripts
cd C:\caminho\para\MenuAdminAPI_Solution\scripts

REM 3. Execute o script
setup-dev-env-windows.bat

REM 4. Feche e reabra o Visual Studio ou Terminal
```

### Linux / macOS

```bash
# 1. Abra o Terminal
# 2. Navegue até o diretório scripts
cd /caminho/para/MenuAdminAPI_Solution/scripts

# 3. Torne o script executável
chmod +x setup-dev-env-linux.sh

# 4. Execute o script
./setup-dev-env-linux.sh

# 5. Recarregue o shell
source ~/.bashrc  # ou ~/.zshrc
```

---

## 📋 Variáveis Configuradas

Todos os scripts configuram as seguintes variáveis **com formato `__` (double underscore)**:

| Variável             | Valor                                                                |
| -------------------- | -------------------------------------------------------------------- |
| `SendGrid__ApiKey`   | `SG.YOUR_API_KEY_HERE` (veja SENDGRID_CONFIGURATION.md) |
| `SendGrid__FromEmail` | `noreply@cafesenador.com.br`                                         |
| `SendGrid__FromName` | `Menu Admin Platform`                                                |

> **Nota:** O .NET converte automaticamente `__` (double underscore) em `:` (colon) ao ler variáveis de ambiente. Este é o formato recomendado para compatibilidade com todos os ambientes.

---

## 🔄 Alternativas

### Opção 1: Usar Arquivo `.env.local`

Recomendado para desenvolvimento rápido:

1. O arquivo `.env.local` já existe na raiz do projeto
2. Configure o `Program.cs` para carregar o arquivo (ver `SETUP_LOCAL_DEV.md`)
3. Execute: `dotnet run`

### Opção 2: Configurar Manualmente

**Windows (PowerShell):**
```powershell
[Environment]::SetEnvironmentVariable("SendGrid__ApiKey", "SG.YOUR_API_KEY_HERE", "User")
[Environment]::SetEnvironmentVariable("SendGrid__FromEmail", "noreply@cafesenador.com.br", "User")
[Environment]::SetEnvironmentVariable("SendGrid__FromName", "Menu Admin Platform", "User")
```

**Linux / macOS:**
```bash
export SendGrid__ApiKey="SG.YOUR_API_KEY_HERE"
export SendGrid__FromEmail="noreply@cafesenador.com.br"
export SendGrid__FromName="Menu Admin Platform"
```

---

## 🧪 Verificar Configuração

### Windows (PowerShell)

```powershell
# Verificar variáveis configuradas
$env:SendGrid__ApiKey
$env:SendGrid__FromEmail
$env:SendGrid__FromName
```

### Linux / macOS

```bash
# Verificar variáveis configuradas
echo $SendGrid__ApiKey
echo $SendGrid__FromEmail
echo $SendGrid__FromName
```

---

## 🐛 Solucionar Problemas

### "Acesso Negado" (Windows)

**Causa:** Script não foi executado como Administrador

**Solução:**
1. Clique com botão direito no PowerShell/CMD
2. Selecione "Executar como Administrador"
3. Execute o script novamente

### "Comando não encontrado" (Linux/macOS)

**Causa:** Script não tem permissão de execução

**Solução:**
```bash
chmod +x setup-dev-env-linux.sh
./setup-dev-env-linux.sh
```

### Variáveis não aparecem após reiniciar

**Causa:** Arquivo de configuração não foi recarregado

**Solução:**
```bash
# Recarregue o shell
source ~/.bashrc  # ou ~/.zshrc

# Ou feche e reabra o terminal
```

---

## 📚 Referências

- [SETUP_LOCAL_DEV.md](../SETUP_LOCAL_DEV.md) - Guia completo de setup
- [.env.local](../.env.local) - Arquivo de variáveis de ambiente
- [.env.example](../.env.example) - Exemplo de arquivo .env

---

## ✅ Próximos Passos

Após executar um dos scripts:

1. Feche e reabra o Visual Studio / Terminal
2. Execute: `dotnet run`
3. Acesse: `http://localhost:5000/swagger`
4. Teste criando um novo usuário para verificar se o email é enviado

---

**Última atualização:** 2026-02-11
