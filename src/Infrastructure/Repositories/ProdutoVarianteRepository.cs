using System.Data;
using Dapper;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Infrastructure.Repositories;

/// <summary>
/// Repositório de ProdutoVariantes com Dapper
/// </summary>
public class ProdutoVarianteRepository : RepositoryBase<ProdutoVariante>, IProdutoVarianteRepository
{
    public ProdutoVarianteRepository(IDbConnection connection)
        : base(connection, "ProdutoVariantes")
    {
    }

    public async Task<IEnumerable<ProdutoVariante>> ObterPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            SELECT Id, ProdutoId, Nome, SKU, Preco, ImagemUrl, ImagemBlobName,
                   Ordem, Status, Ativo, DataCriacao, DataAtualizacao, DataExclusao
            FROM ProdutoVariantes
            WHERE ProdutoId = @ProdutoId
            ORDER BY Ordem, Nome";
        return await _connection.QueryAsync<ProdutoVariante>(sql, new { ProdutoId = produtoId }, transaction: _transaction);
    }

    public async Task<IEnumerable<ProdutoVariante>> ObterAtivosPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            SELECT Id, ProdutoId, Nome, SKU, Preco, ImagemUrl, ImagemBlobName,
                   Ordem, Status, Ativo, DataCriacao, DataAtualizacao, DataExclusao
            FROM ProdutoVariantes
            WHERE ProdutoId = @ProdutoId AND Ativo = 1 AND DataExclusao IS NULL
            ORDER BY Ordem, Nome";
        return await _connection.QueryAsync<ProdutoVariante>(sql, new { ProdutoId = produtoId }, transaction: _transaction);
    }

    public async Task<ProdutoVariante?> ObterComAtributosAsync(Guid id)
    {
        const string sql = @"
            SELECT
                v.Id, v.ProdutoId, v.Nome, v.SKU, v.Preco, v.ImagemUrl, v.ImagemBlobName,
                v.Ordem, v.Status, v.Ativo, v.DataCriacao, v.DataAtualizacao, v.DataExclusao,
                pvv.Id, pvv.ProdutoVarianteId, pvv.AtributoProdutoValorId, pvv.DataCriacao,
                apv.Id, apv.AtributoProdutoId, apv.Valor, apv.Ativo, apv.DataCriacao, apv.DataAtualizacao,
                ap.Id, ap.Nome, ap.Ativo, ap.DataCriacao, ap.DataAtualizacao
            FROM ProdutoVariantes v
            LEFT JOIN ProdutoVariantesValores pvv ON pvv.ProdutoVarianteId = v.Id
            LEFT JOIN AtributosProdutoValores apv ON apv.Id = pvv.AtributoProdutoValorId
            LEFT JOIN AtributosProduto ap ON ap.Id = apv.AtributoProdutoId
            WHERE v.Id = @Id";

        var varianteDict = new Dictionary<Guid, ProdutoVariante>();

        await _connection.QueryAsync<ProdutoVariante, ProdutoVarianteValor, AtributoProdutoValor, AtributoProduto, ProdutoVariante>(
            sql,
            (variante, pvv, apv, ap) =>
            {
                if (!varianteDict.TryGetValue(variante.Id, out var varianteEntry))
                {
                    varianteEntry = variante;
                    varianteEntry.Valores = new List<ProdutoVarianteValor>();
                    varianteDict[variante.Id] = varianteEntry;
                }
                if (pvv?.Id != Guid.Empty && pvv != null)
                {
                    if (apv != null)
                    {
                        apv.Atributo = ap;
                        pvv.AtributoValor = apv;
                    }
                    ((List<ProdutoVarianteValor>)varianteEntry.Valores).Add(pvv);
                }
                return varianteEntry;
            },
            new { Id = id },
            splitOn: "Id,Id,Id",
            transaction: _transaction
        );

        return varianteDict.Values.FirstOrDefault();
    }

    public async Task<IEnumerable<ProdutoVariante>> ObterTodosComAtributosPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            SELECT
                v.Id, v.ProdutoId, v.Nome, v.SKU, v.Preco, v.ImagemUrl, v.ImagemBlobName,
                v.Ordem, v.Status, v.Ativo, v.DataCriacao, v.DataAtualizacao, v.DataExclusao,
                pvv.Id, pvv.ProdutoVarianteId, pvv.AtributoProdutoValorId, pvv.DataCriacao,
                apv.Id, apv.AtributoProdutoId, apv.Valor, apv.Ativo, apv.DataCriacao, apv.DataAtualizacao,
                ap.Id, ap.Nome, ap.Ativo, ap.DataCriacao, ap.DataAtualizacao
            FROM ProdutoVariantes v
            LEFT JOIN ProdutoVariantesValores pvv ON pvv.ProdutoVarianteId = v.Id
            LEFT JOIN AtributosProdutoValores apv ON apv.Id = pvv.AtributoProdutoValorId
            LEFT JOIN AtributosProduto ap ON ap.Id = apv.AtributoProdutoId
            WHERE v.ProdutoId = @ProdutoId AND v.Ativo = 1 AND v.DataExclusao IS NULL
            ORDER BY v.Ordem, v.Nome";

        var varianteDict = new Dictionary<Guid, ProdutoVariante>();

        await _connection.QueryAsync<ProdutoVariante, ProdutoVarianteValor, AtributoProdutoValor, AtributoProduto, ProdutoVariante>(
            sql,
            (variante, pvv, apv, ap) =>
            {
                if (!varianteDict.TryGetValue(variante.Id, out var varianteEntry))
                {
                    varianteEntry = variante;
                    varianteEntry.Valores = new List<ProdutoVarianteValor>();
                    varianteDict[variante.Id] = varianteEntry;
                }
                if (pvv?.Id != Guid.Empty && pvv != null)
                {
                    if (apv != null)
                    {
                        apv.Atributo = ap;
                        pvv.AtributoValor = apv;
                    }
                    ((List<ProdutoVarianteValor>)varianteEntry.Valores).Add(pvv);
                }
                return varianteEntry;
            },
            new { ProdutoId = produtoId },
            splitOn: "Id,Id,Id",
            transaction: _transaction
        );

        return varianteDict.Values;
    }

    public async Task DeletarPorProdutoAsync(Guid produtoId)
    {
        const string sql = @"
            UPDATE ProdutoVariantes
            SET Ativo = 0, Status = 'inativo', DataExclusao = GETUTCDATE(), DataAtualizacao = GETUTCDATE()
            WHERE ProdutoId = @ProdutoId AND DataExclusao IS NULL";
        await _connection.ExecuteAsync(sql, new { ProdutoId = produtoId }, transaction: _transaction);
    }

    public async Task AtivarAsync(Guid id)
    {
        const string sql = @"
            UPDATE ProdutoVariantes
            SET Ativo = 1, Status = 'ativo', DataExclusao = NULL, DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task DesativarAsync(Guid id)
    {
        const string sql = @"
            UPDATE ProdutoVariantes
            SET Ativo = 0, Status = 'inativo', DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }

    public override async Task DeletarAsync(Guid id)
    {
        // Soft delete
        const string sql = @"
            UPDATE ProdutoVariantes
            SET Ativo = 0, Status = 'excluido', DataExclusao = GETUTCDATE(), DataAtualizacao = GETUTCDATE()
            WHERE Id = @Id";
        await _connection.ExecuteAsync(sql, new { Id = id }, transaction: _transaction);
    }

    public async Task<bool> CombinacaoAtributosJaExisteAsync(Guid produtoId, IEnumerable<Guid> atributoValorIds, Guid? varianteIdExcluir = null)
    {
        var ids = atributoValorIds.ToList();
        if (!ids.Any()) return false;

        // Buscar todas as variantes ativas do produto (exceto a que está sendo editada)
        const string sqlVariantes = @"
            SELECT v.Id
            FROM ProdutoVariantes v
            WHERE v.ProdutoId = @ProdutoId AND v.Ativo = 1 AND v.DataExclusao IS NULL
            AND (@VarianteIdExcluir IS NULL OR v.Id != @VarianteIdExcluir)";

        var variantesIds = (await _connection.QueryAsync<Guid>(sqlVariantes,
            new { ProdutoId = produtoId, VarianteIdExcluir = varianteIdExcluir },
            transaction: _transaction)).ToList();

        if (!variantesIds.Any()) return false;

        // Para cada variante existente, verificar se tem exatamente os mesmos atributos
        foreach (var varianteId in variantesIds)
        {
            const string sqlValores = @"
                SELECT AtributoProdutoValorId FROM ProdutoVariantesValores
                WHERE ProdutoVarianteId = @VarianteId";

            var valoresExistentes = (await _connection.QueryAsync<Guid>(sqlValores,
                new { VarianteId = varianteId },
                transaction: _transaction)).ToList();

            if (valoresExistentes.Count == ids.Count &&
                !valoresExistentes.Except(ids).Any() &&
                !ids.Except(valoresExistentes).Any())
            {
                return true; // Combinação duplicada encontrada
            }
        }

        return false;
    }
}
