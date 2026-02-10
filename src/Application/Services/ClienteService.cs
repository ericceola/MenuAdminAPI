using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

public interface IClienteService { }

public class ClienteService : IClienteService
{
    private readonly IUnitOfWork _unitOfWork;

    public ClienteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
