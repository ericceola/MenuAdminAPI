namespace MenuAdminAPI.Application.DTOs;

public record CriarCategoriaRequest(
    Guid EstabelecimentoId,
    string Nome,
    string Descricao,
    int Ordem
);

public record AtualizarCategoriaRequest(
    string Nome,
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
    Guid Id,
    Guid EstabelecimentoId,
    string Nome,
    string Descricao,
    int Ordem,
    bool Ativo,
    DateTime DataCriacao
);

public record SubcategoriaResponse(
    Guid Id,
    Guid CategoriaId,
    string Nome,
    string Descricao,
    int Ordem,
    bool Ativo,
    DateTime DataCriacao
);

public record CategoriaComSubcategoriasResponse(
    Guid Id,
    string Nome,
    string Descricao,
    IEnumerable<SubcategoriaResponse> Subcategorias
);
