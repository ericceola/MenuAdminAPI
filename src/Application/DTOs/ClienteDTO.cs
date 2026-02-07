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
    Guid Id,
    Guid EstabelecimentoId,
    string Nome,
    string Email,
    string Telefone,
    string? CPF,
    bool Ativo,
    DateTime DataCriacao
);

public record EnderecoResponse(
    Guid Id,
    Guid ClienteId,
    string Rua,
    string Numero,
    string? Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string CEP,
    bool Padrao,
    bool Ativo
);

public record ClienteComEndereçosResponse(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    IEnumerable<EnderecoResponse> Enderecos
);

public record ClienteComEstatisticasResponse(
    Guid Id,
    string Nome,
    string Email,
    int TotalPedidos,
    decimal GastoTotal,
    decimal TicketMedio,
    DateTime? UltimoPedido
);
