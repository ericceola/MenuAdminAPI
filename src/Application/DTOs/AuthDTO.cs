namespace MenuAdminAPI.Application.DTOs;

public record LoginRequest(
    string Email,
    string Senha
);

public record LoginResponse(
    Guid UsuarioId,
    string Nome,
    string Email,
    string Perfil,
    Guid EstabelecimentoId,
    string Token,
    string RefreshToken,
    DateTime ExpiracaoToken
);

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);

public record RefreshTokenResponse(
    string Token,
    string RefreshToken,
    DateTime ExpiracaoToken
);

public record CriarUsuarioRequest(
    Guid EstabelecimentoId,
    string Nome,
    string Email,
    string Senha,
    string Perfil = "Gerente"
);

public record AtualizarUsuarioRequest(
    string Nome,
    string Email,
    string Perfil
);

public record UsuarioResponse(
    Guid Id,
    Guid EstabelecimentoId,
    string Nome,
    string Email,
    string Perfil,
    bool Ativo,
    DateTime DataCriacao,
    DateTime? UltimoAcesso
);

public record AlterarSenhaRequest(
    string SenhaAtual,
    string NovaSenha,
    string ConfirmacaoSenha
);

public record ResetarSenhaRequest(
    string Email
);

public record ConfirmarResetSenhaRequest(
    string Token,
    string NovaSenha
);
