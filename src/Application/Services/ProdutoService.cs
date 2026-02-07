using MenuAdminAPI.Application.DTOs;
using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

public interface IProdutoService { }

public class ProdutoService : IProdutoService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
