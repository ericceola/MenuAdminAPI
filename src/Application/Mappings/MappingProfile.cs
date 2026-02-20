using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Domain.Entities;

namespace MenuAdminAPI.Application.Mappings;

/// <summary>
/// Perfil de mapeamento entre entidades e DTOs
/// </summary>
public static class MappingProfile
{
    // Estabelecimento
    public static EstabelecimentoResponse ToResponse(this Estabelecimento estabelecimento)
        => new(
            estabelecimento.Id,
            estabelecimento.Nome,
            estabelecimento.Email,
            estabelecimento.Telefone,
            estabelecimento.CNPJ,
            estabelecimento.Endereco,
            estabelecimento.Numero,
            estabelecimento.Complemento,
            estabelecimento.Bairro,
            estabelecimento.Cidade,
            estabelecimento.Estado,
            estabelecimento.CEP,
            estabelecimento.RazaoSocial,
            estabelecimento.NomeResponsavel,
            estabelecimento.TelefoneResponsavel,
            estabelecimento.EhMatriz,
            estabelecimento.TemFiliais,
            estabelecimento.MatrizId,
            estabelecimento.Plano,
            estabelecimento.Ativo,
            estabelecimento.DataCriacao,
            estabelecimento.DataAtualizacao,
            estabelecimento.Filiais?.Select(f => f.ToResponse()).ToList()
        );

    public static Estabelecimento ToEntity(this CriarEstabelecimentoRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Email = request.Email,
            Telefone = request.Telefone,
            CNPJ = request.CNPJ,
            Endereco = request.Endereco,
            Numero = request.Numero,
            Complemento = request.Complemento,
            Bairro = request.Bairro,
            Cidade = request.Cidade,
            Estado = request.Estado,
            CEP = request.CEP,
            RazaoSocial = request.RazaoSocial,
            NomeResponsavel = request.NomeResponsavel,
            TelefoneResponsavel = request.TelefoneResponsavel,
            EhMatriz = request.EhMatriz,
            TemFiliais = request.TemFiliais,
            MatrizId = request.MatrizId,
            Plano = request.Plano
        };

    public static Estabelecimento ToEntity(this AtualizarEstabelecimentoRequest request, Guid id)
        => new()
        {
            Id = id,
            Nome = request.Nome,
            Email = request.Email,
            Telefone = request.Telefone,
            CNPJ = request.CNPJ,
            Endereco = request.Endereco,
            Numero = request.Numero,
            Complemento = request.Complemento,
            Bairro = request.Bairro,
            Cidade = request.Cidade,
            Estado = request.Estado,
            CEP = request.CEP,
            RazaoSocial = request.RazaoSocial,
            NomeResponsavel = request.NomeResponsavel,
            TelefoneResponsavel = request.TelefoneResponsavel,
            EhMatriz = request.EhMatriz,
            TemFiliais = request.TemFiliais,
            MatrizId = request.MatrizId,
            Plano = request.Plano
        };

    // Usuario
    public static UsuarioResponse ToResponse(this Usuario usuario)
        => new(
            usuario.Id,
            usuario.EstabelecimentoId,
            usuario.Nome,
            usuario.Email,
            usuario.Perfil,
            usuario.Ativo,
            usuario.DataCriacao,
            usuario.UltimoAcesso
        );

    public static Usuario ToEntity(this CriarUsuarioRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            EstabelecimentoId = request.EstabelecimentoId,
            Nome = request.Nome,
            Email = request.Email,
            Perfil = request.Perfil
        };

    // Categoria
    public static CategoriaResponse ToResponse(this Categoria categoria)
        => new(
            categoria.Id,
            categoria.EstabelecimentoId,
            categoria.Nome,
            categoria.Emoji,
            categoria.Descricao,
            categoria.Ordem,
            categoria.Ativo,
            categoria.DataCriacao
        );

    public static Categoria ToEntity(this CriarCategoriaRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            EstabelecimentoId = request.EstabelecimentoId,
            Nome = request.Nome,
            Emoji = request.Emoji ?? "📦",
            Descricao = request.Descricao,
            Ordem = request.Ordem
        };

    // Subcategoria
    public static SubcategoriaResponse ToResponse(this Subcategoria subcategoria)
        => new(
            subcategoria.Id,
            subcategoria.CategoriaId,
            subcategoria.Nome,
            subcategoria.Descricao,
            subcategoria.Ordem,
            subcategoria.Ativo,
            subcategoria.DataCriacao
        );

    public static Subcategoria ToEntity(this CriarSubcategoriaRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            CategoriaId = request.CategoriaId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Ordem = request.Ordem
        };

    // Produto
    public static ProdutoResponse ToResponse(this Produto produto)
        => new(
            produto.Id,
            produto.SubcategoriaId,
            produto.Nome,
            produto.Descricao,
            produto.Preco,
            produto.ImagemUrl,
            produto.Ativo,
            produto.DataCriacao
        );

    public static Produto ToEntity(this CriarProdutoRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            SubcategoriaId = request.SubcategoriaId,
            Nome = request.Nome,
            Descricao = request.Descricao,
            Preco = request.Preco,
            ImagemUrl = request.ImagemUrl
        };

    // Variante
    public static VarianteResponse ToResponse(this Variante variante)
        => new(
            variante.Id,
            variante.ProdutoId,
            variante.Nome,
            variante.PrecoAdicional,
            variante.Ativo
        );

    public static Variante ToEntity(this CriarVarianteRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            ProdutoId = request.ProdutoId,
            Nome = request.Nome,
            PrecoAdicional = request.PrecoAdicional
        };

    // Adicional
    public static AdicionalResponse ToResponse(this Adicional adicional)
        => new(
            adicional.Id,
            adicional.ProdutoId,
            adicional.Nome,
            adicional.Preco,
            adicional.Ativo
        );

    public static Adicional ToEntity(this CriarAdicionalRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            ProdutoId = request.ProdutoId,
            Nome = request.Nome,
            Preco = request.Preco
        };

    // Cliente
    public static ClienteResponse ToResponse(this Cliente cliente)
        => new(
            cliente.Id,
            cliente.EstabelecimentoId,
            cliente.Nome,
            cliente.Email,
            cliente.Telefone,
            cliente.CPF,
            cliente.Ativo,
            cliente.DataCriacao
        );

    public static Cliente ToEntity(this CriarClienteRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            EstabelecimentoId = request.EstabelecimentoId,
            Nome = request.Nome,
            Email = request.Email,
            Telefone = request.Telefone,
            CPF = request.CPF
        };

    // Endereco
    public static EnderecoResponse ToResponse(this Endereco endereco)
        => new(
            endereco.Id,
            endereco.ClienteId,
            endereco.Rua,
            endereco.Numero,
            endereco.Complemento,
            endereco.Bairro,
            endereco.Cidade,
            endereco.Estado,
            endereco.CEP,
            endereco.Padrao,
            endereco.Ativo
        );

    public static Endereco ToEntity(this CriarEnderecoRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            ClienteId = request.ClienteId,
            Rua = request.Rua,
            Numero = request.Numero,
            Complemento = request.Complemento,
            Bairro = request.Bairro,
            Cidade = request.Cidade,
            Estado = request.Estado,
            CEP = request.CEP,
            Padrao = request.Padrao
        };

    // Pedido
    public static PedidoResponse ToResponse(this Pedido pedido)
        => new(
            pedido.Id,
            pedido.EstabelecimentoId,
            pedido.ClienteId,
            pedido.Numero,
            pedido.Status,
            pedido.Total,
            pedido.Observacoes,
            pedido.DataCriacao
        );

    public static Pedido ToEntity(this CriarPedidoRequest request)
        => new()
        {
            Id = Guid.NewGuid(),
            EstabelecimentoId = request.EstabelecimentoId,
            ClienteId = request.ClienteId,
            Numero = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper(),
            Observacoes = request.Observacoes
        };

    // ItemPedido
    public static ItemPedidoResponse ToResponse(this ItemPedido itemPedido)
        => new(
            itemPedido.Id,
            itemPedido.PedidoId,
            itemPedido.ProdutoId,
            itemPedido.Quantidade,
            itemPedido.PrecoUnitario,
            itemPedido.Subtotal,
            itemPedido.Observacoes
        );

    // AdicionalPedido
    public static AdicionalPedidoResponse ToResponse(this AdicionalPedido adicionalPedido)
        => new(
            adicionalPedido.Id,
            adicionalPedido.ItemPedidoId,
            adicionalPedido.AdicionalId,
            adicionalPedido.Quantidade,
            adicionalPedido.Preco,
            adicionalPedido.Subtotal
        );
}
