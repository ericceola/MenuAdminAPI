#!/bin/bash

# ============================================
# Setup Development Environment - Linux/macOS
# ============================================
# Script para configurar variáveis de ambiente SendGrid em desenvolvimento local
# Uso: ./setup-dev-env-linux.sh

echo "================================================"
echo "Setup SendGrid Development Environment"
echo "================================================"
echo ""

# Detectar o shell e o arquivo de configuração
if [ -n "$ZSH_VERSION" ]; then
    SHELL_CONFIG="$HOME/.zshrc"
    SHELL_NAME="zsh"
elif [ -n "$BASH_VERSION" ]; then
    SHELL_CONFIG="$HOME/.bashrc"
    SHELL_NAME="bash"
else
    SHELL_CONFIG="$HOME/.profile"
    SHELL_NAME="sh"
fi

echo "🔍 Shell detectado: $SHELL_NAME"
echo "📁 Arquivo de configuração: $SHELL_CONFIG"
echo ""

# Variáveis de Ambiente
# NOTA: Substitua SG.YOUR_API_KEY_HERE pela sua chave de API do SendGrid
declare -A ENV_VARS=(
    ["SendGrid__ApiKey"]="SG.YOUR_API_KEY_HERE"
    ["SendGrid__FromEmail"]="noreply@cafesenador.com.br"
    ["SendGrid__FromName"]="Menu Admin Platform"
)

echo "🔧 Configurando variáveis de ambiente..."
echo ""

# Criar backup do arquivo de configuração
if [ -f "$SHELL_CONFIG" ]; then
    cp "$SHELL_CONFIG" "$SHELL_CONFIG.backup"
    echo "📦 Backup criado: $SHELL_CONFIG.backup"
fi

# Adicionar variáveis ao arquivo de configuração
{
    echo ""
    echo "# ============================================"
    echo "# SendGrid Configuration (Added by setup script)"
    echo "# ============================================"
    for key in "${!ENV_VARS[@]}"; do
        value="${ENV_VARS[$key]}"
        echo "export $key='$value'"
    done
} >> "$SHELL_CONFIG"

echo "✅ Variáveis adicionadas ao $SHELL_CONFIG"
echo ""

# Exportar variáveis para a sessão atual
for key in "${!ENV_VARS[@]}"; do
    value="${ENV_VARS[$key]}"
    export "$key"="$value"
    echo "✅ $key = $value"
done

echo ""
echo "================================================"
echo "✅ Configuração Concluída!"
echo "================================================"
echo ""
echo "📋 Variáveis Configuradas:"
echo "  • SendGrid__ApiKey = SG.YOUR_API_KEY_HERE"
echo "  • SendGrid__FromEmail = noreply@cafesenador.com.br"
echo "  • SendGrid__FromName = Menu Admin Platform"
echo ""
echo "⚠️  IMPORTANTE:"
echo "  1. Recarregue o shell: source $SHELL_CONFIG"
echo "  2. Ou feche e reabra o terminal"
echo "  3. Ou use o arquivo .env.local na raiz do projeto"
echo ""
echo "🧪 Para Testar:"
echo "  1. Execute: dotnet run"
echo "  2. Faça login na API"
echo "  3. Verifique se recebeu o email com as credenciais"
echo ""
echo "💡 Para Recarregar Agora:"
echo "  source $SHELL_CONFIG"
echo ""
