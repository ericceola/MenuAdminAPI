using MenuAdminAPI.Application.Services;
using MenuAdminAPI.Domain.Repositories;
using MenuAdminAPI.Infrastructure.Data;
using MenuAdminAPI.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MenuAdminAPI.Infrastructure.Configuration;

/// <summary>
/// Configuração de injeção de dependência para a camada de Infraestrutura
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registrar serviços de infraestrutura
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string não pode ser vazia", nameof(connectionString));

        // Registrar Unit of Work
        services.AddScoped<IUnitOfWork>(_ => new UnitOfWork(connectionString));

        // Registrar Repositórios (através do Unit of Work)
        services.AddScoped<IEstabelecimentoRepository>(sp => 
            sp.GetRequiredService<IUnitOfWork>().Estabelecimentos);

        services.AddScoped<IProdutoRepository>(sp => 
            sp.GetRequiredService<IUnitOfWork>().Produtos);

        services.AddScoped<ICategoriaRepository>(sp => 
            sp.GetRequiredService<IUnitOfWork>().Categorias);

        services.AddScoped<ISubcategoriaRepository>(sp => 
            sp.GetRequiredService<IUnitOfWork>().Subcategorias);

        services.AddScoped<IClienteRepository>(sp => 
            sp.GetRequiredService<IUnitOfWork>().Clientes);

        services.AddScoped<IPedidoRepository>(sp => 
            sp.GetRequiredService<IUnitOfWork>().Pedidos);

        services.AddScoped<IUsuarioRepository>(sp => 
            sp.GetRequiredService<IUnitOfWork>().Usuarios);

        services.AddScoped<IVarianteRepository>(sp => 
            sp.GetRequiredService<IUnitOfWork>().Variantes);

        services.AddScoped<IAdicionalRepository>(sp => 
            sp.GetRequiredService<IUnitOfWork>().Adicionais);

        services.AddScoped<IEnderecoRepository>(sp => 
            sp.GetRequiredService<IUnitOfWork>().Enderecos);

        return services;
    }

    /// <summary>
    /// Registrar serviços de aplicação
    /// </summary>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        // Registrar serviços de aplicação
        services.AddScoped<IEstabelecimentoService, EstabelecimentoService>();
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<ISubcategoriaService, SubcategoriaService>();
        services.AddScoped<IClienteService, ClienteService>();
        services.AddScoped<IPedidoService, PedidoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
