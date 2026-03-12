using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Domain.Entities;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

public interface IAtributoProdutoService
{
    Task<IEnumerable<AtributoProdutoResponse>> ObterTodosAsync(bool comValores = false);
    Task<AtributoProdutoResponse?> ObterPorIdAsync(Guid id, bool comValores = false);
    Task<AtributoProdutoResponse> CriarAsync(CriarAtributoProdutoRequest request);
    Task<AtributoProdutoResponse> AtualizarAsync(Guid id, AtualizarAtributoProdutoRequest request);
    Task DeletarAsync(Guid id);

    // Valores
    Task<IEnumerable<AtributoProdutoValorResponse>> ObterValoresPorAtributoAsync(Guid atributoProdutoId);
    Task<AtributoProdutoValorResponse> CriarValorAsync(CriarAtributoProdutoValorRequest request);
    Task<AtributoProdutoValorResponse> AtualizarValorAsync(Guid id, AtualizarAtributoProdutoValorRequest request);
    Task DeletarValorAsync(Guid id);
}

public class AtributoProdutoService : IAtributoProdutoService
{
    private readonly IUnitOfWork _unitOfWork;

    public AtributoProdutoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    private static AtributoProdutoResponse MapAtributo(AtributoProduto a, bool comValores = false)
        => new(
            Id: a.Id,
            Nome: a.Nome,
            Ativo: a.Ativo,
            DataCriacao: a.DataCriacao,
            Valores: comValores
                ? a.Valores.Select(v => new AtributoProdutoValorResponse(
                    v.Id, v.AtributoProdutoId, a.Nome, v.Valor, v.Ativo, v.DataCriacao))
                : null
        );

    private static AtributoProdutoValorResponse MapValor(AtributoProdutoValor v, string atributoNome = "")
        => new(v.Id, v.AtributoProdutoId, atributoNome, v.Valor, v.Ativo, v.DataCriacao);

    public async Task<IEnumerable<AtributoProdutoResponse>> ObterTodosAsync(bool comValores = false)
    {
        if (comValores)
        {
            var atributos = await _unitOfWork.AtributosProduto.ObterTodosComValoresAsync();
            return atributos.Select(a => MapAtributo(a, true));
        }
        else
        {
            var atributos = await _unitOfWork.AtributosProduto.ObterAtivosAsync();
            return atributos.Select(a => MapAtributo(a, false));
        }
    }

    public async Task<AtributoProdutoResponse?> ObterPorIdAsync(Guid id, bool comValores = false)
    {
        if (comValores)
        {
            var atributo = await _unitOfWork.AtributosProduto.ObterComValoresAsync(id);
            return atributo == null ? null : MapAtributo(atributo, true);
        }
        else
        {
            var atributo = await _unitOfWork.AtributosProduto.ObterPorIdAsync(id);
            return atributo == null ? null : MapAtributo(atributo, false);
        }
    }

    public async Task<AtributoProdutoResponse> CriarAsync(CriarAtributoProdutoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("Nome do atributo é obrigatório.");

        if (await _unitOfWork.AtributosProduto.NomeJaExisteAsync(request.Nome.Trim()))
            throw new InvalidOperationException($"Já existe um atributo com o nome '{request.Nome}'.");

        var atributo = new AtributoProduto
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome.Trim(),
            Ativo = true,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        await _unitOfWork.AtributosProduto.AdicionarAsync(atributo);
        await _unitOfWork.SaveChangesAsync();

        return MapAtributo(atributo);
    }

    public async Task<AtributoProdutoResponse> AtualizarAsync(Guid id, AtualizarAtributoProdutoRequest request)
    {
        var atributo = await _unitOfWork.AtributosProduto.ObterPorIdAsync(id);
        if (atributo == null)
            throw new InvalidOperationException($"Atributo com ID {id} não encontrado.");

        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new ArgumentException("Nome do atributo é obrigatório.");

        if (await _unitOfWork.AtributosProduto.NomeJaExisteAsync(request.Nome.Trim(), id))
            throw new InvalidOperationException($"Já existe outro atributo com o nome '{request.Nome}'.");

        atributo.Nome = request.Nome.Trim();
        atributo.Ativo = request.Ativo;
        atributo.DataAtualizacao = DateTime.UtcNow;

        await _unitOfWork.AtributosProduto.AtualizarAsync(atributo);
        await _unitOfWork.SaveChangesAsync();

        return MapAtributo(atributo);
    }

    public async Task DeletarAsync(Guid id)
    {
        var atributo = await _unitOfWork.AtributosProduto.ObterPorIdAsync(id);
        if (atributo == null)
            throw new InvalidOperationException($"Atributo com ID {id} não encontrado.");

        atributo.Ativo = false;
        atributo.DataAtualizacao = DateTime.UtcNow;
        await _unitOfWork.AtributosProduto.AtualizarAsync(atributo);
        await _unitOfWork.SaveChangesAsync();
    }

    // ---- Valores ----

    public async Task<IEnumerable<AtributoProdutoValorResponse>> ObterValoresPorAtributoAsync(Guid atributoProdutoId)
    {
        var atributo = await _unitOfWork.AtributosProduto.ObterPorIdAsync(atributoProdutoId);
        var atributoNome = atributo?.Nome ?? "";

        var valores = await _unitOfWork.AtributosProdutoValores.ObterAtivosPorAtributoAsync(atributoProdutoId);
        return valores.Select(v => MapValor(v, atributoNome));
    }

    public async Task<AtributoProdutoValorResponse> CriarValorAsync(CriarAtributoProdutoValorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Valor))
            throw new ArgumentException("Valor é obrigatório.");

        var atributo = await _unitOfWork.AtributosProduto.ObterPorIdAsync(request.AtributoProdutoId);
        if (atributo == null)
            throw new InvalidOperationException($"Atributo com ID {request.AtributoProdutoId} não encontrado.");

        if (await _unitOfWork.AtributosProdutoValores.ValorJaExisteAsync(request.Valor.Trim(), request.AtributoProdutoId))
            throw new InvalidOperationException($"Já existe o valor '{request.Valor}' para este atributo.");

        var valor = new AtributoProdutoValor
        {
            Id = Guid.NewGuid(),
            AtributoProdutoId = request.AtributoProdutoId,
            Valor = request.Valor.Trim(),
            Ativo = true,
            DataCriacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow
        };

        await _unitOfWork.AtributosProdutoValores.AdicionarAsync(valor);
        await _unitOfWork.SaveChangesAsync();

        return MapValor(valor, atributo.Nome);
    }

    public async Task<AtributoProdutoValorResponse> AtualizarValorAsync(Guid id, AtualizarAtributoProdutoValorRequest request)
    {
        var valor = await _unitOfWork.AtributosProdutoValores.ObterPorIdAsync(id);
        if (valor == null)
            throw new InvalidOperationException($"Valor com ID {id} não encontrado.");

        if (string.IsNullOrWhiteSpace(request.Valor))
            throw new ArgumentException("Valor é obrigatório.");

        if (await _unitOfWork.AtributosProdutoValores.ValorJaExisteAsync(request.Valor.Trim(), valor.AtributoProdutoId, id))
            throw new InvalidOperationException($"Já existe outro valor '{request.Valor}' para este atributo.");

        var atributo = await _unitOfWork.AtributosProduto.ObterPorIdAsync(valor.AtributoProdutoId);

        valor.Valor = request.Valor.Trim();
        valor.Ativo = request.Ativo;
        valor.DataAtualizacao = DateTime.UtcNow;

        await _unitOfWork.AtributosProdutoValores.AtualizarAsync(valor);
        await _unitOfWork.SaveChangesAsync();

        return MapValor(valor, atributo?.Nome ?? "");
    }

    public async Task DeletarValorAsync(Guid id)
    {
        var valor = await _unitOfWork.AtributosProdutoValores.ObterPorIdAsync(id);
        if (valor == null)
            throw new InvalidOperationException($"Valor com ID {id} não encontrado.");

        valor.Ativo = false;
        valor.DataAtualizacao = DateTime.UtcNow;
        await _unitOfWork.AtributosProdutoValores.AtualizarAsync(valor);
        await _unitOfWork.SaveChangesAsync();
    }
}
