using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

public interface IProdutoVarianteService
{
    Task<IEnumerable<ProdutoVarianteResponse>> ObterPorProdutoAsync(Guid produtoId);
    Task<ProdutoVarianteResponse?> ObterPorIdAsync(Guid produtoId, Guid varianteId);
    Task<ProdutoVarianteResponse> CriarAsync(Guid produtoId, CriarProdutoVarianteRequest request);
    Task<ProdutoVarianteResponse> AtualizarAsync(Guid produtoId, Guid varianteId, AtualizarProdutoVarianteRequest request);
    Task DeletarAsync(Guid produtoId, Guid varianteId);
    Task AtivarAsync(Guid produtoId, Guid varianteId);
    Task DesativarAsync(Guid produtoId, Guid varianteId);
    Task<ProdutoVarianteResponse> AssociarAtributosAsync(Guid produtoId, Guid varianteId, AssociarAtributosVarianteRequest request);

    // Produto completo
    Task<ProdutoCompletoResponse?> ObterProdutoCompletoAsync(Guid produtoId);
    Task<ProdutoCompletoResponse> CriarProdutoCompletoAsync(CriarProdutoCompletoRequest request);
    Task<ProdutoCompletoResponse> AtualizarProdutoCompletoAsync(Guid produtoId, AtualizarProdutoCompletoRequest request);
}

public class ProdutoVarianteService : IProdutoVarianteService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoVarianteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private static ProdutoVarianteResponse MapVariante(ProdutoVariante v)
        => new(
            Id: v.Id,
            ProdutoId: v.ProdutoId,
            Nome: v.Nome,
            SKU: v.SKU,
            Preco: v.Preco,
            ImagemUrl: v.ImagemUrl,
            ImagemBlobName: v.ImagemBlobName,
            Ordem: v.Ordem,
            Status: v.Status,
            Ativo: v.Ativo,
            DataCriacao: v.DataCriacao,
            Atributos: v.Valores.Select(pvv => new ProdutoVarianteAtributoResponse(
                AtributoId: pvv.AtributoValor?.AtributoProdutoId ?? Guid.Empty,
                AtributoNome: pvv.AtributoValor?.Atributo?.Nome ?? "",
                AtributoValorId: pvv.AtributoProdutoValorId,
                AtributoValor: pvv.AtributoValor?.Valor ?? ""
            ))
        );

    private static ProdutoCompletoResponse MapProdutoCompleto(Domain.Entities.Produto p, IEnumerable<ProdutoVariante> variantes)
        => new(
            Id: p.Id,
            SubcategoriaId: p.SubcategoriaId,
            EstabelecimentoId: p.EstabelecimentoId,
            Nome: p.Nome,
            Descricao: p.Descricao,
            Preco: p.Preco,
            Ordem: p.Ordem,
            Status: p.Status,
            ImagemUrl: p.ImagemUrl,
            ImagemBlobName: p.ImagemBlobName,
            PossuiVariantes: p.PossuiVariantes,
            Ativo: p.Ativo,
            DataCriacao: p.DataCriacao,
            Variantes: variantes.Select(MapVariante)
        );

    public async Task<IEnumerable<ProdutoVarianteResponse>> ObterPorProdutoAsync(Guid produtoId)
    {
        var variantes = await _unitOfWork.ProdutoVariantes.ObterTodosComAtributosPorProdutoAsync(produtoId);
        return variantes.Select(MapVariante);
    }

    public async Task<ProdutoVarianteResponse?> ObterPorIdAsync(Guid produtoId, Guid varianteId)
    {
        var variante = await _unitOfWork.ProdutoVariantes.ObterComAtributosAsync(varianteId);
        if (variante == null || variante.ProdutoId != produtoId) return null;
        return MapVariante(variante);
    }

    public async Task<ProdutoVarianteResponse> CriarAsync(Guid produtoId, CriarProdutoVarianteRequest request)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(produtoId);
        if (produto == null)
            throw new InvalidOperationException($"Produto com ID {produtoId} não encontrado.");

        var atributoValorIds = request.AtributoValorIds ?? new List<Guid>();

        // Verificar duplicidade de combinação de atributos
        if (atributoValorIds.Any() &&
            await _unitOfWork.ProdutoVariantes.CombinacaoAtributosJaExisteAsync(produtoId, atributoValorIds))
            throw new InvalidOperationException("Já existe uma variante com esta combinação de atributos para este produto.");

        var variante = new ProdutoVariante
        {
            Id = Guid.NewGuid(),
            ProdutoId = produtoId,
            Nome = request.Nome.Trim(),
            SKU = request.SKU?.Trim(),
            Preco = request.Preco,
            ImagemUrl = request.ImagemUrl?.Trim(),
            ImagemBlobName = request.ImagemBlobName?.Trim(),
            Ordem = request.Ordem,
            Status = request.Status,
            Ativo = true,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        await _unitOfWork.ProdutoVariantes.AdicionarAsync(variante);

        // Associar atributos
        foreach (var atributoValorId in atributoValorIds)
        {
            var pvv = new ProdutoVarianteValor
            {
                Id = Guid.NewGuid(),
                ProdutoVarianteId = variante.Id,
                AtributoProdutoValorId = atributoValorId,
                DataCriacao = DateTime.UtcNow
            };
            await _unitOfWork.ProdutoVariantesValores.AdicionarAsync(pvv);
        }

        // Marcar produto como possuindo variantes
        if (!produto.PossuiVariantes)
        {
            produto.PossuiVariantes = true;
            produto.DataAtualizacao = DateTime.UtcNow;
            await _unitOfWork.Produtos.AtualizarAsync(produto);
        }

        await _unitOfWork.SaveChangesAsync();

        // Retornar com atributos carregados
        var varianteCompleta = await _unitOfWork.ProdutoVariantes.ObterComAtributosAsync(variante.Id);
        return MapVariante(varianteCompleta ?? variante);
    }

    public async Task<ProdutoVarianteResponse> AtualizarAsync(Guid produtoId, Guid varianteId, AtualizarProdutoVarianteRequest request)
    {
        var variante = await _unitOfWork.ProdutoVariantes.ObterPorIdAsync(varianteId);
        if (variante == null || variante.ProdutoId != produtoId)
            throw new InvalidOperationException($"Variante com ID {varianteId} não encontrada para o produto {produtoId}.");

        var atributoValorIds = request.AtributoValorIds ?? new List<Guid>();

        // Verificar duplicidade de combinação de atributos (excluindo a variante atual)
        if (atributoValorIds.Any() &&
            await _unitOfWork.ProdutoVariantes.CombinacaoAtributosJaExisteAsync(produtoId, atributoValorIds, varianteId))
            throw new InvalidOperationException("Já existe outra variante com esta combinação de atributos para este produto.");

        variante.Nome = request.Nome.Trim();
        variante.SKU = request.SKU?.Trim();
        variante.Preco = request.Preco;
        variante.ImagemUrl = request.ImagemUrl?.Trim();
        variante.ImagemBlobName = request.ImagemBlobName?.Trim();
        variante.Ordem = request.Ordem;
        variante.Status = request.Status;
        variante.Ativo = request.Ativo;
        variante.DataAtualizacao = DateTime.UtcNow;

        await _unitOfWork.ProdutoVariantes.AtualizarAsync(variante);

        // Atualizar atributos: deletar os existentes e recriar
        if (request.AtributoValorIds != null)
        {
            await _unitOfWork.ProdutoVariantesValores.DeletarPorVarianteAsync(varianteId);
            foreach (var atributoValorId in atributoValorIds)
            {
                var pvv = new ProdutoVarianteValor
                {
                    Id = Guid.NewGuid(),
                    ProdutoVarianteId = varianteId,
                    AtributoProdutoValorId = atributoValorId,
                    DataCriacao = DateTime.UtcNow
                };
                await _unitOfWork.ProdutoVariantesValores.AdicionarAsync(pvv);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        var varianteCompleta = await _unitOfWork.ProdutoVariantes.ObterComAtributosAsync(varianteId);
        return MapVariante(varianteCompleta ?? variante);
    }

    public async Task DeletarAsync(Guid produtoId, Guid varianteId)
    {
        var variante = await _unitOfWork.ProdutoVariantes.ObterPorIdAsync(varianteId);
        if (variante == null || variante.ProdutoId != produtoId)
            throw new InvalidOperationException($"Variante com ID {varianteId} não encontrada para o produto {produtoId}.");

        await _unitOfWork.ProdutoVariantes.DeletarAsync(varianteId);
        await _unitOfWork.SaveChangesAsync();

        // Verificar se o produto ainda tem variantes ativas
        var variantesAtivas = await _unitOfWork.ProdutoVariantes.ObterAtivosPorProdutoAsync(produtoId);
        if (!variantesAtivas.Any())
        {
            var produto = await _unitOfWork.Produtos.ObterPorIdAsync(produtoId);
            if (produto != null)
            {
                produto.PossuiVariantes = false;
                produto.DataAtualizacao = DateTime.UtcNow;
                await _unitOfWork.Produtos.AtualizarAsync(produto);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }

    public async Task AtivarAsync(Guid produtoId, Guid varianteId)
    {
        var variante = await _unitOfWork.ProdutoVariantes.ObterPorIdAsync(varianteId);
        if (variante == null || variante.ProdutoId != produtoId)
            throw new InvalidOperationException($"Variante com ID {varianteId} não encontrada para o produto {produtoId}.");

        await _unitOfWork.ProdutoVariantes.AtivarAsync(varianteId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DesativarAsync(Guid produtoId, Guid varianteId)
    {
        var variante = await _unitOfWork.ProdutoVariantes.ObterPorIdAsync(varianteId);
        if (variante == null || variante.ProdutoId != produtoId)
            throw new InvalidOperationException($"Variante com ID {varianteId} não encontrada para o produto {produtoId}.");

        await _unitOfWork.ProdutoVariantes.DesativarAsync(varianteId);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ProdutoVarianteResponse> AssociarAtributosAsync(Guid produtoId, Guid varianteId, AssociarAtributosVarianteRequest request)
    {
        var variante = await _unitOfWork.ProdutoVariantes.ObterPorIdAsync(varianteId);
        if (variante == null || variante.ProdutoId != produtoId)
            throw new InvalidOperationException($"Variante com ID {varianteId} não encontrada para o produto {produtoId}.");

        if (await _unitOfWork.ProdutoVariantes.CombinacaoAtributosJaExisteAsync(produtoId, request.AtributoValorIds, varianteId))
            throw new InvalidOperationException("Já existe outra variante com esta combinação de atributos.");

        await _unitOfWork.ProdutoVariantesValores.DeletarPorVarianteAsync(varianteId);
        foreach (var atributoValorId in request.AtributoValorIds)
        {
            var pvv = new ProdutoVarianteValor
            {
                Id = Guid.NewGuid(),
                ProdutoVarianteId = varianteId,
                AtributoProdutoValorId = atributoValorId,
                DataCriacao = DateTime.UtcNow
            };
            await _unitOfWork.ProdutoVariantesValores.AdicionarAsync(pvv);
        }

        await _unitOfWork.SaveChangesAsync();

        var varianteCompleta = await _unitOfWork.ProdutoVariantes.ObterComAtributosAsync(varianteId);
        return MapVariante(varianteCompleta ?? variante);
    }

    public async Task<ProdutoCompletoResponse?> ObterProdutoCompletoAsync(Guid produtoId)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(produtoId);
        if (produto == null) return null;

        var variantes = await _unitOfWork.ProdutoVariantes.ObterTodosComAtributosPorProdutoAsync(produtoId);
        return MapProdutoCompleto(produto, variantes);
    }

    public async Task<ProdutoCompletoResponse> CriarProdutoCompletoAsync(CriarProdutoCompletoRequest request)
    {
        var produto = new Domain.Entities.Produto
        {
            Id = Guid.NewGuid(),
            SubcategoriaId = request.SubcategoriaId,
            EstabelecimentoId = request.EstabelecimentoId,
            Nome = request.Nome.Trim(),
            Descricao = request.Descricao?.Trim(),
            Preco = request.Preco,
            Ordem = request.Ordem,
            Status = request.Status,
            ImagemUrl = request.ImagemUrl?.Trim(),
            ImagemBlobName = request.ImagemBlobName?.Trim(),
            PossuiVariantes = request.PossuiVariantes && (request.Variantes?.Any() ?? false),
            Ativo = true,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        await _unitOfWork.Produtos.AdicionarAsync(produto);

        var variantesCriadas = new List<ProdutoVariante>();

        if (request.Variantes != null && request.Variantes.Any())
        {
            foreach (var varianteReq in request.Variantes)
            {
                var variante = new ProdutoVariante
                {
                    Id = Guid.NewGuid(),
                    ProdutoId = produto.Id,
                    Nome = varianteReq.Nome.Trim(),
                    SKU = varianteReq.SKU?.Trim(),
                    Preco = varianteReq.Preco,
                    ImagemUrl = varianteReq.ImagemUrl?.Trim(),
                    ImagemBlobName = varianteReq.ImagemBlobName?.Trim(),
                    Ordem = varianteReq.Ordem,
                    Status = varianteReq.Status,
                    Ativo = true,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow,
                    Valores = new List<ProdutoVarianteValor>()
                };

                await _unitOfWork.ProdutoVariantes.AdicionarAsync(variante);

                if (varianteReq.AtributoValorIds != null)
                {
                    foreach (var atributoValorId in varianteReq.AtributoValorIds)
                    {
                        var pvv = new ProdutoVarianteValor
                        {
                            Id = Guid.NewGuid(),
                            ProdutoVarianteId = variante.Id,
                            AtributoProdutoValorId = atributoValorId,
                            DataCriacao = DateTime.UtcNow
                        };
                        await _unitOfWork.ProdutoVariantesValores.AdicionarAsync(pvv);
                    }
                }

                variantesCriadas.Add(variante);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        // Recarregar variantes com atributos
        var variantesCompletas = await _unitOfWork.ProdutoVariantes.ObterTodosComAtributosPorProdutoAsync(produto.Id);
        return MapProdutoCompleto(produto, variantesCompletas);
    }

    public async Task<ProdutoCompletoResponse> AtualizarProdutoCompletoAsync(Guid produtoId, AtualizarProdutoCompletoRequest request)
    {
        var produto = await _unitOfWork.Produtos.ObterPorIdAsync(produtoId);
        if (produto == null)
            throw new InvalidOperationException($"Produto com ID {produtoId} não encontrado.");

        produto.Nome = request.Nome.Trim();
        produto.Descricao = request.Descricao?.Trim();
        produto.Preco = request.Preco;
        produto.Ordem = request.Ordem;
        produto.Status = request.Status;
        produto.ImagemUrl = request.ImagemUrl?.Trim();
        produto.ImagemBlobName = request.ImagemBlobName?.Trim();
        produto.PossuiVariantes = request.PossuiVariantes && (request.Variantes?.Any() ?? false);
        produto.DataAtualizacao = DateTime.UtcNow;

        await _unitOfWork.Produtos.AtualizarAsync(produto);

        // Se foram enviadas variantes, substituir todas
        if (request.Variantes != null)
        {
            await _unitOfWork.ProdutoVariantes.DeletarPorProdutoAsync(produtoId);

            foreach (var varianteReq in request.Variantes)
            {
                var varianteId = Guid.NewGuid();
                var variante = new ProdutoVariante
                {
                    Id = varianteId,
                    ProdutoId = produtoId,
                    Nome = varianteReq.Nome.Trim(),
                    SKU = varianteReq.SKU?.Trim(),
                    Preco = varianteReq.Preco,
                    ImagemUrl = varianteReq.ImagemUrl?.Trim(),
                    ImagemBlobName = varianteReq.ImagemBlobName?.Trim(),
                    Ordem = varianteReq.Ordem,
                    Status = varianteReq.Status,
                    Ativo = varianteReq.Ativo,
                    DataCriacao = DateTime.UtcNow,
                    DataAtualizacao = DateTime.UtcNow
                };

                await _unitOfWork.ProdutoVariantes.AdicionarAsync(variante);

                if (varianteReq.AtributoValorIds != null)
                {
                    foreach (var atributoValorId in varianteReq.AtributoValorIds)
                    {
                        var pvv = new ProdutoVarianteValor
                        {
                            Id = Guid.NewGuid(),
                            ProdutoVarianteId = varianteId,
                            AtributoProdutoValorId = atributoValorId,
                            DataCriacao = DateTime.UtcNow
                        };
                        await _unitOfWork.ProdutoVariantesValores.AdicionarAsync(pvv);
                    }
                }
            }
        }

        await _unitOfWork.SaveChangesAsync();

        var variantesCompletas = await _unitOfWork.ProdutoVariantes.ObterTodosComAtributosPorProdutoAsync(produtoId);
        return MapProdutoCompleto(produto, variantesCompletas);
    }
}
