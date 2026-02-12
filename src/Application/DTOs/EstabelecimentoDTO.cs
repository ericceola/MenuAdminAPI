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
    string RazaoSocial,
    string NomeResponsavel,
    string TelefoneResponsavel,
    bool EhMatriz = false,
    bool TemFiliais = false,
    Guid? MatrizId = null,
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
    string RazaoSocial,
    string NomeResponsavel,
    string TelefoneResponsavel,
    bool EhMatriz,
    bool TemFiliais,
    Guid? MatrizId,
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
    string RazaoSocial,
    string NomeResponsavel,
    string TelefoneResponsavel,
    bool EhMatriz,
    bool TemFiliais,
    Guid? MatrizId,
    string Plano,
    bool Ativo,
    DateTime DataCriacao,
    DateTime DataAtualizacao,
    List<EstabelecimentoResponse>? Filiais = null
);

public record EstabelecimentoComEstatisticasResponse(
    Guid Id,
    string Nome,
    string Email,
    string RazaoSocial,
    string NomeResponsavel,
    string TelefoneResponsavel,
    bool EhMatriz,
    bool TemFiliais,
    Guid? MatrizId,
    int TotalUsuarios,
    int TotalProdutos,
    int TotalClientes,
    int TotalPedidos,
    decimal ReceitaTotal,
    bool Ativo,
    List<EstabelecimentoResponse>? Filiais = null
);
