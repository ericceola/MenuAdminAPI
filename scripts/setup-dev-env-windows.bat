@echo off
REM ============================================
REM Setup Development Environment - Windows (CMD)
REM ============================================
REM Script para configurar variáveis de ambiente SendGrid em desenvolvimento local
REM Uso: setup-dev-env-windows.bat

setlocal enabledelayedexpansion

echo.
echo ================================================
echo Setup SendGrid Development Environment (Windows)
echo ================================================
echo.

REM Verificar se está rodando como administrador
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo.
    echo [ERROR] Este script precisa ser executado como Administrador!
    echo Por favor, execute o CMD como Administrador e tente novamente.
    echo.
    pause
    exit /b 1
)

echo [INFO] Configurando variáveis de ambiente...
echo.

REM Configurar SendGrid__ApiKey
REM NOTA: Substitua SG.YOUR_API_KEY_HERE pela sua chave de API do SendGrid
setx SendGrid__ApiKey "SG.YOUR_API_KEY_HERE"
if %errorLevel% equ 0 (
    echo [OK] SendGrid__ApiKey configurada
) else (
    echo [ERROR] Erro ao configurar SendGrid__ApiKey
)

REM Configurar SendGrid__FromEmail
setx SendGrid__FromEmail "noreply@cafesenador.com.br"
if %errorLevel% equ 0 (
    echo [OK] SendGrid__FromEmail configurada
) else (
    echo [ERROR] Erro ao configurar SendGrid__FromEmail
)

REM Configurar SendGrid__FromName
setx SendGrid__FromName "Menu Admin Platform"
if %errorLevel% equ 0 (
    echo [OK] SendGrid__FromName configurada
) else (
    echo [ERROR] Erro ao configurar SendGrid__FromName
)

echo.
echo ================================================
echo [SUCCESS] Configuracao Concluida!
echo ================================================
echo.
echo [INFO] Variaveis Configuradas:
echo   \* SendGrid__ApiKey = SG.YOUR_API_KEY_HERE
echo   \* SendGrid__FromEmail = noreply@cafesenador.com.br
echo   \* SendGrid__FromName = Menu Admin Platform
echo.
echo [WARNING] IMPORTANTE:
echo   1. Feche e reabra o Visual Studio / Terminal para aplicar as mudancas
echo   2. Ou use o arquivo .env.local na raiz do projeto
echo   3. Reinicie o IIS Express ou dotnet run
echo.
echo [INFO] Para Testar:
echo   1. Execute: dotnet run
echo   2. Faca login na API
echo   3. Verifique se recebeu o email com as credenciais
echo.
pause
