namespace MenuAdminAPI.Application.DTOs;

// =====================================================================
// DTOs de AtributosProduto
// =====================================================================

public record CriarAtributoProdutoRequest(
    string Nome
);

public record AtualizarAtributoProdutoRequest(
    string Nome,
    bool Ativo = true
);

public record AtributoProdutoResponse(
    Guid Id,
    string Nome,
    bool Ativo,
    DateTime DataCriacao,
    IEnumerable<AtributoProdutoValorResponse>? Valores = null
);

// =====================================================================
// DTOs de AtributosProdutoValores
// =====================================================================

public record CriarAtributoProdutoValorRequest(
    Guid AtributoProdutoId,
    string Valor
);

public record AtualizarAtributoProdutoValorRequest(
    string Valor,
    bool Ativo = true
);

public record AtributoProdutoValorResponse(
    Guid Id,
    Guid AtributoProdutoId,
    string AtributoNome,
    string Valor,
    bool Ativo,
    DateTime DataCriacao
);

// =====================================================================
// DTOs de ProdutoVariantes
// =====================================================================

public record CriarProdutoVarianteRequest(
    string Nome,
    decimal Preco,
    string? SKU = null,
    string? ImagemUrl = null,
    string? ImagemBlobName = null,
    int Ordem = 0,
    string Status = "ativo",
    List<Guid>? AtributoValorIds = null
);

public record AtualizarProdutoVarianteRequest(
    string Nome,
    decimal Preco,
    string? SKU = null,
    string? ImagemUrl = null,
    string? ImagemBlobName = null,
    int Ordem = 0,
    string Status = "ativo",
    bool Ativo = true,
    List<Guid>? AtributoValorIds = null
);

public record ProdutoVarianteAtributoResponse(
    Guid AtributoId,
    string AtributoNome,
    Guid AtributoValorId,
    string AtributoValor
);

public record ProdutoVarianteResponse(
    Guid Id,
    Guid ProdutoId,
    string Nome,
    string? SKU,
    decimal Preco,
    string? ImagemUrl,
    string? ImagemBlobName,
    int Ordem,
    string Status,
    bool Ativo,
    DateTime DataCriacao,
    IEnumerable<ProdutoVarianteAtributoResponse> Atributos
);

// =====================================================================
// DTOs de ProdutoVariantesValores (vínculo)
// =====================================================================

public record AssociarAtributosVarianteRequest(
    List<Guid> AtributoValorIds
);

// =====================================================================
// DTOs compostos (produto completo com variantes)
// =====================================================================

public record CriarProdutoCompletoRequest(
    Guid SubcategoriaId,
    Guid EstabelecimentoId,
    string Nome,
    string? Descricao,
    decimal Preco,
    int Ordem = 0,
    string Status = "ativo",
    string? ImagemUrl = null,
    string? ImagemBlobName = null,
    bool PossuiVariantes = false,
    List<CriarProdutoVarianteRequest>? Variantes = null
);

public record AtualizarProdutoCompletoRequest(
    string Nome,
    string? Descricao,
    decimal Preco,
    int Ordem = 0,
    string Status = "ativo",
    string? ImagemUrl = null,
    string? ImagemBlobName = null,
    bool PossuiVariantes = false,
    List<AtualizarProdutoVarianteRequest>? Variantes = null
);

public record ProdutoCompletoResponse(
    Guid Id,
    Guid SubcategoriaId,
    Guid EstabelecimentoId,
    string Nome,
    string? Descricao,
    decimal Preco,
    int Ordem,
    string Status,
    string? ImagemUrl,
    string? ImagemBlobName,
    bool PossuiVariantes,
    bool Ativo,
    DateTime DataCriacao,
    IEnumerable<ProdutoVarianteResponse> Variantes
);

// =====================================================================
// DTO de paginação genérico
// =====================================================================

public record PagedRequest(
    int Pagina = 1,
    int TamanhoPagina = 20
);

public record PagedResponse<T>(
    IEnumerable<T> Itens,
    int Total,
    int Pagina,
    int TamanhoPagina,
    int TotalPaginas
);
