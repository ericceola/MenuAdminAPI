using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

public interface ISubcategoriaService { }

public class SubcategoriaService : ISubcategoriaService
{
    private readonly IUnitOfWork _unitOfWork;

    public SubcategoriaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
