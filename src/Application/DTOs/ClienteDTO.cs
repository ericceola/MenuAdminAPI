namespace MenuAdminAPI.Application.DTOs;

public record CriarClienteRequest(
    Guid EstabelecimentoId,
    string Nome,
    string Email,
    string Telefone,
    string? CPF = null
);

public record AtualizarClienteRequest(
    string Nome,
    string Email,
    string Telefone,
    string? CPF = null
);

public record CriarEnderecoRequest(
    Guid ClienteId,
    string Rua,
    string Numero,
    string? Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string CEP,
    bool Padrao = false
);

public record AtualizarEnderecoRequest(
    string Rua,
    string Numero,
    string? Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string CEP
);

public record ClienteResponse(
    Guid Id = default,
    Guid EstabelecimentoId = default,
    string Nome = "",
    string Email = "",
    string Telefone = "",
    string? CPF = null,
    bool Ativo = false,
    DateTime DataCriacao = default
);

public record EnderecoResponse(
    Guid Id = default,
    Guid ClienteId = default,
    string Rua = "",
    string Numero = "",
    string? Complemento = null,
    string Bairro = "",
    string Cidade = "",
    string Estado = "",
    string CEP = "",
    bool Padrao = false,
    bool Ativo = false
);

public record ClienteComEndereçosResponse(
    Guid Id = default,
    string Nome = "",
    string Email = "",
    string Telefone = "",
    IEnumerable<EnderecoResponse> Enderecos = null!
);

public record ClienteComEstatisticasResponse(
    Guid Id = default,
    string Nome = "",
    string Email = "",
    int TotalPedidos = 0,
    decimal GastoTotal = 0m,
    decimal TicketMedio = 0m,
    DateTime? UltimoPedido = null
);
