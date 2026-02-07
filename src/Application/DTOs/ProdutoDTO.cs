namespace MenuAdminAPI.Application.DTOs;

public record CriarProdutoRequest(
    Guid SubcategoriaId,
    string Nome,
    string Descricao,
    decimal Preco,
    string? ImagemUrl = null
);

public record AtualizarProdutoRequest(
    string Nome,
    string Descricao,
    decimal Preco,
    string? ImagemUrl = null
);

public record CriarVarianteRequest(
    Guid ProdutoId,
    string Nome,
    decimal PrecoAdicional
);

public record AtualizarVarianteRequest(
    string Nome,
    decimal PrecoAdicional
);

public record CriarAdicionalRequest(
    Guid ProdutoId,
    string Nome,
    decimal Preco
);

public record AtualizarAdicionalRequest(
    string Nome,
    decimal Preco
);

public record ProdutoResponse(
    Guid Id,
    Guid SubcategoriaId,
    string Nome,
    string Descricao,
    decimal Preco,
    string? ImagemUrl,
    bool Ativo,
    DateTime DataCriacao
);

public record VarianteResponse(
    Guid Id,
    Guid ProdutoId,
    string Nome,
    decimal PrecoAdicional,
    bool Ativo
);

public record AdicionalResponse(
    Guid Id,
    Guid ProdutoId,
    string Nome,
    decimal Preco,
    bool Ativo
);

public record ProdutoComDetalhesResponse(
    Guid Id,
    string Nome,
    string Descricao,
    decimal Preco,
    string? ImagemUrl,
    IEnumerable<VarianteResponse> Variantes,
    IEnumerable<AdicionalResponse> Adicionais
);
