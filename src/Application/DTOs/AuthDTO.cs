namespace MenuAdminAPI.Application.DTOs;

public record LoginRequest(
    string Email,
    string Senha
);

public record LoginResponse(
    Guid UsuarioId = default,
    string Nome = "",
    string Email = "",
    string Perfil = "",
    Guid EstabelecimentoId = default,
    string Token = "",
    string RefreshToken = "",
    DateTime ExpiracaoToken = default
);

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);

public record RefreshTokenResponse(
    string Token = "",
    string RefreshToken = "",
    DateTime ExpiracaoToken = default
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
    Guid Id = default,
    Guid EstabelecimentoId = default,
    string Nome = "",
    string Email = "",
    string Perfil = "",
    bool Ativo = false,
    DateTime DataCriacao = default,
    DateTime? UltimoAcesso = null
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
