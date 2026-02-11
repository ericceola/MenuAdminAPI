# ============================================
# Setup Development Environment - Windows
# ============================================
# Script para configurar variáveis de ambiente SendGrid em desenvolvimento local (Windows)
# Uso: .\setup-dev-env-windows.ps1

Write-Host "================================================" -ForegroundColor Cyan
Write-Host "Setup SendGrid Development Environment (Windows)" -ForegroundColor Cyan
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""

# Verificar se está rodando como administrador
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")

if (-not $isAdmin) {
    Write-Host "⚠️  Este script precisa ser executado como Administrador!" -ForegroundColor Yellow
    Write-Host "Por favor, execute o PowerShell como Administrador e tente novamente." -ForegroundColor Yellow
    exit 1
}

# Variáveis de Ambiente
# NOTA: Substitua SG.YOUR_API_KEY_HERE pela sua chave de API do SendGrid
$envVars = @{
    "SendGrid__ApiKey"    = "SG.YOUR_API_KEY_HERE"
    "SendGrid__FromEmail" = "noreply@cafesenador.com.br"
    "SendGrid__FromName"  = "Menu Admin Platform"
}

Write-Host "🔧 Configurando variáveis de ambiente..." -ForegroundColor Green
Write-Host ""

foreach ($key in $envVars.Keys) {
    $value = $envVars[$key]
    
    try {
        [Environment]::SetEnvironmentVariable($key, $value, "User")
        Write-Host "✅ $key = $value" -ForegroundColor Green
    }
    catch {
        Write-Host "❌ Erro ao configurar $key : $_" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "================================================" -ForegroundColor Cyan
Write-Host "✅ Configuração Concluída!" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📋 Variáveis Configuradas:" -ForegroundColor Yellow
Write-Host "  • SendGrid__ApiKey = SG.YOUR_API_KEY_HERE" -ForegroundColor White
Write-Host "  • SendGrid__FromEmail = noreply@cafesenador.com.br" -ForegroundColor White
Write-Host "  • SendGrid__FromName = Menu Admin Platform" -ForegroundColor White
Write-Host ""
Write-Host "⚠️  IMPORTANTE:" -ForegroundColor Yellow
Write-Host "  1. Feche e reabra o Visual Studio / Terminal para aplicar as mudanças" -ForegroundColor White
Write-Host "  2. Ou use o arquivo .env.local na raiz do projeto" -ForegroundColor White
Write-Host "  3. Reinicie o IIS Express ou dotnet run" -ForegroundColor White
Write-Host ""
Write-Host "🧪 Para Testar:" -ForegroundColor Yellow
Write-Host "  1. Execute: dotnet run" -ForegroundColor White
Write-Host "  2. Faça login na API" -ForegroundColor White
Write-Host "  3. Verifique se recebeu o email com as credenciais" -ForegroundColor White
Write-Host ""
