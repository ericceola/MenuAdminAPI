using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

public interface IUsuarioService { }

public class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;

    public UsuarioService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
