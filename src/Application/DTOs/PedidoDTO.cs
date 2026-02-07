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
    Guid Id,
    Guid EstabelecimentoId,
    Guid ClienteId,
    string Numero,
    string Status,
    decimal Total,
    string? Observacoes,
    DateTime DataCriacao
);

public record ItemPedidoResponse(
    Guid Id,
    Guid PedidoId,
    Guid ProdutoId,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal,
    string? Observacoes
);

public record AdicionalPedidoResponse(
    Guid Id,
    Guid ItemPedidoId,
    Guid AdicionalId,
    int Quantidade,
    decimal Preco,
    decimal Subtotal
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
    int TotalPedidos,
    int Pendentes,
    int Confirmados,
    int Entregues,
    int Cancelados,
    decimal ReceitaTotal,
    decimal TicketMedio
);
