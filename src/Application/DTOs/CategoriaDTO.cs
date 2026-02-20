namespace MenuAdminAPI.Application.DTOs;

public record CriarCategoriaRequest(
    Guid EstabelecimentoId,
    string Nome,
    string? Emoji,
    string Descricao,
    int Ordem
);

public record AtualizarCategoriaRequest(
    string Nome,
    string? Emoji,
    string Descricao,
    int Ordem
);

public record CriarSubcategoriaRequest(
    Guid CategoriaId,
    string Nome,
    string Descricao,
    int Ordem
);

public record AtualizarSubcategoriaRequest(
    string Nome,
    string Descricao,
    int Ordem
);

public record CategoriaResponse(
    Guid Id = default,
    Guid EstabelecimentoId = default,
    string Nome = "",
    string? Emoji = "📦",
    string Descricao = "",
    int Ordem = 0,
    bool Ativo = false,
    DateTime DataCriacao = default
);

public record SubcategoriaResponse(
    Guid Id = default,
    Guid CategoriaId = default,
    string Nome = "",
    string Descricao = "",
    int Ordem = 0,
    bool Ativo = false,
    DateTime DataCriacao = default
);

public record CategoriaComSubcategoriasResponse(
    Guid Id,
    string Nome,
    string? Emoji,
    string Descricao,
    IEnumerable<SubcategoriaResponse> Subcategorias
);
