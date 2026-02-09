# Deployment e CI/CD - MenuAdminAPI

## 📋 Visão Geral

Este documento descreve o processo de deployment e integração contínua (CI/CD) da API MenuAdminAPI.

## 🚀 GitHub Actions - Publicação Automática

A API é publicada automaticamente através do GitHub Actions sempre que há um push na branch `main`.

### Workflow: `publish-api.yml`

**Localização:** `.github/workflows/publish-api.yml`

**Acionadores:**
- Push na branch `main`
- Pull Request para a branch `main`

### Etapas do Workflow

1. **Checkout do Código**
   - Faz checkout do repositório

2. **Setup .NET 8.0**
   - Configura o ambiente .NET 8.0

3. **Restaurar Dependências**
   - Executa `dotnet restore`

4. **Build**
   - Compila a solução em modo Release
   - Comando: `dotnet build --configuration Release`

5. **Executar Testes**
   - Executa testes unitários (se existirem)
   - Comando: `dotnet test --configuration Release`

6. **Publicar API**
   - Publica a API em modo Release
   - Saída: `./publish`
   - Comando: `dotnet publish src/Presentation/MenuAdminAPI.Presentation.csproj`

7. **Commit e Push Automático**
   - Faz commit das mudanças de build
   - Push automático para o repositório

8. **Criar Release**
   - Cria uma nova release no GitHub
   - Tag: `v{numero_da_build}`
   - Inclui informações do commit e autor

9. **Upload de Artefatos**
   - Faz upload dos arquivos publicados
   - Retenção: 30 dias

## 📦 Artefatos

Os artefatos publicados estão disponíveis em:
- **GitHub Actions Artifacts:** Aba "Actions" → Workflow → Download dos artefatos
- **GitHub Releases:** Aba "Releases" → Versão específica

## 🔧 Configuração

### Variáveis de Ambiente

As seguintes variáveis podem ser configuradas nos Secrets do GitHub:

- `GITHUB_TOKEN` - Token automático do GitHub (fornecido automaticamente)

### Secrets Necessários

Nenhum secret adicional é necessário para o workflow padrão.

## 📝 Versioning

As releases seguem o padrão:
- **Tag:** `v{numero_da_build}`
- **Exemplo:** `v1`, `v2`, `v3`, etc.

## 🔍 Monitoramento

Para monitorar o status do workflow:

1. Acesse o repositório no GitHub
2. Clique na aba "Actions"
3. Selecione o workflow "Publicar API MenuAdminAPI"
4. Verifique o status das execuções

## ❌ Troubleshooting

### Workflow falha no build

1. Verifique se todas as dependências estão instaladas
2. Verifique se o arquivo `.csproj` está correto
3. Verifique os logs do workflow

### Workflow falha no teste

1. Verifique se os testes estão passando localmente
2. Execute: `dotnet test --configuration Release`

### Workflow falha no push

1. Verifique se o `GITHUB_TOKEN` tem permissões corretas
2. Verifique se a branch está protegida

## 📚 Referências

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [.NET CLI Documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/)
- [GitHub Releases API](https://docs.github.com/en/rest/releases)

## 🤝 Contribuindo

Ao fazer push para a branch `main`, o workflow será acionado automaticamente. Aguarde a conclusão antes de fazer merge de pull requests.

---

**Última atualização:** 2026-02-09
**Versão:** 1.0
