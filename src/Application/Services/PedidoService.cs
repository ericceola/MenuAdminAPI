using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

public interface IPedidoService { }

public class PedidoService : IPedidoService
{
    private readonly IUnitOfWork _unitOfWork;

    public PedidoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
