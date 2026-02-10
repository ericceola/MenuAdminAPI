namespace MenuAdminAPI.Application.DTOs;

public record CriarPedidoRequest(
    Guid EstabelecimentoId,
    Guid ClienteId,
    string? Observacoes = null
);

public record AdicionarItemPedidoRequest(
    Guid PedidoId,
    Guid ProdutoId,
    int Quantidade,
    string? Observacoes = null
);

public record AdicionarAdicionalAoItemRequest(
    Guid ItemPedidoId,
    Guid AdicionalId,
    int Quantidade
);

public record MudarStatusPedidoRequest(
    Guid PedidoId,
    string NovoStatus
);

public record CancelarPedidoRequest(
    Guid PedidoId,
    string Motivo
);

public record PedidoResponse(
    Guid Id = default,
    Guid EstabelecimentoId = default,
    Guid ClienteId = default,
    string Numero = "",
    string Status = "",
    decimal Total = 0m,
    string? Observacoes = null,
    DateTime DataCriacao = default
);

public record ItemPedidoResponse(
    Guid Id = default,
    Guid PedidoId = default,
    Guid ProdutoId = default,
    int Quantidade = 0,
    decimal PrecoUnitario = 0m,
    decimal Subtotal = 0m,
    string? Observacoes = null
);

public record AdicionalPedidoResponse(
    Guid Id = default,
    Guid ItemPedidoId = default,
    Guid AdicionalId = default,
    int Quantidade = 0,
    decimal Preco = 0m,
    decimal Subtotal = 0m
);

public record ItemPedidoComAdicionaisResponse(
    Guid Id,
    Guid ProdutoId,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal,
    IEnumerable<AdicionalPedidoResponse> Adicionais
);

public record PedidoComItensResponse(
    Guid Id,
    string Numero,
    string Status,
    decimal Total,
    IEnumerable<ItemPedidoComAdicionaisResponse> Itens,
    DateTime DataCriacao
);

public record PedidoEstatisticasResponse(
    int TotalPedidos = 0,
    int Pendentes = 0,
    int Confirmados = 0,
    int Entregues = 0,
    int Cancelados = 0,
    decimal ReceitaTotal = 0m,
    decimal TicketMedio = 0m
);
