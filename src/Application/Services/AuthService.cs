using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Domain.Repositories;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace MenuAdminAPI.Application.Services;

/// <summary>
/// Interface para serviço de autenticação
/// </summary>
public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task<UsuarioResponse> ObterUsuarioAutenticadoAsync(Guid usuarioId);
    Task AlterarSenhaAsync(Guid usuarioId, AlterarSenhaRequest request);
    Task ResetarSenhaAsync(ResetarSenhaRequest request);
}

/// <summary>
/// Serviço de autenticação
/// </summary>
public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, IEmailService emailService, ILogger<AuthService> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

    /// <summary>
    /// Realiza login do usuário
    /// </summary>
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Senha))
            throw new ArgumentException("Email e senha são obrigatórios");

        // Buscar usuário pelo email
        var usuario = await _unitOfWork.Usuarios.ObterPorEmailAsync(request.Email);
        if (usuario == null)
        {
            _logger.LogWarning("Tentativa de login com email não encontrado: {Email}", request.Email);
            throw new InvalidOperationException("Email ou senha inválidos");
        }

        // Verificar se o usuário está ativo
        if (!usuario.Ativo)
        {
            _logger.LogWarning("Tentativa de login com usuário inativo: {Email}", request.Email);
            throw new InvalidOperationException("Usuário inativo");
        }

        // Verificar senha
        if (!VerificarSenha(request.Senha, usuario.Senha))
        {
            _logger.LogWarning("Tentativa de login com senha incorreta: {Email}", request.Email);
            throw new InvalidOperationException("Email ou senha inválidos");
        }

        // Atualizar último acesso
        await _unitOfWork.Usuarios.AtualizarUltimoAcessoAsync(usuario.Id);
        await _unitOfWork.SaveChangesAsync();

        // Gerar token JWT
        var token = _jwtService.GerarToken(usuario);
        var refreshToken = _jwtService.GerarRefreshToken();

        _logger.LogInformation("Login bem-sucedido para usuário: {Email}", request.Email);

        return new LoginResponse(
            UsuarioId: usuario.Id,
            Nome: usuario.Nome,
            Email: usuario.Email,
            Perfil: usuario.Perfil,
            EstabelecimentoId: usuario.EstabelecimentoId,
            Token: token,
            RefreshToken: refreshToken,
            ExpiracaoToken: DateTime.UtcNow.AddMinutes(60)
        );
    }

    /// <summary>
    /// Renova o token JWT
    /// </summary>
    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.RefreshToken))
            throw new ArgumentException("Token e refresh token são obrigatórios");

        // Validar token
        var principal = _jwtService.ValidarToken(request.Token);
        if (principal == null)
        {
            _logger.LogWarning("Tentativa de refresh com token inválido");
            throw new InvalidOperationException("Token inválido");
        }

        // Extrair ID do usuário do token
        var usuarioIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (usuarioIdClaim == null || !Guid.TryParse(usuarioIdClaim.Value, out var usuarioId))
            throw new InvalidOperationException("Token inválido");

        // Buscar usuário
        var usuario = await _unitOfWork.Usuarios.ObterPorIdAsync(usuarioId);
        if (usuario == null || !usuario.Ativo)
            throw new InvalidOperationException("Usuário não encontrado ou inativo");

        // Gerar novo token
        var novoToken = _jwtService.GerarToken(usuario);
        var novoRefreshToken = _jwtService.GerarRefreshToken();

        _logger.LogInformation("Token renovado para usuário: {UsuarioId}", usuarioId);

        return new RefreshTokenResponse(
            Token: novoToken,
            RefreshToken: novoRefreshToken,
            ExpiracaoToken: DateTime.UtcNow.AddMinutes(60)
        );
    }

    /// <summary>
    /// Obtém dados do usuário autenticado
    /// </summary>
    public async Task<UsuarioResponse> ObterUsuarioAutenticadoAsync(Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new ArgumentException("ID do usuário inválido");

        var usuario = await _unitOfWork.Usuarios.ObterPorIdAsync(usuarioId);
        if (usuario == null)
            throw new InvalidOperationException("Usuário não encontrado");

        return new UsuarioResponse(
            Id: usuario.Id,
            EstabelecimentoId: usuario.EstabelecimentoId,
            Nome: usuario.Nome,
            Email: usuario.Email,
            Perfil: usuario.Perfil,
            Ativo: usuario.Ativo,
            DataCriacao: usuario.DataCriacao,
            UltimoAcesso: usuario.UltimoAcesso
        );
    }

    /// <summary>
    /// Altera a senha do usuário autenticado
    /// </summary>
    public async Task AlterarSenhaAsync(Guid usuarioId, AlterarSenhaRequest request)
    {
        if (usuarioId == Guid.Empty)
            throw new ArgumentException("ID do usuário inválido");

        if (string.IsNullOrWhiteSpace(request.SenhaAtual) || 
            string.IsNullOrWhiteSpace(request.NovaSenha) || 
            string.IsNullOrWhiteSpace(request.ConfirmacaoSenha))
            throw new ArgumentException("Todos os campos de senha são obrigatórios");

        if (request.NovaSenha != request.ConfirmacaoSenha)
            throw new ArgumentException("Nova senha e confirmação não coincidem");

        if (request.NovaSenha.Length < 6)
            throw new ArgumentException("Nova senha deve ter pelo menos 6 caracteres");

        var usuario = await _unitOfWork.Usuarios.ObterPorIdAsync(usuarioId);
        if (usuario == null)
            throw new InvalidOperationException("Usuário não encontrado");

        // Verificar senha atual
        if (!VerificarSenha(request.SenhaAtual, usuario.Senha))
            throw new InvalidOperationException("Senha atual inválida");

        // Atualizar senha
        var novaSenhaHash = HashSenha(request.NovaSenha);
        await _unitOfWork.Usuarios.AtualizarSenhaAsync(usuario.Id, novaSenhaHash);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Senha alterada para usuário: {UsuarioId}", usuarioId);
    }

    /// <summary>
    /// Reseta a senha do usuário (gera uma nova senha temporária)
    /// </summary>
    public async Task ResetarSenhaAsync(ResetarSenhaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email é obrigatório");

        var usuario = await _unitOfWork.Usuarios.ObterPorEmailAsync(request.Email);
        if (usuario == null)
        {
            _logger.LogWarning("Tentativa de reset de senha com email não encontrado: {Email}", request.Email);
            throw new InvalidOperationException("Usuário não encontrado");
        }

        // Gerar nova senha temporária
        var novaSenhaTemporaria = GerarSenhaTemporaria();
        var novaSenhaHash = HashSenha(novaSenhaTemporaria);

        await _unitOfWork.Usuarios.AtualizarSenhaAsync(usuario.Id, novaSenhaHash);
        await _unitOfWork.SaveChangesAsync();

        // Enviar email com nova senha temporária
        await _emailService.SendPasswordResetAsync(usuario.Email, novaSenhaTemporaria);

        _logger.LogInformation("Senha resetada para usuário: {Email}", request.Email);
    }

    /// <summary>
    /// Verifica se a senha fornecida corresponde ao hash armazenado
    /// </summary>
    private bool VerificarSenha(string senha, string senhaArmazenada)
    {
        // Se a senha armazenada parece ser um hash (contém caracteres base64), comparar com hash
        // Caso contrário, comparar diretamente (para senhas inseridas como texto no banco)
        if (senhaArmazenada.Length > 20 && !senhaArmazenada.Contains(" "))
        {
            // Parece ser um hash, fazer comparação com hash
            var senhaHashCalculada = HashSenha(senha);
            return senhaHashCalculada == senhaArmazenada;
        }
        else
        {
            // Comparação direta (texto simples)
            return senha == senhaArmazenada;
        }
    }

    /// <summary>
    /// Gera hash da senha
    /// </summary>
    private string HashSenha(string senha)
    {
        // Implementação simples com SHA256 (em produção, usar bcrypt)
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    /// <summary>
    /// Gera uma senha temporária aleatória
    /// </summary>
    private string GerarSenhaTemporaria()
    {
        const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var senha = new StringBuilder();

        for (int i = 0; i < 8; i++)
        {
            senha.Append(caracteres[random.Next(caracteres.Length)]);
        }

        return senha.ToString();
    }
}
