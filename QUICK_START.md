# 🚀 Quick Start - MenuAdminAPI

## ⚡ Começar em 5 Minutos

### 1️⃣ Extrair o ZIP
```bash
unzip MenuAdminAPI_Solution.zip
cd MenuAdminAPI_Solution
```

### 2️⃣ Abrir no Visual Studio 2022
```bash
# Opção 1: Abrir arquivo de solução
start MenuAdminAPI.sln

# Opção 2: Abrir pasta
# File → Open Folder → Selecionar MenuAdminAPI_Solution
```

### 3️⃣ Restaurar Dependências
Visual Studio 2022 fará isso automaticamente, ou execute:
```bash
dotnet restore
```

### 4️⃣ Configurar Banco de Dados
1. Edite `src/Presentation/appsettings.json`
2. Atualize a connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=seu-servidor;Database=MenuAdminDB;User Id=sa;Password=sua-senha;Encrypt=false;"
}
```

### 5️⃣ Executar a Aplicação
- Pressione **F5** ou **Ctrl+F5** no Visual Studio
- Ou execute no terminal:
```bash
cd src/Presentation
dotnet run
```

### 6️⃣ Acessar Swagger
- Abra: `https://localhost:5001/swagger`
- Ou: `http://localhost:5000/swagger`

---

## 📋 Checklist de Validação

Após abrir no VS 2022:

- [ ] Todos os 8 projetos aparecem no Solution Explorer
- [ ] Nenhum ícone de erro nos projetos
- [ ] Build completa sem erros (Ctrl+Shift+B)
- [ ] Swagger abre em `https://localhost:5001/swagger`
- [ ] Banco de dados está acessível

---

## 🔧 Configurações Importantes

### JWT Secret (appsettings.json)
```json
"JwtSettings": {
  "Secret": "your-super-secret-key-that-must-be-at-least-32-characters-long",
  "Issuer": "MenuAdminAPI",
  "Audience": "MenuAdminAPI",
  "ExpirationMinutes": 60
}
```

### CORS Origins (appsettings.json)
```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:3000",
    "http://localhost:5173"
  ]
}
```

---

## 📁 Estrutura Rápida

```
MenuAdminAPI_Solution/
├── src/
│   ├── Domain/              # Entidades e interfaces
│   ├── Application/         # Services e DTOs
│   ├── Infrastructure/      # Repositórios e Dapper
│   └── Presentation/        # Controllers e API
├── tests/                   # Testes unitários
├── Database/                # Scripts SQL
└── docs/                    # Documentação
```

---

## 🎯 Próximos Passos

1. **Criar Entidades** em `src/Domain/Entities/`
2. **Criar Services** em `src/Application/Services/`
3. **Criar Controllers** em `src/Presentation/Controllers/`
4. **Executar Scripts SQL** em `Database/`
5. **Testar API** via Swagger

---

## ❓ Problemas Comuns

### "Projeto não carrega"
- Verifique se .NET 8.0 está instalado
- Execute: `dotnet restore`

### "Connection string error"
- Verifique `appsettings.json`
- Confirme que SQL Server está rodando

### "JWT error"
- Altere o JWT Secret em `appsettings.json`
- Deve ter pelo menos 32 caracteres

---

## 📞 Documentação Completa

- **ARCHITECTURE.md** - Arquitetura detalhada
- **VALIDATION_REPORT.md** - Relatório de validação
- **docs/ARCHITECTURE.md** - Documentação técnica
- **src/Infrastructure/README.md** - Documentação de repositórios

---

**Tudo pronto! Comece a desenvolver! 🎉**
