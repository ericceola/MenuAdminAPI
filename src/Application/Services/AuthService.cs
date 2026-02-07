using MenuAdminAPI.Domain.Repositories;

namespace MenuAdminAPI.Application.Services;

public interface IAuthService { }

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}
