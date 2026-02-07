namespace MenuAdminAPI.Application.DTOs;

public record CriarEstabelecimentoRequest(
    string Nome,
    string Email,
    string Telefone,
    string CNPJ,
    string Endereco,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string CEP,
    string Plano = "Básico"
);

public record AtualizarEstabelecimentoRequest(
    string Nome,
    string Email,
    string Telefone,
    string CNPJ,
    string Endereco,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string CEP,
    string Plano
);

public record EstabelecimentoResponse(
    Guid Id,
    string Nome,
    string Email,
    string Telefone,
    string CNPJ,
    string Endereco,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string CEP,
    string Plano,
    bool Ativo,
    DateTime DataCriacao,
    DateTime DataAtualizacao
);

public record EstabelecimentoComEstatisticasResponse(
    Guid Id,
    string Nome,
    string Email,
    int TotalUsuarios,
    int TotalProdutos,
    int TotalClientes,
    int TotalPedidos,
    decimal ReceitaTotal,
    bool Ativo
);
