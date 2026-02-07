namespace MenuAdminAPI.Application.DTOs;

public record ApiResponse<T>(
    bool Sucesso,
    string Mensagem,
    T? Dados = default,
    IEnumerable<string>? Erros = null
)
{
    public static ApiResponse<T> Ok(T dados, string mensagem = "Operação realizada com sucesso")
        => new(true, mensagem, dados);

    public static ApiResponse<T> Erro(string mensagem, IEnumerable<string>? erros = null)
        => new(false, mensagem, default, erros);

    public static ApiResponse<T> ErroValidacao(IEnumerable<string> erros)
        => new(false, "Erros de validação", default, erros);
}

public record PaginatedResponse<T>(
    IEnumerable<T> Dados,
    int PaginaAtual,
    int TamanhoPagina,
    int TotalRegistros,
    int TotalPaginas
)
{
    public bool TemProxima => PaginaAtual < TotalPaginas;
    public bool TemAnterior => PaginaAtual > 1;
}

public record ErrorResponse(
    int StatusCode,
    string Mensagem,
    string? Detalhes = null,
    DateTime Timestamp = default
)
{
    public ErrorResponse(int statusCode, string mensagem, string? detalhes = null)
        : this(statusCode, mensagem, detalhes, DateTime.UtcNow)
    {
    }
}

public record CriacaoResponse(
    Guid Id,
    string Mensagem = "Recurso criado com sucesso"
);

public record AtualizacaoResponse(
    Guid Id,
    string Mensagem = "Recurso atualizado com sucesso"
);

public record DelecaoResponse(
    Guid Id,
    string Mensagem = "Recurso deletado com sucesso"
);
