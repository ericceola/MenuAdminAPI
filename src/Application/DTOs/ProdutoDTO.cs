namespace MenuAdminAPI.Application.DTOs;

public record CriarProdutoRequest(
    Guid SubcategoriaId,
    Guid EstabelecimentoId,
    string Nome,
    string Descricao,
    decimal Preco,
    int Ordem = 0,
    string Status = "ativo",
    string? ImagemUrl = null,
    string? ImagemBlobName = null,
    List<CriarVarianteDto>? Variantes = null,
    List<CriarAdicionalDto>? Adicionais = null
);

public record CriarVarianteDto(
    string Nome,
    decimal PrecoAdicional
);

public record CriarAdicionalDto(
    string Nome,
    decimal Preco
);

public record AtualizarProdutoRequest(
    string Nome,
    string Descricao,
    decimal Preco,
    int Ordem = 0,
    string Status = "ativo",
    string? ImagemUrl = null,
    string? ImagemBlobName = null
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
    Guid Id = default,
    Guid SubcategoriaId = default,
    Guid EstabelecimentoId = default,
    string Nome = "",
    string Descricao = "",
    decimal Preco = 0m,
    int Ordem = 0,
    string Status = "ativo",
    string? ImagemUrl = null,
    string? ImagemBlobName = null,
    bool Ativo = false,
    DateTime DataCriacao = default
);

public record VarianteResponse(
    Guid Id = default,
    Guid ProdutoId = default,
    string Nome = "",
    decimal PrecoAdicional = 0m,
    bool Ativo = false
);

public record AdicionalResponse(
    Guid Id = default,
    Guid ProdutoId = default,
    string Nome = "",
    decimal Preco = 0m,
    bool Ativo = false
);

public record ProdutoComDetalhesResponse(
    Guid Id,
    Guid EstabelecimentoId,
    string Nome,
    string Descricao,
    decimal Preco,
    int Ordem,
    string Status,
    string? ImagemUrl,
    string? ImagemBlobName,
    IEnumerable<VarianteResponse> Variantes,
    IEnumerable<AdicionalResponse> Adicionais
);
